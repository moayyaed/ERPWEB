using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Info.Reportes.ActivoFijo;
using Core.Erp.Bus.Reportes.ActivoFijo;
using System.Collections.Generic;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.ActivoFijo
{
    public partial class ACTF_003_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public ACTF_003_Rpt()
        {
            InitializeComponent();
        }



        private void ACTF_003_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            lbl_fecha.Text = DateTime.Now.ToString("d/MM/yyyy hh:mm:ss");
            lbl_usuario.Text = usuario;
            lbl_empresa.Text = empresa;


            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            decimal IdRetiroActivo = p_IdRetiroActivo.Value == null ? 0 : Convert.ToDecimal(p_IdRetiroActivo.Value);

            ACTF_003_Bus bus_rpt = new ACTF_003_Bus();
            List<ACTF_003_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdRetiroActivo);
            this.DataSource = lst_rpt;


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
            }
        }
    }
}
