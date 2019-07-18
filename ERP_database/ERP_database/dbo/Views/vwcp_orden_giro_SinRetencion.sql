CREATE VIEW vwcp_orden_giro_SinRetencion
AS
SELECT cp_orden_giro.IdEmpresa, cp_orden_giro.IdTipoCbte_Ogiro, cp_orden_giro.IdCbteCble_Ogiro, tb_persona.pe_nombreCompleto, tb_persona.pe_cedulaRuc, cp_orden_giro.co_fechaOg, cp_orden_giro.co_serie, cp_orden_giro.co_factura, 
                  cp_orden_giro.co_observacion, cp_orden_giro.co_subtotal_iva, cp_orden_giro.co_subtotal_siniva, cp_orden_giro.co_baseImponible, cp_orden_giro.co_valoriva, cp_orden_giro.co_total
FROM     cp_orden_giro INNER JOIN
                  cp_proveedor ON cp_orden_giro.IdEmpresa = cp_proveedor.IdEmpresa AND cp_orden_giro.IdProveedor = cp_proveedor.IdProveedor INNER JOIN
                  tb_persona ON cp_proveedor.IdPersona = tb_persona.IdPersona LEFT OUTER JOIN
                  cp_retencion ON cp_orden_giro.IdEmpresa = cp_retencion.IdEmpresa_Ogiro AND cp_orden_giro.IdCbteCble_Ogiro = cp_retencion.IdCbteCble_Ogiro AND cp_orden_giro.IdTipoCbte_Ogiro = cp_retencion.IdTipoCbte_Ogiro
WHERE  (cp_orden_giro.Estado = 'A') AND (cp_retencion.IdRetencion IS NULL)