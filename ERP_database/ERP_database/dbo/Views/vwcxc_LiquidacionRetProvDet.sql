CREATE VIEW dbo.vwcxc_LiquidacionRetProvDet
AS
SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, dbo.cxc_cobro_det.secuencial, dbo.cxc_cobro_det.IdCobro_tipo, dbo.cxc_cobro_det.dc_ValorPago, dbo.cxc_cobro_tipo.tc_descripcion, 
                  dbo.cxc_cobro.cr_fecha, dbo.cxc_cobro.cr_observacion, dbo.cxc_cobro.cr_EsProvision, dbo.cxc_cobro.cr_estado, dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble, 
                  dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble + ' - ' + dbo.ct_plancta.pc_Cuenta AS pc_Cuenta, dbo.cxc_cobro_tipo.ESRetenIVA, dbo.cxc_cobro_tipo.ESRetenFTE, dbo.cxc_cobro.cr_NumDocumento, 
                  dbo.cxc_LiquidacionRetProvDet.IdLiquidacion, dbo.cxc_LiquidacionRetProvDet.Secuencia
FROM     dbo.cxc_cobro_tipo INNER JOIN
                  dbo.cxc_cobro_det ON dbo.cxc_cobro_tipo.IdCobro_tipo = dbo.cxc_cobro_det.IdCobro_tipo INNER JOIN
                  dbo.cxc_cobro ON dbo.cxc_cobro_det.IdEmpresa = dbo.cxc_cobro.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.cxc_cobro.IdSucursal AND dbo.cxc_cobro_det.IdCobro = dbo.cxc_cobro.IdCobro INNER JOIN
                  dbo.cxc_cobro_tipo_Param_conta_x_sucursal ON dbo.cxc_cobro_tipo.IdCobro_tipo = dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCobro_tipo INNER JOIN
                  dbo.ct_plancta ON dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdEmpresa = dbo.ct_plancta.IdEmpresa AND dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdEmpresa = dbo.ct_plancta.IdEmpresa AND 
                  dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble = dbo.ct_plancta.IdCtaCble INNER JOIN
                  dbo.cxc_LiquidacionRetProvDet ON dbo.cxc_cobro_det.IdEmpresa = dbo.cxc_LiquidacionRetProvDet.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.cxc_LiquidacionRetProvDet.IdSucursal AND 
                  dbo.cxc_cobro_det.IdCobro = dbo.cxc_LiquidacionRetProvDet.IdCobro AND dbo.cxc_cobro_det.secuencial = dbo.cxc_LiquidacionRetProvDet.secuencial
WHERE  (dbo.cxc_cobro.cr_estado = N'A') AND (dbo.cxc_cobro.cr_EsProvision = 1)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcxc_LiquidacionRetProvDet';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'     Width = 284
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcxc_LiquidacionRetProvDet';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[61] 4[0] 2[20] 3) )"
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
         Top = -480
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cxc_cobro_tipo"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_det"
            Begin Extent = 
               Top = 20
               Left = 480
               Bottom = 183
               Right = 705
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro"
            Begin Extent = 
               Top = 210
               Left = 230
               Bottom = 373
               Right = 453
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_tipo_Param_conta_x_sucursal"
            Begin Extent = 
               Top = 319
               Left = 585
               Bottom = 482
               Right = 807
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_plancta"
            Begin Extent = 
               Top = 415
               Left = 64
               Bottom = 578
               Right = 284
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_LiquidacionRetProvDet"
            Begin Extent = 
               Top = 586
               Left = 147
               Bottom = 864
               Right = 457
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
    ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcxc_LiquidacionRetProvDet';

