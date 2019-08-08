using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.CuentasPorPagar;
using System.Collections.Generic;
using Core.Erp.Info.Reportes.CuentasPorPagar;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.CuentasPorPagar
{
    public partial class CXP_001_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public CXP_001_Rpt()
        {
            InitializeComponent();
        }

        private void CXP_001_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdTipoCbte_Ogiro = p_IdTipoCbte_Ogiro.Value == null ? 0 : Convert.ToInt32(p_IdTipoCbte_Ogiro.Value);
            decimal IdCbteCble_Ogiro = p_IdCbteCble_Ogiro.Value == null ? 0 : Convert.ToDecimal(p_IdCbteCble_Ogiro.Value);

            CXP_001_Bus bus_rpt = new CXP_001_Bus();
            List<CXP_001_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdTipoCbte_Ogiro, IdCbteCble_Ogiro);
            this.DataSource = lst_rpt;


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
            }
        }

        private void SubReporte_retenciones_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa_Ogiro"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
                ((XRSubreport)sender).ReportSource.Parameters["p_IdTipoCbte_Ogiro"].Value = p_IdTipoCbte_Ogiro.Value == null ? 0 : Convert.ToInt32(p_IdTipoCbte_Ogiro.Value);
                ((XRSubreport)sender).ReportSource.Parameters["p_IdCbteCble_Ogiro"].Value = p_IdCbteCble_Ogiro.Value == null ? 0 : Convert.ToDecimal(p_IdCbteCble_Ogiro.Value);
                ((XRSubreport)sender).ReportSource.RequestParameters = false;
            }
            catch (Exception)
            {
                
            }

        }

        private void Subreporte_detalle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
                ((XRSubreport)sender).ReportSource.Parameters["p_IdTipoCbte_Ogiro"].Value = p_IdTipoCbte_Ogiro.Value == null ? 0 : Convert.ToInt32(p_IdTipoCbte_Ogiro.Value);
                ((XRSubreport)sender).ReportSource.Parameters["p_IdCbteCble_Ogiro"].Value = p_IdCbteCble_Ogiro.Value == null ? 0 : Convert.ToDecimal(p_IdCbteCble_Ogiro.Value);
                ((XRSubreport)sender).ReportSource.RequestParameters = false;
            }
            catch (Exception)
            {
                
            }
        }
    }
}
