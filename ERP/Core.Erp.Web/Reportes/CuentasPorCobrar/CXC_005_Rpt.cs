using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.CuentasPorCobrar;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System.Collections.Generic;
using System.Linq;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.CuentasPorCobrar
{
    public partial class CXC_005_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        public CXC_005_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_005_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);            
            int Idtipo_cliente = string.IsNullOrEmpty(p_Idtipo_cliente.Value.ToString()) ? 0 : Convert.ToInt32(p_Idtipo_cliente.Value);
            decimal IdCLiente = string.IsNullOrEmpty(p_IdCliente.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdCliente.Value);
            DateTime fecha_corte = string.IsNullOrEmpty(p_fecha_corte.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha_corte.Value);
            bool mostrarSaldo0 = string.IsNullOrEmpty(p_mostrarSaldo0.Value.ToString()) ? false : Convert.ToBoolean(p_mostrarSaldo0.Value);
            List<CXC_005_Info> lst_rpt = new List<CXC_005_Info>();


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            lbl_empresa.Text = emp.em_nombre;

            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                logo.Image = (Image)obj.ConvertFrom(emp.em_logo);
            }
            CXC_005_Bus bus_rpt = new CXC_005_Bus();
            
            if (IntArray != null)
            {
                for (int i = 0; i < IntArray.Count(); i++)
                {
                    lst_rpt.AddRange(bus_rpt.get_list(IdEmpresa, IntArray[i], IdCLiente, Idtipo_cliente, fecha_corte, mostrarSaldo0));
                }
            }
            this.DataSource = lst_rpt;
        }
    }
}
