CREATE VIEW [dbo].[vwfa_factura_ParaContabilizar]
AS
SELECT dbo.fa_factura_det.IdEmpresa, dbo.fa_factura_det.IdSucursal, dbo.fa_factura_det.IdBodega, dbo.fa_factura_det.IdCbteVta, dbo.fa_factura_det.Secuencia, dbo.fa_factura_det.vt_iva, dbo.fa_factura_det.vt_Subtotal, 
                  dbo.fa_factura_det.vt_total, dbo.fa_factura_det.IdCentroCosto, dbo.fa_factura_det.vt_por_iva, dbo.tb_sis_Impuesto_x_ctacble.IdCtaCble AS IdCtaCbleIva, case when pxb.IdCtaCble_Vta is null then dbo.tb_sis_Impuesto_x_ctacble.IdCtaCble_vta else pxb.IdCtaCble_Vta end AS IdCtaCble_vta, dbo.fa_factura.vt_fecha, 
                  dbo.tb_persona.pe_nombreCompleto, dbo.fa_cliente.IdCtaCble_cxc_Credito, dbo.fa_factura.vt_serie1, dbo.fa_factura.vt_serie2, dbo.fa_factura.vt_NumFactura, dbo.fa_factura.vt_Observacion, 
                  dbo.fa_factura_resumen.SubtotalConDscto AS RSubtotal, dbo.fa_factura_resumen.ValorIVA AS RValorIva, dbo.fa_factura_resumen.Total AS RTotal, 
                  dbo.fa_factura_det.vt_cantidad * dbo.fa_factura_det.vt_Precio AS vt_SubtotalSinDscto, dbo.fa_factura_det.vt_cantidad * dbo.fa_factura_det.vt_DescUnitario AS vt_DescuentoTotal
FROM     dbo.fa_factura_det INNER JOIN
                  dbo.fa_factura ON dbo.fa_factura_det.IdEmpresa = dbo.fa_factura.IdEmpresa AND dbo.fa_factura_det.IdSucursal = dbo.fa_factura.IdSucursal AND dbo.fa_factura_det.IdBodega = dbo.fa_factura.IdBodega AND 
                  dbo.fa_factura_det.IdCbteVta = dbo.fa_factura.IdCbteVta INNER JOIN
                  dbo.fa_cliente ON dbo.fa_factura.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_factura.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.fa_factura_resumen ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_resumen.IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_resumen.IdSucursal AND dbo.fa_factura.IdBodega = dbo.fa_factura_resumen.IdBodega AND 
                  dbo.fa_factura.IdCbteVta = dbo.fa_factura_resumen.IdCbteVta LEFT OUTER JOIN
                  dbo.tb_sis_Impuesto_x_ctacble ON dbo.fa_factura_det.IdCod_Impuesto_Iva = dbo.tb_sis_Impuesto_x_ctacble.IdCod_Impuesto AND dbo.fa_factura_det.IdEmpresa = dbo.tb_sis_Impuesto_x_ctacble.IdEmpresa_cta left join
				  in_producto_x_tb_bodega as pxb on fa_factura_det.IdEmpresa = pxb.IdEmpresa and fa_factura_det.IdSucursal = pxb.IdSucursal and fa_factura_det.IdBodega = pxb.IdBodega and fa_factura_det.IdProducto = pxb.IdProducto