﻿using Core.Erp.Bus.Caja;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Caja;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Areas.Contabilidad.Controllers;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Core.Erp.Web.Areas.Caja.Controllers
{
    [SessionTimeout]
    public class ConciliacionCajaController : Controller
    {
        #region Variables
        cp_conciliacion_Caja_Bus bus_conciliacion = new cp_conciliacion_Caja_Bus();
        cp_conciliacion_Caja_det_Bus bus_det = new cp_conciliacion_Caja_det_Bus();
        cp_conciliacion_Caja_det_Ing_Caja_Bus bus_ing = new cp_conciliacion_Caja_det_Ing_Caja_Bus();
        cp_conciliacion_Caja_det_x_ValeCaja_Bus bus_vales = new cp_conciliacion_Caja_det_x_ValeCaja_Bus();

        cp_conciliacion_Caja_det_List list_det = new cp_conciliacion_Caja_det_List();
        cp_conciliacion_Caja_det_x_ValeCaja_List list_vale = new cp_conciliacion_Caja_det_x_ValeCaja_List();
        cp_conciliacion_Caja_det_x_ValeCaja_x_Ingresar_List list_vale_x_ingresar = new cp_conciliacion_Caja_det_x_ValeCaja_x_Ingresar_List();
        cp_conciliacion_Caja_det_Ing_Caja_List list_ing = new cp_conciliacion_Caja_det_Ing_Caja_List();
        ct_cbtecble_det_List list_ct = new ct_cbtecble_det_List();  
        
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        caj_Caja_Bus bus_caja = new caj_Caja_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        ct_punto_cargo_Bus bus_pc = new ct_punto_cargo_Bus();
        ct_punto_cargo_grupo_Bus bus_pcg = new ct_punto_cargo_grupo_Bus();
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();

        cp_conciliacion_Caja_det_x_Ingresar_List List_x_Cruzar = new cp_conciliacion_Caja_det_x_Ingresar_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        cp_conciliacion_Caja_List Lista_Conciliacion = new cp_conciliacion_Caja_List();

        string mensaje = string.Empty;
        #endregion

        #region Combobox bajo demanda flujo
        public ActionResult CmbFlujo_ConciliacionCaja()
        {
            decimal model = new decimal();
            return PartialView("_CmbFlujo_ConciliacionCaja", model);
        }
        #endregion

        #region Metodos ComboBox bajo demanda centro de costo

        public ActionResult CmbCentroCosto_ConciliacionCaja()
        {
            string model = string.Empty;
            return PartialView("_CmbCentroCosto_ConciliacionCaja", model);
        }
        public List<ct_CentroCosto_Info> get_list_bajo_demanda_cc(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            List<ct_CentroCosto_Info> Lista = bus_cc.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
            return Lista;
        }
        public ct_CentroCosto_Info get_info_bajo_demanda_cc(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_cc.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos ComboBox bajo demanda punto de cargo        
        public ActionResult CmbPuntoCargoConciliacion()
        {
            string model = string.Empty;
            return PartialView("_CmbPuntoCargoConciliacion", model);
        }
        public List<ct_punto_cargo_Info> get_list_bajo_demandaPC(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            int IdPunto_cargo_grupo = 0;
            List<ct_punto_cargo_Info> Lista = bus_pc.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), IdPunto_cargo_grupo);
            return Lista;
        }
        public ct_punto_cargo_Info get_info_bajo_demandaPC(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_pc.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos ComboBox bajo demanda
        public ActionResult CmbPersona_ConciliacionCaja()
        {
            SessionFixed.TipoPersona = Request.Params["IdTipoPersona"] != null ? Request.Params["IdTipoPersona"].ToString() : "PERSONA";
            decimal model = new decimal();
            return PartialView("_CmbPersona_ConciliacionCaja", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.TipoPersona);
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.TipoPersona);
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "ConciliacionCaja", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cl_filtros_caja_Info model = new cl_filtros_caja_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdCaja = string.IsNullOrEmpty(SessionFixed.IdCaja) ? 0 : Convert.ToInt32(SessionFixed.IdCaja),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };
            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_conciliacion.get_list(model.IdEmpresa, model.IdCaja, model.fecha_ini, model.fecha_fin);
            Lista_Conciliacion.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_caja_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "ConciliacionCaja", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();

            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_conciliacion.get_list(model.IdEmpresa, model.IdCaja, model.fecha_ini, model.fecha_fin);
            Lista_Conciliacion.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_conciliacion_caja(bool Nuevo=false)
        {
            //ViewBag.fecha_ini = fecha_ini;
            //ViewBag.fecha_fin = fecha_fin;
            //ViewBag.IdEmpresa = IdEmpresa;
            //ViewBag.idCaja = IdCaja;
            //var model = bus_conciliacion.get_list(IdEmpresa, IdCaja, fecha_ini, fecha_fin);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Conciliacion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_conciliacion_caja", model);
        }
        public void CargarCombosConsulta(int IdEmpresa)
        {
            var lst_caja = bus_caja.get_list(IdEmpresa, false);
            lst_caja.Add(new caj_Caja_Info
            {
                IdCaja = 0,
                ca_Descripcion = "Todos"
            });
            ViewBag.lst_caja = lst_caja;
        }
        #endregion

        #region Acciones        
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "ConciliacionCaja", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),

                IdEmpresa = IdEmpresa,
                Fecha = DateTime.Now.Date,
                IdPeriodo = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMM")),
                Fecha_ini = DateTime.Now.Date,
                Fecha_fin = DateTime.Now.Date,
                FechaOP = DateTime.Now,
                IdEstadoCierre = cl_enumeradores.eEstadoCierreCaja.EST_CIE_ABI.ToString(),
                lst_det_fact = new List<cp_conciliacion_Caja_det_Info>(),
                lst_det_ing = new List<cp_conciliacion_Caja_det_Ing_Caja_Info>(),
                lst_det_vale = new List<cp_conciliacion_Caja_det_x_ValeCaja_Info>(),
                lst_det_ct = new List<ct_cbtecble_det_Info>(),                
                IdCaja = string.IsNullOrEmpty(SessionFixed.IdCaja) ? 0 : Convert.ToInt32(SessionFixed.IdCaja)
            };            

            list_det.set_list(model.lst_det_fact, model.IdTransaccionSession);
            list_vale.set_list(model.lst_det_vale,model.IdTransaccionSession);
            list_ing.set_list(model.lst_det_ing,model.IdTransaccionSession);
            list_ct.set_list(model.lst_det_ct,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cp_conciliacion_Caja_Info model)
        {
            
            if (!validar(model,ref mensaje))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_conciliacion.guardarDB(model))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdConciliacion_caja = model.IdConciliacion_Caja, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdConciliacion_caja = 0, bool Exito=false)
        {
            cp_conciliacion_Caja_Info model = bus_conciliacion.get_info(IdEmpresa, IdConciliacion_caja);
            if (model.FechaOP.Year == 1)
                model.FechaOP = DateTime.Now;
            if (model == null)
                return RedirectToAction("Index");

            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "ConciliacionCaja", "Index");
            if (model.IdEstadoCierre != "EST_CIE_ABI")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_det_fact = bus_det.get_list(model.IdEmpresa, model.IdConciliacion_Caja);
            list_det.set_list(model.lst_det_fact, model.IdTransaccionSession);
            model.lst_det_vale = bus_vales.get_list(model.IdEmpresa, model.IdConciliacion_Caja);
            list_vale.set_list(model.lst_det_vale, model.IdTransaccionSession);
            model.lst_det_ing = bus_ing.get_list_ingresos_x_conciliar(IdEmpresa, model.Fecha_fin, model.IdCaja);
            list_ing.set_list(model.lst_det_ing, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.CAJA, 0, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 ,decimal IdConciliacion_caja = 0)
        {
            cp_conciliacion_Caja_Info model = bus_conciliacion.get_info(IdEmpresa, IdConciliacion_caja);
            if (model.FechaOP.Year == 1)
                model.FechaOP = DateTime.Now;
            if(model == null)
                return RedirectToAction("Index");

            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "ConciliacionCaja", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_det_fact = bus_det.get_list(model.IdEmpresa, model.IdConciliacion_Caja);
            list_det.set_list(model.lst_det_fact, model.IdTransaccionSession);
            model.lst_det_vale = bus_vales.get_list(model.IdEmpresa, model.IdConciliacion_Caja);
            list_vale.set_list(model.lst_det_vale, model.IdTransaccionSession);
            model.lst_det_ing = bus_ing.get_list_ingresos_x_conciliar(IdEmpresa, model.Fecha_fin, model.IdCaja);
            list_ing.set_list(model.lst_det_ing, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.CAJA, 0, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cp_conciliacion_Caja_Info model)
        {

            if (!validar(model, ref mensaje))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            if (!bus_conciliacion.modificarDB(model))
            {
                ViewBag.mensaje = "No se ha podido modificar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdConciliacion_caja = model.IdConciliacion_Caja, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdConciliacion_caja = 0)
        {
            cp_conciliacion_Caja_Info model = bus_conciliacion.get_info(IdEmpresa, IdConciliacion_caja);
            if (model.FechaOP.Year == 1)
                model.FechaOP = DateTime.Now;
            if (model == null)
                return RedirectToAction("Index");

            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "ConciliacionCaja", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_det_fact = bus_det.get_list(model.IdEmpresa, model.IdConciliacion_Caja);
            list_det.set_list(model.lst_det_fact, model.IdTransaccionSession);
            model.lst_det_vale = bus_vales.get_list(model.IdEmpresa, model.IdConciliacion_Caja);
            list_vale.set_list(model.lst_det_vale, model.IdTransaccionSession);
            model.lst_det_ing = bus_ing.get_list_ingresos_x_conciliar(IdEmpresa, model.Fecha_fin, model.IdCaja);
            list_ing.set_list(model.lst_det_ing, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.CAJA, 0, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cp_conciliacion_Caja_Info model)
        {

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            if (!bus_conciliacion.anularDB(model))
            {
                ViewBag.mensaje = "No se ha podido anular el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            var lst_periodo = bus_periodo.get_list(IdEmpresa, false);
            ViewBag.lst_periodo = lst_periodo;

            var lst_caja = bus_caja.get_list(IdEmpresa, false);
            ViewBag.lst_caja = lst_caja;

            Dictionary<string, string> lst = new Dictionary<string, string>();
            lst.Add(cl_enumeradores.eEstadoCierreCaja.EST_CIE_ABI.ToString(), "ABIERTA");
            lst.Add(cl_enumeradores.eEstadoCierreCaja.EST_CIE_CER.ToString(), "CERRADA");
            ViewBag.lst_estado = lst;

            Dictionary<string, string> lst_tipo_personas = new Dictionary<string, string>();
            lst_tipo_personas.Add(cl_enumeradores.eTipoPersona.PERSONA.ToString(), "Persona");
            lst_tipo_personas.Add(cl_enumeradores.eTipoPersona.PROVEE.ToString(), "Proveedor");
            lst_tipo_personas.Add(cl_enumeradores.eTipoPersona.EMPLEA.ToString(), "Empleado");
            lst_tipo_personas.Add(cl_enumeradores.eTipoPersona.CLIENTE.ToString(), "Cliente");
            ViewBag.lst_tipo_personas = lst_tipo_personas;
        }

        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_punto_cargo_grupo = bus_pcg.GetList(IdEmpresa, false);
            ViewBag.lst_punto_cargo_grupo = lst_punto_cargo_grupo;
        }
        
        private bool validar(cp_conciliacion_Caja_Info i_validar, ref string msg)
        {
            i_validar.IdUsuario = SessionFixed.IdUsuario;
            i_validar.IdUsuarioModificacion = SessionFixed.IdUsuario;


            i_validar.lst_det_ct = list_ct.get_list(i_validar.IdTransaccionSession);

            i_validar.lst_det_ing = list_ing.get_list(i_validar.IdTransaccionSession);
            i_validar.lst_det_vale = list_vale.get_list(i_validar.IdTransaccionSession);
            i_validar.lst_det_fact = list_det.get_list(i_validar.IdTransaccionSession);

            var ingresos = i_validar.lst_det_ing.Sum(q => q.valor_disponible);
            var egresos = Convert.ToDouble(i_validar.lst_det_fact.Count == 0 ? 0 : i_validar.lst_det_fact.Sum(q => q.Valor_a_aplicar)) + Convert.ToDouble(i_validar.lst_det_vale.Count == 0 ? 0 : i_validar.lst_det_vale.Sum(q => q.valor));

            i_validar.Ingresos = Math.Round(ingresos, 2, MidpointRounding.AwayFromZero);
            i_validar.Total_Ing = Math.Round(Math.Abs(i_validar.Saldo_cont_al_periodo) - ingresos, 2, MidpointRounding.AwayFromZero);
            i_validar.Total_fondo = Math.Round(ingresos, 2, MidpointRounding.AwayFromZero);
            i_validar.Total_fact_vale = Math.Round(egresos, 2, MidpointRounding.AwayFromZero);
            i_validar.Dif_x_pagar_o_cobrar = Math.Round(ingresos - egresos, 2, MidpointRounding.AwayFromZero);

            if (i_validar.lst_det_vale.Count() == 0 && i_validar.lst_det_fact.Count()==0)
            {
                msg = "Debe ingresar registros de facturas o vales";
                return false;
            }

            if (i_validar.IdEstadoCierre == cl_enumeradores.eEstadoCierreCaja.EST_CIE_CER.ToString())
            {
                if (i_validar.IdEntidad == 0)
                {
                    msg = "Seleccione el beneficiario";
                    return false;
                }
                if (i_validar.ValorOP == 0)
                {
                    msg = "El valor de la orden de pago no puede ser 0";
                    return false;
                }
                if (i_validar.ValorReposicion == 0)
                {
                    msg = "El valor de la reposición de caja no puede ser 0";
                    return false;
                }
                var persona = bus_persona.get_info(i_validar.IdEmpresa, i_validar.IdTipoPersona, (decimal)i_validar.IdEntidad);
                if (persona == null)
                {
                    msg = "La persona seleccionada no corresponde al tipo asignado";
                    return false;
                }
                i_validar.IdPersona = persona.IdPersona;

                foreach (var item in i_validar.lst_det_fact)
                {
                    item.IdCtaCble_cxp = bus_plancta.get_CtaCble_acreedora(item.IdEmpresa_OGiro, item.IdTipoCbte_Ogiro, item.IdCbteCble_Ogiro);
                    if (string.IsNullOrEmpty(item.IdCtaCble_cxp))
                    {
                        msg = "La cuenta contable de la "+item.Tipo_documento+"# "+item.co_factura+"No se encuentra registrada para el proveedor";
                        return false;
                    }
                }
            }
            
            return true;
        }
        #endregion

        #region Vales
        [ValidateInput(false)]
        public ActionResult GridViewPartial_conciliacion_caja_movimiento()
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //int IdCaja = Request.Params["IdCajaConciliacion"] != null ? Convert.ToInt32(Request.Params["IdCajaConciliacion"]) : 0;

            //var model = bus_det.get_list_x_movimientos_caja(IdEmpresa, IdCaja);
            //list_vale_x_ingresar.set_list(model, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = list_vale_x_ingresar.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_conciliacion_caja_movimiento", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_conciliacion_vales()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_vale = list_vale.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_conciliacion_vales", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNewVale([ModelBinder(typeof(DevExpressEditorsBinder))] cp_conciliacion_Caja_det_x_ValeCaja_Info info_det)
        {
            if (ModelState.IsValid)
                list_vale.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_vale = list_vale.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_conciliacion_vales", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdateVale([ModelBinder(typeof(DevExpressEditorsBinder))] cp_conciliacion_Caja_det_x_ValeCaja_Info info_det)
        {
            if (ModelState.IsValid)
                list_vale.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_vale = list_vale.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_conciliacion_vales", model);
        }

        public ActionResult EditingDeleteVale(int secuencia)
        {
            list_vale.DeleteRow(secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_vale = list_vale.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_conciliacion_vales", model);
        }

        public ActionResult CargarPuntoCargoConciliacion()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            int IdPunto_cargo_grupo = (Request.Params["fx_IdPunto_cargo_grupo_vales"] != null && Request.Params["fx_IdPunto_cargo_grupo_vales"]!="") ? Convert.ToInt32(Request.Params["fx_IdPunto_cargo_grupo_vales"]) : 0;
            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = "nom_punto_cargo";
                p.ValueField = "IdPunto_cargo";
                p.Columns.Add("IdPunto_cargo", "ID", 10);
                p.Columns.Add("nom_punto_cargo", "Punto de cargo", 90);
                p.ClientSideEvents.BeginCallback = "PuntoCargoValesComboBox_BeginCallback";
                p.ValueType = typeof(int);
                p.BindList(bus_pc.GetList(IdEmpresa, IdPunto_cargo_grupo, false, false));
            });
        }
        #endregion

        public ActionResult ComboBoxPartial_persona(decimal IdPersona = 0)
       {
            var model = bus_persona.get_info(IdPersona);
            if (model == null) model = new Info.General.tb_persona_Info();
            return PartialView("_ComboBoxPartial_persona", model);
        }

        #region Facturas
        [ValidateInput(false)]
        public ActionResult GridViewPartial_conciliacion_facturas_x_cruzar()
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //int IdCaja = Request.Params["IdCajaConciliacion"] != null ? Convert.ToInt32(Request.Params["IdCajaConciliacion"]) : 0;
            //List<cp_conciliacion_Caja_det_Info> model;
            //if (IdCaja != 0)
            //{
            //    var Caja = bus_caja.get_info(IdEmpresa, IdCaja);
            //    model = bus_det.get_list_x_pagar(IdEmpresa, Caja.IdSucursal);
            //}
            //else
            //    model = new List<cp_conciliacion_Caja_det_Info>();

            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_x_Cruzar.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_conciliacion_facturas_x_cruzar", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_conciliacion_facturas()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_fact = list_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_conciliacion_facturas", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNewFactura(string IDs = "", decimal IdTransaccionFixed = 0, int IdCaja = 0, int IdEmpresa = 0)
        {
            if (IDs != "")
            {
                var caja = bus_caja.get_info(IdEmpresa, IdCaja);
                //var lst_x_cruzar = bus_det.get_list_x_pagar(IdEmpresa,caja.IdSucursal);
                var lst_x_cruzar = List_x_Cruzar.get_list(IdTransaccionFixed);
                string[] array = IDs.Split(',');
                foreach (var item in array)
                {
                    var info_det = lst_x_cruzar.Where(q => q.IdSecuencia == item).FirstOrDefault();
                    if(info_det != null)
                        list_det.AddRow(info_det, IdTransaccionFixed);
                }
            }
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_fact = list_det.get_list(IdTransaccionFixed);
            return PartialView("_GridViewPartial_conciliacion_facturas", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdateFactura([ModelBinder(typeof(DevExpressEditorsBinder))] cp_conciliacion_Caja_det_Info info_det)
        {
            if (ModelState.IsValid)
                list_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_fact = list_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_conciliacion_facturas", model);
        }

        public ActionResult EditingDeleteFactura(int secuencia)
        {
            list_det.DeleteRow(secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_fact = list_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_conciliacion_facturas", model);
        }
        #endregion

        #region Json
        public JsonResult GetSaldoAnterior(DateTime? FechaCorte, string IdCtaCble = "")
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            double resultado = 0;
            if(FechaCorte != null)
                resultado = bus_plancta.get_saldo_anterior(IdEmpresa, IdCtaCble, Convert.ToDateTime(FechaCorte));

            return Json(Math.Round(resultado,2,MidpointRounding.AwayFromZero), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIdCtaCbleCaja(int IdCaja = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            string resultado = string.Empty;
            if (IdCaja != 0)
                resultado = bus_caja.get_IdCtaCble(IdEmpresa, IdCaja);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPeriodo(int IdPeriodo = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var resultado = new ct_periodo_Info();
            var resultado = bus_periodo.get_info(IdEmpresa, IdPeriodo);

            resultado.IdEmpresa = IdEmpresa;
            //resultado.pe_FechaIni = Inicio;
            //resultado.pe_FechaFin = Fin;
            if (resultado == null)
                resultado = new ct_periodo_Info();

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Calcular(double SaldoContableAnterior = 0, decimal IdTransaccionFixed = 0)
        {
            var lst_ing = list_ing.get_list(IdTransaccionFixed);
            var lst_vale = list_vale.get_list(IdTransaccionFixed);
            var lst_fact = list_det.get_list(IdTransaccionFixed);

            var ingresos = lst_ing.Sum(q => q.valor_disponible);
            var egresos = Convert.ToDouble(lst_fact.Count == 0 ? 0 : lst_fact.Sum(q => q.Valor_a_aplicar)) + Convert.ToDouble(lst_vale.Count == 0 ? 0 : lst_vale.Sum(q => q.valor));
            var resultado = new cp_conciliacion_valores
            {
                Ingresos = Math.Round(ingresos, 2, MidpointRounding.AwayFromZero),
                Dif_ingresos = Math.Round(Math.Abs(SaldoContableAnterior) - ingresos, 2, MidpointRounding.AwayFromZero),
                Fondo = Math.Round(ingresos, 2, MidpointRounding.AwayFromZero),
                Total_fact_vales = Math.Round(egresos, 2, MidpointRounding.AwayFromZero),
                Diferencia = Math.Round(ingresos - egresos, 2, MidpointRounding.AwayFromZero)
            };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetValorCierre(decimal IdTransaccionSession = 0)
        {
            var lst_vale = list_vale.get_list(IdTransaccionSession);
            var lst_fact = list_det.get_list(IdTransaccionSession);
            var valor = Convert.ToDouble(lst_fact.Count == 0 ? 0 : lst_fact.Sum(q => q.Valor_a_aplicar)) + Convert.ToDouble(lst_vale.Count == 0 ? 0 : lst_vale.Sum(q => q.valor));
            return Json(Math.Round(valor,2,MidpointRounding.AwayFromZero),JsonRequestBehavior.AllowGet);
        }

        public JsonResult AgregarCajaMovimiento(string Ids = "", decimal IdTransaccionSession = 0)
        {
            if (Ids != null)
            {
                var lst = list_vale_x_ingresar.get_list(IdTransaccionSession);
               
                string[] array = Ids.Split(',');
                //var output = array.GroupBy(q => q).ToList();
                foreach (var item in array)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var info_add = lst.Where(q => q.IdCbteCble_movcaja == Convert.ToDecimal(item) ).FirstOrDefault();
                        list_vale.AddRow(info_add, IdTransaccionSession);
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListFacturas_PorIngresar(decimal IdTransaccionSession = 0, int IdEmpresa = 0, int IdCaja = 0)
        {
            List<cp_conciliacion_Caja_det_Info> model = new List<cp_conciliacion_Caja_det_Info>();
            if (IdCaja != 0)
            {
                var Caja = bus_caja.get_info(IdEmpresa, IdCaja);
                model = bus_det.get_list_x_pagar(IdEmpresa, Caja.IdSucursal);
            }
            else
                model = new List<cp_conciliacion_Caja_det_Info>();

            List_x_Cruzar.set_list(model, IdTransaccionSession);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListVales_PorIngresar(decimal IdTransaccionSession = 0, int IdEmpresa = 0, int IdCaja = 0)
        {
            var model = bus_det.get_list_x_movimientos_caja(IdEmpresa, IdCaja);
            list_vale_x_ingresar.set_list(model, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ingresos
        [ValidateInput(false)]
        public ActionResult GridViewPartial_conciliacion_ingresos()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            cp_conciliacion_Caja_Info model = new cp_conciliacion_Caja_Info();
            model.lst_det_ing = list_ing.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_conciliacion_ingresos", model); 
        }
        public void GetIngresos(DateTime? FechaFin, int IdCaja = 0, decimal IdTransaccionFixed = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst = bus_ing.get_list_ingresos_x_conciliar(IdEmpresa, FechaFin == null ? DateTime.Now.Date : Convert.ToDateTime(FechaFin), IdCaja);
            list_ing.set_list(lst, IdTransaccionFixed);
        }
        #endregion        
    }

    public class cp_conciliacion_valores
    {
        public double Ingresos { get; set; }
        public double Dif_ingresos { get; set; }
        public double Fondo { get; set; }
        public double Total_fact_vales { get; set; }
        public double Diferencia { get; set; }
    }

    public class cp_conciliacion_Caja_List
    {
        string Variable = "cp_conciliacion_Caja_Info";
        public List<cp_conciliacion_Caja_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<cp_conciliacion_Caja_Info> list = new List<cp_conciliacion_Caja_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<cp_conciliacion_Caja_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<cp_conciliacion_Caja_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }

    public class cp_conciliacion_Caja_det_List
    {
        public List<cp_conciliacion_Caja_det_Info> get_list(decimal IdTransaccionSession )
        {
            if (HttpContext.Current.Session["cp_conciliacion_Caja_det_Info"+IdTransaccionSession.ToString()] == null)
            {
                List<cp_conciliacion_Caja_det_Info> list = new List<cp_conciliacion_Caja_det_Info>();

                HttpContext.Current.Session["cp_conciliacion_Caja_det_Info"+IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_conciliacion_Caja_det_Info>)HttpContext.Current.Session["cp_conciliacion_Caja_det_Info"+IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_conciliacion_Caja_det_Info> list, decimal IdTransaccionSession )
        {
            HttpContext.Current.Session["cp_conciliacion_Caja_det_Info" + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(cp_conciliacion_Caja_det_Info info_det, decimal IdTransaccionSession)
        {
            List<cp_conciliacion_Caja_det_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            if (list.Where(q=>q.IdEmpresa_OGiro == info_det.IdEmpresa_OGiro && q.IdTipoCbte_Ogiro == info_det.IdTipoCbte_Ogiro && q.IdCbteCble_Ogiro == info_det.IdCbteCble_Ogiro).Count() == 0)
                list.Add(info_det);
        }

        public void UpdateRow(cp_conciliacion_Caja_det_Info info_det, decimal IdTransaccionSession)
        {
            cp_conciliacion_Caja_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.Valor_a_aplicar = info_det.Valor_a_aplicar;
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<cp_conciliacion_Caja_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).First());
        }
    }

    public class cp_conciliacion_Caja_det_Ing_Caja_List
    {
        public List<cp_conciliacion_Caja_det_Ing_Caja_Info> get_list(decimal IdTransaccionSession )
        {
            if (HttpContext.Current.Session["cp_conciliacion_Caja_det_Ing_Caja_Info"+IdTransaccionSession.ToString()] == null)
            {
                List<cp_conciliacion_Caja_det_Ing_Caja_Info> list = new List<cp_conciliacion_Caja_det_Ing_Caja_Info>();

                HttpContext.Current.Session["cp_conciliacion_Caja_det_Ing_Caja_Info" + IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_conciliacion_Caja_det_Ing_Caja_Info>)HttpContext.Current.Session["cp_conciliacion_Caja_det_Ing_Caja_Info" + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_conciliacion_Caja_det_Ing_Caja_Info> list, decimal IdTransaccionSession )
        {
            HttpContext.Current.Session["cp_conciliacion_Caja_det_Ing_Caja_Info"+IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(cp_conciliacion_Caja_det_Ing_Caja_Info info_det, decimal IdTransaccionSession)
        {
            List<cp_conciliacion_Caja_det_Ing_Caja_Info> list = get_list(IdTransaccionSession);
            info_det.secuencia = list.Count == 0 ? 1 : list.Max(q => q.secuencia) + 1;
            list.Add(info_det);
        }

        public void UpdateRow(cp_conciliacion_Caja_det_Ing_Caja_Info info_det, decimal IdTransaccionSession)
        {
            cp_conciliacion_Caja_det_Ing_Caja_Info edited_info = get_list(IdTransaccionSession).Where(m => m.secuencia == info_det.secuencia).First();
        }

        public void DeleteRow(int secuencia, decimal IdTransaccionSession)
        {
            List<cp_conciliacion_Caja_det_Ing_Caja_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.secuencia == secuencia).First());
        }
    }

    public class cp_conciliacion_Caja_det_x_ValeCaja_List
    {
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        caj_Caja_Movimiento_Bus bus_mov = new caj_Caja_Movimiento_Bus();
        ct_punto_cargo_Bus bus_pc = new ct_punto_cargo_Bus();
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();

        caj_Caja_Movimiento_Tipo_Bus bus_tipo_movi = new caj_Caja_Movimiento_Tipo_Bus();
        public List<cp_conciliacion_Caja_det_x_ValeCaja_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_Info"+IdTransaccionSession.ToString()] == null)
            {
                List<cp_conciliacion_Caja_det_x_ValeCaja_Info> list = new List<cp_conciliacion_Caja_det_x_ValeCaja_Info>();

                HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_Info"+IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_conciliacion_Caja_det_x_ValeCaja_Info>)HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_Info"+IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_conciliacion_Caja_det_x_ValeCaja_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_Info"+IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(cp_conciliacion_Caja_det_x_ValeCaja_Info info_det, decimal IdTransaccionSession)
        {
            List<cp_conciliacion_Caja_det_x_ValeCaja_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            var per = bus_persona.get_info(info_det.IdPersona);
            if (per != null)
                info_det.pe_nombreCompleto = per.pe_nombreCompleto;

            var tipo = bus_tipo_movi.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(info_det.idTipoMovi));
            if (tipo != null)
                info_det.IdCtaCble = tipo.IdCtaCble;

            if (info_det.IdPunto_cargo_vales == null || info_det.IdPunto_cargo_vales == 0)
                info_det.nom_punto_cargo = string.Empty;
            else
            {
                var pc = bus_pc.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(info_det.IdPunto_cargo_vales));
                if (pc != null)
                {
                    info_det.nom_punto_cargo = pc.nom_punto_cargo;
                }
            }

            #region Centro de costo
            info_det.IdCentroCosto_vales = info_det.IdCentroCosto_vales;
            if (string.IsNullOrEmpty(info_det.IdCentroCosto_vales))
                info_det.cc_Descripcion = string.Empty;
            else
            {
                var cc = bus_cc.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), info_det.IdCentroCosto_vales);
                if (cc != null)
                {
                    info_det.cc_Descripcion = cc.cc_Descripcion;
                }
            }
            #endregion

            list.Add(info_det);
        }

        public void UpdateRow(cp_conciliacion_Caja_det_x_ValeCaja_Info info_det, decimal IdTransaccionSession)
        {
            cp_conciliacion_Caja_det_x_ValeCaja_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.valor = info_det.valor;
            edited_info.IdPersona = info_det.IdPersona;
            if (edited_info.IdPersona != info_det.IdPersona)
            {
                var per = bus_persona.get_info(info_det.IdPersona);
                if (per != null)
                    edited_info.pe_nombreCompleto = per.pe_nombreCompleto;
            }
            if (edited_info.idTipoMovi != info_det.idTipoMovi)
            {
                var tipo = bus_tipo_movi.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(info_det.idTipoMovi));
                if (tipo != null)
                    info_det.IdCtaCble = tipo.IdCtaCble;
            }
            edited_info.Observacion = info_det.Observacion;
            edited_info.idTipoMovi = info_det.idTipoMovi;
            edited_info.fecha = info_det.fecha;
            edited_info.se_modifico = true;

            #region Centro de costo
            edited_info.IdCentroCosto_vales = info_det.IdCentroCosto_vales;
            if (string.IsNullOrEmpty(info_det.IdCentroCosto_vales))
                edited_info.cc_Descripcion = string.Empty;
            else
            {
                var cc = bus_cc.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), info_det.IdCentroCosto_vales);
                if (cc != null)
                {
                    edited_info.cc_Descripcion = cc.cc_Descripcion;
                }
            }
            #endregion

            #region Punto Cargo
            edited_info.IdPunto_cargo_vales = info_det.IdPunto_cargo_vales;
            edited_info.IdPunto_cargo_grupo_vales = info_det.IdPunto_cargo_grupo_vales;
            if (info_det.IdPunto_cargo_vales == null || info_det.IdPunto_cargo_vales == 0)
                edited_info.nom_punto_cargo = string.Empty;
            else
            {
                var pc = bus_pc.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(info_det.IdPunto_cargo_vales));
                if (pc != null)
                {
                    edited_info.nom_punto_cargo = pc.nom_punto_cargo;
                }
            }
            #endregion
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<cp_conciliacion_Caja_det_x_ValeCaja_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).First());
        }
    }

    public class cp_conciliacion_Caja_det_x_ValeCaja_x_Ingresar_List
    {
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        caj_Caja_Movimiento_Bus bus_mov = new caj_Caja_Movimiento_Bus();

        caj_Caja_Movimiento_Tipo_Bus bus_tipo_movi = new caj_Caja_Movimiento_Tipo_Bus();
        public List<cp_conciliacion_Caja_det_x_ValeCaja_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_x_Ingresar_List" + IdTransaccionSession.ToString()] == null)
            {
                List<cp_conciliacion_Caja_det_x_ValeCaja_Info> list = new List<cp_conciliacion_Caja_det_x_ValeCaja_Info>();

                HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_x_Ingresar_List" + IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_conciliacion_Caja_det_x_ValeCaja_Info>)HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_x_Ingresar_List" + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_conciliacion_Caja_det_x_ValeCaja_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session["cp_conciliacion_Caja_det_x_ValeCaja_x_Ingresar_List" + IdTransaccionSession.ToString()] = list;
        }
    }

    public class cp_conciliacion_Caja_det_x_Ingresar_List
    {
        public List<cp_conciliacion_Caja_det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session["cp_conciliacion_Caja_det_x_Ingresar_Info" + IdTransaccionSession.ToString()] == null)
            {
                List<cp_conciliacion_Caja_det_Info> list = new List<cp_conciliacion_Caja_det_Info>();

                HttpContext.Current.Session["cp_conciliacion_Caja_det_x_Ingresar_Info" + IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_conciliacion_Caja_det_Info>)HttpContext.Current.Session["cp_conciliacion_Caja_det_x_Ingresar_Info" + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_conciliacion_Caja_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session["cp_conciliacion_Caja_det_x_Ingresar_Info" + IdTransaccionSession.ToString()] = list;
        }
    }
}