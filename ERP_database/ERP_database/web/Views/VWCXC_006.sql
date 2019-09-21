CREATE VIEW [web].[VWCXC_006]
AS
SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, dbo.cxc_cobro_det.secuencial, dbo.fa_factura.vt_tipoDoc, dbo.fa_factura.IdBodega, dbo.fa_factura.IdCbteVta, dbo.fa_Vendedor.Ve_Vendedor, 
                  dbo.fa_factura.vt_fecha, dbo.fa_proforma_det.NumCotizacion, dbo.fa_proforma_det.NumOPr, dbo.fa_factura.CodCbteVta, dbo.fa_factura.IdCliente, dbo.tb_persona.pe_nombreCompleto, dbo.fa_factura.IdVendedor, 
                  dbo.fa_factura.vt_serie1 + '-' + dbo.fa_factura.vt_serie2 + '-' + dbo.fa_factura.vt_NumFactura AS vt_NumFactura, dbo.fa_factura.vt_fech_venc, dbo.cxc_cobro.cr_fecha, 
                  CASE WHEN fa_factura.vt_fech_venc > cxc_cobro.cr_fecha THEN 0 ELSE DATEDIFF(DAY, fa_factura.vt_fech_venc, cxc_cobro.cr_fecha) END AS DiasAtraso, dbo.cxc_cobro_det.dc_ValorPago AS ValorPago, 
                  CASE WHEN fa_factura_resumen.ValorIVA > 0 THEN round(cxc_cobro_det.dc_ValorPago / 1.12, 2) ELSE cxc_cobro_det.dc_ValorPago END AS BaseComision, dbo.cxc_cobro_det.IdCobro_tipo
FROM     dbo.cxc_cobro_det INNER JOIN
                  dbo.cxc_cobro ON dbo.cxc_cobro_det.IdEmpresa = dbo.cxc_cobro.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.cxc_cobro.IdSucursal AND dbo.cxc_cobro_det.IdCobro = dbo.cxc_cobro.IdCobro INNER JOIN
                  dbo.fa_cliente INNER JOIN
                  dbo.fa_factura INNER JOIN
                  dbo.fa_factura_det ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_det.IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_det.IdSucursal AND dbo.fa_factura.IdBodega = dbo.fa_factura_det.IdBodega AND 
                  dbo.fa_factura.IdCbteVta = dbo.fa_factura_det.IdCbteVta ON dbo.fa_cliente.IdEmpresa = dbo.fa_factura.IdEmpresa AND dbo.fa_cliente.IdCliente = dbo.fa_factura.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.fa_Vendedor ON dbo.fa_factura.IdEmpresa = dbo.fa_Vendedor.IdEmpresa AND dbo.fa_factura.IdVendedor = dbo.fa_Vendedor.IdVendedor INNER JOIN
                  dbo.fa_factura_resumen ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_resumen.IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_resumen.IdSucursal AND dbo.fa_factura.IdBodega = dbo.fa_factura_resumen.IdBodega AND 
                  dbo.fa_factura.IdCbteVta = dbo.fa_factura_resumen.IdCbteVta ON dbo.cxc_cobro_det.IdEmpresa = dbo.fa_factura.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.fa_factura.IdSucursal AND 
                  dbo.cxc_cobro_det.IdBodega_Cbte = dbo.fa_factura.IdBodega AND dbo.cxc_cobro_det.IdCbte_vta_nota = dbo.fa_factura.IdCbteVta AND dbo.cxc_cobro_det.dc_TipoDocumento = dbo.fa_factura.vt_tipoDoc LEFT OUTER JOIN
                  dbo.fa_proforma_det ON dbo.fa_factura_det.IdEmpresa_pf = dbo.fa_proforma_det.IdEmpresa AND dbo.fa_factura_det.IdSucursal_pf = dbo.fa_proforma_det.IdSucursal AND 
                  dbo.fa_factura_det.IdProforma = dbo.fa_proforma_det.IdProforma AND dbo.fa_factura_det.Secuencia_pf = dbo.fa_proforma_det.Secuencia
