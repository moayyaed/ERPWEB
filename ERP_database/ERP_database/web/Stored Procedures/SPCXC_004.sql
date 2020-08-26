--EXEC web.SPCXC_004 1,1,9999,1208,1208,'2019/01/01','2019/09/10',1
CREATE PROCEDURE [web].[SPCXC_004]
(
@IdEmpresa int,
@IdSucursalIni int,
@IdSucursalFin int,
@IdClienteIni int,
@IdClienteFin int,
@FechaIni date,
@FechaFin date,
@MostrarSaldo0 bit
)
AS
SELECT fa_factura.IdEmpresa, fa_factura.IdSucursal, fa_factura.IdBodega, fa_factura.IdCbteVta, fa_factura.vt_serie1+'-'+ fa_factura.vt_serie2+'-'+ fa_factura.vt_NumFactura vt_NumFactura, fa_factura.vt_tipoDoc, fa_factura.IdCliente, fa_factura.vt_fecha, fa_factura.vt_fech_venc, 
                  fa_factura.vt_Observacion, fa_factura.Estado, tb_persona.pe_nombreCompleto, fa_factura_resumen.Total, fa_factura_resumen.Total as Debe, 0 as Haber, fa_factura.vt_serie1+'-'+ fa_factura.vt_serie2+'-'+ fa_factura.vt_NumFactura AS Referencia,
				  1 as Orden, 'FACTURA' AS Tipo, fa_factura.vt_fecha as FechaReferencia, 0 IdReferencia, 0 SecuenciaReferencia, DATEDIFF(day,fa_factura.vt_fecha,CAST(GETDATE() AS DATE)) Dias, tb_sucursal.Su_Descripcion, 
				  ROUND(fa_factura_resumen.Total - isnull(cobro.dc_ValorPago,0),2) as Saldo, vt_tipoDoc as TipoDoc
FROM     fa_factura INNER JOIN
                  fa_cliente ON fa_factura.IdEmpresa = fa_cliente.IdEmpresa AND fa_factura.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  fa_factura_resumen ON fa_factura.IdEmpresa = fa_factura_resumen.IdEmpresa AND fa_factura.IdSucursal = fa_factura_resumen.IdSucursal AND fa_factura.IdBodega = fa_factura_resumen.IdBodega AND 
                  fa_factura.IdCbteVta = fa_factura_resumen.IdCbteVta INNER JOIN 
				  tb_sucursal ON tb_sucursal.IdEmpresa = fa_factura.IdEmpresa AND tb_sucursal.IdSucursal = fa_factura.IdSucursal LEFT JOIN
				  (
				  SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, ROUND(SUM(D.dc_ValorPago),2) dc_ValorPago
				  FROM cxc_cobro_det AS D INNER JOIN cxc_cobro AS C ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdCobro = D.IdCobro
				  where d.dc_TipoDocumento = 'FACT' AND C.cr_estado = 'A'
				  group by D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota
				  ) AS Cobro on Cobro.IdEmpresa = fa_factura.IdEmpresa and Cobro.IdSucursal = fa_factura.IdSucursal and Cobro.IdBodega_Cbte = fa_factura.IdBodega and Cobro.IdCbte_vta_nota = fa_factura.IdCbteVta
