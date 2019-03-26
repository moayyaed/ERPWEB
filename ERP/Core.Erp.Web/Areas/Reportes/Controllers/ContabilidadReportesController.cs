using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using Core.Erp.Web.Reportes.Contabilidad;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Reportes.Controllers
{
    [SessionTimeout]
    public class ContabilidadReportesController : Controller
    {
        #region Combos

        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        public ActionResult CmbCuenta_contable_Conta()
        {
            cl_filtros_Info model = new cl_filtros_Info();

            return PartialView("_CmbCuenta_contable_Conta", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda_cta(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda_cta(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }


        #endregion
        public ActionResult CONTA_001(int IdTipoCbte = 0, decimal IdCbteCble = 0)
        {
            CONTA_001_Rpt model = new CONTA_001_Rpt();
            model.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.p_IdTipoCbte.Value = IdTipoCbte;
            model.p_IdCbteCble.Value = IdCbteCble;
            model.usuario = SessionFixed.IdUsuario;
            model.empresa = SessionFixed.NomEmpresa;
            model.RequestParameters = false;
            return View(model);
        }
        private void cargar_combos(int IdEmpresa)
        {
            ct_plancta_Bus bus_cta = new ct_plancta_Bus();
            var lst_cta = bus_cta.get_list(IdEmpresa, false, false);
            ViewBag.lst_cta = lst_cta;

            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            lst_sucursal.Add(new Info.General.tb_sucursal_Info
            {
                IdSucursal = 0,
                Su_Descripcion = "TODOS"
            });
            ViewBag.lst_sucursal = lst_sucursal;
        }
        private void cargar_nivel()
        {
            Dictionary<int, string> lst_nivel = new Dictionary<int, string>();
            lst_nivel.Add(6, "Nivel 6");
            lst_nivel.Add(5, "Nivel 5");
            lst_nivel.Add(4, "Nivel 4");
            lst_nivel.Add(3, "Nivel 3");
            lst_nivel.Add(2, "Nivel 2");
            lst_nivel.Add(1, "Nivel 1");
            ViewBag.lst_nivel = lst_nivel;

            Dictionary<string, string> lst_balance = new Dictionary<string, string>();
            lst_balance.Add("BG", "Balance general");
            lst_balance.Add("ER", "Estado de resultado");
            lst_balance.Add("", "Balance de comprobación");
            ViewBag.lst_balance = lst_balance;
        }
        public ActionResult CONTA_002()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdCtaCble = "",
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal)
            };
            cargar_combos(model.IdEmpresa);
            CONTA_002_Rpt report = new CONTA_002_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCtaCble.Value = model.IdCtaCble;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_002(cl_filtros_Info model)
        {
            CONTA_002_Rpt report = new CONTA_002_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCtaCble.Value = model.IdCtaCble;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            cargar_combos(model.IdEmpresa);
            ViewBag.Report = report;
            return View(model);
        }
        public ActionResult CONTA_003()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdNivel = 6,
                balance = "ER"
            };
            model.IdAnio = model.fecha_fin.Year;
            model.MostrarSaldoAcumulado = true;
            cargar_combos(model.IdEmpresa);
            CONTA_003_ER_Rpt report = new CONTA_003_ER_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_balance.Value = model.balance;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_nivel();
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_003(cl_filtros_contabilidad_Info model)
        {
            model.IdAnio = model.fecha_fin.Year;
            if (model.balance == "BG")
            {
                CONTA_003_BG_Rpt report = new CONTA_003_BG_Rpt();
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_IdAnio.Value = model.IdAnio;
                report.p_fechaIni.Value = model.fecha_ini;
                report.p_fechaFin.Value = model.fecha_fin;
                report.p_IdUsuario.Value = SessionFixed.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_IdSucursal.Value = model.IdSucursal;
                report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                report.RequestParameters = false;
                ViewBag.Report = report;
            }
            if (model.balance == "ER")
            {
                CONTA_003_ER_Rpt report = new CONTA_003_ER_Rpt();
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_IdAnio.Value = model.IdAnio;
                report.p_fechaIni.Value = model.fecha_ini;
                report.p_fechaFin.Value = model.fecha_fin;
                report.p_IdUsuario.Value = SessionFixed.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_IdSucursal.Value = model.IdSucursal;
                report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                report.RequestParameters = false;
                ViewBag.Report = report;
            }

            if (string.IsNullOrEmpty(model.balance))
            {
                CONTA_003_BC_Rpt report = new CONTA_003_BC_Rpt();
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_IdAnio.Value = model.IdAnio;
                report.p_fechaIni.Value = model.fecha_ini;
                report.p_fechaFin.Value = model.fecha_fin;
                report.p_IdUsuario.Value = SessionFixed.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_IdSucursal.Value = model.IdSucursal;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                report.RequestParameters = false;
                ViewBag.Report = report;
            }
            cargar_combos(model.IdEmpresa);
            cargar_nivel();
            return View(model);
        }
    }
}