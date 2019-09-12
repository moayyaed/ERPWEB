using Core.Erp.Bus.Caja;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorCobrar.Controllers
{
    [SessionTimeout]
    public class ParametroCXCController : Controller
    {
        #region Variables
        cxc_Parametro_Bus bus_parametro = new cxc_Parametro_Bus();
        ct_cbtecble_tipo_Bus bus_tipo_comprobante = new ct_cbtecble_tipo_Bus();
        cxc_cobro_tipo_Bus bus_cobrotipo = new cxc_cobro_tipo_Bus();
        caj_Caja_Movimiento_Tipo_Bus bus_movimiento = new caj_Caja_Movimiento_Tipo_Bus();
        caj_Caja_Bus bus_caja = new caj_Caja_Bus();
        fa_TipoNota_Bus bus_tiponota = new fa_TipoNota_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        #endregion

        #region Combos bajo demanda Fuente
        public ActionResult CmbCuenta_ProvisionFuente()
        {
            cxc_Parametro_Info model = new cxc_Parametro_Info();
            return PartialView("_CmbCuenta_ProvisionFuente", model);
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

        #region Combos bajo demanda Iva
        public ActionResult CmbCuenta_ProvisionIva()
        {
            cxc_Parametro_Info model = new cxc_Parametro_Info();
            return PartialView("_CmbCuenta_ProvisionIva", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda_iva(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda_iva(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Index
        public ActionResult Index( )
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            cxc_Parametro_Info model = bus_parametro.get_info(IdEmpresa);
            if (model == null)
                model = new cxc_Parametro_Info { IdEmpresa = IdEmpresa };
                cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cxc_Parametro_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_parametro.guardarDB(model))
                ViewBag.mensaje = "No se pudieron actualizar los registros";
            cargar_combos(model.IdEmpresa);
            return View(model);
        }

        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {

            var lst_tipo_comprobante = bus_tipo_comprobante.get_list(IdEmpresa, false);
            ViewBag.lst_tipo_comprobante = lst_tipo_comprobante;

            var lst_cobrotipo = bus_cobrotipo.get_list(false);
            ViewBag.lst_cobrotipo = lst_cobrotipo;
            
            var lst_movimiento = bus_movimiento.get_list(IdEmpresa, false);
            ViewBag.lst_movimiento = lst_movimiento;

            var lst_caja = bus_caja.get_list(IdEmpresa, false);
            ViewBag.lst_caja= lst_caja;


            var lst_tiponota = bus_tiponota.get_list(IdEmpresa, "D", false);
            ViewBag.lst_tiponota = lst_tiponota;
        }

        #endregion
    }
}