using Core.Erp.Bus.Caja;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Caja;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Caja.Controllers
{
    [SessionTimeout]
    public class TipoMovimientoCajaController : Controller
    {
        #region variables
        caj_Caja_Movimiento_Tipo_Bus bus_tipomovimiento = new caj_Caja_Movimiento_Tipo_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        caj_Caja_Movimiento_Tipo_List Lista_TipoMov = new caj_Caja_Movimiento_Tipo_List();
        #endregion

        #region Metodos ComboBox bajo demanda

        public ActionResult CmbCuenta_Tipo_movimiento()
        {
            caj_Caja_Movimiento_Tipo_Info model = new caj_Caja_Movimiento_Tipo_Info();
            return PartialView("_CmbCuenta_Tipo_movimiento", model);
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "TipoMovimientoCaja", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion
            caj_Caja_Movimiento_Tipo_Info model = new caj_Caja_Movimiento_Tipo_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_tipomovimiento.get_list(model.IdEmpresa, true);
            Lista_TipoMov.set_list(lst, model.IdTransaccionSession);
            return View(model);     
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipomovimientocaja(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TipoMov.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_tipomovimientocaja", model);
        }


        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            var lst_cuentas = bus_plancta.get_list(IdEmpresa, false, false);
            ViewBag.lst_cuentas = lst_cuentas;

            Dictionary<string, string> lst_signo = new Dictionary<string, string>();
            lst_signo.Add("+", "+");
            lst_signo.Add("-", "-");
            ViewBag.lst_signo = lst_signo;
        }


        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "TipoMovimientoCaja", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            caj_Caja_Movimiento_Tipo_Info model = new caj_Caja_Movimiento_Tipo_Info
            {
                IdEmpresa = IdEmpresa
            };
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(caj_Caja_Movimiento_Tipo_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_tipomovimiento.guardarDB(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoMovi = model.IdTipoMovi, Exito = true });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdTipoMovi = 0, bool Exito=false)
        {
            caj_Caja_Movimiento_Tipo_Info model = bus_tipomovimiento.get_info(IdEmpresa, IdTipoMovi);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "TipoMovimientoCaja", "Index");
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

            cargar_combos(IdEmpresa);
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0 , int IdTipoMovi = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "TipoMovimientoCaja", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            caj_Caja_Movimiento_Tipo_Info model = bus_tipomovimiento.get_info(IdEmpresa, IdTipoMovi);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(caj_Caja_Movimiento_Tipo_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_tipomovimiento.modificarDB(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoMovi = model.IdTipoMovi, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdTipoMovi = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Caja", "TipoMovimientoCaja", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            caj_Caja_Movimiento_Tipo_Info model = bus_tipomovimiento.get_info(IdEmpresa, IdTipoMovi);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(caj_Caja_Movimiento_Tipo_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_tipomovimiento.anularDB(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class caj_Caja_Movimiento_Tipo_List
    {
        string Variable = "caj_Caja_Movimiento_Tipo_Info";
        public List<caj_Caja_Movimiento_Tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<caj_Caja_Movimiento_Tipo_Info> list = new List<caj_Caja_Movimiento_Tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<caj_Caja_Movimiento_Tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<caj_Caja_Movimiento_Tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}