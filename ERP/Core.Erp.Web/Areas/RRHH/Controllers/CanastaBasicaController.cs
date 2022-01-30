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

    public class CanastaBasicaController : Controller
    {
        ro_canasta_basica_Bus bus_canasta = new ro_canasta_basica_Bus();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_canasta_basica_Info model = new ro_canasta_basica_Info
            {
                Anio = DateTime.Now.Year
            };


            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_canasta_basica()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_canasta_basica_Info> model = bus_canasta.get_list();
                return PartialView("_GridViewPartial_canasta_basica", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_canasta_basica_Info info)
        {
            try
            {
                info.IdUsuario = SessionFixed.IdUsuario;
                if (ModelState.IsValid)
                {
                    if (!bus_canasta.guardarDB(info))
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
                ro_canasta_basica_Info info = new ro_canasta_basica_Info();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_canasta_basica_Info info)
        {
            try
            {
                info.IdUsuarioUltMod = SessionFixed.IdUsuario;
                if (ModelState.IsValid)
                {
                    if (!bus_canasta.modificarDB(info))
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

        public ActionResult Modificar(int IdEmpresa = 0, int Anio = 0)
        {
            try
            {
                return View(bus_canasta.get_info(IdEmpresa, Anio));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]

       
        public ActionResult Anular(int IdEmpresa = 0, int Anio = 0)
        {
            try
            {
                return View(bus_canasta.get_info(IdEmpresa, Anio));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
