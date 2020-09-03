using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.CuentasPorPagar;
using Core.Erp.Info.CuentasPorPagar;
using Core.Erp.Web.Helps;
using Core.Erp.Info.Helps;
using Core.Erp.Bus.General;
using Core.Erp.Info.General;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Bus.SeguridadAcceso;
using DevExpress.Web;
using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.Contabilidad;

namespace Core.Erp.Web.Areas.CuentasPorPagar.Controllers
{
    public class SolicitudPagoController : Controller
    {
        #region Variables
        cp_SolicitudPago_Bus bus_solicitud = new cp_SolicitudPago_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
        cp_SolicitudPagoDet_Bus bus_pago_Det = new cp_SolicitudPagoDet_Bus();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        string mensaje = string.Empty;
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        cp_SolicitudPago_List Lista_SolicitudPago = new cp_SolicitudPago_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "SolicitudPago", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };
            cargar_combos_consulta();
            var lst = bus_solicitud.GetList(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin, true);
            Lista_SolicitudPago.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "SolicitudPago", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();

            cargar_combos_consulta();
            var lst = bus_solicitud.GetList(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin, true);
            Lista_SolicitudPago.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        private void cargar_combos_consulta()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, Convert.ToString(SessionFixed.IdUsuario), false);
            lst_sucursal.Add(new tb_sucursal_Info
            {
                IdEmpresa = IdEmpresa,
                IdSucursal = 0,
                Su_Descripcion = "TODAS"
            });
            ViewBag.lst_sucursal = lst_sucursal;
        }
        private void cargar_combos(int IdEmpresa)
        {
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, Convert.ToString(SessionFixed.IdUsuario), false);
            ViewBag.lst_sucursal = lst_sucursal;
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_solicitud_pago(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //ViewBag.IdSucursal = IdSucursal;
            //ViewBag.IdEmpresa = IdEmpresa;
            //ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            //ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);
            //var model = bus_solicitud.GetList(IdEmpresa, IdSucursal, ViewBag.Fecha_ini, ViewBag.Fecha_fin, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_SolicitudPago.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_solicitud_pago", model);
        }
        #endregion
        #region Metodos ComboBox bajo demanda
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbProveedor_CXP()
        {
            decimal model = new decimal();
            return PartialView("_CmbProveedor_CXP", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.PROVEE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.PROVEE.ToString());
        }
        #endregion
        #region Acciones
        public ActionResult Nuevo()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "SolicitudPago", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cp_SolicitudPago_Info model = new cp_SolicitudPago_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                Fecha = DateTime.Now,
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal)
            };
            seg_usuario_Info mod = bus_usuario.get_info(SessionFixed.IdUsuario);
            model.Solicitante = mod.Nombre;
            cargar_combos(model.IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cp_SolicitudPago_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_solicitud.GuardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSolicitud = model.IdSolicitud, Exito = true });

        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdSolicitud = 0, bool Exito = false)
        {
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            cp_SolicitudPago_Info model = bus_solicitud.GetInfo(IdEmpresa, IdSolicitud);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "SolicitudPago", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            cargar_combos(IdEmpresa);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.CXP, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , decimal IdSolicitud = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "SolicitudPago", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            cp_SolicitudPago_Info model = bus_solicitud.GetInfo(IdEmpresa, IdSolicitud);
            if (model == null)
                return RedirectToAction("Index");

            cargar_combos(IdEmpresa);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.CXP, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cp_SolicitudPago_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            if (!bus_solicitud.ModificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSolicitud = model.IdSolicitud, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdSolicitud = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "SolicitudPago", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            cp_SolicitudPago_Info model = bus_solicitud.GetInfo(IdEmpresa, IdSolicitud);
            if (model == null)
                return RedirectToAction("Index");

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.CXP, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]

        public ActionResult Anular(cp_SolicitudPago_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            if (!bus_solicitud.AnularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class cp_SolicitudPago_List
    {
        string Variable = "cp_SolicitudPago_Info";
        public List<cp_SolicitudPago_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cp_SolicitudPago_Info> list = new List<cp_SolicitudPago_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_SolicitudPago_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_SolicitudPago_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}