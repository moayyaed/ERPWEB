using Core.Erp.Data.Facturacion.Base;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Erp.Data.Facturacion
{
    public class fa_factura_det_Data
    {
        public List<fa_factura_det_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta)
        {
            try
            {
                List<fa_factura_det_Info> Lista;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.vwfa_factura_det
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdBodega == IdBodega
                             && q.IdCbteVta == IdCbteVta
                             select new fa_factura_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdCbteVta = q.IdCbteVta,
                                 IdProducto = q.IdProducto,
                                 vt_cantidad = q.vt_cantidad,
                                 vt_DescUnitario = q.vt_DescUnitario,
                                 vt_PrecioFinal = q.vt_PrecioFinal,
                                 vt_Precio = q.vt_Precio,
                                 vt_Subtotal = q.vt_Subtotal,
                                 vt_detallexItems = q.vt_detallexItems,
                                 vt_iva = q.vt_iva,
                                 vt_PorDescUnitario = q.vt_PorDescUnitario,
                                 vt_por_iva = q.vt_por_iva,
                                 vt_total = q.vt_total,
                                 IdCentroCosto = q.IdCentroCosto,
                                 IdCod_Impuesto_Iva = q.IdCod_Impuesto_Iva,
                                 IdEmpresa_pf = q.IdEmpresa_pf,
                                 IdProforma = q.IdProforma,
                                 IdPunto_Cargo = q.IdPunto_Cargo,
                                 IdPunto_cargo_grupo = q.IdPunto_cargo_grupo,
                                 IdSucursal_pf = q.IdSucursal_pf,
                                 Secuencia = q.Secuencia,
                                 Secuencia_pf = q.Secuencia_pf,
                                 pr_descripcion = q.pr_descripcion,
                                 nom_presentacion = q.nom_presentacion,
                                 lote_num_lote = q.lote_num_lote,
                                 lote_fecha_vcto = q.lote_fecha_vcto,
                                 CantidadAnterior = q.vt_cantidad,
                                 tp_manejaInven = q.tp_ManejaInven,
                                 se_distribuye = q.se_distribuye,
                                 cc_Descripcion = q.cc_Descripcion,
                                 vt_Subtotal_item = q.vt_Subtotal,
                                 vt_iva_item = q.vt_iva,
                                 vt_total_item = q.vt_total
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<fa_factura_det_Info> get_list_proformas_x_facturar(int IdEmpresa, int IdSucursal, decimal IdCliente)
        {
            try
            {
                List<fa_factura_det_Info> Lista = new List<fa_factura_det_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT dbo.fa_proforma_det.IdEmpresa, dbo.fa_proforma_det.IdSucursal, dbo.fa_proforma_det.IdProforma, dbo.fa_proforma_det.Secuencia, dbo.fa_proforma_det.IdProducto, dbo.fa_proforma_det.pd_cantidad, dbo.fa_proforma_det.pd_precio, "
                    + " dbo.fa_proforma_det.pd_por_descuento_uni, dbo.fa_proforma_det.pd_descuento_uni, dbo.fa_proforma_det.pd_precio_final, dbo.fa_proforma_det.pd_subtotal, dbo.fa_proforma_det.IdCod_Impuesto, dbo.fa_proforma_det.pd_por_iva, "
                    + " dbo.fa_proforma_det.pd_iva, dbo.fa_proforma_det.pd_total, dbo.fa_proforma_det.anulado, in_Producto_1.pr_descripcion, dbo.in_presentacion.nom_presentacion, in_Producto_1.lote_num_lote, in_Producto_1.lote_fecha_vcto, "
                    + " dbo.fa_proforma.IdCliente, in_Producto_1.se_distribuye, dbo.in_ProductoTipo.tp_ManejaInven, dbo.ct_CentroCosto.cc_Descripcion, dbo.fa_proforma_det.IdCentroCosto, dbo.fa_proforma_det.NumCotizacion, "
                    + " dbo.fa_proforma_det.NumOPr, dbo.fa_proforma_det.pd_DetalleAdicional, ROUND(dbo.fa_proforma_det.pd_cantidad - ISNULL(f.gi_cantidad, 0), 2) AS Saldo, dbo.fa_proforma.pf_observacion, dbo.fa_proforma.IdTerminoPago,  "
                    + " dbo.fa_proforma.pf_plazo, dbo.fa_proforma.IdVendedor "
                    + " FROM dbo.ct_CentroCosto RIGHT OUTER JOIN "
                    + " dbo.fa_proforma INNER JOIN "
                    + " dbo.fa_proforma_det ON dbo.fa_proforma.IdEmpresa = dbo.fa_proforma_det.IdEmpresa AND dbo.fa_proforma.IdSucursal = dbo.fa_proforma_det.IdSucursal AND dbo.fa_proforma.IdProforma = dbo.fa_proforma_det.IdProforma ON "
                    + " dbo.ct_CentroCosto.IdEmpresa = dbo.fa_proforma_det.IdEmpresa AND dbo.ct_CentroCosto.IdCentroCosto = dbo.fa_proforma_det.IdCentroCosto LEFT OUTER JOIN "
                    + " dbo.in_presentacion INNER JOIN "
                    + " dbo.in_Producto AS in_Producto_1 ON dbo.in_presentacion.IdEmpresa = in_Producto_1.IdEmpresa AND dbo.in_presentacion.IdPresentacion = in_Producto_1.IdPresentacion INNER JOIN "
                    + " dbo.in_ProductoTipo ON in_Producto_1.IdProductoTipo = dbo.in_ProductoTipo.IdProductoTipo AND in_Producto_1.IdEmpresa = dbo.in_ProductoTipo.IdEmpresa ON dbo.fa_proforma_det.IdEmpresa = in_Producto_1.IdEmpresa AND "
                    + " dbo.fa_proforma_det.IdProducto = in_Producto_1.IdProducto LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa_pf, IdSucursal_pf, IdProforma, Secuencia_pf, SUM(vt_cantidad) AS gi_cantidad "
                    + " FROM      dbo.fa_factura_det "
                    + " GROUP BY IdEmpresa_pf, IdSucursal_pf, IdProforma, Secuencia_pf) AS f ON dbo.fa_proforma_det.IdEmpresa = f.IdEmpresa_pf AND dbo.fa_proforma_det.IdSucursal = f.IdSucursal_pf AND "
                    + " dbo.fa_proforma_det.IdProforma = f.IdProforma AND dbo.fa_proforma_det.Secuencia = f.Secuencia_pf "
                    + " WHERE(dbo.fa_proforma.estado = 1) AND(ROUND(dbo.fa_proforma_det.pd_cantidad - ISNULL(f.gi_cantidad, 0), 2) > 0) AND(dbo.fa_proforma_det.anulado = 0)"
                    + " AND dbo.fa_proforma.IdEmpresa = " + IdEmpresa + "AND dbo.fa_proforma.IdSucursal = " + IdSucursal + " AND dbo.fa_proforma.IdCliente = " + IdCliente;
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new fa_factura_det_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdProforma = Convert.ToDecimal(reader["IdProforma"]),
                            Secuencia = Convert.ToInt32(reader["Secuencia"]),
                            cc_Descripcion = string.IsNullOrEmpty(reader["cc_Descripcion"].ToString()) ? null : reader["cc_Descripcion"].ToString(),
                            pf_observacion = reader["pf_observacion"].ToString(),
                            IdTerminoPago = reader["IdTerminoPago"].ToString(),
                            pf_plazo = Convert.ToInt32(reader["pf_plazo"]),
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            pd_cantidad = Convert.ToDouble(reader["pd_cantidad"]),
                            vt_cantidad = Convert.ToDouble(reader["Saldo"]),
                            vt_DescUnitario = Convert.ToDouble(reader["pd_descuento_uni"]),
                            vt_PrecioFinal = Convert.ToDouble(reader["pd_precio_final"]),
                            vt_Precio = Convert.ToDouble(reader["pd_precio"]),
                            vt_Subtotal = Convert.ToDouble(reader["pd_subtotal"]),
                            vt_iva = Convert.ToDouble(reader["pd_iva"]),
                            vt_PorDescUnitario = Convert.ToDouble(reader["pd_por_descuento_uni"]),
                            vt_por_iva = Convert.ToDouble(reader["pd_por_iva"]),
                            vt_total = Convert.ToDouble(reader["pd_total"]),
                            IdCod_Impuesto_Iva = reader["IdCod_Impuesto"].ToString(),
                            IdEmpresa_pf = string.IsNullOrEmpty(reader["IdEmpresa"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal_pf = string.IsNullOrEmpty(reader["IdSucursal"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdSucursal"]),
                            Secuencia_pf = string.IsNullOrEmpty(reader["Secuencia"].ToString()) ? (int?)null : Convert.ToInt32(reader["Secuencia"]),
                            pr_descripcion = reader["pr_descripcion"].ToString(),
                            nom_presentacion = reader["nom_presentacion"].ToString(),
                            lote_num_lote = string.IsNullOrEmpty(reader["lote_num_lote"].ToString()) ? null : reader["lote_num_lote"].ToString(),
                            lote_fecha_vcto = string.IsNullOrEmpty(reader["lote_fecha_vcto"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["lote_fecha_vcto"]),
                            tp_manejaInven = reader["tp_manejaInven"].ToString(),
                            se_distribuye = string.IsNullOrEmpty(reader["se_distribuye"].ToString()) ? (bool?)null : Convert.ToBoolean(reader["se_distribuye"]),
                            IdCentroCosto = string.IsNullOrEmpty(reader["IdCentroCosto"].ToString()) ? null : reader["IdCentroCosto"].ToString(),
                            NumCotizacion = string.IsNullOrEmpty(reader["NumCotizacion"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["NumCotizacion"]),
                            NumOPr = string.IsNullOrEmpty(reader["NumOPr"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["NumOPr"]),
                            vt_detallexItems = string.IsNullOrEmpty(reader["pd_DetalleAdicional"].ToString()) ? null : reader["pd_DetalleAdicional"].ToString(),
                            vt_Subtotal_item = Convert.ToDouble(reader["pd_subtotal"]),
                            vt_iva_item = Convert.ToDouble(reader["pd_iva"]),
                            vt_total_item = Convert.ToDouble(reader["pd_total"]),
                            Saldo = Convert.ToDouble(reader["Saldo"]),
                            IdVendedor = Convert.ToInt32(reader["IdVendedor"])
                        });
                    }
                    reader.Close();
                }
                /*
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.vwfa_proforma_det_por_facturar
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdCliente == IdCliente
                             select new fa_factura_det_Info
                             {
                                 IdProducto = q.IdProducto,
                                 vt_cantidad = q.Saldo,
                                 vt_DescUnitario = q.pd_descuento_uni,
                                 vt_PrecioFinal = q.pd_precio_final,
                                 vt_Precio = q.pd_precio,
                                 vt_Subtotal = q.pd_subtotal,
                                 vt_iva = q.pd_iva,
                                 vt_PorDescUnitario = q.pd_por_descuento_uni,
                                 vt_por_iva = q.pd_por_iva,
                                 vt_total = q.pd_total,
                                 IdCod_Impuesto_Iva = q.IdCod_Impuesto,
                                 IdEmpresa_pf = q.IdEmpresa,
                                 IdProforma = q.IdProforma,
                                 IdSucursal_pf = q.IdSucursal,
                                 Secuencia_pf = q.Secuencia,
                                 pr_descripcion = q.pr_descripcion,
                                 nom_presentacion = q.nom_presentacion,
                                 lote_num_lote = q.lote_num_lote,
                                 lote_fecha_vcto = q.lote_fecha_vcto,
                                 tp_manejaInven = q.tp_ManejaInven,
                                 se_distribuye = q.se_distribuye,
                                 IdCentroCosto = q.IdCentroCosto,
                                 cc_Descripcion = q.cc_Descripcion,
                                 NumCotizacion = q.NumCotizacion ?? 0,
                                 NumOPr = q.NumOPr ?? 0,
                                 vt_detallexItems = q.pd_DetalleAdicional,
                                 vt_Subtotal_item = q.pd_subtotal,
                                 vt_iva_item = q.pd_iva,
                                 vt_total_item = q.pd_total,
                                 Saldo = q.Saldo,
                                 pd_cantidad = q.pd_cantidad
                             }).ToList();
                }
                */
                Lista.ForEach(V =>
                {
                    V.secuencial = Convert.ToInt32(V.IdEmpresa_pf).ToString("00") + Convert.ToInt32(V.IdSucursal_pf).ToString("00") + Convert.ToInt32(V.IdProforma).ToString("000000") + Convert.ToInt32(V.Secuencia_pf).ToString("00");
                });
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<fa_factura_det_Info> get_list_proforma(int IdEmpresa, int IdSucursal, decimal IdCliente, decimal IdProforma)
        {
            try
            {
                List<fa_factura_det_Info> Lista = new List<fa_factura_det_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT dbo.fa_proforma_det.IdEmpresa, dbo.fa_proforma_det.IdSucursal, dbo.fa_proforma_det.IdProforma, dbo.fa_proforma_det.Secuencia, dbo.fa_proforma_det.IdProducto, dbo.fa_proforma_det.pd_cantidad, dbo.fa_proforma_det.pd_precio, "
                    + " dbo.fa_proforma_det.pd_por_descuento_uni, dbo.fa_proforma_det.pd_descuento_uni, dbo.fa_proforma_det.pd_precio_final, dbo.fa_proforma_det.pd_subtotal, dbo.fa_proforma_det.IdCod_Impuesto, dbo.fa_proforma_det.pd_por_iva, "
                    + " dbo.fa_proforma_det.pd_iva, dbo.fa_proforma_det.pd_total, dbo.fa_proforma_det.anulado, in_Producto_1.pr_descripcion, dbo.in_presentacion.nom_presentacion, in_Producto_1.lote_num_lote, in_Producto_1.lote_fecha_vcto, "
                    + " dbo.fa_proforma.IdCliente, in_Producto_1.se_distribuye, dbo.in_ProductoTipo.tp_ManejaInven, dbo.ct_CentroCosto.cc_Descripcion, dbo.fa_proforma_det.IdCentroCosto, dbo.fa_proforma_det.NumCotizacion, "
                    + " dbo.fa_proforma_det.NumOPr, dbo.fa_proforma_det.pd_DetalleAdicional, ROUND(dbo.fa_proforma_det.pd_cantidad - ISNULL(f.gi_cantidad, 0), 2) AS Saldo, dbo.fa_proforma.pf_observacion, dbo.fa_proforma.IdTerminoPago,  "
                    + " dbo.fa_proforma.pf_plazo, dbo.fa_proforma.IdVendedor "
                    + " FROM dbo.ct_CentroCosto RIGHT OUTER JOIN "
                    + " dbo.fa_proforma INNER JOIN "
                    + " dbo.fa_proforma_det ON dbo.fa_proforma.IdEmpresa = dbo.fa_proforma_det.IdEmpresa AND dbo.fa_proforma.IdSucursal = dbo.fa_proforma_det.IdSucursal AND dbo.fa_proforma.IdProforma = dbo.fa_proforma_det.IdProforma ON "
                    + " dbo.ct_CentroCosto.IdEmpresa = dbo.fa_proforma_det.IdEmpresa AND dbo.ct_CentroCosto.IdCentroCosto = dbo.fa_proforma_det.IdCentroCosto LEFT OUTER JOIN "
                    + " dbo.in_presentacion INNER JOIN "
                    + " dbo.in_Producto AS in_Producto_1 ON dbo.in_presentacion.IdEmpresa = in_Producto_1.IdEmpresa AND dbo.in_presentacion.IdPresentacion = in_Producto_1.IdPresentacion INNER JOIN "
                    + " dbo.in_ProductoTipo ON in_Producto_1.IdProductoTipo = dbo.in_ProductoTipo.IdProductoTipo AND in_Producto_1.IdEmpresa = dbo.in_ProductoTipo.IdEmpresa ON dbo.fa_proforma_det.IdEmpresa = in_Producto_1.IdEmpresa AND "
                    + " dbo.fa_proforma_det.IdProducto = in_Producto_1.IdProducto LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa_pf, IdSucursal_pf, IdProforma, Secuencia_pf, SUM(vt_cantidad) AS gi_cantidad "
                    + " FROM      dbo.fa_factura_det "
                    + " GROUP BY IdEmpresa_pf, IdSucursal_pf, IdProforma, Secuencia_pf) AS f ON dbo.fa_proforma_det.IdEmpresa = f.IdEmpresa_pf AND dbo.fa_proforma_det.IdSucursal = f.IdSucursal_pf AND "
                    + " dbo.fa_proforma_det.IdProforma = f.IdProforma AND dbo.fa_proforma_det.Secuencia = f.Secuencia_pf "
                    + " WHERE(dbo.fa_proforma.estado = 1) AND(ROUND(dbo.fa_proforma_det.pd_cantidad - ISNULL(f.gi_cantidad, 0), 2) > 0) AND(dbo.fa_proforma_det.anulado = 0)"
                    + " AND dbo.fa_proforma.IdEmpresa = " + IdEmpresa + "AND dbo.fa_proforma.IdSucursal = " + IdSucursal + "AND dbo.fa_proforma.IdCliente = " + IdCliente + "AND dbo.fa_proforma.IdProforma = " + IdProforma;
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new fa_factura_det_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdProforma = Convert.ToDecimal(reader["IdProforma"]),
                            Secuencia = Convert.ToInt32(reader["Secuencia"]),
                            cc_Descripcion = string.IsNullOrEmpty(reader["cc_Descripcion"].ToString()) ? null : reader["cc_Descripcion"].ToString(),
                            pf_observacion = reader["pf_observacion"].ToString(),
                            IdTerminoPago = reader["IdTerminoPago"].ToString(),
                            pf_plazo = Convert.ToInt32(reader["pf_plazo"]),
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            pd_cantidad = Convert.ToDouble(reader["pd_cantidad"]),
                            vt_cantidad = Convert.ToDouble(reader["Saldo"]),
                            vt_DescUnitario = Convert.ToDouble(reader["pd_descuento_uni"]),
                            vt_PrecioFinal = Convert.ToDouble(reader["pd_precio_final"]),
                            vt_Precio = Convert.ToDouble(reader["pd_precio"]),
                            vt_Subtotal = Convert.ToDouble(reader["pd_subtotal"]),
                            vt_iva = Convert.ToDouble(reader["pd_iva"]),
                            vt_PorDescUnitario = Convert.ToDouble(reader["pd_por_descuento_uni"]),
                            vt_por_iva = Convert.ToDouble(reader["pd_por_iva"]),
                            vt_total = Convert.ToDouble(reader["pd_total"]),
                            IdCod_Impuesto_Iva = reader["IdCod_Impuesto"].ToString(),
                            IdEmpresa_pf = string.IsNullOrEmpty(reader["IdEmpresa"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal_pf = string.IsNullOrEmpty(reader["IdSucursal"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdSucursal"]),
                            Secuencia_pf = string.IsNullOrEmpty(reader["Secuencia"].ToString()) ? (int?)null : Convert.ToInt32(reader["Secuencia"]),
                            pr_descripcion = reader["pr_descripcion"].ToString(),
                            nom_presentacion = reader["nom_presentacion"].ToString(),
                            lote_num_lote = string.IsNullOrEmpty(reader["lote_num_lote"].ToString()) ? null : reader["lote_num_lote"].ToString(),
                            lote_fecha_vcto = string.IsNullOrEmpty(reader["lote_fecha_vcto"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["lote_fecha_vcto"]),
                            tp_manejaInven = reader["tp_manejaInven"].ToString(),
                            se_distribuye = string.IsNullOrEmpty(reader["se_distribuye"].ToString()) ? (bool?)null : Convert.ToBoolean(reader["se_distribuye"]),
                            IdCentroCosto = string.IsNullOrEmpty(reader["IdCentroCosto"].ToString()) ? null : reader["IdCentroCosto"].ToString(),
                            NumCotizacion = string.IsNullOrEmpty(reader["NumCotizacion"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["NumCotizacion"]),
                            NumOPr = string.IsNullOrEmpty(reader["NumOPr"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["NumOPr"]),
                            vt_detallexItems = string.IsNullOrEmpty(reader["pd_DetalleAdicional"].ToString()) ? null : reader["pd_DetalleAdicional"].ToString(),
                            vt_Subtotal_item = Convert.ToDouble(reader["pd_subtotal"]),
                            vt_iva_item = Convert.ToDouble(reader["pd_iva"]),
                            vt_total_item = Convert.ToDouble(reader["pd_total"]),
                            Saldo = Convert.ToDouble(reader["Saldo"]),
                            IdVendedor = Convert.ToInt32(reader["IdVendedor"])
                        });
                    }
                    reader.Close();
                }
                /*
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.vwfa_proforma_det_por_facturar
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdCliente == IdCliente
                             && q.IdProforma == IdProforma
                             select new fa_factura_det_Info
                             {
                                 lote_num_lote = q.lote_num_lote,
                                 lote_fecha_vcto = q.lote_fecha_vcto,
                                 tp_manejaInven = q.tp_ManejaInven,
                                 se_distribuye = q.se_distribuye,
                                 IdCentroCosto = q.IdCentroCosto,
                                 cc_Descripcion = q.cc_Descripcion,
                                 NumCotizacion = q.NumCotizacion ?? 0,
                                 NumOPr = q.NumOPr ?? 0,
                                 vt_detallexItems = q.pd_DetalleAdicional,
                                 vt_Subtotal_item = q.pd_subtotal,
                                 vt_iva_item = q.pd_iva,
                                 vt_total_item = q.pd_total,
                                 Saldo = q.Saldo,
                                 pd_cantidad = q.pd_cantidad,
                                 IdProducto = q.IdProducto,
                                 vt_cantidad = q.Saldo,
                                 vt_DescUnitario = q.pd_descuento_uni,
                                 vt_PrecioFinal = q.pd_precio_final,
                                 vt_Precio = q.pd_precio,
                                 vt_Subtotal = q.pd_subtotal,
                                 vt_iva = q.pd_iva,
                                 vt_PorDescUnitario = q.pd_por_descuento_uni,
                                 vt_por_iva = q.pd_por_iva,
                                 vt_total = q.pd_total,
                                 IdCod_Impuesto_Iva = q.IdCod_Impuesto,
                                 IdEmpresa_pf = q.IdEmpresa,
                                 IdProforma = q.IdProforma,
                                 IdSucursal_pf = q.IdSucursal,
                                 Secuencia_pf = q.Secuencia,
                                 pr_descripcion = q.pr_descripcion,
                                 nom_presentacion = q.nom_presentacion
                             }).ToList();
                }*/
                Lista.ForEach(V =>
                {
                    V.secuencial = Convert.ToInt32(V.IdEmpresa_pf).ToString("00") + Convert.ToInt32(V.IdSucursal_pf).ToString("00") + Convert.ToInt32(V.IdProforma).ToString("000000") + Convert.ToInt32(V.Secuencia_pf).ToString("00");
                });
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
