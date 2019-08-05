using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Web.Helps;
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
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            ViewBag.IdBodega = IdBodega;

            List<in_Producto_Info> model = bus_producto.get_list();

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
        public ActionResult EditingUpdate_pro_x_bod([ModelBinder(typeof(DevExpressEditorsBinder))] in_producto_x_tb_bodega_Info info_det)
        {
            in_Producto_Info model = new in_Producto_Info();
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);

            if (info_det != null)
            {
                var suc = bus_sucursal.get_info(IdEmpresa, info_det.IdSucursal);

                info_det.IdBodega = string.IsNullOrEmpty(info_det.IdString) ? 0 : Convert.ToInt32(info_det.IdString.Substring(3, 3));

                var bod = bus_bodega.get_info(IdEmpresa, info_det.IdSucursal, info_det.IdBodega);
                if (suc != null && bod != null)
                {
                    info_det.IdSucursal = info_det.IdSucursal;
                    info_det.Su_Descripcion = suc.Su_Descripcion;
                    info_det.IdBodega = info_det.IdBodega;
                    info_det.bo_Descripcion = bod.bo_Descripcion;
                }
            }

            if (ModelState.IsValid)
            {
                Lis_in_producto_x_tb_bodega_Info_List.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            }
            model.lst_producto_x_bodega = Lis_in_producto_x_tb_bodega_Info_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ParametrizacionContableProducto", model);
        }

    }
}