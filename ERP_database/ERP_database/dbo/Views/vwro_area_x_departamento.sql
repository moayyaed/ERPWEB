CREATE VIEW dbo.vwro_area_x_departamento
AS
SELECT dbo.ro_area_x_departamento.IdEmpresa, dbo.ro_area_x_departamento.Secuencia, dbo.ro_area_x_departamento.IdDivision, dbo.ro_Division.Descripcion AS DivisionDescripcion, dbo.ro_area_x_departamento.IdArea, 
                  dbo.ro_area.Descripcion AS AreaDescripcion, dbo.ro_area_x_departamento.IdDepartamento, dbo.ro_Departamento.de_descripcion AS DepartamentoDescripcion
FROM     dbo.ro_area_x_departamento INNER JOIN
                  dbo.ro_area ON dbo.ro_area_x_departamento.IdEmpresa = dbo.ro_area.IdEmpresa AND dbo.ro_area_x_departamento.IdDivision = dbo.ro_area.IdDivision AND dbo.ro_area_x_departamento.IdArea = dbo.ro_area.IdArea INNER JOIN
                  dbo.ro_Departamento ON dbo.ro_area_x_departamento.IdEmpresa = dbo.ro_Departamento.IdEmpresa AND dbo.ro_area_x_departamento.IdDepartamento = dbo.ro_Departamento.IdDepartamento INNER JOIN
                  dbo.ro_Division ON dbo.ro_area_x_departamento.IdEmpresa = dbo.ro_Division.IdEmpresa AND dbo.ro_area_x_departamento.IdDivision = dbo.ro_Division.IdDivision
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_area_x_departamento';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'   Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_area_x_departamento';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[58] 4[3] 2[20] 3) )"
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
         Begin Table = "ro_area_x_departamento"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 200
               Right = 253
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_area"
            Begin Extent = 
               Top = 79
               Left = 571
               Bottom = 306
               Right = 779
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_Departamento"
            Begin Extent = 
               Top = 0
               Left = 833
               Bottom = 188
               Right = 1041
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_Division"
            Begin Extent = 
               Top = 135
               Left = 334
               Bottom = 298
               Right = 542
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
      ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_area_x_departamento';

