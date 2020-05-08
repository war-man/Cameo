﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class Customer : Person
    {
        public int Balance { get; set; }
        public string AccountNumber { get; set; } //лицевой счет
    }
}