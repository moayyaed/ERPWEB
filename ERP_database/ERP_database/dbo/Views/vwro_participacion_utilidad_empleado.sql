CREATE VIEW dbo.vwro_participacion_utilidad_empleado
AS
SELECT        dbo.ro_participacion_utilidad_empleado.IdEmpresa, dbo.ro_participacion_utilidad_empleado.IdUtilidad, dbo.ro_participacion_utilidad_empleado.IdEmpleado, dbo.ro_participacion_utilidad_empleado.DiasTrabajados, 
                         dbo.ro_participacion_utilidad_empleado.CargasFamiliares, dbo.ro_participacion_utilidad_empleado.ValorIndividual, dbo.ro_participacion_utilidad_empleado.ValorCargaFamiliar, 
                         dbo.ro_participacion_utilidad_empleado.ValorTotal, dbo.ro_participacion_utilidad.IdPeriodo, dbo.ro_participacion_utilidad.Estado, dbo.ro_cargo.ca_descripcion, dbo.tb_persona.pe_apellido, dbo.tb_persona.pe_nombre, 
                         dbo.tb_persona.pe_cedulaRuc, dbo.ro_empleado.em_status, dbo.ro_empleado.em_fechaIngaRol, dbo.ro_empleado.em_fechaIngaRol AS em_fecha_ingreso, dbo.ro_empleado.em_fechaSalida, 
                         dbo.ro_participacion_utilidad.UtilidadDerechoIndividual, dbo.ro_participacion_utilidad.UtilidadCargaFamiliar, dbo.ro_participacion_utilidad.Utilidad, dbo.ro_participacion_utilidad_empleado.Descuento, 
                         dbo.ro_participacion_utilidad_empleado.NetoRecibir, dbo.ro_participacion_utilidad_empleado.Observacion, dbo.ro_empleado.IdSucursal
FROM            dbo.ro_participacion_utilidad INNER JOIN
                         dbo.ro_participacion_utilidad_empleado ON dbo.ro_participacion_utilidad.IdEmpresa = dbo.ro_participacion_utilidad_empleado.IdEmpresa AND 
                         dbo.ro_participacion_utilidad.IdUtilidad = dbo.ro_participacion_utilidad_empleado.IdUtilidad INNER JOIN
                         dbo.ro_empleado ON dbo.ro_participacion_utilidad_empleado.IdEmpresa = dbo.ro_empleado.IdEmpresa AND dbo.ro_participacion_utilidad_empleado.IdEmpleado = dbo.ro_empleado.IdEmpleado INNER JOIN
                         dbo.tb_persona ON dbo.ro_empleado.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                         dbo.ro_cargo ON dbo.ro_empleado.IdEmpresa = dbo.ro_cargo.IdEmpresa AND dbo.ro_empleado.IdCargo = dbo.ro_cargo.IdCargo

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[83] 4[5] 2[5] 3) )"
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
         Begin Table = "ro_participacion_utilidad"
            Begin Extent = 
               Top = 12
               Left = 281
               Bottom = 339
               Right = 508
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "ro_participacion_utilidad_empleado"
            Begin Extent = 
               Top = 45
               Left = 18
               Bottom = 401
               Right = 207
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_empleado"
            Begin Extent = 
               Top = 23
               Left = 719
               Bottom = 297
               Right = 1008
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 205
               Left = 897
               Bottom = 335
               Right = 1129
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_cargo"
            Begin Extent = 
               Top = 321
               Left = 455
               Bottom = 451
               Right = 672
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
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
      End
   End
   Begin CriteriaPane = 
      ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_participacion_utilidad_empleado';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'Begin ColumnWidths = 11
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_participacion_utilidad_empleado';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_participacion_utilidad_empleado';

