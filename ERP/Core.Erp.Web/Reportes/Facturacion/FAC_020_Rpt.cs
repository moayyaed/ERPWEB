using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Facturacion;
using Core.Erp.Info.Reportes.Facturacion;
using System.Collections.Generic;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Reportes.Facturacion
{
    public partial class FAC_020_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public FAC_020_Rpt()
        {
            InitializeComponent();
        }

        private void FAC_020_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = p_IdSucursal.Value == null ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            int IdBodega = p_IdBodega.Value == null ? 0 : Convert.ToInt32(p_IdBodega.Value);
            int IdGuiaRemision = p_IdGuiaRemision.Value == null ? 0 : Convert.ToInt32(p_IdGuiaRemision.Value);

            FAC_020_Bus bus_rpt = new FAC_020_Bus();
            List<FAC_020_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdSucursal, IdBodega, IdGuiaRemision);
            this.DataSource = lst_rpt;


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var empresa = bus_empresa.get_info(IdEmpresa);
            lbl_empresa.Text = empresa.em_nombre;
            lbl_direccion.Text = empresa.em_direccion;
            lbl_contribuyente.Text = empresa.ContribuyenteEspecial;
            lbl_ruc.Text = empresa.em_ruc;

            if (empresa != null && empresa.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(empresa.em_logo);
            }
        }
    }
}
