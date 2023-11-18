using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureOpenAI;
using FairPlayCombined.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FairPlayCombined.LocalizationGenerator;

public class LocalizationGenerator(IServiceScopeFactory serviceScopeFactory,
        ILogger<LocalizationGenerator> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var skipTranslations = Convert.ToBoolean(conf["skipTranslations"]);
        FairPlayCombinedDbContext fairPlayCombinedDbContext =
            scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();
        var adminPortalAssembly = typeof(FairPlayAdminPortal.Data.ApplicationUser).Assembly;
        var adminPortalTypes = adminPortalAssembly.GetTypes();

        var modelsAssembly = typeof(Models.UserModel).Assembly;
        var modelsTypes = modelsAssembly.GetTypes();

        var serverSideServicesAssembly = typeof(BaseService).Assembly;
        var serverSideServicesTypes = serverSideServicesAssembly.GetTypes();

        var commonAssembly = typeof(Common.Constants).Assembly;
        var commonTypes = commonAssembly.GetTypes();
        List<Type> typesToCheck = [
            .. adminPortalTypes,
            .. modelsTypes, 
            .. serverSideServicesTypes,
            .. commonTypes];

        foreach (var singleTypeToCheck in typesToCheck)
        {
            string typeFullName = singleTypeToCheck!.FullName!;
            var fields = singleTypeToCheck.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy
                );
            foreach (var singleField in fields)
            {
                var resourceKeyAttributes =
                    singleField.GetCustomAttributes<ResourceKeyAttribute>();
                if (resourceKeyAttributes != null && resourceKeyAttributes.Any())
                {
                    ResourceKeyAttribute keyAttribute = resourceKeyAttributes.Single();
                    string key = singleField.GetRawConstantValue()!.ToString()!;
                    var entity =
                        await fairPlayCombinedDbContext.Resource
                        .SingleOrDefaultAsync(p => p.CultureId == 1 &&
                        p.Key == key &&
                        p.Type == typeFullName, stoppingToken);
                    if (entity is null)
                    {
                        entity = new Resource()
                        {
                            CultureId = 1,
                            Key = key,
                            Type = typeFullName,
                            Value = keyAttribute.DefaultValue
                        };
                        await fairPlayCombinedDbContext.Resource.AddAsync(entity, stoppingToken);
                    }
                }
            }
        }
        if (fairPlayCombinedDbContext.ChangeTracker.HasChanges())
            await fairPlayCombinedDbContext.SaveChangesAsync(stoppingToken);
        if (skipTranslations)
        {
            logger.LogInformation("Skipping Translation");
            return;
        }
        var allEnglishUSKeys =
            await fairPlayCombinedDbContext.Resource
            .Include(p => p.Culture)
            .Where(p => p.Culture.Name == "en-US")
            .ToListAsync(stoppingToken);
        IAzureOpenAIService azureOpenAIService =
            scope.ServiceProvider.GetRequiredService<IAzureOpenAIService>();
        try
        {
            foreach (var resource in allEnglishUSKeys)
            {
                foreach (var singleCulture in await fairPlayCombinedDbContext.Culture.ToArrayAsync(cancellationToken: stoppingToken))
                {
                    if (!await fairPlayCombinedDbContext.Resource
                        .AnyAsync(p => p.CultureId == singleCulture.CultureId &&
                        p.Key == resource.Key && p.Type == resource.Type, cancellationToken: stoppingToken))
                    {
                        logger.LogInformation("Translating: \"{Value}\" to \"{Name}\"", resource.Value, singleCulture.Name);
                        TranslationResponse? translationResponse = await
                            azureOpenAIService!
                            .TranslateSimpleTextAsync(resource.Value,
                            "en-US", singleCulture.Name,
                            cancellationToken: stoppingToken);
                        if (translationResponse != null)
                        {
                            await fairPlayCombinedDbContext.Resource
                                .AddAsync(new Resource()
                                {
                                    CultureId = singleCulture.CultureId,
                                    Key = resource.Key,
                                    Type = resource.Type,
                                    Value = translationResponse.TranslatedText ?? resource.Value
                                },
                                cancellationToken: stoppingToken);
                            await fairPlayCombinedDbContext
                                .SaveChangesAsync(cancellationToken: stoppingToken);
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
        }
        logger.LogInformation("Process {processName} completed", nameof(LocalizationGenerator));
    }
}
