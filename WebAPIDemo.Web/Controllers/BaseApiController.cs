using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo.Data;
using WebAPIDemo.Web.Models;

namespace WebAPIDemo.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        ICountingKsRepository _countingKsRepository;
        private ModelFactory _modelFactory;

        public BaseApiController(ICountingKsRepository countingKsRepository)
        {
            _countingKsRepository = countingKsRepository;            
        }

        protected ICountingKsRepository TheCountingKsRepository
        {
            get { return _countingKsRepository; }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, _countingKsRepository);
                }

                return _modelFactory;
            }
        }
    }
}
