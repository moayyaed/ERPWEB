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
    public class NivelDescuentoController : Controller
    {
        fa_NivelDescuento_Bus bus_nivel = new fa_NivelDescuento_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_NivelDescuento_List Lista_NivelDescuento = new fa_NivelDescuento_List();

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "NivelDescuento", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_NivelDescuento_Info model = new fa_NivelDescuento_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            var lst = bus_nivel.GetList(model.IdEmpresa, true);
            Lista_NivelDescuento.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        public ActionResult GridViewPartial_nivel_dscto_fa(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_nivel.GetList(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_NivelDescuento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_nivel_dscto_fa", model);
        }
        #region Acciones

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "NivelDescuento", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_NivelDescuento_Info model = new fa_NivelDescuento_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_NivelDescuento_Info model)
        {
            if (!bus_nivel.GuardarDB(model))
            {
                return View(model);

            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdNivel = model.IdNivel, Exito = true });

        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdNivel = 0, bool Exito = false)
        {
            fa_NivelDescuento_Info model = bus_nivel.GetInfo(IdEmpresa, IdNivel);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "NivelDescuento", "Index");
            if (model.Estado == false)
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

        public ActionResult Modificar(int IdEmpresa = 0, int IdNivel = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "NivelDescuento", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_NivelDescuento_Info model = bus_nivel.GetInfo(IdEmpresa, IdNivel);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_NivelDescuento_Info model)
        {
            if (!bus_nivel.ModificarDB(model))

            {
                return View(model);

            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdNivel = model.IdNivel, Exito = true });

        }
        public ActionResult Anular(int IdEmpresa = 0, int IdNivel = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "NivelDescuento", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_NivelDescuento_Info model = bus_nivel.GetInfo(IdEmpresa, IdNivel);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_NivelDescuento_Info model)
        {
            if (!bus_nivel.AnularDB(model))
            {
                return View(model);

            }
            return RedirectToAction("Index");

        }
        #endregion

    }

    public class fa_NivelDescuento_List
    {
        string Variable = "fa_NivelDescuento_Info";
        public List<fa_NivelDescuento_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_NivelDescuento_Info> list = new List<fa_NivelDescuento_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_NivelDescuento_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_NivelDescuento_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}