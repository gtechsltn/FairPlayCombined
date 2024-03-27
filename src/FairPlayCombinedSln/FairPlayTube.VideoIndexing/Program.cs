using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayTube;
using FairPlayTube.VideoIndexing;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayTube.VideoIndexing), ref connectionString);

builder.Services.AddTransient<IUserProviderService, VideoIndexingUserProviderService>();
builder.Services.AddTransient<DbContextOptions<FairPlayCombinedDbContext>>(sp =>
{
    IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();
    DbContextOptionsBuilder<FairPlayCombinedDbContext> optionsBuilder = new();
    optionsBuilder.AddInterceptors(new SaveChangesInterceptor(userProviderService));
    optionsBuilder.UseSqlServer(connectionString,
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.UseNetTopologySuite();
            sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    return optionsBuilder.Options;
});
builder.AddSqlServerDbContext<FairPlayCombinedDbContext>(connectionName: "FairPlayCombinedDb");
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>();
builder.Services.AddTransient(sp =>
{
    var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
    var azureVideoIndexerAccountIdEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY} in database");
    var azureVideoIndexerLocationEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY} in database");
    var azureVideoIndexerResourceGroupEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY} in database");
    var azureVideoIndexerResourceNameEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY} in database");
    var azureVideoIndexerSubscriptionIdEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY} in database");
    AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration = new()
    {
        AccountId = azureVideoIndexerAccountIdEntity.Value,
        IsArmAccount = true,
        Location = azureVideoIndexerLocationEntity.Value,
        ResourceGroup = azureVideoIndexerResourceGroupEntity.Value,
        ResourceName = azureVideoIndexerResourceNameEntity.Value,
        SubscriptionId = azureVideoIndexerSubscriptionIdEntity.Value
    };
    return new AzureVideoIndexerService(azureVideoIndexerServiceConfiguration,
        new HttpClient());
});

builder.Services.AddTransient<VideoInfoService>();
builder.Services.AddHostedService<VideoIndexStatusBackgroundService>();
builder.Services.AddHostedService<VideoCaptionsBackgroundService>();

var host = builder.Build();
host.Run();
