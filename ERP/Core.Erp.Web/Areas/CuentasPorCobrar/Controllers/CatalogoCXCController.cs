using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorCobrar.Controllers
{
    [SessionTimeout]
    public class CatalogoCXCController : Controller
    {
        #region Index
        cxc_Catalogo_Bus bus_catalogo = new cxc_Catalogo_Bus();
        cxc_Catalogo_List Lista_Catalogo = new cxc_Catalogo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        cxc_CatalogoTipo_List Lista_CatalogoTipo = new cxc_CatalogoTipo_List();

        public ActionResult Index(string IdCatalogo_tipo = "")
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            cxc_Catalogo_Info model = new cxc_Catalogo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdCatalogo_tipo = IdCatalogo_tipo
            };

            var lst = bus_catalogo.get_list(IdCatalogo_tipo, true);
            Lista_Catalogo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogocxc(bool Nuevo = false)
        {
            //List<cxc_Catalogo_Info> model = new List<cxc_Catalogo_Info>();
            //model = bus_catalogo.get_list(IdCatalogo_tipo, true);
            //ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogocxc", model);
        }
        private void cargar_combos()
        {
            cxc_CatalogoTipo_Bus bus_catalogotipo = new cxc_CatalogoTipo_Bus();
            var lst_catalogo_tipo = bus_catalogotipo.get_list();
            ViewBag.lst_tipos = lst_catalogo_tipo;

            Dictionary<int, string> lst_orden = new Dictionary<int, string>();
            lst_orden.Add(1, "1");
            lst_orden.Add(2, "2");
            lst_orden.Add(3, "3");
            lst_orden.Add(4, "4");
            lst_orden.Add(5, "5");
            ViewBag.lst_orden = lst_orden;
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo(string IdCatalogo_tipo = "")
        {
            cxc_Catalogo_Info model = new cxc_Catalogo_Info
            {
                IdCatalogo_tipo = IdCatalogo_tipo
            };

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cxc_Catalogo_Info model)
        {
            model.IdUsuario = Session["IdUsuario"].ToString();
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

            return RedirectToAction("Index", new { IdCatalogo_tipo = model.IdCatalogo_tipo });
        }

        public ActionResult Consultar(string IdCatalogo_tipo = "", string IdCatalogo = "", bool Exito=false)
        {
            cxc_Catalogo_Info model = bus_catalogo.get_info(IdCatalogo_tipo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo==true ? 1 : 0 );
            model.Modificar = (info.Modificar==true ? 1: 0);
            model.Anular = (info.Anular ==true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }

        public ActionResult Modificar(string IdCatalogo_tipo = "", string IdCatalogo = "")
        {
            cxc_Catalogo_Info model = bus_catalogo.get_info(IdCatalogo_tipo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cxc_Catalogo_Info model)
        {
            if (!bus_catalogo.modificarDB(model))
            {
                ViewBag.IdCatalogo_tipo = model.IdCatalogo_tipo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdCatalogo_tipo = model.IdCatalogo_tipo });

        }

        public ActionResult Anular(string IdCatalogo_tipo = "", string IdCatalogo = "")
        {
            cxc_Catalogo_Info model = bus_catalogo.get_info(IdCatalogo_tipo, IdCatalogo);
            if (model == null)
                return RedirectToAction("Index", new { IdCatalogo_tipo = IdCatalogo_tipo });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "CatalogoTipoCXC", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ViewBag.IdCatalogo_tipo = IdCatalogo_tipo;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cxc_Catalogo_Info model)
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

    public class cxc_Catalogo_List
    {
        string Variable = "cxc_Catalogo_Info";
        public List<cxc_Catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<cxc_Catalogo_Info> list = new List<cxc_Catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<cxc_Catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<cxc_Catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}
