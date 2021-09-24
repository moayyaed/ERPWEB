using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using DevExpress.Web.Mvc;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.General;
using DevExpress.Web;
using Core.Erp.Info.General;
using Core.Erp.Data.RRHH;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class HorasExtrasController : Controller
    {
        #region variables
        ro_nomina_x_horas_extras_Bus bus_horas_extras = new ro_nomina_x_horas_extras_Bus();
        ro_nomina_x_horas_extras_det_Bus bus_hora_extra_detalle = new ro_nomina_x_horas_extras_det_Bus();
        ro_empleado_Bus bus_empleado = new ro_empleado_Bus();
        ro_horario_Bus bus_horario = new ro_horario_Bus();
        List<ro_nomina_x_horas_extras_Info> lst_horas_extras = new List<ro_nomina_x_horas_extras_Info>();
        ro_nomina_tipo_Bus bus_nomina = new ro_nomina_tipo_Bus();
        ro_Nomina_Tipoliquiliqui_Bus bus_nomina_tipo = new ro_Nomina_Tipoliquiliqui_Bus();
        ro_periodo_x_ro_Nomina_TipoLiqui_Bus bus_periodos = new ro_periodo_x_ro_Nomina_TipoLiqui_Bus();
        ro_nomina_x_horas_extras_Info info = new ro_nomina_x_horas_extras_Info();
        ro_nomina_x_horas_extras_det_Info_list ro_nomina_x_horas_extras_det_Info_list = new ro_nomina_x_horas_extras_det_Info_list();
        ro_empleado_info_list empleado_info_list = new ro_empleado_info_list();
        int IdEmpresa = 0;
        #endregion


        #region Metodos ComboBox bajo demanda
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbEmpleado_HorasExtras()
       {
            decimal model = new decimal();
            return PartialView("_CmbEmpleado_HorasExtras", model);
        }

        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.EMPLEA.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.EMPLEA.ToString());
        }
        #endregion
        #region aprobacion horas extras
        public ActionResult Index2()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index2(cl_filtros_Info model)
        {
            return View(model);

        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_aprobacion_horas_extras(DateTime? Fecha_ini, DateTime? Fecha_fin)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);
            lst_horas_extras = bus_horas_extras.get_list(IdEmpresa);
            return PartialView("_GridViewPartial_aprobacion_horas_extras", lst_horas_extras);
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_aprobacion_horas_extras_det()
        {
            cargar_combos_detalle();
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_aprobacion_horas_extras_det", model);
        }

        public ActionResult Aprobar(int IdNomina_Tipo, int IdNomina_TipoLiqui, int IdPeriodo, int IdHorasExtras)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cargar_combos(IdNomina_Tipo, IdNomina_TipoLiqui);
            IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model = bus_horas_extras.get_info(IdEmpresa, IdHorasExtras);
            if (model != null)
                model.lst_nomina_horas_extras = bus_hora_extra_detalle.get_list(model.IdEmpresa, model.IdHorasExtras);
            ro_nomina_x_horas_extras_det_Info_list.set_list(model.lst_nomina_horas_extras, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            return View(model);
        }
        [HttpPost]
        public ActionResult Aprobar(ro_nomina_x_horas_extras_Info model)
        {

            model.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            model.IdUsuario = Session["IdUsuario"].ToString();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(model.IdTransaccionSession);
            foreach (var item in model.lst_nomina_horas_extras)
            {
                item.IdSucursal =Convert.ToInt32( SessionFixed.IdSucursal);
            }
            if (!bus_horas_extras.aprobacionHE(model))
            {
                cargar_combos(info.IdNomina_Tipo, info.IdNomina_TipoLiqui);
                cargar_combos(model.IdNomina_Tipo, model.IdNomina_TipoLiqui);
                return View(model);
            }
            return RedirectToAction("Index2");

        }
        #endregion
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                fecha_fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            return View(model);

        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_horas_extras(DateTime? Fecha_ini, DateTime? Fecha_fin)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.Fecha_ini = Fecha_ini == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : Convert.ToDateTime(Fecha_ini);
            ViewBag.Fecha_fin = Fecha_fin == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : Convert.ToDateTime(Fecha_fin);
            lst_horas_extras = bus_horas_extras.get_list(IdEmpresa);
            return PartialView("_GridViewPartial_horas_extras", lst_horas_extras);
        }

        [ValidateInput(false)]
        public ActionResult Nuevo()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cargar_combos(0, 0);
            ro_nomina_x_horas_extras_Info info = new ro_nomina_x_horas_extras_Info();
            info.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession) ;
            info.lst_nomina_horas_extras = new List<ro_nomina_x_horas_extras_det_Info>();
            info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            var lst_empleado = bus_empleado.get_list_combo(Convert.ToInt32(SessionFixed.IdEmpresa));
            empleado_info_list.set_list(lst_empleado, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(info);
        }
        [HttpPost]
        public ActionResult Nuevo(ro_nomina_x_horas_extras_Info model)
        {
            model.IdHorasExtras = bus_horas_extras.get_info(model.IdEmpresa, model.IdNomina_Tipo, model.IdNomina_TipoLiqui, model.IdPeriodo).IdHorasExtras;
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(model.IdTransaccionSession);
            if (bus_horas_extras.guardarDB(model))
                return RedirectToAction("Index");
            else
            {
                cargar_combos(model.IdNomina_Tipo, model.IdNomina_TipoLiqui);
                return View(model);
            }
        }
        public ActionResult Modificar(int IdNomina_Tipo, int IdNomina_TipoLiqui, int IdPeriodo, int IdHorasExtras)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cargar_combos(IdNomina_Tipo, IdNomina_TipoLiqui);
            IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model = bus_horas_extras.get_info(IdEmpresa, IdHorasExtras);
            if (model != null)
                model.lst_nomina_horas_extras = bus_hora_extra_detalle.get_list(model.IdEmpresa, model.IdHorasExtras);
            ro_nomina_x_horas_extras_det_Info_list.set_list(model.lst_nomina_horas_extras, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);

            var lst_empleado = bus_empleado.get_list_combo(Convert.ToInt32(SessionFixed.IdEmpresa));
            empleado_info_list.set_list(lst_empleado, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ro_nomina_x_horas_extras_Info model)
        {

            model.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            model.IdUsuario = Session["IdUsuario"].ToString();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(model.IdTransaccionSession);
            if (!bus_horas_extras.modificarDB(model))
            {
                cargar_combos(model.IdNomina_Tipo, model.IdNomina_TipoLiqui);
                return View(model);
            }
            return RedirectToAction("Index");

        }
        public ActionResult Anular(int IdNomina_Tipo, int IdNomina_TipoLiqui, int IdPeriodo, int IdHorasExtras)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cargar_combos(IdNomina_Tipo, IdNomina_TipoLiqui);
            IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model = bus_horas_extras.get_info(IdEmpresa, IdHorasExtras);
            if (model != null)
                model.lst_nomina_horas_extras = bus_hora_extra_detalle.get_list(model.IdEmpresa, model.IdHorasExtras);
            ro_nomina_x_horas_extras_det_Info_list.set_list(model.lst_nomina_horas_extras, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);

            var lst_empleado = bus_empleado.get_list_combo(Convert.ToInt32(SessionFixed.IdEmpresa));
            empleado_info_list.set_list(lst_empleado, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ro_nomina_x_horas_extras_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            model.IdUsuario = Session["IdUsuario"].ToString();
            if (!bus_horas_extras.anularDB(model))
            {
                cargar_combos(model.IdNomina_Tipo, model.IdNomina_TipoLiqui);
                return View(model);
            }
            return RedirectToAction("Index");
        }


        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ro_nomina_x_horas_extras_det_Info info_det)
        {
            info_det.time_entrada1 = new TimeSpan(info_det.time_entrada1_date.Hour, info_det.time_entrada1_date.Minute, info_det.time_entrada1_date.Second);
            info_det.time_salida1 = new TimeSpan(info_det.time_salida1_date.Hour, info_det.time_salida1_date.Minute, info_det.time_salida1_date.Second);
            if (ModelState.IsValid)
                ro_nomina_x_horas_extras_det_Info_list.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_horas_extras_det", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ro_nomina_x_horas_extras_det_Info info_det)
        {
            if (ModelState.IsValid)
                ro_nomina_x_horas_extras_det_Info_list.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_horas_extras_det", model);

        }

        public ActionResult EditingDelete([ModelBinder(typeof(DevExpressEditorsBinder))] ro_nomina_x_horas_extras_det_Info info_det)
        {
            ro_nomina_x_horas_extras_det_Info_list.DeleteRow(Convert.ToInt32(info_det.Secuencia), Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_horas_extras_det", model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate_apro([ModelBinder(typeof(DevExpressEditorsBinder))] ro_nomina_x_horas_extras_det_Info info_det)
        {
            if (ModelState.IsValid)
                ro_nomina_x_horas_extras_det_Info_list.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_aprobacion_horas_extras_det", model);

        }

        public ActionResult EditingDelete_apro([ModelBinder(typeof(DevExpressEditorsBinder))] ro_nomina_x_horas_extras_det_Info info_det)
        {
            ro_nomina_x_horas_extras_det_Info_list.DeleteRow(Convert.ToInt32(info_det.Secuencia), Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_aprobacion_horas_extras_det", model);

        }


        [ValidateInput(false)]
        public ActionResult GridViewPartial_horas_extras_det()
        {
            cargar_combos_detalle();
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.lst_nomina_horas_extras = ro_nomina_x_horas_extras_det_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_horas_extras_det", model);
        }
        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            ViewBag.lst_horario = bus_horario.get_list(IdEmpresa, false);
        }
        private void cargar_combos(int IdNomina, int IdNominaTipo)
        {
            IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString());
            ViewBag.lst_nomina = bus_nomina.get_list(IdEmpresa, false);
            ViewBag.lst_nomina_tipo = bus_nomina_tipo.get_list(IdEmpresa, IdNomina);
            var lst_periodo = bus_periodos.get_list(IdEmpresa, IdNomina, IdNominaTipo);            
            lst_periodo.ForEach(q => q.descripcion = q.pe_FechaIni.ToString("dd/MM/yyyy") + " - " + q.pe_FechaFin.ToString("dd/MM/yyyy"));
            ViewBag.lst_periodo = lst_periodo;

        }


        public JsonResult get_horas_extras(int IdEmpresa = 0, int IdNomina = 0, int IdNomina_Tipo = 0, int IdPeriodo = 0, decimal IdTransaccionSession = 0)
        {
            ro_nomina_x_horas_extras_Info model = new ro_nomina_x_horas_extras_Info();
            model.IdEmpresa = IdEmpresa;
            model.IdNomina_Tipo = IdNomina;
            model.IdNomina_TipoLiqui = IdNomina_Tipo;
            model.IdPeriodo = IdPeriodo;
            bus_horas_extras.ProcesarHorasExtras(model);
            model= bus_horas_extras.get_info(model.IdEmpresa, model.IdNomina_Tipo, model.IdNomina_TipoLiqui, model.IdPeriodo);
            model.lst_nomina_horas_extras = bus_hora_extra_detalle.get_list(model.IdEmpresa, model.IdHorasExtras);
            ro_nomina_x_horas_extras_det_Info_list.set_list(model.lst_nomina_horas_extras, IdTransaccionSession);
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }

    public class ro_nomina_x_horas_extras_det_Info_list
    {
        string variable = "ro_nomina_x_horas_extras_det_Info";
        ro_empleado_Bus bus_empleado = new ro_empleado_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        ro_marcaciones_x_empleado_Bus bus_marcaciones = new ro_marcaciones_x_empleado_Bus();
        ro_empleado_info_list empleado_info_list = new ro_empleado_info_list();
        ro_nomina_x_horas_extras_Bus bus_horas_extras = new ro_nomina_x_horas_extras_Bus();
        ro_horario_Data odata_horario = new ro_horario_Data();
        public List<ro_nomina_x_horas_extras_det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable+ IdTransaccionSession.ToString()] == null)
            {
                List<ro_nomina_x_horas_extras_det_Info> list = new List<ro_nomina_x_horas_extras_det_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_nomina_x_horas_extras_det_Info>)HttpContext.Current.Session[variable+ IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_nomina_x_horas_extras_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ro_nomina_x_horas_extras_det_Info info_det, decimal IdTransaccionSession)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            List<ro_nomina_x_horas_extras_det_Info> list = get_list(IdTransaccionSession);

            if (info_det.time_entrada1!=null && info_det.time_salida1!=null)
            {                
                //var empleado = bus_empleado.get_info(IdEmpresa, info_det.IdEmpleado);
                var empleado = empleado_info_list.get_list(IdTransaccionSession).Where(v => v.IdEmpleado == info_det.IdEmpleado).FirstOrDefault();
                info_det.pe_nombreCompleto = empleado.Empleado;

                List<ro_marcaciones_x_empleado_Info> lista_marcacion = new List<ro_marcaciones_x_empleado_Info>();
                var info_entrada = new ro_marcaciones_x_empleado_Info
                {
                    IdEmpleado = info_det.IdEmpleado,
                    IdEmpresa = IdEmpresa,
                    es_fechaRegistro = info_det.FechaRegistro,
                    IdCalendadrio = Convert.ToInt32(info_det.FechaRegistro.ToString("ddMMyyyy")),
                    IdNomina = empleado.IdTipoNomina,
                    IdUsuario = SessionFixed.IdUsuario,
                    es_Hora = new TimeSpan(info_det.time_entrada1_date.Hour, info_det.time_entrada1_date.Minute, 0),
                    IdTipoMarcaciones = cl_enumeradores.eTipoMarcacionRRHH.IN1.ToString(),
                    pe_NombreCompleato = empleado.Empleado,
                    pe_cedula = empleado.pe_cedulaRuc,
                    EstadoBool = true,
                    //IdRegistro = cont++
                };
                lista_marcacion.Add(info_entrada);

                var info_salida = new ro_marcaciones_x_empleado_Info
                {
                    IdEmpleado = info_det.IdEmpleado,
                    IdEmpresa = IdEmpresa,
                    es_fechaRegistro = info_det.FechaRegistro,
                    IdCalendadrio = Convert.ToInt32(info_det.FechaRegistro.ToString("ddMMyyyy")),
                    IdNomina = empleado.IdTipoNomina,
                    IdUsuario = SessionFixed.IdUsuario,
                    es_Hora = new TimeSpan(info_det.time_salida1_date.Hour, info_det.time_salida1_date.Minute, 0),
                    IdTipoMarcaciones = cl_enumeradores.eTipoMarcacionRRHH.OUT1.ToString(),
                    pe_NombreCompleato = empleado.Empleado,
                    pe_cedula = empleado.pe_cedulaRuc,
                    EstadoBool = true,
                    //IdRegistro = cont++
                };
                lista_marcacion.Add(info_salida);

                if (bus_marcaciones.guardarDB(lista_marcacion, IdEmpresa))
                {
                    list.Add(info_det);
                }                                             
            }            
        }

        public void UpdateRow(ro_nomina_x_horas_extras_det_Info info_det, decimal IdTransaccionSession)
        {
            ro_nomina_x_horas_extras_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.hora_extra100 = info_det.hora_extra100;
            edited_info.hora_extra50 = info_det.hora_extra50;
            edited_info.hora_extra25 = info_det.hora_extra25;

        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<ro_nomina_x_horas_extras_det_Info> list = get_list(IdTransaccionSession);
            var info = list.Where(m => m.Secuencia == Secuencia).FirstOrDefault();
            var marcacion = new ro_marcaciones_x_empleado_Info
            {
                IdCalendadrio = info.IdCalendario,
                IdEmpleado = info.IdEmpleado,
            };
            if (bus_marcaciones.eliminarDB(marcacion))
            {
                list.Remove(list.Where(m => m.Secuencia == Secuencia).FirstOrDefault());
            }

        }

    }
}
