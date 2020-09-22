using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class CatalogoTipoFacturacionController : Controller
    {
        #region Index
        fa_catalogo_tipo_Bus bus_fa_catalogotipo = new fa_catalogo_tipo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_catalogo_tipo_List Lista_CatalogoTipo = new fa_catalogo_tipo_List();

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_catalogo_tipo_Info model = new fa_catalogo_tipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_fa_catalogotipo.get_list(true);
            Lista_CatalogoTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        public ActionResult GridViewPartial_catalogotipo_fa(bool Nuevo = false)
        {
            //List<fa_catalogo_tipo_Info> model = bus_fa_catalogotipo.get_list(true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CatalogoTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogotipo_fa", model);
        }

        #endregion
        #region Acciones

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_catalogo_tipo_Info model = new fa_catalogo_tipo_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_catalogo_tipo_Info model)
        {
            if (!bus_fa_catalogotipo.guardarDB(model))
            {
                return View(model);

            }
            return RedirectToAction("Consultar", new { IdCatalogo_tipo = model.IdCatalogo_tipo, Exito = true });

        }

        public ActionResult Consultar(int IdCatalogo_tipo = 0, bool Exito = false)
        {
            fa_catalogo_tipo_Info model = bus_fa_catalogotipo.get_info(IdCatalogo_tipo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
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
        public ActionResult Modificar(int IdCatalogo_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_catalogo_tipo_Info model = bus_fa_catalogotipo.get_info(IdCatalogo_tipo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_catalogo_tipo_Info model)
        {
            if (!bus_fa_catalogotipo.modificarDB(model))

            {
                return View(model);

            }
            return RedirectToAction("Consultar", new { IdCatalogo_tipo = model.IdCatalogo_tipo, Exito = true });

        }
        public ActionResult Anular(int IdCatalogo_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_catalogo_tipo_Info model = bus_fa_catalogotipo.get_info(IdCatalogo_tipo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_catalogo_tipo_Info model)
        {
            if (!bus_fa_catalogotipo.anularDB(model))
            {
                return View(model);

            }
            return RedirectToAction("Index");

        }
        #endregion
    }

    public class fa_catalogo_tipo_List
    {
        string Variable = "fa_catalogo_tipo_Info";
        public List<fa_catalogo_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_catalogo_tipo_Info> list = new List<fa_catalogo_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_catalogo_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_catalogo_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}