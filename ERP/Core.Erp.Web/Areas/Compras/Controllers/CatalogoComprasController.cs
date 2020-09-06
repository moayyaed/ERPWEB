using Core.Erp.Bus.Compras;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Compras;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Compras.Controllers
{
    [SessionTimeout]
    public class CatalogoComprasController : Controller
    {
        #region Index
        com_catalogo_Bus bus_catalogo = new com_catalogo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        com_catalogo_List Lista_Catalogo = new com_catalogo_List();

        public ActionResult Index(string IdCatalogocompra_tipo = "")
        {
            ViewBag.IdCatalogocompra_tipo = IdCatalogocompra_tipo;
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

            com_catalogo_Info model = new com_catalogo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdCatalogocompra_tipo = IdCatalogocompra_tipo
            };

            var lst = bus_catalogo.get_list(model.IdCatalogocompra_tipo, true);
            Lista_Catalogo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        
        [ValidateInput(false)]
        public ActionResult GridViewPartial_cat_compras(string IdCatalogocompra_tipo = "", bool Nuevo=false)
        {
            //var model = bus_catalogo.get_list(IdCatalogocompra_tipo, true);
            //ViewBag.IdCatalogocompra_tipo = IdCatalogocompra_tipo;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_cat_compras", model);
        }
        private void cargar_combos()
        {
            com_catalogo_tipo_Bus bus_catalogotipo = new com_catalogo_tipo_Bus();
            var lst_catalogo_tipo = bus_catalogotipo.get_list(false);
            ViewBag.lst_tipos = lst_catalogo_tipo;
        }

        #endregion

        #region Acciones

        public ActionResult Nuevo(string IdCatalogocompra_tipo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            com_catalogo_Info model = new com_catalogo_Info
            {
                IdCatalogocompra_tipo = IdCatalogocompra_tipo
            };
            ViewBag.IdCatalogocompra_tipo = IdCatalogocompra_tipo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(com_catalogo_Info model)
        {
            model.IdUsuario = Session["IdUsuario"].ToString();
            if (bus_catalogo.validar_existe_IdCatalogo(model.IdCatalogocompra))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdCatalogocompra_tipo = model.IdCatalogocompra_tipo;
                cargar_combos();
                return View(model);
            }

            if (!bus_catalogo.guardarDB(model))
            {
                ViewBag.IdCatalogocompra_tipo = model.IdCatalogocompra_tipo;
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Index", new { IdCatalogocompra_tipo = model.IdCatalogocompra_tipo });
        }

        public ActionResult Consultar(string IdCatalogocompra_tipo = "", string IdCatalogocompra = "", bool Exito=false)
        {
            com_catalogo_Info model = bus_catalogo.get_info(IdCatalogocompra_tipo, IdCatalogocompra);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogocompra_tipo = IdCatalogocompra_tipo });

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

            ViewBag.IdCatalogocompra_tipo = IdCatalogocompra_tipo;
            cargar_combos();
            return View(model);
        }

        public ActionResult Modificar(string IdCatalogocompra_tipo = "", string IdCatalogocompra = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            com_catalogo_Info model = bus_catalogo.get_info(IdCatalogocompra_tipo, IdCatalogocompra);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogocompra_tipo = IdCatalogocompra_tipo });
            ViewBag.IdCatalogocompra_tipo = IdCatalogocompra_tipo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(com_catalogo_Info model)
        {
            if (!bus_catalogo.modificarDB(model))
            {
                ViewBag.IdCatalogocompra_tipo = model.IdCatalogocompra_tipo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdCatalogocompra_tipo = model.IdCatalogocompra_tipo });

        }
        public ActionResult Anular(string IdCatalogocompra_tipo = "", string IdCatalogocompra = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "CatalogoTipoCompras", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            com_catalogo_Info model = bus_catalogo.get_info(IdCatalogocompra_tipo, IdCatalogocompra);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogocompra_tipo = IdCatalogocompra_tipo });
            ViewBag.IdCatalogocompra_tipo = IdCatalogocompra_tipo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(com_catalogo_Info model)
        {
            if (!bus_catalogo.anularDB(model))
            {
                ViewBag.IdCatalogocompra_tipo = model.IdCatalogocompra_tipo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdCatalogocompra_tipo = model.IdCatalogocompra_tipo });

        }
        #endregion
    }

    public class com_catalogo_List
    {
        string Variable = "com_catalogo_Info";
        public List<com_catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<com_catalogo_Info> list = new List<com_catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<com_catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<com_catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}