using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WebAPIDemo.Data;
using WebAPIDemo.Web.Models;
using WebAPIDemo.Web.Services;

namespace WebAPIDemo.Web.Controllers
{
    public class DiariesController : BaseApiController
    {
        private readonly ICountingKsIdentityService _identityService;
        public DiariesController(ICountingKsRepository countingKsRepository, 
            ICountingKsIdentityService identityService) : base (countingKsRepository)
        {
            _identityService = identityService;
        }

        public IEnumerable<DiaryModel> Get()
        {
            //use Thread.CurrentPrincipal.Identity.Name; but not very testable
            var username = _identityService.CurrentUser;
            var results = TheCountingKsRepository.GetDiaries(username)
                .OrderByDescending(d => d.CurrentDate)
                .Take(10)
                .ToList()
                .Select(d => TheModelFactory.Create(d));

            return results;
        }

        public HttpResponseMessage Get(DateTime diaryid)
        {            
            var username = _identityService.CurrentUser;
            var results = TheCountingKsRepository.GetDiary(username, diaryid);

            if (results == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(results));
            }
        }
    }
}
