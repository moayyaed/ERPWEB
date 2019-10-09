using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class BaseImpuestoRentaController : Controller
    {
        ro_tabla_Impu_Renta_Bus bus_imp_renta = new ro_tabla_Impu_Renta_Bus();
        ro_tabla_Impu_Renta_List Lista_ImpRenta = new ro_tabla_Impu_Renta_List();
        // GET: RRHH/Division
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_tabla_Impu_Renta_Info model = new ro_tabla_Impu_Renta_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_tabla_Impu_Renta_Info> lista = bus_imp_renta.get_list();
            Lista_ImpRenta.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_base_impuesto_renta()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_tabla_Impu_Renta_Info> model = Lista_ImpRenta.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_base_impuesto_renta", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Nuevo(ro_tabla_Impu_Renta_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    if (!bus_imp_renta.guardarDB(info))
                        return View(info);
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
                ro_tabla_Impu_Renta_Info info = new ro_tabla_Impu_Renta_Info();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Modificar(ro_tabla_Impu_Renta_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_imp_renta.modificarDB(info))
                        return View(info);
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
        public ActionResult Modificar(int AnioFiscal, int Secuencia = 0)
        {
            try
            {

                return View(bus_imp_renta.get_info( AnioFiscal, Secuencia));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Anular(ro_tabla_Impu_Renta_Info info)
        {
            try
            {

                if (!bus_imp_renta.anularDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int AnioFiscal, int Secuencia = 0)
        {
            try
            {

                return View(bus_imp_renta.get_info(AnioFiscal, Secuencia));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_tabla_Impu_Renta_List
    {
        string Variable = "ro_tabla_Impu_Renta_Info";
        public List<ro_tabla_Impu_Renta_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_tabla_Impu_Renta_Info> list = new List<ro_tabla_Impu_Renta_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_tabla_Impu_Renta_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_tabla_Impu_Renta_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}