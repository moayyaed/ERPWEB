using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.General;
using Core.Erp.Web.Reportes.Inventario;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class RevisionController : Controller
    {
        #region Variables
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
        in_Marca_Bus bus_marca = new in_Marca_Bus();

        #endregion
        #region ProductoPadre
        public ActionResult CmbProductoPadre_Revision()
        {
            decimal model = new decimal();
            return PartialView("_CmbProductoPadre_Revision", model);
        }

        public List<in_Producto_Info> get_list_ProductoPadre_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_producto.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoBusquedaProducto.SOLOPADRES, cl_enumeradores.eModulo.INV, 0,0);
        }
        public in_Producto_Info get_info_producto_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_producto.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion
        private void cargar_combos(int IdEmpresa)
        {
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_bodega = bus_bodega.get_list(IdEmpresa, false);
            ViewBag.lst_bodega = lst_bodega;

            var lst_marca = bus_marca.get_list(IdEmpresa, false);
            lst_marca.Add(new in_Marca_Info
            {
                IdEmpresa = IdEmpresa,
                IdMarca = 0,
                Descripcion = "Todas"
            });
            ViewBag.lst_marca = lst_marca;

        }
        public JsonResult cargar_bodega(int IdEmpresa = 0, int IdSucursal = 0)
        {
            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var resultado = bus_bodega.get_list(IdEmpresa, IdSucursal, false);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index( )
        {
            cl_filtros_inventario_Info model = new cl_filtros_inventario_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdProductoPadre = 0,
                IdMarca = 0,
                dIAS = 40
            };
            INV_004_Rpt report = new INV_004_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdBodega.Value = model.IdBodega;
            report.p_IdProducto.Value = model.IdProductoPadre == null ? 0 : model.IdProductoPadre;
            report.p_IdMarca.Value = model.IdMarca;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa.ToString();
            cargar_combos(model.IdEmpresa);
            ViewBag.Report_s = report;

            INV_012_Rpt report_list = new INV_012_Rpt();
            report_list.p_IdEmpresa.Value = model.IdEmpresa;
            report_list.p_IdSucursal.Value = model.IdSucursal;
            report_list.p_IdBodega.Value = model.IdBodega;
            report_list.p_IdProducto.Value = model.IdProductoPadre == null ? 0 : model.IdProductoPadre;
            report_list.p_IdMarca.Value = model.IdMarca;
            report_list.p_fechaIni.Value = model.fecha_fin;
            report_list.p_dIAS.Value = model.dIAS;
            report_list.usuario = SessionFixed.IdUsuario.ToString();
            report_list.empresa = SessionFixed.NomEmpresa.ToString();
            cargar_combos(model.IdEmpresa);
            ViewBag.Report_l = report_list;
            return View(model);
            
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_inventario_Info model)
        {
            INV_004_Rpt report = new INV_004_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdBodega.Value = model.IdBodega;
            report.p_IdProducto.Value = model.IdProductoPadre == null ? 0 : model.IdProductoPadre;
            report.p_IdMarca.Value = model.IdMarca;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa.ToString();
            cargar_combos(model.IdEmpresa);
            ViewBag.Report_s = report;

            INV_012_Rpt report_list = new INV_012_Rpt();
            report_list.p_IdEmpresa.Value = model.IdEmpresa;
            report_list.p_IdSucursal.Value = model.IdSucursal;
            report_list.p_IdBodega.Value = model.IdBodega;
            report_list.p_IdProducto.Value = model.IdProductoPadre == null ? 0 : model.IdProductoPadre;
            report_list.p_IdMarca.Value = model.IdMarca;
            report_list.p_fechaIni.Value = model.fecha_fin;
            report_list.p_dIAS.Value = model.dIAS;
            report_list.usuario = SessionFixed.IdUsuario.ToString();
            report_list.empresa = SessionFixed.NomEmpresa.ToString();
            cargar_combos(model.IdEmpresa);
            ViewBag.Report_l = report_list;
            return View(model);
        }
    }
}