using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    [SessionTimeout]
    public class PlanDeCuentasNivelController : Controller
    {
        #region Index
        ct_plancta_nivel_Bus bus_plancta_nivel = new ct_plancta_nivel_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ct_plancta_nivel_List Lista_PlanCuentaNivel = new ct_plancta_nivel_List();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlanDeCuentasNivel", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_plancta_nivel_Info model = new ct_plancta_nivel_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_plancta_nivel.get_list(model.IdEmpresa, true);
            Lista_PlanCuentaNivel.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_plancta_nivel(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            //List<ct_plancta_nivel_Info> model = bus_plancta_nivel.get_list(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_PlanCuentaNivel.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_plancta_nivel", model);
        }

        #endregion
        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlanDeCuentasNivel", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_plancta_nivel_Info model = new ct_plancta_nivel_Info()
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ct_plancta_nivel_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            if (bus_plancta_nivel.validar_existe_nivel(model.IdEmpresa, model.IdNivelCta))
            {
                ViewBag.mensaje = "El nivel ya se encuentra registrado";
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_plancta_nivel.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdNivelCta = model.IdNivelCta, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdNivelCta = 0, bool Exito=false)
        {
            ct_plancta_nivel_Info model = bus_plancta_nivel.get_info(IdEmpresa, IdNivelCta);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlanDeCuentasNivel", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa=0, int IdNivelCta = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlanDeCuentasNivel", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ct_plancta_nivel_Info model = bus_plancta_nivel.get_info(IdEmpresa,IdNivelCta);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_plancta_nivel_Info model)
        {
            model.IdUsuarioUltModi = SessionFixed.IdUsuario;
            if (!bus_plancta_nivel.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdNivelCta = model.IdNivelCta, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa=0, int IdNivelCta = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlanDeCuentasNivel", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_plancta_nivel_Info model = bus_plancta_nivel.get_info(IdEmpresa, IdNivelCta);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_plancta_nivel_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_plancta_nivel.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class ct_plancta_nivel_List
    {
        string Variable = "ct_plancta_nivel_Info";
        public List<ct_plancta_nivel_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_CentroCostoNivel_Info> list = new List<ct_CentroCostoNivel_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_plancta_nivel_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_plancta_nivel_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}