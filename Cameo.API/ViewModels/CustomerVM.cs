﻿using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class CustomerEditVM : PersonEditVM
    {
        public CustomerEditVM() { }

        public CustomerEditVM(Customer model) : base(model)
        {
            if (model == null)
                return;
        }
    }

    public class CustomerShortInfoVM : PersonShortInfoVM
    {
        public CustomerShortInfoVM() { }

        public CustomerShortInfoVM(Customer model)
            : base(model)
        {
            if (model == null)
                return;
        }
    }
}