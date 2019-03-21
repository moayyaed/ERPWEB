using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.CuentasPorPagar
{
    public partial class CXP_015_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public CXP_015_Rpt()
        {
            InitializeComponent();
        }

        private void CXP_015_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;

            //int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            //int IdSucursal = p_IdSucursal.Value == null ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            //decimal IdCLiente = string.IsNullOrEmpty(p_IdCliente.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdCliente.Value);
            //DateTime fecha_corte = p_fecha_corte.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_corte.Value);
            //bool mostrarSaldo0 = p_mostrarSaldo0.Value == null ? false : Convert.ToBoolean(p_mostrarSaldo0.Value);

            //CXP_015_Bus bus_rpt = new CXP_015_Bus();
            //List<CXP_015_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdSucursal, IdCLiente, fecha_corte, mostrarSaldo0);
            //this.DataSource = lst_rpt;
        }
    }
}
