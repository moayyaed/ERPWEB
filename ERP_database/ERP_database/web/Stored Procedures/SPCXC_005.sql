﻿--exec [web].[SPCXC_005] 1,1,9999,1,9999,1,9999,'2019/08/05',0
CREATE PROCEDURE [web].[SPCXC_005]
(
@IdEmpresa int,
@IdSucursalIni int,
@IdSucursalFin int,
@IdClienteTipoIni int,
@IdClienteTipoFin int,
@IdClienteIni numeric,
@IdClienteFin numeric,
@FechaCorte datetime,
@MostrarSaldo0 bit
)
AS
SELECT        c.IdEmpresa, c.IdSucursal, c.IdBodega, c.IdCbteVta, c.vt_tipoDoc, C.vt_serie1+'-'+C.vt_serie2+'-'+ c.vt_NumFactura vt_NumFactura, c.IdCliente, RTRIM(LTRIM(tb_persona.pe_nombreCompleto)) AS NomCliente, 
 c.vt_fecha, c.vt_fech_venc, SubtotalConDscto AS Subtotal, ValorIVA AS IVA, Total AS Total, isnull(cobro.ValorPago,0) as Cobrado, ISNULL(NC.ValorPago,0) as NotaCredito, ROUND(D.Total - ISNULL(NC.ValorPago,0) - ISNULL(cobro.ValorPago,0),2) AS Saldo,
 S.Su_Descripcion, fa_cliente.Idtipo_cliente, fa_cliente_tipo.Descripcion_tip_cliente
FROM                     fa_factura AS c INNER JOIN
                         fa_cliente ON c.IdEmpresa = fa_cliente.IdEmpresa AND c.IdCliente = fa_cliente.IdCliente INNER JOIN
                         tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona inner join
						 fa_cliente_tipo on fa_cliente_tipo.IdEmpresa = fa_cliente.IdEmpresa and fa_cliente_tipo.Idtipo_cliente = fa_cliente.Idtipo_cliente inner JOIN 
						 fa_factura_resumen d ON c.IdEmpresa = d.IdEmpresa AND c.IdSucursal = d.IdSucursal AND c.IdBodega = d.IdBodega AND c.IdCbteVta = d.IdCbteVta INNER JOIN 
						 tb_sucursal AS S ON C.IdEmpresa = S.IdEmpresa AND C.IdSucursal = S.IdSucursal
LEFT OUTER JOIN(
SELECT        cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento, SUM(cxc_cobro_det.dc_ValorPago) AS ValorPago
FROM            cxc_cobro_det INNER JOIN
                         cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                         cxc_cobro_tipo ON cxc_cobro_det.IdCobro_tipo = cxc_cobro_tipo.IdCobro_tipo
WHERE        (cxc_cobro_det.estado = 'A') AND (cxc_cobro.cr_estado = N'A') AND (cxc_cobro.cr_fecha <= @FechaCorte) AND (cxc_cobro.IdEmpresa = @IdEmpresa)
and cxc_cobro_tipo.IdMotivo_tipo_cobro <> 'NTCR'
GROUP BY cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento
) AS Cobro on c.IdEmpresa = Cobro.IdEmpresa AND c.IdSucursal = Cobro.IdSucursal AND c.IdBodega = Cobro.IdBodega_Cbte AND c.IdCbteVta = Cobro.IdCbte_vta_nota and cobro.dc_TipoDocumento = c.vt_tipoDoc 

LEFT OUTER JOIN(
SELECT        cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento, SUM(cxc_cobro_det.dc_ValorPago) AS ValorPago
FROM            cxc_cobro_det INNER JOIN
                         cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                         cxc_cobro_tipo ON cxc_cobro_det.IdCobro_tipo = cxc_cobro_tipo.IdCobro_tipo
WHERE        (cxc_cobro_det.estado = 'A') AND (cxc_cobro.cr_estado = N'A') AND (cxc_cobro.cr_fecha <= @FechaCorte) AND (cxc_cobro.IdEmpresa = @IdEmpresa)
and cxc_cobro_tipo.IdMotivo_tipo_cobro = 'NTCR'
GROUP BY cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento
) AS NC on c.IdEmpresa = NC.IdEmpresa AND c.IdSucursal = NC.IdSucursal AND c.IdBodega = NC.IdBodega_Cbte AND c.IdCbteVta = NC.IdCbte_vta_nota and NC.dc_TipoDocumento = c.vt_tipoDoc 

WHERE c.Estado = 'A' and c.IdEmpresa = @IdEmpresa AND C.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin
and c.IdCliente between @IdClienteIni and @IdClienteFin and c.vt_fecha <= @FechaCorte
AND fa_cliente.Idtipo_cliente BETWEEN @IdClienteTipoIni AND @IdClienteTipoFin
and ROUND(D.Total - ISNULL(NC.ValorPago,0) - ISNULL(cobro.ValorPago,0),2) != IIF(@MostrarSaldo0 = 1, -9999999999, 0)

