using FluentAssertions;
using GraphQLProductApp.Controllers;
using GraphQLProductApp.Data;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace RestSharp_simple_project
{
    public class BasicTests
    {
        private RestClientOptions _restClientOptions;
        public BasicTests()
        {
            _restClientOptions = new RestClientOptions
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
            };
        }    

        [Fact]
        public async Task GetOperationTest()
        {
            // Rest Client
            var client = new RestClient(_restClientOptions);

            // Rest Request
            var request = new RestRequest("Product/GetProductById/1");

            // Perform GET operation
            var response = await client.GetAsync<Product>(request);

            // Assert
            response?.Name.Should().Be("Keyboard");
        }

        [Fact]
        public async Task GetWithQuerySegmentTest()
        {
            // Rest Client
            var client = new RestClient(_restClientOptions);

            // Rest Request
            var request = new RestRequest("Product/GetProductById/{id}");
            request.AddUrlSegment("id", 2);

            // Perform GET operation
            var response = await client.GetAsync<Product>(request);

            // Assert
            response?.Price.Should().Be(400);
        }

        [Fact]
        public async Task GetWithQueryParameterTest()
        {
            // Rest Client
            var client = new RestClient(_restClientOptions);

            // Rest Request
            var request = new RestRequest("Product/GetProductByIdAndName/");
            request.AddQueryParameter("id", 2);
            request.AddQueryParameter("name", "Monitor");

            // Perform GET operation
            var response = await client.GetAsync<Product>(request);

            // Assert
            response?.Price.Should().Be(400);
        }

        [Fact]
        public async Task PostCreateProductTest()
        {
            // Rest Client
            var client = new RestClient(_restClientOptions);

            // Rest Request
            var request = new RestRequest("Product/Create/");
            request.AddJsonBody(new Product
            {
                Name = "Printer",
                Description = "Colour printer",
                Price = 500,
                ProductType = ProductType.PERIPHARALS
            });

            // Perform POST operation
            var response = await client.PostAsync<Product>(request);

            // Assert
            response?.Price.Should().Be(500);
        }

        [Fact]
        public async Task FileUploadTest()
        {
            // Rest Client
            var client = new RestClient(_restClientOptions);

            // Rest Request
            var request = new RestRequest("Product", Method.Post);
            request.AddFile("myFile", @"C:\\Users\\andys\\OneDrive\\Documents\\coding\\testing\\C#\\DemoAppWithApiTests\\RestSharpDemoAppApiTests\\images\\TestImage.png", "multipart/form-data");

            // Perform POST operation
            var response = await client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

    }
}

