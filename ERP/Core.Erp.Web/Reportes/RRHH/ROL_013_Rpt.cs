using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.RRHH;
using Core.Erp.Info.Reportes.RRHH;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.RRHH
{
    public partial class ROL_013_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public ROL_013_Rpt()
        {
            InitializeComponent();
        }

        private void ROL_013_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdNomina = string.IsNullOrEmpty(p_IdNomina.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNomina.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            DateTime FechaIni = string.IsNullOrEmpty(p_FechaIni.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_FechaIni.Value);
            DateTime FechaFin = string.IsNullOrEmpty(p_FechaFin.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_FechaFin.Value);
            decimal IdEmpleado = string.IsNullOrEmpty(p_IdEmpleado.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdEmpleado.Value);
            int IdDivision = 0;
            int IdArea = 0;
            ROL_013_Bus bus_rpt = new ROL_013_Bus();
            List<ROL_013_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdNomina, IdSucursal, FechaIni, FechaFin, IdEmpleado, IdDivision, IdArea);
            this.DataSource = lst_rpt;
        }
    }
}
