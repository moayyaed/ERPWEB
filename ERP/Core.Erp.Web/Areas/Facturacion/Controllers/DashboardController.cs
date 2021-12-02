using Core.Erp.Bus.Caja;
using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class DashboardController : Controller
    {
        #region Variables
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_VentasClientes_List Lista_VentasClientes = new fa_VentasClientes_List();
        cxc_cobro_Bus bus_cobros = new cxc_cobro_Bus();
        caj_Caja_Bus bus_caja = new caj_Caja_Bus();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Dashboard", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                fecha_ini = Convert.ToDateTime("01-01-" + DateTime.Now.Year),
                fecha_fin = Convert.ToDateTime("31-12-" + DateTime.Now.Year),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            return View(model);
        }
        #endregion

        #region JSON
        public JsonResult UltimasVentasAnio(int IdEmpresa=0)
        {
            var lst_UltimasVentasAnio = bus_factura.get_list_UltimasVentasAnio(IdEmpresa);
            return Json(lst_UltimasVentasAnio, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult UltimasVentasMeses(int IdEmpresa = 0)
        {
            var lst_UltimasVentasAnio = bus_factura.get_list_UltimasVentasMeses(IdEmpresa);
            return Json(lst_UltimasVentasAnio, JsonRequestBehavior.AllowGet);
        }
        public JsonResult VentasClientes( DateTime FechaIni, DateTime FechaFin, int IdEmpresa = 0)
        {
            var lst_VentasClientes = bus_factura.get_list_VentasClientes(IdEmpresa, FechaIni, FechaFin);
            return Json(lst_VentasClientes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VentasClientesListado(DateTime FechaIni, DateTime FechaFin, int IdEmpresa = 0, decimal IdTransaccionSession=0)
        {
            var lst_VentasClientesListado = bus_factura.get_list_VentasClientesListado(IdEmpresa, FechaIni, FechaFin);
            return Json(lst_VentasClientesListado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult VentasProductos(DateTime FechaIni, DateTime FechaFin, int IdEmpresa = 0)
        {
            var lst_VentasClientes = bus_factura.get_list_VentasProductos(IdEmpresa, FechaIni, FechaFin);
            return Json(lst_VentasClientes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VentasProductosListado(DateTime FechaIni, DateTime FechaFin, int IdEmpresa = 0, decimal IdTransaccionSession = 0)
        {
            var lst_VentasClientesListado = bus_factura.get_list_VentasProductosListado(IdEmpresa, FechaIni, FechaFin);
            return Json(lst_VentasClientesListado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistroDia(int IdEmpresa = 0, decimal IdTransaccionSession = 0)
        {
            var Fecha = DateTime.Now.Date;
            var info_Facturacion = bus_factura.FacturadoPorDia(IdEmpresa, Fecha.Date);
            var info_Cobros = bus_cobros.CobrosPorDia(IdEmpresa, Fecha.Date);
            var info_Caja = new fa_Dashboard_Info();
            
            return Json(new { Fecha=Fecha.ToString("dd-MM-yyyy"), Facturado = info_Facturacion.Total_String, Cobrado = info_Cobros.Total_String, Caja = info_Caja.Total_String }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class fa_VentasClientes_List
    {
        string Variable = "fa_Dashboard_VentasClientes_Info";
        public List<fa_Dashboard_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_Dashboard_Info> list = new List<fa_Dashboard_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_Dashboard_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_Dashboard_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}