using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using WebAPIDemo.Data;
using WebAPIDemo.Data.Entities;
using WebAPIDemo.Web.Models;

namespace WebAPIDemo.Web.Controllers
{
    public class FoodsController : BaseApiController
    {     
        public FoodsController(ICountingKsRepository countingKsRepository) : base(countingKsRepository)
        {           
        }

        // Using Composer in Fiddler, set the return format of the data
        // User-Agent: Fiddler
        // Host: localhost:23042
        // Accept: text/xml, application/json, text/html 
        public IEnumerable<FoodModel> Get(bool includeMeasures = true)
        {
            IQueryable<Food> query;

            if (includeMeasures)
            {
                query = TheCountingKsRepository.GetAllFoodsWithMeasures();
            }
            else
            {
                query = TheCountingKsRepository.GetAllFoods();
            }

            var results = query
                .OrderBy(f => f.Description)
                .Take(25)
                .ToList()
                .Select(f => TheModelFactory.Create(f));

            return results;
        }

        public FoodModel Get(int foodid)
        {
            return TheModelFactory.Create(TheCountingKsRepository.GetFood(foodid));
        }
    }
}
