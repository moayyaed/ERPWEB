using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    public class AprobacionMovimientoInventarioController : Controller
    {
        #region Variables
        in_Ing_Egr_Inven_Bus bus_ing_inv = new in_Ing_Egr_Inven_Bus();
        in_Ing_Egr_Inven_Aprobacion_List Lista_AprobacionInventario = new in_Ing_Egr_Inven_Aprobacion_List();
        #endregion

        #region Vistas
        public ActionResult Index(int IdSucursal = 0, int IdBodega = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = IdSucursal == 0 ? Convert.ToInt32(SessionFixed.IdSucursal) : IdSucursal,
                IdBodega = IdBodega,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date,
            };

            CargarCombosConsulta(model);
            var lst = bus_ing_inv.get_list_x_aprobar(model.IdEmpresa, model.IdSucursal, model.IdBodega);
            Lista_AprobacionInventario.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            CargarCombosConsulta(model);
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_ing_inv.get_list_x_aprobar(model.IdEmpresa, model.IdSucursal, model.IdBodega);
            Lista_AprobacionInventario.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        private void CargarCombosConsulta(cl_filtros_Info model)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(model.IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;

            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var lst_bodega = bus_bodega.get_list(model.IdEmpresa, model.IdSucursal, false);
            ViewBag.lst_bodega = lst_bodega;
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_movimiento_inventario_x_aprobar()
        {
            ////int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ////ViewBag.IdEmpresa = IdEmpresa;
            ////ViewBag.IdSucursal = IdSucursal;
            ////ViewBag.IdBodega = IdBodega;
            ////List<in_Ing_Egr_Inven_Info> model = bus_ing_inv.get_list_x_aprobar(IdEmpresa, IdSucursal, IdBodega);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_AprobacionInventario.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_movimiento_inventario_x_aprobar", model);
        }
        #endregion

        #region Json
        public JsonResult CargarBodega(int IdEmpresa = 0, int IdSucursal = 0)
        {
            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var resultado = bus_bodega.get_list(IdEmpresa, IdSucursal, false);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AprobarMovimiento(string SecuencialID = "")
        {
            string resultado = string.Empty;
            int IdEmpresa = Convert.ToInt32(SecuencialID.Substring(0, 2));
            int IdSucursal = Convert.ToInt32(SecuencialID.Substring(2, 2));
            int IdMovi_inven_tipo = Convert.ToInt32(SecuencialID.Substring(4, 2));
            int IdNumMovi = Convert.ToInt32(SecuencialID.Substring(6, 8));

            var model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdUsuarioAR = SessionFixed.IdUsuario;

            if (model != null)
            {
                if (bus_ing_inv.aprobarDB(model))
                    resultado = "Aprobación exitosa";
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class in_Ing_Egr_Inven_Aprobacion_List
    {
        string variable = "in_Ing_Egr_Inven_Aprobacion_Info";
        public List<in_Ing_Egr_Inven_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_Info> list = new List<in_Ing_Egr_Inven_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }
        public void set_list(List<in_Ing_Egr_Inven_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }
    }
}