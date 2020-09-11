using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Info.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Web.Helps;
using Core.Erp.Info.Contabilidad;
using DevExpress.Web;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class TipoNotaController : Controller
    {
        #region Variables
        fa_TipoNota_Bus bus_tiponota = new fa_TipoNota_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_TipoNota_List Lista_TipoNota = new fa_TipoNota_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "TipoNota", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_TipoNota_Info model = new fa_TipoNota_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_tiponota.get_list(model.IdEmpresa, true);
            Lista_TipoNota.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tiponota(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_tiponota.get_list(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TipoNota.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_tiponota", model);
        }
        private void cargar_combos()
        {
            Dictionary<string, string> lst_tipos = new Dictionary<string, string>();
            lst_tipos.Add("C", "Credito");
            lst_tipos.Add("D", "Debito");
            ViewBag.lst_tipos = lst_tipos;
        }

        #endregion

        #region Metodos ComboBox bajo demanda
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        public ActionResult CmbCuenta_TipoNota()
        {
            fa_TipoNota_Info model = new fa_TipoNota_Info();
            return PartialView("_CmbCuenta_TipoNota", model);
        }
     
        public List<ct_plancta_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "TipoNota", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_TipoNota_Info model = new fa_TipoNota_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_TipoNota_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario.ToString();
            if (!bus_tiponota.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoNota = model.IdTipoNota, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdTipoNota = 0, bool Exito=false)
        {
            fa_TipoNota_Info model = bus_tiponota.get_info(IdEmpresa, IdTipoNota);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "TipoNota", "Index");
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

            cargar_combos();
            return View(model);
        }

        public ActionResult Modificar( int IdEmpresa = 0, int  IdTipoNota = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "TipoNota", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_TipoNota_Info model = bus_tiponota.get_info(IdEmpresa, IdTipoNota);            
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_TipoNota_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario.ToString();
            if (!bus_tiponota.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoNota = model.IdTipoNota, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0, int IdTipoNota = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "TipoNota", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_TipoNota_Info model = bus_tiponota.get_info(IdEmpresa, IdTipoNota);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_TipoNota_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario.ToString();
            if (!bus_tiponota.anularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
        
    }

    public class fa_TipoNota_List
    {
        string Variable = "fa_TipoNota_Info";
        public List<fa_TipoNota_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_TipoNota_Info> list = new List<fa_TipoNota_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_TipoNota_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_TipoNota_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}