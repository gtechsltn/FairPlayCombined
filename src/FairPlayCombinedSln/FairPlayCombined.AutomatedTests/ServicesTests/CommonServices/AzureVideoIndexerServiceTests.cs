﻿using Azure.Core;
using Azure.Identity;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class AzureVideoIndexerServiceTests: ServicesBase
    {
        [TestMethod]
        public async Task Test_GetAccessTokenForArmAccountAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<AzureOpenAIServiceTests>();
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
                    IsArmAccount=true,
                    Location = azureVideoIndexerLocation,
                    ResourceGroup = azureVideoIndexerResourceGroup,
                    ResourceName = azureVideoIndexerResourceName,
                    SubscriptionId = azureVideoIndexerSubscriptionId,
                };
            AzureVideoIndexerService azureVideoIndexerService = new(azureVideoIndexerServiceConfiguration,
                new HttpClient());
            string bearerToken = await this.AuthenticatedToAzureArmAsync();
            var result = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(bearerToken, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        private async Task<string> AuthenticatedToAzureArmAsync()
        {
            var tokenRequestContext = new TokenRequestContext(new[] { "https://management.azure.com/.default" });
            var tokenRequestResult = await new DefaultAzureCredential().GetTokenAsync(tokenRequestContext, CancellationToken.None);
            return tokenRequestResult.Token;
        }
    }
}
