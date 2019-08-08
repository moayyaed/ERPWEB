using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.CuentasPorPagar;
using Core.Erp.Info.Reportes.CuentasPorPagar;
using System.Collections.Generic;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.CuentasPorPagar
{
    public partial class CXP_017_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public CXP_017_Rpt()
        {
            InitializeComponent();
        }

        private void CXP_017_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                lbl_empresa.Text = empresa;
                lbl_usuario.Text = usuario;
                int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
                string selectedIDs = p_selectedIDs.Value == null ? "" : Convert.ToString(p_selectedIDs.Value);
                string[] array = selectedIDs.Split(',');

                CXP_017_Bus bus_rpt = new CXP_017_Bus();
                List<CXP_017_Info> lst_rpt = new List<CXP_017_Info>();
                List<decimal> LstOP = new List<decimal>();

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

                foreach (var item in array)
                {
                    LstOP.Add(Convert.ToDecimal(item));
                }

                lst_rpt = bus_rpt.GetList(IdEmpresa, LstOP);
                this.DataSource = lst_rpt;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
