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
    public class CatalogoFacturacionController : Controller
    {
        #region Index

        fa_catalogo_Bus bus_catalogo = new fa_catalogo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_catalogo_List Lista_Catalogo = new fa_catalogo_List();

        public ActionResult Index(int IdCatalogo_tipo = 0)
        {
            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;

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

            fa_catalogo_Info model = new fa_catalogo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdCatalogo_tipo = IdCatalogo_tipo,
            };

            var lst = bus_catalogo.get_list(IdCatalogo_tipo, true);
            Lista_Catalogo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        public ActionResult GridViewPartial_catalogo_fa(int IdCatalogo_tipo = 0, bool Nuevo = false)
        {
            //List<fa_catalogo_Info> model = bus_catalogo.get_list(IdCatalogo_tipo, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogo_fa", model);
        }
        private void cargar_combos()
        {
            fa_catalogo_tipo_Bus bus_tipo = new fa_catalogo_tipo_Bus();
            var lst_tipo = bus_tipo.get_list(false);
            ViewBag.lst_tipo = lst_tipo;
        }
        #endregion
        #region Acciones

        public ActionResult Nuevo(int IdCatalogo_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_catalogo_Info model = new fa_catalogo_Info
            {
                IdCatalogo_tipo = IdCatalogo_tipo
            };
            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }
       [HttpPost]
        public ActionResult Nuevo(fa_catalogo_Info model)
        {
            if (bus_catalogo.validar_existe_IdCatalogo(model.IdCatalogo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                cargar_combos();
                return View(model);
            }

            if (!bus_catalogo.guardarDB(model))
            {
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdCatalogo_tipo = model.IdCatalogo_tipo, IdCatalogo=model.IdCatalogo, Exito = true });
        }
        public ActionResult Consultar(int IdCatalogo_tipo = 0, string IdCatalogo = "", bool Exito=false)
        {
            fa_catalogo_Info model = bus_catalogo.get_info(IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            if (model.Estado == "I")
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

            ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }
        public ActionResult Modificar(int IdCatalogo_tipo = 0, string IdCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_catalogo_Info model = bus_catalogo.get_info(IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });
            ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(fa_catalogo_Info model)
        {
            if (!bus_catalogo.modificarDB(model))
            {
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdCatalogo_tipo = model.IdCatalogo_tipo, IdCatalogo = model.IdCatalogo, Exito = true });
        }
        public ActionResult Anular(int IdCatalogo_tipo = 0, string IdCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "CatalogoTipoFacturacion", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_catalogo_Info model = bus_catalogo.get_info(IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });
            ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(fa_catalogo_Info model)
        {
            if (!bus_catalogo.anularDB(model))
            {
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdCatalogo_tipo = model.IdCatalogo_tipo });
        }
        #endregion
    }

    public class fa_catalogo_List
    {
        string Variable = "fa_catalogo_Info";
        public List<fa_catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_catalogo_Info> list = new List<fa_catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}