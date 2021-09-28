using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinanceManager.Models
{
    public class Account
    {
        public string Name { get; set; }

        public float Balance { get; set; }
    }
}
