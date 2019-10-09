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
    public class DepartamentoController : Controller
    {
        ro_departamento_Bus bus_departamento = new ro_departamento_Bus();
        ro_departamento_List Lista_Departamento = new ro_departamento_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_departamento_Info model = new ro_departamento_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_departamento_Info> lista = bus_departamento.get_list(model.IdEmpresa, true);
            Lista_Departamento.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_departamento()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_departamento_Info> model = Lista_Departamento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_departamento", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_departamento_Info info)
        {
            try
            {
                info.IdUsuario = SessionFixed.IdUsuario;
                if (ModelState.IsValid)
                {
                    if (!bus_departamento.guardarDB(info))
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
        public ActionResult Nuevo(int IdEmpresa=0)
        {
            try
            {
                ro_departamento_Info info = new ro_departamento_Info();
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_departamento_Info info)
        {
            try
            {
                info.IdUsuarioUltMod = SessionFixed.IdUsuario;
                if (ModelState.IsValid)
                {
                    if (!bus_departamento.modificarDB(info))
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

        public ActionResult Modificar(int IdEmpresa=0, int IdDepartamento = 0)
        {
            try
            {
                return View(bus_departamento.get_info(IdEmpresa, IdDepartamento));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]

        public ActionResult Anular(ro_departamento_Info info)
        {
            try
            {
                info.IdUsuarioUltMod = SessionFixed.IdUsuario;

                if (!bus_departamento.anularDB(info))
                        return View(info);
                    else
                        return RedirectToAction("Index");
              
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdEmpresa=0, int IdDepartamento = 0)
        {
            try
            {
                return View(bus_departamento.get_info(IdEmpresa, IdDepartamento));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_departamento_List
    {
        string Variable = "ro_departamento_Info";
        public List<ro_departamento_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_departamento_Info> list = new List<ro_departamento_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_departamento_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_departamento_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}