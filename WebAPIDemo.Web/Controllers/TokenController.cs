using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebAPIDemo.Data;
using WebAPIDemo.Data.Entities;
using WebAPIDemo.Web.Models;

namespace WebAPIDemo.Web.Controllers
{
    public class TokenController : BaseApiController
    {
        public TokenController(ICountingKsRepository countingKsRepository) : base(countingKsRepository)
        {
            
        }

        // Use to get an Auth Token
        // requires that the user has an ApiKey already
        public HttpResponseMessage Post([FromBody] TokenRequestModel model)
        {
            try
            {
                var user = TheCountingKsRepository.GetApiUsers().FirstOrDefault(u => u.AppId == model.ApiKey);
                if (user != null)
                {
                    var secret = user.Secret;

                    // Simplistic implementation DO NOT USE
                    var key = Convert.FromBase64String(secret);
                    var provider = new System.Security.Cryptography.HMACSHA256(key);

                    // Compute Hash from API Key (NOT SECURE)
                    var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(user.AppId));
                    var signature = Convert.ToBase64String(hash);

                    if (signature == model.Signature)
                    {
                        var rawTokenInfo = string.Concat(user.AppId + DateTime.UtcNow.ToString("d"));
                        var rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
                        var token = provider.ComputeHash(rawTokenByte);

                        var authToken = new AuthToken()
                        {
                            Token = Convert.ToBase64String(token),
                            Expiration = DateTime.UtcNow.AddDays(7),
                            ApiUser = user
                        };
                        if (TheCountingKsRepository.Insert(authToken) && TheCountingKsRepository.SaveAll())
                        {
                            return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(authToken));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
