using Core.Erp.Bus.General;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_009_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        List<CONTA_009_Info> lst_rpt = new List<CONTA_009_Info>();
        List<CONTA_009_Info> lst_cab = new List<CONTA_009_Info>();
        List<CONTA_009_Info> lst_det = new List<CONTA_009_Info>();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        public CONTA_009_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_009_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            DateTime fechaIni = p_fechaIni.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaIni.Value);
            DateTime fechaFin = p_fechaFin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaFin.Value);
            string IdUsuario = p_IdUsuario.Value == null ? "" : Convert.ToString(p_IdUsuario.Value);
            bool mostrarSaldo0 = p_mostrarSaldo0.Value == null ? false : Convert.ToBoolean(p_mostrarSaldo0.Value);
            bool MostrarSaldoAcumulado = string.IsNullOrEmpty(p_MostrarSaldoAcumulado.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarSaldoAcumulado.Value);
            string IdCentroCosto = string.IsNullOrEmpty(p_IdCentroCosto.Value.ToString()) ? "" : p_IdCentroCosto.Value.ToString();
            CONTA_009_Bus bus_rpt = new CONTA_009_Bus();
            string Sucursal = "";

            tb_FiltroReportes_Bus bus_filtro = new tb_FiltroReportes_Bus();
            Sucursal = bus_filtro.GuardarDB(IdEmpresa, IntArray, IdUsuario);

            lst_rpt.AddRange(bus_rpt.get_list(IdEmpresa, fechaIni, fechaFin, IdUsuario, mostrarSaldo0, MostrarSaldoAcumulado,IdCentroCosto));
            txtUtilidadPerdida.Text = (lst_rpt.Where(q => q.IdNivelCta == 1).Sum(q => q.SaldoFinal) * -1).ToString("n2");
            txtCostoDeProduccion.Text = (lst_rpt.Where(q => q.IdNivelCta == 1 && q.IdCtaCble.StartsWith("5")).Sum(q => q.SaldoFinalNaturaleza)).ToString("n2");
            lst_cab = lst_rpt.Where(q => string.IsNullOrEmpty(q.pc_cuenta_padre) && (q.IdCtaCble.StartsWith("4") || q.IdCtaCble.StartsWith("51"))).ToList();
            lst_det = lst_rpt.Where(q => string.IsNullOrEmpty(q.pc_cuenta_padre) && (q.IdCtaCble.StartsWith("6") || q.IdCtaCble.StartsWith("7") || q.IdCtaCble.StartsWith("8") || q.IdCtaCble.StartsWith("9"))).ToList();
            lst_rpt = lst_rpt.Where(q => !string.IsNullOrEmpty(q.pc_cuenta_padre)).ToList();
            lst_rpt.ForEach(q => q.Su_Descripcion = Sucursal);
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

        private void SubCabecera_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.DataSource = lst_cab;
        }

        private void SubPie_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.DataSource = lst_det;
        }
    }
}
