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

    public class CategoriaAFController : Controller
    {
        #region variables
        Af_Activo_fijo_Categoria_Bus bus_categoria = new Af_Activo_fijo_Categoria_Bus();
        Af_Activo_fijo_tipo_Bus bus_tipo = new Af_Activo_fijo_tipo_Bus();
        Af_Activo_fijo_Categoria_List Lista_Categoria = new Af_Activo_fijo_Categoria_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion

        #region Index
        public ActionResult Index(int IdEmpresa = 0 ,int IdActivoFijoTipo = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "TipoAF", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdActivoFijoTipo = IdActivoFijoTipo;

            Af_Activo_fijo_Categoria_Info model = new Af_Activo_fijo_Categoria_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdActivoFijoTipo = IdActivoFijoTipo
            };

            var lst = bus_categoria.get_list(model.IdEmpresa, model.IdActivoFijoTipo, true);
            Lista_Categoria.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_categoria_activo(int IdEmpresa = 0, int IdActivoFijoTipo = 0, bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Categoria.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdActivoFijoTipo = IdActivoFijoTipo;
            return PartialView("_GridViewPartial_categoria_activo", model);
        }
        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            var lst_tipo= bus_tipo.get_list(IdEmpresa, false);
            ViewBag.lst_tipo = lst_tipo;
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0, int IdActivoFijoTipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "TipoAF", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            Af_Activo_fijo_Categoria_Info model = new Af_Activo_fijo_Categoria_Info
            {
                IdEmpresa = IdEmpresa,
                IdActivoFijoTipo = IdActivoFijoTipo

            };
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(Af_Activo_fijo_Categoria_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_categoria.guardarDB(model))
            {
                ViewBag.IdActivoFijoTipo = model.IdActivoFijoTipo;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdActivoFijoTipo = model.IdActivoFijoTipo });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdActivoFijoTipo = 0, int IdCategoriaAF = 0, bool Exito=false)
        {
            Af_Activo_fijo_Categoria_Info model = bus_categoria.get_info(IdEmpresa, IdCategoriaAF);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdActivoFijoTipo = IdActivoFijoTipo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "TipoAF", "Index");
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

            ViewBag.IdActivoFijoTipo = IdActivoFijoTipo;
            cargar_combos(IdEmpresa);
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, int IdActivoFijoTipo = 0, int IdCategoriaAF = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "TipoAF", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            Af_Activo_fijo_Categoria_Info model = bus_categoria.get_info(IdEmpresa, IdCategoriaAF);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdActivoFijoTipo = IdActivoFijoTipo });
            ViewBag.IdActivoFijoTipo = IdActivoFijoTipo;
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(Af_Activo_fijo_Categoria_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_categoria.modificarDB(model))
            {
                ViewBag.IdActivoFijoTipo = model.IdActivoFijoTipo;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdActivoFijoTipo = model.IdActivoFijoTipo });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdActivoFijoTipo = 0, int IdCategoriaAF = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "TipoAF", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            Af_Activo_fijo_Categoria_Info model = bus_categoria.get_info(IdEmpresa, IdCategoriaAF);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdActivoFijoTipo = IdActivoFijoTipo });
            ViewBag.IdActivoFijoTipo = IdActivoFijoTipo;
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(Af_Activo_fijo_Categoria_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_categoria.anularDB(model))
            {
                ViewBag.IdActivoFijoTipo = model.IdActivoFijoTipo;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdActivoFijoTipo = model.IdActivoFijoTipo });
        }
    }
    #endregion

    public class Af_Activo_fijo_Categoria_List
    {
        string Variable = "Af_Activo_fijo_Categoria_Info";
        public List<Af_Activo_fijo_Categoria_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Activo_fijo_Categoria_Info> list = new List<Af_Activo_fijo_Categoria_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Activo_fijo_Categoria_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Activo_fijo_Categoria_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}