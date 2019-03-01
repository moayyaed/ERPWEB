using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using Core.Erp.Web.Reportes.CuentasPorCobrar;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Reportes.Controllers
{
    [SessionTimeout]
    public class CuentasPorCobrarReportesController : Controller
    {
        #region Metodos ComboBox bajo demanda
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbCliente_CXC()
        {
            decimal model = new decimal();
            return PartialView("_CmbCliente_CXC", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        #endregion
        #region Json
        private void cargar_cliente_contacto(cl_filtros_facturacion_Info model)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(model.IdEmpresa, false);
            lst_sucursal.Add(new Info.General.tb_sucursal_Info
            {
                IdSucursal = 0,
                Su_Descripcion = "Todas"
            });
            ViewBag.lst_sucursal = lst_sucursal;

            fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
            var lst_cliente = bus_cliente.get_list(model.IdEmpresa, false);
            lst_cliente.Add(new fa_cliente_Info
             { 
                IdEmpresa = model.IdEmpresa,
                IdCliente = 0,
                Descripcion_tip_cliente = "Todos"
            });
            ViewBag.lst_cliente = lst_cliente;

            fa_cliente_contactos_Bus bus_contacto = new fa_cliente_contactos_Bus();
            var lst_contacto = bus_contacto.get_list(model.IdEmpresa, model.IdCliente == null ? 0 : Convert.ToDecimal(model.IdCliente));
            lst_contacto.Add(new fa_cliente_contactos_Info
            {

                IdContacto = 0,
                Nombres = "Todos"
            });
            ViewBag.lst_contacto = lst_contacto;
        }
        public JsonResult cargar_cliente(int IdEmpresa = 0 , decimal IdCliente = 0)
        {
            fa_cliente_contactos_Bus bus_contacto = new fa_cliente_contactos_Bus();
            var resultado = bus_contacto.get_list(IdEmpresa, IdCliente);
            resultado.Add(new Info.Facturacion.fa_cliente_contactos_Info
            {
                IdContacto = 0,
                Nombres = "Todos"
            });
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        private void cargar_combos(int IdEmpresa)
        {
            fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
            var lst_cliente = bus_cliente.get_list(IdEmpresa, false);
            ViewBag.lst_cliente = lst_cliente;

            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;

            cxc_cobro_tipo_Bus bus_cobro = new cxc_cobro_tipo_Bus();
            var lst_cobro = bus_cobro.get_list(false);
            lst_cobro.Add(new Info.CuentasPorCobrar.cxc_cobro_tipo_Info
            {
                IdCobro_tipo = "",
                tc_descripcion = "TODOS"
            });
            ViewBag.lst_cobro = lst_cobro;
        }
        public ActionResult CXC_001(int IdSucursal = 0, decimal IdCobro = 0)
        {
            CXC_001_Rpt model = new CXC_001_Rpt();
            model.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.p_IdSucursal.Value = IdSucursal;
            model.p_IdCobro.Value = IdCobro;
            model.usuario = SessionFixed.IdUsuario.ToString();
            model.empresa = SessionFixed.NomEmpresa.ToString();
            return View(model);
        }
        public ActionResult CXC_002(int IdSucursal = 0, int IdBodega_Cbte = 0, decimal IdCbte_vta_nota =0, string dc_TipoDocumento ="")
        {
            CXC_002_Rpt model = new CXC_002_Rpt();
            model.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.p_IdSucursal.Value = IdSucursal;
            model.p_IdBodega_Cbte.Value = IdBodega_Cbte;
            model.p_IdCbte_vta_nota.Value = IdCbte_vta_nota;
            model.p_dc_TipoDocumento.Value = dc_TipoDocumento;
            model.usuario = SessionFixed.IdUsuario.ToString();
            model.empresa = SessionFixed.NomEmpresa.ToString();
            model.RequestParameters = false;
            return View(model);
        }
        public ActionResult CXC_003()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdCliente = 0
            };
            cargar_combos(model.IdEmpresa);
            CXC_003_Rpt report = new CXC_003_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCliente.Value = model.IdCliente;
            report.p_Fecha_ini.Value = model.fecha_ini;
            report.p_Fecha_fin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_003(cl_filtros_facturacion_Info model)
        {

            CXC_003_Rpt report = new CXC_003_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCliente.Value = model.IdCliente;
            report.p_Fecha_ini.Value = model.fecha_ini;
            report.p_Fecha_fin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            cargar_combos(model.IdEmpresa);
            ViewBag.Report = report;
            return View(model);
        }
        public ActionResult CXC_004()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdCliente = 0

            };
            cargar_cliente_contacto(model);
            CXC_004_Rpt report = new CXC_004_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCliente.Value = model.IdCliente;
            report.p_IdContacto.Value = model.IdClienteContacto;
            report.p_fecha_corte.Value = model.fecha_corte;
            report.p_MostrarSaldo0.Value = model.Check1;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_004(cl_filtros_facturacion_Info model)
        {
            CXC_004_Rpt report = new CXC_004_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCliente.Value = model.IdCliente;
            report.p_IdContacto.Value = model.IdClienteContacto;
            report.p_fecha_corte.Value = model.fecha_corte;
            report.p_MostrarSaldo0.Value = model.Check1;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            cargar_cliente_contacto(model);
            ViewBag.Report = report;

            return View(model);
        }
        public ActionResult CXC_005()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = 0

            };
            cargar_cliente_contacto(model);
            CXC_005_Rpt report = new CXC_005_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdCliente.Value = model.IdCliente == null ? 0 : Convert.ToDecimal(model.IdCliente);
            report.p_IdContacto.Value = model.IdClienteContacto;
            report.p_fecha_corte.Value = model.fecha_corte;
            report.p_mostrarSaldo0.Value = model.mostrarSaldo0;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_005(cl_filtros_facturacion_Info model)
        {
            CXC_005_Rpt report = new CXC_005_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdCliente.Value = model.IdCliente == null ? 0 : Convert.ToDecimal(model.IdCliente);
            report.p_IdContacto.Value = model.IdClienteContacto;
            report.p_fecha_corte.Value = model.fecha_corte;
            report.p_mostrarSaldo0.Value = model.mostrarSaldo0;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            cargar_cliente_contacto(model);
            ViewBag.Report = report;

            return View(model);
        }
        public ActionResult CXC_006( decimal IdLiquidacion= 0)
        {
            CXC_006_Rpt report = new CXC_006_Rpt();
            report.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            report.p_IdLiquidacion.Value = IdLiquidacion;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            return View(report);
        }

        public ActionResult CXC_007(int IdSucursal = 0, decimal IdLiquidacion = 0)
        {
            CXC_007_Rpt report = new CXC_007_Rpt();
            report.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            report.p_IdSucursal.Value = IdSucursal;
            report.p_IdLiquidacion.Value = IdLiquidacion;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            return View(report);
        }

        public ActionResult CXC_008()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = 0,
                IdCobro_tipo = ""

            };
            cargar_combos(model.IdEmpresa);
            CXC_008_Rpt report = new CXC_008_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdCliente.Value = model.IdCliente ?? 0;
            report.p_IdCobro_tipo.Value = model.IdCobro_tipo;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_mostrar_anulados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_008(cl_filtros_facturacion_Info model)
        {
            CXC_008_Rpt report = new CXC_008_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdCliente.Value = model.IdCliente ?? 0;
            report.p_IdCobro_tipo.Value = model.IdCobro_tipo;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_mostrar_anulados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            cargar_combos(model.IdEmpresa);
            ViewBag.Report = report;

            return View(model);
        }
    }
}