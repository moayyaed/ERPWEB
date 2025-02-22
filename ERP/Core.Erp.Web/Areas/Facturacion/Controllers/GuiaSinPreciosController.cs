﻿using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Info.Inventario;
using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class GuiaSinPreciosController : Controller
    {
        #region variables
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        fa_guia_remision_Bus bus_guia = new fa_guia_remision_Bus();
        fa_guia_remision_det_Bus bus_detalle = new fa_guia_remision_det_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        fa_PuntoVta_Bus bus_punto_venta = new fa_PuntoVta_Bus();
        tb_transportista_Bus bus_transportista = new tb_transportista_Bus();
        tb_sis_Documento_Tipo_Talonario_Bus bus_talonario = new tb_sis_Documento_Tipo_Talonario_Bus();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        fa_guia_remision_det_Info_lst detalle_info = new fa_guia_remision_det_Info_lst();
        fa_cliente_contactos_Bus bus_contacto = new fa_cliente_contactos_Bus();
        fa_factura_x_fa_guia_remision_Bus bus_detalle_x_factura = new fa_factura_x_fa_guia_remision_Bus();
        fa_catalogo_Bus bus_catalogo = new fa_catalogo_Bus();
        fa_factura_x_fa_guia_remision_Info_List List_rel = new fa_factura_x_fa_guia_remision_Info_List();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        tb_sis_Impuesto_Bus bus_impuesto = new tb_sis_Impuesto_Bus();
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        fa_MotivoTraslado_Bus bus_traslado = new fa_MotivoTraslado_Bus();
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        fa_Vendedor_Bus bus_vendedor = new fa_Vendedor_Bus();
        fa_TerminoPago_Bus bus_termino_pago = new fa_TerminoPago_Bus();
        fa_cliente_contactos_Bus bus_cliente_contactos = new fa_cliente_contactos_Bus();
        fa_guia_remision_SinPrecio_List Lista_GuiaSinPrecio = new fa_guia_remision_SinPrecio_List();
        string mensaje = string.Empty;
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion

        #region Metodos ComboBox bajo demanda cliente
        public ActionResult CmbCliente_Guia()
        {
            decimal model = new decimal();
            return PartialView("_CmbCliente_Guia", model);
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

        public ActionResult CmbProducto_Guia()
        {
            decimal model = new decimal();
            return PartialView("_CmbProducto_Guia", model);
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

        public ActionResult CmbCentroCosto_Guia()
        {
            string model = string.Empty;
            return PartialView("_CmbCentroCosto_Guia", model);
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

        #region vistas

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "GuiaSinPrecios", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info()
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };

            var lst = bus_guia.get_list(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            Lista_GuiaSinPrecio.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "GuiaSinPrecios", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_guia.get_list(model.IdEmpresa, model.fecha_ini, model.fecha_fin);
            Lista_GuiaSinPrecio.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }


        public ActionResult GridViewPartial_guias_remision(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            //ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);
            //if (IdSucursal == 0)
            //    IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            //ViewBag.IdSucursal = IdSucursal;
            //List<fa_guia_remision_Info> model = new List<fa_guia_remision_Info>();
            //model = bus_guia.get_list(IdEmpresa, ViewBag.Fecha_ini, ViewBag.Fecha_fin);

            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_GuiaSinPrecio.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_guias_remision", model);
        }
        public ActionResult GridViewPartial_guias_remision_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            SessionFixed.IdEntidad = !string.IsNullOrEmpty(Request.Params["IdCliente"]) ? Request.Params["IdCliente"].ToString() : "-1";
            var model = detalle_info.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_guias_remision_det", model);
        }

        public ActionResult GridViewPartial_FacturasSinGuia()
        {
            List<fa_factura_Info> model = new List<fa_factura_Info>();
            model = Session["fa_factura_Info"] as List<fa_factura_Info>;
            return PartialView("_GridViewPartial_FacturasSinGuia", model);
        }

        public ActionResult GridViewPartial_Facturas_x_guia()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_rel.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_Facturas_x_guia", model);
        }
        private bool validar(fa_guia_remision_Info i_validar, ref string msg)
        {
            i_validar.IdUsuarioCreacion = SessionFixed.IdUsuario;
            i_validar.IdUsuarioModificacion = SessionFixed.IdUsuario;
            //if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.gi_fecha, cl_enumeradores.eModulo.FAC, i_validar.IdSucursal, ref msg))
            //{
            //    return false;
            //}

            return true;
        }
        #endregion

        #region Metodos
        private void cargar_combos(fa_guia_remision_Info model)
        {
            var lst_sucursal = bus_sucursal.get_list(model.IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_punto_venta = bus_punto_venta.get_list_x_tipo_doc(model.IdEmpresa, model.IdSucursal, cl_enumeradores.eTipoDocumento.GUIA.ToString());
            ViewBag.lst_punto_venta = lst_punto_venta;

            var lst_transportista = bus_transportista.get_list(model.IdEmpresa, false);
            ViewBag.lst_transportista = lst_transportista;

            var lst_contacto = bus_contacto.get_list(model.IdEmpresa, model.IdCliente);
            ViewBag.lst_contacto = lst_contacto;

            var lst_tipo_traslado = bus_traslado.get_list(model.IdEmpresa, false);
            ViewBag.lst_tipo_traslado = lst_tipo_traslado;

            var lst_punto_venta_factura = bus_punto_venta.get_list_x_tipo_doc(model.IdEmpresa, model.IdSucursal, cl_enumeradores.eTipoDocumento.FACT.ToString());
            ViewBag.lst_punto_venta_factura = lst_punto_venta_factura;

            var lst_vendedor = bus_vendedor.get_list(model.IdEmpresa, false);
            ViewBag.lst_vendedor = lst_vendedor;

            var lst_pago = bus_termino_pago.get_list(false);
            ViewBag.lst_pago = lst_pago;

            var lst_formapago = bus_catalogo.get_list((int)cl_enumeradores.eTipoCatalogoFact.FormaDePago, false);
            ViewBag.lst_formapago = lst_formapago;

            var lst_cliente_contactos = bus_cliente_contactos.get_list(model.IdEmpresa, model.IdCliente);
            ViewBag.lst_cliente_contactos = lst_cliente_contactos;
        }

        private void cargar_combos_detalle()
        {
            var lst_impuesto = bus_impuesto.get_list("IVA", false);
            ViewBag.lst_impuesto = lst_impuesto;
        }

        #endregion

        #region acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "GuiaSinPrecios", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_guia_remision_Info model = new fa_guia_remision_Info
            {

                gi_fecha = DateTime.Now,
                gi_FechaFinTraslado = DateTime.Now,
                gi_FechaInicioTraslado = DateTime.Now,
                IdEmpresa = IdEmpresa,
                IdSucursal = IdSucursal,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                lst_detalle = new List<fa_guia_remision_det_Info>(),
                lst_detalle_x_factura = new List<fa_factura_x_fa_guia_remision_Info>(),
                GenerarFactura = false
            };
            detalle_info.set_list(model.lst_detalle, model.IdTransaccionSession);
            List_rel.set_list(model.lst_detalle_x_factura, model.IdTransaccionSession);
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(fa_guia_remision_Info model)
        {
            try
            {
                fa_PuntoVta_Info info_puntovta = new fa_PuntoVta_Info();
                info_puntovta = bus_punto_venta.get_info(model.IdEmpresa, model.IdSucursal, model.IdPuntoVta);

                model.IdBodega = info_puntovta.IdBodega;
                model.IdUsuarioCreacion = SessionFixed.IdUsuario;
                model.CodGuiaRemision = (model.CodGuiaRemision == null) ? "" : model.CodGuiaRemision;
                model.lst_detalle_x_factura = List_rel.get_list(model.IdTransaccionSession);
                model.lst_detalle = detalle_info.get_list(model.IdTransaccionSession);
                model.CodDocumentoTipo = cl_enumeradores.eTipoDocumento.GUIA.ToString();

                string mensaje = bus_guia.validar(model);
                if (mensaje != "")
                {
                    cargar_combos(model);
                    ViewBag.mensaje = mensaje;
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    return View(model);
                }
                if (!validar(model, ref mensaje))
                {
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    cargar_combos(model);
                    ViewBag.mensaje = mensaje;
                    return View(model);
                }
                if (!bus_guia.guardarDB(model))
                {
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    cargar_combos(model);
                    return View(model);
                }

                return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdGuiaRemision = model.IdGuiaRemision, Exito = true });
            }
            catch (Exception ex)
            {
                //SisLogError.set_list((ex.InnerException) == null ? ex.Message.ToString() : ex.InnerException.ToString());

                ViewBag.error = ex.Message.ToString();
                cargar_combos(model);
                return View(model);
            }
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdGuiaRemision = 0, bool Exito = false)
        {
            bus_guia = new fa_guia_remision_Bus();
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            fa_guia_remision_Info model = bus_guia.get_info(IdEmpresa, IdGuiaRemision);
            model.GenerarFactura = false;

            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_detalle = bus_detalle.get_list(IdEmpresa, IdGuiaRemision);
            detalle_info.set_list(model.lst_detalle, model.IdTransaccionSession);
            List_rel.set_list(bus_detalle_x_factura.get_list(IdEmpresa, IdGuiaRemision), model.IdTransaccionSession);
            cargar_combos(model);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            #region Validacion Periodo

            ViewBag.MostrarBoton = true;
            /*if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.gi_fecha, cl_enumeradores.eModulo.FAC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }*/
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "GuiaSinPrecios", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            else
            {
                if ((model.IdCbteVta != null || model.IdCbteVta != 0) && !string.IsNullOrEmpty(model.NUAutorizacion))
                {
                    info.Modificar = false;
                }
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal IdGuiaRemision = 0, bool Exito = false)
        {
            bus_guia = new fa_guia_remision_Bus();
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            fa_guia_remision_Info model = bus_guia.get_info(IdEmpresa, IdGuiaRemision);
            model.GenerarFactura = false;

            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_detalle = bus_detalle.get_list(IdEmpresa, IdGuiaRemision);
            detalle_info.set_list(model.lst_detalle, model.IdTransaccionSession);
            List_rel.set_list(bus_detalle_x_factura.get_list(IdEmpresa, IdGuiaRemision), model.IdTransaccionSession);
            cargar_combos(model);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            #region Validacion Periodo
            
            ViewBag.MostrarBoton = true;
            /*if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.gi_fecha, cl_enumeradores.eModulo.FAC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }*/
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "GuiaSinPrecios", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(fa_guia_remision_Info model)
        {
            try
            {
                model.IdUsuarioModificacion = SessionFixed.IdUsuario.ToString();
                model.CodGuiaRemision = (model.CodGuiaRemision == null) ? "" : model.CodGuiaRemision;
                model.CodDocumentoTipo = "GUIA";
                model.lst_detalle_x_factura = List_rel.get_list(model.IdTransaccionSession);
                model.lst_detalle = detalle_info.get_list(model.IdTransaccionSession);
                string mensaje = bus_guia.validar(model);
                if (!validar(model, ref mensaje))
                {
                    cargar_combos(model);
                    ViewBag.mensaje = mensaje;
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    return View(model);
                }
                if (mensaje != "")
                {
                    cargar_combos(model);
                    ViewBag.mensaje = mensaje;
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    return View(model);
                }
                if (!bus_guia.modificarDB(model))
                {
                    cargar_combos(model);
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    return View(model);
                }

                MensajeSuccess = "Registro actualizado exitósamente";
                return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdGuiaRemision = model.IdGuiaRemision, Exito = true });
            }
            catch (Exception ex)
            {
                //SisLogError.set_list((ex.InnerException) == null ? ex.Message.ToString() : ex.InnerException.ToString());
                ViewBag.error = ex.Message.ToString();
                cargar_combos(model);
                return View(model);
            }

        }
        public ActionResult Anular(int IdEmpresa = 0, decimal IdGuiaRemision = 0)
        {
            bus_guia = new fa_guia_remision_Bus();
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            fa_guia_remision_Info model = bus_guia.get_info(IdEmpresa, IdGuiaRemision);
            model.GenerarFactura = false;
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            detalle_info.set_list(bus_detalle.get_list(IdEmpresa, IdGuiaRemision), model.IdTransaccionSession);
            List_rel.set_list(bus_detalle_x_factura.get_list(IdEmpresa, IdGuiaRemision), model.IdTransaccionSession);
            cargar_combos(model);
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            /*if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.gi_fecha, cl_enumeradores.eModulo.FAC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }*/
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "GuiaSinPrecios", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_guia_remision_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            try
            {
                if (!bus_guia.anularDB(model))
                {
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    cargar_combos(model);
                    return View(model);
                }
                Session["fa_guia_remision_det_Info"] = null;
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                //SisLogError.set_list((ex.InnerException) == null ? ex.Message.ToString() : ex.InnerException.ToString());
                ViewBag.error = ex.Message.ToString();
                cargar_combos(model);
                return View(model);
            }
        }
        #endregion

        #region Json
        //public JsonResult GetInfoProducto(int IdEmpresa = 0, int IdProducto = 0)
        //{
        //    in_Producto_Bus bus_producto = new in_Producto_Bus();
        //    var resultado = bus_producto.get_info(IdEmpresa, IdProducto);

        //    return Json(resultado, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult CargarPuntosDeVenta(int IdSucursal = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var resultado = bus_punto_venta.get_list_x_tipo_doc(IdEmpresa, IdSucursal, cl_enumeradores.eTipoDocumento.GUIA.ToString());
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUltimoDocumento(int IdSucursal = 0, int IdPuntoVta = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            tb_sis_Documento_Tipo_Talonario_Info resultado = new tb_sis_Documento_Tipo_Talonario_Info();
            var punto_venta = bus_punto_venta.get_info(IdEmpresa, IdSucursal, IdPuntoVta);
            if (punto_venta != null)
            {
                var sucursal = bus_sucursal.get_info(IdEmpresa, IdSucursal);
                resultado = bus_talonario.GetUltimoNoUsado(IdEmpresa, cl_enumeradores.eTipoDocumento.GUIA.ToString(), sucursal.Su_CodigoEstablecimiento, punto_venta.cod_PuntoVta, punto_venta.EsElectronico, false);
            }

            if (resultado == null)
                resultado = new tb_sis_Documento_Tipo_Talonario_Info();

            return Json(new { data_puntovta = punto_venta, data_talonario = resultado }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MostrarBotonesSRI(int IdSucursal = 0, int IdPuntoVta = 0)
        {
            var resultado = 0;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var punto_venta = bus_punto_venta.get_info(IdEmpresa, IdSucursal, IdPuntoVta);

            resultado = (punto_venta.EsElectronico == true) ? 1 : 0;

            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Cargar_facturas(decimal IdCliente = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_facturas_sin_guias = bus_factura.get_list_fac_sin_guia(IdEmpresa, IdCliente);
            Session["fa_factura_Info"] = lst_facturas_sin_guias;
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult seleccionar_aprobacion(string Ids, int IdSucursal = 0, int IdPuntoVta = 0, decimal IdTransaccionSession = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            fa_PuntoVta_Info punto_venta = new fa_PuntoVta_Info();
            punto_venta = bus_punto_venta.get_info(IdEmpresa, IdSucursal, IdPuntoVta);

            if (Ids != null)
            {
                var facturas_x_seleccionar = Session["fa_factura_Info"] as List<fa_factura_Info>;

                string[] array = Ids.Split(',');
                var output = array.GroupBy(q => q).ToList();
                var lst_det = detalle_info.get_list(IdTransaccionSession);
                var lst_rel = List_rel.get_list(IdTransaccionSession);

                foreach (var item in output)
                {
                    if (item.Key != "")
                    {
                        if (lst_det.Where(q => q.IdCbteVta == Convert.ToDecimal(item.Key)).Count() == 0)
                        {
                            var lst_tmp = bus_detalle.get_list_x_factura(IdEmpresa, IdSucursal, punto_venta.IdBodega, Convert.ToDecimal(item.Key));
                            lst_det.AddRange(lst_tmp);
                        }

                        if (facturas_x_seleccionar.Where(q => q.IdCbteVta == Convert.ToDecimal(item.Key)).Count() > 0)
                        {

                            var factura = facturas_x_seleccionar.Where(q => q.IdCbteVta == Convert.ToDecimal(item.Key)).FirstOrDefault();
                            if (factura != null)
                            {
                                if (lst_rel.Where(q => q.IdCbteVta == Convert.ToDecimal(item.Key)).Count() == 0)
                                    lst_rel.Add(new fa_factura_x_fa_guia_remision_Info
                                    {
                                        IdEmpresa = factura.IdEmpresa,
                                        IdSucursal = factura.IdSucursal,
                                        IdBodega = factura.IdBodega,
                                        IdCbteVta = factura.IdCbteVta,
                                        vt_serie1 = factura.vt_serie1,
                                        vt_serie2 = factura.vt_serie2,
                                        vt_NumFactura = factura.vt_NumFactura,
                                        vt_tipoDoc = "FAC"
                                    });

                            }

                        }
                    }
                }
                List_rel.set_list(lst_rel, IdTransaccionSession);
                detalle_info.set_list(lst_det, IdTransaccionSession);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_direcciones(decimal IdCliente = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            fa_cliente_contactos_Info resultado = new fa_cliente_contactos_Info();

            fa_cliente_Info info_cliente = bus_cliente.get_info(IdEmpresa, IdCliente);

            if (info_cliente != null)
            {
                resultado = bus_contacto.get_info(IdEmpresa, IdCliente, info_cliente.IdContacto);
            }

            if (resultado != null)
                resultado.Direccion_emp = SessionFixed.em_direccion.ToString();
            else
                resultado = new fa_cliente_contactos_Info();

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_placa(int Idtransportista = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var resultado = bus_transportista.get_info(IdEmpresa, Idtransportista);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_direccion_origen(int IdSucursal = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var resultado = bus_sucursal.get_info(IdEmpresa, IdSucursal);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProformasPorFacturar(int IdSucursal = 0, decimal IdCliente = 0)
        {
            bool resultado = true;

            set_list_proformas(bus_detalle.get_list_proformas_x_guia(Convert.ToInt32(SessionFixed.IdEmpresa), IdSucursal, IdCliente));

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProformas(int IdSucursal = 0, decimal IdCliente = 0, decimal IdProforma = 0, decimal IdTransaccionSession = 0)
        {
            bool resultado = true;

            var list = bus_detalle.get_list_proforma(Convert.ToInt32(SessionFixed.IdEmpresa), IdSucursal, IdCliente, IdProforma);
            if (list.Count() == 0)
                resultado = false;
            var detalle_guia = detalle_info.get_list(IdTransaccionSession);
            if (detalle_guia.Where(v => v.IdProforma == IdProforma).Count() == 0)
            {
                int Secuencia = detalle_guia.Count == 0 ? 1 : detalle_guia.Max(q => q.Secuencia) + 1;
                list.ForEach(q => q.Secuencia = Secuencia++);
                detalle_guia.AddRange(list);
            }
            detalle_info.set_list(detalle_guia, IdTransaccionSession);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarPuntosDeVenta_Factura(int IdSucursal = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var resultado = bus_punto_venta.get_list_x_tipo_doc(IdEmpresa, IdSucursal, cl_enumeradores.eTipoDocumento.FACT.ToString());
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUltimoDocumento_Factura(int IdSucursal = 0, int IdPuntoVta = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            tb_sis_Documento_Tipo_Talonario_Info resultado = new tb_sis_Documento_Tipo_Talonario_Info();
            var punto_venta = bus_punto_venta.get_info(IdEmpresa, IdSucursal, IdPuntoVta);
            if (punto_venta != null)
            {
                var sucursal = bus_sucursal.get_info(IdEmpresa, IdSucursal);
                resultado = bus_talonario.GetUltimoNoUsado(IdEmpresa, cl_enumeradores.eTipoDocumento.FACT.ToString(), sucursal.Su_CodigoEstablecimiento, punto_venta.cod_PuntoVta, punto_venta.EsElectronico, false);
            }

            if (resultado == null)
                resultado = new tb_sis_Documento_Tipo_Talonario_Info();

            return Json(new { data_puntovta = punto_venta, data_talonario = resultado }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetCodigo(decimal IdTransaccionSession = 0)
        {
            string Codigo = "COT: ";
            var ListaDetalle = detalle_info.get_list(IdTransaccionSession);
            var lstCotizacion = ListaDetalle.GroupBy(q => q.NumCotizacion).Select(q => q.Key).ToList();
            var lstOpr = ListaDetalle.GroupBy(q => q.NumOPr).Select(q => q.Key).ToList();

            int Cont = 0;
            lstCotizacion.ForEach(q =>
            {
                Codigo += Cont == 0 ? q.ToString() : (" - " + q.ToString());
                Cont++;
            });
            Cont = 0;
            string OP = " OP: ";
            lstOpr.ForEach(q =>
            {
                OP += Cont == 0 ? q.ToString() : (" - " + q.ToString());
                Cont++;
            });
            Codigo += OP;

            return Json(Codigo, JsonRequestBehavior.AllowGet);
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
            subtotal = Convert.ToDouble(Cantidad * PrecioFinal);

            var impuesto = bus_impuesto.get_info(IdCodImpuesto);
            if (impuesto != null)
                iva_porc = impuesto.porcentaje;

            iva = subtotal * (iva_porc / 100);
            total = subtotal + iva;

            return Json(new { subtotal = subtotal, iva = iva, total = total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutorizarSRI(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdGuiaRemision)
        {
            string retorno = string.Empty;

            if (bus_guia.modificarEstadoAutorizacion(IdEmpresa, IdSucursal, IdBodega, IdGuiaRemision))
                retorno = "Autorización exitosa";


            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region funciones del detalle

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] fa_guia_remision_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            decimal IdCliente = Convert.ToDecimal(SessionFixed.IdEntidad);

            var producto = bus_producto.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), info_det.IdProducto);
            if (producto != null)
            {
                info_det.pr_descripcion = producto.pr_descripcion_combo;
                info_det.IdCod_Impuesto = producto.IdCod_Impuesto_Iva;
            }

            if (ModelState.IsValid)
                detalle_info.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            var model = detalle_info.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_guias_remision_det", model);
        }
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] fa_guia_remision_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            decimal IdCliente = Convert.ToDecimal(SessionFixed.IdEntidad);

            if (info_det != null)
                if (info_det.Secuencia != 0 && info_det.gi_cantidad != 0)
                {
                    var producto = bus_producto.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), info_det.IdProducto);
                    if (producto != null)
                    {
                        info_det.pr_descripcion = producto.pr_descripcion_combo;
                        info_det.IdCod_Impuesto = producto.IdCod_Impuesto_Iva;
                    }


                    if (ModelState.IsValid)
                        detalle_info.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                }

            var model = detalle_info.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_guias_remision_det", model);
        }
        public ActionResult EditingDelete(int Secuencia)
        {
            decimal IdComprobante = Convert.ToDecimal(detalle_info.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(v => v.Secuencia == Secuencia).FirstOrDefault().IdCbteVta);
            if (detalle_info.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(v => v.IdCbteVta == IdComprobante).Count() == 1)
            {
                var list_facturas_seleccionadas = List_rel.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                var Info = list_facturas_seleccionadas.Where(v => v.IdCbteVta == IdComprobante).FirstOrDefault();
                list_facturas_seleccionadas.Remove(Info);
                List_rel.set_list(list_facturas_seleccionadas, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            }
            detalle_info.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = detalle_info.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_guias_remision_det", model);
        }
        #endregion

        #region Detalle de proforma
        [ValidateInput(false)]
        public ActionResult GridViewPartial_PGuia_det()
        {
            var model = get_list_proformas();
            return PartialView("_GridViewPartial_PGuia_det", model);
        }
        public void AddProformas(string IDs = "", decimal IdTransaccionSession = 0)
        {
            if (!string.IsNullOrEmpty(IDs))
            {
                string[] array = IDs.Split(',');
                var lst = get_list_proformas();
                var lst_det_guia = detalle_info.get_list(IdTransaccionSession);
                foreach (var item in array)
                {
                    var pf = lst.Where(q => q.SecuencialUnico == item).FirstOrDefault();
                    if (pf != null)
                        if (lst_det_guia.Where(q => q.IdEmpresa == pf.IdEmpresa_pf && q.IdSucursal_pf == pf.IdSucursal_pf && q.IdProforma == pf.IdProforma && q.Secuencia_pf == pf.Secuencia_pf).Count() == 0)
                        {
                            pf.Secuencia = lst_det_guia.Count == 0 ? 1 : lst_det_guia.Max(q => q.Secuencia) + 1;
                            lst_det_guia.Add(pf);
                        }
                }
                detalle_info.set_list(lst_det_guia, IdTransaccionSession);
            }
        }
        public List<fa_guia_remision_det_Info> get_list_proformas()
        {
            if (Session["fa_guia_remision_det_Info"] == null)
            {
                List<fa_guia_remision_det_Info> list = new List<fa_guia_remision_det_Info>();

                Session["fa_guia_remision_det_Info"] = list;
            }
            return (List<fa_guia_remision_det_Info>)Session["fa_guia_remision_det_Info"];
        }

        public void set_list_proformas(List<fa_guia_remision_det_Info> list)
        {
            Session["fa_guia_remision_det_Info"] = list;
        }
        #endregion
    }

    public class fa_guia_remision_SinPrecio_List
    {
        string Variable = "fa_guia_remision_SinPrecio_Info";
        public List<fa_guia_remision_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_guia_remision_Info> list = new List<fa_guia_remision_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_guia_remision_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_guia_remision_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}