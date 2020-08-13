using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class CentroCostoController : Controller
    {
        #region Variables
        ct_CentroCosto_Info Lista = new ct_CentroCosto_Info();
        ct_CentroCosto_Bus bus_centrocosto = new ct_CentroCosto_Bus();
        ct_CentroCostoNivel_Bus bus_centrocosto_nivel = new ct_CentroCostoNivel_Bus();
        ct_CentroCosto_List Lista_CentroCosto = new ct_CentroCosto_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion

        #region Index        
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCosto", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_CentroCosto_Info model = new ct_CentroCosto_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_centrocosto.get_list(model.IdEmpresa, true,true);
            Lista_CentroCosto.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_centrocosto(bool Nuevo=false)
        {
            //int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);
            //List<ct_CentroCosto_Info> model = bus_centrocosto.get_list(IdEmpresa, true, false);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CentroCosto.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_centrocosto", model);
        }
        private void cargar_combos(int IdEmpresa)
        {
            var lst_centrocosto_nivel = bus_centrocosto_nivel.get_list(IdEmpresa, false);
            ViewBag.lst_centrocosto_nivel = lst_centrocosto_nivel;
        }
        #endregion

        #region Metodos ComboBox bajo demanda

        public ActionResult CmbPadre_CentroCosto()
        {
            ct_CentroCosto_Info model = new ct_CentroCosto_Info();
            return PartialView("_CmbPadre_CentroCosto", model);
        }
        public List<ct_CentroCosto_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_centrocosto.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), true);
        }
        public ct_CentroCosto_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_centrocosto.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCosto", "Index");
            if(!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_CentroCosto_Info model = new ct_CentroCosto_Info
            {
                IdEmpresa = IdEmpresa
            };
            cargar_combos(model.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ct_CentroCosto_Info model)
        {
            if (bus_centrocosto.validar_existe_id(model.IdEmpresa, model.IdCentroCosto))
            {
                ViewBag.mensaje = "El código de la cuenta ya se encuentra registrado";
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_centrocosto.guardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdCentroCosto = model.IdCentroCosto, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, string IdCentroCosto = "", bool Exito = false)
        {
            ct_CentroCosto_Info model = bus_centrocosto.get_info(IdEmpresa, IdCentroCosto);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCosto", "Index");
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

            cargar_combos(model.IdEmpresa);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, string IdCentroCosto = "", bool Exito=false)
        {
            ct_CentroCosto_Info model = bus_centrocosto.get_info(IdEmpresa, IdCentroCosto);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCosto", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(model.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_CentroCosto_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            if (!bus_centrocosto.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdCentroCosto = model.IdCentroCosto, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0, string IdCentroCosto = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "CentroCosto", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            ct_CentroCosto_Info model = bus_centrocosto.get_info(IdEmpresa, IdCentroCosto);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(model.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_CentroCosto_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            if (!bus_centrocosto.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Json
        public JsonResult get_info_nuevo(int IdEmpresa = 0, string IdCentroCostoPadre = "")
        {
            var resultado = bus_centrocosto.get_info_nuevo(IdEmpresa, IdCentroCostoPadre);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }

    public class ct_CentroCosto_List
    {
        string Variable = "ct_CentroCosto_Info";
        public List<ct_CentroCosto_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_CentroCosto_Info> list = new List<ct_CentroCosto_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_CentroCosto_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_CentroCosto_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}