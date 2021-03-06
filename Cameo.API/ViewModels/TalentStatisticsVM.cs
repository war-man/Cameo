﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class TalentStatisticsVM
    {
        public string total { get; set; }
        public string not_completed { get; set; }
        //public string waiting_for_answer { get; set; }
        public string waiting_for_video { get; set; }
        public string completed { get; set; }
        //public string waiting_for_payment_confirmation { get; set; }
        //public string payment_confirmed { get; set; }
        public string earned { get; set; }
    }

    public class TalentVisibilityWarningVM
    {
        public List<string> names { get; set; }
    }
}
