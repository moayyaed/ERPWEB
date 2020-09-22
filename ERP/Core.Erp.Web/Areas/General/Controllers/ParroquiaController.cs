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
    public class ParroquiaController : Controller
    {
        #region Index / Metodos

        tb_parroquia_Bus bus_parroquia = new tb_parroquia_Bus();
        tb_ciudad_Bus bus_ciudad = new tb_ciudad_Bus();
        tb_parroquia_Info_List Lista_Parroquia = new tb_parroquia_Info_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        tb_provincia_Bus bus_provincia = new tb_provincia_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index(string IdPais="", string IdProvincia = "",string IdCiudad = "")
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

            tb_parroquia_Info model = new tb_parroquia_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdPais = IdPais,
                IdProvincia = IdProvincia,
                IdCiudad_Canton = IdCiudad
            };

            var lst = bus_parroquia.get_list(model.IdCiudad_Canton, true);
            Lista_Parroquia.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        
        [ValidateInput(false)]
        public ActionResult GridViewPartial_parroquia(string IdPais = "", string IdProvincia = "", string IdCiudad = "", bool Nuevo = false)
        {
            ViewBag.IdPais = IdPais;
            ViewBag.IdProvincia = IdProvincia;
            ViewBag.IdCiudad_Canton = IdCiudad;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Parroquia.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_parroquia", model);
        }
        private void cargar_combos(tb_parroquia_Info model)
        {
            var lst_ciudades = bus_ciudad.get_list(model.IdProvincia,false);
            ViewBag.lst_ciudades = lst_ciudades;
        }
        #endregion
        #region Acciones
        public ActionResult Nuevo(string IdPais = "", string IdProvincia = "", string IdCiudad = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            tb_parroquia_Info model = new tb_parroquia_Info
            {
                IdPais=IdPais,
                IdProvincia=IdProvincia,
                IdCiudad_Canton = IdCiudad
            };
            ViewBag.IdCiudad = model.IdCiudad_Canton;
            ViewBag.IdProvincia = model.IdProvincia;
            ViewBag.IdPais = model.IdPais;
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(tb_parroquia_Info model)
        {
            if (!bus_parroquia.guardarDB(model))
            {
                ViewBag.IdCiudad = model.IdCiudad_Canton;
                ViewBag.IdProvincia = model.IdProvincia;
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia, IdCiudad = model.IdCiudad_Canton } );
        }

        private ActionResult RedirectToAction(string v, dynamic dynamic, object p1, object p2)
        {
            throw new NotImplementedException();
        }
        public ActionResult Consultar(string IdParroquia = "", bool Exito=false)
        {
            tb_parroquia_Info model = bus_parroquia.get_info(IdParroquia);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia, IdCiudad = model.IdCiudad_Canton });
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

            var info_ciudad = bus_ciudad.get_info(model.IdCiudad_Canton);
            model.IdProvincia = (info_ciudad == null ? null : info_ciudad.IdProvincia);
            var info_provincia = bus_provincia.get_info(model.IdProvincia);
            model.IdPais = (info_provincia == null ? null : info_provincia.IdPais);

            ViewBag.IdCiudad = model.IdCiudad_Canton;
            ViewBag.IdProvincia = model.IdProvincia;
            ViewBag.IdPais = model.IdPais;

            cargar_combos(model);
            return View(model);
        }
        public ActionResult Modificar( string IdParroquia = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            tb_parroquia_Info model = bus_parroquia.get_info( IdParroquia);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia, IdCiudad = model.IdCiudad_Canton });
            }

            var info_ciudad = bus_ciudad.get_info(model.IdCiudad_Canton);
            model.IdProvincia = (info_ciudad == null ? null : info_ciudad.IdProvincia);
            var info_provincia = bus_provincia.get_info(model.IdProvincia);
            model.IdPais = (info_provincia == null ? null : info_provincia.IdPais);

            ViewBag.IdCiudad = model.IdCiudad_Canton;
            ViewBag.IdProvincia = model.IdProvincia;
            ViewBag.IdPais = model.IdPais;

            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(tb_parroquia_Info model)
        {
            if (!bus_parroquia.modificarDB(model))
            {
                ViewBag.IdCiudad = model.IdCiudad_Canton;
                ViewBag.IdProvincia = model.IdProvincia;
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia, IdCiudad = model.IdCiudad_Canton });
        }


        public ActionResult Anular( string IdParroquia = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            tb_parroquia_Info model = bus_parroquia.get_info( IdParroquia);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia, IdCiudad = model.IdCiudad_Canton });
            }

            var info_ciudad = bus_ciudad.get_info(model.IdCiudad_Canton);
            model.IdProvincia = (info_ciudad == null ? null : info_ciudad.IdProvincia);
            var info_provincia = bus_provincia.get_info(model.IdProvincia);
            model.IdPais = (info_provincia == null ? null : info_provincia.IdPais);

            ViewBag.IdCiudad = model.IdCiudad_Canton;
            ViewBag.IdProvincia = model.IdProvincia;
            ViewBag.IdPais = model.IdPais;

            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(tb_parroquia_Info model)
        {
            if (!bus_parroquia.anularDB(model))
            {
                ViewBag.IdCiudad = model.IdCiudad_Canton;
                ViewBag.IdProvincia = model.IdProvincia;
                ViewBag.IdPais = model.IdPais;
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index", new { IdPais = model.IdPais, IdProvincia = model.IdProvincia, IdCiudad = model.IdCiudad_Canton });
        }

        #endregion

        public JsonResult get_lst_ciudad_x_provincia(string IdCiudad)
        {
            try
            {
               var lst_parroquia = bus_parroquia.get_list(IdCiudad, true);
                return Json(lst_parroquia, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
               throw;
            }
        }
    }

    public class tb_parroquia_Info_List
    {
        string Variable = "tb_parroquia_Lista_Info";
        public List<tb_parroquia_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_parroquia_Info> list = new List<tb_parroquia_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_parroquia_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_parroquia_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class tb_parroquia_List
    {
        string Variable = "tb_parroquia_Info";
        public List<tb_parroquia_Info> get_list()
        {
            if (HttpContext.Current.Session[Variable] == null)
            {
                List<tb_parroquia_Info> list = new List<tb_parroquia_Info>();

                HttpContext.Current.Session[Variable] = list;
            }
            return (List<tb_parroquia_Info>)HttpContext.Current.Session[Variable];
        }

        public void set_list(List<tb_parroquia_Info> list)
        {
            HttpContext.Current.Session[Variable] = list;
        }
    }
}