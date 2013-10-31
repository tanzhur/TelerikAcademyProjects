using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystem.Areas.Adminstration.ViewModels
{
    public class SamplePlanViewModel
    {
        [ScaffoldColumn(false)]
        public int PlanId { get; set; }
        public string PlanName { get; set; }
    }
}