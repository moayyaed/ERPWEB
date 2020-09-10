using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Web.Areas.Contabilidad.Controllers;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Web.Helps;
using Core.Erp.Info.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    [SessionTimeout]
    public class VentaActivoController : Controller
    {
        #region Variables
        Af_Venta_Activo_Bus bus_venta = new Af_Venta_Activo_Bus();
        ct_cbtecble_det_List list_ct_cbtecble_det = new ct_cbtecble_det_List();
        ct_cbtecble_det_Bus bus_comprobante_detalle = new ct_cbtecble_det_Bus();
        Af_Activo_fijo_Bus bus_fijo = new Af_Activo_fijo_Bus();
        ct_cbtecble_tipo_Bus bus_tipo = new ct_cbtecble_tipo_Bus();
        string mensaje = string.Empty;
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        Af_Venta_Activo_List Lista_Venta = new Af_Venta_Activo_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "VentaActivo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            Af_Venta_Activo_Info model = new Af_Venta_Activo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_venta.get_list(model.IdEmpresa, true);
            Lista_Venta.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }
        public ActionResult GridViewPartial_venta_activo(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Venta.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_venta_activo", model);
        }

        #endregion

        #region Metodos

        private bool validar(Af_Venta_Activo_Info i_validar, ref string msg)
        {
            if (i_validar.lst_ct_cbtecble_det.Count == 0)
            {
                mensaje = "Debe ingresar registros en el detalle, por favor verifique";
                return false;
            }
            if (i_validar.lst_ct_cbtecble_det.Sum(q => q.dc_Valor) != 0)
            {
                mensaje = "La suma de los detalles debe ser 0, por favor verifique";
                return false;
            }
            foreach (var item in i_validar.lst_ct_cbtecble_det)
            {
                if (string.IsNullOrEmpty(item.IdCtaCble))
                {
                    mensaje = "Faltan cuentas contables, por favor verifique";
                    return false;
                }
            }
            if (i_validar.lst_ct_cbtecble_det.Where(q => q.dc_Valor == 0).Count() > 0)
            {
                mensaje = "Existen detalles con valor 0 en el debe o haber, por favor verifique";
                return false;
            }
            return true;
        }
        [ValidateInput(false)]
        private void cargar_combos(int IdEmpresa)
        {
            
            var lst_fijo = bus_fijo.get_list(IdEmpresa, false);
            ViewBag.lst_fijo = lst_fijo;            

            var lst_tipo = bus_tipo.get_list(IdEmpresa, false);
            ViewBag.lst_tipo = lst_tipo;

        }
        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ct_plancta_Bus bus_cuenta = new ct_plancta_Bus();
            var lst_cuentas = bus_cuenta.get_list(IdEmpresa, false, true);
            ViewBag.lst_cuentas = lst_cuentas;
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "VentaActivo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            Af_Venta_Activo_Info model = new Af_Venta_Activo_Info
            {
                IdEmpresa = IdEmpresa,
                Fecha_Venta = DateTime.Now,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            model.lst_ct_cbtecble_det = new List<ct_cbtecble_det_Info>();
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det,model.IdTransaccionSession);
            cargar_combos_detalle();
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(Af_Venta_Activo_Info model)
        {
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_venta.guardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdVtaActivo = model.IdVtaActivo, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdVtaActivo = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            Af_Venta_Activo_Info model = bus_venta.get_info(IdEmpresa, IdVtaActivo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "VentaActivo", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, model.IdTipoCbte == null ? 0 : Convert.ToInt32(model.IdTipoCbte), model.IdCbteCble == null ? 0 : Convert.ToDecimal(model.IdCbteCble));
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det, model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha_Venta, cl_enumeradores.eModulo.ACF, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0 , decimal IdVtaActivo = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "VentaActivo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            Af_Venta_Activo_Info model = bus_venta.get_info(IdEmpresa, IdVtaActivo);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, model.IdTipoCbte == null ? 0 : Convert.ToInt32(model.IdTipoCbte), model.IdCbteCble == null ? 0 : Convert.ToDecimal(model.IdCbteCble));
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha_Venta, cl_enumeradores.eModulo.ACF, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(Af_Venta_Activo_Info model)
        {
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuarioUltMod = Session["IdUsuario"].ToString();
            if (!bus_venta.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdVtaActivo = model.IdVtaActivo, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdVtaActivo = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "VentaActivo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            Af_Venta_Activo_Info model = bus_venta.get_info(IdEmpresa, IdVtaActivo);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, model.IdTipoCbte == null ? 0 : Convert.ToInt32(model.IdTipoCbte), model.IdCbteCble == null ? 0 : Convert.ToDecimal(model.IdCbteCble));
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha_Venta, cl_enumeradores.eModulo.ACF, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(Af_Venta_Activo_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_venta.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Json

        public JsonResult get_valores(int IdActivoFijo = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            Af_Activo_fijo_Bus bus_activo = new Af_Activo_fijo_Bus();
            var resultado = bus_activo.get_valores(IdEmpresa, IdActivoFijo);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

    public class Af_Venta_Activo_List
    {
        string Variable = "Af_Venta_Activo_Info";

        public List<Af_Venta_Activo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Venta_Activo_Info> list = new List<Af_Venta_Activo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Venta_Activo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Venta_Activo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}