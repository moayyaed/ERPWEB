using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using Core.Erp.Bus.General;
using System.Collections.Generic;
using Core.Erp.Bus.Contabilidad;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_010_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public string[] StringArray { get; set; }
        public CONTA_010_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_010_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            var Grupo = "";
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            string IdCtaCble = p_IdCtaCble.Value == null ? "" : Convert.ToString(p_IdCtaCble.Value);
            DateTime FechaIni = p_fechaini.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechaini.Value);
            DateTime FechaFin = p_fechafin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fechafin.Value);

            CONTA_010_Bus bus_rpt = new CONTA_010_Bus();
            List<CONTA_010_Info> lst_rpt = new List<CONTA_010_Info>();
            ct_grupocble_Bus bus_grupo = new ct_grupocble_Bus();

            foreach (var item in StringArray)
            {
                lst_rpt.AddRange(bus_rpt.get_list(IdEmpresa, item, IdCtaCble, FechaIni, FechaFin));
                Grupo += bus_grupo.get_info(item).gc_GrupoCble.ToString().TrimEnd()+", ";
            }

            this.DataSource = lst_rpt;
            lbl_GrupoCble.Text = Grupo;
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

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!Convert.ToBoolean(p_MostrarGrupo.Value))
            {
                e.Cancel = true;
            }
        }

        private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!Convert.ToBoolean(p_MostrarGrupo.Value))
            {
                e.Cancel = true;
            }
        }
    }
}
