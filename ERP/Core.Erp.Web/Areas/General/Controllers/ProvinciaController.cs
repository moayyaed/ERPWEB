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
    public class ProvinciaController : Controller
    {
        #region Index/ Metodo

        tb_provincia_Bus bus_provincia = new tb_provincia_Bus();
        tb_pais_Bus bus_pais = new tb_pais_Bus();
        tb_region_Bus bus_region = new tb_region_Bus();
        tb_provincia_List Lista_Provincia = new tb_provincia_List();
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

            tb_provincia_Info model = new tb_provincia_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdPais = IdPais
            };

            var lst = bus_provincia.get_list(model.IdPais, true);
            Lista_Provincia.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_provincia(string IdPais = "", bool Nuevo = false)
        {
            ViewBag.IdPais = IdPais;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Provincia.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_provincia", model);
        }

        private void cargar_combos(string IdPais)
        {
            
            var lst_pais = bus_pais.get_list(false);
            ViewBag.lst_pais = lst_pais;
            var lst_region = bus_region.get_list(IdPais,false);
            ViewBag.lst_region = lst_region;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(string IdPais)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            tb_provincia_Info model = new tb_provincia_Info
            {
                IdPais = IdPais
            };
            ViewBag.IdPais = IdPais;
            cargar_combos(IdPais);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(tb_provincia_Info model)
        {
            if (!bus_provincia.guardarDB(model))
            {
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model.IdPais);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais });
        }

        public ActionResult Consultar(string IdProvincia = "", bool Exito=false)
        {
            tb_provincia_Info model = bus_provincia.get_info(IdProvincia);
            if (model == null)
            {
                ViewBag.IdPais = model.IdPais;
                return RedirectToAction("Index", new { IdPais = model.IdPais });
            }
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
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

            cargar_combos(model.IdPais);
            return View(model);
        }
        public ActionResult Modificar( string IdProvincia = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            tb_provincia_Info model = bus_provincia.get_info( IdProvincia);
            if (model == null)
            {
                ViewBag.IdPais = model.IdPais;
                return RedirectToAction("Index",new { IdPais = model.IdPais });
            }
            cargar_combos(model.IdPais);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(tb_provincia_Info model)
        {
            if (!bus_provincia.modificarDB(model))
            {
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model.IdPais);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais });
        }

        public ActionResult Anular( string IdProvincia = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            tb_provincia_Info model = bus_provincia.get_info( IdProvincia);
            if (model == null)
            {
                ViewBag.IdPais = model.IdPais;
                return RedirectToAction("Index", new { IdPais = model.IdPais });
            }
            cargar_combos(model.IdPais);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(tb_provincia_Info model)
        {
            if (!bus_provincia.anularDB(model))
            {
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model.IdPais);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais });
        }
        #endregion

        #region Json
        public JsonResult get_lst_provincia_pais(string IdPais)
        {
            try
            {
                List<tb_provincia_Info> lst_provincia =new List<tb_provincia_Info>();

                lst_provincia = bus_provincia.get_list(IdPais, true);
                return Json(lst_provincia, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }

    public class tb_provincia_List
    {
        string Variable = "tb_provincia_Info";
        public List<tb_provincia_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_provincia_Info> list = new List<tb_provincia_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_provincia_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_provincia_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}