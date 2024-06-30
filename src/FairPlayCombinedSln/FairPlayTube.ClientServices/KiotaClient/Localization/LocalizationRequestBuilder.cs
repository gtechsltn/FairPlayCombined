// <auto-generated/>
using FairPlayTube.ClientServices.KiotaClient.Localization.GetAllResources;
using FairPlayTube.ClientServices.KiotaClient.Localization.GetSupportedCultures;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace FairPlayTube.ClientServices.KiotaClient.Localization {
    /// <summary>
    /// Builds and executes requests for operations under \localization
    /// </summary>
    public class LocalizationRequestBuilder : BaseRequestBuilder {
        /// <summary>The GetAllResources property</summary>
        public GetAllResourcesRequestBuilder GetAllResources { get =>
            new GetAllResourcesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The GetSupportedCultures property</summary>
        public GetSupportedCulturesRequestBuilder GetSupportedCultures { get =>
            new GetSupportedCulturesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new LocalizationRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public LocalizationRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/localization", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new LocalizationRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public LocalizationRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/localization", rawUrl) {
        }
    }
}
