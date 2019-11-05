CREATE VIEW [dbo].[vwfa_ProbabilidadCobroDet]
AS
SELECT fa_factura.IdEmpresa, fa_factura.IdSucursal, fa_factura.IdBodega, fa_factura.IdCbteVta, fa_factura.vt_tipoDoc, fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura AS vt_NumFactura, fa_factura.vt_fecha, 
                  fa_factura.vt_fech_venc, CASE WHEN datediff(day, fa_factura.vt_fech_venc, getdate()) < 0 THEN 0 ELSE datediff(day, fa_factura.vt_fech_venc, getdate()) END DiasVencido, fa_factura.vt_Observacion, 
                  fa_factura_resumen.SubtotalConDscto, fa_factura_resumen.ValorIVA, fa_factura_resumen.Total, tb_persona.pe_nombreCompleto, isnull(round(SUM(cxc_cobro_det.dc_ValorPago), 2), 0) AS dc_ValorPago, 
                  round(fa_factura_resumen.Total - isnull(SUM(cxc_cobro_det.dc_ValorPago), 0), 2) Saldo, fa_ProbabilidadCobroDet.IdProbabilidad
FROM     fa_factura INNER JOIN
                  fa_factura_resumen ON fa_factura.IdEmpresa = fa_factura_resumen.IdEmpresa AND fa_factura.IdSucursal = fa_factura_resumen.IdSucursal AND fa_factura.IdBodega = fa_factura_resumen.IdBodega AND 
                  fa_factura.IdCbteVta = fa_factura_resumen.IdCbteVta INNER JOIN
                  fa_cliente ON fa_factura.IdEmpresa = fa_cliente.IdEmpresa AND fa_factura.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona LEFT OUTER JOIN
                  cxc_cobro_det ON fa_factura.IdEmpresa = cxc_cobro_det.IdEmpresa AND fa_factura.IdSucursal = cxc_cobro_det.IdSucursal AND fa_factura.IdBodega = cxc_cobro_det.IdBodega_Cbte AND 
                  fa_factura.IdCbteVta = cxc_cobro_det.IdCbte_vta_nota AND fa_factura.vt_tipoDoc = cxc_cobro_det.dc_TipoDocumento LEFT JOIN
                  fa_ProbabilidadCobroDet ON fa_ProbabilidadCobroDet.IdEmpresa = fa_factura.IdEmpresa AND fa_ProbabilidadCobroDet.IdSucursal = fa_factura.IdSucursal AND fa_ProbabilidadCobroDet.IdBodega = fa_factura.IdBodega AND 
                  fa_ProbabilidadCobroDet.IdCbteVta = fa_factura.IdCbteVta AND fa_ProbabilidadCobroDet.vt_tipoDoc = fa_factura.vt_tipoDoc
where fa_factura.Estado = 'A'
GROUP BY fa_factura.IdEmpresa, fa_factura.IdSucursal, fa_factura.IdBodega, fa_factura.IdCbteVta, fa_factura.vt_tipoDoc, fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura, fa_factura.vt_fecha, fa_factura.vt_plazo, 
                  fa_factura.vt_fech_venc, fa_factura.vt_Observacion, fa_factura_resumen.SubtotalConDscto, fa_factura_resumen.ValorIVA, fa_factura_resumen.Total, tb_persona.pe_nombreCompleto, fa_ProbabilidadCobroDet.IdProbabilidad
