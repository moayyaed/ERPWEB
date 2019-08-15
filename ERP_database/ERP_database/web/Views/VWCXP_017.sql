CREATE VIEW [web].[VWCXP_017]
AS
SELECT OP.IdEmpresa, OP.IdOrdenPago, OP.IdTipo_op, OP.IdTipo_Persona, OP.IdPersona, OP.IdEntidad, OP.Fecha, OP.IdEstadoAprobacion, OP.IdFormaPago, OP.Estado, dbo.tb_persona.pe_nombreCompleto, CAN.Total_OP, 
                  ISNULL(PAGO.Total_cancelado, 0) AS Total_cancelado, ROUND(CAN.Total_OP - ISNULL(PAGO.Total_cancelado, 0), 2) AS Saldo, OP.Observacion, NULL AS IdTipoFlujo, '' AS nom_tipoFlujo, 
                  CASE WHEN round(CAN.Total_OP - ISNULL(PAGO.Total_cancelado, 0), 2) > 0 THEN 'PENDIENTE' WHEN round(CAN.Total_OP - ISNULL(PAGO.Total_cancelado, 0), 2) <= 0 THEN 'CANCELADA' END AS EstadoCancelacion, 
                  dbo.cp_orden_pago_estado_aprob.Descripcion, OP.IdSucursal, dbo.tb_sucursal.Su_Descripcion, 
				  CASE WHEN cp_orden_giro.co_factura IS NULL AND cp_nota_DebCre.cod_nota IS NOT NULL 
                  THEN cp_nota_DebCre.cod_nota WHEN cp_orden_giro.co_factura IS NOT NULL AND cp_nota_DebCre.cod_nota IS NULL THEN 
				   cp_orden_giro.co_serie+'-'+ cp_orden_giro.co_factura ELSE OP.IdTipo_op END AS Referencia
FROM     dbo.cp_orden_pago AS OP INNER JOIN
                  dbo.tb_persona ON OP.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.vwcp_orden_pago_total AS CAN ON OP.IdEmpresa = CAN.IdEmpresa AND OP.IdOrdenPago = CAN.IdOrdenPago INNER JOIN
                  dbo.cp_orden_pago_estado_aprob ON OP.IdEstadoAprobacion = dbo.cp_orden_pago_estado_aprob.IdEstadoAprobacion INNER JOIN
                  dbo.cp_orden_pago_det ON OP.IdEmpresa = dbo.cp_orden_pago_det.IdEmpresa AND OP.IdOrdenPago = dbo.cp_orden_pago_det.IdOrdenPago LEFT OUTER JOIN
                  dbo.cp_nota_DebCre ON dbo.cp_orden_pago_det.IdEmpresa_cxp = dbo.cp_nota_DebCre.IdEmpresa AND dbo.cp_orden_pago_det.IdCbteCble_cxp = dbo.cp_nota_DebCre.IdCbteCble_Nota AND 
                  dbo.cp_orden_pago_det.IdTipoCbte_cxp = dbo.cp_nota_DebCre.IdTipoCbte_Nota LEFT OUTER JOIN
                  dbo.cp_orden_giro ON dbo.cp_orden_pago_det.IdEmpresa_cxp = dbo.cp_orden_giro.IdEmpresa AND dbo.cp_orden_pago_det.IdCbteCble_cxp = dbo.cp_orden_giro.IdCbteCble_Ogiro AND 
                  dbo.cp_orden_pago_det.IdTipoCbte_cxp = dbo.cp_orden_giro.IdTipoCbte_Ogiro LEFT OUTER JOIN
                  dbo.tb_sucursal ON OP.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND OP.IdSucursal = dbo.tb_sucursal.IdSucursal LEFT OUTER JOIN
                  dbo.vwcp_orden_pago_Total_Pagado AS PAGO ON CAN.IdEmpresa = PAGO.IdEmpresa_op AND CAN.IdOrdenPago = PAGO.IdOrdenPago_op