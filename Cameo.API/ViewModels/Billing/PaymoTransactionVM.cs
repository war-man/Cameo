using Cameo.Models;
using Cameo.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class PaymoTransactionRequestVM
    {
        public int invoice { get; set; }
        public int store_id { get; set; }
        public string transaction_id { get; set; }
        public string transaction_time { get; set; }
        public int amount { get; set; }
        public string sign { get; set; }


        //public PaymoTransaction toModel(PaymoTransaction transaction)
        //{
        //    var model = new ClickTransaction();
        //    model.StatusID = (int)ClickTransactionStatusEnum.PENDING;

        //    model.ClickTransID = click_trans_id ?? 0;
        //    model.ServiceID = service_id ?? 0;
        //    model.ClickPaydocID = click_paydoc_id ?? 0;
        //    model.MerchantTransID = merchant_trans_id;
        //    model.Amount = amount ?? 0f;
        //    model.Error = error ?? 0;
        //    model.ErrorNote = error_note;
        //    model.SignTime = sign_time;
        //    model.SignString = sign_string;
        //    model.Customer = user;

        //    return model;
        //}
    }

    public class PaymoTransactionResponseVM
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
