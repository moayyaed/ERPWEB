CREATE VIEW web.VWCXC_002_diario
AS
SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, 1 AS secuencial, dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_det.IdBodega_Cbte, 
                  dbo.cxc_cobro_det.IdCbte_vta_nota, dbo.cxc_cobro_x_ct_cbtecble.ct_IdEmpresa AS IdEmpresa_ct, dbo.cxc_cobro_x_ct_cbtecble.ct_IdTipoCbte AS IdTipoCbte_ct, 
                  dbo.cxc_cobro_x_ct_cbtecble.ct_IdCbteCble AS IdCbteCble_ct, dbo.ct_cbtecble_det.IdCtaCble, dbo.ct_plancta.pc_Cuenta, dbo.ct_cbtecble_det.dc_Valor, 
                  CASE WHEN ct_cbtecble_det.dc_Valor > 0 THEN ct_cbtecble_det.dc_Valor ELSE 0 END AS dc_Valor_Debe, CASE WHEN ct_cbtecble_det.dc_Valor < 0 THEN ABS(ct_cbtecble_det.dc_Valor) 
                  ELSE 0 END AS dc_Valor_Haber
FROM     dbo.cxc_cobro AS cxc_cobro_1 INNER JOIN
                  dbo.cxc_cobro_det ON cxc_cobro_1.IdEmpresa = dbo.cxc_cobro_det.IdEmpresa AND cxc_cobro_1.IdSucursal = dbo.cxc_cobro_det.IdSucursal AND cxc_cobro_1.IdCobro = dbo.cxc_cobro_det.IdCobro INNER JOIN
                  dbo.cxc_cobro_x_ct_cbtecble ON cxc_cobro_1.IdEmpresa = dbo.cxc_cobro_x_ct_cbtecble.cbr_IdEmpresa AND cxc_cobro_1.IdSucursal = dbo.cxc_cobro_x_ct_cbtecble.cbr_IdSucursal AND 
                  cxc_cobro_1.IdCobro = dbo.cxc_cobro_x_ct_cbtecble.cbr_IdCobro AND cxc_cobro_1.cr_estado <> 'I' INNER JOIN
                  dbo.ct_plancta INNER JOIN
                  dbo.ct_cbtecble_det ON dbo.ct_plancta.IdEmpresa = dbo.ct_cbtecble_det.IdEmpresa AND dbo.ct_plancta.IdCtaCble = dbo.ct_cbtecble_det.IdCtaCble ON 
                  dbo.cxc_cobro_x_ct_cbtecble.ct_IdEmpresa = dbo.ct_cbtecble_det.IdEmpresa AND dbo.cxc_cobro_x_ct_cbtecble.ct_IdTipoCbte = dbo.ct_cbtecble_det.IdTipoCbte AND 
                  dbo.cxc_cobro_x_ct_cbtecble.ct_IdCbteCble = dbo.ct_cbtecble_det.IdCbteCble
WHERE  (cxc_cobro_1.IdCobro_tipo IS NULL)
GROUP BY dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_det.IdBodega_Cbte, dbo.cxc_cobro_det.IdCbte_vta_nota, 
                  dbo.cxc_cobro_x_ct_cbtecble.ct_IdEmpresa, dbo.cxc_cobro_x_ct_cbtecble.ct_IdTipoCbte, dbo.cxc_cobro_x_ct_cbtecble.ct_IdCbteCble, dbo.ct_cbtecble_det.IdCtaCble, dbo.ct_plancta.pc_Cuenta, 
                  dbo.ct_cbtecble_det.dc_Valor

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_002_diario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'1440
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_002_diario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "cxc_cobro_1"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 287
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_det"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 289
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_x_ct_cbtecble"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_plancta"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 275
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_cbtecble_det"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 300
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
      Begin ColumnWidths = 9
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = ', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_002_diario';

