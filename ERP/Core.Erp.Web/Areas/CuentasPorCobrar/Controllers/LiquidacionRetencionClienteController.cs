using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.General;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Areas.Contabilidad.Controllers;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorCobrar.Controllers
{
    public class LiquidacionRetencionClienteController : Controller
    {
        #region Variables
        cxc_LiquidacionRetProv_Bus bus_liquidacion = new cxc_LiquidacionRetProv_Bus();
        cxc_LiquidacionRetProvDet_Bus bus_liquidacion_det = new cxc_LiquidacionRetProvDet_Bus();
        ct_cbtecble_tipo_Bus bus_tipo_cbte = new ct_cbtecble_tipo_Bus();
        cxc_LiquidacionRetProvDet_List ListaDetalle = new cxc_LiquidacionRetProvDet_List();
        cxc_LiquidacionRetProvDet_XCruzar_List Lista_XCruzar = new cxc_LiquidacionRetProvDet_XCruzar_List();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        cxc_Parametro_Bus bus_parametro = new cxc_Parametro_Bus();
        ct_cbtecble_det_List list_ct_cbtecble_det = new ct_cbtecble_det_List();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        ct_cbtecble_det_Bus bus_cbte_det = new ct_cbtecble_det_Bus();
        string mensaje = string.Empty;
        #endregion

        #region Index
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
            };
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            cargar_combos();
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_LiquidacionRetencionCliente(DateTime? Fecha_ini, DateTime? Fecha_fin, int IdSucursal=0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.IdSucursal = IdSucursal == 0 ? Convert.ToInt32(SessionFixed.IdSucursal) : IdSucursal;
            ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);

            var model = bus_liquidacion.get_list(IdEmpresa, IdSucursal, ViewBag.Fecha_ini, ViewBag.Fecha_fin);

            return PartialView("_GridViewPartial_LiquidacionRetencionCliente", model);
        }
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_sucursal = bus_sucursal.GetList( IdEmpresa, Convert.ToString(SessionFixed.IdUsuario), false);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        private bool validar(cxc_LiquidacionRetProv_Info i_validar, ref string msg)
        {
            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.li_Fecha, cl_enumeradores.eModulo.CXC, i_validar.IdSucursal, ref mensaje))
            {
                return false;
            }

            if (i_validar.lst_detalle.Count == 0)
            {
                mensaje = "Debe ingresar registros en el detalle";
                return false;
            }

            if (i_validar.lst_detalle_cbte.Count == 0)
            {
                mensaje = "Debe ingresar registros en el detalle del diario";
                return false;
            }

            if (Math.Round(i_validar.lst_detalle_cbte.Sum(q => q.dc_Valor), 2) != 0)
            {
                mensaje = "La suma de los detalles debe ser 0";
                return false;
            }

            if (i_validar.lst_detalle_cbte.Where(q => q.dc_Valor == 0).Count() > 0)
            {
                mensaje = "Existen detalles con valor 0 en el debe o haber";
                return false;
            }

            if (i_validar.lst_detalle_cbte.Where(q => string.IsNullOrEmpty(q.IdCtaCble)).Count() > 0)
            {
                mensaje = "Existen detalles sin cuenta contable";
                return false;
            }

            return true;
        }
        #endregion

        #region acciones
        public ActionResult Nuevo(int IdEmpresa = 0, int IdSucursal=0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_LiquidacionRetProv_Info model = new cxc_LiquidacionRetProv_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                li_Fecha = DateTime.Now,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            model.lst_detalle_cbte = new List<ct_cbtecble_det_Info>();
            ListaDetalle.set_list(model.lst_detalle, model.IdTransaccionSession);
            list_ct_cbtecble_det.set_list(model.lst_detalle_cbte, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.li_Fecha, cl_enumeradores.eModulo.FAC, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cxc_LiquidacionRetProv_Info model)
        {
            model.lst_detalle = ListaDetalle.get_list(model.IdTransaccionSession);
            model.lst_detalle_cbte = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;

            if (!validar(model, ref mensaje))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos();
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                return View(model);
            }

            if (!bus_liquidacion.guardarDB(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos();
                ViewBag.MostrarBoton = true;
                return View(model);
            };

            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdSucursal=model.IdSucursal, IdLiquidacion = model.IdLiquidacion, Exito = true });
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdSucursal = 0, decimal IdLiquidacion = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_LiquidacionRetProv_Info model = bus_liquidacion.get_info(IdEmpresa, IdSucursal, IdLiquidacion);
            if (model == null)
                return RedirectToAction("Index");

            model.lst_detalle = bus_liquidacion_det.GetList(IdEmpresa, IdSucursal, IdLiquidacion);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_detalle_cbte = bus_cbte_det.get_list(model.IdEmpresa, Convert.ToInt32(model.IdTipoCbte), Convert.ToInt32(model.IdCbteCble));
            ListaDetalle.set_list(model.lst_detalle, model.IdTransaccionSession);
            list_ct_cbtecble_det.set_list(model.lst_detalle_cbte, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.li_Fecha, cl_enumeradores.eModulo.FAC, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                cargar_combos();
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cxc_LiquidacionRetProv_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario.ToString();
            model.lst_detalle = ListaDetalle.get_list(model.IdTransaccionSession);
            model.lst_detalle_cbte = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);

            if (!validar(model, ref mensaje))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos();
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                return View(model);
            }

            if (!bus_liquidacion.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            };

            cargar_combos();
            return RedirectToAction("Index");
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdSucursal=0, decimal IdLiquidacion = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_LiquidacionRetProv_Info model = bus_liquidacion.get_info(IdEmpresa, IdSucursal, IdLiquidacion);
            if (model == null)
                return RedirectToAction("Index");

            model.lst_detalle = bus_liquidacion_det.GetList(IdEmpresa, IdSucursal, IdLiquidacion);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_detalle_cbte = bus_cbte_det.get_list(model.IdEmpresa, Convert.ToInt32(model.IdTipoCbte), Convert.ToInt32(model.IdCbteCble));
            ListaDetalle.set_list(model.lst_detalle, model.IdTransaccionSession);
            list_ct_cbtecble_det.set_list(model.lst_detalle_cbte, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.li_Fecha, cl_enumeradores.eModulo.FAC, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                cargar_combos();
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cxc_LiquidacionRetProv_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario.ToString();
            model.lst_detalle = ListaDetalle.get_list(model.IdTransaccionSession);
            model.lst_detalle_cbte = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);

            if (!validar(model, ref mensaje))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos();
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                return View(model);
            }

            if (!bus_liquidacion.anularDB(model))
            {
                cargar_combos();
                return View(model);
            };

            cargar_combos();
            return RedirectToAction("Index");
        }
        #endregion

        #region Json
        public JsonResult GetList_Liquidaciones_x_Cruzar(decimal IdTransaccionSession = 0, int IdEmpresa = 0, int IdSucursal=0)
        {
            var lst = bus_liquidacion_det.GetList_X_Cruzar(IdEmpresa, IdSucursal);
            Lista_XCruzar.set_list(lst, IdTransaccionSession);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult EditingAddNewDetalle(string IDs = "", decimal IdTransaccionSession = 0)
        {
            if (IDs != "")
            {
                int IdEmpresaSesion = Convert.ToInt32(SessionFixed.IdEmpresa);
                var lst_x_ingresar = Lista_XCruzar.get_list(IdTransaccionSession);
                string[] array = IDs.Split(',');

                foreach (var item in array)
                {
                    var info_det = lst_x_ingresar.Where(q => q.SecuencialCobro == item).FirstOrDefault();
                    if (info_det != null)
                    {
                        ListaDetalle.AddRow(info_det, IdTransaccionSession);
                    }
                }
            }
            var lst = ListaDetalle.get_list(IdTransaccionSession);
            ListaDetalle.set_list(lst, IdTransaccionSession);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult armar_diario(decimal IdTransaccionSession = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_parametro = bus_parametro.get_info(IdEmpresa);

            list_ct_cbtecble_det.set_list(new List<ct_cbtecble_det_Info>(), IdTransaccionSession);
            var lst_detalle = ListaDetalle.get_list(IdTransaccionSession);
            foreach (var item in lst_detalle)
            {
                list_ct_cbtecble_det.AddRow(new ct_cbtecble_det_Info
                {
                    IdCtaCble = item.IdCtaCble,
                    dc_Valor_debe = Math.Round(item.Valor, 2, MidpointRounding.AwayFromZero),
                    dc_Valor = Math.Round(item.Valor * -1, 2, MidpointRounding.AwayFromZero),
                    IdPunto_cargo_grupo = (item.ESRetenFTE == "S" ? info_parametro.IdPunto_cargo_grupo_Fte : info_parametro.IdPunto_cargo_grupo_Iva),
                    IdPunto_cargo = (item.ESRetenFTE == "S" ? info_parametro.IdPunto_cargo_Fte : info_parametro.IdPunto_cargo_Iva)
                }, IdTransaccionSession);

                list_ct_cbtecble_det.AddRow(new ct_cbtecble_det_Info
                {
                    IdCtaCble = (item.ESRetenFTE=="S" ? info_parametro.IdCtaCble_ProvisionFuente : info_parametro.IdCtaCble_ProvisionIva),
                    dc_Valor_haber = Math.Round(item.Valor, 2, MidpointRounding.AwayFromZero),
                    dc_Valor = Math.Round(item.Valor * -1, 2, MidpointRounding.AwayFromZero),
                    IdPunto_cargo_grupo = (item.ESRetenFTE == "S" ? info_parametro.IdPunto_cargo_grupo_Fte : info_parametro.IdPunto_cargo_grupo_Iva),
                    IdPunto_cargo = (item.ESRetenFTE == "S" ? info_parametro.IdPunto_cargo_Fte : info_parametro.IdPunto_cargo_Iva)
                }, IdTransaccionSession);
            }

            var lst_cbtecble = list_ct_cbtecble_det.get_list(IdTransaccionSession);

            var lst = (from q in lst_cbtecble
                       group q by new { q.IdEmpresa, q.IdCtaCble, q.pc_Cuenta, q.IdPunto_cargo_grupo, q.IdPunto_cargo }
                       into g
                       select new
                       {
                           IdCtaCble = g.Key,
                           Valor = g.Sum(q => q.dc_Valor),
                       }).ToList();


            return Json(lst_cbtecble, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Grids
        [ValidateInput(false)]
        public ActionResult GridViewPartial_LiquidacionRetencionClienteDet()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = ListaDetalle.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_LiquidacionRetencionClienteDet", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_LiquidacionRetencionClienteXCruzar()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_XCruzar.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_LiquidacionRetencionClienteXCruzar", model);
        }

        public ActionResult EditingDeleteFactura(int Secuencia)
        {
            ListaDetalle.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = ListaDetalle.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_LiquidacionRetencionClienteDet", model);
        }
        #endregion
    }

    public class cxc_LiquidacionRetProvDet_List
    {
        string Variable = "cxc_LiquidacionRetProvDet_Info";
        public List<cxc_LiquidacionRetProvDet_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_LiquidacionRetProvDet_Info> list = new List<cxc_LiquidacionRetProvDet_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_LiquidacionRetProvDet_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_LiquidacionRetProvDet_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
        public void AddRow(cxc_LiquidacionRetProvDet_Info info_det, decimal IdTransaccionSession)
        {
            List<cxc_LiquidacionRetProvDet_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;

            list.Add(info_det);
        }
        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<cxc_LiquidacionRetProvDet_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).FirstOrDefault());
        }
    }

    public class cxc_LiquidacionRetProvDet_XCruzar_List
    {
        string Variable = "cxc_LiquidacionRetProvDet_XCruzar_Info";
        public List<cxc_LiquidacionRetProvDet_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_LiquidacionRetProvDet_Info> list = new List<cxc_LiquidacionRetProvDet_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_LiquidacionRetProvDet_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_LiquidacionRetProvDet_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}