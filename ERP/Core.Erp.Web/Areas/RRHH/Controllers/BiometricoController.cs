using Core.Erp.Bus.RRHH;
using Core.Erp.Info.RRHH;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class BiometricoController : Controller
    {
        ro_biometrico_Bus bu_biometrico = new ro_biometrico_Bus();

        // GET: RRHH/Biometrico
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_biometrico()
        {
            try
            {
                List<ro_biometrico_Info> model = bu_biometrico.get_list(Convert.ToInt32(SessionFixed.IdEmpresa), true);
                return PartialView("_GridViewPartial_biometrico", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Nuevo(ro_biometrico_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    if (!bu_biometrico.guardarDB(info))
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
                ro_biometrico_Info info = new ro_biometrico_Info();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Modificar(ro_biometrico_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

                    if (!bu_biometrico.modificarDB(info))
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
        public ActionResult Modificar(int IdDivision = 0)
        {
            try
            {

                return View(bu_biometrico.get_info(Convert.ToInt32( SessionFixed.IdEmpresa), IdDivision));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Anular(ro_biometrico_Info info)
        {
            try
            {
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                if (!bu_biometrico.anularDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdDivision = 0)
        {
            try
            {

                return View(bu_biometrico.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdDivision));

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}