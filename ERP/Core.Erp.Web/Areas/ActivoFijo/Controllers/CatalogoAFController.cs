using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    [SessionTimeout]

    public class CatalogoAFController : Controller
    {
        #region Index

        Af_Catalogo_Bus bus_catalogo = new Af_Catalogo_Bus();
        Af_Catalogo_List Lista_Catalogo = new Af_Catalogo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index(string IdTipoCatalogo = "")
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
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            Af_Catalogo_Info model = new Af_Catalogo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdTipoCatalogo = IdTipoCatalogo
            };

            var lst = bus_catalogo.get_list(model.IdTipoCatalogo, true);
            Lista_Catalogo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogo_af(string IdTipoCatalogo = "", bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogo_af", model);
        }
        private void cargar_combos()
        {
            Af_CatalogoTipo_Bus bus_catalogotipo = new Af_CatalogoTipo_Bus();
            var lst_catalogo_tipo = bus_catalogotipo.get_list();
            ViewBag.lst_tipos = lst_catalogo_tipo;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(string IdTipoCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            Af_Catalogo_Info model = new Af_Catalogo_Info
            {
                IdTipoCatalogo = IdTipoCatalogo
            };
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(Af_Catalogo_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            if (bus_catalogo.validar_existe_IdCatalogo(model.IdCatalogo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
                cargar_combos();
                return View(model);
            }

            if (!bus_catalogo.guardarDB(model))
            {
                ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Index", new { IdTipoCatalogo = model.IdTipoCatalogo });
        }

        public ActionResult Consultar(string IdTipoCatalogo = "", string IdCatalogo = "", bool Exito=false)
        {
            Af_Catalogo_Info model = bus_catalogo.get_info(IdTipoCatalogo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdTipoCatalogo = IdTipoCatalogo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
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

            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }
        public ActionResult Modificar(string IdTipoCatalogo = "", string IdCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            Af_Catalogo_Info model = bus_catalogo.get_info(IdTipoCatalogo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdTipoCatalogo = IdTipoCatalogo });
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(Af_Catalogo_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_catalogo.modificarDB(model))
            {
                ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdTipoCatalogo = model.IdTipoCatalogo });

        }
        public ActionResult Anular(string IdTipoCatalogo = "", string IdCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "CatalogoTipoAF", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            Af_Catalogo_Info model = bus_catalogo.get_info(IdTipoCatalogo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdTipoCatalogo = IdTipoCatalogo });
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(Af_Catalogo_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_catalogo.anularDB(model))
            {
                ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdTipoCatalogo = model.IdTipoCatalogo });

        }

        #endregion

    }

    public class Af_Catalogo_List
    {
        string Variable = "Af_Catalogo_Info";
        public List<Af_Catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Catalogo_Info> list = new List<Af_Catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}