CREATE view [dbo].[vwcp_orden_giro_LiquidacionDeCompras]
as
SELECT a.IdEmpresa, a.IdTipoCbte_Ogiro, a.IdCbteCble_Ogiro, a.co_FechaFactura, a.co_factura, d.pe_nombreCompleto, a.Num_Autorizacion, b.Codigo, a.Estado, a.co_total, a.co_observacion, a.IdSucursal, b.Descripcion, a.fecha_autorizacion, 
                  a.co_FechaContabilizacion
FROM     dbo.cp_orden_giro AS a INNER JOIN
                  dbo.cp_TipoDocumento AS b ON a.IdOrden_giro_Tipo = b.CodTipoDocumento AND YEAR(a.co_FechaFactura) >= YEAR(b.FechaInicioTalonario) INNER JOIN
                  dbo.cp_proveedor AS c ON a.IdEmpresa = c.IdEmpresa AND a.IdProveedor = c.IdProveedor INNER JOIN
                  dbo.tb_persona AS d ON c.IdPersona = d.IdPersona INNER JOIN
                  dbo.tb_sis_Documento_Tipo_Talonario AS e ON a.IdEmpresa = e.IdEmpresa AND a.co_serie = e.Establecimiento + '-' + e.PuntoEmision AND e.NumDocumento = a.co_factura AND b.Codigo = e.CodDocumentoTipo
WHERE  (b.ManejaTalonario = 1) AND (e.es_Documento_Electronico = 1)