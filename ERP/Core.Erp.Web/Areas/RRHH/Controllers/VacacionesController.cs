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
        List<ro_historico_vacaciones_x_empleado_Info> lst_vacaciones = new List<ro_historico_vacaciones_x_empleado_Info>();
        ro_Solicitud_Vacaciones_x_empleado_Bus bus_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Bus();
        ro_empleado_Bus bus_empleado = new ro_empleado_Bus();
        ro_historico_vacaciones_x_empleado_Info_list ro_historico_vacaciones_x_empleado_Info_list = new ro_historico_vacaciones_x_empleado_Info_list();
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
            ro_historico_vacaciones_x_empleado_Info model = new ro_historico_vacaciones_x_empleado_Info();
            return PartialView("_cmb_vacaciones", model);
        }
        public List<ro_historico_vacaciones_x_empleado_Info> get_list_bajo_demanda_periodo(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_vacaciones.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        public ro_historico_vacaciones_x_empleado_Info get_info_bajo_demanda_periodo(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_vacaciones.get_info_bajo_demanda(Convert.ToInt32(SessionFixed.IdEmpresa), args);
        }
        #endregion
        int IdEmpresa = 0;
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
        public ActionResult GridViewPartial_solicitud_vacaciones(DateTime? Fecha_ini, DateTime? Fecha_fin)
        {
            try
            {
                IdEmpresa = GetIdEmpresa();
                ViewBag.Fecha_ini = Fecha_ini == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : Convert.ToDateTime(Fecha_ini);
                ViewBag.Fecha_fin = Fecha_fin == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : Convert.ToDateTime(Fecha_fin);

                List<ro_Solicitud_Vacaciones_x_empleado_Info> model = bus_solicitud.get_list(IdEmpresa, ViewBag.Fecha_ini, ViewBag.Fecha_fin);
                return PartialView("_GridViewPartial_solicitud_vacaciones", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

       /* public ActionResult cmb_vacaciones()
        {
            try
            {
                IdEmpresa = GetIdEmpresa();
                List<ro_historico_vacaciones_x_empleado_Info> model = new List<ro_historico_vacaciones_x_empleado_Info>();
                model=ro_historico_vacaciones_x_empleado_Info_list.get_list();
                
                return PartialView("_cmb_vacaciones", model);
            }
            catch (Exception)
            {

                throw;
            }
        }*/
        #region acciones
        [HttpPost]
        public ActionResult Nuevo(ro_Solicitud_Vacaciones_x_empleado_Info info)
        {
            try
            {
                bus_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Bus();
                if (ModelState.IsValid)
                {
                    string mensaje = "";
                    ro_historico_vacaciones_x_empleado_Info info_historico = null;
                    lst_vacaciones =ro_historico_vacaciones_x_empleado_Info_list.get_list();
                    info_historico = lst_vacaciones.Where(v => v.IdVacacion == info.IdVacacion).FirstOrDefault();
                    info.Dias_a_disfrutar = Convert.ToInt32((info.Fecha_Hasta.AddDays(1) - info.Fecha_Desde).TotalDays);
                    info.Dias_q_Corresponde = info_historico.DiasGanado;
                    info.Dias_pendiente = info_historico.DiasGanado - info.Dias_a_disfrutar;
                    info.Anio_Desde = info_historico.FechaIni.Date;
                    info.Anio_Hasta = info_historico.FechaFin.Date;
                    info.IdVacacion = info_historico.IdVacacion;
                    info.Fecha_Desde = info.Fecha_Desde.Date;
                    info.Fecha_Hasta = info.Fecha_Hasta.Date;
                    mensaje = bus_solicitud.validar(info);
                    if (mensaje != "")
                    {
                        ViewBag.mensaje = mensaje;
                        cargar_combo();
                        return View(info);
                    }
                    info.IdEmpresa = GetIdEmpresa();
                    if (!bus_solicitud.guardarDB(info))
                    {
                        cargar_combo();
                        return View(info);
                    }
                    else
                        info_historico.DiasTomados=info.Dias_a_disfrutar;
                    bus_vacaciones = new ro_historico_vacaciones_x_empleado_Bus();
                        bus_vacaciones.ModificarBD(info_historico);
                    return RedirectToAction("Index");
                }
                else
                    return View(info);

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
                ro_Solicitud_Vacaciones_x_empleado_Info info = new ro_Solicitud_Vacaciones_x_empleado_Info
                {
                    Fecha_Desde = DateTime.Now,
                    Fecha_Hasta = DateTime.Now,
                    Fecha_Retorno = DateTime.Now,
                    IdVacacion = 1,
                };
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
                bus_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Bus();
                if (ModelState.IsValid)
                {
                    string mensaje = "";
                    ro_historico_vacaciones_x_empleado_Info info_historico = null;
                    lst_vacaciones = ro_historico_vacaciones_x_empleado_Info_list.get_list();
                    info_historico = lst_vacaciones.Where(v => v.IdVacacion == info.IdVacacion).FirstOrDefault();
                    info.Dias_a_disfrutar = Convert.ToInt32((info.Fecha_Hasta - info.Fecha_Desde).TotalDays)+1;
                    info.Dias_q_Corresponde = info_historico.DiasGanado;
                    info.Dias_pendiente = info_historico.DiasGanado - info.Dias_a_disfrutar;
                    info.Anio_Desde = info_historico.FechaIni.Date;
                    info.Anio_Hasta = info_historico.FechaFin.Date;
                    info.IdVacacion = info_historico.IdVacacion;
                    info.Fecha_Desde = info.Fecha_Desde.Date;
                    info.Fecha_Hasta = info.Fecha_Hasta.Date;
                    mensaje = bus_solicitud.validar(info);
                    if (mensaje != "")
                    {
                        ViewBag.mensaje = mensaje;
                        cargar_combo();
                        return View(info);
                    }
                    info.IdEmpresa = GetIdEmpresa();
                    if (!bus_solicitud.modificarDB(info))
                    {
                        cargar_combo();
                        return View(info);
                    }
                    else
                        return RedirectToAction("Index");
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Modificar(decimal IdEmpleado = 0, decimal IdSolicitud = 0)
        {
            try
            {
                IdEmpresa = GetIdEmpresa();
                lst_vacaciones = bus_vacaciones.get_lst_vaciones_x_empleado(IdEmpresa, IdEmpleado);
                ro_historico_vacaciones_x_empleado_Info_list.set_list(lst_vacaciones);
                cargar_combo();
                ro_Solicitud_Vacaciones_x_empleado_Info model = bus_solicitud.get_info(GetIdEmpresa(), IdEmpleado, IdSolicitud);
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
        public ActionResult Anular(decimal IdEmpleado = 0, decimal IdSolicitud = 0)
        {
            try
            {

                IdEmpresa = GetIdEmpresa();
                lst_vacaciones = bus_vacaciones.get_lst_vaciones_x_empleado(IdEmpresa, IdEmpleado);
                ro_historico_vacaciones_x_empleado_Info_list.set_list(lst_vacaciones);

                cargar_combo();
                return View(bus_solicitud.get_info(GetIdEmpresa(), IdEmpleado, IdSolicitud));
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        private int GetIdEmpresa()
        {
            try
            {
                if (Session["IdEmpresa"] != null)
                    return Convert.ToInt32(Session["IdEmpresa"]);
                else
                    return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cargar_combo()
        {
            IdEmpresa = GetIdEmpresa();
            ViewBag.lst_empleado = bus_empleado.get_list_combo(IdEmpresa);
            ViewBag.lst_vacaciones = lst_vacaciones;
        }
        public JsonResult get_list_vacaciones(decimal IdEmpleado)
        {
            IdEmpresa = GetIdEmpresa();
             bus_vacaciones.get_lst_vaciones_x_empleado(IdEmpresa, IdEmpleado);// recalculando vacaciones
            lst_vacaciones = bus_vacaciones.get_list(IdEmpresa, IdEmpleado);
            ro_historico_vacaciones_x_empleado_Info_list.set_list(lst_vacaciones);

           return Json(lst_vacaciones, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult GridLookupPartial_vacaciones()
        {
            lst_vacaciones = ro_historico_vacaciones_x_empleado_Info_list.get_list();
            return PartialView("_GridViewPartial_vaciones_periodos", lst_vacaciones);
        }

         
    }


    public class ro_historico_vacaciones_x_empleado_Info_list
    {
        string variable = "";
        public List<ro_historico_vacaciones_x_empleado_Info> get_list()
        {
            if (HttpContext.Current.Session[variable] == null)
            {
                List<ro_historico_vacaciones_x_empleado_Info> list = new List<ro_historico_vacaciones_x_empleado_Info>();

                HttpContext.Current.Session[variable] = list;
            }
            return (List<ro_historico_vacaciones_x_empleado_Info>)HttpContext.Current.Session[variable];
        }

        public void set_list(List<ro_historico_vacaciones_x_empleado_Info> list)
        {
            HttpContext.Current.Session[variable] = list;
        }

    }
}