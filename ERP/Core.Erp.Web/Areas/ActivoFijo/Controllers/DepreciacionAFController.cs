using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Areas.Contabilidad.Controllers;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    [SessionTimeout]

    public class DepreciacionAFController : Controller
    {
        #region Variables

        Af_Depreciacion_Bus bus_depreciacion = new Af_Depreciacion_Bus();
        Af_Depreciacion_Det_Bus bus_depreciacion_det = new Af_Depreciacion_Det_Bus();
        Af_Depreciacion_Det_list lst_depreciacion_det = new Af_Depreciacion_Det_list();
        ct_cbtecble_det_List lst_comprobante_detalle = new ct_cbtecble_det_List();
        ct_cbtecble_det_Bus bus_comprobante_detalle = new ct_cbtecble_det_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        Af_Depreciacion_List Lista_Depreciacion = new Af_Depreciacion_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "DepreciacionAF", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };

            var lst = bus_depreciacion.get_list(model.IdEmpresa, true, model.fecha_ini, model.fecha_fin);
            Lista_Depreciacion.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "DepreciacionAF", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_depreciacion.get_list(model.IdEmpresa, true, model.fecha_ini, model.fecha_fin);
            Lista_Depreciacion.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_depreciacion(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Depreciacion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_depreciacion", model);
        }

        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
            var lst_periodo = bus_periodo.get_list(IdEmpresa, false);
            ViewBag.lst_periodo = lst_periodo;
        }

        private bool validar(Af_Depreciacion_Info i_validar, ref string msg)
        {
            i_validar.lst_detalle = lst_depreciacion_det.get_list(i_validar.IdTransaccionSession);
            if (i_validar.lst_detalle.Count == 0)
            {
                msg = "No existen activos a depreciarse";
                return false;
            }
            i_validar.lst_detalle_ct = lst_comprobante_detalle.get_list(i_validar.IdTransaccionSession);
            if (i_validar.lst_detalle_ct.Count == 0)
            {
                mensaje = "Debe ingresar registros en el detalle, por favor verifique";
                return false;
            }

            foreach (var item in i_validar.lst_detalle_ct)
            {
                if (string.IsNullOrEmpty(item.IdCtaCble))
                {
                    mensaje = "Faltan cuentas contables, por favor verifique";
                    return false;
                }
            }
            if (i_validar.lst_detalle_ct.Sum(q => q.dc_Valor) != 0)
            {
                mensaje = "La suma de los detalles debe ser 0, por favor verifique";
                return false;
            }
            if (i_validar.lst_detalle_ct.Where(q => q.dc_Valor == 0).Count() > 0)
            {
                mensaje = "Existen detalles con valor 0 en el debe o haber, por favor verifique";
                return false;
            }
            return true;
        }

        public void Get_list_activos_a_depreciar(int IdPeriodo = 0, decimal IdTransaccionSession = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            string IdUsuario = SessionFixed.IdUsuario;
            var lst = bus_depreciacion_det.get_list_a_depreciar(IdEmpresa, IdPeriodo, IdUsuario);
            lst_depreciacion_det.set_list(lst, IdTransaccionSession);

            List<ct_cbtecble_det_Info> lst_ct = new List<ct_cbtecble_det_Info>();
            int secuencia = 1;
            foreach (var item in lst)
            {
                item.Valor_Depreciacion = Math.Round(item.Valor_Depreciacion, 2, MidpointRounding.AwayFromZero);
                lst_ct.Add(new ct_cbtecble_det_Info
                {
                    //Debe
                    secuencia = secuencia++,
                    IdCtaCble = item.IdCtaCble_Gastos_Depre,
                    dc_Valor = Math.Round(item.Valor_Depreciacion, 2, MidpointRounding.AwayFromZero),
                    dc_Valor_debe = Math.Round(item.Valor_Depreciacion, 2, MidpointRounding.AwayFromZero)
                });
                lst_ct.Add(new ct_cbtecble_det_Info
                {
                    //Haber
                    secuencia = secuencia++,
                    IdCtaCble = item.IdCtaCble_Dep_Acum,
                    dc_Valor = Math.Round(item.Valor_Depreciacion, 2, MidpointRounding.AwayFromZero) * -1,
                    dc_Valor_haber = Math.Round(item.Valor_Depreciacion, 2, MidpointRounding.AwayFromZero)
                });
            }
            lst_comprobante_detalle.set_list(lst_ct, IdTransaccionSession);
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "DepreciacionAF", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            Af_Depreciacion_Info model = new Af_Depreciacion_Info
            {
                IdEmpresa = IdEmpresa,
                Fecha_Depreciacion = DateTime.Now.Date,
                IdPeriodo = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMM")),
                lst_detalle = new List<Af_Depreciacion_Det_Info>(),
                lst_detalle_ct = new List<ct_cbtecble_det_Info>(),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            lst_depreciacion_det.set_list(model.lst_detalle, model.IdTransaccionSession);
            lst_comprobante_detalle.set_list(model.lst_detalle_ct,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(Af_Depreciacion_Info model)
        {
            model.lst_detalle = lst_depreciacion_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_depreciacion.guardarDB(model))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdDepreciacion = model.IdDepreciacion, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdDepreciacion = 0, bool Exito=false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            Af_Depreciacion_Info model = bus_depreciacion.get_info(IdEmpresa, IdDepreciacion);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "DepreciacionAF", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            model.lst_detalle = bus_depreciacion_det.get_list(IdEmpresa, IdDepreciacion);
            model.lst_detalle_ct = bus_comprobante_detalle.get_list(IdEmpresa, Convert.ToInt32(model.IdTipoCbte), Convert.ToInt32(model.IdCbteCble));
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            lst_depreciacion_det.set_list(model.lst_detalle, model.IdTransaccionSession);
            lst_comprobante_detalle.set_list(model.lst_detalle_ct, model.IdTransaccionSession);
            cargar_combos(IdEmpresa);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa =0, decimal IdDepreciacion = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "DepreciacionAF", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            Af_Depreciacion_Info model = bus_depreciacion.get_info(IdEmpresa, IdDepreciacion);
            if (model == null)
                return RedirectToAction("Index");
            model.lst_detalle = bus_depreciacion_det.get_list(IdEmpresa, IdDepreciacion);
            model.lst_detalle_ct = bus_comprobante_detalle.get_list(IdEmpresa, Convert.ToInt32(model.IdTipoCbte), Convert.ToInt32(model.IdCbteCble));
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            lst_depreciacion_det.set_list(model.lst_detalle, model.IdTransaccionSession);
            lst_comprobante_detalle.set_list(model.lst_detalle_ct,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(Af_Depreciacion_Info model)
        {
            model.lst_detalle = lst_depreciacion_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_depreciacion.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = "No se ha podido modificar el registro";
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdDepreciacion = model.IdDepreciacion, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa =0 , decimal IdDepreciacion = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "DepreciacionAF", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            Af_Depreciacion_Info model = bus_depreciacion.get_info(IdEmpresa, IdDepreciacion);
            if (model == null)
                return RedirectToAction("Index");
            model.lst_detalle = bus_depreciacion_det.get_list(IdEmpresa, IdDepreciacion);
            model.lst_detalle_ct = bus_comprobante_detalle.get_list(IdEmpresa, Convert.ToInt32(model.IdTipoCbte), Convert.ToInt32(model.IdCbteCble));
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            lst_depreciacion_det.set_list(model.lst_detalle, model.IdTransaccionSession);
            lst_comprobante_detalle.set_list(model.lst_detalle_ct,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(Af_Depreciacion_Info model)
        {
            model.lst_detalle = lst_depreciacion_det.get_list(model.IdTransaccionSession);
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_depreciacion.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = "No se ha podido anular el registro";
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Detalle


        [ValidateInput(false)]
        public ActionResult GridViewPartial_depreciacion_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            Af_Depreciacion_Info model = new Af_Depreciacion_Info();
            model.lst_detalle = lst_depreciacion_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_depreciacion_det", model);
        }
        #endregion
    }

    public class Af_Depreciacion_List
    {
        string Variable = "Af_Depreciacion_Info";

        public List<Af_Depreciacion_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Depreciacion_Info> list = new List<Af_Depreciacion_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Depreciacion_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Depreciacion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    public class Af_Depreciacion_Det_list
    {
        string Variable = "Af_Depreciacion_Det_Info";

        public List<Af_Depreciacion_Det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Depreciacion_Det_Info> list = new List<Af_Depreciacion_Det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Depreciacion_Det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Depreciacion_Det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}