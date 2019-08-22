using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Inventario;
using Core.Erp.Info.Reportes.Inventario;
using System.Collections.Generic;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.Inventario
{
    public partial class INV_012_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public INV_012_Rpt()
        {
            InitializeComponent();
        }

        private void INV_012_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            int IdBodega = string.IsNullOrEmpty(p_IdBodega.Value.ToString()) ? 0 : Convert.ToInt32(p_IdBodega.Value);
            int IdMovi_inven_tipo = string.IsNullOrEmpty(p_IdMovi_inven_tipo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdMovi_inven_tipo.Value);
            string tipo_movi = string.IsNullOrEmpty(p_tipo_movi.Value.ToString()) ? "" : Convert.ToString(p_tipo_movi.Value);
            DateTime fechaIni = p_fecha_ini.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fechaFin = p_fecha_fin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_fin.Value);
            decimal IdNumMovi = string.IsNullOrEmpty(p_IdNumMovi.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdNumMovi.Value);

            INV_012_Bus bus_rpt = new INV_012_Bus();
            List<INV_012_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, IdSucursal, IdBodega, tipo_movi, IdMovi_inven_tipo, IdNumMovi, fechaIni, fechaFin);
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
