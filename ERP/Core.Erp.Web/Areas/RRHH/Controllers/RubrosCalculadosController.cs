using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using Core.Erp.Web.Helps;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class RubrosCalculadosController : Controller
    {
        ro_rubros_calculados_Bus bus_rubro_calculado = new ro_rubros_calculados_Bus();
        ro_rubro_tipo_Bus bus_rubro = new ro_rubro_tipo_Bus();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_rubros_calculados_Info model = new ro_rubros_calculados_Info();
            model = bus_rubro_calculado.get_info(Convert.ToInt32(SessionFixed.IdEmpresa));
            cargar_combos();
            return View(model);
        }

       
        [HttpPost]
        public ActionResult Index(ro_rubros_calculados_Info info)
        {
            try
            {
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    if (!bus_rubro_calculado.guardarDB(info))
                    {
                        cargar_combos();
                        return View(info);
                    }
                    else
                    {
                        cargar_combos();
                        return View(info);

                    }
                
               
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        private void cargar_combos()
        {
            try
            {
                int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                ViewBag.lst_rubro = bus_rubro.get_list(IdEmpresa, false);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}