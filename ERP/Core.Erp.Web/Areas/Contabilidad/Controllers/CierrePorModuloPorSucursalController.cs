using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class CierrePorModuloPorSucursalController : Controller
    {
        // GET: Contabilidad/CierrePorModuloPorSucursal
        #region Variables
        ct_CierrePorModuloPorSucursal_Bus bus_CierreModulo = new ct_CierrePorModuloPorSucursal_Bus();
        tb_sucursal_Bus bus_Sucursal = new tb_sucursal_Bus();
        tb_modulo_Bus bus_modulo = new tb_modulo_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        ct_CierrePorModuloPorSucursal_List Lista_CierreModulo = new ct_CierrePorModuloPorSucursal_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CierrePorModuloPorSucursal", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };

            cargar_filtros(model.IdEmpresa);
            var lst = bus_CierreModulo.GetList(model.IdEmpresa, model.IdSucursal, true);
            Lista_CierreModulo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            cargar_filtros(model.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_CierreModulo.GetList(model.IdEmpresa, model.IdSucursal, true);
            Lista_CierreModulo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_CierrePorModuloPorSucursal(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //ViewBag.IdSucursal = IdSucursal == 0 ? 0 : Convert.ToInt32(IdSucursal);
            //List<ct_CierrePorModuloPorSucursal_Info> model = bus_CierreModulo.GetList(IdEmpresa, IdSucursal, true);

            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CierreModulo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_CierrePorModuloPorSucursal", model);
        }
        #endregion

        #region Metodos
        private void cargar_filtros(int IdEmpresa)
        {
            try
            {
                var lst_Sucursal = bus_Sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
                ViewBag.lst_Sucursal = lst_Sucursal;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void cargar_combos(int IdEmpresa)
        {
            try
            {
                var lst_Sucursal = bus_Sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, false);
                ViewBag.lst_Sucursal = lst_Sucursal;

                var lst_Modulo = bus_modulo.get_list();
                ViewBag.lst_Modulo = lst_Modulo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CierrePorModuloPorSucursal", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_CierrePorModuloPorSucursal_Info model = new ct_CierrePorModuloPorSucursal_Info();
            model.FechaIni = DateTime.Now;
            model.FechaFin = DateTime.Now;
            cargar_combos(IdEmpresa);
            return View(model);

        }

        [HttpPost]
        public ActionResult Nuevo(ct_CierrePorModuloPorSucursal_Info model)
        {
            if (!bus_CierreModulo.GuardarBD(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdCierre = model.IdCierre, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdCierre = 0, bool Exito = false)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CierrePorModuloPorSucursal", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_CierrePorModuloPorSucursal_Info model = bus_CierreModulo.GetInfo(IdEmpresa, IdCierre);
            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdCierre = 0, bool Exito=false)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CierrePorModuloPorSucursal", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ct_CierrePorModuloPorSucursal_Info model = bus_CierreModulo.GetInfo(IdEmpresa, IdCierre);
            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ct_CierrePorModuloPorSucursal_Info model)
        {
            if (!bus_CierreModulo.ModificarBD(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdCierre = model.IdCierre, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdCierre = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CierrePorModuloPorSucursal", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_CierrePorModuloPorSucursal_Info model = bus_CierreModulo.GetInfo(IdEmpresa, IdCierre);
            if (model == null)
                return RedirectToAction("Index");

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(ct_CierrePorModuloPorSucursal_Info model)
        {
            if (!bus_CierreModulo.AnularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class ct_CierrePorModuloPorSucursal_List
    {
        string Variable = "ct_CierrePorModuloPorSucursal_Info";
        public List<ct_CierrePorModuloPorSucursal_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ct_CierrePorModuloPorSucursal_Info> list = new List<ct_CierrePorModuloPorSucursal_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ct_CierrePorModuloPorSucursal_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ct_CierrePorModuloPorSucursal_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}