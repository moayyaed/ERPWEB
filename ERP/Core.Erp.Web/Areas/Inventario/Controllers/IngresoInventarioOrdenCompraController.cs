﻿using Core.Erp.Bus.Compras;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Compras;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    public class IngresoInventarioOrdenCompraController : Controller
    {
        #region variables
        in_Ing_Egr_Inven_Bus bus_ing_inv = new in_Ing_Egr_Inven_Bus();
        in_Ing_Egr_Inven_det_Bus bus_det_ing_inv = new in_Ing_Egr_Inven_det_Bus();
        in_Ing_Egr_Inven_det_OCList List_in_Ing_Egr_Inven_det = new in_Ing_Egr_Inven_det_OCList();
        com_ordencompra_local_det_List ListaPorIngresar = new com_ordencompra_local_det_List();
        in_parametro_Bus bus_in_param = new in_parametro_Bus();
        string mensaje = string.Empty;
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        in_UnidadMedida_Equiv_conversion_Bus bus_UnidadMedidaEquivalencia = new in_UnidadMedida_Equiv_conversion_Bus();
        tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
        com_parametro_Bus bus_com_param = new com_parametro_Bus();
        com_ordencompra_local_Bus bus_orden_compra = new com_ordencompra_local_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        in_producto_x_tb_bodega_Bus bus_producto_x_bodega = new in_producto_x_tb_bodega_Bus();
        in_IngresoOC_Inven_Lista Lista_IngresoOC = new in_IngresoOC_Inven_Lista();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        #endregion

        #region Metodos ComboBox bajo demanda
        public ActionResult CmbProducto_IngresoInventario()
        {
            decimal model = new decimal();
            return PartialView("_CmbProducto_IngresoInventario", model);
        }
        public List<in_Producto_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = string.IsNullOrEmpty(SessionFixed.IdSucursalInv) ? 0 : Convert.ToInt32(SessionFixed.IdSucursalInv);
            int IdBodega = string.IsNullOrEmpty(SessionFixed.IdBodegaInv) ? 0 : Convert.ToInt32(SessionFixed.IdBodegaInv);

            return bus_producto.get_list_bajo_demanda(args, IdEmpresa, cl_enumeradores.eTipoBusquedaProducto.PORBODEGA, cl_enumeradores.eModulo.INV, IdSucursal, IdBodega);
        }
        public in_Producto_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_producto.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos ComboBox bajo demanda proveedor
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbProveedor_IngresoOrdenCompra()
        {
            decimal model = new decimal();
            return PartialView("_CmbProveedor_IngresoOrdenCompra", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_proveedor(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.PROVEE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_proveedor(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.PROVEE.ToString());
        }
        #endregion

        #region Metodos ComboBox bajo demanda centro de costo

        public ActionResult CmbCentroCosto_Inv_Ing_OC()
        {
            string model = string.Empty;
            return PartialView("_CmbCentroCosto_Inv_Ing_OC", model);
        }
        public List<ct_CentroCosto_Info> get_list_bajo_demandaIngOC(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            List<ct_CentroCosto_Info> Lista = bus_cc.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
            return Lista;
        }
        public ct_CentroCosto_Info get_info_bajo_demandaIngOC(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_cc.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Vistas
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "IngresoInventarioOrdenCompra", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdBodega = 0,
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };
            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_ing_inv.get_list_orden_compra(model.IdEmpresa, model.IdSucursal, true, model.IdBodega, model.fecha_ini, model.fecha_fin);
            Lista_IngresoOC.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        private void CargarCombosConsulta(int IdEmpresa)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;

            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var lst_bodega = bus_bodega.get_list(IdEmpresa, true);
            ViewBag.lst_bodega = lst_bodega;
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "IngresoInventarioOrdenCompra", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion
            CargarCombosConsulta(model.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_ing_inv.get_list_orden_compra(model.IdEmpresa, model.IdSucursal, true, model.IdBodega, model.fecha_ini, model.fecha_fin);
            Lista_IngresoOC.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_IngresoOrdenCompra(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_IngresoOC.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            //com_parametro_Info info_parametro_compra = bus_com_param.get_info(IdEmpresa);
            //List<in_Ing_Egr_Inven_Info> model = bus_ing_inv.get_list_orden_compra(IdEmpresa, IdSucursal, true, IdBodega, ViewBag.fecha_ini, ViewBag.fecha_fin);
            return PartialView("_GridViewPartial_IngresoOrdenCompra", model);
        }
        #endregion

        #region Cargar Unidad de medida
        public ActionResult CargarUnidadMedida()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            decimal IdProducto = (Request.Params["in_IdProducto"] != null && Request.Params["in_IdProducto"] != "" && Convert.ToDecimal(Request.Params["in_IdProducto"]) != 0) ? Convert.ToDecimal(Request.Params["in_IdProducto"]) : 0;

            in_Producto_Info info_produto = bus_producto.get_info(IdEmpresa, IdProducto);
            string IdUnidadMedida = (info_produto == null) ? "" : info_produto.IdUnidadMedida_Consumo;

            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = "Descripcion";
                p.ValueField = "IdUnidadMedida_equiva";
                p.ValueType = typeof(string);
                p.BindList(bus_UnidadMedidaEquivalencia.get_list_combo(IdUnidadMedida));
            });

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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "IngresoInventarioOrdenCompra", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            in_parametro_Info i_param = bus_in_param.get_info(IdEmpresa);
            if (i_param == null)
                return RedirectToAction("Index");
            in_Ing_Egr_Inven_Info model = new in_Ing_Egr_Inven_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdEmpresa = IdEmpresa,
                cm_fecha = DateTime.Now,
                signo = "+",
                IdMovi_inven_tipo = i_param.P_IdMovi_inven_tipo_default_ing
            };
            List_in_Ing_Egr_Inven_det.set_list(new List<in_Ing_Egr_Inven_det_Info>(), model.IdTransaccionSession);
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(in_Ing_Egr_Inven_Info model)
        {
            com_parametro_Info info_param_oc = bus_com_param.get_info(model.IdEmpresa);
            model.lst_in_Ing_Egr_Inven_det = List_in_Ing_Egr_Inven_det.get_list(model.IdTransaccionSession);
            var IdOrdenCompra = (model.lst_in_Ing_Egr_Inven_det.Count() >0) ? model.lst_in_Ing_Egr_Inven_det.Where(q => q.IdEmpresa == model.IdEmpresa).FirstOrDefault().SecuenciaTipo : 0;
            model.CodMoviInven = Convert.ToString(IdOrdenCompra);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            //model.IdResponsable = model.IdProveedor;
            model.IdUsuario = SessionFixed.IdUsuario;
            model.signo = "+";
            model.IdMovi_inven_tipo = info_param_oc.IdMovi_inven_tipo_OC;
            model.lst_in_Ing_Egr_Inven_det.ForEach(q => q.IdBodega_inv = model.IdBodega);
            model.lst_in_Ing_Egr_Inven_det.ForEach(q => { q.IdEmpresa_oc = model.IdEmpresa; });
            if (!bus_ing_inv.guardarDB(model, "+"))
            {
                cargar_combos(model);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdMovi_inven_tipo = model.IdMovi_inven_tipo, IdNumMovi = model.IdNumMovi, Exito = true });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            in_Ing_Egr_Inven_Info model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            if (model == null)
                return RedirectToAction("Index");
            //model.IdProveedor = Convert.ToInt32(model.IdResponsable);
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "IngresoInventarioOrdenCompra", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            else
            {
                if (model.Estado == "A" && model.IdEstadoAproba== "APRO" && string.IsNullOrEmpty(model.co_factura) )
                {
                    info.Modificar = false;
                }
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion
            model.lst_in_Ing_Egr_Inven_det = bus_det_ing_inv.get_list(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_in_Ing_Egr_Inven_det.set_list(model.lst_in_Ing_Egr_Inven_det, model.IdTransaccionSession);
            cargar_combos(model);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cm_fecha, cl_enumeradores.eModulo.INV, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }

            if (model.IdEstadoAproba == "APRO" || model.IdEstadoAproba == "REP")
            {
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "IngresoInventarioOrdenCompra", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            in_Ing_Egr_Inven_Info model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            if (model == null)
                return RedirectToAction("Index");
            //model.IdProveedor = Convert.ToInt32(model.IdResponsable);
            model.lst_in_Ing_Egr_Inven_det = bus_det_ing_inv.get_list(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_in_Ing_Egr_Inven_det.set_list(model.lst_in_Ing_Egr_Inven_det, model.IdTransaccionSession);
            cargar_combos(model);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cm_fecha, cl_enumeradores.eModulo.INV, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }

            if (model.IdEstadoAproba =="APRO" || model.IdEstadoAproba == "REP")
            {
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(in_Ing_Egr_Inven_Info model)
        {
            model.lst_in_Ing_Egr_Inven_det = List_in_Ing_Egr_Inven_det.get_list(model.IdTransaccionSession);
            var IdOrdenCompra = (model.lst_in_Ing_Egr_Inven_det.Count() > 0) ? model.lst_in_Ing_Egr_Inven_det.Where(q => q.IdEmpresa == model.IdEmpresa).FirstOrDefault().SecuenciaTipo : 0;
            model.CodMoviInven = Convert.ToString(IdOrdenCompra);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuarioUltModi = SessionFixed.IdUsuario;
            //model.IdResponsable = model.IdProveedor;
            
            if (!bus_ing_inv.modificarDB(model))
            {
                cargar_combos(model);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdMovi_inven_tipo = model.IdMovi_inven_tipo, IdNumMovi = model.IdNumMovi, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0, int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "IngresoInventarioOrdenCompra", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            in_Ing_Egr_Inven_Info model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            if (model == null)
                return RedirectToAction("Index");
            model.lst_in_Ing_Egr_Inven_det = bus_det_ing_inv.get_list(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_in_Ing_Egr_Inven_det.set_list(model.lst_in_Ing_Egr_Inven_det, model.IdTransaccionSession);
            cargar_combos(model);
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cm_fecha, cl_enumeradores.eModulo.INV, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(in_Ing_Egr_Inven_Info model)
        {
            model.lst_in_Ing_Egr_Inven_det = List_in_Ing_Egr_Inven_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdusuarioUltAnu = SessionFixed.IdUsuario;

            if (!bus_ing_inv.anularDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Funciones del detalle
        [ValidateInput(false)]
        public ActionResult GridViewPartial_inv_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_inv_det", model);
        }
        private void cargar_combos_detalle()
        {
            in_UnidadMedida_Bus bus_unidad = new in_UnidadMedida_Bus();
            var lst_unidad = bus_unidad.get_list(false);
            ViewBag.lst_unidad = lst_unidad;
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult EditingAddNew(string IDs = "", decimal IdTransaccionSession = 0, int IdEmpresa = 0)
        {
            string ObservacionOC = string.Empty;
            if (IDs != "")
            {

                int IdEmpresaSesion = Convert.ToInt32(SessionFixed.IdEmpresa);
                var lst_x_ingresar = ListaPorIngresar.get_list(IdTransaccionSession);
                string[] array = IDs.Split(',');
                foreach (var item in array)
                {
                    int IdEmpresaOC = Convert.ToInt32(item.Substring(0,3));
                    int IdSucursalOC = Convert.ToInt32(item.Substring(3, 3));
                    int IdOrdenCompra = Convert.ToInt32(item.Substring(6, 6));
                    int Secuencia = Convert.ToInt32(item.Substring(12, 6));

                    var info_det = lst_x_ingresar.Where(q => q.IdEmpresa == IdEmpresaOC && q.IdSucursal == IdSucursalOC && q.IdOrdenCompra == IdOrdenCompra && q.Secuencia == Secuencia).FirstOrDefault();

                    in_Ing_Egr_Inven_det_Info info_det_inv;
                    com_parametro_Info info_param_oc = bus_com_param.get_info(IdEmpresaSesion);
                    if (info_det != null)
                    {
                        info_det_inv = new in_Ing_Egr_Inven_det_Info
                        {
                            IdEmpresa = info_det.IdEmpresa,
                            IdSucursal = info_det.IdSucursal,
                            IdOrdenCompra = info_det.IdOrdenCompra,
                            IdProducto = info_det.IdProducto,
                            pr_descripcion = info_det.pr_descripcion,

                            mv_costo_sinConversion = info_det.do_precioFinal,
                            dm_cantidad_sinConversion = info_det.do_Cantidad_vw,
                            IdUnidadMedida_sinConversion = info_det.IdUnidadMedida,
                            Saldo = info_det.Saldo,

                            IdEmpresa_oc = IdEmpresaOC,
                            IdSucursal_oc = IdSucursalOC,
                            Secuencia_oc = Secuencia,
                            IdCentroCosto = info_det.IdCentroCosto,
                            IdPunto_cargo = info_det.IdPunto_cargo,
                            IdPunto_cargo_grupo = info_det.IdPunto_cargo_grupo,
                            cc_Descripcion = info_det.cc_Descripcion,
                            SecuenciaTipo = info_det.SecuenciaTipo
                        };
                        ObservacionOC = "OC# "+info_det.SecuenciaTipo+" " + info_det.oc_observacion;
                        List_in_Ing_Egr_Inven_det.AddRow(info_det_inv, IdTransaccionSession);
                    }
                }
            }
            //var model = List_in_Ing_Egr_Inven_det.get_list(IdTransaccionSession);
            return Json(ObservacionOC, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] in_Ing_Egr_Inven_det_Info info_det)
        {
            com_ordencompra_local_det_List Lista_OC = new com_ordencompra_local_det_List();
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            List<in_Ing_Egr_Inven_det_Info> lista_detalle = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            List<com_ordencompra_local_Info> lista_detalle_oc = Lista_OC.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            in_Ing_Egr_Inven_det_Info info_det_editar = lista_detalle.Where(q=> q.Secuencia == info_det.Secuencia).FirstOrDefault();
            com_ordencompra_local_Info info_oc = lista_detalle_oc.Where(q => q.IdEmpresa == info_det_editar.IdEmpresa && q.IdSucursal == info_det_editar.IdSucursal && q.IdOrdenCompra == info_det_editar.IdOrdenCompra && q.IdProducto == info_det_editar.IdProducto).FirstOrDefault();
            info_det_editar.dm_cantidad_sinConversion = info_det.dm_cantidad_sinConversion;

            if (info_det_editar != null)
                if (info_det_editar.IdProducto != 0)
                {
                    in_Producto_Info info_producto = bus_producto.get_info(IdEmpresa, info_det_editar.IdProducto);
                    if (info_producto != null)
                    {
                        info_det.Saldo = info_det_editar.Saldo;
                        info_det.IdProducto = info_det_editar.IdProducto;
                        info_det.pr_descripcion = info_producto.pr_descripcion_combo;
                        info_det.IdUnidadMedida_sinConversion = info_producto.IdUnidadMedida;
                    }
                }


            if (info_det_editar.dm_cantidad_sinConversion > 0 && info_det_editar.dm_cantidad_sinConversion <= info_det_editar.Saldo)
                List_in_Ing_Egr_Inven_det.UpdateRow(info_det_editar, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_inv_det", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            List_in_Ing_Egr_Inven_det.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            in_Ing_Egr_Inven_Info model = new in_Ing_Egr_Inven_Info();
            model.lst_in_Ing_Egr_Inven_det = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_inv_det", model.lst_in_Ing_Egr_Inven_det);
        }
        #endregion

        #region OC por ingresar
        public ActionResult GridViewPartial_oc_x_ingresar()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = ListaPorIngresar.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_oc_x_ingresar", model);
        }
        #endregion

        #region validaciones cargar cobo
        private bool validar(in_Ing_Egr_Inven_Info i_validar, ref string msg)
        {
            if (i_validar.lst_in_Ing_Egr_Inven_det.Count == 0)
            {
                mensaje = "Debe ingresar al menos un producto";
                return false;
            }
            else
            {
                com_ordencompra_local_det_List Lista_OC = new com_ordencompra_local_det_List();
                List<com_ordencompra_local_Info> lista_detalle_oc = Lista_OC.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

                if (i_validar.IdNumMovi == 0)
                {
                    foreach (var item in i_validar.lst_in_Ing_Egr_Inven_det)
                    {
                        com_ordencompra_local_Info info_oc = lista_detalle_oc.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdSucursal == item.IdSucursal && q.IdOrdenCompra == item.IdOrdenCompra && q.IdProducto == item.IdProducto).FirstOrDefault();
                        if (item.dm_cantidad_sinConversion > info_oc.Saldo)
                        {
                            mensaje = "La cantidad ingresada supera al saldo pendiente del producto: "+item.pr_descripcion;
                            return false;
                        }
                    }

                    var Cant_OC = i_validar.lst_in_Ing_Egr_Inven_det.GroupBy(q => q.IdOrdenCompra).Count();
                    if (Cant_OC > 1)
                    {
                        mensaje = "Debe ingresar items de una sola orden de compra";
                        return false;
                    }

                    #region ValidarExisteProductoxBodega
                    var param = bus_in_param.get_info(i_validar.IdEmpresa);
                    mensaje = bus_producto_x_bodega.ValidarProductoPorBodega(new List<in_producto_x_tb_bodega_Info>(i_validar.lst_in_Ing_Egr_Inven_det.Select(q => new in_producto_x_tb_bodega_Info
                    {
                        IdEmpresa = i_validar.IdEmpresa,
                        IdSucursal = i_validar.IdSucursal,
                        IdBodega = i_validar.IdBodega ?? 0,
                        IdProducto = q.IdProducto,
                        pr_descripcion = q.pr_descripcion
                    }).ToList()), (param.ValidarCtaCbleTransacciones ?? false));
                    if (!string.IsNullOrEmpty(mensaje))
                        return false;
                    #endregion
                }
            }
            

            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.cm_fecha, cl_enumeradores.eModulo.INV, i_validar.IdSucursal, ref msg))
            {
                return false;
            }
            return true;
        }
        private void cargar_combos(in_Ing_Egr_Inven_Info model)
        {
            in_Motivo_Inven_Bus bus_motivo = new in_Motivo_Inven_Bus();
            var lst_motivo = bus_motivo.get_list(model.IdEmpresa, cl_enumeradores.eTipoIngEgr.ING.ToString(), false);
            ViewBag.lst_motivo = lst_motivo;

            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(model.IdEmpresa, SessionFixed.IdUsuario, false);
            ViewBag.lst_sucursal = lst_sucursal;

            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var lst_bodega = bus_bodega.get_list(model.IdEmpresa, model.IdSucursal, false);
            ViewBag.lst_bodega = lst_bodega;
        }
        #endregion

        #region Json
        public JsonResult CargarBodega(int IdEmpresa = 0, int IdSucursal = 0)
        {
            bus_bodega = new tb_bodega_Bus();
            var resultado = bus_bodega.get_list(IdEmpresa, IdSucursal, false);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListPorIngresar(decimal IdTransaccionSession = 0, int IdEmpresa = 0, int IdSucursal = 0, decimal IdResponsable=0)
        {
            com_parametro_Info info_parametro_compra = bus_com_param.get_info(IdEmpresa);
            var lst = bus_orden_compra.get_list_x_ingresar(IdEmpresa, IdSucursal, IdResponsable);
            ListaPorIngresar.set_list(lst, IdTransaccionSession);

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class in_IngresoOC_Inven_Lista
    {
        string Variable = "in_IngresoOC_Inven_Info";
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

    public class in_Ing_Egr_Inven_det_OCList
    {
        string Variable = "in_Ing_Egr_Inven_det_Info";
        public List<in_Ing_Egr_Inven_det_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_det_Info> list = new List<in_Ing_Egr_Inven_det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_Ing_Egr_Inven_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(in_Ing_Egr_Inven_det_Info info_det_inv, decimal IdTransaccionSession)
        {
            List<in_Ing_Egr_Inven_det_Info> list = get_list(IdTransaccionSession);
            info_det_inv.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            //info_det_inv.secuencia_inv = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            //info_det_inv.Secuencia_oc = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;

            list.Add(info_det_inv);
        }

        public void UpdateRow(in_Ing_Egr_Inven_det_Info info_det, decimal IdTransaccionSession)
        {
            in_Ing_Egr_Inven_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();

            edited_info.IdProducto = info_det.IdProducto;
            edited_info.IdUnidadMedida_sinConversion = info_det.IdUnidadMedida_sinConversion;
            edited_info.mv_costo_sinConversion = info_det.mv_costo_sinConversion;
            edited_info.dm_cantidad_sinConversion = info_det.dm_cantidad_sinConversion;
            edited_info.pr_descripcion = info_det.pr_descripcion;
            edited_info.IdProducto = info_det.IdProducto;
            edited_info.Saldo = info_det.Saldo;
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<in_Ing_Egr_Inven_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).FirstOrDefault());
        }
    }

    public class com_ordencompra_local_det_List
    {
        string Variable = "com_ordencompra_local_Info";
        public List<com_ordencompra_local_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<com_ordencompra_local_Info> list = new List<com_ordencompra_local_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<com_ordencompra_local_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<com_ordencompra_local_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(com_ordencompra_local_Info info_det, decimal IdTransaccionSession)
        {
            List<com_ordencompra_local_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            info_det.IdProducto = info_det.IdProducto;
            info_det.IdUnidadMedida = info_det.IdUnidadMedida;
            list.Add(info_det);
        }

        public void UpdateRow(com_ordencompra_local_Info info_det, decimal IdTransaccionSession)
        {
            com_ordencompra_local_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.IdProducto = info_det.IdProducto;
            edited_info.IdUnidadMedida = info_det.IdUnidadMedida;
            edited_info.pr_descripcion = info_det.pr_descripcion;
            edited_info.IdProducto = info_det.IdProducto;
        }
    }
}