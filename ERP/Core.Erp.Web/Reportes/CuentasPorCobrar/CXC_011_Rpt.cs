using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.CuentasPorCobrar;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System.Collections.Generic;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.CuentasPorCobrar
{
    public partial class CXC_011_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public CXC_011_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_011_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_usuario.Text = usuario;
            lbl_empresa.Text = empresa;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdCliente = string.IsNullOrEmpty(p_IdCliente.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdCliente.Value);
            int Idtipo_cliente = string.IsNullOrEmpty(p_Idtipo_cliente.Value.ToString()) ? 0 : Convert.ToInt32(p_Idtipo_cliente.Value);
            int DiasVencimiento = string.IsNullOrEmpty(p_DiasVencimiento.Value.ToString()) ? 0 : Convert.ToInt32(p_DiasVencimiento.Value);
            DateTime fechaCorte = p_fechaCorte.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaCorte.Value);
            bool MostrarSoloCarteraVencida = p_MostrarSoloCarteraVencida.Value == null ? false : Convert.ToBoolean(p_MostrarSoloCarteraVencida.Value);

            CXC_011_Bus bus_rpt = new CXC_011_Bus();
            List<CXC_011_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdSucursal, IdCliente, Idtipo_cliente, fechaCorte, MostrarSoloCarteraVencida, DiasVencimiento);
            this.DataSource = lst_rpt;


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
            }
        }
    }
}
