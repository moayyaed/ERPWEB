﻿CREATE VIEW [web].[VWCXP_003_cancelaciones]
AS
SELECT dbo.cp_orden_pago_cancelaciones.IdEmpresa, dbo.cp_orden_pago_cancelaciones.Idcancelacion, dbo.cp_orden_pago_cancelaciones.Secuencia, dbo.cp_orden_pago_cancelaciones.IdEmpresa_cxp, 
                  dbo.cp_orden_pago_cancelaciones.IdTipoCbte_cxp, dbo.cp_orden_pago_cancelaciones.IdCbteCble_cxp, CASE WHEN cp_orden_giro.co_factura IS NULL AND cp_nota_DebCre.cod_nota IS NULL 
                  THEN 'OP ' + CAST(cp_orden_pago_cancelaciones.IdOrdenPago_op AS VARCHAR(50)) WHEN cp_orden_giro.co_factura IS NULL AND cp_nota_DebCre.cod_nota IS NOT NULL THEN 'ND ' + CAST(CAST(cp_nota_DebCre.cod_nota AS numeric) 
                  AS VARCHAR) ELSE 'FACT ' + CAST(CAST(cp_orden_giro.co_factura AS numeric) AS VARCHAR(50)) END AS Referencia, CASE WHEN cp_orden_giro.co_factura IS NULL AND cp_nota_DebCre.cod_nota IS NULL 
                  THEN cp_orden_pago.Observacion WHEN cp_orden_giro.co_factura IS NULL AND cp_nota_DebCre.cod_nota IS NOT NULL THEN cp_nota_DebCre.cn_observacion ELSE cp_orden_giro.co_observacion END AS Observacion, 
                  dbo.cp_orden_pago_cancelaciones.MontoAplicado, dbo.cp_orden_pago_cancelaciones.IdEmpresa_pago, dbo.cp_orden_pago_cancelaciones.IdTipoCbte_pago, dbo.cp_orden_pago_cancelaciones.IdCbteCble_pago
FROM     dbo.cp_orden_pago_cancelaciones INNER JOIN
                  dbo.cp_nota_DebCre ON dbo.cp_orden_pago_cancelaciones.IdEmpresa_pago = dbo.cp_nota_DebCre.IdEmpresa AND dbo.cp_orden_pago_cancelaciones.IdTipoCbte_pago = dbo.cp_nota_DebCre.IdTipoCbte_Nota AND 
                  dbo.cp_orden_pago_cancelaciones.IdCbteCble_pago = dbo.cp_nota_DebCre.IdCbteCble_Nota INNER JOIN
                  dbo.cp_orden_pago ON dbo.cp_orden_pago_cancelaciones.IdEmpresa_op = dbo.cp_orden_pago.IdEmpresa AND dbo.cp_orden_pago_cancelaciones.IdOrdenPago_op = dbo.cp_orden_pago.IdOrdenPago LEFT OUTER JOIN
                  dbo.cp_nota_DebCre AS cp_nota_DebCre_1 ON dbo.cp_orden_pago_cancelaciones.IdEmpresa_cxp = cp_nota_DebCre_1.IdEmpresa AND dbo.cp_orden_pago_cancelaciones.IdCbteCble_cxp = cp_nota_DebCre_1.IdCbteCble_Nota AND 
                  dbo.cp_orden_pago_cancelaciones.IdTipoCbte_cxp = cp_nota_DebCre_1.IdTipoCbte_Nota LEFT OUTER JOIN
                  dbo.cp_orden_giro ON dbo.cp_orden_pago_cancelaciones.IdEmpresa_cxp = dbo.cp_orden_giro.IdEmpresa AND dbo.cp_orden_pago_cancelaciones.IdTipoCbte_cxp = dbo.cp_orden_giro.IdTipoCbte_Ogiro AND 
                  dbo.cp_orden_pago_cancelaciones.IdCbteCble_cxp = dbo.cp_orden_giro.IdCbteCble_Ogiro
