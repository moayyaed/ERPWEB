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
    public class TipoPrestamoController : Controller
    {
        ro_tipo_prestamo_Bus bus_tipo_prestamo = new ro_tipo_prestamo_Bus();
        ro_tipo_prestamo_List Lista_TipoPrestamo = new ro_tipo_prestamo_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_tipo_prestamo_Info model = new ro_tipo_prestamo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_tipo_prestamo_Info> lista = bus_tipo_prestamo.get_list(model.IdEmpresa, true);
            Lista_TipoPrestamo.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipo_prestamo()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_tipo_prestamo_Info> model = Lista_TipoPrestamo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_tipo_prestamo", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Nuevo(ro_tipo_prestamo_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_tipo_prestamo.guardarDB(info))
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
                ro_tipo_prestamo_Info info = new ro_tipo_prestamo_Info();
                info.IdEmpresa= Convert.ToInt32(SessionFixed.IdEmpresa);
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Modificar(ro_tipo_prestamo_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_tipo_prestamo.modificarDB(info))
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
        public ActionResult Modificar(int IdEmpresa = 0, int IdTipoPrestamo = 0)
        {
            try
            {

                return View(bus_tipo_prestamo.get_info(IdEmpresa, IdTipoPrestamo));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Anular(ro_tipo_prestamo_Info info)
        {
            try
            {
                    if (!bus_tipo_prestamo.anularDB(info))
                        return View(info);
                    else
                        return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdEmpresa=0, int IdTipoPrestamo = 0)
        {
            try
            {

                return View(bus_tipo_prestamo.get_info(IdEmpresa, IdTipoPrestamo));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_tipo_prestamo_List
    {
        string Variable = "ro_tipo_prestamo_Info";
        public List<ro_tipo_prestamo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_tipo_prestamo_Info> list = new List<ro_tipo_prestamo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_tipo_prestamo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_tipo_prestamo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}