HAVING round(fa_factura_resumen.Total - isnull(SUM(cxc_cobro_det.dc_ValorPago), 0), 2) > 0
UNION ALL
SELECT fa_notaCreDeb.IdEmpresa, fa_notaCreDeb.IdSucursal, fa_notaCreDeb.IdBodega, fa_notaCreDeb.IdNota, fa_notaCreDeb.CodDocumentoTipo, 
                  CASE WHEN fa_notaCreDeb.NaturalezaNota = 'SRI' THEN fa_notaCreDeb.Serie1 + '-' + fa_notaCreDeb.Serie2 + '-' + fa_notaCreDeb.NumNota_Impresa ELSE ISNULL(fa_notaCreDeb.CodNota, 
                  CAST(fa_notaCreDeb.IdNota AS VARCHAR(20))) END AS vt_NumFactura, fa_notaCreDeb.no_fecha, fa_notaCreDeb.no_fecha_venc, CASE WHEN datediff(day, fa_notaCreDeb.no_fecha_venc, getdate()) < 0 THEN 0 ELSE datediff(day, 
                  fa_notaCreDeb.no_fecha_venc, getdate()) END DiasVencido, fa_notaCreDeb.sc_observacion, fa_notaCreDeb_resumen.SubtotalConDscto, fa_notaCreDeb_resumen.ValorIVA, fa_notaCreDeb_resumen.Total, 
                  tb_persona.pe_nombreCompleto, isnull(round(SUM(cxc_cobro_det.dc_ValorPago), 2), 0) AS dc_ValorPago, round(fa_notaCreDeb_resumen.Total - isnull(SUM(cxc_cobro_det.dc_ValorPago), 0), 2) Saldo, 
                  fa_ProbabilidadCobroDet.IdProbabilidad
FROM     fa_notaCreDeb INNER JOIN
                  fa_notaCreDeb_resumen ON fa_notaCreDeb.IdEmpresa = fa_notaCreDeb_resumen.IdEmpresa AND fa_notaCreDeb.IdSucursal = fa_notaCreDeb_resumen.IdSucursal AND 
                  fa_notaCreDeb.IdBodega = fa_notaCreDeb_resumen.IdBodega AND fa_notaCreDeb.IdNota = fa_notaCreDeb_resumen.IdNota INNER JOIN
                  fa_cliente ON fa_notaCreDeb.IdEmpresa = fa_cliente.IdEmpresa AND fa_notaCreDeb.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona LEFT OUTER JOIN
                  cxc_cobro_det ON fa_notaCreDeb.IdEmpresa = cxc_cobro_det.IdEmpresa AND fa_notaCreDeb.IdSucursal = cxc_cobro_det.IdSucursal AND fa_notaCreDeb.IdBodega = cxc_cobro_det.IdBodega_Cbte AND 
                  fa_notaCreDeb.IdNota = cxc_cobro_det.IdCbte_vta_nota AND fa_notaCreDeb.CodDocumentoTipo = cxc_cobro_det.dc_TipoDocumento LEFT JOIN
                  fa_ProbabilidadCobroDet ON fa_ProbabilidadCobroDet.IdEmpresa = fa_notaCreDeb.IdEmpresa AND fa_ProbabilidadCobroDet.IdSucursal = fa_notaCreDeb.IdSucursal AND 
                  fa_ProbabilidadCobroDet.IdBodega = fa_notaCreDeb.IdBodega AND fa_ProbabilidadCobroDet.IdCbteVta = fa_notaCreDeb.IdNota AND fa_ProbabilidadCobroDet.vt_tipoDoc = fa_notaCreDeb.CodDocumentoTipo
WHERE fa_notaCreDeb.Estado = 'A' AND fa_notaCreDeb.CreDeb = 'D'
GROUP BY fa_notaCreDeb.IdEmpresa, fa_notaCreDeb.IdSucursal, fa_notaCreDeb.IdBodega, fa_notaCreDeb.IdNota, fa_notaCreDeb.CodDocumentoTipo, fa_notaCreDeb.Serie1 + '-' + fa_notaCreDeb.Serie2 + '-' + fa_notaCreDeb.NumNota_Impresa, fa_notaCreDeb.CodNota,
                  fa_notaCreDeb.no_fecha, fa_notaCreDeb.no_fecha_venc, fa_notaCreDeb.no_fecha_venc, fa_notaCreDeb.sc_observacion, fa_notaCreDeb_resumen.SubtotalConDscto, fa_notaCreDeb_resumen.ValorIVA, fa_notaCreDeb_resumen.Total, 
                  tb_persona.pe_nombreCompleto, fa_notaCreDeb.NaturalezaNota, fa_ProbabilidadCobroDet.IdProbabilidad
HAVING round(fa_notaCreDeb_resumen.Total - isnull(SUM(cxc_cobro_det.dc_ValorPago), 0), 2) > 0