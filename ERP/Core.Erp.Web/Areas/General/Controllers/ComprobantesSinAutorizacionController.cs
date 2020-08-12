using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.General;
using Core.Erp.Bus.General;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Bus.SeguridadAcceso;

namespace Core.Erp.Web.Areas.General.Controllers
{
    [SessionTimeout]

    public class ComprobantesSinAutorizacionController : Controller
    {
        tb_comprobantes_sin_autorizacion_Bus bus_comprobantes = new tb_comprobantes_sin_autorizacion_Bus();
        tb_comprobantes_sin_autorizacion_List Lis_tb_comprobantes_sin_autorizacion_List = new tb_comprobantes_sin_autorizacion_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();

        #region vistas 
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CierreAnual", "Index");
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
            CargarCombos(model.IdEmpresa);
            var lst = bus_comprobantes.get_list(model.IdEmpresa, "", model.fecha_ini, model.fecha_fin, model.IdSucursal);
            Lis_tb_comprobantes_sin_autorizacion_List.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        private void CargarCombos(int IdEmpresa)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            CargarCombos(model.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_comprobantes.get_list(model.IdEmpresa, "", model.fecha_ini, model.fecha_fin, model.IdSucursal);
            Lis_tb_comprobantes_sin_autorizacion_List.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_documentos_pendientes_enviar_sri()
        {
            //ViewBag.IdEmpresa = IdEmpresa;
            //ViewBag.IdSucursal = IdSucursal;
            //ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            //ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);

            //var model = bus_comprobantes.get_list(IdEmpresa,"", ViewBag.Fecha_ini, ViewBag.Fecha_fin,IdSucursal);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lis_tb_comprobantes_sin_autorizacion_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_documentos_pendientes_enviar_sri", model);
        }

        #endregion

        #region json
        public JsonResult guardar_aprobacion(string Ids, decimal IdTransaccionSession=0)
        {
            string[] array = Ids.Split(',');
            var output = array.GroupBy(q => q).ToList();
            foreach (var item in output)
            {
                if (item.Key != "")
                {
                    var info = Lis_tb_comprobantes_sin_autorizacion_List.get_list(IdTransaccionSession).Where(v => v.secuencia == Convert.ToDecimal(item.Key)).FirstOrDefault();
                    if(info!=null)
                    bus_comprobantes.modificarEstado(info);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
    public class tb_comprobantes_sin_autorizacion_List
    {
        string Variable = "tb_comprobantes_sin_autorizacion_List";

        public List<tb_comprobantes_sin_autorizacion_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_comprobantes_sin_autorizacion_Info> list = new List<tb_comprobantes_sin_autorizacion_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_comprobantes_sin_autorizacion_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_comprobantes_sin_autorizacion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

    }
}