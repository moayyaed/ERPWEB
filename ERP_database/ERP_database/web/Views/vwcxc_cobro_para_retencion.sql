CREATE VIEW [web].[vwcxc_cobro_para_retencion]
AS
SELECT dbo.fa_factura_resumen.IdEmpresa, dbo.fa_factura_resumen.IdSucursal, dbo.fa_factura_resumen.IdBodega, dbo.fa_factura_resumen.IdCbteVta, dbo.fa_factura.vt_tipoDoc, ISNULL(dbo.fa_factura_resumen.SubtotalConDscto, 0) 
                  AS vt_Subtotal, ISNULL(dbo.fa_factura_resumen.ValorIVA, 0) AS vt_iva, ISNULL(dbo.fa_factura_resumen.Total, 0) AS vt_total, tb_persona.pe_nombreCompleto Nombres, dbo.fa_factura.vt_fecha, dbo.fa_factura.vt_fech_venc, 
                  dbo.fa_factura.vt_Observacion, dbo.fa_factura.vt_serie1 + '-' + dbo.fa_factura.vt_serie2 + '-' + dbo.fa_factura.vt_NumFactura AS vt_NumFactura, dbo.tb_sucursal.Su_Descripcion, dbo.fa_factura.IdCliente, 
                  CASE WHEN RET.IdEmpresa IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS TieneRetencion, isnull(cr_EsElectronico, cast(0 AS bit)) cr_EsElectronico,
				  ROUND(fa_factura_resumen.Total - ISNULL(Cobro.dc_ValorPago,0),2) Saldo
FROM     dbo.fa_factura INNER JOIN
                  dbo.fa_factura_resumen ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_resumen.IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_resumen.IdSucursal AND dbo.fa_factura.IdBodega = dbo.fa_factura_resumen.IdBodega AND 
                  dbo.fa_factura.IdCbteVta = dbo.fa_factura_resumen.IdCbteVta INNER JOIN
                  dbo.tb_sucursal ON dbo.fa_factura_resumen.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.fa_factura_resumen.IdSucursal = dbo.tb_sucursal.IdSucursal INNER JOIN
                  fa_cliente ON fa_factura.IdEmpresa = fa_cliente.IdEmpresa AND fa_factura.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona LEFT JOIN
                      (SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdBodega_Cbte, dbo.cxc_cobro_det.IdCbte_vta_nota, dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro, 
                                         cr_EsElectronico
                       FROM      dbo.cxc_cobro INNER JOIN
                                         dbo.cxc_cobro_det ON dbo.cxc_cobro.IdEmpresa = dbo.cxc_cobro_det.IdEmpresa AND dbo.cxc_cobro.IdSucursal = dbo.cxc_cobro_det.IdSucursal AND dbo.cxc_cobro.IdCobro = dbo.cxc_cobro_det.IdCobro INNER JOIN
                                         dbo.cxc_cobro_tipo ON dbo.cxc_cobro_det.IdCobro_tipo = dbo.cxc_cobro_tipo.IdCobro_tipo
                       WHERE   (dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro = 'RET') AND (dbo.cxc_cobro_det.estado = 'A') AND (dbo.cxc_cobro.cr_estado = N'A')
                       GROUP BY dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdBodega_Cbte, dbo.cxc_cobro_det.IdCbte_vta_nota, dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro, 
                                         cr_EsElectronico) AS RET ON fa_factura.IdEmpresa = RET.IdEmpresa AND fa_factura.IdSucursal = RET.IdSucursal AND fa_factura.IdBodega = RET.IdBodega_Cbte AND fa_factura.IdCbteVta = RET.IdCbte_vta_nota AND 
                  fa_factura.vt_tipoDoc = RET.dc_TipoDocumento
				  left join (
				  select d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento, sum(d.dc_ValorPago)dc_ValorPago from cxc_cobro_det d
				  where d.dc_TipoDocumento = 'FACT'
				  group by d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento
				  ) AS Cobro on Cobro.IdEmpresa = fa_factura.IdEmpresa AND COBRO.IdSucursal = fa_factura.IdSucursal AND COBRO.IdBodega_Cbte = fa_factura.IdBodega AND COBRO.IdCbte_vta_nota = fa_factura.IdCbteVta AND COBRO.dc_TipoDocumento = fa_factura.vt_tipoDoc
