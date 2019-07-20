using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
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
}