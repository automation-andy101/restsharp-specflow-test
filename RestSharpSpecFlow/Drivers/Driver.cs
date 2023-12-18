using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestSharpSpecFlow.Drivers
{
    public class Driver
    {
        public Driver(ScenarioContext scenarioContext)
        {
            var restClientOptions = new RestClientOptions
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
            };

            // Rest client
            var restClient = new RestClient(restClientOptions);
            scenarioContext.Add("RestClient", restClient);
        }
    }
}
