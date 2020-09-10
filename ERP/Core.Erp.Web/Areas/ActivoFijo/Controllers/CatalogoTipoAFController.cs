using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    [SessionTimeout]
    public class CatalogoTipoAFController : Controller
    {
        #region Index
        Af_CatalogoTipo_Bus bus_catalogo = new Af_CatalogoTipo_Bus();
        Af_CatalogoTipo_List Lista_CatalogoTipo = new Af_CatalogoTipo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            Af_CatalogoTipo_Info model = new Af_CatalogoTipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_catalogo.get_list();
            Lista_CatalogoTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogotipo_af(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CatalogoTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogotipo_af", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            Af_CatalogoTipo_Info model = new Af_CatalogoTipo_Info();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(Af_CatalogoTipo_Info model)
        {
            if (bus_catalogo.validar_existe_IdTipoCatalogo(model.IdTipoCatalogo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
                return View(model);
            }
            if (!bus_catalogo.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoCatalogo = model.IdTipoCatalogo, Exito = true });
        }

        public ActionResult Consultar(string IdTipoCatalogo = "", bool Exito=false)
        {
            Af_CatalogoTipo_Info model = bus_catalogo.get_info(IdTipoCatalogo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(string IdTipoCatalogo = "")
        {
            Af_CatalogoTipo_Info model = bus_catalogo.get_info(IdTipoCatalogo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(Af_CatalogoTipo_Info model)
        {
            if (!bus_catalogo.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoCatalogo = model.IdTipoCatalogo, Exito = true });
        }

        #endregion
    }

    public class Af_CatalogoTipo_List
    {
        string Variable = "Af_CatalogoTipo_Info";
        public List<Af_CatalogoTipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_CatalogoTipo_Info> list = new List<Af_CatalogoTipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_CatalogoTipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_CatalogoTipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}