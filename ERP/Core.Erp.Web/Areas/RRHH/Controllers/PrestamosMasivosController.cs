using Core.Erp.Bus.General;
using Core.Erp.Bus.RRHH;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Info.RRHH;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class PrestamosMasivosController : Controller
    {
        #region Variables
        ro_prestamo_Bus bus_prestamos = new ro_prestamo_Bus();
        ro_nomina_tipo_Bus bus_nomina = new ro_nomina_tipo_Bus();
        ro_Nomina_Tipoliquiliqui_Bus bus_nomina_tipo = new ro_Nomina_Tipoliquiliqui_Bus();
        ro_prestamo_detalle_Bus bus_detalle = new ro_prestamo_detalle_Bus();
        ro_prestamo_detalle_lst Lis_ro_prestamo_detalle_lst = new ro_prestamo_detalle_lst();
        ro_rubro_tipo_Bus bus_rubro = new ro_rubro_tipo_Bus();
        ro_empleado_Bus bus_empleado = new ro_empleado_Bus();
        ro_catalogo_Bus bus_catalogo = new ro_catalogo_Bus();
        int IdEmpresa = 0;
        ro_prestamo_Info info = new ro_prestamo_Info();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        #endregion

        #region Metodos ComboBox bajo demanda
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbEmpleado_prestamos()
        {
            ro_prestamo_Info model = new ro_prestamo_Info();
            return PartialView("_CmbEmpleado_prestamos", model);
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

        #region vistas
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdEmpleado = 0
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            return View(model);
        }


        [ValidateInput(false)]
        public ActionResult GridViewPartial_prestamos_masivos(decimal IdEmpleado = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.IdEmpleado = IdEmpleado == 0 ? 0 : IdEmpleado;
            List<ro_prestamo_Info> model = bus_prestamos.get_list_prestamo(IdEmpresa, IdEmpleado);
            return PartialView("_GridViewPartial_prestamos_masivos", model);
        }
        #endregion
    }
}