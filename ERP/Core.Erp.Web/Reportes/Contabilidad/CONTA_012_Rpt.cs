using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Erp.Bus.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using Core.Erp.Bus.General;
using System.Collections.Generic;
using Core.Erp.Bus.Contabilidad;
using System.Linq;

namespace Core.Erp.Web.Reportes.Contabilidad
{
    public partial class CONTA_012_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public int[] IntArray { get; set; }
        List<CONTA_012_Info> lst_rpt = new List<CONTA_012_Info>();
        public CONTA_012_Rpt()
        {
            InitializeComponent();
        }

        private void CONTA_012_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
            CONTA_012_Bus bus_rpt = new CONTA_012_Bus();
            int cont = 1;
            foreach (var item in IntArray)
            {
                var info_periodo = bus_periodo.get_info(IdEmpresa, item);
                List<CONTA_012_Info> lst = bus_rpt.get_list(IdEmpresa, item, cont);
                switch (cont)
                {
                    case 1:
                        col1.Text = info_periodo.AnioMes;
                        break;
                    case 2:
                        col2.Text = info_periodo.AnioMes;
                        break;
                    case 3:
                        col3.Text = info_periodo.AnioMes;
                        break;
                    case 4:
                        col4.Text = info_periodo.AnioMes;
                        break;
                    case 5:
                        col5.Text = info_periodo.AnioMes;
                        break;
                    case 6:
                        col6.Text = info_periodo.AnioMes;
                        break;
                    case 7:
                        col7.Text = info_periodo.AnioMes;
                        break;
                    case 8:
                        col8.Text = info_periodo.AnioMes;
                        break;
                    case 9:
                        col9.Text = info_periodo.AnioMes;
                        break;
                    case 10:
                        col10.Text = info_periodo.AnioMes;
                        break;
                    case 11:
                        col11.Text = info_periodo.AnioMes;
                        break;
                    case 12:
                        col12.Text = info_periodo.AnioMes;
                        break;
                }
                lst_rpt.AddRange(lst);
                cont++;
            }

            lst_rpt = lst_rpt.GroupBy(q=> new { q.IdEmpresa, q.IdCta, q.IdTipo_Gasto, q.nivel, q.nom_cuenta, q.nom_grupo_CC, q.nom_tipo_Gasto, q.nom_tipo_Gasto_padre, q.orden, q.orden_tipo_gasto })
                .Select(q=> new CONTA_012_Info
                {
                    IdEmpresa = q.Key.IdEmpresa,
                    IdCta = q.Key.IdCta,
                    IdTipo_Gasto = q.Key.IdTipo_Gasto,
                    nivel = q.Key.nivel,
                    nom_cuenta = q.Key.nom_cuenta,
                    nom_grupo_CC = q.Key.nom_grupo_CC,
                    nom_tipo_Gasto = q.Key.nom_tipo_Gasto,
                    nom_tipo_Gasto_padre = q.Key.nom_tipo_Gasto_padre,
                    orden = q.Key.orden,
                    orden_tipo_gasto = q.Key.orden_tipo_gasto,
                    Col1 = q.Sum(g=>g.Col1),
                    Col2 = q.Sum(g => g.Col2),
                    Col3 = q.Sum(g => g.Col3),
                    Col4 = q.Sum(g => g.Col4),
                    Col5 = q.Sum(g => g.Col5),
                    Col6 = q.Sum(g => g.Col6),
                    Col7 = q.Sum(g => g.Col7),
                    Col8 = q.Sum(g => g.Col8),
                    Col9 = q.Sum(g => g.Col9),
                    Col10 = q.Sum(g => g.Col10),
                    Col11 = q.Sum(g => g.Col11),
                    Col12 = q.Sum(g => g.Col12)
                }).ToList();

            foreach (var item in lst_rpt)
            {
                item.Col1 = item.Col1 == 0 ? null : item.Col1;
                item.Col2 = item.Col2 == 0 ? null : item.Col2;
                item.Col3 = item.Col3 == 0 ? null : item.Col3;
                item.Col4 = item.Col4 == 0 ? null : item.Col4;
                item.Col5 = item.Col5 == 0 ? null : item.Col5;
                item.Col6 = item.Col6 == 0 ? null : item.Col6;
                item.Col7 = item.Col7 == 0 ? null : item.Col7;
                item.Col8 = item.Col8 == 0 ? null : item.Col8;
                item.Col9 = item.Col9 == 0 ? null : item.Col9;
                item.Col10 = item.Col10 == 0 ? null : item.Col10;
                item.Col11 = item.Col11 == 0 ? null : item.Col11;
                item.Col12 = item.Col12 == 0 ? null : item.Col12;
            }
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
