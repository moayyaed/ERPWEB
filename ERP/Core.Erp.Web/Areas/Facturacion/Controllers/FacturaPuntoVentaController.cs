using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Facturacion;
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

            fa_factura_Info model = new fa_factura_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                vt_fecha = DateTime.Now,
                vt_plazo = 1,
                vt_fech_venc = DateTime.Now.AddDays(1),
                vt_tipoDoc = "FACT",
                IdCatalogo_FormaPago = "CRE",
                IdVendedor=1,
                info_resumen = new fa_factura_resumen_Info(),
                lst_det = new List<fa_factura_det_Info>(),
                lst_cuota = new List<fa_cuotas_x_doc_Info>(),

            };

            var lst = bus_marca.get_list(model.IdEmpresa, false);
            Lista_Marca.set_list(lst, model.IdTransaccionSession);
            Lista_Producto.set_list(new List<in_Producto_Info>(), model.IdTransaccionSession);
            return View(model);
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

            return Json(new { subtotal= Subtotal_Detalle, iva= Iva_Detalle, total= Total_Detalle }, JsonRequestBehavior.AllowGet);
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
            return Json(new { subtotal = Subtotal_Detalle, iva = Iva_Detalle, total = Total_Detalle }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EliminarProducto(int SecuencialID = 0, decimal IdTransaccionSession = 0)
        {
            var lst_actual = List_det.get_list(IdTransaccionSession).ToList();
            lst_actual.Remove(lst_actual.Where(q => q.Secuencia == SecuencialID).FirstOrDefault());
            List_det.set_list(lst_actual, IdTransaccionSession);

            return Json(lst_actual.Count(), JsonRequestBehavior.AllowGet);
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
    }
}