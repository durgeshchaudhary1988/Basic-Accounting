using Accounting.Model.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounting.UI.ViewModels
{
    public class CreateTransactionViewModel : TransactionSummary
    {
        public List<TransactionAccountDetail> TransactionAccounts { get; set; }
    }
}