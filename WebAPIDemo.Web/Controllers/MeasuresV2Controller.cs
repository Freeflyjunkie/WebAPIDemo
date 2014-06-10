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
    public class MeasuresV2Controller : BaseApiController
    {
        public MeasuresV2Controller(ICountingKsRepository countingKsRepository)
            : base(countingKsRepository)
        {            
        }

        public IEnumerable<MeasureV2Model> Get(int foodid)
        {
            var results = TheCountingKsRepository.GetMeasuresForFood(foodid)
                .ToList()
                .Select(m => TheModelFactory.Create2(m));

            return results;
        }

        public HttpResponseMessage Get(int foodid, int id)
        {
            var results = TheCountingKsRepository.GetMeasure(id);

            if (results == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Measure Not Found.");

            if (results.Food.Id == foodid)
            {
                return Request.CreateResponse(HttpStatusCode.Found, TheModelFactory.Create2(results));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Measure Not Found For Food.");
            }             
        }
    }
}