WHERE  (dbo.fa_factura.Estado = 'A')
UNION ALL
SELECT dbo.fa_notaCreDeb_det.IdEmpresa, dbo.fa_notaCreDeb_det.IdSucursal, dbo.fa_notaCreDeb_det.IdBodega, dbo.fa_notaCreDeb_det.IdNota, dbo.fa_notaCreDeb.CodDocumentoTipo, ISNULL(SUM(dbo.fa_notaCreDeb_det.sc_subtotal), 0) 
                  AS sc_subtotal, ISNULL(SUM(dbo.fa_notaCreDeb_det.sc_iva), 0) AS sc_iva, ISNULL(SUM(dbo.fa_notaCreDeb_det.sc_total), 0) AS sc_total, dbo.tb_persona.pe_nombreCompleto, dbo.fa_notaCreDeb.no_fecha, 
                  dbo.fa_notaCreDeb.no_fecha_venc, dbo.fa_notaCreDeb.sc_observacion, 
                  CASE WHEN fa_notaCreDeb.NaturalezaNota = 'SRI' THEN fa_notaCreDeb.Serie1 + '-' + fa_notaCreDeb.Serie2 + '-' + fa_notaCreDeb.NumNota_Impresa ELSE ISNULL(fa_notaCreDeb.CodNota, CAST(fa_notaCreDeb.IdNota AS varchar(20))) 
                  END AS vt_NumFactura, dbo.tb_sucursal.Su_Descripcion, dbo.fa_cliente.IdCliente, CASE WHEN RET.IdEmpresa IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS TieneRetencion, isnull(cr_EsElectronico, cast(0 AS bit)),
				  round(ISNULL(SUM(dbo.fa_notaCreDeb_det.sc_total), 0) - ISNULL(Cobro.dc_ValorPago,0),2) Saldo
FROM     dbo.fa_notaCreDeb INNER JOIN
                  dbo.fa_notaCreDeb_det ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_notaCreDeb_det.IdEmpresa AND dbo.fa_notaCreDeb.IdSucursal = dbo.fa_notaCreDeb_det.IdSucursal AND 
                  dbo.fa_notaCreDeb.IdBodega = dbo.fa_notaCreDeb_det.IdBodega AND dbo.fa_notaCreDeb.IdNota = dbo.fa_notaCreDeb_det.IdNota INNER JOIN
                  dbo.fa_cliente ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_notaCreDeb.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.tb_sucursal ON dbo.fa_notaCreDeb_det.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.fa_notaCreDeb_det.IdSucursal = dbo.tb_sucursal.IdSucursal LEFT JOIN
                      (SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdBodega_Cbte, dbo.cxc_cobro_det.IdCbte_vta_nota, dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro, 
                                         cxc_cobro.cr_EsElectronico
                       FROM      dbo.cxc_cobro INNER JOIN
                                         dbo.cxc_cobro_det ON dbo.cxc_cobro.IdEmpresa = dbo.cxc_cobro_det.IdEmpresa AND dbo.cxc_cobro.IdSucursal = dbo.cxc_cobro_det.IdSucursal AND dbo.cxc_cobro.IdCobro = dbo.cxc_cobro_det.IdCobro INNER JOIN
                                         dbo.cxc_cobro_tipo ON dbo.cxc_cobro_det.IdCobro_tipo = dbo.cxc_cobro_tipo.IdCobro_tipo
                       WHERE   (dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro = 'RET') AND (dbo.cxc_cobro_det.estado = 'A') AND (dbo.cxc_cobro.cr_estado = N'A')
                       GROUP BY dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdBodega_Cbte, dbo.cxc_cobro_det.IdCbte_vta_nota, dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro, 
                                         cr_EsElectronico) AS RET ON fa_notaCreDeb.IdEmpresa = RET.IdEmpresa AND fa_notaCreDeb.IdSucursal = RET.IdSucursal AND fa_notaCreDeb.IdBodega = RET.IdBodega_Cbte AND 
                  fa_notaCreDeb.IdNota = RET.IdCbte_vta_nota AND fa_notaCreDeb.CodDocumentoTipo = RET.dc_TipoDocumento
				  left join (
				  select d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento, sum(d.dc_ValorPago)dc_ValorPago from cxc_cobro_det d
				  where d.dc_TipoDocumento = 'FACT'
				  group by d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento
				  ) AS Cobro on Cobro.IdEmpresa = fa_notaCreDeb.IdEmpresa AND COBRO.IdSucursal = fa_notaCreDeb.IdSucursal AND COBRO.IdBodega_Cbte = fa_notaCreDeb.IdBodega AND COBRO.IdCbte_vta_nota = fa_notaCreDeb.IdNota AND COBRO.dc_TipoDocumento = fa_notaCreDeb.CodDocumentoTipo
WHERE  (dbo.fa_notaCreDeb.CreDeb = 'D')
GROUP BY dbo.fa_notaCreDeb_det.IdEmpresa, dbo.fa_notaCreDeb_det.IdSucursal, dbo.fa_notaCreDeb_det.IdBodega, dbo.fa_notaCreDeb_det.IdNota, dbo.fa_notaCreDeb.CodDocumentoTipo, dbo.tb_persona.pe_nombreCompleto, 
                  dbo.fa_notaCreDeb.no_fecha, dbo.fa_notaCreDeb.no_fecha_venc, dbo.fa_notaCreDeb.sc_observacion, dbo.fa_notaCreDeb.CodNota, dbo.fa_notaCreDeb.Serie1, dbo.fa_notaCreDeb.Serie2, dbo.fa_notaCreDeb.IdNota, 
                  dbo.fa_notaCreDeb.NaturalezaNota, dbo.fa_notaCreDeb.NumNota_Impresa, dbo.tb_sucursal.Su_Descripcion, dbo.fa_cliente.IdCliente, RET.IdEmpresa, cr_EsElectronico,Cobro.dc_ValorPago