UNION ALL

SELECT        c.IdEmpresa, c.IdSucursal, c.IdBodega, c.IdNota, c.CodDocumentoTipo, 
CASE WHEN C.NaturalezaNota = 'SRI' THEN C.Serie1+'-'+C.Serie2+'-'+C.NumNota_Impresa
ELSE ISNULL(C.CodNota,CAST(C.IdNota as varchar(20))) END, 

c.IdCliente, tb_persona.pe_nombreCompleto AS NomCliente, 
 c.no_fecha, c.no_fecha_venc, d.SubtotalConDscto Subtotal, d.ValorIVA IVA, D.Total, isnull(cobro.ValorPago,0) as Cobrado, ISNULL(NC.ValorPago,0) as NotaCredito, ROUND(D.Total - ISNULL(NC.ValorPago,0) -  ISNULL(cobro.ValorPago,0),2) AS Saldo, S.Su_Descripcion,
 fa_cliente.Idtipo_cliente, fa_cliente_tipo.Descripcion_tip_cliente
FROM           
          fa_notaCreDeb AS c INNER JOIN
                         fa_cliente ON c.IdEmpresa = fa_cliente.IdEmpresa AND c.IdCliente = fa_cliente.IdCliente INNER JOIN
                         tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona inner join
						 fa_cliente_tipo on fa_cliente_tipo.IdEmpresa = fa_cliente.IdEmpresa and fa_cliente_tipo.Idtipo_cliente = fa_cliente.Idtipo_cliente LEFT OUTER JOIN
                             fa_notaCreDeb_resumen AS d ON c.IdEmpresa = d.IdEmpresa AND c.IdSucursal = d.IdSucursal AND c.IdBodega = d.IdBodega AND c.IdNota = d.IdNota
							   INNER JOIN tb_sucursal AS S ON C.IdEmpresa = S.IdEmpresa AND C.IdSucursal = S.IdSucursal

LEFT OUTER JOIN(
SELECT        cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento, SUM(cxc_cobro_det.dc_ValorPago) AS ValorPago
FROM            cxc_cobro_det INNER JOIN
                         cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                         cxc_cobro_tipo ON cxc_cobro_det.IdCobro_tipo = cxc_cobro_tipo.IdCobro_tipo
WHERE        (cxc_cobro_det.estado = 'A') AND (cxc_cobro.cr_estado = N'A') AND (cxc_cobro.cr_fecha <= @FechaCorte) AND (cxc_cobro.IdEmpresa = @IdEmpresa)
and cxc_cobro_tipo.IdMotivo_tipo_cobro <> 'NTCR'
GROUP BY cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento
) AS Cobro on c.IdEmpresa = Cobro.IdEmpresa AND c.IdSucursal = Cobro.IdSucursal AND c.IdBodega = Cobro.IdBodega_Cbte AND c.IdNota = Cobro.IdCbte_vta_nota and cobro.dc_TipoDocumento = c.CodDocumentoTipo 

LEFT OUTER JOIN(
SELECT        cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento, SUM(cxc_cobro_det.dc_ValorPago) AS ValorPago
FROM            cxc_cobro_det INNER JOIN
                         cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                         cxc_cobro_tipo ON cxc_cobro_det.IdCobro_tipo = cxc_cobro_tipo.IdCobro_tipo
WHERE        (cxc_cobro_det.estado = 'A') AND (cxc_cobro.cr_estado = N'A') AND (cxc_cobro.cr_fecha <= @FechaCorte) AND (cxc_cobro.IdEmpresa = @IdEmpresa)
and cxc_cobro_tipo.IdMotivo_tipo_cobro = 'NTCR'
GROUP BY cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdBodega_Cbte, cxc_cobro_det.IdCbte_vta_nota, cxc_cobro_det.dc_TipoDocumento
) AS NC on c.IdEmpresa = NC.IdEmpresa AND c.IdSucursal = NC.IdSucursal AND c.IdBodega = NC.IdBodega_Cbte AND c.IdNota = NC.IdCbte_vta_nota and NC.dc_TipoDocumento = c.CodDocumentoTipo 
WHERE c.Estado = 'A' and c.CreDeb = 'D' and c.IdEmpresa = @IdEmpresa AND C.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin
and c.IdCliente between @IdClienteIni and @IdClienteFin and c.no_fecha <= @FechaCorte
AND fa_cliente.Idtipo_cliente BETWEEN @IdClienteTipoIni AND @IdClienteTipoFin
and ROUND(D.Total - ISNULL(NC.ValorPago,0) - ISNULL(cobro.ValorPago,0),2) != IIF(@MostrarSaldo0 = 1, -9999999999, 0)