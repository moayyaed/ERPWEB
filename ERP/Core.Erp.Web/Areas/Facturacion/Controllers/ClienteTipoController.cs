using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class ClienteTipoController : Controller
    {
        #region Variables
        fa_cliente_tipo_Bus bus_clientetipo = new fa_cliente_tipo_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_cliente_tipo_List Lista_ClienteTipo = new fa_cliente_tipo_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_cliente_tipo_Info model = new fa_cliente_tipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_clientetipo.get_list(model.IdEmpresa, true);
            Lista_ClienteTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_clientetipo(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //List<fa_cliente_tipo_Info> model = bus_clientetipo.get_list(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_ClienteTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_clientetipo", model);
        }

        #endregion
        #region Combo Cuenta bajo demanda
        public ActionResult CmbCuenta_Anticipo()
        {
            fa_cliente_tipo_Info model = new fa_cliente_tipo_Info();
            return PartialView("_CmbCuenta_Anticipo", model);
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
        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
            var lst_ctacble = bus_plancta.get_list(IdEmpresa, false, true);
            ViewBag.lst_cuentas = lst_ctacble;
        }

        #endregion
        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_cliente_tipo_Info model = new fa_cliente_tipo_Info
            {
                IdEmpresa = IdEmpresa
            };
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_cliente_tipo_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_clientetipo.guardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, Idtipo_cliente = model.Idtipo_cliente, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int Idtipo_cliente = 0, bool Exito=false)
        {
            fa_cliente_tipo_Info model = bus_clientetipo.get_info(IdEmpresa, Idtipo_cliente);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (model.Estado == "I")
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

            cargar_combos(IdEmpresa);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , int Idtipo_cliente = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_cliente_tipo_Info model = bus_clientetipo.get_info(IdEmpresa, Idtipo_cliente);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_cliente_tipo_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_clientetipo.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, Idtipo_cliente = model.Idtipo_cliente, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0 , int Idtipo_cliente = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_cliente_tipo_Info model = bus_clientetipo.get_info(IdEmpresa, Idtipo_cliente);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_cliente_tipo_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_clientetipo.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class fa_cliente_tipo_List
    {
        string Variable = "fa_cliente_tipo_Info";
        public List<fa_cliente_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_cliente_tipo_Info> list = new List<fa_cliente_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_cliente_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_cliente_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}