using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounting.UI.ViewModels
{
    public class DisplayTransactionViewModel : CreateTransactionViewModel
    {
        public double CreditAmount { get; set; }
        public double DebitAmount { get; set; }
        public double Balance { get; set; }
    }
}