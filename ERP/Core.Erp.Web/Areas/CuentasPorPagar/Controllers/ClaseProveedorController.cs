using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.CuentasPorPagar;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.CuentasPorPagar;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorPagar.Controllers
{
    [SessionTimeout]
    public class ClaseProveedorController : Controller
    {
        #region Variable
        cp_proveedor_clase_Bus bus_clase_proveedor = new cp_proveedor_clase_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        string mensaje = string.Empty;
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        cp_proveedor_clase_List Lista_ProveedorTipo = new cp_proveedor_clase_List();
        #endregion

        #region Metodos ComboBox bajo demanda CtaCbleCXP
        public ActionResult CmbCtaCbleCXP_Proveedor()
        {
            int model = new int();
            return PartialView("_CmbCtaCbleCXP_Proveedor", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda_ctacble_cxp(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return bus_plancta.get_list_bajo_demanda(args, IdEmpresa, false);
        }
        public ct_plancta_Info get_info_bajo_demanda_ctacble_cxp(ListEditItemRequestedByValueEventArgs args)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return bus_plancta.get_info_bajo_demanda(args, IdEmpresa);
        }
        #endregion

        #region Metodos ComboBox bajo demanda CtaCbleGasto
        public ActionResult CmbCtaCbleGasto_Proveedor()
        {
            int model = new int();
            return PartialView("_CmbCtaCbleGasto_Proveedor", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda_ctacble_gasto(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return bus_plancta.get_list_bajo_demanda(args, IdEmpresa, false);
        }
        public ct_plancta_Info get_info_bajo_demanda_ctacble_gasto(ListEditItemRequestedByValueEventArgs args)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return bus_plancta.get_info_bajo_demanda(args, IdEmpresa);
        }
        #endregion

        #region Metodos ComboBox bajo demanda CtaCbleGasto
        public ActionResult CmbCtaCbleAnticipo_Proveedor()
        {
            int model = new int();
            return PartialView("_CmbCtaCbleAnticipo_Proveedor", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda_ctacble_Anticipo(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return bus_plancta.get_list_bajo_demanda(args, IdEmpresa, false);
        }
        public ct_plancta_Info get_info_bajo_demanda_ctacble_Anticipo(ListEditItemRequestedByValueEventArgs args)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return bus_plancta.get_info_bajo_demanda(args, IdEmpresa);
        }
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "ClaseProveedor", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cp_proveedor_clase_Info model = new cp_proveedor_clase_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_clase_proveedor.get_list(model.IdEmpresa, true);
            Lista_ProveedorTipo.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_clase_proveedor(bool Nuevo=true)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_clase_proveedor.get_list(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_ProveedorTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_clase_proveedor", model);
        }
        private void cargar_combos()
        {
            var lst_ctacble = bus_plancta.get_list(Convert.ToInt32(Session["IdEmpresa"]), false, true);
            ViewBag.lst_cuentas = lst_ctacble;
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0, int IdClaseProveedor = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "ClaseProveedor", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cp_proveedor_clase_Info model = new cp_proveedor_clase_Info
            {
                IdEmpresa = IdEmpresa
            };
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cp_proveedor_clase_Info model)
        {
            if (!bus_clase_proveedor.guardarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdClaseProveedor = model.IdClaseProveedor, Exito = true });
        }

        public ActionResult Consultar(int IdClaseProveedor = 0, bool Exito=false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cp_proveedor_clase_Info model = bus_clase_proveedor.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdClaseProveedor);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "ClaseProveedor", "Index");
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

        public ActionResult Modificar(int IdClaseProveedor = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "ClaseProveedor", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            cp_proveedor_clase_Info model = bus_clase_proveedor.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdClaseProveedor);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cp_proveedor_clase_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (!bus_clase_proveedor.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdClaseProveedor = model.IdClaseProveedor, Exito = true });
        }

        public ActionResult Anular(int IdClaseProveedor = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "ClaseProveedor", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            cp_proveedor_clase_Info model = bus_clase_proveedor.get_info(Convert.ToInt32(Session["IdEmpresa"]), IdClaseProveedor);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cp_proveedor_clase_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (!bus_clase_proveedor.anularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class cp_proveedor_clase_List
    {
        string Variable = "cp_proveedor_clase_Info";
        public List<cp_proveedor_clase_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cp_proveedor_clase_Info> list = new List<cp_proveedor_clase_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_proveedor_clase_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_proveedor_clase_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}