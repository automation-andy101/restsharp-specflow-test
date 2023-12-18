using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_simple_project.Base
{
    public interface IRestFactory
    {
        IRestBuilder Create();
    }

    public class RestFactory : IRestFactory
    {
        private readonly IRestBuilder _restBuilder;

        public RestFactory(IRestBuilder restBuilder)
        {
            _restBuilder = restBuilder;
        }

        public IRestBuilder Create()
        {
            return _restBuilder;
        }

    }

}
