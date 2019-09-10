using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using Core.Erp.Info.Reportes.CuentasPorPagar;
using Core.Erp.Bus.Reportes.CuentasPorPagar;
using Core.Erp.Bus.General;

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

            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = p_IdSucursal.Value == null ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdProveedor = string.IsNullOrEmpty(p_IdProveedor.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdProveedor.Value);
            DateTime fecha_corte = p_fecha_corte.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_corte.Value);
            bool mostrarSaldo0 = p_mostrarSaldo0.Value == null ? false : Convert.ToBoolean(p_mostrarSaldo0.Value);
            int IdClaseProveedor = string.IsNullOrEmpty(p_IdClaseProveedor.Value.ToString()) ? 0 : Convert.ToInt32(p_IdClaseProveedor.Value);

            if (Convert.ToBoolean(p_MostrarObservacion.Value))
            {
                CeldaObservacion.WordWrap = true;
                CeldaObservacion.CanGrow = true;
            }
            else
            {
                CeldaObservacion.WordWrap = false;
                CeldaObservacion.CanGrow = false;
            }

            CXP_015_Bus bus_rpt = new CXP_015_Bus();
            List<CXP_015_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, IdSucursal, IdProveedor, fecha_corte, mostrarSaldo0, IdClaseProveedor);
            this.DataSource = lst_rpt;


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
            }
        }

        private void GrupoClase_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_QuitarGrupo.Value))
            {
                e.Cancel = true;
            }
        }

        private void GrupoProveedor_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_QuitarGrupo.Value))
            {
                e.Cancel = true;
            }
        }

        private void PieProveedor_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_QuitarGrupo.Value))
            {
                e.Cancel = true;
            }
        }

        private void PieClase_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_QuitarGrupo.Value))
            {
                e.Cancel = true;
            }
        }
    }
}
