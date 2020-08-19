using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Info.Facturacion;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class FormaPagoController : Controller
    {
        #region MyRegion
        fa_formaPago_Bus bus_forma = new fa_formaPago_Bus();
        fa_formaPago_List Lista_FormaPago = new fa_formaPago_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        #endregion

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FormaPago", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_formaPago_Info model = new fa_formaPago_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_forma.get_list(true);
            Lista_FormaPago.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FormaPago", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_formaPago_Info model = new fa_formaPago_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_formaPago_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;

            if (bus_forma.ValidarIdFormaPago(model.IdFormaPago))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                return View(model);
            }

            if (!bus_forma.GuardarDB(model))
            {
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdFormaPago = model.IdFormaPago, Exito = true });
        }
        public ActionResult Consultar(string IdFormaPago = "", bool Exito=false)
        {
            fa_formaPago_Info model = bus_forma.GetInfo(IdFormaPago);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FormaPago", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }
        public ActionResult Modificar(string IdFormaPago = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FormaPago", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_formaPago_Info model = bus_forma.GetInfo(IdFormaPago);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_formaPago_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;

            if (!bus_forma.ModificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdFormaPago = model.IdFormaPago, Exito = true });

        }
        public ActionResult Anular(string IdFormaPago = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FormaPago", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_formaPago_Info model = bus_forma.GetInfo(IdFormaPago);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_formaPago_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;

            if (!bus_forma.AnularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");

        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_forma_pago(bool Nuevo=false)
        {
            //var model = bus_forma.get_list(true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_FormaPago.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_forma_pago", model);
        }
    }

    public class fa_formaPago_List
    {
        string Variable = "fa_formaPago_Info";
        public List<fa_formaPago_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_formaPago_Info> list = new List<fa_formaPago_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_formaPago_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_formaPago_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}