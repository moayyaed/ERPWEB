using Core.Erp.Bus.Compras;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Compras;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Compras.Controllers
{
    [SessionTimeout]
    public class EstadoCierreComprasController : Controller
    {
        #region Index
        com_estado_cierre_Bus bus_estado = new com_estado_cierre_Bus();
        com_estado_cierre_List Lista_EstadoCierre = new com_estado_cierre_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "EstadoCierreCompras", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            com_estado_cierre_Info model = new com_estado_cierre_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_estado.get_list(true);
            Lista_EstadoCierre.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_estadocierre(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_EstadoCierre.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_estadocierre", model);
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "EstadoCierreCompras", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            com_estado_cierre_Info model = new com_estado_cierre_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(com_estado_cierre_Info model)
        {
            if (bus_estado.validar_existe_IdEstado(model.IdEstado_cierre))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                return View(model);
            }
            if (!bus_estado.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEstado_cierre = model.IdEstado_cierre, Exito = true });
        }
        public ActionResult Consultar(string IdEstado_cierre = "", bool Exito=false)
        {
            com_estado_cierre_Info model = bus_estado.get_info(IdEstado_cierre);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "EstadoCierreCompras", "Index");
            if (model.estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }
        public ActionResult Modificar(string IdEstado_cierre = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "EstadoCierreCompras", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            com_estado_cierre_Info model = bus_estado.get_info( IdEstado_cierre);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(com_estado_cierre_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_estado.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEstado_cierre = model.IdEstado_cierre, Exito = true });
        }

        public ActionResult Anular(string IdEstado_cierre = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "EstadoCierreCompras", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            com_estado_cierre_Info model = bus_estado.get_info(IdEstado_cierre);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(com_estado_cierre_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_estado.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class com_estado_cierre_List
    {
        string Variable = "com_estado_cierre_Info";
        public List<com_estado_cierre_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<com_estado_cierre_Info> list = new List<com_estado_cierre_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<com_estado_cierre_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<com_estado_cierre_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}