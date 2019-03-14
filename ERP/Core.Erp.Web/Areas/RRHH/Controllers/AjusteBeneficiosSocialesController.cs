using Core.Erp.Bus.General;
using Core.Erp.Bus.RRHH;
using Core.Erp.Info.Helps;
using Core.Erp.Info.RRHH;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class AjusteBeneficiosSocialesController : Controller
    {
        #region Variables
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        ro_nomina_tipo_Bus bus_nomina_tipo = new ro_nomina_tipo_Bus();
        ro_rol_detalle_x_rubro_acumulado_Bus bus_detalle_rubro_acumulado = new ro_rol_detalle_x_rubro_acumulado_Bus();
        #endregion

        // GET: RRHH/AjusteBeneficiosSociales
        #region Index
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = string.IsNullOrEmpty(SessionFixed.IdSucursal) ? 0 : Convert.ToInt32(SessionFixed.IdSucursal),
                IdNomina = 1,
                IdRubro = "12",
                IdSigno = "+",
                Valor= 0,
            };

            cargar_combos(model.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            model.IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);

            cargar_combos(model.IdEmpresa);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_AjusteBeneficiosSociales(DateTime FechaIni, DateTime FechaFin, int IdSucursal = 0, int IdNomina_Tipo = 0, string IdRubro="")
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.IdSucursal = IdSucursal == 0 ? 0 : Convert.ToInt32(IdSucursal);
            ViewBag.IdNomina_Tipo = IdNomina_Tipo == 0 ? 0 : Convert.ToInt32(IdNomina_Tipo);
            ViewBag.IdRubro = IdRubro == "" ? "" : Convert.ToString(IdRubro);
            ViewBag.FechaIni = FechaIni == null ? DateTime.Now : Convert.ToDateTime(FechaIni);
            ViewBag.FechaFin = FechaFin == null ? DateTime.Now : Convert.ToDateTime(FechaFin);
            
            List<ro_rol_detalle_x_rubro_acumulado_Info> lst_detalle = bus_detalle_rubro_acumulado.GetList_BeneficiosSociales(IdEmpresa, IdSucursal, IdNomina_Tipo, IdRubro, FechaIni, FechaFin);
            return PartialView("_GridViewPartial_AjusteBeneficiosSociales", lst_detalle);
        }
        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            try
            {
                var lst_Sucursal = bus_sucursal.get_list(IdEmpresa, false);
                ViewBag.lst_Sucursal = lst_Sucursal;

                var lst_tipo_nomina = bus_nomina_tipo.get_list(IdEmpresa, false);
                ViewBag.lst_tipo_nomina = lst_tipo_nomina;

                Dictionary<string, string> lst_Rubro = new Dictionary<string, string>();
                lst_Rubro.Add("11", "Décimo cuarto sueldo");
                lst_Rubro.Add("12", "Décimo tercer sueldo");
                ViewBag.lst_Rubro = lst_Rubro;

                Dictionary<string, string> lst_Tipo = new Dictionary<string, string>();
                lst_Tipo.Add("+", "+");
                lst_Tipo.Add("-", "-");
                ViewBag.lst_Tipo = lst_Tipo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Json
        public JsonResult ActualizarValores(DateTime FechaIni, DateTime FechaFin, int IdEmpresa, int IdSucursal, int IdNomina_Tipo, string IdRubro, string IdSigno, double Valor)
        {
            var IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            ViewBag.FechaIni = FechaIni == null ? DateTime.Now.Date : Convert.ToDateTime(FechaIni);
            ViewBag.FechaFin = FechaFin == null ? DateTime.Now.Date : Convert.ToDateTime(FechaFin);
            ViewBag.IdEmpresa = IdEmpresa == 0 ? 0 : IdEmpresa;
            ViewBag.IdSucursal = IdSucursal == 0 ? 0 : IdSucursal;
            ViewBag.IdRubro = IdRubro == null ? "" : IdRubro;
            /*
            var model = bus_ing_egr_Inven.BuscarMovimientos(IdEmpresa, ViewBag.fecha_ini, ViewBag.fecha_fin, ViewBag.TipoMovimiento);
            ListaIngEgrInvMovimientos.set_list(model, IdTransaccionSession);
            */
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Acciones
        [HttpPost]
        public ActionResult Nuevo(List<ro_rol_detalle_x_rubro_acumulado_Info> model)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (!bus_detalle_rubro_acumulado.ModificarBD(model))
            {
                cargar_combos(IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}