WHERE fa_factura.Estado = 'A' and fa_factura.IdEmpresa = @IdEmpresa and fa_factura.IdSucursal between @IdSucursalIni and @IdSucursalFin
and fa_factura.IdCliente between @IdClienteIni and @IdClienteFin and fa_factura.vt_fecha between @FechaIni and @FechaFin AND ROUND(fa_factura_resumen.Total - isnull(cobro.dc_ValorPago,0),2) > CASE WHEN @MostrarSaldo0 = 1 THEN -1 ELSE 0 END
UNION ALL
SELECT fa_factura.IdEmpresa, fa_factura.IdSucursal, fa_factura.IdBodega, fa_factura.IdCbteVta, fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura AS vt_NumFactura, fa_factura.vt_tipoDoc, fa_factura.IdCliente, 
                  fa_factura.vt_fecha, fa_factura.vt_fech_venc, fa_factura.vt_Observacion, fa_factura.Estado, tb_persona.pe_nombreCompleto, cxc_cobro_det.dc_ValorPago * - 1 AS Expr1, 0 AS Debe, cxc_cobro_det.dc_ValorPago AS Haber, 
                  cxc_cobro.cr_NumDocumento AS Referencia, 2 AS Orden, cxc_cobro_tipo.tc_descripcion As Tipo, cxc_cobro.cr_fecha, cxc_cobro_det.IdCobro, cxc_cobro_det.secuencial, DATEDIFF(day,fa_factura.vt_fecha,CAST(GETDATE() AS DATE)) Dias,
				  tb_sucursal.Su_Descripcion, ROUND(fa_factura_resumen.Total - isnull(cobro.dc_ValorPago,0),2) as Saldo, cxc_cobro_det.IdCobro_tipo
FROM     fa_factura INNER JOIN
                  fa_cliente ON fa_factura.IdEmpresa = fa_cliente.IdEmpresa AND fa_factura.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  fa_factura_resumen ON fa_factura.IdEmpresa = fa_factura_resumen.IdEmpresa AND fa_factura.IdSucursal = fa_factura_resumen.IdSucursal AND fa_factura.IdBodega = fa_factura_resumen.IdBodega AND 
                  fa_factura.IdCbteVta = fa_factura_resumen.IdCbteVta INNER JOIN
                  cxc_cobro_det ON fa_factura.IdEmpresa = cxc_cobro_det.IdEmpresa AND fa_factura.IdSucursal = cxc_cobro_det.IdSucursal AND fa_factura.IdBodega = cxc_cobro_det.IdBodega_Cbte AND 
                  fa_factura.IdCbteVta = cxc_cobro_det.IdCbte_vta_nota AND fa_factura.vt_tipoDoc = cxc_cobro_det.dc_TipoDocumento INNER JOIN
                  cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                  cxc_cobro_tipo ON cxc_cobro_det.IdCobro_tipo = cxc_cobro_tipo.IdCobro_tipo INNER JOIN 
				  tb_sucursal ON tb_sucursal.IdEmpresa = fa_factura.IdEmpresa AND tb_sucursal.IdSucursal = fa_factura.IdSucursal LEFT JOIN
				  (
				  SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, ROUND(SUM(D.dc_ValorPago),2) dc_ValorPago
				  FROM cxc_cobro_det AS D INNER JOIN cxc_cobro AS C ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdCobro = D.IdCobro
				  where d.dc_TipoDocumento = 'FACT' AND C.cr_estado = 'A'
				  group by D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota
				  ) AS Cobro on Cobro.IdEmpresa = fa_factura.IdEmpresa and Cobro.IdSucursal = fa_factura.IdSucursal and Cobro.IdBodega_Cbte = fa_factura.IdBodega and Cobro.IdCbte_vta_nota = fa_factura.IdCbteVta
