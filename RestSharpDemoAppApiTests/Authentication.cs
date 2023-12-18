using GraphQLProductApp.Controllers;
using GraphQLProductApp.Data;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using FluentAssertions;

namespace RestSharp_simple_project
{
    public class Authentication
    {
        private RestClientOptions _restClientOptions;
        public Authentication()
        {
            _restClientOptions = new RestClientOptions
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
            };
        }


        [Fact]
        public async Task GetWithQueryParameterTest()
        {
            // Rest Client
            var client = new RestClient(_restClientOptions);

            // Rest Request
            var authRequest = new RestRequest("api/Authenticate/Login");

            // Anonymous object being passed as body in request
            //request.AddJsonBody(new
            //{
            //    username = "kk",
            //    password = "123456"
            //});

            // Using login model typed object being passed as body in request
            authRequest.AddJsonBody(new LoginModel
            {
                UserName = "kk",
                Password = "123456"
            });

            // Perform POST operation
            var authResponse = client.PostAsync(authRequest).Result.Content;
            var token = JObject.Parse(authResponse)["token"];

            // Rest Request
            var getProductRequest = new RestRequest("Product/GetProductById/1");
            getProductRequest.AddHeader("Authorization", "Bearer " + token?.ToString());

            // Perform GET operation
            var productResponse = await client.GetAsync<Product>(getProductRequest);

            // Assert
            productResponse?.Name.Should().Be("Keyboard");

        }
    }
}