WHERE  (dbo.fa_factura.Estado = 'A') AND (dbo.cxc_cobro.cr_estado = 'A') AND (dbo.fa_factura_det.vt_PrecioFinal > 0)
GROUP BY dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, dbo.cxc_cobro_det.secuencial, dbo.fa_factura.vt_tipoDoc, dbo.fa_factura.IdBodega, dbo.fa_factura.IdCbteVta, dbo.fa_Vendedor.Ve_Vendedor, 
                  dbo.fa_factura.vt_fecha, dbo.fa_proforma_det.NumCotizacion, dbo.fa_proforma_det.NumOPr, dbo.fa_factura.CodCbteVta, dbo.fa_factura.IdCliente, dbo.tb_persona.pe_nombreCompleto, dbo.fa_factura.IdVendedor, 
                  dbo.fa_factura.vt_serie1 + '-' + dbo.fa_factura.vt_serie2 + '-' + dbo.fa_factura.vt_NumFactura, dbo.fa_factura.vt_fech_venc, dbo.cxc_cobro.cr_fecha, dbo.cxc_cobro_det.dc_ValorPago, dbo.cxc_cobro_det.IdCobro_tipo, 
                  dbo.fa_factura_resumen.ValorIVA
UNION ALL
SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, dbo.cxc_cobro_det.secuencial, dbo.fa_notaCreDeb.CodDocumentoTipo, dbo.fa_notaCreDeb.IdBodega, dbo.fa_notaCreDeb.IdNota, 
                  dbo.fa_Vendedor.Ve_Vendedor, dbo.fa_notaCreDeb.no_fecha, NULL, NULL, dbo.fa_notaCreDeb.sc_observacion, dbo.fa_notaCreDeb.IdCliente, dbo.tb_persona.pe_nombreCompleto, dbo.fa_notaCreDeb.IdVendedor, 
                  CASE WHEN fa_notaCreDeb.NaturalezaNota = 'SRI' THEN dbo.fa_notaCreDeb.Serie1 + '-' + dbo.fa_notaCreDeb.Serie2 + '-' + dbo.fa_notaCreDeb.NumNota_Impresa ELSE ISNULL(fa_notaCreDeb.CodNota, 
                  CAST(fa_notaCreDeb.IdNota AS VARCHAR(20))) END AS vt_NumFactura, dbo.fa_notaCreDeb.no_fecha_venc, dbo.cxc_cobro.cr_fecha, CASE WHEN fa_notaCreDeb.no_fecha_venc > cxc_cobro.cr_fecha THEN 0 ELSE DATEDIFF(DAY, 
                  fa_notaCreDeb.no_fecha_venc, cxc_cobro.cr_fecha) END AS DiasAtraso, dbo.cxc_cobro_det.dc_ValorPago AS ValorPago, CASE WHEN fa_notaCreDeb_resumen.ValorIVA > 0 THEN round(cxc_cobro_det.dc_ValorPago / 1.12, 2) 
                  ELSE cxc_cobro_det.dc_ValorPago END AS BaseComision, dbo.cxc_cobro_det.IdCobro_tipo
FROM     dbo.cxc_cobro_det INNER JOIN
                  dbo.cxc_cobro ON dbo.cxc_cobro_det.IdEmpresa = dbo.cxc_cobro.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.cxc_cobro.IdSucursal AND dbo.cxc_cobro_det.IdCobro = dbo.cxc_cobro.IdCobro INNER JOIN
                  dbo.fa_cliente INNER JOIN
                  dbo.fa_notaCreDeb ON dbo.fa_cliente.IdEmpresa = dbo.fa_notaCreDeb.IdEmpresa AND dbo.fa_cliente.IdCliente = dbo.fa_notaCreDeb.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.fa_Vendedor ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_Vendedor.IdEmpresa AND dbo.fa_notaCreDeb.IdVendedor = dbo.fa_Vendedor.IdVendedor INNER JOIN
                  dbo.fa_notaCreDeb_resumen ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_notaCreDeb_resumen.IdEmpresa AND dbo.fa_notaCreDeb.IdSucursal = dbo.fa_notaCreDeb_resumen.IdSucursal AND 
                  dbo.fa_notaCreDeb.IdBodega = dbo.fa_notaCreDeb_resumen.IdBodega AND dbo.fa_notaCreDeb.IdNota = dbo.fa_notaCreDeb_resumen.IdNota ON dbo.cxc_cobro_det.IdEmpresa = dbo.fa_notaCreDeb.IdEmpresa AND 
                  dbo.cxc_cobro_det.IdSucursal = dbo.fa_notaCreDeb.IdSucursal AND dbo.cxc_cobro_det.IdBodega_Cbte = dbo.fa_notaCreDeb.IdBodega AND dbo.cxc_cobro_det.IdCbte_vta_nota = dbo.fa_notaCreDeb.IdNota AND 
                  dbo.cxc_cobro_det.dc_TipoDocumento = dbo.fa_notaCreDeb.CodDocumentoTipo
WHERE  (dbo.fa_notaCreDeb.Estado = 'A') AND (dbo.cxc_cobro.cr_estado = 'A')