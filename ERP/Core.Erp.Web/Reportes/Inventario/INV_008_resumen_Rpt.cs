using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Inventario;
using Core.Erp.Info.Reportes.Inventario;
using Core.Erp.Bus.General;
using System.Collections.Generic;

namespace Core.Erp.Web.Reportes.Inventario
{
    public partial class INV_008_resumen_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public INV_008_resumen_Rpt()
        {
            InitializeComponent();
        }

        private void INV_008_resumen_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_empresa.Text = empresa;
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            int IdBodega = string.IsNullOrEmpty(p_IdBodega.Value.ToString()) ? 0 : Convert.ToInt32(p_IdBodega.Value);
            int IdProducto = string.IsNullOrEmpty(p_IdProducto.Value.ToString()) ? 0 : Convert.ToInt32(p_IdProducto.Value);
            DateTime fecha_ini = string.IsNullOrEmpty(p_fecha_ini.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fecha_fin = string.IsNullOrEmpty(p_fecha_fin.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha_fin.Value);
            string IdCentroCosto = string.IsNullOrEmpty(p_IdCentroCosto.Value.ToString()) ? "" : Convert.ToString(p_IdCentroCosto.Value);
            string signo = string.IsNullOrEmpty(p_signo.Value.ToString()) ? "" : Convert.ToString(p_signo.Value);
            int IdMovi_inven_tipo = string.IsNullOrEmpty(p_IdMovi_Inven_Tipo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdMovi_Inven_Tipo.Value);
            int IdProductoTipo = string.IsNullOrEmpty(p_IdProductoTipo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdProductoTipo.Value);
            string IdCategoria = string.IsNullOrEmpty(p_IdCategoria.Value.ToString()) ? "" : p_IdCategoria.Value.ToString();
            int IdLinea = string.IsNullOrEmpty(p_IdLinea.Value.ToString()) ? 0 : Convert.ToInt32(p_IdLinea.Value);
            int IdGrupo = string.IsNullOrEmpty(p_IdGrupo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdGrupo.Value);
            int IdSubGrupo = string.IsNullOrEmpty(p_IdSubGrupo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSubGrupo.Value);
            INV_008_Bus bus_rpt = new INV_008_Bus();

            List<INV_008_Info> lst_rpt = bus_rpt.GetListResumen(IdEmpresa, IdSucursal, IdBodega, IdProducto, fecha_ini, fecha_fin, IdCentroCosto, signo, IdMovi_inven_tipo, IdProductoTipo, IdCategoria, IdLinea, IdGrupo, IdSubGrupo);
            this.DataSource = lst_rpt;
            xrCrossTab1.DataSource = lst_rpt;
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
