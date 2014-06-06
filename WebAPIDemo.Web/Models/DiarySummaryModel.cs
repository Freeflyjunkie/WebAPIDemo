using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPIDemo.Web.Models
{
    public class DiarySummaryModel
    {
        public DateTime DiaryDate { get; set; }
        public double TotalCalories { get; set; }
    }
}
