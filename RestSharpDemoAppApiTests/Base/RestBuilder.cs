using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_simple_project.Base
{
    public interface IRestBuilder
    {
        IRestBuilder WithRequest(string request);
        IRestBuilder WithHeader(string name, string value);
        IRestBuilder WithQueryParameter(string name, string value);
        IRestBuilder WithUrlSegment(string name, string value);
        IRestBuilder WithBody(object body);
        IRestBuilder WithFile(string name, string value, string formType);
        Task<T?> WithGet<T>();
        Task<T?> WithPost<T>();
        Task<RestResponse> WithPost();
        Task<T?> WithPut<T>();
        Task<T?> WithPatch<T>();
        Task<T?> WithDelete<T>();

    }

    public class RestBuilder : IRestBuilder
    {
        private readonly IRestLibrary _restLibrary;
        public RestBuilder(IRestLibrary restLibrary)
        {
            _restLibrary = restLibrary;
        }

        private RestRequest RestRequest { get; set; } = null;

        public IRestBuilder WithRequest(string request)
        {
            RestRequest = new RestRequest(request);
            return this;
        }

        public IRestBuilder WithHeader(string name, string value)
        {
            RestRequest.AddHeader(name, value);
            return this;
        }

        public IRestBuilder WithQueryParameter(string name, string value)
        {
            RestRequest.AddQueryParameter(name, value);
            return this;
        }

        public IRestBuilder WithUrlSegment(string name, string value)
        {
            RestRequest.AddUrlSegment(name, value);
            return this;
        }

        public IRestBuilder WithBody(object body)
        {
            RestRequest.AddBody(body);
            return this;
        }

        public IRestBuilder WithFile(string name, string value, string formType)
        {
            RestRequest.AddFile(name, value, formType);
            return this;
        }

        public async Task<T?> WithGet<T>()
        {
            return await _restLibrary.RestClient.GetAsync<T>(RestRequest);
        }

        public async Task<T?> WithPost<T>()
        {
            return await _restLibrary.RestClient.PostAsync<T>(RestRequest);
        }

        public async Task<RestResponse> WithPost()
        {
            return await _restLibrary.RestClient.PostAsync(RestRequest);
        }

        public async Task<RestResponse> WithExecute<T>()
        {
            return await _restLibrary.RestClient.ExecuteAsync(RestRequest);
        }

        public async Task<T?> WithPut<T>()
        {
            return await _restLibrary.RestClient.PutAsync<T>(RestRequest);
        }

        public async Task<T?> WithPatch<T>()
        {
            return await _restLibrary.RestClient.PatchAsync<T>(RestRequest);
        }

        public async Task<T?> WithDelete<T>()
        {
            return await _restLibrary.RestClient.DeleteAsync<T>(RestRequest);
        }
    }
}
