using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorCobrar.Controllers
{
    [SessionTimeout]
    public class CatalogoTipoCXCController : Controller
    {
        #region Index

        cxc_CatalogoTipo_Bus bus_catalogotipo = new cxc_CatalogoTipo_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        cxc_CatalogoTipo_List Lista_CatalogoTipo = new cxc_CatalogoTipo_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            cxc_CatalogoTipo_Info model = new cxc_CatalogoTipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_catalogotipo.get_list();
            Lista_CatalogoTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogotipocxc(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CatalogoTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogotipocxc", model);
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo ()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cxc_CatalogoTipo_Info model = new cxc_CatalogoTipo_Info();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(cxc_CatalogoTipo_Info model)
        {
            if (bus_catalogotipo.validar_existe_IdCatalogotipo(model.IdCatalogo_tipo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                return View(model);
            }
            if (!bus_catalogotipo.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdCatalogo_tipo = model.IdCatalogo_tipo, Exito = true });
        }

        public ActionResult Consultar(string IdCatalogo_tipo = "", bool Exito=false)
        {
            cxc_CatalogoTipo_Info model = bus_catalogotipo.get_info(IdCatalogo_tipo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            //if (model.Estado == "I")
            //{
            //    info.Modificar = false;
            //    info.Anular = false;
            //}
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(string IdCatalogo_tipo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            cxc_CatalogoTipo_Info model = bus_catalogotipo.get_info(IdCatalogo_tipo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cxc_CatalogoTipo_Info model)
        {
            if (!bus_catalogotipo.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdCatalogo_tipo = model.IdCatalogo_tipo, Exito = true });
        }
        #endregion
    }

    public class cxc_CatalogoTipo_List
    {
        string Variable = "cxc_CatalogoTipo_Info";
        public List<cxc_CatalogoTipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<cxc_CatalogoTipo_Info> list = new List<cxc_CatalogoTipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<cxc_CatalogoTipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<cxc_CatalogoTipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}