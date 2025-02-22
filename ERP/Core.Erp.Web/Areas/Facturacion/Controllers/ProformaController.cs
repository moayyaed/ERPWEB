﻿using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Areas.Inventario.Controllers;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class ProformaController : Controller
    {
        #region Variables
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_proforma_Bus bus_proforma = new fa_proforma_Bus();
        fa_proforma_det_Bus bus_proforma_det = new fa_proforma_det_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        fa_cliente_x_fa_Vendedor_x_sucursal_Bus bus_v_x_c = new fa_cliente_x_fa_Vendedor_x_sucursal_Bus();
        fa_proforma_det_List List_det = new fa_proforma_det_List();
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        tb_sis_Impuesto_Bus bus_impuesto = new tb_sis_Impuesto_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        in_Producto_x_fa_NivelDescuento_Bus bus_nivelproducto = new in_Producto_x_fa_NivelDescuento_Bus();
        fa_proforma_det_Bus bus_det = new fa_proforma_det_Bus();
        in_Producto_List List_producto = new in_Producto_List();
        string mensaje = string.Empty;
        fa_parametro_Bus bus_param = new fa_parametro_Bus();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        fa_NivelDescuento_Bus bus_nivel = new fa_NivelDescuento_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        fa_TerminoPago_Bus bus_pago = new fa_TerminoPago_Bus();
        tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
        fa_Vendedor_Bus bus_vendedor = new fa_Vendedor_Bus();
        fa_catalogo_Bus bus_catalogo = new fa_catalogo_Bus();
        fa_cliente_contactos_Bus bus_contacto = new fa_cliente_contactos_Bus();
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        tb_sucursal_FormaPago_x_fa_NivelDescuento_Bus bus_formapago_x_niveldescuento = new tb_sucursal_FormaPago_x_fa_NivelDescuento_Bus();
        fa_cliente_contactos_Bus bus_cliente_contactos = new fa_cliente_contactos_Bus();
        fa_proforma_List Lista_Proforma = new fa_proforma_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Proforma", "Index");
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
            var lst = bus_proforma.get_list(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin);
            Lista_Proforma.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Proforma", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_proforma.get_list(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin);
            Lista_Proforma.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_proforma(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //List<fa_proforma_Info> model = new List<fa_proforma_Info>();
            //ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            //ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);
            //ViewBag.IdSucursal = IdSucursal;
            //model = bus_proforma.get_list(IdEmpresa, IdSucursal, ViewBag.Fecha_ini, ViewBag.Fecha_fin);

            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Proforma.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_proforma", model);
        }

        #endregion
        #region Metodos ComboBox bajo demanda cliente
        public ActionResult CmbCliente_Proforma()
        {
            fa_proforma_Info model = new fa_proforma_Info();
            return PartialView("_CmbCliente_Proforma", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        #endregion
        #region Metodos ComboBox bajo demanda producto
        public ActionResult ChangeValuePartial(decimal value = 0)
        {
            return PartialView("_CmbProducto_Proforma", value);
        }
        public ActionResult CmbProducto_Proforma()
        {
            decimal model = new decimal();
            return PartialView("_CmbProducto_Proforma", model);
        }
        public List<in_Producto_Info> get_list_bajo_demandaProducto(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            List<in_Producto_Info> Lista = bus_producto.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoBusquedaProducto.PORSUCURSAL, cl_enumeradores.eModulo.FAC, Convert.ToInt32(SessionFixed.IdSucursal));
            return Lista;
        }
        public in_Producto_Info get_info_bajo_demandaProducto(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_producto.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion
        #region Metodos ComboBox bajo demanda centro de costo

        public ActionResult CmbCentroCosto_Proforma()
        {
            string model = string.Empty;
            return PartialView("_CmbCentroCosto_Proforma", model);
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
        #region Metodos

        private bool validar(fa_proforma_Info i_validar, ref string msg)
        {
            i_validar.IdEntidad = i_validar.IdCliente;
            i_validar.lst_det = List_det.get_list(Convert.ToDecimal(i_validar.IdTransaccionSession));
            if (i_validar.lst_det.Count == 0)
            {
                msg = "No ha ingresado registros en el detalle de la proforma";
                return false;
            }
            if (i_validar.lst_det.Where(q => q.pd_cantidad == 0).Count() > 0)
            {
                msg = "Existen registros con cantidad 0 en el detalle de la proforma";
                return false;
            }
            if (i_validar.lst_det.Where(q => q.IdProducto == 0).Count() > 0)
            {
                msg = "Existen registros sin producto en el detalle de la proforma";
                return false;
            }
            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.pf_fecha, cl_enumeradores.eModulo.FAC, i_validar.IdSucursal, ref msg))
            {
                return false;
            }
            return true;
        }
        private void CargarCombosConsulta(int IdEmpresa)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;
        }
        private void cargar_combos(fa_proforma_Info model)
        {
            var lst_sucursal = bus_sucursal.GetList(model.IdEmpresa, SessionFixed.IdUsuario, false);
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_bodega = bus_bodega.get_list(model.IdEmpresa, false);
            ViewBag.lst_bodega = lst_bodega;

            var lst_vendedor = bus_vendedor.get_list(model.IdEmpresa, false);
            ViewBag.lst_vendedor = lst_vendedor;

            var lst_pago = bus_pago.get_list(false);
            ViewBag.lst_pago = lst_pago;

            var lst_NivelDescuento = bus_nivel.GetList(model.IdEmpresa, false);
            ViewBag.lst_NivelDescuento = lst_NivelDescuento;

            var lst_formapago = bus_catalogo.get_list((int)cl_enumeradores.eTipoCatalogoFact.FormaDePago, false);
            ViewBag.lst_formapago = lst_formapago;

            var lst_cliente_contactos = bus_cliente_contactos.get_list(model.IdEmpresa, model.IdCliente);
            ViewBag.lst_cliente_contactos = lst_cliente_contactos;
        }
        #endregion
        #region acciones
        public ActionResult Nuevo(int IdEmpresa = 0 )
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Proforma", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_proforma_Info model = new fa_proforma_Info
            {
                IdEmpresa = IdEmpresa,
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                pf_fecha = DateTime.Now,
                pf_fecha_vcto = DateTime.Now,
                lst_det = new List<fa_proforma_det_Info>(),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_proforma_Info model)
        {
            model.lst_det = List_det.get_list(model.IdTransaccionSession);
            
            if (!validar(model, ref mensaje))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                ViewBag.mensaje = mensaje;
                cargar_combos(model);
                return View(model);
            }
            model.IdUsuario_creacion = Session["IdUsuario"].ToString();
            if (!bus_proforma.guardarDB(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                ViewBag.mensaje = mensaje;
                cargar_combos(model);
                return View(model);
            };
           return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdProforma = model.IdProforma, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdSucursal = 0, decimal IdProforma = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            fa_proforma_Info model = bus_proforma.get_info(IdEmpresa, IdSucursal, IdProforma);
            var info_vista = bus_proforma.get_info_vw(IdEmpresa, IdSucursal, IdProforma);
            model.EstadoCierre = (info_vista == null ? "" : info_vista.EstadoCierre);

            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Proforma", "Index");
            if (model.estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }

            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            model.IdEntidad = model.IdCliente;
            model.lst_det = bus_det.get_list(IdEmpresa, IdSucursal, IdProforma);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(model);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.pf_fecha, cl_enumeradores.eModulo.FAC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, int IdSucursal = 0, decimal IdProforma = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Proforma", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_proforma_Info model = bus_proforma.get_info(IdEmpresa, IdSucursal, IdProforma);
            if (model == null)
                return RedirectToAction("Index");
            model.IdEntidad = model.IdCliente;
            model.lst_det = bus_det.get_list(IdEmpresa, IdSucursal, IdProforma);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(model);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.pf_fecha, cl_enumeradores.eModulo.FAC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_proforma_Info model)
        {
            model.lst_det = List_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model);
                return View(model);
            }
            model.IdUsuario_modificacion = Session["IdUsuario"].ToString();

            if (!bus_proforma.modificarDB(model))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model);
                return View(model);
            };

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdProforma = model.IdProforma, Exito = true });
        }

        public ActionResult ModificarProFact(int IdEmpresa = 0, int IdSucursal = 0, decimal IdProforma = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Proforma", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_proforma_Info model = bus_proforma.get_info(IdEmpresa, IdSucursal, IdProforma);
            if (model == null)
                return RedirectToAction("Index");
            model.IdEntidad = model.IdCliente;
            model.lst_det = bus_det.get_list(IdEmpresa, IdSucursal, IdProforma);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(model);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.pf_fecha, cl_enumeradores.eModulo.FAC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            
            return View(model);
        }

        [HttpPost]
        public ActionResult ModificarProFact(fa_proforma_Info model)
        {
            model.lst_det = List_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model);
                return View(model);
            }
            model.IdUsuario_modificacion = Session["IdUsuario"].ToString();

            if (!bus_proforma.modificarDB(model))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model);
                return View(model);
            };

            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdProforma = model.IdProforma, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0 , int IdSucursal = 0, decimal IdProforma = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Proforma", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_proforma_Info model = bus_proforma.get_info(IdEmpresa, IdSucursal, IdProforma);
            if (model == null)
                return RedirectToAction("Index");
            model.IdEntidad = model.IdCliente;
            model.lst_det = bus_det.get_list(IdEmpresa, IdSucursal, IdProforma);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(model);
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.pf_fecha, cl_enumeradores.eModulo.FAC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_proforma_Info model)
        {
            model.IdUsuario_anulacion = SessionFixed.IdUsuario.ToString();

            if (!bus_proforma.anularDB(model))
            {
                cargar_combos(model);
                return View(model);
            };
            return RedirectToAction("Index");
        }
        #endregion
        #region json
        public JsonResult cargar_bodega(int IdEmpresa = 0, int IdSucursal = 0)
        {
            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var resultado = bus_bodega.get_list(IdEmpresa, IdSucursal, false);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_info_termino_pago(string IdTerminoPago = "")
        {
            fa_TerminoPago_Bus bus_termino_pago = new fa_TerminoPago_Bus();
            var resultado = bus_termino_pago.get_info(IdTerminoPago);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ModificarLineaProducto(int Secuencia = 0, decimal IdTransaccionSession = 0, decimal IdProducto = 0)
        {
            var linea = List_det.get_list(IdTransaccionSession).Where(q => q.Secuencia == Secuencia).FirstOrDefault();
            if (linea != null)
            {
                var producto = bus_producto.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdProducto);
                if (producto != null)
                {
                    linea.IdProducto = IdProducto;
                    linea.pr_descripcion = producto.pr_descripcion_combo;
                }
                List_det.UpdateRow(linea, IdTransaccionSession);
            }
            return Json(linea, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuscarProducto(int IdSucursal = 0, int IdBodega = 0, int Secuencia = 0, decimal IdTransaccionSession = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var linea = List_det.get_list(IdTransaccionSession).Where(q => q.Secuencia == Secuencia).FirstOrDefault();

            var resultado = bus_producto.get_info(IdEmpresa, linea == null ? 0 : linea.IdProducto);
            if (resultado == null)
                resultado = new in_Producto_Info();

            if (resultado.IdProducto_padre > 0)
                List_producto.set_list(bus_producto.get_list_stock_lotes(IdEmpresa, IdSucursal, IdBodega, Convert.ToDecimal(resultado.IdProducto_padre)));
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Desbloquear(string Contrasenia = "")
        {
            string EstadoDesbloqueo = "";

            var param = bus_param.get_info(Convert.ToInt32(SessionFixed.IdEmpresa));
            if (param != null)
            {
                if (Contrasenia.ToLower() == param.clave_desbloqueo_precios.ToLower())
                {
                    EstadoDesbloqueo = "DESBLOQUEADO";
                }
            }

            return Json(EstadoDesbloqueo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLineaDetalle(int Secuencia = 0, decimal IdTransaccionSession = 0)
        {
            fa_proforma_det_Info lineaF = new fa_proforma_det_Info();

            var linea = List_det.get_list(IdTransaccionSession).Where(q => q.Secuencia == Secuencia).FirstOrDefault();
            if (linea != null)
                lineaF = linea;
            return Json(linea, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ModificarLinea(int Secuencia = 0, decimal IdTransaccionSession = 0, double Precio = 0, double PorDescuento = 0, bool AplicarTodaFactura = false)
        {

            var lista = List_det.get_list(IdTransaccionSession);
            if (AplicarTodaFactura)
            {
                foreach (var linea in lista)
                {
                    if (linea.Secuencia == Secuencia)
                        linea.pd_precio = Precio;

                    linea.pd_por_descuento_uni = PorDescuento;
                    List_det.UpdateRow(linea, IdTransaccionSession);
                }
            }
            else
            {
                var linea = lista.Where(q => q.Secuencia == Secuencia).FirstOrDefault();
                if (linea != null)
                {
                    linea.pd_precio = Precio;
                    linea.pd_por_descuento_uni = PorDescuento;
                    List_det.UpdateRow(linea, IdTransaccionSession);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_info_cliente(decimal IdCliente = 0, int IdSucursal = 0)
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
            fa_TerminoPago_Bus bus_termino = new fa_TerminoPago_Bus();
            fa_cliente_Info resultado = bus_cliente.get_info(IdEmpresa, IdCliente);
            fa_cliente_x_fa_Vendedor_x_sucursal_Info info_vendedor = bus_v_x_c.get_info(IdEmpresa, IdCliente, IdSucursal);
            //var ultima_proforma = bus_proforma.get_info_ultima_proforma(IdEmpresa, IdSucursal, IdCliente);
            string IdTerminoPago = "";
            int Dias_Vct = 0;
            int IdVendedor = 1;

            if (resultado == null)
            {
                resultado = new fa_cliente_Info
                {
                    info_persona = new tb_persona_Info()
                };
            }else
            {
                if(info_vendedor != null)
                    IdVendedor = info_vendedor.IdVendedor;

                IdTerminoPago = resultado.IdTipoCredito;
                var info_termino = bus_termino.get_info(IdTerminoPago);

                if (info_termino != null)
                    Dias_Vct = info_termino.Dias_Vct;
            }

            return Json(new { IdVendedor = IdVendedor, IdTerminoPago = IdTerminoPago, Dias_Vct = Dias_Vct } , JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_NivelDescuento_x_FormaPago(int IdEmpresa = 0, int IdSucursal = 0, string IdCatalogo_FormaPago = "")
        {
            tb_sucursal_FormaPago_x_fa_NivelDescuento_Info info_NivelDescuento_x_FormaPago = new tb_sucursal_FormaPago_x_fa_NivelDescuento_Info();

            info_NivelDescuento_x_FormaPago = bus_formapago_x_niveldescuento.GetInfo(IdEmpresa, IdSucursal, IdCatalogo_FormaPago);
            var IdNivel = info_NivelDescuento_x_FormaPago == null ? 0 : info_NivelDescuento_x_FormaPago.IdNivel;

            return Json(IdNivel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalcularValores(int Cantidad = 0, double Precio = 0, string IdCodImpuesto = "", double PorcentajeDesc = 0)
        {
            double subtotal = 0;
            double iva_porc = 0;
            double iva = 0;
            double total = 0;
            double DescUnitario = 0;
            double PrecioFinal = 0;

            DescUnitario = Convert.ToDouble(Precio * (PorcentajeDesc / 100));
            PrecioFinal = Precio - DescUnitario;
            subtotal = Math.Round(Convert.ToDouble(Cantidad * PrecioFinal), 2, MidpointRounding.AwayFromZero);

            var impuesto = bus_impuesto.get_info(IdCodImpuesto);
            if (impuesto != null)
                iva_porc = impuesto.porcentaje;

            iva = Math.Round(subtotal * (iva_porc / 100),2, MidpointRounding.AwayFromZero);
            total = Math.Round((subtotal + iva), 2 , MidpointRounding.AwayFromZero);

            return Json(new { subtotal = subtotal, iva = iva, total = total }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region funciones del detalle
        private void cargar_combos_detalle()
        {
            var lst_impuesto = bus_impuesto.get_list("IVA", false);
            ViewBag.lst_impuesto = lst_impuesto;
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_proforma_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            SessionFixed.IdNivelDescuento = Request.Params["NivelDescuento"] != null ? Request.Params["NivelDescuento"].ToString() : SessionFixed.IdNivelDescuento;
            SessionFixed.IdEntidad = !string.IsNullOrEmpty(Request.Params["IdCliente"]) ? Request.Params["IdCliente"].ToString() : "-1";
            var model = List_det.get_list( Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();            
            return PartialView("_GridViewPartial_proforma_det", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_proforma_facturada_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            SessionFixed.IdNivelDescuento = Request.Params["NivelDescuento"] != null ? Request.Params["NivelDescuento"].ToString() : SessionFixed.IdNivelDescuento;
            SessionFixed.IdEntidad = !string.IsNullOrEmpty(Request.Params["IdCliente"]) ? Request.Params["IdCliente"].ToString() : "-1";
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_proforma_facturada_det", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] fa_proforma_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            decimal IdCliente = Convert.ToDecimal(SessionFixed.IdEntidad);
            int IdNivelDescuento = Convert.ToInt32(SessionFixed.IdNivelDescuento);
            if (info_det != null && info_det.IdProducto != 0)
            {
                var producto = bus_producto.get_info(IdEmpresa, info_det.IdProducto);
                if (producto != null)
                {
                    info_det.pr_descripcion = producto.pr_descripcion_combo;                  
                }
            }

            if (ModelState.IsValid)
                List_det.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_proforma_det", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] fa_proforma_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            decimal IdCliente = Convert.ToDecimal(SessionFixed.IdEntidad);
            int IdNivelDescuento = Convert.ToInt32(SessionFixed.IdNivelDescuento);
            if (info_det != null && info_det.IdProducto != 0)
            {
                var producto = bus_producto.get_info(IdEmpresa, info_det.IdProducto);
                if (producto != null)
                {
                    info_det.pr_descripcion = producto.pr_descripcion_combo;
                }
            }

            if (ModelState.IsValid)
                List_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_proforma_det", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate_Fact([ModelBinder(typeof(DevExpressEditorsBinder))] fa_proforma_fact_det_Info info_det)
        {
            if (ModelState.IsValid) { }            
            List_det.UpdateRowFact(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            bus_proforma_det.modificarDB(List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q => q.Secuencia == info_det.Secuencia).FirstOrDefault());

            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_proforma_facturada_det", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            List_det.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_det.get_list( Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_proforma_det", model);
        }
        #endregion

    }
    public class fa_proforma_List
    {
        string Variable = "fa_proforma_Info";
        public List<fa_proforma_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_proforma_Info> list = new List<fa_proforma_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_proforma_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_proforma_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    public class fa_proforma_det_List
    {
        tb_sis_Impuesto_Bus bus_impuesto = new tb_sis_Impuesto_Bus();
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        string Variable = "fa_proforma_det_Info";
        public List<fa_proforma_det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_proforma_det_Info> list = new List<fa_proforma_det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_proforma_det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_proforma_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(fa_proforma_det_Info info_det, decimal IdTransaccionSession)
        {
            List<fa_proforma_det_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            info_det.IdProducto = info_det.IdProducto;
            info_det.pr_descripcion = info_det.pr_descripcion;
            info_det.pd_descuento_uni = info_det.pd_precio * (info_det.pd_por_descuento_uni / 100);
            info_det.pd_precio_final = info_det.pd_precio - info_det.pd_descuento_uni;
            info_det.pd_subtotal = info_det.pd_cantidad * info_det.pd_precio_final;

            var impuesto = bus_impuesto.get_info(info_det.IdCod_Impuesto);
            if (impuesto != null)
                info_det.pd_por_iva = impuesto.porcentaje;

            info_det.pd_iva = info_det.pd_subtotal * (info_det.pd_por_iva / 100);
            info_det.pd_total = info_det.pd_subtotal + info_det.pd_iva;

            info_det.pd_subtotal_item = info_det.pd_subtotal;
            info_det.pd_iva_item = info_det.pd_iva;
            info_det.pd_total_item = info_det.pd_total;

            #region Centro de costo
            info_det.IdCentroCosto = info_det.IdCentroCosto;
            if (string.IsNullOrEmpty(info_det.IdCentroCosto))
                info_det.cc_Descripcion = string.Empty;
            else
            {
                var cc = bus_cc.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), info_det.IdCentroCosto);
                if (cc != null)
                    info_det.cc_Descripcion = cc.cc_Descripcion;
            }
            #endregion

            list.Add(info_det);
        }

        public void UpdateRow(fa_proforma_det_Info info_det, decimal IdTransaccionSession)
        {
            fa_proforma_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.IdProducto = info_det.IdProducto;
            edited_info.pr_descripcion = info_det.pr_descripcion;
            edited_info.pd_cantidad = info_det.pd_cantidad;
            edited_info.pd_por_descuento_uni = info_det.pd_por_descuento_uni;
            edited_info.pd_precio = info_det.pd_precio;
            edited_info.pd_descuento_uni = info_det.pd_precio * (info_det.pd_por_descuento_uni / 100);
            edited_info.pd_precio_final = info_det.pd_precio - edited_info.pd_descuento_uni;
            edited_info.pd_subtotal = info_det.pd_cantidad * edited_info.pd_precio_final;
            edited_info.pd_DetalleAdicional = info_det.pd_DetalleAdicional;
            edited_info.IdCod_Impuesto = info_det.IdCod_Impuesto;
            if (!string.IsNullOrEmpty(info_det.IdCod_Impuesto))
            {
                var impuesto = bus_impuesto.get_info(info_det.IdCod_Impuesto);
                if (impuesto != null)
                    edited_info.pd_por_iva = impuesto.porcentaje;
            }
            edited_info.pd_iva = edited_info.pd_subtotal * (edited_info.pd_por_iva / 100);
            edited_info.pd_total = edited_info.pd_subtotal + edited_info.pd_iva;

            edited_info.NumCotizacion = info_det.NumCotizacion;
            edited_info.NumOPr = info_det.NumOPr;

            edited_info.pd_subtotal_item = info_det.pd_cantidad * edited_info.pd_precio_final;
            edited_info.pd_iva_item = edited_info.pd_subtotal * (edited_info.pd_por_iva / 100);
            edited_info.pd_total_item = edited_info.pd_subtotal + edited_info.pd_iva;

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
        }

        public void UpdateRowFact(fa_proforma_fact_det_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            fa_proforma_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            var producto = bus_producto.get_info(IdEmpresa, edited_info.IdProducto);
            if (producto != null)
            {
                edited_info.pr_descripcion = producto.pr_descripcion;
            }
           
            if (!string.IsNullOrEmpty(edited_info.IdCod_Impuesto))
            {
                var impuesto = bus_impuesto.get_info(edited_info.IdCod_Impuesto);
                if (impuesto != null)
                    edited_info.pd_por_iva = impuesto.porcentaje;
            }

            edited_info.NumCotizacion = info_det.NumCotizacion;
            edited_info.NumOPr = info_det.NumOPr;

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
        }
        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<fa_proforma_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).FirstOrDefault());
        }
    }
}