WHERE fa_factura.Estado = 'A' and fa_factura.IdEmpresa = @IdEmpresa AND cxc_cobro.cr_estado = 'A' and fa_factura.IdSucursal between @IdSucursalIni and @IdSucursalFin
and fa_factura.IdCliente between @IdClienteIni and @IdClienteFin and fa_factura.vt_fecha between @FechaIni and @FechaFin AND ROUND(fa_factura_resumen.Total - isnull(cobro.dc_ValorPago,0),2) > CASE WHEN @MostrarSaldo0 = 1 THEN -1 ELSE 0 END
UNION ALL
SELECT fa_notaCreDeb.IdEmpresa, fa_notaCreDeb.IdSucursal, fa_notaCreDeb.IdBodega, fa_notaCreDeb.IdNota, CASE WHEN fa_notaCreDeb.NaturalezaNota = 'SRI' THEN fa_notaCreDeb.Serie1+'-'+fa_notaCreDeb.Serie2+'-'+fa_notaCreDeb.NumNota_Impresa ELSE fa_notaCreDeb.CodNota  END vt_NumFactura, fa_notaCreDeb.CodDocumentoTipo, fa_notaCreDeb.IdCliente, fa_notaCreDeb.no_fecha, fa_notaCreDeb.no_fecha_venc, 
                  fa_notaCreDeb.sc_observacion, fa_notaCreDeb.Estado, tb_persona.pe_nombreCompleto, d.Total, d.Total as Debe, 0 as Haber, fa_notaCreDeb.CodNota AS Referencia,
				  1 as Orden, 'NOTA DE DEBITO' AS Tipo, fa_notaCreDeb.no_fecha as FechaReferencia, 0, 0 as Secuencia, DATEDIFF(day,fa_notaCreDeb.no_fecha,CAST(GETDATE() AS DATE)) Dias, tb_sucursal.Su_Descripcion,
				  ROUND(d.Total - isnull(cobro.dc_ValorPago,0),2) as Saldo,'NTDB'
FROM     fa_notaCreDeb INNER JOIN
                  fa_cliente ON fa_notaCreDeb.IdEmpresa = fa_cliente.IdEmpresa AND fa_notaCreDeb.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  (select IdEmpresa, IdSucursal, IdBodega, IdNota, sum(sc_total) Total from fa_notaCreDeb_det
				  group by IdEmpresa, IdSucursal, IdBodega, IdNota) as d ON fa_notaCreDeb.IdEmpresa = d.IdEmpresa AND fa_notaCreDeb.IdSucursal = d.IdSucursal AND fa_notaCreDeb.IdBodega = d.IdBodega AND 
                  fa_notaCreDeb.IdNota = d.IdNota INNER JOIN 
				  tb_sucursal ON tb_sucursal.IdEmpresa = fa_notaCreDeb.IdEmpresa AND tb_sucursal.IdSucursal = fa_notaCreDeb.IdSucursal LEFT JOIN
				  (
				  SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, ROUND(SUM(D.dc_ValorPago),2) dc_ValorPago
				  FROM cxc_cobro_det AS D INNER JOIN cxc_cobro AS C ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdCobro = D.IdCobro
				  where d.dc_TipoDocumento = 'NTDB' AND C.cr_estado = 'A'
				  group by D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota
				  ) AS Cobro on Cobro.IdEmpresa = fa_notaCreDeb.IdEmpresa and Cobro.IdSucursal = fa_notaCreDeb.IdSucursal and Cobro.IdBodega_Cbte = fa_notaCreDeb.IdBodega and Cobro.IdCbte_vta_nota = fa_notaCreDeb.IdNota
WHERE fa_notaCreDeb.IdEmpresa = @IdEmpresa and
fa_notaCreDeb.CreDeb = 'D' AND fa_notaCreDeb.Estado = 'A' AND 
fa_notaCreDeb.IdSucursal between @IdSucursalIni and @IdSucursalFin
and fa_notaCreDeb.IdCliente between @IdClienteIni and @IdClienteFin and fa_notaCreDeb.no_fecha between @FechaIni and @FechaFin AND ROUND(d.Total - isnull(cobro.dc_ValorPago,0),2) > CASE WHEN @MostrarSaldo0 = 1 THEN -1 ELSE 0 END
UNION ALL
SELECT fa_notaCreDeb.IdEmpresa, fa_notaCreDeb.IdSucursal, fa_notaCreDeb.IdBodega, fa_notaCreDeb.IdNota, CASE WHEN fa_notaCreDeb.NaturalezaNota = 'SRI' THEN fa_notaCreDeb.Serie1+'-'+fa_notaCreDeb.Serie2+'-'+fa_notaCreDeb.NumNota_Impresa ELSE fa_notaCreDeb.CodNota  END AS vt_NumFactura, fa_notaCreDeb.CodDocumentoTipo, fa_notaCreDeb.IdCliente, 
                  fa_notaCreDeb.no_fecha, fa_notaCreDeb.no_fecha_venc, fa_notaCreDeb.sc_observacion, fa_notaCreDeb.Estado, tb_persona.pe_nombreCompleto, cxc_cobro_det.dc_ValorPago * - 1 AS Expr1, 0 AS Debe, cxc_cobro_det.dc_ValorPago AS Haber, 
                  cxc_cobro.cr_NumDocumento AS Referencia, 2 AS Orden, cxc_cobro_tipo.tc_descripcion As Tipo, cxc_cobro.cr_fecha,  cxc_cobro_det.IdCobro, cxc_cobro_det.secuencial, DATEDIFF(day,fa_notaCreDeb.no_fecha,CAST(GETDATE() AS DATE)) Dias, tb_sucursal.Su_Descripcion,
				  ROUND(d.Total - isnull(cobro.dc_ValorPago,0),2) as Saldo, cxc_cobro_det.IdCobro_tipo
