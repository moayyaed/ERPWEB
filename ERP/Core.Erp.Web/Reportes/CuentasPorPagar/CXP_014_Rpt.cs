using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.CuentasPorPagar;
using Core.Erp.Info.Reportes.CuentasPorPagar;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.CuentasPorPagar
{
    public partial class CXP_014_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public CXP_014_Rpt()
        {
            InitializeComponent();
        }

        private void CXP_014_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;


            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = p_IdSucursal.Value == null ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdProveedor = p_IdProveedor.Value == null ? 0 : Convert.ToDecimal(p_IdProveedor.Value);
            DateTime fechaIni = p_fecha_ini.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fechaFin = p_fecha_fin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_fin.Value);
            string IdTipoServicio = p_IdTipoServicio.Value == null ? "" : Convert.ToString(p_IdTipoServicio.Value);
            bool mostrar_anulados = p_mostrar_anulados.Value == null ? false : Convert.ToBoolean(p_mostrar_anulados.Value);

            CXP_014_Bus bus_rpt = new CXP_014_Bus();
            List<CXP_014_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, IdSucursal, IdProveedor, fechaIni, fechaFin, IdTipoServicio, mostrar_anulados);
            this.DataSource = lst_rpt;
        }
    }
}
