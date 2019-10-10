using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.RRHH;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.General;
using Core.Erp.Info.General;
using DevExpress.Web;
using Core.Erp.Web.Helps;
using Core.Erp.Info.Helps;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class MarcacionEmpleadoController : Controller
    {
        ro_catalogo_Bus bus_catalogo = new ro_catalogo_Bus();
        ro_marcaciones_x_empleado_Bus bus_marcaciones = new ro_marcaciones_x_empleado_Bus();
        ro_empleado_Bus bus_empleado = new ro_empleado_Bus();
        ro_marcaciones_tipo_Bus bus_tipo = new ro_marcaciones_tipo_Bus();
        ro_marcaciones_x_empleado_List Lista_Marcarciones = new ro_marcaciones_x_empleado_List();
        int IdEmpresa = 0;
        #region Metodos ComboBox bajo demanda
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbEmpleado_Marcacion()
        {
            decimal model = new decimal();
            return PartialView("_CmbEmpleado_Marcacion", model);
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
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
        };

            List<ro_marcaciones_x_empleado_Info> lista = bus_marcaciones.get_list(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            Lista_Marcarciones.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            List<ro_marcaciones_x_empleado_Info> lista = bus_marcaciones.get_list(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            Lista_Marcarciones.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));

            return View(model);

        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_marcaciones_empleado(DateTime? Fecha_ini, DateTime? Fecha_fin)
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_marcaciones_x_empleado_Info> model = Lista_Marcarciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_marcaciones_empleado", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_marcaciones_x_empleado_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdUsuario = SessionFixed.IdUsuario;
                    if (!bus_marcaciones.guardarDB(info))
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
        public ActionResult Nuevo()
        {
            try
            {
                ro_marcaciones_x_empleado_Info info = new ro_marcaciones_x_empleado_Info
                {
                    IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                    es_fechaRegistro = DateTime.Now.Date,
                    es_Hora = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
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
        public ActionResult Modificar(ro_marcaciones_x_empleado_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_marcaciones.modificarDB(info))
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
        public ActionResult Modificar(int IdEmpresa=0, decimal IdEmpleado = 0, decimal IdRegistro = 0)
        {
            try
            {
                cargar_combo();
                return View(bus_marcaciones.get_info(IdEmpresa, IdEmpleado, IdRegistro));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Anular(ro_marcaciones_x_empleado_Info info)
        {
            try
            {

                if (!bus_marcaciones.anularDB(info))
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
        public ActionResult Anular(int IdEmpresa=0, decimal IdEmpleado = 0, decimal IdRegistro = 0)
        {
            try
            {
                cargar_combo();
                return View(bus_marcaciones.get_info(IdEmpresa, IdEmpleado, IdRegistro));

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void cargar_combo()
        {
            try
            {
                bus_tipo = new ro_marcaciones_tipo_Bus();
                ViewBag.lst_tipo = bus_tipo.get_list();

                bus_empleado = new ro_empleado_Bus();
                bus_catalogo = new ro_catalogo_Bus();
                ro_nomina_tipo_Bus bus_nomina = new ro_nomina_tipo_Bus();
                int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                ViewBag.lst_empleado = bus_empleado.get_list_combo(IdEmpresa);
                ViewBag.lst_tipomarcacion = bus_catalogo.get_list_x_tipo(18);

               
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_marcaciones_x_empleado_List
    {
        string Variable = "ro_marcaciones_x_empleado_Info";
        public List<ro_marcaciones_x_empleado_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_marcaciones_x_empleado_Info> list = new List<ro_marcaciones_x_empleado_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_marcaciones_x_empleado_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_marcaciones_x_empleado_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}