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
    public class NominaTipoLiquidacionController : Controller
    {
        ro_Nomina_Tipoliquiliqui_Bus bus_nomina_tipo = new ro_Nomina_Tipoliquiliqui_Bus();
        ro_nomina_tipo_Bus bus_nomina = new ro_nomina_tipo_Bus();
        List<ro_nomina_tipo_Info> lst_nominas = new List<ro_nomina_tipo_Info>();
        ro_Nomina_Tipoliqui_List Lista_NominaTipoLiqui = new ro_Nomina_Tipoliqui_List();

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_Nomina_Tipoliqui_Info model = new ro_Nomina_Tipoliqui_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_Nomina_Tipoliqui_Info> lista = bus_nomina_tipo.get_list(model.IdEmpresa, true);
            Lista_NominaTipoLiqui.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_nomina_tipo_liquidacion()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_Nomina_Tipoliqui_Info> model = Lista_NominaTipoLiqui.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_nomina_tipo_liquidacion", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Nuevo(ro_Nomina_Tipoliqui_Info info)
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
                cargar_combos();
                ro_Nomina_Tipoliqui_Info info = new ro_Nomina_Tipoliqui_Info
                {
                    IdNomina_Tipo = 1
                };
                info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Modificar(ro_Nomina_Tipoliqui_Info info)
        {
            try
            {
                cargar_combos();
                if (ModelState.IsValid)
                {
                    info.IdUsuarioUltModi = SessionFixed.IdUsuario;
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
        public ActionResult Modificar(int IdEmpresa = 0, int IdNomina_Tipo = 0, int IdNomina_TipoLiqui=0)
        {
            try
            {
                cargar_combos();
                return View(bus_nomina_tipo.get_info(IdEmpresa, IdNomina_Tipo, IdNomina_TipoLiqui));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Anular(ro_Nomina_Tipoliqui_Info info)
        {
            try
            {
                info.IdUsuarioAnu = SessionFixed.IdUsuario;
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
        public ActionResult Anular(int IdEmpresa=0, int IdNomina_Tipo = 0, int IdNomina_TipoLiqui=0)
        {
            try
            {
                cargar_combos();
                return View(bus_nomina_tipo.get_info(IdEmpresa, IdNomina_Tipo, IdNomina_TipoLiqui));

            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult get_lst_nomina_tipo_liq(int IdNomina = 0)
        {
            try
            {
                int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                List<ro_Nomina_Tipoliqui_Info> lst_tipo_nomina = new List<ro_Nomina_Tipoliqui_Info>();
                lst_tipo_nomina = bus_nomina_tipo.get_list(IdEmpresa, IdNomina);
                return Json(lst_tipo_nomina, JsonRequestBehavior.AllowGet);
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
                lst_nominas = bus_nomina.get_list(IdEmpresa, false);
                ViewBag.lst_nomina = lst_nominas;
            }
            catch (Exception)
            {
               
                throw;
            }
        }
    }

    public class ro_Nomina_Tipoliqui_List
    {
        string Variable = "ro_Nomina_Tipoliqui_Info";
        public List<ro_Nomina_Tipoliqui_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_Nomina_Tipoliqui_Info> list = new List<ro_Nomina_Tipoliqui_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_Nomina_Tipoliqui_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_Nomina_Tipoliqui_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}