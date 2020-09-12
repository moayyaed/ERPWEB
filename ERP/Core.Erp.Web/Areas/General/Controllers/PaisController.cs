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
    public class PaisController : Controller
    {
        #region Index
        tb_pais_Bus bus_pais = new tb_pais_Bus();
        tb_pais_List Lista_Pais = new tb_pais_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index()
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

            tb_pais_Info model = new tb_pais_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_pais.get_list(true);
            Lista_Pais.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_pais(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Pais.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_pais", model);
        }

        #endregion
        #region Acciones

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            tb_pais_Info model = new tb_pais_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo (tb_pais_Info model)
        {
            if (!bus_pais.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdPais = model.IdPais, Exito = true });
        }

        public ActionResult Consultar(string IdPais = "", bool Exito=false)
        {
            tb_pais_Info model = bus_pais.get_info(IdPais);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (model.estado == "I")
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

            return View(model);
        }

        public ActionResult Modificar (string IdPais = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            tb_pais_Info model = bus_pais.get_info(IdPais);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar (tb_pais_Info model)
        {
            if(!bus_pais.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdPais = model.IdPais, Exito = true });
        }

        public ActionResult Anular(string IdPais)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Pais", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            tb_pais_Info model = bus_pais.get_info(IdPais);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(tb_pais_Info model)
        {
            if (!bus_pais.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class tb_pais_List
    {
        string Variable = "tb_pais_Info";
        public List<tb_pais_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_pais_Info> list = new List<tb_pais_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_pais_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_pais_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}