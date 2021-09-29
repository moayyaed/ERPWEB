using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using Core.Erp.Bus.General;
using Core.Erp.Info.General;
using DevExpress.Web;
using Core.Erp.Web.Helps;
using Core.Erp.Info.Helps;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class VacacionesController : Controller
    {
        #region variables
        ro_historico_vacaciones_x_empleado_Bus bus_vacaciones = new ro_historico_vacaciones_x_empleado_Bus();
        ro_Solicitud_Vacaciones_x_empleado_det_List ro_Solicitud_Vacaciones_x_empleado_det_List = new ro_Solicitud_Vacaciones_x_empleado_det_List();
        ro_historico_vacaciones_x_empleado_Info_list ro_historico_vacaciones_x_empleado_Info_list = new ro_historico_vacaciones_x_empleado_Info_list();
        ro_Solicitud_Vacaciones_x_empleado_List ro_Solicitud_Vacaciones_x_empleado_List = new ro_Solicitud_Vacaciones_x_empleado_List();

        ro_Solicitud_Vacaciones_x_empleado_Bus bus_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Bus();
        ro_empleado_Bus bus_empleado = new ro_empleado_Bus();
        ro_catalogo_Bus bus_catalogo = new ro_catalogo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion

        #region Metodos ComboBox bajo demanda
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbEmpleado_vacaciones()
        {
            ro_Solicitud_Vacaciones_x_empleado_Info model = new ro_Solicitud_Vacaciones_x_empleado_Info();
            return PartialView("_CmbEmpleado_vacaciones", model);
        }
        public ActionResult CmbEmpleado_autoriza_vacaciones()
        {
            ro_Solicitud_Vacaciones_x_empleado_Info model = new ro_Solicitud_Vacaciones_x_empleado_Info();
            return PartialView("_CmbEmpleado_autoriza_vacaciones", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.EMPLEA.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.EMPLEA.ToString());
        }


        public ActionResult cmb_vacaciones()
        {
            int model = new int();
            return PartialView("_cmb_vacaciones", model);
        }
       
        #endregion
        public ActionResult Index()
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
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                fecha_fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            List<ro_Solicitud_Vacaciones_x_empleado_Info> lista = bus_solicitud.get_list(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            ro_Solicitud_Vacaciones_x_empleado_List.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            List<ro_Solicitud_Vacaciones_x_empleado_Info> lista = bus_solicitud.get_list(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            ro_Solicitud_Vacaciones_x_empleado_List.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_solicitud_vacaciones(DateTime? Fecha_ini, DateTime? Fecha_fin)
        {
            try
            {
                int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                ViewBag.Fecha_ini = Fecha_ini == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : Convert.ToDateTime(Fecha_ini);
                ViewBag.Fecha_fin = Fecha_fin == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : Convert.ToDateTime(Fecha_fin);

                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_Solicitud_Vacaciones_x_empleado_Info> model = ro_Solicitud_Vacaciones_x_empleado_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_solicitud_vacaciones", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GridViewPartial_solicitud_vacaciones_det()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                var model = ro_Solicitud_Vacaciones_x_empleado_det_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                cargar_combo();
                return PartialView("_GridViewPartial_solicitud_vacaciones_det", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GridViewPartial_vacaciones_periodos()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                var model = ro_historico_vacaciones_x_empleado_Info_list.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_vacaciones_periodos", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region acciones
        [HttpPost]
        public ActionResult Nuevo(ro_Solicitud_Vacaciones_x_empleado_Info info)
        {
            try
            {
                bus_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Bus();
               
                    string mensaje = "";
                    info.lst_vacaciones = ro_Solicitud_Vacaciones_x_empleado_det_List.get_list(info.IdTransaccionSession);
                   
                    info.IdUsuario = SessionFixed.IdUsuario;
                    mensaje = bus_solicitud.validar(info);
                    if (mensaje != "")
                    {
                        ViewBag.mensaje = mensaje;
                        cargar_combo();
                        return View(info);
                    }

                    if (!bus_solicitud.guardarDB(info))
                    {
                        cargar_combo();
                        return View(info);
                    }
                   

                    return RedirectToAction("Modificar", new { IdEmpresa = info.IdEmpresa, IdEmpleado = info.IdEmpleado, IdSolicitud = info.IdSolicitud, Exito = true });
                

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Nuevo()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            try
            {
                ro_Solicitud_Vacaciones_x_empleado_Info info = new ro_Solicitud_Vacaciones_x_empleado_Info
                {
                    IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                    Fecha_Desde = DateTime.Now,
                    Fecha_Hasta = DateTime.Now,
                    Fecha_Retorno = DateTime.Now,
                    IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                };
                ro_historico_vacaciones_x_empleado_Info_list.set_list(new List<ro_historico_vacaciones_x_empleado_Info>(), info.IdTransaccionSession);
                cargar_combo();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_Solicitud_Vacaciones_x_empleado_Info info)
        {
            try
            {
                string mensaje = "";

                bus_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Bus();
                if (ModelState.IsValid)
                {
                    

                    if (mensaje != "")
                    {
                        ViewBag.mensaje = mensaje;
                        cargar_combo();
                        return View(info);
                    }

                    if (!bus_solicitud.modificarDB(info))
                    {
                        cargar_combo();
                        return View(info);
                    }
                    else
                    {
                        return RedirectToAction("Modificar", new { IdEmpleado = info.IdEmpleado, IdSolicitud = info.IdSolicitud, Exito = true });
                    }                        
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal IdEmpleado = 0, decimal IdSolicitud = 0, bool Exito = false)
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion

                cargar_combo();
                ro_Solicitud_Vacaciones_x_empleado_Info model = bus_solicitud.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdEmpleado, IdSolicitud);
                model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
                
                if (Exito)
                    ViewBag.MensajeSuccess = MensajeSuccess;

                return View(model);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]

        public ActionResult Anular(ro_Solicitud_Vacaciones_x_empleado_Info info)
        {
            try
            {
                if (!bus_solicitud.anularDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index");


            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdEmpresa = 0, decimal IdEmpleado = 0, decimal IdSolicitud = 0)
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion

                var model = bus_solicitud.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdEmpleado, IdSolicitud);
                model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
               

                cargar_combo();
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region funciones del detalle
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ro_Solicitud_Vacaciones_x_empleado_det_Info info_det)
        {
            if (ModelState.IsValid)
                ro_Solicitud_Vacaciones_x_empleado_det_List.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
           var model_ = ro_Solicitud_Vacaciones_x_empleado_det_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combo();
            return PartialView("_GridViewPartial_solicitud_vacaciones_det", model_);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ro_Solicitud_Vacaciones_x_empleado_det_Info info_det)
        {
            if (ModelState.IsValid)
                ro_Solicitud_Vacaciones_x_empleado_det_List.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
           var model_ = ro_Solicitud_Vacaciones_x_empleado_det_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combo();
            return PartialView("_GridViewPartial_solicitud_vacaciones_det", model_);
        }

        public ActionResult EditingDelete([ModelBinder(typeof(DevExpressEditorsBinder))] ro_Solicitud_Vacaciones_x_empleado_det_Info info_det)
        {
            ro_Solicitud_Vacaciones_x_empleado_det_List.DeleteRow(info_det.Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model_ = ro_Solicitud_Vacaciones_x_empleado_det_List.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combo();

            return PartialView("_GridViewPartial_solicitud_vacaciones_det", model_);
        }
        #endregion


        private void cargar_combo()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_tipo_liquidacion = bus_catalogo.get_list_x_tipo(45);
            ViewBag.lst_tipo_liquidacion = lst_tipo_liquidacion;

            var lst_tipo_solicitud = bus_catalogo.get_list_x_tipo(46);
            ViewBag.lst_tipo_solicitud = lst_tipo_solicitud;

        }
        public JsonResult calcular_vacaciones(decimal IdEmpleado, decimal IdTransaccionSession=0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
           var lst_vacaciones_periodos= bus_vacaciones.calcular_vacaciones(IdEmpresa, IdEmpleado);// recalculando vacaciones
            ro_historico_vacaciones_x_empleado_Info_list.set_list(lst_vacaciones_periodos, IdTransaccionSession);
           return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult add_vacaciones(decimal KeyValue = 0, decimal IdTransaccionSession=0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst = ro_historico_vacaciones_x_empleado_Info_list.get_list(IdTransaccionSession);
            if(lst!=null)
            {
                var info_periodo_vac = lst.FirstOrDefault(s => s.IdVacacion == KeyValue);
                if(info_periodo_vac!=null)
                {
                    int dias = info_periodo_vac.DiasPendientes;
                    int dias_adicionales = 0;
                    ro_Solicitud_Vacaciones_x_empleado_det_Info info;
                    if (dias >15)
                    {
                        dias_adicionales = dias - 15;
                    }
                     info = new ro_Solicitud_Vacaciones_x_empleado_det_Info
                    {
                    IdPeriodo_Inicio=info_periodo_vac.IdPeriodo_Inicio,
                    IdPeriodo_Fin= info_periodo_vac.IdPeriodo_Fin,
                    FechaIni=info_periodo_vac.FechaIni,
                    FechaFin=info_periodo_vac.FechaFin,
                    Tipo_liquidacion= "GOZA",
                    Tipo_vacacion= "DIAS_VAC",
                    Dias_tomados = dias-dias_adicionales,
                    };
                   if( ro_Solicitud_Vacaciones_x_empleado_det_List.get_list(IdTransaccionSession).Where(s =>s.IdPeriodo_Inicio== info_periodo_vac.IdPeriodo_Inicio).Count()==0)
                    ro_Solicitud_Vacaciones_x_empleado_det_List.AddRow(info, IdTransaccionSession);
                    if(dias_adicionales >0)
                    {
                        info = new ro_Solicitud_Vacaciones_x_empleado_det_Info
                        {
                            IdPeriodo_Inicio = info_periodo_vac.IdPeriodo_Inicio,
                            IdPeriodo_Fin = info_periodo_vac.IdPeriodo_Fin,
                            FechaIni = info_periodo_vac.FechaIni,
                            FechaFin = info_periodo_vac.FechaFin,
                            Tipo_liquidacion = "GOZA",
                            Tipo_vacacion = "DIAS_ADIC",
                            Dias_tomados = dias_adicionales,
                        };
                        if (ro_Solicitud_Vacaciones_x_empleado_det_List.get_list(IdTransaccionSession).Where(s => s.IdPeriodo_Inicio == info_periodo_vac.IdPeriodo_Inicio&& s.Tipo_vacacion== "DIAS_ADIC").Count() == 0)
                            ro_Solicitud_Vacaciones_x_empleado_det_List.AddRow(info, IdTransaccionSession);
                    }

                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_list_vacaciones( DateTime ? FechaDesde, DateTime?FechaHasta, decimal IdEmpleado = 0, decimal IdTransaccionSession=0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int Dias_a_disfrutar = Convert.ToInt32((Convert.ToDateTime( FechaHasta).AddDays(1) -Convert.ToDateTime( FechaDesde)).TotalDays);
            var lista_vacaciones_pemdientes = bus_vacaciones.get_list_periodo_con_saldo(IdEmpresa, IdEmpleado);
            List<ro_historico_vacaciones_x_empleado_Info> lista_tmp = new List<ro_historico_vacaciones_x_empleado_Info>();
            if (lista_vacaciones_pemdientes != null)
            {
                while (Dias_a_disfrutar > 0)
                {
                  var item=  lista_vacaciones_pemdientes.FirstOrDefault();
                    if(item!=null)
                    {
                        if (Dias_a_disfrutar >= (item.DiasGanado - item.DiasTomados))
                            item.DiasTomados = (item.DiasGanado - item.DiasTomados);
                        else
                            item.DiasTomados = Dias_a_disfrutar;
                        lista_tmp.Add(item);
                        Dias_a_disfrutar = Dias_a_disfrutar - item.DiasTomados;
                        lista_vacaciones_pemdientes.Remove(item);
                    }
                    break;  
                }
            }
            ro_historico_vacaciones_x_empleado_Info_list.set_list(lista_tmp, Convert.ToDecimal(IdTransaccionSession));
            return Json("", JsonRequestBehavior.AllowGet);
        }
      
         
    }

    public class ro_Solicitud_Vacaciones_x_empleado_List
    {
        string Variable = "ro_Solicitud_Vacaciones_x_empleado_List";
        public List<ro_Solicitud_Vacaciones_x_empleado_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_Solicitud_Vacaciones_x_empleado_Info> list = new List<ro_Solicitud_Vacaciones_x_empleado_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_Solicitud_Vacaciones_x_empleado_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_Solicitud_Vacaciones_x_empleado_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    public class ro_historico_vacaciones_x_empleado_Info_list
    {
        string Variable = "ro_historico_vacaciones_x_empleado_Info";
        public List<ro_historico_vacaciones_x_empleado_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_historico_vacaciones_x_empleado_Info> list = new List<ro_historico_vacaciones_x_empleado_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_historico_vacaciones_x_empleado_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_historico_vacaciones_x_empleado_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class ro_Solicitud_Vacaciones_x_empleado_det_List
    {
        string Variable = "ro_Solicitud_Vacaciones_x_empleado_det_Info";
        public List<ro_Solicitud_Vacaciones_x_empleado_det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_Solicitud_Vacaciones_x_empleado_det_Info> list = new List<ro_Solicitud_Vacaciones_x_empleado_det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_Solicitud_Vacaciones_x_empleado_det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_Solicitud_Vacaciones_x_empleado_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ro_Solicitud_Vacaciones_x_empleado_det_Info info_det, decimal IdTransaccionSession)
        {
            ro_rubro_tipo_Bus bus_rub = new ro_rubro_tipo_Bus();
            List<ro_Solicitud_Vacaciones_x_empleado_det_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            list.Add(info_det);
        }

        public void UpdateRow(ro_Solicitud_Vacaciones_x_empleado_det_Info info_det, decimal IdTransaccionSession)
        {
            ro_Solicitud_Vacaciones_x_empleado_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.Dias_tomados = info_det.Dias_tomados;
            edited_info.Tipo_vacacion = info_det.Tipo_vacacion;
            edited_info.Tipo_liquidacion = info_det.Tipo_liquidacion;

        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<ro_Solicitud_Vacaciones_x_empleado_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).First());
        }
    }

}