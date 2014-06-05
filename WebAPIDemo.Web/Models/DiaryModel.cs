using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace WebAPIDemo.Web.Models
{
    public class DiaryModel
    {
        public string Url { get; set; }
        public DateTime CurrentDate { get; set; }
        public IEnumerable<DiaryEntryModel> Entries { get; set; }
    }
}