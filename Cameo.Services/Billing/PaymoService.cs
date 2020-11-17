using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;

namespace Cameo.Services
{
    public class PaymoService : IPaymoService
    {
        public string ApplyForHold(Invoice invoice)
        {
            return "123456";
        }
    }
}