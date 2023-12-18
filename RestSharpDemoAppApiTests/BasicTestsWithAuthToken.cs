using FluentAssertions;
using GraphQLParser;
using GraphQLProductApp.Controllers;
using GraphQLProductApp.Data;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp_simple_project.Base;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace RestSharp_simple_project
{
    public class BasicTestsWithAuthToken
    {
        private readonly RestClient _client;
        public BasicTestsWithAuthToken(IRestLibrary restLibrary) =>  _client = restLibrary.RestClient; 

        [Fact]
        public async Task GetOperationTest()
        {
            // Rest Request
            var request = new RestRequest("Product/GetProductById/1");
            request.AddHeader("Authorization", "Bearer " + GetToken());

            // Perform GET operation
            var response = await _client.GetAsync<Product>(request);

            // Assert
            response?.Name.Should().Be("Keyboard");
        }

        [Fact]
        public async Task GetWithQuerySegmentTest()
        {
            // Rest Request
            var request = new RestRequest("Product/GetProductById/{id}");
            request.AddHeader("Authorization", "Bearer " + GetToken());
            request.AddUrlSegment("id", 2);

            // Perform GET operation
            var response = await _client.GetAsync<Product>(request);

            // Assert
            response?.Price.Should().Be(400);
        }

        [Fact]
        public async Task GetWithQueryParameterTest()
        {
            // Rest Request
            var request = new RestRequest("Product/GetProductByIdAndName/");
            request.AddHeader("Authorization", "Bearer " + GetToken());
            request.AddQueryParameter("id", 2);
            request.AddQueryParameter("name", "Monitor");

            // Perform GET operation
            var response = await _client.GetAsync<Product>(request);

            // Assert
            response?.Price.Should().Be(400);
        }

        [Fact]
        public async Task PostCreateProductTest()
        {
            // Rest Request
            var request = new RestRequest("Product/Create/");
            request.AddHeader("Authorization", "Bearer " + GetToken());
            request.AddJsonBody(new Product
            {
                Name = "Printer",
                Description = "Colour printer",
                Price = 500,
                ProductType = ProductType.PERIPHARALS
            });

            // Perform POST operation
            var response = await _client.PostAsync<Product>(request);

            // Assert
            response?.Price.Should().Be(500);
        }

        [Fact]
        public async Task FileUploadTest()
        {
            // Rest Request
            var request = new RestRequest("Product", Method.Post);
            request.AddHeader("Authorization", "Bearer " + GetToken());
            request.AddFile("myFile", @"C:\\Users\\andys\\OneDrive\\Documents\\coding\\testing\\C#\\DemoAppWithApiTests\\RestSharpDemoAppApiTests\\images\\TestImage.png", "multipart/form-data");

            // Perform POST operation
            var response = await _client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        private string GetToken()
        {
            // Rest Request
            var authRequest = new RestRequest("api/Authenticate/Login");

            // Using login model typed object being passed as body in request
            authRequest.AddJsonBody(new LoginModel
            {
                UserName = "kk",
                Password = "123456"
            });

            // Perform POST operation
            var authResponse = _client.PostAsync(authRequest).Result.Content;

            return JObject.Parse(authResponse)["token"].ToString();
        }
    }
}

