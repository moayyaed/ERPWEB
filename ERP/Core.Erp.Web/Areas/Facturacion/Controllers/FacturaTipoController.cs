using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.Facturacion;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Web.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Helps;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class FacturaTipoController : Controller
    {
        #region Variables
        fa_factura_tipo_Bus busTipo = new fa_factura_tipo_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_factura_tipo_List ListaConsulta = new fa_factura_tipo_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FacturaTipo", "Index");
            if (info != null)
            {
                ViewBag.Nuevo = info.Nuevo;
                ViewBag.Modificar = info.Modificar;
                ViewBag.Anular = info.Anular;
            }
            else
            {
                ViewBag.Nuevo = false;
                ViewBag.Modificar = false;
                ViewBag.Anular = false;
            }
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };
            var lst = busTipo.GetList(model.IdEmpresa,true);
            ListaConsulta.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_facturaTipo(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = ListaConsulta.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_facturaTipo", model);
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "TipoComprobante", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_factura_tipo_Info model = new fa_factura_tipo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_factura_tipo_Info model)
        {
            if (!busTipo.GuardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdFacturaTipo = model.IdFacturaTipo, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdFacturaTipo = 0, bool Exito = false)
        {
            fa_factura_tipo_Info model = busTipo.GetInfo(IdEmpresa, IdFacturaTipo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FacturaTipo", "Index");
            if (model.Estado == false)
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

        public ActionResult Modificar(int IdEmpresa = 0, int IdFacturaTipo = 0)
        {
            fa_factura_tipo_Info model = busTipo.GetInfo(IdEmpresa,IdFacturaTipo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FacturaTipo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(fa_factura_tipo_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (!busTipo.ModificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdFacturaTipo = model.IdFacturaTipo, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdFacturaTipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "FacturaTipo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_factura_tipo_Info model = busTipo.GetInfo(IdEmpresa,IdFacturaTipo);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(fa_factura_tipo_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (!busTipo.AnularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class fa_factura_tipo_List
    {
        string Variable = "fa_factura_tipo_List";
        public List<fa_factura_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_factura_tipo_Info> list = new List<fa_factura_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_factura_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_factura_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}