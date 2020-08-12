using Core.Erp.Bus.Banco;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Presupuesto;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    [SessionTimeout]
    public class ComprobanteContableController : Controller
    {
        #region Variables
        ct_cbtecble_Bus bus_comprobante = new ct_cbtecble_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        ct_cbtecble_det_Bus bus_comprobante_detalle = new ct_cbtecble_det_Bus();
        ct_cbtecble_det_List list_ct_cbtecble_det = new ct_cbtecble_det_List();
        string mensaje = string.Empty;
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        //tb_sis_log_error_List SisLogError = new tb_sis_log_error_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ct_punto_cargo_Bus bus_pc = new ct_punto_cargo_Bus();
        ct_punto_cargo_grupo_Bus bus_pcg = new ct_punto_cargo_grupo_Bus();
        ba_Conciliacion_det_IngEgr_Bus bus_ConciliacionDet = new ba_Conciliacion_det_IngEgr_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        ct_cbtecble_List Lista_Comprobante = new ct_cbtecble_List();
        #endregion

        #region Metodos ComboBox bajo demanda

        public ActionResult CmbCuenta_comprobante_contable()
        {
            ct_cbtecble_det_Info model = new ct_cbtecble_det_Info();
            return PartialView("_CmbCuenta_comprobante_contable", model);
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
        #region Metodos ComboBox bajo demanda centro de costo
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();

        public ActionResult CmbCentroCosto_Conta()
        {
            string model = string.Empty;
            return PartialView("_CmbCentroCosto_Conta", model);
        }
        public List<ct_CentroCosto_Info> get_list_bajo_demandaCC(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            List<ct_CentroCosto_Info> Lista = bus_cc.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
            return Lista;
        }
        public ct_CentroCosto_Info get_info_bajo_demandaCC(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_cc.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion
        #region Metodos ComboBox bajo demanda punto de cargo        
        public ActionResult CmbPuntoCargo()
        {
            string model = string.Empty;
            return PartialView("_CmbPuntoCargo", model);
        }
        public List<ct_punto_cargo_Info> get_list_bajo_demandaPC(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            int IdPunto_cargo_grupo = 0;
            List<ct_punto_cargo_Info> Lista = bus_pc.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), IdPunto_cargo_grupo);
            return Lista;
        }
        public ct_punto_cargo_Info get_info_bajo_demandaPC(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_pc.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ComprobanteContable", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };
            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_comprobante.get_list(model.IdEmpresa, model.IdSucursal, true, model.fecha_ini, model.fecha_fin);
            Lista_Comprobante.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ComprobanteContable", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_comprobante.get_list(model.IdEmpresa, model.IdSucursal, true, model.fecha_ini, model.fecha_fin);
            Lista_Comprobante.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_comprobante_contable(bool Nuevo=false)
        {
            ////ViewBag.fecha_ini = fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : fecha_ini;
            ////ViewBag.fecha_fin = fecha_fin == null ? DateTime.Now.Date : fecha_fin;
            ////ViewBag.IdEmpresa = IdEmpresa;
            ////ViewBag.IdSucursal = IdSucursal;
            ////List<ct_cbtecble_Info> model = bus_comprobante.get_list(IdEmpresa, IdSucursal, true, ViewBag.fecha_ini, ViewBag.fecha_fin);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Comprobante.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_comprobante_contable", model);
        }

        public void CargarCombosConsulta(int IdEmpresa)
        {
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        #endregion

        #region Metodos
        private bool validar(ct_cbtecble_Info i_validar, ref string msg)
        {
            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.cb_Fecha, cl_enumeradores.eModulo.CONTA, i_validar.IdSucursal, ref mensaje))
            {
                return false;
            }

            if (i_validar.lst_ct_cbtecble_det.Count == 0)
            {
                mensaje = "Debe ingresar registros en el detalle";
                return false;
            }

            if(Math.Round(i_validar.lst_ct_cbtecble_det.Sum(q => q.dc_Valor),2) != 0)
            {
                mensaje = "La suma de los detalles debe ser 0";
                return false;
            }

            if (i_validar.lst_ct_cbtecble_det.Where(q=>q.dc_Valor == 0).Count() > 0)
            {
                mensaje = "Existen detalles con valor 0 en el debe o haber";
                return false;
            }

            if (i_validar.lst_ct_cbtecble_det.Where(q => string.IsNullOrEmpty(q.IdCtaCble)).Count() > 0)
            {
                mensaje = "Existen detalles sin cuenta contable";
                return false;
            }

            return true;
        }
        private void cargar_combos(int IdEmpresa)
        {            
            ct_cbtecble_tipo_Bus bus_tipo_comprobante = new ct_cbtecble_tipo_Bus();
            var lst_tipo_comprobante = bus_tipo_comprobante.get_list(IdEmpresa, false);
            ViewBag.lst_tipo_comprobante = lst_tipo_comprobante;

            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, false);
            ViewBag.lst_sucursal = lst_sucursal;
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ComprobanteContable", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_cbtecble_Info model = new ct_cbtecble_Info
            {
                IdEmpresa = IdEmpresa,
                cb_Fecha = DateTime.Now,
                IdTipoCbte = 1,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal)
            };
            model.lst_ct_cbtecble_det = new List<ct_cbtecble_det_Info>();
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det, model.IdTransaccionSession);
            cargar_combos(model.IdEmpresa);
           
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ct_cbtecble_Info model)
        {
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
            if (!validar(model,ref mensaje))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_comprobante.guardarDB(model))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoCbte = model.IdTipoCbte, IdCbteCble = model.IdCbteCble, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdTipoCbte = 0, decimal IdCbteCble = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ct_cbtecble_Info model = bus_comprobante.get_info(IdEmpresa, IdTipoCbte, IdCbteCble);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ComprobanteContable", "Index");
            if (model.cb_Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, IdTipoCbte, IdCbteCble);
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det, model.IdTransaccionSession);
            cargar_combos(model.IdEmpresa);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cb_Fecha, cl_enumeradores.eModulo.CONTA, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            #region Validacion de conciliación bancaria
            if (!bus_ConciliacionDet.ValidarComprobanteEnConciliacion(IdEmpresa, IdTipoCbte, IdCbteCble, ref mensaje))
            {
                ViewBag.mensaje = "El comprobante se encuentra en una conciliación bancaria y solo se modificará la observación";
                ViewBag.MostrarBoton = true;
            }
            #endregion

            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, int IdTipoCbte = 0, decimal IdCbteCble = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            
            ct_cbtecble_Info model = bus_comprobante.get_info(IdEmpresa, IdTipoCbte, IdCbteCble);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ComprobanteContable", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, IdTipoCbte, IdCbteCble);
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det,model.IdTransaccionSession);
            cargar_combos(model.IdEmpresa);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cb_Fecha, cl_enumeradores.eModulo.CONTA, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            #region Validacion de conciliación bancaria
            if (!bus_ConciliacionDet.ValidarComprobanteEnConciliacion(IdEmpresa, IdTipoCbte, IdCbteCble, ref mensaje))
            {
                ViewBag.mensaje = "El comprobante se encuentra en una conciliación bancaria y solo se modificará la observación";
                ViewBag.MostrarBoton = true;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ct_cbtecble_Info model)
        {
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuarioUltModi = SessionFixed.IdUsuario;
            if (!bus_comprobante.modificarDB(model))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoCbte = model.IdTipoCbte, IdCbteCble = model.IdCbteCble, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdTipoCbte = 0, decimal IdCbteCble = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "ComprobanteContable", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_cbtecble_Info model = bus_comprobante.get_info(IdEmpresa, IdTipoCbte, IdCbteCble);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, IdTipoCbte, IdCbteCble);
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det, model.IdTransaccionSession);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cb_Fecha, cl_enumeradores.eModulo.CONTA, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            cargar_combos(model.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_cbtecble_Info model)
        {
            model.IdUsuarioAnu = SessionFixed.IdUsuario;
            if (!bus_comprobante.anularDB(model))
            {
                ViewBag.MostrarBoton = true;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");

        }

        #endregion

        #region GRids

        [ValidateInput(false)]
        public ActionResult GridViewPartial_comprobante_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            ct_cbtecble_Info model = new ct_cbtecble_Info();
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_comprobante_detalle", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_comprobante_detalle_readonly()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            ct_cbtecble_Info model = new ct_cbtecble_Info();
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            
            return PartialView("_GridViewPartial_comprobante_detalle_readonly", model);
        }

        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_punto_cargo_grupo = bus_pcg.GetList(IdEmpresa, false);
            ViewBag.lst_punto_cargo_grupo = lst_punto_cargo_grupo;
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ct_cbtecble_det_Info info_det)
        {
            if (ModelState.IsValid)
                list_ct_cbtecble_det.AddRow(info_det,Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ct_cbtecble_Info model = new ct_cbtecble_Info();
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_comprobante_detalle", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ct_cbtecble_det_Info info_det)
        {
           
            if (ModelState.IsValid)
                list_ct_cbtecble_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ct_cbtecble_Info model = new ct_cbtecble_Info();
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_comprobante_detalle", model);
        }

        public ActionResult EditingDelete(int secuencia)
        {
            list_ct_cbtecble_det.DeleteRow(secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ct_cbtecble_Info model = new ct_cbtecble_Info();
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_comprobante_detalle", model);
        }

        public ActionResult CargarPuntoCargo()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);            
            int IdPunto_cargo_grupo =  (Request.Params["fx_IdPunto_cargo_grupo"] != null && Request.Params["fx_IdPunto_cargo_grupo"] != "") ? Convert.ToInt32(Request.Params["fx_IdPunto_cargo_grupo"]) : 0;
            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = "nom_punto_cargo";
                p.ValueField = "IdPunto_cargo";
                p.Columns.Add("IdPunto_cargo", "ID", 10);
                p.Columns.Add("nom_punto_cargo", "Punto de cargo", 90);
                p.ClientSideEvents.BeginCallback = "PuntoCargoComboBox_BeginCallback";
                p.ValueType = typeof(int);
                p.BindList(bus_pc.GetList(IdEmpresa, IdPunto_cargo_grupo, false,false));
            });
        }
        #endregion

        #region Importacion
        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControlFile", UploadControlSettingscbte.UploadValidationSettings, UploadControlSettingscbte.FileUploadComplete);
            return null;
        }
        public ActionResult Importar(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ct_cbtecble_Info model = new ct_cbtecble_Info
            {
                IdEmpresa = IdEmpresa,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Importar(ct_cbtecble_Info model)
        {
            try
            {
                var ListaDet = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
                model.lst_ct_cbtecble_det = ListaDet;
            }
            catch (Exception ex)
            {
                //SisLogError.set_list((ex.InnerException) == null ? ex.Message.ToString() : ex.InnerException.ToString());

                ViewBag.error = ex.Message.ToString();
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public JsonResult ActualizarVariablesSession(int IdEmpresa = 0, decimal IdTransaccionSession = 0)
        {
            string retorno = string.Empty;
            SessionFixed.IdEmpresa = IdEmpresa.ToString();
            SessionFixed.IdTransaccionSession = IdTransaccionSession.ToString();
            SessionFixed.IdTransaccionSessionActual = IdTransaccionSession.ToString();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }

    public class UploadControlSettingscbte
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".xlsx" },
            MaxFileSize = 40000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            #region Variables
            ct_plancta_Bus bus_ctacble = new ct_plancta_Bus();
            ct_cbtecble_det_List ListaDet = new ct_cbtecble_det_List();
            List<ct_cbtecble_det_Info> Lista_Det = new List<ct_cbtecble_det_Info>();

            int cont = 0;
            decimal IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            #endregion


            Stream stream = new MemoryStream(e.UploadedFile.FileBytes);
            if (stream.Length > 0)
            {
                IExcelDataReader reader = null;
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                var SecDet = 1;
                #region Presupuesto                
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && cont > 0)
                    {
                        var IdCtaCble = Convert.ToString(reader.GetValue(1));
                        ct_plancta_Info infoCtaCble = bus_ctacble.get_info(IdEmpresa, IdCtaCble);

                        if(infoCtaCble!= null)
                        {
                            ct_cbtecble_det_Info info = new ct_cbtecble_det_Info
                            {
                                IdEmpresa = IdEmpresa,
                                secuencia = SecDet++,
                                IdCtaCble = IdCtaCble,
                                pc_Cuenta = infoCtaCble.pc_Cuenta,
                                dc_Valor = Convert.ToDouble(reader.GetValue(2)) > 0 ? Convert.ToDouble(reader.GetValue(2)) : (Convert.ToDouble(reader.GetValue(3)) * -1),
                                dc_Valor_debe = Convert.ToDouble(reader.GetValue(2)),
                                dc_Valor_haber = Convert.ToDouble(reader.GetValue(3))
                            };
                            Lista_Det.Add(info);
                        }                        
                    }
                    else
                        cont++;
                }
                ListaDet.set_list(Lista_Det, IdTransaccionSession);
                #endregion
            }
        }
    }

    public class ct_cbtecble_List
    {
        string Variable = "ct_cbtecble_Info";
        public List<ct_cbtecble_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ct_cbtecble_Info> list = new List<ct_cbtecble_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ct_cbtecble_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ct_cbtecble_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class ct_cbtecble_det_List
    {
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        string variable = "ct_cbtecble_det_Info";
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        ct_punto_cargo_Bus bus_pc = new ct_punto_cargo_Bus();
        public List<ct_cbtecble_det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<ct_cbtecble_det_Info> list = new List<ct_cbtecble_det_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ct_cbtecble_det_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ct_cbtecble_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ct_cbtecble_det_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);

            List<ct_cbtecble_det_Info> list = get_list(IdTransaccionSession);
            info_det.secuencia = list.Count == 0 ? 1 : list.Max(q => q.secuencia) + 1;
            info_det.dc_Valor = info_det.dc_Valor_debe > 0 ? info_det.dc_Valor_debe : info_det.dc_Valor_haber * -1;
            if (info_det.IdCtaCble != null)
            {
                var cta = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble);
                if (cta != null)
                    info_det.pc_Cuenta = cta.IdCtaCble + " - " + cta.pc_Cuenta;
            }

            #region Centro de costo
            if (string.IsNullOrEmpty(info_det.IdCentroCosto))
                info_det.cc_Descripcion = string.Empty;
            else
            {
                var cc = bus_cc.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), info_det.IdCentroCosto);
                if (cc != null)
                    info_det.cc_Descripcion = cc.cc_Descripcion;
            }
            #endregion

            #region Punto de cargo
            if (info_det.IdPunto_cargo == null || info_det.IdPunto_cargo == 0)
                info_det.nom_punto_cargo = string.Empty;
            else
            {
                var pc = bus_pc.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(info_det.IdPunto_cargo));
                if (pc != null)
                    info_det.nom_punto_cargo = pc.nom_punto_cargo;
            }
            #endregion
            list.Add(info_det);
        }

        public void UpdateRow(ct_cbtecble_det_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);

            ct_cbtecble_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.secuencia == info_det.secuencia).First();
            edited_info.IdCtaCble = info_det.IdCtaCble;
            edited_info.dc_para_conciliar = info_det.dc_para_conciliar;
            edited_info.dc_Valor = info_det.dc_Valor_debe > 0 ? info_det.dc_Valor_debe : info_det.dc_Valor_haber * - 1;
            edited_info.dc_Valor_debe = info_det.dc_Valor_debe;
            edited_info.dc_Valor_haber = info_det.dc_Valor_haber;
            edited_info.dc_Observacion = info_det.dc_Observacion;

            var cta = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble);
            if (cta != null)
                info_det.pc_Cuenta = cta.IdCtaCble + " - " + cta.pc_Cuenta;
            edited_info.pc_Cuenta = info_det.pc_Cuenta;

            #region Centro de costo
            edited_info.IdCentroCosto = info_det.IdCentroCosto;
            if (string.IsNullOrEmpty(info_det.IdCentroCosto))
                edited_info.cc_Descripcion = string.Empty;
            else
            {
                var cc = bus_cc.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), info_det.IdCentroCosto);
                if (cc != null)
                {
                    edited_info.cc_Descripcion = cc.cc_Descripcion;
                }
            }
            #endregion

            #region Punto Cargo
            edited_info.IdPunto_cargo = info_det.IdPunto_cargo;
            edited_info.IdPunto_cargo_grupo = info_det.IdPunto_cargo_grupo;
            if (info_det.IdPunto_cargo == null || info_det.IdPunto_cargo == 0)
                edited_info.nom_punto_cargo = string.Empty;
            else { 
                var pc = bus_pc.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(info_det.IdPunto_cargo));
                if (pc != null)
                {
                    edited_info.nom_punto_cargo = pc.nom_punto_cargo;
                }
            }
            #endregion

        }

        public void DeleteRow(int secuencia, decimal IdTransaccionSession)
        {
            List<ct_cbtecble_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.secuencia == secuencia).First());
        }
    }
}