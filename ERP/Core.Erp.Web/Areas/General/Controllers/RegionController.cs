using Core.Erp.Bus.General;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.General;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.General.Controllers
{
    [SessionTimeout]
    public class RegionController : Controller
    {
        #region Index / Metodos

        tb_region_Bus bus_region = new tb_region_Bus();
        tb_pais_Bus bus_pais = new tb_pais_Bus();
        tb_region_List Lista_Region = new tb_region_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index(string IdPais = "")
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            tb_region_Info model = new tb_region_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdPais =IdPais
            };

            var lst = bus_region.get_list(model.IdPais, true);
            Lista_Region.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_region(string IdPais = "", bool Nuevo = false)
        {
            ViewBag.IdPais = IdPais;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Region.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_region", model);
        }
        private void cargar_combos()
        {
            var lst_pais = bus_pais.get_list( false);
            ViewBag.lst_pais = lst_pais;
        }
        #endregion
        #region Acciones
        public ActionResult Nuevo(string IdPais = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            tb_region_Info model = new tb_region_Info
            {
                IdPais = IdPais
            };
            ViewBag.IdPais = IdPais;
            cargar_combos();
            return View(model);
        }
    
        [HttpPost]
        public ActionResult Nuevo(tb_region_Info model)
        {
            if(!bus_region.guardarDB(model))
            {
                ViewBag.IdPais = model.IdPais;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais });
        }

        public ActionResult Consultar(string codRegion = "", bool Exito=false)
        {
            tb_region_Info model = bus_region.get_info(codRegion);
            if (model == null)
            {
                ViewBag.IdPais = model.IdPais;
                return RedirectToAction("Index", ViewBag.IdPais = model.IdPais);
            }
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (model.estado == false)
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

            cargar_combos();
            return View(model);
        }
        public ActionResult Modificar( string codRegion = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            tb_region_Info model = bus_region.get_info(codRegion);
            if(model == null)
            {
                ViewBag.IdPais = model.IdPais;
                return RedirectToAction("Index", ViewBag.IdPais = model.IdPais);
            }
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(tb_region_Info model)
        {
            if (!bus_region.modificarDB(model))
            {
                ViewBag.IdPais = model.IdPais;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais });
        }
        public ActionResult Anular( string codRegion = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            tb_region_Info model = bus_region.get_info( codRegion);
            if (model == null)
            {
                ViewBag.IdPais = model.IdPais;
                return RedirectToAction("Index", ViewBag.IdPais = model.IdPais);
            }
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(tb_region_Info model)
        {
            if (!bus_region.anularDB(model))
            {
                ViewBag.IdPais = model.IdPais;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais });
        }

        #endregion
    }

    public class tb_region_List
    {
        string Variable = "tb_region_Info";
        public List<tb_region_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_region_Info> list = new List<tb_region_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_region_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_region_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}