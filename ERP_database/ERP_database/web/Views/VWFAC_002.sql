CREATE VIEW web.VWFAC_002
AS
SELECT fa_notaCreDeb_det.IdEmpresa, fa_notaCreDeb_det.IdSucursal, fa_notaCreDeb_det.IdBodega, fa_notaCreDeb_det.IdNota, fa_notaCreDeb_det.Secuencia, fa_notaCreDeb.Serie1, fa_notaCreDeb.Serie2, fa_notaCreDeb.NumNota_Impresa, 
                  tb_persona.pe_nombreCompleto, tb_persona.pe_cedulaRuc, fa_notaCreDeb.no_fecha, ISNULL(cr_fa.vt_fecha, cr_nd.no_fecha) AS FechaDocumentoAplica, fa_notaCreDeb.sc_observacion, fa_notaCreDeb_det.IdProducto, 
                  in_Producto.pr_codigo, in_Producto.pr_descripcion, fa_notaCreDeb_det.sc_observacion AS DetalleAdicional, fa_notaCreDeb_det.sc_precioFinal, fa_cliente_contactos.Telefono, fa_cliente_contactos.Celular, 
                  fa_cliente_contactos.Correo, fa_cliente_contactos.Direccion, case when fa_notaCreDeb_det.vt_por_iva > 0 then fa_notaCreDeb_det.sc_cantidad * fa_notaCreDeb_det.sc_Precio else 0 end as SubtotalIva,
				  case when fa_notaCreDeb_det.vt_por_iva = 0 then fa_notaCreDeb_det.sc_cantidad * fa_notaCreDeb_det.sc_Precio else 0 end as SubtotalSinIva, fa_notaCreDeb_det.sc_cantidad * fa_notaCreDeb_det.sc_Precio SubtotalAntesDescuento, fa_notaCreDeb_det.sc_subtotal, fa_notaCreDeb_det.sc_cantidad * fa_notaCreDeb_det.sc_descUni as TotalDescuento,
				  fa_notaCreDeb_det.sc_iva, fa_notaCreDeb_det.sc_total, case when cr_fa.vt_NumFactura is null then case when fa_notaCreDeb.NaturalezaNota = 'SRI' THEN fa_notaCreDeb.Serie1+'-'+fa_notaCreDeb.Serie2+'-'+fa_notaCreDeb.NumNota_Impresa ELSE ISNULL(fa_notaCreDeb.CodNota,CAST(fa_notaCreDeb.IdNota as varchar(20))) end else cr_fa.vt_serie1+'-'+cr_fa.vt_serie2+'-'+cr_fa.vt_NumFactura end as DocumentoAplicado,
				  fa_notaCreDeb.NumAutorizacion, fa_notaCreDeb.Fecha_Autorizacion
FROM     fa_notaCreDeb INNER JOIN
                  fa_notaCreDeb_det ON fa_notaCreDeb.IdEmpresa = fa_notaCreDeb_det.IdEmpresa AND fa_notaCreDeb.IdSucursal = fa_notaCreDeb_det.IdSucursal AND fa_notaCreDeb.IdBodega = fa_notaCreDeb_det.IdBodega AND 
                  fa_notaCreDeb.IdNota = fa_notaCreDeb_det.IdNota INNER JOIN
                  fa_cliente ON fa_notaCreDeb.IdEmpresa = fa_cliente.IdEmpresa AND fa_notaCreDeb.IdCliente = fa_cliente.IdCliente INNER JOIN
                  tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                  fa_notaCreDeb_x_fa_factura_NotaDeb ON fa_notaCreDeb.IdEmpresa = fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_nt AND fa_notaCreDeb.IdSucursal = fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_nt AND 
                  fa_notaCreDeb.IdBodega = fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_nt AND fa_notaCreDeb.IdNota = fa_notaCreDeb_x_fa_factura_NotaDeb.IdNota_nt INNER JOIN
                  in_Producto ON fa_notaCreDeb_det.IdEmpresa = in_Producto.IdEmpresa AND fa_notaCreDeb_det.IdProducto = in_Producto.IdProducto INNER JOIN
                  fa_cliente_contactos ON fa_cliente.IdEmpresa = fa_cliente_contactos.IdEmpresa AND fa_cliente.IdCliente = fa_cliente_contactos.IdCliente LEFT OUTER JOIN
                  fa_factura AS cr_fa ON fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_fac_nd_doc_mod = cr_fa.IdEmpresa AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_fac_nd_doc_mod = cr_fa.IdSucursal AND 
                  fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_fac_nd_doc_mod = cr_fa.IdBodega AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdCbteVta_fac_nd_doc_mod = cr_fa.IdCbteVta AND 
                  fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = cr_fa.vt_tipoDoc LEFT OUTER JOIN
                  fa_notaCreDeb AS cr_nd ON fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_fac_nd_doc_mod = cr_nd.IdEmpresa AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_fac_nd_doc_mod = cr_nd.IdSucursal AND 
                  fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_fac_nd_doc_mod = cr_nd.IdBodega AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdCbteVta_fac_nd_doc_mod = cr_nd.IdNota AND 
                  fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = cr_nd.CodDocumentoTipo