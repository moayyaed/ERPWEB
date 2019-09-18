using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Core.Erp.Web.Reportes.Inventario
{
    public partial class INV_021_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public INV_021_Rpt()
        {
            InitializeComponent();
        }

        private void INV_021_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
