using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.General;
using Core.Erp.Bus.General;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.General.Controllers
{
    [SessionTimeout]
    public class CiudadController : Controller
    {
        #region Inex

        tb_ciudad_Bus bus_ciudad = new tb_ciudad_Bus();
        tb_provincia_Bus bus_provincia = new tb_provincia_Bus();
        tb_ciudad_List Lista_Ciudad = new tb_ciudad_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index(string IdPais = "", string IdProvincia = "")
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

            tb_ciudad_Info model = new tb_ciudad_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdPais = IdPais, 
                IdProvincia = IdProvincia
            };

            var lst = bus_ciudad.get_list(model.IdProvincia, true);
            Lista_Ciudad.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        
        [ValidateInput(false)]
        public ActionResult GridViewPartial_Ciudad(string IdPais = "", string IdProvincia="", bool Nuevo = false)
        {
            ViewBag.IdPais = IdPais;
            ViewBag.IdProvincia = IdProvincia;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Ciudad.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_Ciudad", model);
        }
        private void cargar_combos(tb_ciudad_Info model)
        {
         var  lst_provincia = bus_provincia.get_list(model.IdPais, false);
            ViewBag.lst_provincia = lst_provincia;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(string IdPais, string IdProvincia)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            tb_ciudad_Info model = new tb_ciudad_Info
            {
                IdProvincia = IdProvincia,
                IdPais = IdPais
            };

            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(tb_ciudad_Info model)
        {
            if (!bus_ciudad.guardarDB(model))
            {
                ViewBag.IdProvincia = model.IdProvincia;
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia});
        }
        public ActionResult Consultar(string IdCiudad = "", bool Exito=false)
        {
            tb_ciudad_Info model = bus_ciudad.get_info(IdCiudad);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdProvincia = model.IdProvincia, IdPais = model.IdPais, });
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

            var info_provincia = bus_provincia.get_info(model.IdProvincia);
            model.IdPais = (info_provincia==null ? null : info_provincia.IdPais);

            ViewBag.IdProvincia = model.IdProvincia;
            ViewBag.IdPais = model.IdPais;

            cargar_combos(model);
            return View(model);
        }
        public ActionResult Modificar( string IdCiudad="")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            tb_ciudad_Info model = bus_ciudad.get_info( IdCiudad);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia });
            }

            var info_provincia = bus_provincia.get_info(model.IdProvincia);
            model.IdPais = (info_provincia == null ? null : info_provincia.IdPais);

            ViewBag.IdProvincia = model.IdProvincia;
            ViewBag.IdPais = model.IdPais;

            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(tb_ciudad_Info model)
        {
            if (!bus_ciudad.modificarDB(model))
            {
                ViewBag.IdProvincia = model.IdProvincia;
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia });
        }

        public ActionResult Anular( string IdCiudad="")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            tb_ciudad_Info model = bus_ciudad.get_info( IdCiudad);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdProvincia = model.IdProvincia, IdPais = model.IdPais, });
            }

            var info_provincia = bus_provincia.get_info(model.IdProvincia);
            model.IdPais = (info_provincia == null ? null : info_provincia.IdPais);

            ViewBag.IdProvincia = model.IdProvincia;
            ViewBag.IdPais = model.IdPais;

            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(tb_ciudad_Info model)
        {
            if (!bus_ciudad.anularDB(model))
            {
                ViewBag.IdProvincia = model.IdProvincia;
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia });
        }

        public JsonResult get_lst_ciudad_x_provincia(string IdProvincia)
        {
            try
            {
                List<tb_ciudad_Info> lst_ciudad = new List<tb_ciudad_Info>();
                lst_ciudad = bus_ciudad.get_list(IdProvincia, true);
                return Json(lst_ciudad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }

    public class tb_ciudad_List
    {
        string Variable = "tb_ciudad_Info";
        public List<tb_ciudad_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_ciudad_Info> list = new List<tb_ciudad_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_ciudad_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_ciudad_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}