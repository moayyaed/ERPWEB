using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using Core.Erp.Bus.General;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_012_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        public CONTA_012_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_012_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdPeriodo = p_IdPeriodo.Value == null ? 0 : Convert.ToInt32(p_IdPeriodo.Value);

            CONTA_012_Bus bus_rpt = new CONTA_012_Bus();
            foreach (var item in IntArray)
            {

            }
            List<CONTA_012_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdPeriodo);

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
