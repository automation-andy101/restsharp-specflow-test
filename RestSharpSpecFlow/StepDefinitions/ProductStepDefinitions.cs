using GraphQL.Language.AST;
using GraphQLProductApp.Controllers;
using GraphQLProductApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IO;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Table = TechTalk.SpecFlow.Table;

namespace RestSharpSpecFlow.StepDefinitions
{
    [Binding]
    public sealed class ProductStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private RestClient _restClient;
        private Product _response;
        private RestResponse _getAllProductsResponse;

        public ProductStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _restClient = _scenarioContext.Get<RestClient>("RestClient");
        }


        [When(@"user sends a request to GET a single product to endpoint ""([^""]*)""")]
        public async Task WhenUserSendsARequestToGETASingleProductToEndpoint(string path, Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            var token = GetToken();

            // Rest Request
            var request = new RestRequest(path);
            request.AddUrlSegment("id", (int)data.ProductId);
            request.AddHeader("Authorization", $"Bearer {token}");

            // Perform GET operation
            _response = await _restClient.GetAsync<Product>(request);
        }



        [Then(@"response body contains product name ""(.*)""")]
        public void ThenResponseBodyContainsProductName(string value)
        {
            _response.Name.Should().Be(value);
        }


        [When(@"user sends a GET request to get product with id '([^']*)' to endpoint ""([^""]*)""")]
        public async Task WhenUserSendsAGETRequestToGetProductWithIdToEndpoint(string productId, string path)
        {
            var token = GetToken();

            // Rest Request
            var request = new RestRequest(path);
            request.AddUrlSegment("id", productId);
            request.AddHeader("Authorization", $"Bearer {token}");

            // Perform GET operation
            _response = await _restClient.GetAsync<Product>(request);
        }


        [Then(@"response body contains product with name '([^']*)'")]
        public void ThenResponseBodyContainsProductWithName(string value)
        {
            _response.Name.Should().Be(value);
        }


        [When(@"user sends a GET request with '([^']*)' and '([^']*)' to endpoint ""([^""]*)""")]
        public async Task WhenUserSendsAGETRequestWithIdAndNameToEndpoint(string productId, string productName, string path)
        {
            var token = GetToken();

            // Rest Request
            var request = new RestRequest(path);
            request.AddQueryParameter("id", productId);
            request.AddQueryParameter("name", productName);
            request.AddHeader("Authorization", $"Bearer {token}");

            // Perform GET operation
            _response = await _restClient.GetAsync<Product>(request);
        }


        [Then(@"response body contains product name '([^']*)' and '([^']*)'")]
        public void ThenResponseBodyContainsProductIdAndName(string productId, string productName)
        {
            _response.Name.Should().Be(productName);
            _response.ProductId.Should().Be(Int32.Parse(productId));
        }


        [When(@"user sends a request to GET all products to endpoint ""([^""]*)""")]
        public async Task WhenUserSendsARequestToGETAllProductsToEndpoint(string path)
        {
            var token = GetToken();

            // Rest Request
            var request = new RestRequest(path);
            request.AddHeader("Authorization", $"Bearer {token}");

            // Perform GET operation
            _getAllProductsResponse = await _restClient.ExecuteGetAsync(request);
        }


        [Then(@"response body contains an array of products")]
        public void ThenResponseBodyContainsAnArrayOfProducts()
        {
            _getAllProductsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }


        [Then(@"response status code is OK")]
        public void ThenResponseStatusCodeIsOK()
        {
            _getAllProductsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [When(@"user sends a POST request to create a new product to endpoint ""([^""]*)""")]
        public async Task WhenUserSendsAPOSTRequestToCreateANewProductToEndpoint(string path, Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            var token = GetToken();

            // Rest Request
            var request = new RestRequest(path);
            request.AddJsonBody(new Product
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ProductType = ProductType.PERIPHARALS
            });
            request.AddHeader("Authorization", $"Bearer {token}");

            // Perform GET operation
            _response = await _restClient.PostAsync<Product>(request);
        }

        [Then(@"response body contains")]
        public void ThenResponseBodyContains(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            _response.Name.Should().Be(data.Name);
            _response.Description.Should().Be(data.Description);
            _response.Price.Should().Be(data.Price);
            _response.ProductType.Should().Be(ProductType.PERIPHARALS);
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
            var authResponse = _restClient.PostAsync(authRequest).Result.Content;

            return JObject.Parse(authResponse)["token"].ToString();
        }
    }
}