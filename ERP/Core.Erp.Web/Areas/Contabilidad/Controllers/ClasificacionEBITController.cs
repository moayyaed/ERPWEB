using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class ClasificacionEBITController : Controller
    {
        #region Variables
        ct_ClasificacionEBIT_Bus bus_clasificacion = new ct_ClasificacionEBIT_Bus();
        ct_ClasificacionEBIT_List Lista_ClasificacionEBIT = new ct_ClasificacionEBIT_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        #endregion
        #region Index
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ClasificacionEBIT", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_ClasificacionEBIT_Info model = new ct_ClasificacionEBIT_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            var lst = bus_clasificacion.GetList();
            Lista_ClasificacionEBIT.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ClasificacionEBIT(bool Nuevo=false)
        {
            //List<ct_ClasificacionEBIT_Info> model = new List<ct_ClasificacionEBIT_Info>();
            //model = bus_clasificacion.GetList();
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_ClasificacionEBIT.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ClasificacionEBIT", model);
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ClasificacionEBIT", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_ClasificacionEBIT_Info model = new ct_ClasificacionEBIT_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ct_ClasificacionEBIT_Info model)
        {
            if (!bus_clasificacion.GuardarBD(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdClasificacionEBIT = model.IdClasificacionEBIT, Exito = true });
        }

        public ActionResult Consultar(int IdClasificacionEBIT = 0, bool Exito=false)
        {
            ct_ClasificacionEBIT_Info model = bus_clasificacion.GetInfo(IdClasificacionEBIT);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ClasificacionEBIT", "Index");
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(int IdClasificacionEBIT = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ClasificacionEBIT", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ct_ClasificacionEBIT_Info model = bus_clasificacion.GetInfo(IdClasificacionEBIT);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_ClasificacionEBIT_Info model)
        {
            if (!bus_clasificacion.ModificarBD(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdClasificacionEBIT = model.IdClasificacionEBIT, Exito = true });
        }

        public ActionResult Anular(int IdClasificacionEBIT = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ClasificacionEBIT", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_ClasificacionEBIT_Info model = bus_clasificacion.GetInfo(IdClasificacionEBIT);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_ClasificacionEBIT_Info model)
        {
            if (!bus_clasificacion.AnularBD(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class ct_ClasificacionEBIT_List
    {
        string Variable = "ct_ClasificacionEBIT_Info";
        public List<ct_ClasificacionEBIT_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_ClasificacionEBIT_Info> list = new List<ct_ClasificacionEBIT_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_ClasificacionEBIT_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_ClasificacionEBIT_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}