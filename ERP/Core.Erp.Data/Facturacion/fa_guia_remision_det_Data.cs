using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Facturacion
{
   public class fa_guia_remision_det_Data
    {
        public List<fa_guia_remision_det_Info> get_list(int IdEmpresa, decimal IdGuiaRemision)
        {
            try
            {
                List<fa_guia_remision_det_Info> Lista;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.vwfa_guia_remision_det
                             where q.IdEmpresa == IdEmpresa
                             && q.IdGuiaRemision == IdGuiaRemision
                             select new fa_guia_remision_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdProducto = q.IdProducto,
                                 IdGuiaRemision = q.IdGuiaRemision,
                                 Secuencia = q.Secuencia,
                                 gi_cantidad = q.gi_cantidad,
                                 gi_detallexItems = q.gi_detallexItems,
                                 pr_descripcion=q.pr_descripcion,
                                 gi_precio = q.gi_precio,
                                 gi_por_desc = q.gi_por_desc,
                                 gi_descuentoUni = q.gi_descuentoUni,
                                 gi_PrecioFinal = q.gi_PrecioFinal,
                                 gi_Subtotal = q.gi_Subtotal,
                                 IdCod_Impuesto = q.IdCod_Impuesto,
                                 gi_por_iva = q.gi_por_iva,
                                 gi_Iva = q.gi_Iva,
                                 gi_Total = q.gi_Total,
                                 IdCentroCosto = q.IdCentroCosto,
                                 cc_Descripcion = q.cc_Descripcion,
                                 gi_Subtotal_item = q.gi_Subtotal,
                                 gi_Iva_item = q.gi_Iva,
                                 gi_Total_item = q.gi_Total
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool eliminar(int IdEmpresa, decimal IdGuiaRemision)
        {
            try
            {
                using (Entities_importacion context = new Entities_importacion())
                {
                    string sql = "delete fa_guia_remision_det where IdEmpresa='" + IdEmpresa + "' and IdGuiaRemision='" + IdGuiaRemision + "'";
                    context.Database.ExecuteSqlCommand(sql);
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<fa_guia_remision_det_Info> get_list_proformas_x_guia(int IdEmpresa, int IdSucursal, decimal IdCliente)
        {
            try
            {
                List<fa_guia_remision_det_Info> Lista;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.vwfa_proforma_det_por_guia
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdCliente == IdCliente
                             select new fa_guia_remision_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdProforma = q.IdProforma,
                                 Secuencia_pf = q.Secuencia,
                                 IdProducto = q.IdProducto,
                                 gi_cantidad = q.pd_cantidad,
                                 pr_descripcion = q.pr_descripcion,
                                 gi_precio = q.pd_precio,
                                 gi_por_desc = q.pd_por_descuento_uni,
                                 gi_descuentoUni = q.pd_descuento_uni,
                                 gi_PrecioFinal = q.pd_precio_final,
                                 gi_Subtotal = q.pd_subtotal,
                                 IdCod_Impuesto = q.IdCod_Impuesto,
                                 gi_por_iva = q.pd_por_iva,
                                 gi_Iva = q.pd_iva,
                                 gi_Total = q.pd_total,
                                 IdEmpresa_pf = q.IdEmpresa,
                                 IdSucursal_pf = q.IdSucursal,
                                 IdCentroCosto = q.IdCentroCosto,
                                 cc_Descripcion = q.cc_Descripcion,
                                 NumCotizacion = q.NumCotizacion ?? 0,
                                 NumOPr = q.NumOPr ?? 0,
                                 gi_detallexItems = q.pd_DetalleAdicional,
                                 gi_Subtotal_item = q.pd_subtotal,
                                 gi_Iva_item = q.pd_iva,
                                 gi_Total_item = q.pd_total
                             }).ToList();
                }
                Lista.ForEach(V =>
                {                   
                    V.SecuencialUnico = Convert.ToInt32(V.IdEmpresa_pf).ToString("00") + Convert.ToInt32(V.IdSucursal_pf).ToString("00") + Convert.ToInt32(V.IdProforma).ToString("000000") + Convert.ToInt32(V.Secuencia_pf).ToString("00");
                });
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<fa_guia_remision_det_Info> get_list_proforma(int IdEmpresa, int IdSucursal, decimal IdCliente, decimal IdProforma)
        {
            try
            {
                List<fa_guia_remision_det_Info> Lista;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.vwfa_proforma_det_por_guia
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdCliente == IdCliente
                             && q.IdProforma == IdProforma
                             select new fa_guia_remision_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdProforma = q.IdProforma,
                                 Secuencia_pf = q.Secuencia,
                                 IdProducto = q.IdProducto,
                                 gi_cantidad = q.pd_cantidad,
                                 pr_descripcion = q.pr_descripcion,
                                 gi_precio = q.pd_precio,
                                 gi_por_desc = q.pd_por_descuento_uni,
                                 gi_descuentoUni = q.pd_descuento_uni,
                                 gi_PrecioFinal = q.pd_precio_final,
                                 gi_Subtotal = q.pd_subtotal,
                                 IdCod_Impuesto = q.IdCod_Impuesto,
                                 gi_por_iva = q.pd_por_iva,
                                 gi_Iva = q.pd_iva,
                                 gi_Total = q.pd_total,
                                 IdEmpresa_pf = q.IdEmpresa,
                                 IdSucursal_pf = q.IdSucursal,
                                 IdCentroCosto = q.IdCentroCosto,
                                 cc_Descripcion = q.cc_Descripcion,
                                 NumCotizacion = q.NumCotizacion??0,
                                 NumOPr = q.NumOPr??0,
                                 gi_detallexItems = q.pd_DetalleAdicional,
                                 gi_Subtotal_item = q.pd_subtotal,
                                 gi_Iva_item = q.pd_iva,
                                 gi_Total_item = q.pd_total
                             }).ToList();
                }
                Lista.ForEach(V =>
                {                  
                    V.SecuencialUnico = Convert.ToInt32(V.IdEmpresa_pf).ToString("00") + Convert.ToInt32(V.IdSucursal_pf).ToString("00") + Convert.ToInt32(V.IdProforma).ToString("000000") + Convert.ToInt32(V.Secuencia_pf).ToString("00");
                });
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
