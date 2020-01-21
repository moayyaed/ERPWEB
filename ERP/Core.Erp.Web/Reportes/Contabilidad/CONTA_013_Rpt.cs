using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Info.Reportes.Contabilidad;
using Core.Erp.Bus.Reportes.Contabilidad;
using System.Collections.Generic;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_013_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public CONTA_013_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_013_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            DateTime fechaIni = p_fechaini.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaini.Value);
            DateTime fechaFin = p_fechafin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechafin.Value);
            int IdPunto_cargo_grupo = p_IdPunto_cargo_grupo.Value == null ? 0 : Convert.ToInt32(p_IdPunto_cargo_grupo.Value);

            CONTA_013_Bus bus_rpt = new CONTA_013_Bus();
            List<CONTA_013_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdPunto_cargo_grupo, fechaIni, fechaFin);

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
