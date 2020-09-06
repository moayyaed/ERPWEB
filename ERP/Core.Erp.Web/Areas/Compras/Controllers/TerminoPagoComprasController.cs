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
    public class TerminoPagoComprasController : Controller
    {
        #region Index

        com_TerminoPago_Bus bus_termino = new com_TerminoPago_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        com_TerminoPago_List Lista_TerminoPago = new com_TerminoPago_List();

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "TerminoPagoCompras", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            com_TerminoPago_Info model = new com_TerminoPago_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            var lst = bus_termino.get_list(model.IdEmpresa, true);
            Lista_TerminoPago.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_terminopago_com(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TerminoPago.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_terminopago_com", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo (int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "TerminoPagoCompras", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            com_TerminoPago_Info model = new com_TerminoPago_Info
            {
                IdEmpresa = IdEmpresa
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(com_TerminoPago_Info model)
        {
            if (!bus_termino.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTerminoPago = model.IdTerminoPago, Exito = true });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdTerminoPago = 0, bool Exito=false)
        {
            com_TerminoPago_Info model = bus_termino.get_info(IdEmpresa, IdTerminoPago);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "TerminoPagoCompras", "Index");
            if (model.Estado == "I")
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
        public ActionResult Modificar(int IdEmpresa = 0, int IdTerminoPago = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "TerminoPagoCompras", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            com_TerminoPago_Info model = bus_termino.get_info(IdEmpresa, IdTerminoPago);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(com_TerminoPago_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_termino.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTerminoPago = model.IdTerminoPago, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdTerminoPago = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "TerminoPagoCompras", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            com_TerminoPago_Info model = bus_termino.get_info(IdEmpresa,IdTerminoPago);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(com_TerminoPago_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_termino.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class com_TerminoPago_List
    {
        string Variable = "com_TerminoPago_Info";
        public List<com_TerminoPago_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<com_TerminoPago_Info> list = new List<com_TerminoPago_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<com_TerminoPago_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<com_TerminoPago_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}