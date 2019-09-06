using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Reportes.CuentasPorCobrar;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.CuentasPorCobrar
{
    public partial class CXC_006_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public string[] StringArray { get; set; }
        public CXC_006_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_006_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdVendedor = string.IsNullOrEmpty(p_IdVendedor.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdVendedor.Value);
            decimal IdCliente = string.IsNullOrEmpty(p_IdCliente.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdCliente.Value);
            DateTime fecha_ini = string.IsNullOrEmpty(p_fecha_ini.Value.ToString()) ? DateTime.Now.Date : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fecha_fin = string.IsNullOrEmpty(p_fecha_fin.Value.ToString()) ? DateTime.Now.Date : Convert.ToDateTime(p_fecha_fin.Value);

            CXC_006_Bus bus_rpt = new CXC_006_Bus();
            List<CXC_006_Info> lst_rpt = new List<CXC_006_Info>();

            if (StringArray != null)
            {
                foreach (var item in StringArray)
                {
                    lst_rpt.AddRange(bus_rpt.GetList(IdEmpresa, IdSucursal, IdVendedor, IdCliente, item, fecha_ini, fecha_fin));
                }
            }

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
