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
    public class ReversoMovimientoInventarioController : Controller
    {
        #region Variables
        in_Ing_Egr_Inven_Bus bus_ing_inv = new in_Ing_Egr_Inven_Bus();
        #endregion

        #region Vistas
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal)
            };
            CargarCombosConsulta(model.IdEmpresa);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            CargarCombosConsulta(model.IdEmpresa);
            return View(model);
        }

        private void CargarCombosConsulta(int IdEmpresa)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_movimiento_inventario_x_reversar(DateTime? fecha_ini, DateTime? fecha_fin, int IdSucursal = 0)
        {
            ViewBag.fecha_ini = fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : fecha_ini;
            ViewBag.fecha_fin = fecha_fin == null ? DateTime.Now.Date : fecha_fin;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            List<in_Ing_Egr_Inven_Info> model = bus_ing_inv.get_list_x_reversar(IdEmpresa, IdSucursal, true, ViewBag.fecha_ini, ViewBag.fecha_fin);

            return PartialView("_GridViewPartial_movimiento_inventario_x_reversar", model);
        }
        #endregion

        #region Json
        public JsonResult ReversarMovimiento(string SecuencialID = "")
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
                if (bus_ing_inv.reversarDB(model))
                    resultado = "Reverso exitoso";
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}