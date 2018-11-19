using Accounting.Model.Model.Ledger;
using System.Collections.Generic;

namespace Accounting.DemoData.Model
{
    /// <summary>
    /// Ledger Type definition
    /// </summary>
    public class SampleLedgerType
    {
        /// <summary>
        /// Ledger Type definition
        /// </summary>
        public LedgerType ledgerType { get; set; }

        /// <summary>
        /// Ledger Heads under given Ledger Type
        /// </summary>
        public List<SampleLedgerHead> SampleLedgerHeads { get; set; }
    }
}
