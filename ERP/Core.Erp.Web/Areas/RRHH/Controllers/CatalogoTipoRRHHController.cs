using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.RRHH;
using Core.Erp.Info.RRHH;
using Core.Erp.Web.Helps;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class CatalogoTipoRRHHController : Controller
    {
        ro_catalogoTipo_Bus bus_catalogoTipo = new ro_catalogoTipo_Bus();
        ro_catalogoTipo_List Lista_CatalogoTipo = new ro_catalogoTipo_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_catalogoTipo_Info model = new ro_catalogoTipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_catalogoTipo_Info> lista = bus_catalogoTipo.get_list(true);
            Lista_CatalogoTipo.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_CatalogoTipo()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_catalogoTipo_Info> model = Lista_CatalogoTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_CatalogoTipo", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_catalogoTipo_Info info)
        {
            try
            {
                info.IdUsuario = SessionFixed.IdUsuario; if (ModelState.IsValid)
                {
                    info.IdUsuario = Session["IdUsuario"].ToString();
                    if (!bus_catalogoTipo.guardarDB(info))
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
                ro_catalogoTipo_Info info = new ro_catalogoTipo_Info();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_catalogoTipo_Info info)
        {
            try
            {
                info.IdUsuarioUltMod = SessionFixed.IdUsuario;
                    if (ModelState.IsValid)
                {
                    info.IdUsuarioUltMod = Session["IdUsuario"].ToString();
                    info.Fecha_UltMod = DateTime.Now;
                    if (!bus_catalogoTipo.modificarDB(info))
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

        public ActionResult Modificar(int IdTipoCatalogo = 0)
        {
            try
            {
                return View(bus_catalogoTipo.get_info( IdTipoCatalogo));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]

        public ActionResult Anular(ro_catalogoTipo_Info info)
        {
            try
            {
                info.IdUsuarioUltAnu = SessionFixed.IdUsuario;
                info.Fecha_UltAnu = DateTime.Now;
                if (!bus_catalogoTipo.anularDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index");


            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdTipoCatalogo = 0)
        {
            try
            {
                return View(bus_catalogoTipo.get_info(IdTipoCatalogo));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ro_catalogoTipo_List
    {
        string Variable = "ro_catalogoTipo_Info";
        public List<ro_catalogoTipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_catalogoTipo_Info> list = new List<ro_catalogoTipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_catalogoTipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_catalogoTipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}