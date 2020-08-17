CREATE VIEW web.VWCXP_005_cancelaciones
AS
SELECT dbo.cp_orden_pago_cancelaciones.IdEmpresa, dbo.cp_orden_pago_cancelaciones.Idcancelacion, dbo.cp_orden_pago_cancelaciones.Secuencia, dbo.cp_orden_pago_cancelaciones.IdEmpresa_cxp, 
                  dbo.cp_orden_pago_cancelaciones.IdTipoCbte_cxp, dbo.cp_orden_pago_cancelaciones.IdCbteCble_cxp, CASE WHEN cp_orden_giro.co_factura IS NULL THEN 'ND ' + CAST(CAST(cp_nota_DebCre.cod_nota AS INT) AS VARCHAR(20)) 
                  ELSE 'FACT ' + CAST(CAST(cp_orden_giro.co_factura AS INT) AS VARCHAR(20)) END AS Referencia, CASE WHEN cp_orden_giro.co_factura IS NULL 
                  THEN cp_nota_DebCre.cn_observacion ELSE cp_orden_giro.co_observacion END AS Observacion, dbo.cp_orden_pago_cancelaciones.MontoAplicado, dbo.cp_ConciliacionAnticipo.IdEmpresa AS IdEmpresa_conciliacion, 
                  dbo.cp_ConciliacionAnticipo.IdConciliacion
FROM     dbo.cp_orden_pago_cancelaciones INNER JOIN
                  dbo.cp_ConciliacionAnticipo ON dbo.cp_orden_pago_cancelaciones.IdEmpresa_pago = dbo.cp_ConciliacionAnticipo.IdEmpresa AND dbo.cp_orden_pago_cancelaciones.IdTipoCbte_pago = dbo.cp_ConciliacionAnticipo.IdTipoCbte AND 
                  dbo.cp_orden_pago_cancelaciones.IdCbteCble_pago = dbo.cp_ConciliacionAnticipo.IdCbteCble LEFT OUTER JOIN
                  dbo.cp_nota_DebCre ON dbo.cp_orden_pago_cancelaciones.IdEmpresa_cxp = dbo.cp_nota_DebCre.IdEmpresa AND dbo.cp_orden_pago_cancelaciones.IdCbteCble_cxp = dbo.cp_nota_DebCre.IdCbteCble_Nota AND 
                  dbo.cp_orden_pago_cancelaciones.IdTipoCbte_cxp = dbo.cp_nota_DebCre.IdTipoCbte_Nota LEFT OUTER JOIN
                  dbo.cp_orden_giro ON dbo.cp_orden_pago_cancelaciones.IdEmpresa_cxp = dbo.cp_orden_giro.IdEmpresa AND dbo.cp_orden_pago_cancelaciones.IdTipoCbte_cxp = dbo.cp_orden_giro.IdTipoCbte_Ogiro AND 
                  dbo.cp_orden_pago_cancelaciones.IdCbteCble_cxp = dbo.cp_orden_giro.IdCbteCble_Ogiro

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXP_005_cancelaciones';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[90] 4[3] 2[3] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cp_orden_pago_cancelaciones"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 552
               Right = 300
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_ConciliacionAnticipo"
            Begin Extent = 
               Top = 0
               Left = 493
               Bottom = 319
               Right = 738
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_nota_DebCre"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 307
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_orden_giro"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 355
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXP_005_cancelaciones';

