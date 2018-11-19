using Accounting.Model.Model.Ledger;
using System.Collections.Generic;

namespace Accounting.DemoData.Model
{
    /// <summary>
    /// Ledger Head Definition
    /// </summary>
    public class SampleLedgerHead
    {
        /// <summary>
        /// Ledger Head Definition
        /// </summary>
        public LedgerHead ledgerHead { get; set; }
        /// <summary>
        /// Ledger Accounts under given Ledger head
        /// </summary>
        public List<LedgerAccount> SampleLedgerAccounts { get; set; }
        /// <summary>
        /// Ledger Heads under given Ledger head
        /// </summary>
        public List<SampleLedgerHead> SampleLedgerHeads { get; set; }
    }
}
