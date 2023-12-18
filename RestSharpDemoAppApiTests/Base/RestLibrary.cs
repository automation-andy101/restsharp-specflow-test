using Microsoft.AspNetCore.Mvc.Testing;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_simple_project.Base
{
    public interface IRestLibrary
    {
        RestClient RestClient { get; }
    }

    public class RestLibrary : IRestLibrary
    {
        public RestLibrary(WebApplicationFactory<GraphQLProductApp.Startup> webApplicationFactory)
        {
            var restClientOptions = new RestClientOptions
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
            };

            // Start GraphQL Demo app before running tests
            var client = webApplicationFactory.CreateDefaultClient();

            // Rest Client
            RestClient = new RestClient(client, restClientOptions);
        }

        public RestClient RestClient { get; }
    }
}
