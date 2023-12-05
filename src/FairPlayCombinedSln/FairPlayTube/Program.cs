using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FairPlayTube.Components;
using FairPlayTube.Components.Account;
using FairPlayTube.Data;
using FairPlayCombined.Common.Identity;
using Blazored.Toast;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.Post;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using FairPlayCombined.DataAccess.Interceptors;
using Microsoft.Extensions.Localization;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlayCombined.Services.FairPlayTube;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddTransient<IStringLocalizerFactory, EFStringLocalizerFactory>();
builder.Services.AddTransient<IStringLocalizer, EFStringLocalizer>();
builder.Services.AddLocalization();
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultScheme = IdentityConstants.ApplicationScheme;
    configureOptions.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies();
var clientAppsAuthPolicy = "ClientAppsAuthPolicy";
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(clientAppsAuthPolicy, policy =>
    {
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes(IdentityConstants.BearerScheme);
    });

var azureVideoIndexerAccountId = Environment.GetEnvironmentVariable("AzureVideoIndexerAccountId") ??
    throw new InvalidOperationException("'AzureVideoIndexerAccountId' not found");
var azureVideoIndexerLocation = Environment.GetEnvironmentVariable("AzureVideoIndexerLocation") ??
    throw new InvalidOperationException("'AzureVideoIndexerLocation' not found");
var azureVideoIndexerResourceGroup = Environment.GetEnvironmentVariable("AzureVideoIndexerResourceGroup") ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceGroup' not found");
var azureVideoIndexerResourceName = Environment.GetEnvironmentVariable("AzureVideoIndexerResourceName") ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceName' not found");
var azureVideoIndexerSubscriptionId = Environment.GetEnvironmentVariable("AzureVideoIndexerSubscriptionId") ??
    throw new InvalidOperationException("'AzureVideoIndexerSubscriptionId' not found");
AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration = new()
{
    AccountId = azureVideoIndexerAccountId,
    IsArmAccount = true,
    Location = azureVideoIndexerLocation,
    ResourceGroup = azureVideoIndexerResourceGroup,
    ResourceName = azureVideoIndexerResourceName,
    SubscriptionId = azureVideoIndexerSubscriptionId,
};
var connectionString = Environment.GetEnvironmentVariable("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayTube), ref connectionString);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddApiEndpoints()
    .AddDefaultTokenProviders();


builder.Services.AddTransient<IUserProviderService, UserProviderService>();
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(
    (sp, optionsAction) =>
    {
        IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();
        optionsAction.AddInterceptors(new SaveChangesInterceptor(userProviderService));
        optionsAction.UseSqlServer(connectionString,
            sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.UseNetTopologySuite();
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
            });
    });

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});
builder.Services.AddSingleton(azureVideoIndexerServiceConfiguration);
builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<ICultureService, CultureService>();
builder.Services.AddTransient(sp =>
{
    return new AzureVideoIndexerService(azureVideoIndexerServiceConfiguration,
        new HttpClient());
});
builder.Services.AddTransient<VideoInfoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseGlobalExceptionHandler();
}
else
{
    app.UseGlobalExceptionHandler();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

using var scope = app.Services.CreateScope();
using var ctx = scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();
var supportedCultures = ctx.Culture.Select(p => p.Name).ToArray();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
// Add additional endpoints required by the Identity /Account Razor components.
app.MapIdentityApi<ApplicationUser>();
app.MapAdditionalIdentityEndpoints();

app.MapGet("/api/video/{videoId}/captions/{language}",
    async (
        [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        [FromRoute] string videoId, 
        [FromRoute] string language,
        CancellationToken cancellationToken) => 
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.VideoCaptions
        .Include(p=>p.VideoInfo)
        .Where(p => p.VideoInfo.VideoId == videoId &&
        p.Language == language)
        .Select(p => p.Content)
        .SingleOrDefaultAsync(cancellationToken);
        return TypedResults.Content(result, System.Net.Mime.MediaTypeNames.Text.Plain);
    });

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
