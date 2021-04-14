using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Web.Areas.Inventario.Controllers;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class FacturaPuntoVentaController : Controller
    {
        #region Variables
        in_Marca_Bus bus_marca = new in_Marca_Bus();
        in_Marca_List Lista_Marca = new in_Marca_List();
        in_ProductoListado_List Lista_Producto = new in_ProductoListado_List();
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        fa_TerminoPago_Bus bus_termino_pago = new fa_TerminoPago_Bus();
        fa_factura_det_List List_det = new fa_factura_det_List();
        tb_sis_Impuesto_Bus bus_impuesto = new tb_sis_Impuesto_Bus();
        fa_parametro_Bus bus_parametro = new fa_parametro_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        fa_catalogo_Bus bus_facatalogo = new fa_catalogo_Bus();
        string mensajeValidar = string.Empty;
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        fa_PuntoVta_Bus bus_punto_venta = new fa_PuntoVta_Bus();
        tb_sis_Documento_Tipo_Talonario_Bus bus_talonario = new tb_sis_Documento_Tipo_Talonario_Bus();
        fa_parametro_Bus bus_param = new fa_parametro_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
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

            var info_parametro = bus_parametro.get_info(Convert.ToInt32(SessionFixed.IdEmpresa));
            decimal ConsumidorFinal = info_parametro == null ? 0 : Convert.ToDecimal(info_parametro.IdClienteConsumidorFinal);
            var info_cliente = bus_cliente.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), ConsumidorFinal);
            fa_factura_Info model = new fa_factura_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = ConsumidorFinal,
                IdPersona = info_cliente.IdPersona,
                pe_apellido = info_cliente.info_persona.pe_apellido,
                pe_nombre = info_cliente.info_persona.pe_nombre,
                pe_razonSocial = info_cliente.info_persona.pe_razonSocial,
                pe_nombreCompleto = info_cliente.info_persona.pe_nombreCompleto,
                pe_cedulaRuc = info_cliente.info_persona.pe_cedulaRuc,
                pe_Naturaleza = info_cliente.info_persona.pe_Naturaleza,
                IdTipoDocumento = info_cliente.info_persona.IdTipoDocumento,
                pe_celular = info_cliente.Celular,
                pe_telfono_Contacto = info_cliente.Telefono,
                pe_correo = info_cliente.Correo,
                pe_direccion = info_cliente.Direccion,

                vt_fecha = DateTime.Now,
                vt_plazo = 1,
                vt_fech_venc = DateTime.Now.AddDays(1),
                vt_tipoDoc = "FACT",
                IdCatalogo_FormaPago = "EFEC",
                IdVendedor = 1,
                info_resumen = new fa_factura_resumen_Info(),
                lst_det = new List<fa_factura_det_Info>(),
                lst_cuota = new List<fa_cuotas_x_doc_Info>(),
                Subtotal = 0,
                SubtotalFactura = 0,
                Iva = 0,
                IvaFactura = 0,
                Total = 0,
                TotalFactura = 0,
        };
            cargar_combos(model);
            var lst = bus_marca.get_list(model.IdEmpresa, false);
            Lista_Marca.set_list(lst, model.IdTransaccionSession);
            Lista_Producto.set_list(new List<in_Producto_Info>(), model.IdTransaccionSession);

            var MostrarBoton = (ConsumidorFinal == model.IdCliente ? 0 : 1);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(fa_factura_Info model)
        {
            //if (!validar_factura(model, ref mensaje))
            //{
            //    List_det.set_list(List_det.get_list(model.IdTransaccionSession), model.IdTransaccionSession);
            //    ViewBag.mensaje = mensaje;
            //    cargar_combos(model);
            //    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            //    return View(model);
            //}
            //model.IdUsuario = SessionFixed.IdUsuario.ToString();
            //if (!bus_factura.guardarDB(model))
            //{
            //    List_det.set_list(List_det.get_list(model.IdTransaccionSession), model.IdTransaccionSession);
            //    ViewBag.mensaje = "No se ha podido guardar el registro";
            //    cargar_combos(model);
            //    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            //    return View(model);
            //};
            return RedirectToAction("Index");
        }
        #endregion

        #region Metodos
        private void cargar_combos(fa_factura_Info info)
        {
            var lst_formapago = bus_facatalogo.get_list((int)cl_enumeradores.eTipoCatalogoFact.FormaDePago, false);
            ViewBag.lst_formapago = lst_formapago;

            tb_Catalogo_Bus bus_catalogo = new tb_Catalogo_Bus();
            var lst_tipo_doc = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPODOC), false);
            ViewBag.lst_tipo_doc = lst_tipo_doc;
        }

        private bool validar(fa_cliente_Info i_validar, ref string msg)
        {
            string return_naturaleza = "";
            if (cl_funciones.ValidaIdentificacion(i_validar.info_persona.IdTipoDocumento, i_validar.info_persona.pe_Naturaleza, i_validar.info_persona.pe_cedulaRuc, ref return_naturaleza))
            {
                i_validar.info_persona.pe_Naturaleza = return_naturaleza;
            }
            else
            {
                msg = "Número de identificación inválida";
                return false;
            }

            if (i_validar.info_persona.IdTipoDocumento == "RUC")
            {
                if (string.IsNullOrEmpty(i_validar.info_persona.pe_razonSocial))
                {
                    msg = "Razon social inválida";
                    return false;
                }
                else
                {
                    i_validar.info_persona.pe_nombreCompleto = i_validar.info_persona.pe_razonSocial;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(i_validar.info_persona.pe_nombre) || string.IsNullOrEmpty(i_validar.info_persona.pe_apellido))
                {
                    msg = "Nombres o apellidos inválidos";
                    return false;
                }
                else
                {
                    i_validar.info_persona.pe_nombreCompleto = i_validar.info_persona.pe_apellido + " " + i_validar.info_persona.pe_nombre;
                }
            }

            if (string.IsNullOrEmpty(i_validar.Correo))
            {
                msg = "Correo inválido";
                return false;
            }
            if (string.IsNullOrEmpty(i_validar.Direccion))
            {
                msg = "Dirección inválida";
                return false;
            }
            return true;
        }
        private bool validar_factura(fa_factura_Info i_validar, ref string msg)
        {
            string MsgValidaciones = string.Empty;
            i_validar.PedirDesbloqueo = false;

            //if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.vt_fecha, cl_enumeradores.eModulo.FAC, i_validar.IdSucursal, ref msg))
            //{
            //    return false;
            //}

            //if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.vt_fecha, cl_enumeradores.eModulo.CONTA, i_validar.IdSucursal, ref msg))
            //{
            //    return false;
            //}

            i_validar.lst_det = List_det.get_list(i_validar.IdTransaccionSession);
            if (i_validar.lst_det.Count == 0)
            {
                msg = "No ha ingresado registros en el detalle de la factura";
                return false;
            }
            if (i_validar.lst_det.Where(q => q.vt_cantidad == 0).Count() > 0)
            {
                msg = "Existen registros con cantidad 0 en el detalle de la factura";
                return false;
            }
            if (i_validar.lst_det.Where(q => q.IdProducto == 0).Count() > 0)
            {
                msg = "Existen registros sin producto en el detalle de la factura";
                return false;
            }
            if (i_validar.lst_det.Sum(q => q.vt_total) == 0)
            {
                msg = "La factura no tiene valor, por favor revise";
                return false;
            }

            #region Talonario

            var pto_vta = bus_punto_venta.get_info(i_validar.IdEmpresa, i_validar.IdSucursal, Convert.ToInt32(i_validar.IdPuntoVta));
            if (pto_vta != null && pto_vta.EsElectronico)
            {
                var info_documento = bus_talonario.GetUltimoNoUsado(i_validar.IdEmpresa, cl_enumeradores.eTipoDocumento.FACT.ToString(), pto_vta.Su_CodigoEstablecimiento, pto_vta.cod_PuntoVta, pto_vta.EsElectronico, false);
                i_validar.vt_NumFactura = info_documento.NumDocumento;
            }
            i_validar.IdBodega = pto_vta.IdBodega;
            i_validar.vt_serie1 = pto_vta.Su_CodigoEstablecimiento;
            i_validar.vt_serie2 = pto_vta.cod_PuntoVta;
            i_validar.IdCaja = pto_vta.IdCaja;
            #endregion

            #region Validar cliente final
            var param = bus_param.get_info(i_validar.IdEmpresa);
            if (param != null && param.IdClienteConsumidorFinal != null && param.MontoMaximoConsumidorFinal > 0 && i_validar.IdCliente == param.IdClienteConsumidorFinal)
            {
                if (i_validar.info_resumen.Total > Convert.ToDecimal(param.MontoMaximoConsumidorFinal ?? 0))
                {
                    msg = "El límite de venta para consumidor final es de $ " + param.MontoMaximoConsumidorFinal.ToString() + ", por favor revise.";
                    return false;
                }
            }
            #endregion

            #region Resumen
            i_validar.info_resumen = new fa_factura_resumen_Info
            {
                SubtotalIVASinDscto = (decimal)Math.Round(i_validar.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero),
                SubtotalSinIVASinDscto = (decimal)Math.Round(i_validar.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero),

                Descuento = (decimal)Math.Round(i_validar.lst_det.Sum(q => q.vt_DescUnitario * q.vt_cantidad), 2, MidpointRounding.AwayFromZero),

                SubtotalIVAConDscto = (decimal)Math.Round(i_validar.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero),
                SubtotalSinIVAConDscto = (decimal)Math.Round(i_validar.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero),

                ValorIVA = (decimal)Math.Round(i_validar.lst_det.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero)
            };
            i_validar.info_resumen.SubtotalSinDscto = i_validar.info_resumen.SubtotalIVASinDscto + i_validar.info_resumen.SubtotalSinIVASinDscto;
            i_validar.info_resumen.SubtotalConDscto = i_validar.info_resumen.SubtotalIVAConDscto + i_validar.info_resumen.SubtotalSinIVAConDscto;
            i_validar.info_resumen.Total = i_validar.info_resumen.SubtotalConDscto + i_validar.info_resumen.ValorIVA;
            #endregion


            i_validar.IdUsuario = SessionFixed.IdUsuario;
            i_validar.IdUsuarioUltModi = SessionFixed.IdUsuario;

            #region ValidacionDeTalonario

            if (i_validar.IdCbteVta == 0 && !pto_vta.EsElectronico)
            {
                var talonario = bus_talonario.get_info(i_validar.IdEmpresa, i_validar.vt_tipoDoc, i_validar.vt_serie1, i_validar.vt_serie2, i_validar.vt_NumFactura);
                if (talonario == null)
                {
                    msg = "No existe un talonario creado con la numeración: " + i_validar.vt_serie1 + "-" + i_validar.vt_serie2 + "-" + i_validar.vt_NumFactura;
                    return false;
                }
                if (talonario.Usado == true)
                {
                    msg = "El talonario: " + i_validar.vt_serie1 + "-" + i_validar.vt_serie2 + "-" + i_validar.vt_NumFactura + " se encuentra utilizado.";
                    return false;
                }
                if (bus_factura.factura_existe(i_validar.IdEmpresa, i_validar.vt_serie1, i_validar.vt_serie2, i_validar.vt_NumFactura))
                {
                    msg = "Existe una factura registrada con el número: " + i_validar.vt_serie1 + "-" + i_validar.vt_serie2 + "-" + i_validar.vt_NumFactura + ".";
                    return false;
                }
            }

            #endregion

            #region ValidarStock
            if (i_validar.lst_det.Where(q => q.tp_manejaInven == "S").Count() > 0)
            {
                if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.vt_fecha, cl_enumeradores.eModulo.INV, i_validar.IdSucursal, ref msg))
                {
                    return false;
                }
                var lst_validar = i_validar.lst_det.GroupBy(q => new { q.IdProducto, q.pr_descripcion, q.tp_manejaInven, q.se_distribuye }).Select(q => new in_Producto_Stock_Info
                {
                    IdEmpresa = i_validar.IdEmpresa,
                    IdSucursal = i_validar.IdSucursal,
                    IdBodega = i_validar.IdBodega,
                    IdProducto = q.Key.IdProducto,
                    pr_descripcion = q.Key.pr_descripcion,
                    tp_manejaInven = q.Key.tp_manejaInven,
                    SeDestribuye = q.Key.se_distribuye ?? false,

                    Cantidad = q.Sum(v => v.vt_cantidad),
                    CantidadAnterior = q.Sum(v => v.CantidadAnterior),
                }).ToList();

                if (!bus_producto.validar_stock(lst_validar, ref msg))
                {
                    return false;
                }
            }
            #endregion

            #region ValidarCentroCosto
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ct_parametro_Bus bus_parametro = new ct_parametro_Bus();
            var info_ct_parametro = bus_parametro.get_info(IdEmpresa);

            if (i_validar.lst_det.Count > 0)
            {
                if (info_ct_parametro.EsCentroCostoObligatorio == true)
                {
                    foreach (var item in i_validar.lst_det)
                    {
                        if (item.IdCentroCosto == "" || item.IdCentroCosto == null)
                        {
                            mensaje = "Debe seleccionar el centro de costo para los items del detalle";
                            return false;
                        }
                    }
                }
            }
            #endregion

            return true;
        }
        #endregion

        #region JSON
        public JsonResult VerProductos(string SecuencialID = "", decimal IdTransaccionSession=0)
        {
            int IdEmpresa = Convert.ToInt32(SecuencialID.Substring(0, 4));
            int IdMarca = Convert.ToInt32(SecuencialID.Substring(4, 4));

            var resultado = bus_producto.get_list_x_marca(IdEmpresa, IdMarca, false);
            Lista_Producto.set_list(resultado, IdTransaccionSession);
            return Json(resultado.Count(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AgregarPedido(string SecuencialID = "", decimal IdTransaccionSession=0)
        {
            int IdEmpresa = Convert.ToInt32(SecuencialID.Substring(0, 4));
            int IdProducto = Convert.ToInt32(SecuencialID.Substring(4, 6));

            var producto = bus_producto.get_info(IdEmpresa, IdProducto);
            
            double subtotal = 0;
            double iva_porc = 0;
            double iva = 0;
            double total = 0;
            double Subtotal_Detalle = 0;
            double Iva_Detalle = 0;
            double Total_Detalle = 0;

            var impuesto = bus_impuesto.get_info(producto.IdCod_Impuesto_Iva);
            if (impuesto != null)
                iva_porc = impuesto.porcentaje;

            subtotal = 1 * producto.precio_1;
            iva = Math.Round((subtotal * (iva_porc / 100)), 2);
            total = Math.Round((subtotal + iva), 2);

            var lst_actual = List_det.get_list(IdTransaccionSession);
            var info_det = new fa_factura_det_Info
            {
                IdEmpresa = IdEmpresa,
                Secuencia = lst_actual.Count + 1,
                IdProducto = producto.IdProducto,
                pr_descripcion = producto.pr_descripcion,
                vt_cantidad = 1,
                vt_PorDescUnitario = 0,
                vt_Precio = producto.precio_1,
                vt_DescUnitario = 0,
                vt_PrecioFinal = producto.precio_1 - 0,
                vt_Subtotal = subtotal,
                tp_manejaInven = producto.tp_ManejaInven,
                se_distribuye = producto.se_distribuye,
                vt_detallexItems = "",
                IdCod_Impuesto_Iva = producto.IdCod_Impuesto_Iva,
                //vt_Subtotal_item = 0,
                //vt_iva_item=0,
                //vt_total_item=0,
                vt_iva = iva,
                vt_total = total,
                vt_por_iva = iva_porc
            };
            
            var existe_producto = lst_actual.Where(q=>q.IdEmpresa==IdEmpresa && q.IdProducto==IdProducto).Count();
            if (existe_producto==0)
            {                
                lst_actual.Add(info_det);
                List_det.set_list(lst_actual, IdTransaccionSession);
            }

            Subtotal_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
            Iva_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero);
            Total_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_total), 2, MidpointRounding.AwayFromZero);
            var x = Subtotal_Detalle.ToString("C2");
            return Json(new { subtotal= Subtotal_Detalle.ToString("C2"), iva= Iva_Detalle.ToString("C2"), total= Total_Detalle.ToString("C2") }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SetearCantidad(int SecuencialID = 0, decimal IdTransaccionSession = 0)
        {
            var lst_actual = List_det.get_list(IdTransaccionSession).ToList();
            var editar = lst_actual.Where(q => q.Secuencia == SecuencialID).FirstOrDefault();
            var resultado = editar==null ? 0 : editar.vt_cantidad;

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ModificarCantidad(int SecuenciaModificar = 0, double Cantidad=0, decimal IdTransaccionSession = 0)
        {
            var lst_actual = List_det.get_list(IdTransaccionSession).ToList();
            var editar = lst_actual.Where(q => q.Secuencia == SecuenciaModificar).FirstOrDefault();
            var impuesto = bus_impuesto.get_info(editar.IdCod_Impuesto_Iva);
            double subtotal = 0;
            double iva_porc = 0;
            double iva = 0;
            double total = 0;
            double Subtotal_Detalle = 0;
            double Iva_Detalle = 0;
            double Total_Detalle = 0;

            if (impuesto != null)
                iva_porc = impuesto.porcentaje;

            subtotal = editar.vt_Precio * Cantidad;
            iva = Math.Round((subtotal * (iva_porc / 100)), 2);
            total = Math.Round((subtotal + iva), 2);

            lst_actual.Where(q => q.Secuencia == SecuenciaModificar).FirstOrDefault().vt_cantidad = Cantidad;
            lst_actual.Where(q => q.Secuencia == SecuenciaModificar).FirstOrDefault().vt_Subtotal = subtotal;
            lst_actual.Where(q => q.Secuencia == SecuenciaModificar).FirstOrDefault().vt_iva = iva;
            lst_actual.Where(q => q.Secuencia == SecuenciaModificar).FirstOrDefault().vt_total = total;

            Subtotal_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
            Iva_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero);
            Total_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_total), 2, MidpointRounding.AwayFromZero);

            List_det.set_list(lst_actual, IdTransaccionSession);
            return Json(new { subtotal = Subtotal_Detalle.ToString("C2"), iva = Iva_Detalle.ToString("C2"), total = Total_Detalle.ToString("C2") }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EliminarProducto(int SecuencialID = 0, decimal IdTransaccionSession = 0)
        {
            double Subtotal_Detalle = 0;
            double Iva_Detalle = 0;
            double Total_Detalle = 0;
            var lst_actual = List_det.get_list(IdTransaccionSession).ToList();
            lst_actual.Remove(lst_actual.Where(q => q.Secuencia == SecuencialID).FirstOrDefault());
            List_det.set_list(lst_actual, IdTransaccionSession);
            Subtotal_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
            Iva_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero);
            Total_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_total), 2, MidpointRounding.AwayFromZero);

            return Json(new { subtotal = Subtotal_Detalle.ToString("C2"), iva = Iva_Detalle.ToString("C2"), total = Total_Detalle.ToString("C2") }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_info_x_num_cedula(int IdEmpresa = 0, string pe_cedulaRuc = "")
        {
            var resultado = bus_cliente.get_info_x_num_cedula(IdEmpresa, pe_cedulaRuc);
            var info_parametro = bus_parametro.get_info(IdEmpresa);
            decimal ConsumidorFinal = info_parametro == null ? 0 : Convert.ToDecimal(info_parametro.IdClienteConsumidorFinal);
            var MostrarBoton = (ConsumidorFinal == resultado.IdCliente ? 0 : 1);

            
            return Json(new { info = resultado, boton = MostrarBoton }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConsumidorFinal(int IdEmpresa = 0)
        {
            var info_parametro = bus_parametro.get_info(IdEmpresa);
            decimal IdCliente = info_parametro == null ? 0 : Convert.ToDecimal(info_parametro.IdClienteConsumidorFinal);
            var info_cliente = bus_cliente.get_info(IdEmpresa, IdCliente);
            if (info_cliente == null)
                info_cliente = new fa_cliente_Info();

            info_cliente.MostrarBoton = 0;

            return Json(info_cliente, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GuardarCliente(int IdEmpresa = 0, decimal IdCliente = 0, int IdPersona = 0, string pe_cedulaRuc = "", string pe_Naturaleza = "", string IdTipoDocumento = "", string pe_nombreCompleto = "", string pe_razonSocial = "", string pe_apellido = "", string pe_nombre = "", string pe_direccion = "", string pe_telfono_Contacto = "", string pe_celular = "", string pe_correo = "", decimal IdTransaccionSession=0)
        {
            var mensaje = string.Empty;
            var pe_NaturalezaCliente = string.Empty;
            

            var info_cliente = new fa_cliente_Info
            {
                IdEmpresa = IdEmpresa,
                IdCliente = IdCliente,
                cl_Cupo = 0,
                cl_plazo = 0,
                Codigo = IdCliente.ToString(),
                Estado = "A",
                es_empresa_relacionada = false,
                FormaPago = "01",
                IdCtaCble_cxc_Credito = null,
                IdCtaCble_Anticipo = null,
                IdPersona = IdPersona,
                IdTipoCredito = "CON",
                Idtipo_cliente = 1,
                IdNivel = 1,
                EsClienteExportador = false,
                IdUsuario = SessionFixed.IdUsuario,
                Fecha_Transac = DateTime.Now,
                IdCiudad = "09",
                IdParroquia = "09",
                Celular = pe_celular,
                Correo = pe_correo,
                Direccion = pe_direccion,
                Telefono = pe_telfono_Contacto,
                info_persona = new Info.General.tb_persona_Info
                {
                    IdPersona =IdPersona,
                    pe_nombre = pe_nombre,
                    pe_apellido = pe_apellido,
                    pe_nombreCompleto = pe_nombreCompleto,
                    pe_cedulaRuc = pe_cedulaRuc,
                    pe_Naturaleza = pe_NaturalezaCliente,
                    IdTipoDocumento = IdTipoDocumento,
                    pe_razonSocial = pe_razonSocial,

                    //Campos opcionales
                    pe_direccion = pe_direccion,
                    pe_telfono_Contacto = pe_telfono_Contacto,
                    pe_celular = pe_celular,
                    pe_correo = pe_correo,
                },
                lst_fa_cliente_contactos = new List<fa_cliente_contactos_Info>(),
                Lst_fa_cliente_x_fa_Vendedor_x_sucursal = new List<fa_cliente_x_fa_Vendedor_x_sucursal_Info>()
            };

            var info_contacto = new fa_cliente_contactos_Info
            {
                IdEmpresa = IdEmpresa,
                IdCliente = IdCliente,
                IdContacto = 1,
                IdCiudad = "09",
                IdParroquia = "09",
                Celular = pe_celular,
                Correo = pe_correo,
                Direccion = pe_direccion,
                Nombres = pe_nombreCompleto,
                Telefono = pe_telfono_Contacto
            };
            info_cliente.lst_fa_cliente_contactos.Add(info_contacto);

            var info_vendedor = new fa_cliente_x_fa_Vendedor_x_sucursal_Info
            {
                IdEmpresa = IdEmpresa,
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = IdCliente,
                IdVendedor = 1,
                observacion = null
            };

            var info_parametro = bus_parametro.get_info(IdEmpresa);
            decimal ConsumidorFinal = info_parametro == null ? 0 : Convert.ToDecimal(info_parametro.IdClienteConsumidorFinal);
            var resultado = false;

            if (!validar(info_cliente, ref mensajeValidar))
            {
                mensaje = mensajeValidar;
                Lista_Marca.set_list(Lista_Marca.get_list(IdTransaccionSession), IdTransaccionSession);
                SessionFixed.IdTransaccionSessionActual = IdTransaccionSession.ToString();
                resultado = false;
            }
            else
            {
                if (IdCliente != ConsumidorFinal && IdCliente == 0)
                {
                    resultado = bus_cliente.guardarClientePV(info_cliente);
                }
                else if (IdCliente != ConsumidorFinal && IdCliente != 0)
                {
                    resultado = bus_cliente.modificarClientePV(info_cliente);
                }
            }

            var MostrarBoton = (ConsumidorFinal == info_cliente.IdCliente ? 0 : 1);
            if (resultado == true)
            {
                mensaje = "Registro guardardo correctamente";
            }
            //else
            //{
            //    mensaje = "No se pudo guardar el registro";
            //}

            return Json(new { mensaje = mensaje, boton = MostrarBoton }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResumenFactura(decimal IdTransaccionSession = 0)
        {
            var lst_actual = List_det.get_list(IdTransaccionSession);
            double Subtotal_Detalle = 0;
            double Iva_Detalle = 0;
            double Total_Detalle = 0;

            List_det.set_list(lst_actual, IdTransaccionSession);
            Subtotal_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
            Iva_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero);
            Total_Detalle = (double)Math.Round(lst_actual.Sum(q => q.vt_total), 2, MidpointRounding.AwayFromZero);

            return Json(new { subtotal = Subtotal_Detalle.ToString("C2"), iva = Iva_Detalle.ToString("C2"), total = Total_Detalle.ToString("C2") }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CalcularCambio(double Recibido=0, decimal IdTransaccionSession = 0)
        {
            var lst_actual = List_det.get_list(IdTransaccionSession);
            double Total_Detalle = lst_actual.Sum(q=>q.vt_total);
            double Cambio = Recibido - Total_Detalle;
            

            return Json(Cambio.ToString("C2") , JsonRequestBehavior.AllowGet);
        }
        #endregion

        [ValidateInput(false)]
        public ActionResult GridViewPartial_Familia()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Marca.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_Familia", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_Producto()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Producto.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_Producto", model);
        }

        #region Pedido
        [ValidateInput(false)]
        public ActionResult GridViewPartial_Pedido()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Pedido", model);
        }
        #endregion
        #region Finalizar Pedido
        [ValidateInput(false)]
        public ActionResult GridViewPartial_FinalizarPedido()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_FinalizarPedido", model);
        }
        #endregion
    }
}