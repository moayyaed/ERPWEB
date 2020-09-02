using Core.Erp.Bus.CuentasPorPagar;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.CuentasPorPagar;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorPagar.Controllers
{
    [SessionTimeout]
    public class TipoCodigoSRIController : Controller
    {
        #region Index
        cp_codigo_SRI_tipo_Bus bus_tipo_codigo = new cp_codigo_SRI_tipo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        cp_codigo_SRI_tipo_List Lista_Codigo = new cp_codigo_SRI_tipo_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cp_codigo_SRI_tipo_Info model = new cp_codigo_SRI_tipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_tipo_codigo.get_list(true);
            Lista_Codigo.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipo_codigo_sri(bool Nuevo=false)
        {
            //List<cp_codigo_SRI_tipo_Info> model = new List<cp_codigo_SRI_tipo_Info>();
            //model = bus_tipo_codigo.get_list(true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Codigo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_tipo_codigo_sri", model);
        }

        #endregion
        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cp_codigo_SRI_tipo_Info model = new cp_codigo_SRI_tipo_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cp_codigo_SRI_tipo_Info model)
        {
            if(bus_tipo_codigo.validar_existe_codigo_tipo(model.IdTipoSRI))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                return View(model);
            }

            if (!bus_tipo_codigo.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoSRI = model.IdTipoSRI, Exito = true });
        }

        public ActionResult Consultar(string IdTipoSRI = "", bool Exito=false)
        {
            cp_codigo_SRI_tipo_Info model = bus_tipo_codigo.get_info(IdTipoSRI);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
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

        public ActionResult Modificar(string IdTipoSRI = "")
        {
            cp_codigo_SRI_tipo_Info model = bus_tipo_codigo.get_info(IdTipoSRI);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cp_codigo_SRI_tipo_Info model)
        {
            if (!bus_tipo_codigo.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoSRI = model.IdTipoSRI, Exito = true });
        }

        public ActionResult Anular(string IdTipoSRI = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            cp_codigo_SRI_tipo_Info model = bus_tipo_codigo.get_info(IdTipoSRI);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cp_codigo_SRI_tipo_Info model)
        {
            if (!bus_tipo_codigo.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

    }

    public class cp_codigo_SRI_tipo_List
    {
        string Variable = "cp_codigo_SRI_tipo_Info";
        public List<cp_codigo_SRI_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cp_codigo_SRI_tipo_Info> list = new List<cp_codigo_SRI_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_codigo_SRI_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_codigo_SRI_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}