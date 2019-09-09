--exec [web].[SPCXC_009] 1,70,'22/08/2019'
CREATE PROCEDURE [web].[SPCXC_009]
(
@IdEmpresa int,
@IdCliente numeric,
@FechaCorte DATE
)
AS

SELECT fa_factura.IdEmpresa, fa_factura.IdSucursal, fa_factura.IdBodega, fa_factura.IdCbteVta,fa_factura.vt_tipoDoc, 
fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura AS NumDocumento, fa_factura.vt_fecha, fa_factura.vt_fech_venc, 
                  tb_sucursal.Su_Descripcion, fa_factura.IdCliente, tb_persona.pe_nombreCompleto, tb_persona.pe_cedulaRuc, fa_factura.IdVendedor, fa_Vendedor.Ve_Vendedor, fa_factura_resumen.Total, CASE WHEN DATEDIFF(DAY,fa_factura.vt_fech_venc,@FechaCorte) < 0 THEN 0 ELSE DATEDIFF(DAY,fa_factura.vt_fech_venc,@FechaCorte) END DiasVencido,
				  isnull(COBRO.dc_ValorPago,0) TotalCobrado, round(fa_factura_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) as Saldo, fa_factura.vt_Observacion,
				  CASE WHEN DATEDIFF(DAY,fa_factura.vt_fech_venc,@FechaCorte) < 0  THEN round(fa_factura_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS X_VENCER,
				  CASE WHEN DATEDIFF(DAY,fa_factura.vt_fech_venc,@FechaCorte) BETWEEN 1 AND 30  THEN round(fa_factura_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO30, 
				  CASE WHEN DATEDIFF(DAY,fa_factura.vt_fech_venc,@FechaCorte) BETWEEN 31 AND 60 THEN round(fa_factura_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO60,
				  CASE WHEN DATEDIFF(DAY,fa_factura.vt_fech_venc,@FechaCorte) BETWEEN 61 AND 90 THEN round(fa_factura_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO90,
				  CASE WHEN DATEDIFF(DAY,fa_factura.vt_fech_venc,@FechaCorte) >= 91 THEN round(fa_factura_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO90MAS
FROM     fa_factura INNER JOIN
                  fa_Vendedor ON fa_factura.IdEmpresa = fa_Vendedor.IdEmpresa AND fa_factura.IdVendedor = fa_Vendedor.IdVendedor INNER JOIN
                  fa_cliente ON fa_factura.IdEmpresa = fa_cliente.IdEmpresa AND fa_factura.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  tb_sucursal ON fa_factura.IdEmpresa = tb_sucursal.IdEmpresa AND fa_factura.IdSucursal = tb_sucursal.IdSucursal LEFT OUTER JOIN
                  fa_factura_resumen ON fa_factura.IdEmpresa = fa_factura_resumen.IdEmpresa AND fa_factura.IdSucursal = fa_factura_resumen.IdSucursal AND fa_factura.IdBodega = fa_factura_resumen.IdBodega AND 
                  fa_factura.IdCbteVta = fa_factura_resumen.IdCbteVta left join
				  (
				  select d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento, SUM(D.dc_ValorPago)dc_ValorPago
				  from cxc_cobro_det as d inner join cxc_cobro as c
				  on c.IdEmpresa = d.IdEmpresa and c.IdSucursal = d.IdSucursal and c.IdCobro = d.IdCobro
				  where d.IdEmpresa = @IdEmpresa and c.cr_fecha <= @FechaCorte and c.cr_estado = 'A'
				  GROUP BY d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento
				  ) as Cobro on fa_factura.IdEmpresa = COBRO.IdEmpresa AND fa_factura.IdSucursal = COBRO.IdSucursal AND fa_factura.IdBodega = COBRO.IdBodega_Cbte AND fa_factura.IdCbteVta = COBRO.IdCbte_vta_nota AND fa_factura.vt_tipoDoc = COBRO.dc_TipoDocumento
where fa_factura.IdEmpresa = @IdEmpresa and fa_factura.IdCliente = @IdCliente 
AND fa_factura.Estado = 'A' 
and round(fa_factura_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) >  0
UNION ALL
SELECT fa_notaCreDeb.IdEmpresa, fa_notaCreDeb.IdSucursal, fa_notaCreDeb.IdBodega, fa_notaCreDeb.IdNota,fa_notaCreDeb.CodDocumentoTipo, 
isnull(fa_notaCreDeb.Serie1 + '-' + fa_notaCreDeb.Serie2 + '-' + fa_notaCreDeb.NumNota_Impresa, fa_notaCreDeb.CodNota)AS NumDocumento, fa_notaCreDeb.no_fecha, fa_notaCreDeb.no_fecha_venc, 
                  tb_sucursal.Su_Descripcion, fa_notaCreDeb.IdCliente, tb_persona.pe_nombreCompleto, tb_persona.pe_cedulaRuc, 0, '' Ve_Vendedor, fa_notaCreDeb_resumen.Total, CASE WHEN DATEDIFF(DAY,fa_notaCreDeb.no_fecha_venc,@FechaCorte) < 0 THEN 0 ELSE DATEDIFF(DAY,fa_notaCreDeb.no_fecha_venc,@FechaCorte) END DiasVencido,
				  isnull(COBRO.dc_ValorPago,0) TotalCobrado, round(fa_notaCreDeb_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) as Saldo, fa_notaCreDeb.sc_observacion,
				  CASE WHEN DATEDIFF(DAY,fa_notaCreDeb.no_fecha_venc,@FechaCorte) < 0  THEN round(fa_notaCreDeb_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS X_VENCER,
				  CASE WHEN DATEDIFF(DAY,fa_notaCreDeb.no_fecha_venc,@FechaCorte) BETWEEN 1 AND 30  THEN round(fa_notaCreDeb_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO30, 
				  CASE WHEN DATEDIFF(DAY,fa_notaCreDeb.no_fecha_venc,@FechaCorte) BETWEEN 31 AND 60 THEN round(fa_notaCreDeb_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO60,
				  CASE WHEN DATEDIFF(DAY,fa_notaCreDeb.no_fecha_venc,@FechaCorte) BETWEEN 61 AND 90 THEN round(fa_notaCreDeb_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO90,
				  CASE WHEN DATEDIFF(DAY,fa_notaCreDeb.no_fecha_venc,@FechaCorte) >= 91 THEN round(fa_notaCreDeb_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) ELSE 0 END AS VENCIDO90MAS
FROM     fa_notaCreDeb INNER JOIN
                  fa_cliente ON fa_notaCreDeb.IdEmpresa = fa_cliente.IdEmpresa AND fa_notaCreDeb.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  tb_sucursal ON fa_notaCreDeb.IdEmpresa = tb_sucursal.IdEmpresa AND fa_notaCreDeb.IdSucursal = tb_sucursal.IdSucursal LEFT OUTER JOIN
                  fa_notaCreDeb_resumen ON fa_notaCreDeb.IdEmpresa = fa_notaCreDeb_resumen.IdEmpresa AND fa_notaCreDeb.IdSucursal = fa_notaCreDeb_resumen.IdSucursal AND fa_notaCreDeb.IdBodega = fa_notaCreDeb_resumen.IdBodega AND 
                  fa_notaCreDeb.IdNota = fa_notaCreDeb_resumen.IdNota left join
				  (
				  select d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento, SUM(D.dc_ValorPago)dc_ValorPago
				  from cxc_cobro_det as d inner join cxc_cobro as c
				  on c.IdEmpresa = d.IdEmpresa and c.IdSucursal = d.IdSucursal and c.IdCobro = d.IdCobro
				  where d.IdEmpresa = @IdEmpresa and c.cr_fecha <= @FechaCorte and c.cr_estado = 'A'
				  GROUP BY d.IdEmpresa, d.IdSucursal, d.IdBodega_Cbte, d.IdCbte_vta_nota, d.dc_TipoDocumento
				  ) as Cobro on fa_notaCreDeb.IdEmpresa = COBRO.IdEmpresa AND fa_notaCreDeb.IdSucursal = COBRO.IdSucursal AND fa_notaCreDeb.IdBodega = COBRO.IdBodega_Cbte AND fa_notaCreDeb.IdNota = COBRO.IdCbte_vta_nota AND fa_notaCreDeb.CodDocumentoTipo = COBRO.dc_TipoDocumento
where fa_notaCreDeb.IdEmpresa = @IdEmpresa and fa_notaCreDeb.IdCliente = @IdCliente 
AND fa_notaCreDeb.Estado = 'A' and fa_notaCreDeb.CreDeb = 'D'
and round(fa_notaCreDeb_resumen.Total - isnull(COBRO.dc_ValorPago,0),2) >  0