CREATE VIEW dbo.vwct_cbtecble_Plantilla_det
AS
SELECT dbo.ct_cbtecble_Plantilla_det.IdEmpresa, dbo.ct_cbtecble_Plantilla_det.IdPlantilla, dbo.ct_cbtecble_Plantilla_det.secuencia, dbo.ct_cbtecble_Plantilla_det.IdCtaCble, dbo.ct_cbtecble_Plantilla_det.dc_Valor, 
                  dbo.ct_cbtecble_Plantilla_det.dc_Observacion, dbo.ct_cbtecble_Plantilla_det.IdPunto_cargo_grupo, dbo.ct_cbtecble_Plantilla_det.IdPunto_cargo, dbo.ct_cbtecble_Plantilla_det.IdCentroCosto, 
                  dbo.ct_plancta.pc_Cuenta, dbo.ct_CentroCosto.cc_Descripcion, dbo.ct_punto_cargo.nom_punto_cargo
FROM     dbo.ct_cbtecble_Plantilla_det INNER JOIN
                  dbo.ct_plancta ON dbo.ct_cbtecble_Plantilla_det.IdEmpresa = dbo.ct_plancta.IdEmpresa AND dbo.ct_cbtecble_Plantilla_det.IdCtaCble = dbo.ct_plancta.IdCtaCble LEFT OUTER JOIN
                  dbo.ct_punto_cargo ON dbo.ct_cbtecble_Plantilla_det.IdEmpresa = dbo.ct_punto_cargo.IdEmpresa AND dbo.ct_cbtecble_Plantilla_det.IdPunto_cargo = dbo.ct_punto_cargo.IdPunto_cargo LEFT OUTER JOIN
                  dbo.ct_CentroCosto ON dbo.ct_cbtecble_Plantilla_det.IdEmpresa = dbo.ct_CentroCosto.IdEmpresa AND dbo.ct_cbtecble_Plantilla_det.IdCentroCosto = dbo.ct_CentroCosto.IdCentroCosto
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwct_cbtecble_Plantilla_det';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[48] 4[13] 2[9] 3) )"
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
         Begin Table = "ct_cbtecble_Plantilla_det"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 229
               Right = 241
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "ct_plancta"
            Begin Extent = 
               Top = 1
               Left = 357
               Bottom = 143
               Right = 540
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_CentroCosto"
            Begin Extent = 
               Top = 49
               Left = 952
               Bottom = 212
               Right = 1197
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "ct_punto_cargo"
            Begin Extent = 
               Top = 207
               Left = 601
               Bottom = 370
               Right = 846
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
      Begin ColumnWidths = 13
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1200
         Width = 1200
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
         SortOrder = ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwct_cbtecble_Plantilla_det';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwct_cbtecble_Plantilla_det';

