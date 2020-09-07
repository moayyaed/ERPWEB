using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class CatalogoInventarioController : Controller
    {
        #region Variables
        in_Catalogo_Bus bus_catalogo = new in_Catalogo_Bus();
        in_CatalogoTipo_Bus bus_catalogo_tipo = new in_CatalogoTipo_Bus();
        in_Catalogo_List Lista_Catalogo = new in_Catalogo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion
        #region Index
        public ActionResult Index(int IdCatalogo_tipo = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "CatalogoTipoInventario", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            in_CatalogoTipo_Info model = new in_CatalogoTipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdCatalogo_tipo = IdCatalogo_tipo
            };

            var lst = bus_catalogo.get_list(model.IdCatalogo_tipo, true);
            Lista_Catalogo.set_list(lst, model.IdTransaccionSession);
            return View(model);
            
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogo_inventario(int IdCatalogo_tipo = 0, bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            return PartialView("_GridViewPartial_catalogo_inventario", model);
        }

        private void cargar_combos()
        {
            var lst_catalogo_tipo = bus_catalogo_tipo.get_list(false);
            ViewBag.lst_tipos = lst_catalogo_tipo;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdCatalogo_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "CatalogoTipoInventario", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            in_Catalogo_Info model = new in_Catalogo_Info
            {
                IdCatalogo_tipo = IdCatalogo_tipo
            };
            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(in_Catalogo_Info model)
        {
            if (bus_catalogo.validar_existe_IdCatalogo(model.IdCatalogo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                return View(model);
            }

            if (!bus_catalogo.guardarDB(model))
            {
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                return View(model);
            }

            return RedirectToAction("Index", new { IdCatalogo_tipo = model.IdCatalogo_tipo });
        }
        public ActionResult Consultar(string IdCatalogo = "", int IdCatalogo_tipo = 0, bool Exito=false)
        {
            in_Catalogo_Info model = bus_catalogo.get_info(IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "CatalogoTipoInventario", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }
        public ActionResult Modificar(string IdCatalogo = "", int IdCatalogo_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "CatalogoTipoInventario", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            in_Catalogo_Info model = bus_catalogo.get_info(IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });
            ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(in_Catalogo_Info model)
        {
            if (!bus_catalogo.modificarDB(model))
            {
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                return View(model);
            }
            return RedirectToAction("Index", new { IdCatalogo_tipo = model.IdCatalogo_tipo });

        }

        public ActionResult Anular(string IdCatalogo = "", int IdCatalogo_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "CatalogoTipoInventario", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            in_Catalogo_Info model = bus_catalogo.get_info(IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdTipoCatalogo = IdCatalogo_tipo });
            ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(in_Catalogo_Info model)
        {
            if (!bus_catalogo.anularDB(model))
            {
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                return View(model);
            }
            return RedirectToAction("Index", new { IdCatalogo_tipo = model.IdCatalogo_tipo });

        }

        #endregion
    }

    public class in_Catalogo_List
    {
        string Variable = "in_Catalogo_Info";
        public List<in_Catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<in_Catalogo_Info> list = new List<in_Catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<in_Catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<in_Catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}