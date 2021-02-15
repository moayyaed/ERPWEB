using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using Core.Erp.Web.Helps;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Bus.Contabilidad;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class ParticipacionUtilidadController : Controller
    {
        #region variables
        ro_participacion_utilidad_empleado_Info_lst ro_participacion_utilidad_empleado_Info_lst = new ro_participacion_utilidad_empleado_Info_lst();
        ro_participacion_utilidad_Bus bus_utilidad = new ro_participacion_utilidad_Bus();
        List<ro_nomina_tipo_Info> lista_nomina = new List<ro_nomina_tipo_Info>();
        List<ro_participacion_utilidad_empleado_Info> lst_detalle = new List<ro_participacion_utilidad_empleado_Info>();
        ro_participacion_utilidad_empleado_Bus bus_detalle = new ro_participacion_utilidad_empleado_Bus();
        ro_nomina_tipo_Bus bus_nomina = new ro_nomina_tipo_Bus();
        ro_Nomina_Tipoliquiliqui_Bus bus_nomina_tipo = new ro_Nomina_Tipoliquiliqui_Bus();
        List<ro_Nomina_Tipoliqui_Info> lst_nomina_tipo = new List<ro_Nomina_Tipoliqui_Info>();
        ro_periodo_x_ro_Nomina_TipoLiqui_Bus bus_periodos_x_nomina = new ro_periodo_x_ro_Nomina_TipoLiqui_Bus();
        List<ro_periodo_x_ro_Nomina_TipoLiqui_Info> lst_periodos = new List<ro_periodo_x_ro_Nomina_TipoLiqui_Info>();
        ro_participacion_utilidad_Info info_utilidad = new ro_participacion_utilidad_Info();
        ct_periodo_Bus bus_anio_fiscal = new ct_periodo_Bus();
        #endregion

        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_utilidades()
        {
            try
            {
                List<ro_participacion_utilidad_Info> model = bus_utilidad.get_list(Convert.ToInt32(SessionFixed.IdEmpresa), true);
                return PartialView("_GridViewPartial_utilidades", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GridViewPartial_utilidades_detalle()
        {
            try
            {
                ro_participacion_utilidad_Info info = new ro_participacion_utilidad_Info();
                info.detalle = ro_participacion_utilidad_empleado_Info_lst.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                if (info == null)
                    info = new ro_participacion_utilidad_Info();
                return PartialView("_GridViewPartial_utilidades_detalle", info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_participacion_utilidad_Info info)
        {
            try
            {

                info.IdSucursal = Convert.ToInt32(SessionFixed.IdEmpresa);
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    info.detalle = ro_participacion_utilidad_empleado_Info_lst.get_list(Convert.ToDecimal(info.IdTransaccionSession));
                if (!bus_utilidad.modificarDB(info))
                {
                    cargar_combo();
                    return View(info);

                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Modificar(int IdUtilidad = 0)
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion
                ro_participacion_utilidad_Info info = new ro_participacion_utilidad_Info();
                info= bus_utilidad.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdUtilidad);
                ro_participacion_utilidad_empleado_Info_lst.set_list( info.detalle, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                info.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
                cargar_combo();
                return View(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Anular(ro_participacion_utilidad_Info info)
        {
            try
            {
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                if (!bus_utilidad.anularDB(info))
                {
                    cargar_combo();
                    return View(info);
                }
                else
                    return RedirectToAction("Index");


            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdUtilidad = 0)
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion

                ro_participacion_utilidad_Info info = new ro_participacion_utilidad_Info();
                info = bus_utilidad.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdUtilidad);
                ro_participacion_utilidad_empleado_Info_lst.set_list(info.detalle, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
                info.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);

                return View(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_participacion_utilidad_Info info)
        {
            try
            {
                    info.IdSucursal = Convert.ToInt32(SessionFixed.IdEmpresa);
                    info.UsuarioIngresa = SessionFixed.IdUsuario;
                    info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                info.detalle = ro_participacion_utilidad_empleado_Info_lst.get_list(Convert.ToDecimal(info.IdTransaccionSession));
                if (!bus_utilidad.guardarDB(info))
                {
                    cargar_combo();
                    return View(info);
                }
                    return RedirectToAction("Index");


            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Nuevo()
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion
                
                ro_participacion_utilidad_Info info = new ro_participacion_utilidad_Info();
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                info.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
                cargar_combo();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region funciones del detalle
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ro_participacion_utilidad_empleado_Info info_det)
        {
            if (ModelState.IsValid)
                ro_participacion_utilidad_empleado_Info_lst.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_participacion_utilidad_Info model = new ro_participacion_utilidad_Info();
            model.detalle = ro_participacion_utilidad_empleado_Info_lst.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_utilidades_detalle", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ro_participacion_utilidad_empleado_Info info_det)
        {
            if (ModelState.IsValid)
                ro_participacion_utilidad_empleado_Info_lst.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_participacion_utilidad_Info model = new ro_participacion_utilidad_Info();
            model.detalle = ro_participacion_utilidad_empleado_Info_lst.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_utilidades_detalle", model);
        }

        public ActionResult EditingDelete([ModelBinder(typeof(DevExpressEditorsBinder))] ro_participacion_utilidad_empleado_Info info_det)
        {
            ro_participacion_utilidad_empleado_Info_lst.DeleteRow(Convert.ToInt32( info_det.IdEmpleado), Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_participacion_utilidad_Info model = new ro_participacion_utilidad_Info();
            model.detalle = ro_participacion_utilidad_empleado_Info_lst.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_utilidades_detalle", model);
        }
        #endregion


        public JsonResult calcular(int IdPeriodo = 0, double BaseUtilidad = 0)
        {
            try
            {

                ro_participacion_calculo_Info calculo = new ro_participacion_calculo_Info();
                calculo.BaseUtilidad = BaseUtilidad;
                calculo.Utilidad =Math.Round( (BaseUtilidad / Convert.ToInt32(cl_enumeradores.eTipoPorcentajeUtilidad.PORCENTAJE_BASE)) * Convert.ToInt32(cl_enumeradores.eTipoPorcentajeUtilidad.PORCENTAJE_UTILIDAD),2);
                calculo.UtilidadDerechoIndividual =Math.Round( (calculo.Utilidad / Convert.ToInt32(cl_enumeradores.eTipoPorcentajeUtilidad.PORCENTAJE_UTILIDAD)) * Convert.ToInt32(cl_enumeradores.eTipoPorcentajeUtilidad.PORCENTAJE_INDIVIDUAL),2);
                calculo.UtilidadCargaFamiliar =Math.Round( (calculo.Utilidad / Convert.ToInt32(cl_enumeradores.eTipoPorcentajeUtilidad.PORCENTAJE_UTILIDAD)) * Convert.ToInt32(cl_enumeradores.eTipoPorcentajeUtilidad.PORCENTAJE_CARGAS),2);


                info_utilidad.IdPeriodo = IdPeriodo;
                info_utilidad.UtilidadDerechoIndividual = calculo.UtilidadDerechoIndividual;
                info_utilidad.UtilidadCargaFamiliar = calculo.UtilidadCargaFamiliar;
                info_utilidad.detalle = bus_detalle.calcular(Convert.ToInt32(SessionFixed.IdEmpresa),  IdPeriodo, calculo.UtilidadDerechoIndividual, calculo.UtilidadCargaFamiliar);
                ro_participacion_utilidad_empleado_Info_lst.set_list(info_utilidad.detalle,Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return Json(calculo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cargar_combo()
        {
            ct_anio_fiscal_Bus bus_anio = new ct_anio_fiscal_Bus();
            var lst_anio = bus_anio.get_list(false);
            ViewBag.lst_anio = lst_anio;
        }


    }


    public class ro_participacion_utilidad_empleado_Info_lst
    {
        string Variable = "ro_participacion_utilidad_empleado_Info";
        public List<ro_participacion_utilidad_empleado_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_participacion_utilidad_empleado_Info> list = new List<ro_participacion_utilidad_empleado_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_participacion_utilidad_empleado_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_participacion_utilidad_empleado_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ro_participacion_utilidad_empleado_Info info_det, decimal IdTransaccionSession)
        {
            ro_rubro_tipo_Bus bus_rub = new ro_rubro_tipo_Bus();
            List<ro_participacion_utilidad_empleado_Info> list = get_list(IdTransaccionSession);
            info_det.IdEmpleado = list.Count == 0 ? 1 : list.Max(q => q.IdEmpleado) + 1;

            list.Add(info_det);
        }

        public void UpdateRow(ro_participacion_utilidad_empleado_Info info_det, decimal IdTransaccionSession)
        {
            ro_participacion_utilidad_empleado_Info edited_info = get_list(IdTransaccionSession).Where(m => m.IdEmpleado == info_det.IdEmpleado).First();
            edited_info.UtilidadCargaFamiliar = info_det.UtilidadCargaFamiliar;
            edited_info.UtilidadDerechoIndividual = info_det.UtilidadDerechoIndividual;
            edited_info.ValorTotal = info_det.ValorTotal;
            edited_info.Observacion = info_det.Observacion;
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<ro_participacion_utilidad_empleado_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.IdEmpleado == Secuencia).First());
        }
    }
}