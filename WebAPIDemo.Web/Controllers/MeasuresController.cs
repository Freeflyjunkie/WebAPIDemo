using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo.Data;
using WebAPIDemo.Data.Entities;
using WebAPIDemo.Web.Models;

namespace WebAPIDemo.Web.Controllers
{
    public class MeasuresController : BaseApiController
    {
        public MeasuresController(ICountingKsRepository countingKsRepository)
            : base(countingKsRepository)
        {            
        }

        public IEnumerable<MeasureModel> Get(int foodid)
        {
            var results = TheCountingKsRepository.GetMeasuresForFood(foodid)
                .ToList()
                .Select(m => TheModelFactory.Create(m));

            return results;
        }

        public HttpResponseMessage Get(int foodid, int id)
        {
            var results = TheCountingKsRepository.GetMeasure(id);

            if (results == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Measure Not Found.");

            if (results.Food.Id == foodid)
            {
                return Request.CreateResponse(HttpStatusCode.Found, TheModelFactory.Create(results));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Measure Not Found For Food.");
            }             
        }
    }
}
