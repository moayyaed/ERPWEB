using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    public class ParametrizacionContableProductoController : Controller
    {
        #region Variables
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        in_producto_x_tb_bodega_Bus bus_producto_x_tbbodega = new in_producto_x_tb_bodega_Bus();
        in_producto_x_tb_bodega_Info_List Lis_in_producto_x_tb_bodega_Info_List = new in_producto_x_tb_bodega_Info_List();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        #endregion

        #region Metodos ComboBox bajo demanda

        public ActionResult CmbCuenta()
        {
            in_producto_x_tb_bodega_Info model = new in_producto_x_tb_bodega_Info();
            return PartialView("_CmbCuenta", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos ComboBox bajo demanda CtaInven
        public ActionResult CmbCuenta_Inven()
        {
            in_producto_x_tb_bodega_Info model = new in_producto_x_tb_bodega_Info();
            return PartialView("_CmbCuenta_Inven", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda_cta_inven(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda_cta_inven(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos ComboBox bajo demanda CtaInven
        public ActionResult CmbCuenta_Vta()
        {
            in_producto_x_tb_bodega_Info model = new in_producto_x_tb_bodega_Info();
            return PartialView("_CmbCuenta_Vta", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda_cta_vta(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda_cta_vta(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Vistas
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdBodega = 0
            };

            CargarCombosConsulta(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            CargarCombosConsulta(model);
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
        public ActionResult GridViewPartial_ParametrizacionContableProducto(int IdSucursal = 0, int IdBodega = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            ViewBag.IdBodega = IdBodega;

            List<in_producto_x_tb_bodega_Info> model = bus_producto_x_tbbodega.get_list_x_bodega(IdEmpresa, IdSucursal, IdBodega);
            Lis_in_producto_x_tb_bodega_Info_List.set_list(model, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_ParametrizacionContableProducto", model);
        }
        #endregion

        #region Json
        public JsonResult CargarBodega(int IdEmpresa = 0, int IdSucursal = 0)
        {
            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var resultado = bus_bodega.get_list(IdEmpresa, IdSucursal, false);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] in_producto_x_tb_bodega_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst = Lis_in_producto_x_tb_bodega_Info_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).ToList();
            in_producto_x_tb_bodega_Info edited_info = lst.Where(m => m.Secuencia == info_det.Secuencia).FirstOrDefault();

            if (edited_info != null)
            {
                var suc = bus_sucursal.get_info(IdEmpresa, edited_info.IdSucursal);
                var bod = bus_bodega.get_info(IdEmpresa, edited_info.IdSucursal, edited_info.IdBodega);
                var cta = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble_Costo);
                var cta_inven = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble_Inven);
                var cta_vta = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble_Vta);

                if (suc != null && bod != null)
                {
                    info_det.IdSucursal = edited_info.IdSucursal;
                    info_det.Su_Descripcion = suc.Su_Descripcion;
                    info_det.IdBodega = edited_info.IdBodega;
                    info_det.bo_Descripcion = bod.bo_Descripcion;
                }
                if (cta != null)
                {
                    edited_info.IdCtaCble_Costo = info_det.IdCtaCble_Costo;
                    edited_info.pc_Cuenta = cta.IdCtaCble + " - " + cta.pc_Cuenta;
                    info_det.pc_Cuenta = cta.IdCtaCble + " - " + cta.pc_Cuenta;
                }else
                {
                    edited_info.IdCtaCble_Costo = null;
                    edited_info.pc_Cuenta = null;
                    info_det.pc_Cuenta = null;
                }

                if (cta_inven != null)
                {
                    edited_info.IdCtaCble_Inven = info_det.IdCtaCble_Inven;
                    edited_info.pc_Cuenta_inven = cta_inven.IdCtaCble + " - " + cta_inven.pc_Cuenta;
                    info_det.pc_Cuenta_inven = cta_inven.IdCtaCble + " - " + cta_inven.pc_Cuenta;
                }else
                {
                    edited_info.IdCtaCble_Inven = null;
                    edited_info.pc_Cuenta_inven = null;
                    info_det.pc_Cuenta_inven = null;
                }

                if (cta_vta != null)
                {
                    edited_info.IdCtaCble_Vta = info_det.IdCtaCble_Vta;
                    edited_info.pc_Cuenta_Vta = cta_vta.IdCtaCble + " - " + cta_vta.pc_Cuenta;
                    info_det.pc_Cuenta_Vta = cta_vta.IdCtaCble + " - " + cta_vta.pc_Cuenta;
                }else
                {
                    edited_info.IdCtaCble_Vta = null;
                    edited_info.pc_Cuenta_Vta = null;
                    info_det.pc_Cuenta_Vta = null;
                }

                bus_producto_x_tbbodega.modificarDB(edited_info);
            }

            List<in_producto_x_tb_bodega_Info> model = Lis_in_producto_x_tb_bodega_Info_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_ParametrizacionContableProducto", model);
        }

    }
}