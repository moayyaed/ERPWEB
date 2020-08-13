using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info;
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

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class PlantillaComprobanteContableController : Controller
    {
        #region Variables
        ct_cbtecble_Plantilla_Bus bus_CbteCble_Plantilla = new ct_cbtecble_Plantilla_Bus();
        ct_cbtecble_Plantilla_det_Bus bus_CbteCblePlantillaDet = new ct_cbtecble_Plantilla_det_Bus();
        ct_cbtecble_Plantilla_det_List CbteCble_PlantillaDet_Lista = new ct_cbtecble_Plantilla_det_List();
        ct_cbtecble_det_List List_Det_Cbte = new ct_cbtecble_det_List();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        ct_punto_cargo_Bus bus_pc = new ct_punto_cargo_Bus();
        ct_punto_cargo_grupo_Bus bus_pcg = new ct_punto_cargo_grupo_Bus();
        ct_cbtecble_Plantilla_List Lista_PlantillaComprobante = new ct_cbtecble_Plantilla_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        #endregion

        #region Metodos ComboBox bajo demanda

        public ActionResult CmbCuentaContable()
        {
            ct_cbtecble_Plantilla_det_Info model = new ct_cbtecble_Plantilla_det_Info();
            return PartialView("_CmbCuentaContable", model);
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
        public ActionResult CmbCentroCosto_CbteCblePlantilla()
        {
            string model = string.Empty;
            return PartialView("_CmbCentroCosto_CbteCblePlantilla", model);
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
        public ActionResult CmbPuntoCargo_CbteCblePlantilla()
        {
            string model = string.Empty;
            return PartialView("_CmbPuntoCargo_CbteCblePlantilla", model);
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlantillaComprobanteContable", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_cbtecble_Plantilla_Info model = new ct_cbtecble_Plantilla_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_CbteCble_Plantilla.GetList(model.IdEmpresa, true);
            Lista_PlantillaComprobante.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ComprobanteContablePlantilla(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //List<ct_cbtecble_Plantilla_Info> model = bus_CbteCble_Plantilla.GetList(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_PlantillaComprobante.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ComprobanteContablePlantilla", model);
        }
        #endregion

        #region Plantilla por asignar
        public ActionResult GridViewPartial_ComprobanteContablePlantilla_Asignar()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            List<ct_cbtecble_Plantilla_Info> model = bus_CbteCble_Plantilla.GetList(IdEmpresa, false);

            return PartialView("_GridViewPartial_ComprobanteContablePlantilla_Asignar", model);
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlantillaComprobanteContable", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            ct_cbtecble_Plantilla_Info model = new ct_cbtecble_Plantilla_Info
            {
                IdEmpresa = IdEmpresa,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdUsuarioCreacion = SessionFixed.IdUsuario
            };

            cargar_combos(model.IdEmpresa);
            CbteCble_PlantillaDet_Lista.set_list(new List<ct_cbtecble_Plantilla_det_Info>(), model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ct_cbtecble_Plantilla_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            model.cb_Estado = "A";
            model.lst_cbtecble_plantilla_det = CbteCble_PlantillaDet_Lista.get_list(model.IdTransaccionSession);

            if (!Validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            if (!bus_CbteCble_Plantilla.GuardarBD(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdPlantilla = model.IdPlantilla, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdPlantilla = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ct_cbtecble_Plantilla_Info model = bus_CbteCble_Plantilla.GetInfo(IdEmpresa, IdPlantilla);

            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlantillaComprobanteContable", "Index");
            if (model.cb_Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_cbtecble_plantilla_det = bus_CbteCblePlantillaDet.GetList(model.IdEmpresa, model.IdPlantilla);
            CbteCble_PlantillaDet_Lista.set_list(model.lst_cbtecble_plantilla_det, model.IdTransaccionSession);
            cargar_combos(model.IdEmpresa);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal IdPlantilla = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ct_cbtecble_Plantilla_Info model = bus_CbteCble_Plantilla.GetInfo(IdEmpresa, IdPlantilla);

            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlantillaComprobanteContable", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_cbtecble_plantilla_det = bus_CbteCblePlantillaDet.GetList(model.IdEmpresa, model.IdPlantilla);
            CbteCble_PlantillaDet_Lista.set_list(model.lst_cbtecble_plantilla_det, model.IdTransaccionSession);
            cargar_combos(model.IdEmpresa);

            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ct_cbtecble_Plantilla_Info model)
        {
            model.lst_cbtecble_plantilla_det = CbteCble_PlantillaDet_Lista.get_list(model.IdTransaccionSession);
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;

            if (!Validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            if (!bus_CbteCble_Plantilla.ModificarBD(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdPlantilla = model.IdPlantilla, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdPlantilla = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PlantillaComprobanteContable", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_cbtecble_Plantilla_Info model = bus_CbteCble_Plantilla.GetInfo(IdEmpresa, IdPlantilla);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_cbtecble_plantilla_det = bus_CbteCblePlantillaDet.GetList(model.IdEmpresa, model.IdPlantilla);
            CbteCble_PlantillaDet_Lista.set_list(model.lst_cbtecble_plantilla_det, model.IdTransaccionSession);
            cargar_combos(model.IdEmpresa);

            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_cbtecble_Plantilla_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
           if (!bus_CbteCble_Plantilla.AnularBD(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Metodos del detalle
        public ActionResult GridViewPartial_ComprobanteContablePlantillaDet()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = CbteCble_PlantillaDet_Lista.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_ComprobanteContablePlantillaDet", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ct_cbtecble_Plantilla_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);

            if (info_det != null)
                if (info_det.IdCtaCble != "")
                {
                    ct_plancta_Info info_Cuenta = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble);
                    if (info_Cuenta != null)
                    {
                        info_det.pc_Cuenta = info_Cuenta.pc_Cuenta;
                    }
                }

            CbteCble_PlantillaDet_Lista.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = CbteCble_PlantillaDet_Lista.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_ComprobanteContablePlantillaDet", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ct_cbtecble_Plantilla_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            if (info_det != null)
                if (info_det.IdCtaCble != "")
                {
                    ct_plancta_Info info_Cuenta = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble);
                    if (info_Cuenta != null)
                    {
                        info_det.IdCtaCble = info_Cuenta.IdCtaCble;
                        info_det.pc_Cuenta = info_Cuenta.pc_Cuenta;
                    }
                }
            CbteCble_PlantillaDet_Lista.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = CbteCble_PlantillaDet_Lista.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_ComprobanteContablePlantillaDet", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            CbteCble_PlantillaDet_Lista.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ct_cbtecble_Plantilla_Info model = new ct_cbtecble_Plantilla_Info();
            model.lst_cbtecble_plantilla_det = CbteCble_PlantillaDet_Lista.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_ComprobanteContablePlantillaDet", model.lst_cbtecble_plantilla_det);
        }

        private bool Validar(ct_cbtecble_Plantilla_Info i_validar, ref string msg)
        {
            i_validar.lst_cbtecble_plantilla_det = CbteCble_PlantillaDet_Lista.get_list(i_validar.IdTransaccionSession);

            if (i_validar.lst_cbtecble_plantilla_det.Count == 0)
            {
                mensaje = "Debe ingresar al menos una cuenta contable";
                return false;
            }

            if (i_validar.lst_cbtecble_plantilla_det.Where(q => string.IsNullOrEmpty(q.IdCtaCble)).Count() > 0)
            {
                mensaje = "Existen detalles sin cuenta contable";
                return false;
            }

            if (Math.Round(i_validar.lst_cbtecble_plantilla_det.Sum(q => q.dc_Valor_debe), 2) != Math.Round(i_validar.lst_cbtecble_plantilla_det.Sum(q => q.dc_Valor_haber), 2))
            {
                mensaje = "La suma del detalle del debe y del haber debe coincidir";
                return false;
            }

            return true;
        }
        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            ct_cbtecble_tipo_Bus bus_tipo_comprobante = new ct_cbtecble_tipo_Bus();
            var lst_tipo_comprobante = bus_tipo_comprobante.get_list(IdEmpresa, false);
            ViewBag.lst_tipo_comprobante = lst_tipo_comprobante;
        }
        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_punto_cargo_grupo = bus_pcg.GetList(IdEmpresa, false);
            ViewBag.lst_punto_cargo_grupo = lst_punto_cargo_grupo;
        }
        #endregion

        #region Json
        public JsonResult cargar_PlantillaComprobante(decimal IdPlantilla = 0)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);

            var info_plantilla = bus_CbteCble_Plantilla.GetInfo(IdEmpresa, IdPlantilla);
            var ListaPlantillaDetalle = bus_CbteCblePlantillaDet.GetList(IdEmpresa, IdPlantilla);
            var ListaDetalleCbte = new List<ct_cbtecble_det_Info>();
            var secuencia = 1;
            foreach (var item in ListaPlantillaDetalle)
            {                
                ListaDetalleCbte.Add(new ct_cbtecble_det_Info
                {
                    secuencia = secuencia++,
                    IdCtaCble = item.IdCtaCble,
                    pc_Cuenta = item.pc_Cuenta,
                    dc_Valor = item.dc_Valor,
                    dc_Valor_debe = item.dc_Valor > 0 ? Math.Abs(item.dc_Valor) : 0,
                    dc_Valor_haber = item.dc_Valor < 0 ? Math.Abs(item.dc_Valor) : 0,
                    dc_Observacion = item.dc_Observacion,
                    IdPunto_cargo = item.IdPunto_cargo,
                    IdPunto_cargo_grupo = item.IdPunto_cargo_grupo,
                    IdCentroCosto = item.IdCentroCosto,
                    cc_Descripcion = item.cc_Descripcion,
                    nom_punto_cargo = item.nom_punto_cargo
                });
            }

            List_Det_Cbte.set_list(ListaDetalleCbte, IdTransaccionSession);
            return Json(info_plantilla, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class ct_cbtecble_Plantilla_List
    {
        string Variable = "ct_cbtecble_Plantilla_Info";
        public List<ct_cbtecble_Plantilla_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_cbtecble_Plantilla_Info> list = new List<ct_cbtecble_Plantilla_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_cbtecble_Plantilla_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_cbtecble_Plantilla_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }

    public class ct_cbtecble_Plantilla_det_List
    {
        string Variable = "ct_cbtecble_Plantilla_det_Info";
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        ct_punto_cargo_Bus bus_pc = new ct_punto_cargo_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        public List<ct_cbtecble_Plantilla_det_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ct_cbtecble_Plantilla_det_Info> list = new List<ct_cbtecble_Plantilla_det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ct_cbtecble_Plantilla_det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ct_cbtecble_Plantilla_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ct_cbtecble_Plantilla_det_Info info_det, decimal IdTransaccionSession)
        {
            List<ct_cbtecble_Plantilla_det_Info> list = get_list(IdTransaccionSession);
            //if (list.Where(q => q.IdPlantilla == info_det.IdPlantilla).Count() == 0)
            //{
                info_det.secuencia = list.Count == 0 ? 1 : list.Max(q => q.secuencia) + 1;
                info_det.dc_Valor = info_det.dc_Valor_debe > 0 ? info_det.dc_Valor_debe : info_det.dc_Valor_haber * -1;

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
            //}
        }

        public void UpdateRow(ct_cbtecble_Plantilla_det_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ct_cbtecble_Plantilla_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.secuencia == info_det.secuencia).First();

            edited_info.dc_Valor = info_det.dc_Valor_debe > 0 ? info_det.dc_Valor_debe : info_det.dc_Valor_haber * -1;
            edited_info.dc_Valor_debe = info_det.dc_Valor_debe;
            edited_info.dc_Valor_haber = info_det.dc_Valor_haber;
            edited_info.dc_Observacion = info_det.dc_Observacion;

            #region Cuenta
            edited_info.IdCtaCble = info_det.IdCtaCble;
            ct_plancta_Info info_Cuenta = bus_plancta.get_info(IdEmpresa, info_det.IdCtaCble);
            if (info_Cuenta != null)
            { 
                edited_info.pc_Cuenta = info_Cuenta.pc_Cuenta;
            }
            #endregion

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
            else
            {
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
            List<ct_cbtecble_Plantilla_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.secuencia == secuencia).FirstOrDefault());
        }
    }
}