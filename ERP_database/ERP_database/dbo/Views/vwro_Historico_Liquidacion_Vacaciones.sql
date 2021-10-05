

CREATE VIEW [dbo].[vwro_Historico_Liquidacion_Vacaciones]
AS
SELECT        dbo.ro_Solicitud_Vacaciones_x_empleado.IdEmpresa, dbo.ro_Solicitud_Vacaciones_x_empleado.IdSolicitud, dbo.ro_Solicitud_Vacaciones_x_empleado.IdEmpleado, 
                         dbo.ro_Solicitud_Vacaciones_x_empleado.IdEstadoAprobacion, dbo.ro_Solicitud_Vacaciones_x_empleado.Fecha,
						 cast( dbo.ro_Solicitud_Vacaciones_x_empleado.Fecha_Desde as date)Fecha_Desde, 
                          cast(dbo.ro_Solicitud_Vacaciones_x_empleado.Fecha_Hasta as date)Fecha_Hasta,
						  cast(dbo.ro_Solicitud_Vacaciones_x_empleado.Fecha_Retorno as date)Fecha_Retorno,
						 dbo.ro_Solicitud_Vacaciones_x_empleado.Observacion, dbo.ro_Solicitud_Vacaciones_x_empleado.Gozadas, 
                         dbo.ro_Historico_Liquidacion_Vacaciones.IdLiquidacion, dbo.ro_Historico_Liquidacion_Vacaciones.IdOrdenPago, dbo.ro_Historico_Liquidacion_Vacaciones.IdEmpresa_OP, dbo.ro_Historico_Liquidacion_Vacaciones.IdTipo_op, 
                         dbo.ro_Historico_Liquidacion_Vacaciones.ValorCancelado, dbo.ro_Historico_Liquidacion_Vacaciones.FechaPago, dbo.ro_Historico_Liquidacion_Vacaciones.Observaciones,
						 CONCAT(pe_apellido,' ', pe_nombre) pe_nombreCompleto
, ro_Historico_Liquidacion_Vacaciones.Estado
FROM            dbo.ro_Historico_Liquidacion_Vacaciones INNER JOIN
                         dbo.ro_empleado ON dbo.ro_Historico_Liquidacion_Vacaciones.IdEmpresa = dbo.ro_empleado.IdEmpresa AND dbo.ro_Historico_Liquidacion_Vacaciones.IdEmpleado = dbo.ro_empleado.IdEmpleado INNER JOIN
                         dbo.tb_persona ON dbo.ro_empleado.IdPersona = dbo.tb_persona.IdPersona RIGHT OUTER JOIN
                         dbo.ro_Solicitud_Vacaciones_x_empleado ON dbo.ro_Historico_Liquidacion_Vacaciones.IdEmpresa = dbo.ro_Solicitud_Vacaciones_x_empleado.IdEmpresa AND 
                         dbo.ro_Historico_Liquidacion_Vacaciones.IdEmpleado = dbo.ro_Solicitud_Vacaciones_x_empleado.IdEmpleado AND dbo.ro_Historico_Liquidacion_Vacaciones.IdSolicitud = dbo.ro_Solicitud_Vacaciones_x_empleado.IdSolicitud



where ro_Solicitud_Vacaciones_x_empleado.Estado='A'
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_Historico_Liquidacion_Vacaciones';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_Historico_Liquidacion_Vacaciones';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[76] 4[5] 2[1] 3) )"
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
         Begin Table = "ro_Historico_Liquidacion_Vacaciones"
            Begin Extent = 
               Top = 19
               Left = 493
               Bottom = 402
               Right = 797
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_empleado"
            Begin Extent = 
               Top = 2
               Left = 788
               Bottom = 132
               Right = 1077
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 132
               Left = 1027
               Bottom = 371
               Right = 1258
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "ro_Solicitud_Vacaciones_x_empleado"
            Begin Extent = 
               Top = 35
               Left = 73
               Bottom = 374
               Right = 319
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
         ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwro_Historico_Liquidacion_Vacaciones';

