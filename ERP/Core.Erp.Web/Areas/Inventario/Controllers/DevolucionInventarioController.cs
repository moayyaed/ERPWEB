using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class DevolucionInventarioController : Controller
    {
        #region Variables
        in_devolucion_inven_Bus bus_devolucion = new in_devolucion_inven_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        in_devolucion_inven_det_List List_det = new in_devolucion_inven_det_List();
        in_Ing_Egr_Inven_List Lista_IngEgr_Inven = new in_Ing_Egr_Inven_List();
        in_devolucion_inven_List Lista_Devolucion = new in_devolucion_inven_List();
        in_Ing_Egr_Inven_Bus bus_inv = new in_Ing_Egr_Inven_Bus();
        in_devolucion_inven_det_Bus bus_det = new in_devolucion_inven_det_Bus();
        string mensaje = string.Empty;
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "DevolucionInventario", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };
            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_devolucion.get_list(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin);
            Lista_Devolucion.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "DevolucionInventario", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion
            CargarCombosConsulta(model.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_devolucion.get_list(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin);
            Lista_Devolucion.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        public ActionResult GridViewPartial_devolucion( bool Nuevo=false )
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Devolucion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            //var model = bus_devolucion.get_list(IdEmpresa, IdSucursal, ViewBag.Fecha_ini, ViewBag.Fecha_fin);
            return PartialView("_GridViewPartial_devolucion",model);
        }
        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            Dictionary<string, string> lst_signo = new Dictionary<string, string>();
            lst_signo.Add("+", "Ingreso por devolución");
            lst_signo.Add("-", "Egreso por devolución");            
            ViewBag.lst_signo = lst_signo;

            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, false);
            ViewBag.lst_sucursal = lst_sucursal;
        }
        private void CargarCombosConsulta(int IdEmpresa)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        private bool validar(in_devolucion_inven_Info i_validar, ref string msg)
        {
            i_validar.lst_det = List_det.get_list(i_validar.IdTransaccionSession).Where(q=>q.cant_devuelta > 0).ToList();
            if (i_validar.lst_det.Count == 0)
            {
                msg = "No ha ingresado detalles a la devolución";
                return false;
            }
            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.Fecha, cl_enumeradores.eModulo.INV, i_validar.dev_IdSucursal, ref msg))
            {
                return false;
            }
            return true;
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "DevolucionInventario", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            in_devolucion_inven_Info model = new in_devolucion_inven_Info
            {
                IdEmpresa = IdEmpresa,
                Fecha_ini = DateTime.Now.Date.AddMonths(-1),
                Fecha_fin = DateTime.Now.Date,
                Fecha = DateTime.Now.Date,
                lst_det = new List<in_devolucion_inven_det_Info>(),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };
            List_det.set_list(new List<in_devolucion_inven_det_Info>(), model.IdTransaccionSession);
            Lista_IngEgr_Inven.set_list(new List<in_Ing_Egr_Inven_Info>(), model.IdTransaccionSession);
            //set_list(new List<in_Ing_Egr_Inven_Info>());
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(in_devolucion_inven_Info model)
        {
            model.lst_det = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            if (!validar(model,ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            model.IdUsuario = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_devolucion.guardarDB(model))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdDev_Inven = model.IdDev_Inven, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdDev_Inven = 0, bool Exito=false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            in_devolucion_inven_Info model = bus_devolucion.get_info(IdEmpresa, IdDev_Inven);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "DevolucionInventario", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_det = bus_det.get_list(IdEmpresa, IdDev_Inven);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.FAC, 0, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , decimal IdDev_Inven = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "DevolucionInventario", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            in_devolucion_inven_Info model = bus_devolucion.get_info(IdEmpresa, IdDev_Inven);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_det = bus_det.get_list(IdEmpresa, IdDev_Inven);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.FAC, 0, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(in_devolucion_inven_Info model)
        {
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            model.IdUsuarioUltModi = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_devolucion.modificarDB(model))
            {
                ViewBag.mensaje = "No se ha podido modificar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdDev_Inven = model.IdDev_Inven, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0 , decimal IdDev_Inven = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "DevolucionInventario", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            in_devolucion_inven_Info model = bus_devolucion.get_info(IdEmpresa, IdDev_Inven);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_det = bus_det.get_list(IdEmpresa, IdDev_Inven);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.FAC, 0, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(in_devolucion_inven_Info model)
        {
            model.IdusuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_devolucion.anularDB(model))
            {
                ViewBag.mensaje = "No se ha podido modificar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Json
        public JsonResult GetMovimientos(DateTime? Fecha_ini, DateTime? Fecha_fin, int IdSucursal = 0, string signo = "")
        {
            bool resultado = false;
            DateTime Fechaini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            DateTime Fechafin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);
            var lst = bus_inv.get_list_por_devolver(Convert.ToInt32(SessionFixed.IdEmpresa), signo == "+" ? "-" : "+", Fechaini, Fechafin);
            //set_list(lst);
            Lista_IngEgr_Inven.set_list(lst, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return Json(resultado,JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetMovimiento(string SecuencialID)
        {
            bool resultado = false;
            //var lst = get_list();
            var lst = Lista_IngEgr_Inven.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var mov = lst.Where(q => q.SecuencialID == SecuencialID).FirstOrDefault();
            if (mov != null)
            {
                var lista = bus_det.get_list_x_movimiento(mov.IdEmpresa, mov.IdSucursal, mov.IdMovi_inven_tipo, mov.IdNumMovi);
                List_det.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Detalle
        public ActionResult GridViewPartial_devolucion_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_devolucion_det",model);
        }
        public ActionResult GridViewPartial_devolucion_det_x_cruzar()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_IngEgr_Inven.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_devolucion_det_x_cruzar", model);
        }
        //public List<in_Ing_Egr_Inven_Info> get_list()
        //{
        //    if (Session["in_Ing_Egr_Inven_x_devolver_Info"] == null)
        //    {
        //        List<in_Ing_Egr_Inven_Info> list = new List<in_Ing_Egr_Inven_Info>();

        //        Session["in_Ing_Egr_Inven_x_devolver_Info"] = list;
        //    }
        //    return (List<in_Ing_Egr_Inven_Info>)Session["in_Ing_Egr_Inven_x_devolver_Info"];
        //}
        //public void set_list(List<in_Ing_Egr_Inven_Info> list)
        //{
        //    Session["in_Ing_Egr_Inven_x_devolver_Info"] = list;
        //}
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] in_devolucion_inven_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            if (info_det != null)
                List_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_devolucion_det", model);
        }
        public ActionResult EditingDelete(int secuencia)
        {
            List_det.DeleteRow(secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_devolucion_det", model);
        }
        #endregion
    }

    public class in_devolucion_inven_List
    {
        string Variable = "in_devolucion_inven_Info";
        public List<in_devolucion_inven_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_devolucion_inven_Info> list = new List<in_devolucion_inven_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_devolucion_inven_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_devolucion_inven_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class in_Ing_Egr_Inven_List
    {
        string Variable = "in_Ing_Egr_Inven_Info";
        public List<in_Ing_Egr_Inven_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_Info> list = new List<in_Ing_Egr_Inven_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_Ing_Egr_Inven_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    public class in_devolucion_inven_det_List
    {
        string Variable = "in_devolucion_inven_det_Info";
        public List<in_devolucion_inven_det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_devolucion_inven_det_Info> list = new List<in_devolucion_inven_det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_devolucion_inven_det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_devolucion_inven_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(in_devolucion_inven_det_Info info_det, decimal IdTransaccionSession)
        {
            List<in_devolucion_inven_det_Info> list = get_list(IdTransaccionSession);
            info_det.secuencia = list.Count == 0 ? 1 : list.Max(q => q.secuencia) + 1;
            info_det.cant_devuelta = info_det.cant_devuelta;
            list.Add(info_det);
        }

        public void UpdateRow(in_devolucion_inven_det_Info info_det, decimal IdTransaccionSession)
        {
            in_devolucion_inven_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.secuencia == info_det.secuencia).First();
            edited_info.cant_devuelta = info_det.cant_devuelta;
        }

        public void DeleteRow(int secuencia, decimal IdTransaccionSession)
        {
            List<in_devolucion_inven_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.secuencia == secuencia).First());
        }
    }
}