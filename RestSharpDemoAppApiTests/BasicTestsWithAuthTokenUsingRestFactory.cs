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
    public class BasicTestsWithAuthTokenUsingRestFactory
    {

        private readonly IRestFactory _restFactory;
        private readonly string? _token;

        public BasicTestsWithAuthTokenUsingRestFactory(IRestFactory restFactory)
        {
            _restFactory = restFactory;
            _token = GetToken();
        }

        [Fact]
        public async Task GetOperationTest()
        {
            var response = await _restFactory.Create()
                .WithRequest("Product/GetProductById/1")
                .WithHeader("Authorization", $"Bearer {_token}")
                .WithGet<Product>();

            response?.Name.Should().Be("Keyboard");
        }

        [Fact]
        public async Task GetWithQuerySegmentTest()
        {
            var response = await _restFactory.Create()
                .WithRequest("Product/GetProductById/{id}")
                .WithUrlSegment("id", "2")
                .WithHeader("Authorization", $"Bearer {_token}")
                .WithGet<Product>();

            // Assert
            response?.Price.Should().Be(400);
        }

        [Fact]

        public async Task GetWithQueryParameterTest()
        {
            var response = await _restFactory.Create()
                .WithRequest("Product/GetProductByIdAndName/")
                .WithQueryParameter("id", "2")
                .WithQueryParameter("name", "Monitor")
                .WithHeader("Authorization", $"Bearer {_token}")
                .WithGet<Product>();

            // Assert
            response?.Price.Should().Be(400);
        }

        [Fact]
        public async Task PostCreateProductTest()
        {
            var response = await _restFactory.Create()
                .WithRequest("Product/Create")
                .WithBody(new Product
                {
                    Name = "Printer",
                    Description = "Colour printer",
                    Price = 500,
                    ProductType = ProductType.PERIPHARALS
                })
                .WithHeader("Authorization", $"Bearer {_token}")
                .WithPost<Product>();

            // Assert
            response?.Price.Should().Be(500);
        }

        [Fact]
        public async Task FileUploadTest()
        {
            var response = await _restFactory.Create()
                .WithRequest("Product")
                .WithFile("myFile", @"C:\\Users\\andys\\OneDrive\\Documents\\coding\\testing\\C#\\DemoAppWithApiTests\\RestSharpDemoAppApiTests\\images\\TestImage.png", "multipart/form-data")
                .WithHeader("Authorization", $"Bearer {_token}")
                .WithPost();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        private string GetToken()
        {
            var authResponse = _restFactory
                .Create()
                .WithRequest("api/Authenticate/Login")
                .WithBody(new LoginModel
                {
                    UserName = "kk",
                    Password = "123456"
                })
                .WithPost().Result.Content;

            return JObject.Parse(authResponse)["token"].ToString();
        }
    }
}

