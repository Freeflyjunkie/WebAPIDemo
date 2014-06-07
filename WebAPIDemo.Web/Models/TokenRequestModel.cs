using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPIDemo.Web.Models
{
    public class TokenRequestModel
    {
        // Acquired when developer or user registered
        public String ApiKey { get; set; }
        // string that contains information that tells api who they are
        public String Signature { get; set; }
    }
}
