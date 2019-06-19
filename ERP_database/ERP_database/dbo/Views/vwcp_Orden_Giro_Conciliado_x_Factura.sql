CREATE VIEW [dbo].[vwcp_Orden_Giro_Conciliado_x_Factura] AS
SELECT        og.IdEmpresa, og.IdCbteCble_Ogiro, og.IdTipoCbte_Ogiro, og.IdOrden_giro_Tipo, conci.IdEmpresa_Apro_Ing, conci.IdAprobacion_Apro_Ing ,
						 conci.IdConciliacion, conci.IdEmpresa AS IdEmpresaConciliacion ,og.IdProveedor, per.pe_nombreCompleto AS nombreProveedor, og.co_fechaOg, 
                         og.co_factura, og.co_FechaFactura, og.co_observacion, og.co_subtotal_iva, og.co_subtotal_siniva, og.co_baseImponible, og.co_total, og.Estado, 
                         rete.IdEmpresa AS IdEmpresa_ret, rete.IdRetencion, rete.serie1 +'-'+ rete.serie2 AS re_serie, rete.NumRetencion AS re_NumRetencion, rete.re_EstaImpresa, 
                         null AS TipoFlujo, og.IdIden_credito, (CASE WHEN (rtrim(ltrim(og.co_serie)) = '' OR
                         rtrim(ltrim(og.co_serie)) IS NULL) THEN '000' ELSE (substring(og.co_serie, 1, 3)) END) AS Serie, (CASE WHEN (rtrim(ltrim(og.co_serie)) = '' OR
                         rtrim(ltrim(og.co_serie)) IS NULL) THEN '000' ELSE (substring(og.co_serie, 5, 3)) END) AS Serie2, og.co_factura AS numDocFactura, og.Num_Autorizacion, 
                         og.Num_Autorizacion_Imprenta, 0 co_OtroValor_a_descontar, og.co_Por_iva, og.co_valoriva, og.fecha_autorizacion, '' IdCtaCble_Gasto, '' IdCtaCble_IVA
FROM            dbo.cp_orden_giro AS og INNER JOIN
                         dbo.cp_proveedor AS pro ON og.IdEmpresa = pro.IdEmpresa AND og.IdProveedor = pro.IdProveedor INNER JOIN 
						  dbo.cp_Aprobacion_Ing_Bod_x_OC AS apro ON apro.IdEmpresa_Ogiro = og.IdEmpresa AND apro.IdCbteCble_Ogiro = og.IdCbteCble_Ogiro AND 
						  apro.IdTipoCbte_Ogiro = og.IdTipoCbte_Ogiro AND  apro.IdOrden_giro_Tipo = og.IdOrden_giro_Tipo INNER JOIN 
						  cp_Conciliacion_Ing_Bodega_x_Orden_Giro conci ON conci.IdEmpresa_Apro_Ing = apro.IdEmpresa AND conci.IdAprobacion_Apro_Ing = apro.IdAprobacion LEFT OUTER JOIN
                         dbo.cp_retencion AS rete ON og.IdEmpresa = rete.IdEmpresa_Ogiro AND og.IdCbteCble_Ogiro = rete.IdCbteCble_Ogiro AND 
                         og.IdTipoCbte_Ogiro = rete.IdTipoCbte_Ogiro LEFT OUTER JOIN
                         
                         dbo.tb_sucursal AS su ON og.IdSucursal = su.IdSucursal AND og.IdEmpresa = su.IdEmpresa inner join tb_persona
						 as per on per.IdPersona = pro.IdPersona