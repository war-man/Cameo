using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    //[CustomerAuthorization] - must be implemented
    public class CustomerVideoRequestController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IVideoRequestSearchService SearchService;

        public CustomerVideoRequestController(
            ICustomerService customerService,
            IVideoRequestSearchService searchService)
        {
            CustomerService = customerService;
            SearchService = searchService;
        }

        public IActionResult Index()
        {
            
            return View();
        }
    }
}