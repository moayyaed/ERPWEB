using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class CentroCostoNivelController : Controller
    {
        #region Index
        ct_CentroCostoNivel_Bus bus_cc_nivel = new ct_CentroCostoNivel_Bus();
        ct_CentroCostoNivel_List Lista_CentroCostoNivel = new ct_CentroCostoNivel_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCostoNivel", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_CentroCostoNivel_Info model = new ct_CentroCostoNivel_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_cc_nivel.get_list(model.IdEmpresa, true);
            Lista_CentroCostoNivel.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_cencost_nivel(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            //List<ct_CentroCostoNivel_Info> model = bus_cc_nivel.get_list(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CentroCostoNivel.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_cencost_nivel", model);
        }

        #endregion
        #region Acciones
        public ActionResult Nuevo(int IdEmpresa=0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCostoNivel", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_CentroCostoNivel_Info model = new ct_CentroCostoNivel_Info()
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ct_CentroCostoNivel_Info model)
        {
            if (bus_cc_nivel.validar_existe_nivel(model.IdEmpresa, model.IdNivel))
            {
                ViewBag.mensaje = "El nivel ya se encuentra registrado";
                return View(model);
            }
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_cc_nivel.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdNivel = model.IdNivel, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdNivel = 0, bool Exito = false)
        {
            ct_CentroCostoNivel_Info model = bus_cc_nivel.get_info(IdEmpresa, IdNivel);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCostoNivel", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdNivel = 0, bool Exito=false)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCostoNivel", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ct_CentroCostoNivel_Info model = bus_cc_nivel.get_info(IdEmpresa, IdNivel);
            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_CentroCostoNivel_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            if (!bus_cc_nivel.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdNivel = model.IdNivel, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdNivel = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCostoNivel", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_CentroCostoNivel_Info model = bus_cc_nivel.get_info(IdEmpresa, IdNivel);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_CentroCostoNivel_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            if (!bus_cc_nivel.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class ct_CentroCostoNivel_List
    {
        string Variable = "ct_CentroCostoNivel_Info";
        public List<ct_CentroCostoNivel_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_CentroCostoNivel_Info> list = new List<ct_CentroCostoNivel_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_CentroCostoNivel_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_CentroCostoNivel_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}