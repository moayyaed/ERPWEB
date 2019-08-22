using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Info.Reportes.Inventario;
using Core.Erp.Bus.Reportes.Inventario;
using System.Collections.Generic;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.Inventario
{
    public partial class INV_009_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public INV_009_Rpt()
        {
            InitializeComponent();
        }

        private void INV_009_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            int IdBodega = string.IsNullOrEmpty(p_IdBodega.Value.ToString()) ? 0 : Convert.ToInt32(p_IdBodega.Value);
            decimal IdProducto = string.IsNullOrEmpty(p_IdProducto.Value.ToString()) ? 0 : Convert.ToInt32(p_IdProducto.Value);
            int IdCategoria = string.IsNullOrEmpty(p_IdCategoria.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCategoria.Value);
            int IdLinea = string.IsNullOrEmpty(p_IdLinea.Value.ToString()) ? 0 : Convert.ToInt32(p_IdLinea.Value);
            int IdGrupo = string.IsNullOrEmpty(p_IdGrupo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdGrupo.Value);
            int IdSubgrupo = string.IsNullOrEmpty(p_IdSubgrupo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSubgrupo.Value);
            bool considerarSinAprobar = string.IsNullOrEmpty(p_considerarSinAprobar.Value.ToString()) ? false : Convert.ToBoolean(p_considerarSinAprobar.Value);
            bool mostrarSinMovimiento = string.IsNullOrEmpty(p_mostrarSinMovimiento.Value.ToString()) ? false : Convert.ToBoolean(p_mostrarSinMovimiento.Value);
            DateTime fecha_ini = string.IsNullOrEmpty(p_fecha_ini.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fecha_fin = string.IsNullOrEmpty(p_fecha_fin.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha_fin.Value);
            string IdUsuario = string.IsNullOrEmpty(p_IdUsuario.Value.ToString()) ? "" : Convert.ToString(p_IdUsuario.Value);
            INV_009_Bus bus_rpt = new INV_009_Bus();

            List<INV_009_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, IdUsuario,  IdSucursal, IdBodega, IdProducto, IdCategoria, IdLinea, IdGrupo, IdSubgrupo, considerarSinAprobar, mostrarSinMovimiento , fecha_ini, fecha_fin);
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
