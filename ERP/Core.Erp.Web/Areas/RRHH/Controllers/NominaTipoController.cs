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
    public class NominaTipoController : Controller
    {
        ro_nomina_tipo_Bus bus_nomina_tipo = new ro_nomina_tipo_Bus();
        ro_nomina_tipo_List ListaNominaTipo = new ro_nomina_tipo_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ro_nomina_tipo_Info model = new ro_nomina_tipo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            List<ro_nomina_tipo_Info> lista = bus_nomina_tipo.get_list(model.IdEmpresa, true);
            ListaNominaTipo.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_nomina_tipo()
        {
            try
            {
                List<ro_nomina_tipo_Info> model = ListaNominaTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_nomina_tipo", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Nuevo(ro_nomina_tipo_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_nomina_tipo.guardarDB(info))
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
                ro_nomina_tipo_Info info = new ro_nomina_tipo_Info();
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Modificar(ro_nomina_tipo_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_nomina_tipo.modificarDB(info))
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
        public ActionResult Modificar(int IdEmpresa=0, int IdNomina_Tipo = 0)
        {
            try
            {

                return View(bus_nomina_tipo.get_info(IdEmpresa, IdNomina_Tipo));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Anular(ro_nomina_tipo_Info info)
        {
            try
            {
                    if (!bus_nomina_tipo.anularDB(info))
                        return View(info);
                    else
                        return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdEmpresa=0, int IdNomina_Tipo = 0)
        {
            try
            {

                return View(bus_nomina_tipo.get_info(IdEmpresa, IdNomina_Tipo));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_nomina_tipo_List
    {
        string Variable = "ro_nomina_tipo_Info";
        public List<ro_nomina_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_nomina_tipo_Info> list = new List<ro_nomina_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_nomina_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_nomina_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}