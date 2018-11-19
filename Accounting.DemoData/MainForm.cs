using Accounting.DemoData.Abstract;
using Accounting.Model.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Accounting.DemoData
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var type = typeof(IDemoDataGenerator);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => type.IsAssignableFrom(x) && !x.IsAbstract);
            foreach(var t in types)
            {
                cmbSampleDataSource.Items.Add(t);
            }
            if (cmbSampleDataSource.Items.Count > 0)
            {
                cmbSampleDataSource.SelectedIndex = 0;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var selectedType = (Type)cmbSampleDataSource.SelectedItem;
            var dataGenerator = (IDemoDataGenerator)Activator.CreateInstance(selectedType, new EFLedgerRepository(), new EFTransactionRepository());
            // dataGenerator.LedgerRepository = ;
            // dataGenerator.TransactionRepository = ;
            dataGenerator.CreateAccountingStructure();
            var response = dataGenerator.CreateTransactions();

        }
    }
}
