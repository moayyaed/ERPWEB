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
    public class AreaController : Controller
    {

        ro_area_Bus bus_area = new ro_area_Bus();
        List<ro_division_Info> lista_division = new List<ro_division_Info>();
        ro_division_Bus bus_division = new ro_division_Bus();
        ro_area_List Lista_Area = new ro_area_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_area_Info model = new ro_area_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_area_Info> lista = bus_area.get_list(model.IdEmpresa, true);
            Lista_Area.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_area()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_area_Info> model = Lista_Area.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_area", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_area_Info info)
        {
            try
            {

                info.IdUsuario = SessionFixed.IdUsuario;
                if (ModelState.IsValid)
                {
                    if (!bus_area.guardarDB(info))
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
                cargar_combos();
                ro_area_Info info = new ro_area_Info();
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_area_Info info)
        {
            try
            {
                info.IdUsuarioUltMod = SessionFixed.IdUsuario;
                if (ModelState.IsValid)
                {
                    if (!bus_area.modificarDB(info))
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

        public ActionResult Modificar(int IdEmpresa = 0, int IdArea = 0)
        {
            try
            {
                cargar_combos();

                return View(bus_area.get_info(IdEmpresa, IdArea));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]

        public ActionResult Anular(ro_area_Info info)
        {
            try
            {
                info.IdUsuario = SessionFixed.IdUsuario;
                if (!bus_area.anularDB(info))
                        return View(info);
                    else
                        return RedirectToAction("Index");              

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdEmpresa=0, int IdArea = 0)
        {
            try
            {
                cargar_combos();
                return View(bus_area.get_info(IdEmpresa, IdArea));

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
                lista_division = bus_division.get_list(IdEmpresa, false);
                ViewBag.lista_division = lista_division;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public JsonResult get_areas(int IdDivision)
        {
            try
            {
                int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                List<ro_area_Info> lst_areas = new List<ro_area_Info>();
                lst_areas = bus_area.get_list(IdEmpresa, IdDivision);
                return Json(lst_areas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_area_List
    {
        string Variable = "ro_area_Info";
        public List<ro_area_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_area_Info> list = new List<ro_area_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_area_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_area_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}