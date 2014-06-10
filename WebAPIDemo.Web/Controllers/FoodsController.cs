using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.Routing;
using WebAPIDemo.Data;
using WebAPIDemo.Data.Entities;
using WebAPIDemo.Web.Filter;
using WebAPIDemo.Web.Models;

namespace WebAPIDemo.Web.Controllers
{    
    public class FoodsController : BaseApiController
    {
        private const int PAGE_SIZE = 5;

        public FoodsController(ICountingKsRepository countingKsRepository) : base(countingKsRepository)
        {           
        }

        // Using Composer in Fiddler, set the return format of the data
        // use base64encode.org for Authorization credential encoding

        //http://localhost:23042/api/nutrition/foods?page=3
        //User-Agent: Fiddler
        //Host: localhost:23042
        //Content-Type: application/json
        //Authorization: Basic c2hhd253aWxkZXJtdXRoOnBsdXJhbHNpZ2h0

        //[CountingKsAuthorization]
        public object Get(bool includeMeasures = true, int page = 0)
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

            var baseQuery = query.OrderBy(f => f.Description);

            var totalCount = query.Count();
            var totalPages = Math.Ceiling((double) totalCount/PAGE_SIZE);

            var urlHelper = new UrlHelper(Request);
            var prevPageUrl = page > 0 ? urlHelper.Link("Food", new { page = page - 1 }) : "";
            var nextPageUrl = page < totalPages - 1 ? urlHelper.Link("Food", new { page = page + 1 }) : "";

            var results = baseQuery                
                        .Skip(PAGE_SIZE * page)
                        .Take(PAGE_SIZE)
                        .ToList()
                        .Select(f => TheModelFactory.Create(f));

            return new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageUrl = prevPageUrl,
                NextPageUrl = nextPageUrl,
                Results = results
            };
        }

        public FoodModel Get(int foodid)
        {
            return TheModelFactory.Create(TheCountingKsRepository.GetFood(foodid));
        }
    }
}