FROM     fa_notaCreDeb INNER JOIN
                  fa_cliente ON fa_notaCreDeb.IdEmpresa = fa_cliente.IdEmpresa AND fa_notaCreDeb.IdCliente = fa_cliente.IdCliente INNER JOIN
                  (select IdEmpresa, IdSucursal, IdBodega, IdNota, sum(sc_total) Total from fa_notaCreDeb_det
				  group by IdEmpresa, IdSucursal, IdBodega, IdNota) as d ON fa_notaCreDeb.IdEmpresa = d.IdEmpresa AND fa_notaCreDeb.IdSucursal = d.IdSucursal AND fa_notaCreDeb.IdBodega = d.IdBodega AND 
                  fa_notaCreDeb.IdNota = d.IdNota INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  cxc_cobro_det ON fa_notaCreDeb.IdEmpresa = cxc_cobro_det.IdEmpresa AND fa_notaCreDeb.IdSucursal = cxc_cobro_det.IdSucursal AND fa_notaCreDeb.IdBodega = cxc_cobro_det.IdBodega_Cbte AND 
                  fa_notaCreDeb.IdNota = cxc_cobro_det.IdCbte_vta_nota AND fa_notaCreDeb.CodDocumentoTipo = cxc_cobro_det.dc_TipoDocumento INNER JOIN
                  cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                  cxc_cobro_tipo ON cxc_cobro_det.IdCobro_tipo = cxc_cobro_tipo.IdCobro_tipo INNER JOIN 
				  tb_sucursal ON tb_sucursal.IdEmpresa = fa_notaCreDeb.IdEmpresa AND tb_sucursal.IdSucursal = fa_notaCreDeb.IdSucursal LEFT JOIN
				  (
				  SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, ROUND(SUM(D.dc_ValorPago),2) dc_ValorPago
				  FROM cxc_cobro_det AS D INNER JOIN cxc_cobro AS C ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdCobro = D.IdCobro
				  where d.dc_TipoDocumento = 'NTDB' AND C.cr_estado = 'A'
				  group by D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota
				  ) AS Cobro on Cobro.IdEmpresa = fa_notaCreDeb.IdEmpresa and Cobro.IdSucursal = fa_notaCreDeb.IdSucursal and Cobro.IdBodega_Cbte = fa_notaCreDeb.IdBodega and Cobro.IdCbte_vta_nota = fa_notaCreDeb.IdNota
WHERE cxc_cobro.IdEmpresa = @IdEmpresa and cxc_cobro.cr_estado = 'A' AND fa_notaCreDeb.CreDeb = 'D' AND fa_notaCreDeb.Estado = 'A' AND 
fa_notaCreDeb.IdSucursal between @IdSucursalIni and @IdSucursalFin
and fa_notaCreDeb.IdCliente between @IdClienteIni and @IdClienteFin and fa_notaCreDeb.no_fecha between @FechaIni and @FechaFin AND ROUND(d.Total - isnull(cobro.dc_ValorPago,0),2) > CASE WHEN @MostrarSaldo0 = 1 THEN -1 ELSE 0 END