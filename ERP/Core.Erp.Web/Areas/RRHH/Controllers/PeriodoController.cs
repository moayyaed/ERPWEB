using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using Core.Erp.Info.General;
using Core.Erp.Bus.General;
using Core.Erp.Web.Helps;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class PeriodoController : Controller
    {
        ro_periodo_Bus bus_periodo = new ro_periodo_Bus();
        tb_region_Bus bus_region = new tb_region_Bus();
        List<tb_region_Info> lst_region = new List<tb_region_Info>();
        ro_catalogo_Bus bus_catalogo = new ro_catalogo_Bus();
        List<ro_catalogo_Info> lista_catalogo = new List<ro_catalogo_Info>();
        ro_periodo_List Lista_Periodo = new ro_periodo_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_periodo_Info model = new ro_periodo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_periodo_Info> lista = bus_periodo.get_list(model.IdEmpresa, true);
            Lista_Periodo.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_periodo()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_periodo_Info> model = Lista_Periodo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
                return PartialView("_GridViewPartial_periodo", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_periodo_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!bus_periodo.guardarDB(info))
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
                ro_periodo_Info info = new ro_periodo_Info();
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                cargar_combo();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_periodo_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdUsuarioUltMod = SessionFixed.IdUsuario;

                    if (!bus_periodo.modificarDB(info))
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
        public ActionResult Modificar(int IdEmpresa=0, int IdPeriodo = 0)
        {
            try
            {
                cargar_combo();
                return View(bus_periodo.get_info(IdEmpresa, IdPeriodo));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Anular(ro_periodo_Info info)
        {
            try
            {
                info.IdUsuarioUltAnu = SessionFixed.IdUsuario;
                    if (!bus_periodo.anularDB(info))
                        return View(info);
                    else
                        return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdEmpresa = 0, int IdPeriodo = 0)
        {
            try
            {
                cargar_combo();
                return View(bus_periodo.get_info(IdEmpresa, IdPeriodo));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Generar_Periodos(ro_periodo_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdUsuario = SessionFixed.IdUsuario;
                    if (!bus_periodo.Generar_Periodos(info))
                    {
                        cargar_combo();
                        return View(info);
                    }
                    else
                        return RedirectToAction("Index");
                }
                else
                {
                    cargar_combo();
                    return View(info);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Generar_Periodos()
        {
            try
            {
                ro_periodo_Info info = new ro_periodo_Info();
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                cargar_combo();
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
    
        private void cargar_combo()
        {
            try
            {
                lista_catalogo = bus_catalogo.get_list_x_tipo(17);
                lst_region = bus_region.get_list("1", false);
                lst_region = bus_region.get_list( "1", false);
                ViewBag.lst_region = lst_region;
                ViewBag.lista_catalogo = lista_catalogo;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }

    public class ro_periodo_List
    {
        string Variable = "ro_periodo_Info";
        public List<ro_periodo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_periodo_Info> list = new List<ro_periodo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_periodo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_periodo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}