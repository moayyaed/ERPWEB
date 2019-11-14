CREATE VIEW [web].[VWBAN_002_cancelaciones]
AS
SELECT dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mba_IdEmpresa, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mba_IdCbteCble, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mba_IdTipocbte, 
                  dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdEmpresa, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdCbteCble, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdTipocbte, 
                  dbo.caj_Caja_Movimiento.cm_observacion, dbo.caj_Caja_Movimiento.cm_fecha, dbo.caj_Caja_Movimiento.cm_valor, isnull(dbo.cxc_cobro_x_ct_cbtecble.cbr_IdCobro,0) cbr_IdCobro, caj_Caja_Movimiento.IdCbteCble ct_IdCbteCble
FROM     dbo.caj_Caja_Movimiento INNER JOIN
                  dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito ON dbo.caj_Caja_Movimiento.IdEmpresa = dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdEmpresa AND 
                  dbo.caj_Caja_Movimiento.IdCbteCble = dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdCbteCble AND dbo.caj_Caja_Movimiento.IdTipocbte = dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdTipocbte INNER JOIN
                  dbo.tb_persona ON dbo.caj_Caja_Movimiento.IdPersona = dbo.tb_persona.IdPersona LEFT OUTER JOIN
                  dbo.cxc_cobro_x_ct_cbtecble ON dbo.caj_Caja_Movimiento.IdEmpresa = dbo.cxc_cobro_x_ct_cbtecble.ct_IdEmpresa AND dbo.caj_Caja_Movimiento.IdTipocbte = dbo.cxc_cobro_x_ct_cbtecble.ct_IdTipoCbte AND 
                  dbo.caj_Caja_Movimiento.IdCbteCble = dbo.cxc_cobro_x_ct_cbtecble.ct_IdCbteCble

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWBAN_002_cancelaciones';




GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[73] 4[3] 2[5] 3) )"
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
         Begin Table = "ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 269
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_x_ct_cbtecble"
            Begin Extent = 
               Top = 279
               Left = 924
               Bottom = 560
               Right = 1118
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "caj_Caja_Movimiento"
            Begin Extent = 
               Top = 232
               Left = 417
               Bottom = 395
               Right = 641
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 14
               Left = 729
               Bottom = 177
               Right = 1003
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
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWBAN_002_cancelaciones';



