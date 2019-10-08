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
    public class HorarioController : Controller
    {
        ro_horario_Bus bus_cargo = new ro_horario_Bus();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_horarios()
        {
            try
            {
                int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                List<ro_horario_Info> model = bus_cargo.get_list(IdEmpresa, true);
                return PartialView("_GridViewPartial_horarios", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_horario_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdUsuario = SessionFixed.IdUsuario;
                    if (!bus_cargo.guardarDB(info))
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
                ro_horario_Info info = new ro_horario_Info();
                info.IdEmpresa= Convert.ToInt32(SessionFixed.IdEmpresa);
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_horario_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdUsuarioUltMod = SessionFixed.IdUsuario;
                    if (!bus_cargo.modificarDB(info))
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

        public ActionResult Modificar(int IdEmpresa=0, int IdHorario = 0)
        {
            try
            {
                return View(bus_cargo.get_info(IdEmpresa, IdHorario));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]

        public ActionResult Anular(ro_horario_Info info)
        {
            try
            {
                info.IdUsuarioUltAnu = SessionFixed.IdUsuario;
                if (!bus_cargo.anularDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index");


            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdEmpresa=0, int IdHorario = 0)
        {
            try
            {
                return View(bus_cargo.get_info(IdEmpresa, IdHorario));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_horario_List
    {
        string Variable = "ro_horario_Info";
        public List<ro_horario_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_horario_Info> list = new List<ro_horario_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_horario_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_horario_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}