using Core.Erp.Bus.Banco;
using Core.Erp.Bus.General;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Banco;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Banco.Controllers
{
    public class TipoFlujoMovimientoController : Controller
    {
        #region Variables
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        ba_TipoFlujo_Bus bus_TipoFlujo = new ba_TipoFlujo_Bus();
        ba_Banco_Cuenta_Bus bus_banco_cuenta = new ba_Banco_Cuenta_Bus();
        ba_TipoFlujo_Movimiento_Bus bus_TipoFlujo_Movimiento = new ba_TipoFlujo_Movimiento_Bus();
        ba_TipoFlujo_Movimiento_List Lista_TipoFlujoMov = new ba_TipoFlujo_Movimiento_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        #endregion

        #region Combo bajo demanda CmbTipoFlujo

        public ActionResult CmbTipoFlujo()
        {
            ba_TipoFlujo_Movimiento_Info model = new ba_TipoFlujo_Movimiento_Info();
            return PartialView("_CmbTipoFlujo", model);
        }
        public List<ba_TipoFlujo_Info> get_list_bajo_demanda_tipoflujo(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var TipoFlujo_GetList = bus_TipoFlujo.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
            return TipoFlujo_GetList;
        }
        public ba_TipoFlujo_Info get_info_bajo_demanda_tipoflujo(ListEditItemRequestedByValueEventArgs args)
        {
            var TipoFlujo_GetInfo = bus_TipoFlujo.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
            return TipoFlujo_GetInfo;
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoFlujoMovimiento", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            var model = new ba_TipoFlujo_Movimiento_Info()
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            var lst = bus_TipoFlujo_Movimiento.GetList(model.IdEmpresa, true);
            Lista_TipoFlujoMov.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_TipoFlujoMovimiento(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //List<ba_TipoFlujo_Movimiento_Info> model = bus_TipoFlujo_Movimiento.GetList(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TipoFlujoMov.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_TipoFlujoMovimiento", model);
        }
        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa, int IdSucursal)
        {
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, Convert.ToString(SessionFixed.IdUsuario), false);
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_banco_cuenta = bus_banco_cuenta.get_list(IdEmpresa, IdSucursal, false);
            ViewBag.lst_banco_cuenta = lst_banco_cuenta;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoFlujoMovimiento", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ba_TipoFlujo_Movimiento_Info model = new ba_TipoFlujo_Movimiento_Info
            {
                IdEmpresa = IdEmpresa,
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdUsuarioCreacion = SessionFixed.IdUsuario
            };

            cargar_combos(IdEmpresa, model.IdSucursal);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ba_TipoFlujo_Movimiento_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;

            if (!bus_TipoFlujo_Movimiento.GuardarBD(model))
            {
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMovimiento = model.IdMovimiento, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdMovimiento = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ba_TipoFlujo_Movimiento_Info model = bus_TipoFlujo_Movimiento.GetInfo(IdEmpresa, IdMovimiento);

            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoFlujoMovimiento", "Index");
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

            cargar_combos(IdEmpresa, model.IdSucursal);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal IdMovimiento = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ba_TipoFlujo_Movimiento_Info model = bus_TipoFlujo_Movimiento.GetInfo(IdEmpresa, IdMovimiento);

            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoFlujoMovimiento", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa, model.IdSucursal);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ba_TipoFlujo_Movimiento_Info model)
        {
            model.IdUsuarioModificacion = Session["IdUsuario"].ToString();

            if (!bus_TipoFlujo_Movimiento.ModificarBD(model))
            {
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMovimiento = model.IdMovimiento, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdMovimiento = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoFlujoMovimiento", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ba_TipoFlujo_Movimiento_Info model = bus_TipoFlujo_Movimiento.GetInfo(IdEmpresa, IdMovimiento);
            if (model == null)
                return RedirectToAction("Index");

            cargar_combos(IdEmpresa, model.IdSucursal);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ba_TipoFlujo_Movimiento_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;

            if (!bus_TipoFlujo_Movimiento.AnularBD(model))
            {
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class ba_TipoFlujo_Movimiento_List
    {
        string Variable = "ba_TipoFlujo_Movimiento_Info";
        public List<ba_TipoFlujo_Movimiento_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_TipoFlujo_Movimiento_Info> list = new List<ba_TipoFlujo_Movimiento_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_TipoFlujo_Movimiento_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_TipoFlujo_Movimiento_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}