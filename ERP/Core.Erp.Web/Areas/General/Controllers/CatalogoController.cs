using Core.Erp.Bus.General;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.General;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.General.Controllers
{
    [SessionTimeout]
    public class CatalogoController : Controller
    {
        #region Variables
        tb_Catalogo_Bus bus_catalogo = new tb_Catalogo_Bus();
        tb_CatalogoTipo_Bus bus_catalogo_tipo = new tb_CatalogoTipo_Bus();
        tb_Catalogo_List Lista_Catalogo = new tb_Catalogo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        #endregion

        #region Index

        public ActionResult Index(int IdTipoCatalogo = 0)
        {
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "CatalogoTipo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            tb_Catalogo_Info model = new tb_Catalogo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdTipoCatalogo = IdTipoCatalogo
            };

            var lst = bus_catalogo.get_list(model.IdTipoCatalogo, true);
            Lista_Catalogo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogo(int IdTipoCatalogo = 0, bool Nuevo=false)
        {
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogo", model);
        }

        private void cargar_combos()
        {
            var lst_catalogo_tipo = bus_catalogo_tipo.get_list();
            ViewBag.lst_tipos = lst_catalogo_tipo;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdTipoCatalogo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "CatalogoTipo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            tb_Catalogo_Info model = new tb_Catalogo_Info
            {
                IdTipoCatalogo = IdTipoCatalogo
            };
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(tb_Catalogo_Info model)
        {
            if (bus_catalogo.validar_existe_CodCatalogo(model.CodCatalogo))
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

        public ActionResult Consultar(string CodCatalogo = "", int IdTipoCatalogo = 0, bool Exito=false)
        {
            tb_Catalogo_Info model = bus_catalogo.get_info(CodCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdTipoCatalogo = IdTipoCatalogo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "CatalogoTipo", "Index");
            if (model.ca_estado == "I")
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

            ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }

        public ActionResult Modificar(string CodCatalogo = "", int IdTipoCatalogo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "CatalogoTipo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            tb_Catalogo_Info model = bus_catalogo.get_info(CodCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdTipoCatalogo = IdTipoCatalogo });
            ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(tb_Catalogo_Info model)
        {
            if (!bus_catalogo.modificarDB(model))
            {
                ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdTipoCatalogo = model.IdTipoCatalogo });

        }

        public ActionResult Anular(string CodCatalogo = "", int IdTipoCatalogo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "CatalogoTipo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            tb_Catalogo_Info model = bus_catalogo.get_info(CodCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdTipoCatalogo = IdTipoCatalogo });
            ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(tb_Catalogo_Info model)
        {
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

    public class tb_Catalogo_List
    {
        string Variable = "tb_Catalogo_Info";
        public List<tb_Catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_Catalogo_Info> list = new List<tb_Catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_Catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_Catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}