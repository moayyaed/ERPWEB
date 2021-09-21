﻿using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using Core.Erp.Web.Reportes.Contabilidad;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult CmbCuenta_contable_ContaFin()
        {
            cl_filtros_Info model = new cl_filtros_Info();

            return PartialView("_CmbCuenta_contable_ContaFin", model);
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

        #region Combo punto cargo grupo
        ct_punto_cargo_Bus bus_punto_cargo = new ct_punto_cargo_Bus();
        ct_punto_cargo_grupo_Bus bus_grupo = new ct_punto_cargo_grupo_Bus();
        public List<ct_punto_cargo_grupo_Info> get_list_bajo_demanda_grupo(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_grupo.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        public ct_punto_cargo_grupo_Info get_info_bajo_demanda_grupo(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_grupo.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        public ActionResult CmbPuntoCargoGrupo()
        {
            int model = new int();
            return PartialView("_CmbPuntoCargoGrupo_Reportes", model);
        }

        public List<ct_punto_cargo_Info> get_list_bajo_demanda_punto(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            int IdGrupo = string.IsNullOrEmpty(SessionFixed.IdDivision_IC) ? 0 : Convert.ToInt32(SessionFixed.IdDivision_IC);
            cl_filtros_Info model = new cl_filtros_Info { IdPunto_cargo_grupo = IdGrupo };
            return bus_punto_cargo.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa),IdGrupo);
        }
        public ct_punto_cargo_Info get_info_bajo_demanda_punto(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_punto_cargo.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        public ActionResult CmbPuntoCargo()
        {
            int IdGrupo = string.IsNullOrEmpty(SessionFixed.IdDivision_IC) ? 0 : Convert.ToInt32(SessionFixed.IdDivision_IC);
            cl_filtros_Info model = new cl_filtros_Info { IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa), IdPunto_cargo_grupo = IdGrupo };
            return PartialView("_CmbPuntoCargo_Reportes", model);
        }
        #endregion

        #region Json
        public JsonResult SetPuntoCargoGrupo(int IdGrupo = 0)
        {
            SessionFixed.IdDivision_IC = IdGrupo.ToString();
            return Json(IdGrupo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarPuntoCargo(int IdEmpresa = 0, int IdPuntoCargoGrupo = 0)
        {
            ct_punto_cargo_Bus bus_punto_cargo = new ct_punto_cargo_Bus();
            var resultado = bus_punto_cargo.GetList(IdEmpresa, IdPuntoCargoGrupo, false,true);
            resultado.Add(new ct_punto_cargo_Info
            {
                IdEmpresa = IdEmpresa,
                IdPunto_cargo = 0,
                nom_punto_cargo = "TODOS"
            });

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        ct_grupocble_Bus bus_grupo_cble = new ct_grupocble_Bus();
        tb_sis_reporte_x_tb_empresa_Bus bus_rep_x_emp = new tb_sis_reporte_x_tb_empresa_Bus();
        string RootReporte = System.IO.Path.GetTempPath() + "Rpt_Facturacion.repx";
        public ActionResult CONTA_001(int IdTipoCbte = 0, decimal IdCbteCble = 0)
        {
            CONTA_001_Rpt model = new CONTA_001_Rpt();
            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var reporte = bus_rep_x_emp.GetInfo(IdEmpresa, "CONTA_001");
            if (reporte != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, reporte.ReporteDisenio);
                model.LoadLayout(RootReporte);
            }
            #endregion
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
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;

            ct_cbtecble_tipo_Bus bus_tipo = new ct_cbtecble_tipo_Bus();
            var lst_tipo = bus_tipo.get_list(IdEmpresa, false);
            ViewBag.lst_tipo = lst_tipo;

            ct_punto_cargo_grupo_Bus bus_punto = new ct_punto_cargo_grupo_Bus();
            var lst_punto = bus_punto.GetList(IdEmpresa, false);
            ViewBag.lst_punto = lst_punto;
        }

        private void cargar_combos_CONTA013(int IdEmpresa, int IdPunto_cargo_grupo)
        {            
            ct_punto_cargo_grupo_Bus bus_punto_grupo = new ct_punto_cargo_grupo_Bus();
            var lst_punto = bus_punto_grupo.GetList(IdEmpresa, false);
            ViewBag.lst_punto = lst_punto;

            ct_punto_cargo_Bus bus_punto = new ct_punto_cargo_Bus();
            var lst_punto_cargo = bus_punto.GetList(IdEmpresa, IdPunto_cargo_grupo, false, false);
            lst_punto_cargo.Add(new ct_punto_cargo_Info
            {
                IdPunto_cargo_grupo = IdPunto_cargo_grupo,
                IdPunto_cargo = 0,
                nom_punto_cargo = "TODOS"
                
            });
            ViewBag.lst_punto_cargo = lst_punto_cargo;
        }
        private void cargar_combo_cbte(cl_filtros_Info model)
        {
            ct_cbtecble_tipo_Bus bus_tipo = new ct_cbtecble_tipo_Bus();
            var lst_tipo = bus_tipo.get_list(model.IdEmpresa, false);
            ViewBag.lst_tipo = lst_tipo;
        }
        private void cargar_cc(int IdEmpresa)
        {
            var lst = bus_cc.GetListPorNivel(IdEmpresa, 1);
            lst.Add(new ct_CentroCosto_Info
            {
                IdCentroCosto = "",
                cc_Descripcion = "Todos"
            });
            ViewBag.lstCC = lst;
        }
        private void cargar_combos_punto_cargo(cl_filtros_Info model)
        {
            ct_punto_cargo_grupo_Bus bus_punto = new ct_punto_cargo_grupo_Bus();
            var lst_punto = bus_punto.GetList(model.IdEmpresa, false);
            lst_punto.Add(new ct_punto_cargo_grupo_Info
            {
                IdEmpresa = model.IdEmpresa,
                IdPunto_cargo_grupo = 0,
                nom_punto_cargo_grupo = "TODOS"
            });
            ViewBag.lst_punto = lst_punto;

            ct_punto_cargo_Bus bus_punto_cargo = new ct_punto_cargo_Bus();
            var lst_punto_cargo = bus_punto_cargo.GetList(model.IdEmpresa, model.IdPunto_cargo_grupo, false,true);
            lst_punto_cargo.Add(new ct_punto_cargo_Info
            {
                IdEmpresa = model.IdEmpresa,
                IdPunto_cargo = 0,
                nom_punto_cargo = "TODOS"
            });
            ViewBag.lst_punto_cargo = lst_punto_cargo;

        }
        private void cargar_grupo_cble()
        {
            var lst_grupo = bus_grupo_cble.get_list(false);
            //lst_grupo.Add(new ct_grupocble_Info
            //{
            //    IdGrupoCble = "",
            //    gc_GrupoCble = "Todos"
            //});
            ViewBag.lst_grupo = lst_grupo;
        }
        private void cargar_sucursal_check(int IdEmpresa, int[] intArray)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            if (intArray == null || intArray.Count() == 0)
            {
                lst_sucursal.Where(q => q.IdSucursal == Convert.ToInt32(SessionFixed.IdSucursal)).FirstOrDefault().Seleccionado = true;
            }
            else
                foreach (var item in lst_sucursal)
                {
                    item.Seleccionado = (intArray.Where(q => q == item.IdSucursal).Count() > 0 ? true : false);
                }
            ViewBag.lst_sucursal = lst_sucursal;
        }
        private void cargar_grupo_check(int IdEmpresa, string[] StringArray)
        {
            ct_grupocble_Bus bus_grupo = new ct_grupocble_Bus();
            var lst_grupo = bus_grupo.get_list(false);
            if (StringArray == null || StringArray.Count() == 0)
            {

            }
            else
                foreach (var item in lst_grupo)
                {
                    item.Seleccionado = (StringArray.Where(q => q == item.IdGrupoCble).Count() > 0 ? true : false);
                }
            ViewBag.lst_grupo = lst_grupo;
        }
        private void cargar_periodo_check(int IdEmpresa, int[] intArray)
        {
            ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
            var lst_periodo = bus_periodo.get_list(IdEmpresa, false);
            if (intArray == null || intArray.Count() == 0)
            {
                //lst_periodo.FirstOrDefault().Seleccionado = true;
            }
            else
                foreach (var item in lst_periodo)
                {
                    item.Seleccionado = (intArray.Where(q => q == item.IdPeriodo).Count() > 0 ? true : false);
                }
            ViewBag.lst_periodo = lst_periodo;
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
        private void cargar_nivel_CONTA006()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
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
            ViewBag.lst_balance = lst_balance;

            ct_anio_fiscal_Bus bus_anio = new ct_anio_fiscal_Bus();
            var lst_anio = bus_anio.get_list(false);
            ViewBag.lst_anio = lst_anio;

            ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
            var lst_periodo = bus_periodo.get_list(IdEmpresa, false);
            ViewBag.lst_periodo = lst_periodo;
        }
        public ActionResult CONTA_002()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdCtaCble = "",
                IdCtaCbleFin = "",
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdTipoCbte = 1,
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) },
                IdPunto_cargo_grupo = 0
            };
            cargar_combo_cbte(model);
            cargar_combos_punto_cargo(model);
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            CONTA_002_Rpt report = new CONTA_002_Rpt();
            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var reporte = bus_rep_x_emp.GetInfo(IdEmpresa, "CONTA_002");
            if (reporte != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, reporte.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCtaCble.Value = model.IdCtaCble;
            report.p_IdCtaCbleFin.Value = model.IdCtaCbleFin;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdPuntoCargo.Value = model.IdPunto_cargo;
            report.p_IdGrupo.Value = model.IdPunto_cargo_grupo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            SessionFixed.IdDivision_IC = "0";
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_002(cl_filtros_Info model)
        {
            CONTA_002_Rpt report = new CONTA_002_Rpt();
            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var reporte = bus_rep_x_emp.GetInfo(IdEmpresa, "CONTA_002");
            if (reporte != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, reporte.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCtaCble.Value = model.IdCtaCble;
            report.p_IdCtaCbleFin.Value = model.IdCtaCbleFin;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.p_IdPuntoCargo.Value = model.IdPunto_cargo;
            report.p_IdGrupo.Value = model.IdPunto_cargo_grupo;
            SessionFixed.IdDivision_IC = model.IdPunto_cargo_grupo.ToString();
            cargar_combo_cbte(model);
            cargar_combos_punto_cargo(model);
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
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
                balance = "ER",
                QuebrarPagina = true,
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) }
            };

            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            model.IdAnio = model.fecha_fin.Year;
            model.MostrarSaldoAcumulado = false;
            CONTA_003_ER_Rpt report = new CONTA_003_ER_Rpt();
            report.IntArray = model.IntArray;
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
            report.p_QuebrarPagina.Value = model.QuebrarPagina;
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
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);

            if (model.balance == "BG")
            {
                CONTA_003_BG_Rpt report = new CONTA_003_BG_Rpt();
                report.IntArray = model.IntArray;
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_IdAnio.Value = model.IdAnio;
                report.p_fechaIni.Value = model.fecha_ini;
                report.p_fechaFin.Value = model.fecha_fin;
                report.p_IdUsuario.Value = SessionFixed.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_IdSucursal.Value = model.IdSucursal;
                report.p_QuebrarPagina.Value = model.QuebrarPagina;
                report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                report.RequestParameters = false;
                ViewBag.Report = report;
            }
            if (model.balance == "ER")
            {
                CONTA_003_ER_Rpt report = new CONTA_003_ER_Rpt();
                report.IntArray = model.IntArray;
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_IdAnio.Value = model.IdAnio;
                report.p_fechaIni.Value = model.fecha_ini;
                report.p_fechaFin.Value = model.fecha_fin;
                report.p_IdUsuario.Value = SessionFixed.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_IdSucursal.Value = model.IdSucursal;
                report.p_QuebrarPagina.Value = model.QuebrarPagina;
                report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                report.RequestParameters = false;
                ViewBag.Report = report;
            }

            if (string.IsNullOrEmpty(model.balance))
            {
                CONTA_003_BC_Rpt report = new CONTA_003_BC_Rpt();
                report.IntArray = model.IntArray;
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
            cargar_nivel();
            return View(model);
        }
        public ActionResult CONTA_004()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdNivel = 6,
                balance = "ER",
                IdPeriodoFin = Convert.ToInt32(DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString("00")),
                IdPeriodoIni = Convert.ToInt32(DateTime.Now.AddMonths(-1).Year.ToString() + DateTime.Now.AddMonths(-1).Month.ToString("00")),
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) }
            };
            
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            model.IdAnio = Convert.ToInt32(model.IdPeriodoFin.ToString().Substring(0,4));
            model.MostrarSaldoAcumulado = false;
            CONTA_004_ER_Rpt report = new CONTA_004_ER_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdPeriodoIni.Value = model.IdPeriodoIni;
            report.p_IdPeriodoFin.Value = model.IdPeriodoFin;
            report.p_IdUsuario.Value = model.IdUsuario;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_balance.Value = model.balance;
            report.p_mostrarAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_nivel_CONTA006();
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_004(cl_filtros_contabilidad_Info model)
        {
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            model.IdAnio = Convert.ToInt32(model.IdPeriodoFin.ToString().Substring(0, 4));
            if (model.balance == "BG")
            {
                CONTA_004_BG_Rpt report = new CONTA_004_BG_Rpt();
                report.IntArray = model.IntArray;
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_IdAnio.Value = model.IdAnio;
                report.p_IdPeriodoIni.Value = model.IdPeriodoIni;
                report.p_IdPeriodoFin.Value = model.IdPeriodoFin;
                report.p_IdUsuario.Value = model.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_mostrarAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                ViewBag.Report = report;
            }
            if (model.balance == "ER")
            {
                CONTA_004_ER_Rpt report = new CONTA_004_ER_Rpt();
                report.IntArray = model.IntArray;
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_IdAnio.Value = model.IdAnio;
                report.p_IdPeriodoIni.Value = model.IdPeriodoIni;
                report.p_IdPeriodoFin.Value = model.IdPeriodoFin;
                report.p_IdUsuario.Value = model.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_mostrarAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                ViewBag.Report = report;
            }
                cargar_nivel_CONTA006();
            return View(model);
        }
        public ActionResult CONTA_005()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) }
            };
            cargar_combos(model.IdEmpresa);
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            CONTA_005_Rpt report = new CONTA_005_Rpt();
            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var reporte = bus_rep_x_emp.GetInfo(IdEmpresa, "CONTA_005");
            if (reporte != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, reporte.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdPunto_cargo_grupo.Value = model.IdPunto_cargo_grupo;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_005(cl_filtros_contabilidad_Info model)
        {
            CONTA_005_Rpt report = new CONTA_005_Rpt();
            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var reporte = bus_rep_x_emp.GetInfo(IdEmpresa, "CONTA_005");
            if (reporte != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, reporte.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdPunto_cargo_grupo.Value = model.IdPunto_cargo_grupo;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            cargar_combos(model.IdEmpresa);
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            ViewBag.Report = report;
            return View(model);
        }
        public ActionResult CONTA_006()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa =  Convert.ToInt32(SessionFixed.IdEmpresa),
                IdNivel = 6,
                balance = "ER",
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) }
            };
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            model.IdAnio = model.fecha_fin.Year;
            model.MostrarSaldoAcumulado = false;
            CONTA_006_ER_Rpt report = new CONTA_006_ER_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = model.IdUsuario;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_balance.Value = model.balance;
            report.p_mostrarAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_nivel_CONTA006();
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_006(cl_filtros_contabilidad_Info model)
        {
            model.IdAnio = model.fecha_fin.Year;
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);

            if (model.balance == "BG")
            {
                CONTA_006_BG_Rpt report = new CONTA_006_BG_Rpt();
                report.IntArray = model.IntArray;
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_FechaIni.Value = model.fecha_ini;
                report.p_FechaFin.Value = model.fecha_fin;
                report.p_IdUsuario.Value = model.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_mostrarAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                ViewBag.Report = report;
            }
            if (model.balance == "ER")
            {
                CONTA_006_ER_Rpt report = new CONTA_006_ER_Rpt();
                report.IntArray = model.IntArray;
                report.p_IdEmpresa.Value = model.IdEmpresa;
                report.p_FechaIni.Value = model.fecha_ini;
                report.p_FechaFin.Value = model.fecha_fin;
                report.p_IdUsuario.Value = model.IdUsuario;
                report.p_IdNivel.Value = model.IdNivel;
                report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
                report.p_balance.Value = model.balance;
                report.p_mostrarAcumulado.Value = model.MostrarSaldoAcumulado;
                report.usuario = SessionFixed.IdUsuario;
                report.empresa = SessionFixed.NomEmpresa;
                ViewBag.Report = report;
            }
            cargar_nivel_CONTA006();
            return View(model);
        }

        public ActionResult CONTA_007()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) },
                MostrarSaldoAcumulado = false,
                MostrarSaldoDetallado = false
            };

            cargar_sucursal_check(model.IdEmpresa, model.IntArray);

            CONTA_007_Rpt report = new CONTA_007_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaini.Value = model.fecha_ini;
            report.p_fechafin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_MostrarAcumulado.Value = model.MostrarSaldoAcumulado;
            report.p_MostrarDetallado.Value = model.MostrarSaldoDetallado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_nivel();
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_007(cl_filtros_contabilidad_Info model)
        {
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);

            CONTA_007_Rpt report = new CONTA_007_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaini.Value = model.fecha_ini;
            report.p_fechafin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_MostrarAcumulado.Value = model.MostrarSaldoAcumulado;
            report.p_MostrarDetallado.Value = model.MostrarSaldoDetallado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            return View(model);
        }
        public ActionResult CONTA_008()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdNivel = 6,
                balance = "ER",
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) },
                IdCentroCosto = ""
            };

            model.IdAnio = model.fecha_fin.Year;
            model.MostrarSaldoAcumulado = false;
            CONTA_008_Rpt report = new CONTA_008_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_nivel();
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_008(cl_filtros_contabilidad_Info model)
        {
            model.IdAnio = model.fecha_fin.Year;


            CONTA_008_Rpt report = new CONTA_008_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_nivel();
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }

        public ActionResult CONTA_009()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdNivel = 6,
                balance = "ER",
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) },
                IdCentroCosto = ""
            };

            model.IdAnio = model.fecha_fin.Year;
            model.MostrarSaldoAcumulado = false;
            CONTA_009_Rpt report = new CONTA_009_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_IdCentroCosto.Value = model.IdCentroCosto;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_nivel();
            cargar_cc(model.IdEmpresa);
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_009(cl_filtros_contabilidad_Info model)
        {
            model.IdAnio = model.fecha_fin.Year;


            CONTA_009_Rpt report = new CONTA_009_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdCentroCosto.Value = model.IdCentroCosto;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_mostrarSaldo0.Value = model.mostrar_saldos_en_0;
            report.p_MostrarSaldoAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_nivel();
            cargar_cc(model.IdEmpresa);
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }

        public ActionResult CONTA_010()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCtaCble = "",
                StringArray = new string[] { },
                mostrar_agrupado = false
            };

            CONTA_010_Rpt report = new CONTA_010_Rpt();
            report.StringArray = model.StringArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaini.Value = model.fecha_ini;
            report.p_fechafin.Value = model.fecha_fin;
            report.p_IdCtaCble.Value = model.IdCtaCble;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_MostrarGrupo.Value = model.mostrar_agrupado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_grupo_check(model.IdEmpresa, model.StringArray);
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_010(cl_filtros_Info model)
        {
            CONTA_010_Rpt report = new CONTA_010_Rpt();
            report.StringArray = model.StringArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaini.Value = model.fecha_ini;
            report.p_fechafin.Value = model.fecha_fin;
            report.p_IdCtaCble.Value = model.IdCtaCble;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_MostrarGrupo.Value = model.mostrar_agrupado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_grupo_check(model.IdEmpresa, model.StringArray);
            return View(model);
        }

        public ActionResult CONTA_011()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) },
            };

            CONTA_011_Rpt report = new CONTA_011_Rpt();
            report.IntArray.Value = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_011(cl_filtros_contabilidad_Info model)
        {
            model.IdAnio = model.fecha_fin.Year;


            CONTA_011_Rpt report = new CONTA_011_Rpt();
            report.IntArray.Value = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;

            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }

        public ActionResult CONTA_012()
        {
            int anio = DateTime.Now.Date.Year;
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IntArray = new int[] { Convert.ToInt32(anio.ToString()+"01"), Convert.ToInt32(anio.ToString() + "02"), Convert.ToInt32(anio.ToString() + "03"), Convert.ToInt32(anio.ToString() + "04"),
                Convert.ToInt32(anio.ToString()+"05"),Convert.ToInt32(anio.ToString()+"06"),Convert.ToInt32(anio.ToString()+"07"),Convert.ToInt32(anio.ToString()+"08"),
                Convert.ToInt32(anio.ToString()+"09"),Convert.ToInt32(anio.ToString()+"10"),Convert.ToInt32(anio.ToString()+"11"),Convert.ToInt32(anio.ToString()+"12")},
            };

            CONTA_012_Rpt report = new CONTA_012_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            cargar_periodo_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_012(cl_filtros_contabilidad_Info model)
        {
            CONTA_012_Rpt report = new CONTA_012_Rpt();
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

            cargar_periodo_check(model.IdEmpresa, model.IntArray);
            return View(model);
        }

        public ActionResult CONTA_013()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdPunto_cargo_grupo = 0
            };

            CONTA_013_Rpt report = new CONTA_013_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaini.Value = model.fecha_ini;
            report.p_fechafin.Value = model.fecha_fin;
            report.p_IdPunto_cargo_grupo.Value = model.IdPunto_cargo_grupo;
            report.p_IdPunto_cargo.Value = model.IdPunto_cargo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            cargar_combos_CONTA013(model.IdEmpresa, model.IdPunto_cargo_grupo);
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_013(cl_filtros_contabilidad_Info model)
        {
            CONTA_013_Rpt report = new CONTA_013_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaini.Value = model.fecha_ini;
            report.p_fechafin.Value = model.fecha_fin;
            report.p_IdPunto_cargo_grupo.Value = model.IdPunto_cargo_grupo;
            report.p_IdPunto_cargo.Value = model.IdPunto_cargo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            cargar_combos_CONTA013(model.IdEmpresa, model.IdPunto_cargo_grupo);
            return View(model);
        }

        #region CONTA_014
        public ActionResult CONTA_014()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) },
                MostrarSaldoAcumulado = false,
                MostrarSaldoDetallado = false
            };
            
            CONTA_014_Rpt report = new CONTA_014_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaDesde.Value = model.fecha_ini;
            report.p_FechaHasta.Value = model.fecha_fin;
            report.p_MostrarAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_014(cl_filtros_contabilidad_Info model)
        {
            CONTA_014_Rpt report = new CONTA_014_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaDesde.Value = model.fecha_ini;
            report.p_FechaHasta.Value = model.fecha_fin;
            report.p_MostrarAcumulado.Value = model.MostrarSaldoAcumulado;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CONTA_015
        public ActionResult CONTA_015()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.AddMonths(-1),
                fecha_fin = DateTime.Now,
            };

            CONTA_015_Rpt report = new CONTA_015_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_015(cl_filtros_contabilidad_Info model)
        {
            CONTA_015_Rpt report = new CONTA_015_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CONTA_016
        public ActionResult CONTA_016()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.AddMonths(-1),
                fecha_fin = DateTime.Now,
            };

            CONTA_016_Rpt report = new CONTA_016_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CONTA_016(cl_filtros_contabilidad_Info model)
        {
            CONTA_016_Rpt report = new CONTA_016_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;
            return View(model);
        }
        #endregion
    }
}
