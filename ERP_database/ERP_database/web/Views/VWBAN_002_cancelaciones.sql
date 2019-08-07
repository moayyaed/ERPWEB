CREATE VIEW web.VWBAN_002_cancelaciones
AS
SELECT dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mba_IdEmpresa, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mba_IdCbteCble, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mba_IdTipocbte, 
                  dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdEmpresa, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdCbteCble, dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdTipocbte, 
                  dbo.caj_Caja_Movimiento.cm_observacion, dbo.caj_Caja_Movimiento.cm_fecha, dbo.caj_Caja_Movimiento.cm_valor, dbo.cxc_cobro_x_ct_cbtecble.cbr_IdCobro, dbo.cxc_cobro_x_ct_cbtecble.ct_IdCbteCble
FROM     dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito INNER JOIN
                  dbo.cxc_cobro_x_ct_cbtecble ON dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdEmpresa = dbo.cxc_cobro_x_ct_cbtecble.ct_IdEmpresa AND 
                  dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdTipocbte = dbo.cxc_cobro_x_ct_cbtecble.ct_IdTipoCbte AND 
                  dbo.ba_Caja_Movimiento_x_Cbte_Ban_x_Deposito.mcj_IdCbteCble = dbo.cxc_cobro_x_ct_cbtecble.ct_IdCbteCble INNER JOIN
                  dbo.caj_Caja_Movimiento ON dbo.cxc_cobro_x_ct_cbtecble.ct_IdEmpresa = dbo.caj_Caja_Movimiento.IdEmpresa AND dbo.cxc_cobro_x_ct_cbtecble.ct_IdTipoCbte = dbo.caj_Caja_Movimiento.IdTipocbte AND 
                  dbo.cxc_cobro_x_ct_cbtecble.ct_IdCbteCble = dbo.caj_Caja_Movimiento.IdCbteCble INNER JOIN
                  dbo.tb_persona ON dbo.caj_Caja_Movimiento.IdPersona = dbo.tb_persona.IdPersona

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWBAN_002_cancelaciones';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'eft = 48
               Bottom = 1178
               Right = 315
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_notaCreDeb"
            Begin Extent = 
               Top = 1183
               Left = 48
               Bottom = 1346
               Right = 294
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro"
            Begin Extent = 
               Top = 261
               Left = 670
               Bottom = 574
               Right = 909
            End
            DisplayFlags = 280
            TopColumn = 1
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
      Begin ColumnWidths = 12
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
         Begin Table = "cxc_cobro_x_caj_Caja_Movimiento"
            Begin Extent = 
               Top = 7
               Left = 514
               Bottom = 170
               Right = 729
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_det"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 289
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "caj_Caja_Movimiento_det"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "caj_Caja_Movimiento"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_det_x_ct_cbtecble_det"
            Begin Extent = 
               Top = 847
               Left = 48
               Bottom = 1010
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_factura"
            Begin Extent = 
               Top = 1015
               L', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWBAN_002_cancelaciones';

