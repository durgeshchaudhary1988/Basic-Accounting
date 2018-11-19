using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounting.UI.ViewModels
{
    public class ReportViewModel
    {
        public enum ReportEntryType
        {
            Account,
            Head
        }
        public class ReportEntry
        {
            public ReportEntryType EntryType { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public double OpeningBalance { get; set; }
            public double CreditAmount { get; set; }
            public List<ReportEntry> SubEntry { get; set; }
        }

        public string ReportName { get; set; }

        public List<ReportEntry> LeftEntries { get; set; }
        public List<ReportEntry> RightEntries { get; set; }

        public string LeftHeader { get; set; }
        public string RightHeader { get; set; }

        public string LeftBalance { get; set; }
        public string RightBalance { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}