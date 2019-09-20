using Core.Erp.Bus.General;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_009_Rpt_Resumen : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        public CONTA_009_Rpt_Resumen()
        {
            InitializeComponent();
        }

        private void CONTA_009_Rpt_Resumen_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }
    }
}
