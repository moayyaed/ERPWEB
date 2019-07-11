CREATE view [web].[vwcxc_cobro_para_retencion]
as
SELECT        dbo.fa_factura_det.IdEmpresa, dbo.fa_factura_det.IdSucursal, dbo.fa_factura_det.IdBodega, dbo.fa_factura_det.IdCbteVta, dbo.fa_factura.vt_tipoDoc, ISNULL(SUM(dbo.fa_factura_det.vt_Subtotal), 0) AS vt_Subtotal, 
                         ISNULL(SUM(dbo.fa_factura_det.vt_iva), 0) AS vt_iva, ISNULL(SUM(dbo.fa_factura_det.vt_total), 0) AS vt_total,  tb_persona.pe_nombreCompleto Nombres, dbo.fa_factura.vt_fecha, dbo.fa_factura.vt_fech_venc, 
                         dbo.fa_factura.vt_Observacion, dbo.fa_factura.vt_tipoDoc + '-' + CAST(CAST(dbo.fa_factura.vt_NumFactura AS numeric) AS varchar(20)) AS vt_NumFactura, dbo.tb_sucursal.Su_Descripcion, 
                         dbo.fa_factura.IdCliente
FROM            dbo.fa_factura INNER JOIN
                         dbo.fa_factura_det ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_det.IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_det.IdSucursal AND dbo.fa_factura.IdBodega = dbo.fa_factura_det.IdBodega AND 
                         dbo.fa_factura.IdCbteVta = dbo.fa_factura_det.IdCbteVta INNER JOIN
                         dbo.tb_sucursal ON dbo.fa_factura_det.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.fa_factura_det.IdSucursal = dbo.tb_sucursal.IdSucursal INNER JOIN
						 fa_cliente on fa_factura.IdEmpresa = fa_cliente.IdEmpresa and fa_factura.IdCliente = fa_cliente.IdCliente inner join
						 tb_persona on fa_cliente.IdPersona = tb_persona.IdPersona
WHERE        (dbo.fa_factura.Estado = 'A')
GROUP BY dbo.fa_factura_det.IdEmpresa, dbo.fa_factura_det.IdSucursal, dbo.fa_factura_det.IdBodega, dbo.fa_factura_det.IdCbteVta, dbo.fa_factura.vt_tipoDoc, tb_persona.pe_nombreCompleto, dbo.fa_factura.vt_fecha, 
                         dbo.fa_factura.vt_fech_venc, dbo.fa_factura.vt_Observacion, dbo.fa_factura.vt_NumFactura, dbo.tb_sucursal.Su_Descripcion, dbo.fa_factura.IdCliente
UNION ALL
SELECT        dbo.fa_notaCreDeb_det.IdEmpresa, dbo.fa_notaCreDeb_det.IdSucursal, dbo.fa_notaCreDeb_det.IdBodega, dbo.fa_notaCreDeb_det.IdNota, dbo.fa_notaCreDeb.CodDocumentoTipo, 
                         ISNULL(SUM(dbo.fa_notaCreDeb_det.sc_subtotal), 0) AS sc_subtotal, ISNULL(SUM(dbo.fa_notaCreDeb_det.sc_iva), 0) AS sc_iva, ISNULL(SUM(dbo.fa_notaCreDeb_det.sc_total), 0) AS sc_total, 
                         dbo.tb_persona.pe_nombreCompleto, dbo.fa_notaCreDeb.no_fecha, dbo.fa_notaCreDeb.no_fecha_venc, dbo.fa_notaCreDeb.sc_observacion, 
                         dbo.fa_notaCreDeb.CodDocumentoTipo + '-' + CASE WHEN fa_notaCreDeb.NumNota_Impresa IS NULL THEN fa_notaCreDeb.CodNota ELSE CAST(CAST(fa_notaCreDeb.NumNota_Impresa AS numeric) AS varchar(20)) 
                         END AS vt_NumFactura, dbo.tb_sucursal.Su_Descripcion, dbo.fa_cliente.IdCliente
FROM            dbo.fa_notaCreDeb INNER JOIN
                         dbo.fa_notaCreDeb_det ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_notaCreDeb_det.IdEmpresa AND dbo.fa_notaCreDeb.IdSucursal = dbo.fa_notaCreDeb_det.IdSucursal AND 
                         dbo.fa_notaCreDeb.IdBodega = dbo.fa_notaCreDeb_det.IdBodega AND dbo.fa_notaCreDeb.IdNota = dbo.fa_notaCreDeb_det.IdNota INNER JOIN
                         dbo.fa_cliente ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_notaCreDeb.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                         dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                         dbo.tb_sucursal ON dbo.fa_notaCreDeb_det.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.fa_notaCreDeb_det.IdSucursal = dbo.tb_sucursal.IdSucursal
WHERE        (dbo.fa_notaCreDeb.CreDeb = 'D')
GROUP BY dbo.fa_notaCreDeb_det.IdEmpresa, dbo.fa_notaCreDeb_det.IdSucursal, dbo.fa_notaCreDeb_det.IdBodega, dbo.fa_notaCreDeb_det.IdNota, dbo.fa_notaCreDeb.CodDocumentoTipo, dbo.tb_persona.pe_nombreCompleto, 
                         dbo.fa_notaCreDeb.no_fecha, dbo.fa_notaCreDeb.no_fecha_venc, dbo.fa_notaCreDeb.sc_observacion, dbo.fa_notaCreDeb.CodNota, dbo.fa_notaCreDeb.NumNota_Impresa, dbo.tb_sucursal.Su_Descripcion, 
                         dbo.fa_cliente.IdCliente