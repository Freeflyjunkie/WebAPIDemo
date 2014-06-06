using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo.Data;
using WebAPIDemo.Web.Models;
using WebAPIDemo.Web.Services;

namespace WebAPIDemo.Web.Controllers
{
    public class DiarySummaryController : BaseApiController
    {
        private readonly ICountingKsIdentityService _identityService;
        public DiarySummaryController(ICountingKsRepository countingKsRepository, 
            ICountingKsIdentityService identityService) : base (countingKsRepository)
        {
            _identityService = identityService;
        }

        public HttpResponseMessage Get(DateTime diaryId)
        {
            try
            {
                var diary = TheCountingKsRepository.GetDiary(_identityService.CurrentUser, diaryId);

                if (diary == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateSummary(diary));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
