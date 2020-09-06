using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Banco;
using Core.Erp.Info.Banco;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Banco.Controllers
{
    [SessionTimeout]
    public class CatalogoBancoController : Controller
    {
        #region Index

        ba_Catalogo_Bus bus_catalogo = new ba_Catalogo_Bus();
        ba_Catalogo_List Lista_Catalogo = new ba_Catalogo_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        public ActionResult Index(string IdTipoCatalogo = "")
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            ba_Catalogo_Info model = new ba_Catalogo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdTipoCatalogo = IdTipoCatalogo
            };

            var lst = bus_catalogo.get_list(model.IdTipoCatalogo, true);
            Lista_Catalogo.set_list(lst, model.IdTransaccionSession);

            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogo_banco(bool Nuevo=false, string IdTipoCatalogo = "")
        {
            ViewBag.Nuevo = Nuevo;
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogo_banco", model);
        }
        private void cargar_combos()
        {
            ba_CatalogoTipo_Bus bus_catalogotipo = new ba_CatalogoTipo_Bus();
            var lst_catalogo_tipo = bus_catalogotipo.get_list();
            ViewBag.lst_tipos = lst_catalogo_tipo;
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo(string IdTipoCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ba_Catalogo_Info model = new ba_Catalogo_Info
            {
                IdTipoCatalogo = IdTipoCatalogo
            };
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ba_Catalogo_Info model)
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
        public ActionResult Modificar(string IdTipoCatalogo = "", string IdCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ba_Catalogo_Info model = bus_catalogo.get_info(IdTipoCatalogo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdTipoCatalogo });
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ba_Catalogo_Info model)
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ba_Catalogo_Info model = bus_catalogo.get_info(IdTipoCatalogo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdTipoCatalogo });
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(ba_Catalogo_Info model)
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

    public class ba_Catalogo_List
    {
        string Variable = "ba_Catalogo_Info";
        public List<ba_Catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_Catalogo_Info> list = new List<ba_Catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_Catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_Catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}