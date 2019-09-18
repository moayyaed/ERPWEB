using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Info.Reportes.Contabilidad;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_007_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        List<CONTA_007_Info> lst_rpt = new List<CONTA_007_Info>();
        public CONTA_007_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_007_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            DateTime fechaini = p_fechaini.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaini.Value);
            DateTime fechafin = p_fechafin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechafin.Value);
            string IdUsuario = p_IdUsuario.Value == null ? "" : Convert.ToString(p_IdUsuario.Value);
            bool MostrarDetallado = p_MostrarDetallado.Value == null ? false : Convert.ToBoolean(p_MostrarDetallado.Value);
            bool MostrarAcumulado = string.IsNullOrEmpty(p_MostrarAcumulado.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarAcumulado.Value);
            CONTA_007_Bus bus_rpt = new CONTA_007_Bus();
            string Sucursal = "";

            tb_FiltroReportes_Bus bus_filtro = new tb_FiltroReportes_Bus();
            Sucursal = bus_filtro.GuardarDB(IdEmpresa, IntArray, IdUsuario);

            lst_rpt.AddRange(bus_rpt.GetList(IdEmpresa, IdUsuario, fechaini, fechafin, MostrarAcumulado, MostrarDetallado));
            lbl_sucursal_cab.Text = Sucursal;
            this.DataSource = lst_rpt;

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null)
            {
                if (emp.em_logo != null)
                {
                    ImageConverter obj = new ImageConverter();
                    lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                }
            }
        }
    }
}
