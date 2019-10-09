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
    public class TipoGastosPersonalesController : Controller
    {
        ro_tipo_gastos_personales_Bus bus_gastos = new ro_tipo_gastos_personales_Bus();
        ro_tipo_gastos_personales_List Lista_GastosPersonales = new ro_tipo_gastos_personales_List();
        // GET: RRHH/Division
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_tipo_gastos_personales_Info model = new ro_tipo_gastos_personales_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_tipo_gastos_personales_Info> lista = bus_gastos.get_list(true);
            Lista_GastosPersonales.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipo_gastos_personales()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_tipo_gastos_personales_Info> model = Lista_GastosPersonales.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_tipo_gastos_personales", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Nuevo(ro_tipo_gastos_personales_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = bus_gastos.get_info(info.IdTipoGasto);
                    if(model!=null){
                        ViewBag.mensaje = "El codigo ya se encuentra registrado";
                        return View(model);
                    }
                    if (!bus_gastos.guardarDB(info))
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
                ro_tipo_gastos_personales_Info info = new ro_tipo_gastos_personales_Info();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Modificar(ro_tipo_gastos_personales_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_gastos.modificarDB(info))
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
        public ActionResult Modificar(string IdTipoGasto )
        {
            try
            {

                return View(bus_gastos.get_info(IdTipoGasto));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Anular(ro_tipo_gastos_personales_Info info)
        {
            try
            {

                if (!bus_gastos.anularDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(string IdTipoGasto )
        {
            try
            {

                return View(bus_gastos.get_info(IdTipoGasto));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_tipo_gastos_personales_List
    {
        string Variable = "ro_tipo_gastos_personales_Info";
        public List<ro_tipo_gastos_personales_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_tipo_gastos_personales_Info> list = new List<ro_tipo_gastos_personales_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_tipo_gastos_personales_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_tipo_gastos_personales_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}