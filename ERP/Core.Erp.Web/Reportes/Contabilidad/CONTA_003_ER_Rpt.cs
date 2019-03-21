using Core.Erp.Bus.General;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_003_ER_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public CONTA_003_ER_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_003_ER_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdAnio = p_IdAnio.Value == null ? 0 : Convert.ToInt32(p_IdAnio.Value);
            DateTime fechaIni = p_fechaIni.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaIni.Value);
            DateTime fechaFin = p_fechaFin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaFin.Value);
            string IdUsuario = p_IdUsuario.Value == null ? "" : Convert.ToString(p_IdUsuario.Value);
            int IdNivel = p_IdNivel.Value == null ? 0 : Convert.ToInt32(p_IdNivel.Value);
            bool mostrarSaldo0 = p_mostrarSaldo0.Value == null ? false : Convert.ToBoolean(p_mostrarSaldo0.Value);
            string balance = p_balance.Value == null ? "" : Convert.ToString(p_balance.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            bool MostrarSaldoAcumulado = string.IsNullOrEmpty(p_MostrarSaldoAcumulado.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarSaldoAcumulado.Value);
            CONTA_003_balances_Bus bus_rpt = new CONTA_003_balances_Bus();
            List<CONTA_003_balances_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdAnio, fechaIni, fechaFin, IdUsuario, IdNivel, mostrarSaldo0, balance,IdSucursal, MostrarSaldoAcumulado);
            this.DataSource = lst_rpt;

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);

            ImageConverter obj = new ImageConverter();
            lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
        }
    }
}
