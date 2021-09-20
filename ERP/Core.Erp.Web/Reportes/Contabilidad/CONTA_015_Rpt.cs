using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_015_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public CONTA_015_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_015_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            DateTime fechaIni = p_FechaIni.Value == null ? DateTime.Now : Convert.ToDateTime(p_FechaIni.Value);
            DateTime fechaFin = p_FechaFin.Value == null ? DateTime.Now : Convert.ToDateTime(p_FechaFin.Value);

            CONTA_015_Bus bus_rpt = new CONTA_015_Bus();
            List<CONTA_015_Info> lst_rpt = new List<CONTA_015_Info>();

            lst_rpt = bus_rpt.GetList(IdEmpresa, fechaIni, fechaFin);
            this.DataSource = lst_rpt;
        }
    }
}
