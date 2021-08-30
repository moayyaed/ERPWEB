using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using Core.Erp.Bus.General;
using DevExpress.XtraReports.UI.CrossTab;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_014_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public CONTA_014_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_014_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;

            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            DateTime FechaDesde = p_FechaDesde.Value == null ? DateTime.Now.Date : Convert.ToDateTime(p_FechaDesde.Value);
            DateTime FechaHasta = p_FechaHasta.Value == null ? DateTime.Now.Date : Convert.ToDateTime(p_FechaHasta.Value);
            bool MostrarAcumulado = p_MostrarAcumulado.Value == null ? false : Convert.ToBoolean(p_MostrarAcumulado.Value);

            CONTA_014_Bus bus_rpt = new CONTA_014_Bus();
            List<CONTA_014_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, FechaDesde,FechaHasta,MostrarAcumulado);
            xrCrossTab1.DataSource = lst_rpt;

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null )
            {
                lblDireccion.Text = emp.em_direccion;
                lblTelefono.Text = string.IsNullOrEmpty(emp.em_telefonos) ? "" : "Tel. " +emp.em_telefonos;
                if (emp.em_logo != null)
                {
                    ImageConverter obj = new ImageConverter();
                    lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                }
            }
            
        }

        private void crossTabDataCell1_BeforePrint(object sender, DevExpress.XtraReports.UI.CrossTab.CrossTabCellPrintEventArgs e)
        {
            XRCrossTabCell cell = (XRCrossTabCell)sender;
            var Orden = cell.GetCurrentFieldValue<double>("gc_Orden");
            if (Orden == 9)
            {
                decimal value = string.IsNullOrEmpty(cell.Text) ? 0 : Convert.ToDecimal(cell.Text);
                if (value > 0)
                    cell.ForeColor = Color.Green;
                else if (value == 0)
                    cell.ForeColor = Color.Black;
                else cell.ForeColor = Color.Red;
            }
            else
                cell.ForeColor = Color.Black;
        }

        private void crossTabTotalCell3_BeforePrint(object sender, CrossTabCellPrintEventArgs e)
        {
            XRCrossTabCell cell = (XRCrossTabCell)sender;
            var Orden = cell.GetCurrentFieldValue<double>("gc_Orden");
            if (Orden == 9)
            {
                decimal value = string.IsNullOrEmpty(cell.Text) ? 0 : Convert.ToDecimal(cell.Text);
                if (value > 0)
                    cell.ForeColor = Color.Green;
                else if (value == 0)
                    cell.ForeColor = Color.Black;
                else cell.ForeColor = Color.Red;
            }
            else
                cell.ForeColor = Color.Black;
        }
    }
}
