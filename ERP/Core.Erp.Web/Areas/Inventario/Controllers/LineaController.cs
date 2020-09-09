using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class LineaController : Controller
    {
        #region Index / Metodos
        in_linea_Bus bus_linea = new in_linea_Bus();
        in_categorias_Bus bus_categoria = new in_categorias_Bus();
        in_linea_List Lista_Linea = new in_linea_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index(int IdEmpresa = 0, string IdCategoria = "")
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "Categoria", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            in_linea_Info model = new in_linea_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdCategoria = IdCategoria,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_linea.get_list(model.IdEmpresa, model.IdCategoria, true);
            Lista_Linea.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_linea(int IdEmpresa = 0 , string IdCategoria = "", bool Nuevo=false)
        {
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdCategoria = IdCategoria;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Linea.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_linea", model);
        }

        private void cargar_combos(int IdEmpresa)
        {
            var lst_categoria = bus_categoria.get_list(IdEmpresa, false);
            ViewBag.lst_categorias = lst_categoria;
        }

        #endregion
        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0 , string IdCategoria = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "Categoria", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            in_linea_Info model = new in_linea_Info
            {
                IdCategoria = IdCategoria,
                IdEmpresa = IdEmpresa
            };
            ViewBag.IdCategoria = IdCategoria;
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(in_linea_Info model)
        {
            model.IdUsuario = Session["IdUsuario"].ToString();
            if (!bus_linea.guardarDB(model))
            {
                ViewBag.IdCategoria = model.IdCategoria;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdCategoria = model.IdCategoria });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdLinea = 0, string IdCategoria = "", bool Exito = false)
        {
            in_linea_Info model = bus_linea.get_info(IdEmpresa, IdCategoria, IdLinea);
            if (model == null)
            {
                ViewBag.IdCategoria = IdCategoria;
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdCategoria = IdCategoria });
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "Categoria", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            cargar_combos(IdEmpresa);
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0 , int IdLinea = 0, string IdCategoria = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "Categoria", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            in_linea_Info model = bus_linea.get_info(IdEmpresa, IdCategoria, IdLinea);
            if (model == null)
            {
                ViewBag.IdCategoria = IdCategoria;
                return RedirectToAction("Index",new { IdEmpresa = IdEmpresa, IdCategoria = IdCategoria });
            }
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(in_linea_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario.ToString();
            if (!bus_linea.modificarDB(model))
            {
                ViewBag.IdCategoria = model.IdCategoria;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdCategoria = model.IdCategoria });
        }
        public ActionResult Anular(int IdEmpresa = 0 , int IdLinea = 0, string IdCategoria = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "Categoria", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            in_linea_Info model = bus_linea.get_info(IdEmpresa, IdCategoria, IdLinea);
            if (model == null)
            {
                ViewBag.IdCategoria = IdCategoria;
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdCategoria = model.IdCategoria });
            }
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(in_linea_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario.ToString();
            if (!bus_linea.anularDB(model))
            {
                ViewBag.IdCategoria = model.IdCategoria;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa,  IdCategoria = model.IdCategoria });
        }

        #endregion
    }
    public class in_linea_List
    {
        string Variable = "in_linea_Info";
        public List<in_linea_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_linea_Info> list = new List<in_linea_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_linea_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_linea_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

}