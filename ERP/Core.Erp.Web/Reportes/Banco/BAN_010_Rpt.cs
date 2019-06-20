using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Info.Reportes.Banco;
using Core.Erp.Bus.Reportes.Banco;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.Banco
{
    public partial class BAN_010_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public BAN_010_Rpt()
        {
            InitializeComponent();
        }

        private void BAN_010_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            decimal IdArchivo = string.IsNullOrEmpty(p_IdArchivo.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdArchivo.Value);

            BAN_010_Bus bus_rpt = new BAN_010_Bus();
            List<BAN_010_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, IdArchivo);
            this.DataSource = lst_rpt;
        }
    }
}
