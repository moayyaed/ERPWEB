using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.Helps;
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
        string mensaje = string.Empty;
        #endregion

        #region Index
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_LiquidacionRetencionCliente(DateTime? Fecha_ini, DateTime? Fecha_fin)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);

            var model = bus_liquidacion.get_list(IdEmpresa, ViewBag.Fecha_ini, ViewBag.Fecha_fin);

            return PartialView("_GridViewPartial_LiquidacionRetencionCliente", model);
        }
        #endregion

        #region Metodos
        private void cargar_combos(cxc_LiquidacionRetProv_Info model)
        {
            var lst_tipo_cbte = bus_tipo_cbte.get_list(model.IdEmpresa, false);
            ViewBag.lst_tipo_cbte = lst_tipo_cbte;
        }
        #endregion

        #region acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_LiquidacionRetProv_Info model = new cxc_LiquidacionRetProv_Info
            {
                IdEmpresa = IdEmpresa,
                li_Fecha = DateTime.Now,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            ListaDetalle.set_list(model.lst_detalle, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.li_Fecha, cl_enumeradores.eModulo.FAC, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cxc_LiquidacionRetProv_Info model)
        {
            model.lst_detalle = ListaDetalle.get_list(model.IdTransaccionSession);
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_liquidacion.guardarDB(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                return View(model);
            };
            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdLiquidacion = model.IdLiquidacion, Exito = true });
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal IdLiquidacion = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_LiquidacionRetProv_Info model = bus_liquidacion.get_info(IdEmpresa, IdLiquidacion);
            if (model == null)
                return RedirectToAction("Index");

            model.lst_detalle = bus_liquidacion_det.GetList(IdEmpresa, IdLiquidacion);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            ListaDetalle.set_list(model.lst_detalle, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.li_Fecha, cl_enumeradores.eModulo.FAC, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cxc_LiquidacionRetProv_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario.ToString();

            if (!bus_liquidacion.modificarDB(model))
            {
                return View(model);
            };
            return RedirectToAction("Index");
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdLiquidacion = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_LiquidacionRetProv_Info model = bus_liquidacion.get_info(IdEmpresa, IdLiquidacion);
            if (model == null)
                return RedirectToAction("Index");

            model.lst_detalle = bus_liquidacion_det.GetList(IdEmpresa, IdLiquidacion);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            ListaDetalle.set_list(model.lst_detalle, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.li_Fecha, cl_enumeradores.eModulo.FAC, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cxc_LiquidacionRetProv_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario.ToString();

            if (!bus_liquidacion.anularDB(model))
            {
                return View(model);
            };
            return RedirectToAction("Index");
        }
        #endregion

        #region Json
        public JsonResult GetList_Liquidaciones_x_Cruzar(decimal IdTransaccionSession = 0, int IdEmpresa = 0)
        {
            var lst = bus_liquidacion_det.GetList_X_Cruzar(IdEmpresa);
            Lista_XCruzar.set_list(lst, IdTransaccionSession);

            return Json("", JsonRequestBehavior.AllowGet);
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
                    var info_det = lst_x_ingresar.Where(q => q.secuencial == Convert.ToInt32(item)).FirstOrDefault();
                    if (info_det != null)
                    {
                        //ListaDetalle.AddRow(info_det, IdTransaccionSession);
                    }
                }
            }
            var lst = ListaDetalle.get_list(IdTransaccionSession);
            ListaDetalle.set_list(lst, IdTransaccionSession);

            return Json(lst, JsonRequestBehavior.AllowGet);
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