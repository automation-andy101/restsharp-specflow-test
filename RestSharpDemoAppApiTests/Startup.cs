using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RestSharp_simple_project.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_simple_project
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IRestLibrary>(new RestLibrary(new WebApplicationFactory<GraphQLProductApp.Startup>()))
                .AddScoped<IRestBuilder, RestBuilder>()
                .AddScoped<IRestFactory, RestFactory>();
        }
    }
}
