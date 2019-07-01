using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Info.Reportes.Facturacion;
using Core.Erp.Bus.Reportes.Facturacion;
using System.Collections.Generic;
using System.Linq;

namespace Core.Erp.Web.Reportes.Facturacion
{
    public partial class FAC_018_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        List<FAC_018_Info> Lista = new List<FAC_018_Info>();

        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        public FAC_018_Rpt()
        {
            InitializeComponent();
        }

        private void FAC_018_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdCliente = string.IsNullOrEmpty(p_IdCliente.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdCliente.Value);
            int IdTipoNota = string.IsNullOrEmpty(p_IdTipoNota.Value.ToString()) ? 0 : Convert.ToInt32(p_IdTipoNota.Value);
            DateTime fecha_ini = string.IsNullOrEmpty(p_fecha_ini.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fecha_fin = string.IsNullOrEmpty(p_fecha_fin.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha_fin.Value);
            bool mostrar_anulados = p_mostrar_anulados.Value == null ? false : Convert.ToBoolean(p_mostrar_anulados.Value);

            FAC_018_Bus bus_rpt = new FAC_018_Bus();
            List<FAC_018_Info> lst_rpt = bus_rpt.GetList(IdEmpresa, IdSucursal, IdCliente, IdTipoNota, fecha_ini, fecha_fin, mostrar_anulados);

            #region Grupo

            Lista = (from q in lst_rpt
                     group q by new
                     {
                         q.IdEmpresa,
                         q.IdSucursal,
                         q.No_Descripcion
                     } into Area
                     select new FAC_018_Info
                     {
                         Total = Area.Sum(q => q.Total),
                         IdEmpresa = Area.Key.IdEmpresa,
                         IdSucursal = Area.Key.IdSucursal,
                         No_Descripcion = Area.Key.No_Descripcion

                     }).ToList();
            
            #endregion


            this.DataSource = lst_rpt;

        }

        private void SubReporte_resumen_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.DataSource = Lista;

        }
    }
}
