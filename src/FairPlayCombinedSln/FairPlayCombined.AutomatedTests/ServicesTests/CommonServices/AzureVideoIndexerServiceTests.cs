﻿#if Debug_Enable_Paid_Tests
using Azure.Core;
using Azure.Identity;
using FairPlayCombined.Models.AzureVideoIndexer;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{

    [TestClass]
    public class AzureVideoIndexerServiceTests : ServicesBase
    {
        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddUserSecrets<ServicesBase>();
                var configuration = configurationBuilder.Build();
                var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"]!;
                var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"]!;
                var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"]!;
                var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"]!;
                var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"]!;
                var testVideoId = configuration["TestVideoId"];
                AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                    new()
                    {
                        AccountId = azureVideoIndexerAccountId,
                        IsArmAccount = true,
                        Location = azureVideoIndexerLocation,
                        ResourceGroup = azureVideoIndexerResourceGroup,
                        ResourceName = azureVideoIndexerResourceName,
                        SubscriptionId = azureVideoIndexerSubscriptionId,
                    };
                AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                    new HttpClient());
                string bearerToken = await AuthenticatedToAzureArmAsync();
                var getAccessToken = await azureVideoIndexerService
                    .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
                Assert.IsNotNull(getAccessToken);
                var foundVideos = await azureVideoIndexerService.SearchVideosByNameAsync(
                    viAccessToken: getAccessToken!.AccessToken!,
                    name: "AT File ",
                    cancellationToken: CancellationToken.None);
                if (foundVideos?.results?.Length > 0)
                {
                    foreach (var singleVideo in foundVideos.results)
                    {
                        if (singleVideo.id != testVideoId)
                        {
                            await azureVideoIndexerService.DeleteVideoByIdAsync(
                                videoId: singleVideo.id!,
                                viAccessToken: getAccessToken.AccessToken!,
                                cancellationToken: CancellationToken.None);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
            }

        }

        [TestMethod]
        public async Task Test_GetVideoStreamingUrlAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"]!;
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"]!;
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"]!;
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"]!;
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"]!;
            var testVideoId = configuration["TestVideoId"]!;
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result = await azureVideoIndexerService.GetVideoStreamingUrlAsync(testVideoId,
                viAccessToken:getAccessToken.AccessToken!,
                CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetVideoThumbnailAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"]!;
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"]!;
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"]!;
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"]!;
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"]!;
            var testVideoId = configuration["TestVideoId"]!;
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var indexResponse = await azureVideoIndexerService.GetVideoIndexAsync(
                testVideoId!, getAccessToken.AccessToken!,
                CancellationToken.None);
            Assert.IsNotNull(indexResponse);
            var result = await azureVideoIndexerService.GetVideoThumbnailAsync(testVideoId,
                indexResponse.videos![0].thumbnailId!,
                getAccessToken.AccessToken!, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetSupportedLanguagesAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            _ = configuration["TestVideoId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result =
            await azureVideoIndexerService
            .GetSupportedLanguagesAsync(getAccessToken!.AccessToken!,
            CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetVideoVTTCaptionsAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var testVideoId = configuration["TestVideoId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result =
            await azureVideoIndexerService.GetVideoVTTCaptionsAsync(testVideoId!,
                getAccessToken!.AccessToken!,
                language: "en",
                CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetVideoIndexAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var testVideoId = configuration["TestVideoId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await AuthenticatedToAzureArmAsync();
            var getAccessToken = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(getAccessToken);
            var result =
            await azureVideoIndexerService.GetVideoIndexAsync(testVideoId!,
                getAccessToken!.AccessToken!,
                CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.videos?.Length);
        }

        [TestMethod]
        public async Task Test_GetAccessTokenForArmAccountAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await AuthenticatedToAzureArmAsync();
            var result = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_IndexVideoFromBytesAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var videoToIndexFullPath = configuration["VideoToIndexFullPath"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string armAccesstoken = await AuthenticatedToAzureArmAsync();
            var getAccessTokenResult = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(armAccesstoken, CancellationToken.None);
            Assert.IsNotNull(getAccessTokenResult);
            var fileBytes = await File.ReadAllBytesAsync(videoToIndexFullPath!);
            var result = await azureVideoIndexerService.IndexVideoFromBytesAsync(
                new IndexVideoFromBytesFormatModel()
                {
                    FileBytes = fileBytes,
                    Name = $"AT File {Random.Shared.Next(1, 100)}"
                },
                viAccountAccessToken: getAccessTokenResult!.AccessToken!, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_IndexVideoFromUriAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var azureVideoIndexerAccountId = configuration["AzureVideoIndexerAccountId"];
            var azureVideoIndexerLocation = configuration["AzureVideoIndexerLocation"];
            var azureVideoIndexerResourceGroup = configuration["AzureVideoIndexerResourceGroup"];
            var azureVideoIndexerResourceName = configuration["AzureVideoIndexerResourceName"];
            var azureVideoIndexerSubscriptionId = configuration["AzureVideoIndexerSubscriptionId"];
            var videoToIndexUrl = configuration["VideoToIndexUrl"];
            AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration =
                new()
                {
                    AccountId = azureVideoIndexerAccountId,
                    IsArmAccount = true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string armAccesstoken = await AuthenticatedToAzureArmAsync();
            var getAccessTokenResult = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(armAccesstoken, CancellationToken.None);
            Assert.IsNotNull(getAccessTokenResult);
            var result = await azureVideoIndexerService.IndexVideoFromUriAsync(
                new IndexVideoFromUriParameters()
                {
                    ArmAccessToken = getAccessTokenResult.AccessToken!,
                    Description = "Test Desc",
                    FileName = $"TestFile {Random.Shared.Next(1, 10000)}",
                    Name = $"AT File {Random.Shared.Next(1, 10000)}",
                    VideoUri = new Uri(videoToIndexUrl!)
                });
            Assert.IsNotNull(result);
        }

        private static async Task<string> AuthenticatedToAzureArmAsync()
        {
            var tokenRequestContext = new TokenRequestContext(["https://management.azure.com/.default"]);
            var tokenRequestResult = await new DefaultAzureCredential().GetTokenAsync(tokenRequestContext, CancellationToken.None);
            return tokenRequestResult.Token;
        }
    }
}
#endif