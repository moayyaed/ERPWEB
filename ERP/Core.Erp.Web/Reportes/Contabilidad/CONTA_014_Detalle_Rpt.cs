using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System.Collections.Generic;
using Core.Erp.Bus.General;
using System.Linq;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_014_Detalle_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        List<CONTA_003_balances_Info> lst_rpt = new List<CONTA_003_balances_Info>();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        public CONTA_014_Detalle_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_014_Detalle_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            DateTime FechaDesde = p_FechaDesde.Value == null ? DateTime.Now.Date : Convert.ToDateTime(p_FechaDesde.Value);
            DateTime FechaHasta = p_FechaHasta.Value == null ? DateTime.Now.Date : Convert.ToDateTime(p_FechaHasta.Value);
            bool MostrarAcumulado = p_MostrarAcumulado.Value == null ? false : Convert.ToBoolean(p_MostrarAcumulado.Value);

            CONTA_014_Bus bus_rpt = new CONTA_014_Bus();
            List<CONTA_014_Detalle_Info> lst_rpt = bus_rpt.GetList_Detalle(IdEmpresa, FechaDesde, FechaHasta, MostrarAcumulado);
            this.DataSource = lst_rpt;

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null)
            {
                lblDireccion.Text = emp.em_direccion;
                lblTelefono.Text = string.IsNullOrEmpty(emp.em_telefonos) ? "" : "Tel. " + emp.em_telefonos;
                if (emp.em_logo != null)
                {
                    ImageConverter obj = new ImageConverter();
                    lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                }
            }

        }
    }
}
