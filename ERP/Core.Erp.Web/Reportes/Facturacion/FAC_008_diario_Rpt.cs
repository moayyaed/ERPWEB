using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Core.Erp.Web.Reportes.Facturacion
{
    public partial class FAC_008_diario_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public FAC_008_diario_Rpt()
        {
            InitializeComponent();
        }

        private void FAC_008_diario_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
