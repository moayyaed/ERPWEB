﻿using System;
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
    public partial class CXP_019_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public CXP_019_Rpt()
        {
            InitializeComponent();
        }

        private void CXP_019_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("d/MM/yyyy hh:mm:ss");
            lbl_usuario.Text = usuario;
            lbl_empresa.Text = empresa;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString())? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdProveedor = string.IsNullOrEmpty(p_IdProveedor.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdProveedor.Value);
            DateTime fecha = string.IsNullOrEmpty(p_fecha.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_fecha.Value);
            bool no_mostrar_en_conciliacion = string.IsNullOrEmpty(p_no_mostrar_en_conciliacion.ToString()) ? false : Convert.ToBoolean(p_no_mostrar_en_conciliacion.Value);
            bool no_mostrar_saldo_0 = string.IsNullOrEmpty(p_no_mostrar_saldo_0.ToString()) ? false : Convert.ToBoolean(p_no_mostrar_saldo_0.Value);
            bool mostrarGrupo = string.IsNullOrEmpty(p_mostrarGrupo.ToString()) ? false : Convert.ToBoolean(p_mostrarGrupo.Value);
            int IdClaseProveedor = string.IsNullOrEmpty(p_IdClaseProveedor.Value.ToString()) ? 0 : Convert.ToInt32(p_IdClaseProveedor.Value);

            CXP_019_Bus bus_rpt = new CXP_019_Bus();
            List<CXP_019_Info> lst_rpt = bus_rpt.get_list(IdEmpresa,  fecha, IdSucursal, IdClaseProveedor, IdProveedor, no_mostrar_en_conciliacion, no_mostrar_saldo_0);
            this.DataSource = lst_rpt;


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
            }
        }

        private void EsRelacionadoTit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void EsRelacionadoDet_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_mostrarGrupo.Value))
            {
                e.Cancel = true;
            }
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_mostrarGrupo.Value))
            {
                e.Cancel = true;
            }
        }

        private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_mostrarGrupo.Value))
            {
                e.Cancel = true;
            }
        }

        private void GroupFooter2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(p_mostrarGrupo.Value))
            {
                e.Cancel = true;
            }
        }
    }
}
