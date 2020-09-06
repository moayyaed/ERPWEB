using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Compras;
using Core.Erp.Info.Compras;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Compras.Controllers
{
    [SessionTimeout]
    public class CatalogoTipoComprasController : Controller
    {
        #region Index
        com_catalogo_tipo_Bus bus_catalogotipo = new com_catalogo_tipo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        com_catalogo_tipo_List Lista_CatalogoTipo = new com_catalogo_tipo_List();

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            com_catalogo_tipo_Info model = new com_catalogo_tipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            var lst = bus_catalogotipo.get_list(true);
            Lista_CatalogoTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catipocompras(bool Nuevo=false)
        {
            //var model = bus_catalogotipo.get_list(true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CatalogoTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catipocompras", model);
        }

        #endregion

        #region Acciones

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            com_catalogo_tipo_Info model = new com_catalogo_tipo_Info();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(com_catalogo_tipo_Info model)
        {
            if (bus_catalogotipo.validar_existe_IdCatalogotipo(model.IdCatalogocompra_tipo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdCatalogocompra_tipo = model.IdCatalogocompra_tipo;
                return View(model);
            }
            if (!bus_catalogotipo.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdCatalogocompra_tipo = model.IdCatalogocompra_tipo, Exito = true });
        }
        public ActionResult Consultar(string IdCatalogocompra_tipo = "", bool Exito=false)
        {
            com_catalogo_tipo_Info model = bus_catalogotipo.get_info(IdCatalogocompra_tipo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
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
        public ActionResult Modificar(string IdCatalogocompra_tipo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            com_catalogo_tipo_Info model = bus_catalogotipo.get_info(IdCatalogocompra_tipo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(com_catalogo_tipo_Info model)
        {
            if (!bus_catalogotipo.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdCatalogocompra_tipo = model.IdCatalogocompra_tipo, Exito = true });
        }
        public ActionResult Anular(string IdCatalogocompra_tipo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            com_catalogo_tipo_Info model = bus_catalogotipo.get_info(IdCatalogocompra_tipo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(com_catalogo_tipo_Info model)
        {
            if (!bus_catalogotipo.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

    }

    public class com_catalogo_tipo_List
    {
        string Variable = "com_catalogo_tipo_Info";
        public List<com_catalogo_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<com_catalogo_tipo_Info> list = new List<com_catalogo_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<com_catalogo_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<com_catalogo_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}