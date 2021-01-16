using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class Invoice : BaseModel
    {
        [ForeignKey("VideoRequest")]
        public int? VideoRequestID { get; set; }
        public virtual VideoRequest VideoRequest { get; set; }

        [StringLength(32)]
        public string card_number { get; set; } //ok
        public DateTime card_expiry { get; set; } //ok

        public string hold_id { get; set; } //ok
        public int duration_in_minutes { get; set; } //ok
        public DateTime? hold_till { get; set; } //ok

        public string transaction_id { get; set; } //ok
        public string transaction_time { get; set; } //ok
        public float commission_value { get; set; } //ok
        public string terminal_id { get; set; } //ok
        public string prepay_time { get; set; } //Время создания драфта транзакции
        public string confirm_time { get; set; } //Время завершения транзакции
        public string status_code { get; set; } //Код состояния транзакции
        public string status_message { get; set; } //Сообщение состояния транзакции

        public int Amount { get; set; } //ok

        public int StatusID { get; set; } //ok //1 - pending, 2 - success, 3 - cancelled

        public DateTime? DateSuccess { get; set; } //ok
        public DateTime? DateCancelled { get; set; } //ok

        //public int ClickTransID { get; set; }
        //public int ServiceID { get; set; }
        //public int ClickPaydocID { get; set; }
        //public string MerchantTransID { get; set; } //litcevoy schot
        //public float Amount { get; set; }
        //public int Error { get; set; }
        //public string ErrorNote { get; set; }
        //public string SignTime { get; set; }
        //public string SignString { get; set; }
        //public DateTime? DateSuccess { get; set; }
        //public DateTime? DateCancelled { get; set; }
    }
}
