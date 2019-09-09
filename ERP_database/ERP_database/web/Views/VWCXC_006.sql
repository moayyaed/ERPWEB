CREATE VIEW web.VWCXC_006
AS
SELECT cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdCobro, cxc_cobro_det.secuencial, fa_factura.vt_tipoDoc, fa_factura.IdBodega, fa_factura.IdCbteVta, fa_Vendedor.Ve_Vendedor, fa_factura.vt_fecha, 
                  fa_proforma_det.NumCotizacion, fa_proforma_det.NumOPr, fa_factura.CodCbteVta, fa_factura.IdCliente, tb_persona.pe_nombreCompleto, fa_factura.IdVendedor, 
                  fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura AS vt_NumFactura, fa_factura.vt_fech_venc, cxc_cobro.cr_fecha, CASE WHEN fa_factura.vt_fech_venc > cxc_cobro.cr_fecha THEN 0 ELSE DATEDIFF(DAY, 
                  fa_factura.vt_fech_venc, cxc_cobro.cr_fecha) END AS DiasAtraso, cxc_cobro_det.dc_ValorPago AS ValorPago, CASE WHEN fa_factura_resumen.ValorIVA > 0 THEN round(cxc_cobro_det.dc_ValorPago / 1.12, 2) 
                  ELSE cxc_cobro_det.dc_ValorPago END AS BaseComision, cxc_cobro_det.IdCobro_tipo
FROM     cxc_cobro_det INNER JOIN
                  cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                  fa_cliente INNER JOIN
                  fa_factura INNER JOIN
                  fa_factura_det ON fa_factura.IdEmpresa = fa_factura_det.IdEmpresa AND fa_factura.IdSucursal = fa_factura_det.IdSucursal AND fa_factura.IdBodega = fa_factura_det.IdBodega AND fa_factura.IdCbteVta = fa_factura_det.IdCbteVta ON 
                  fa_cliente.IdEmpresa = fa_factura.IdEmpresa AND fa_cliente.IdCliente = fa_factura.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  fa_Vendedor ON fa_factura.IdEmpresa = fa_Vendedor.IdEmpresa AND fa_factura.IdVendedor = fa_Vendedor.IdVendedor INNER JOIN
                  fa_factura_resumen ON fa_factura.IdEmpresa = fa_factura_resumen.IdEmpresa AND fa_factura.IdSucursal = fa_factura_resumen.IdSucursal AND fa_factura.IdBodega = fa_factura_resumen.IdBodega AND 
                  fa_factura.IdCbteVta = fa_factura_resumen.IdCbteVta ON cxc_cobro_det.IdEmpresa = fa_factura.IdEmpresa AND cxc_cobro_det.IdSucursal = fa_factura.IdSucursal AND cxc_cobro_det.IdBodega_Cbte = fa_factura.IdBodega AND 
                  cxc_cobro_det.IdCbte_vta_nota = fa_factura.IdCbteVta AND cxc_cobro_det.dc_TipoDocumento = fa_factura.vt_tipoDoc LEFT OUTER JOIN
                  fa_proforma_det ON fa_factura_det.IdEmpresa_pf = fa_proforma_det.IdEmpresa AND fa_factura_det.IdSucursal_pf = fa_proforma_det.IdSucursal AND fa_factura_det.IdProforma = fa_proforma_det.IdProforma AND 
                  fa_factura_det.Secuencia_pf = fa_proforma_det.Secuencia
WHERE fa_factura.Estado = 'A' AND cxc_cobro.cr_estado = 'A' AND fa_factura_det.vt_PrecioFinal > 0
GROUP BY cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdCobro, cxc_cobro_det.secuencial, fa_factura.vt_tipoDoc, fa_factura.IdBodega, fa_factura.IdCbteVta, fa_Vendedor.Ve_Vendedor, fa_factura.vt_fecha, 
                  fa_proforma_det.NumCotizacion, fa_proforma_det.NumOPr, fa_factura.CodCbteVta, fa_factura.IdCliente, tb_persona.pe_nombreCompleto, fa_factura.IdVendedor, 
                  fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura, fa_factura.vt_fech_venc, cxc_cobro.cr_fecha, cxc_cobro_det.dc_ValorPago, cxc_cobro_det.IdCobro_tipo,fa_factura_resumen.ValorIVA