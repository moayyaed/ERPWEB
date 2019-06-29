using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Info.Reportes.Facturacion;
using Core.Erp.Bus.Reportes.Facturacion;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.Facturacion
{
    public partial class FAC_018_Rpt : DevExpress.XtraReports.UI.XtraReport
    {

        public string usuario { get; set; }
        public string empresa { get; set; }
        public FAC_018_Rpt()
        {
            InitializeComponent();
        }

        private void FAC_018_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;

            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = p_IdSucursal.Value == null ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdCliente = p_IdCliente.Value == null ? 0 : Convert.ToDecimal(p_IdCliente.Value);
            int IdTipoNota = p_IdTipoNota.Value == null ? 0 : Convert.ToInt32(p_IdTipoNota.Value);
            DateTime fecha_ini = p_fecha_ini.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fecha_fin = p_fecha_fin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_fin.Value);
            bool mostrar_anulados = p_mostrar_anulados.Value == null ? false : Convert.ToBoolean(p_mostrar_anulados.Value);

            FAC_018_Bus bus_rpt = new FAC_018_Bus();
            List<FAC_018_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, IdSucursal, IdCliente, IdTipoNota, fecha_ini, fecha_fin, mostrar_anulados);
            this.DataSource = lst_rpt;

        }
    }
}
