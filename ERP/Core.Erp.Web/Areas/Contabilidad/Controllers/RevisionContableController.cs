using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.Facturacion;
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
    public class RevisionContableController : Controller
    {

        #region Variables
        ct_RevisionContable_Bus bus_revision = new ct_RevisionContable_Bus();
        ct_RevisionContableFacturas_List List_facturas = new ct_RevisionContableFacturas_List();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        ct_RevisionContableFacturas_Index_List Lista_RevisionContable = new ct_RevisionContableFacturas_Index_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "RevisionContable", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };

            var lst = bus_revision.get_list_facturas(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            List_facturas.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "RevisionContable", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            var lst = bus_revision.get_list_facturas(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            List_facturas.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        #endregion

        #region Facturacion
        [ValidateInput(false)]
        public ActionResult GridViewPartial_Facturacion(bool Nuevo= false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_facturas.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_Facturacion", model);
        }
        #endregion

        #region json
        public JsonResult ContabilizarFactura(decimal IdSecuencia = 0, decimal IdTransaccionSession = 0)
        {
            string resultado = string.Empty;

            var factura = List_facturas.get_list(IdTransaccionSession).FirstOrDefault(q => q.IdSecuencia == IdSecuencia);
            if (factura != null)
            {
                if (bus_factura.Contabilizar(factura.IdEmpresa, factura.IdSucursal, factura.IdBodega, factura.IdCbteVta, factura.Nombres))
                    resultado = "Contabilización exitosa";
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContabilizarTodasFacturas(DateTime FechaIni, DateTime FechaFin, decimal IdTransaccionSession = 0, int IdEmpresa = 0)
        {
            string resultado = string.Empty;
            var Lista = List_facturas.get_list(IdTransaccionSession);
            foreach (var factura in Lista)
            {
                bus_factura.Contabilizar(factura.IdEmpresa, factura.IdSucursal, factura.IdBodega, factura.IdCbteVta, factura.Nombres);
            }
            List_facturas.set_list(bus_revision.get_list_facturas(IdEmpresa, FechaIni, FechaFin), IdTransaccionSession);
            resultado = "Contabilización exitosa";            

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }

    public class ct_RevisionContableFacturas_Index_List
    {
        string Variable = "ct_RevisionContableFacturas_Info";
        public List<ct_RevisionContableFacturas_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ct_RevisionContableFacturas_Info> list = new List<ct_RevisionContableFacturas_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ct_RevisionContableFacturas_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ct_RevisionContableFacturas_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class ct_RevisionContableFacturas_List
    {
        string variable = "ct_RevisionContableFacturas";
        public List<ct_RevisionContableFacturas_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<ct_RevisionContableFacturas_Info> list = new List<ct_RevisionContableFacturas_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ct_RevisionContableFacturas_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ct_RevisionContableFacturas_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ct_RevisionContableFacturas_Info info_det, decimal IdTransaccionSession)
        {
            List<ct_RevisionContableFacturas_Info> list = get_list(IdTransaccionSession);
            info_det.IdSecuencia = list.Count == 0 ? 1 : list.Max(q => q.IdSecuencia) + 1;
            list.Add(info_det);
        }

        public void UpdateRow(ct_RevisionContableFacturas_Info info_det, decimal IdTransaccionSession)
        {
            ct_RevisionContableFacturas_Info edited_info = get_list(IdTransaccionSession).Where(m => m.IdSecuencia == info_det.IdSecuencia).First();
        }

        public void DeleteRow(int secuencia, decimal IdTransaccionSession)
        {
            List<ct_RevisionContableFacturas_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.IdSecuencia == secuencia).First());
        }
    }
}