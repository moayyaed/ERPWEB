CREATE VIEW dbo.vwcom_ordencompra_local_x_ingresar
AS
SELECT        d.IdEmpresa, d.IdSucursal, d.IdOrdenCompra, d.Secuencia, C.IdProveedor, C.oc_fecha, C.oc_observacion, C.Estado, C.IdEstadoAprobacion_cat, C.IdEstado_cierre, d.IdProducto, d.IdCod_Impuesto, d.Por_Iva, d.do_Cantidad, 
                         d.do_precioCompra, d.do_porc_des, d.do_descuento, d.do_precioFinal, (d.do_Cantidad - ISNULL(SUM(i.dm_cantidad_sinConversion), 0)) * d.do_precioFinal AS do_subtotal, 
                         ((d.do_Cantidad - ISNULL(SUM(i.dm_cantidad_sinConversion), 0)) * d.do_precioFinal) * (d.Por_Iva / 100) AS do_iva, ((d.do_Cantidad - ISNULL(SUM(i.dm_cantidad_sinConversion), 0)) * d.do_precioFinal) * (1 + d.Por_Iva / 100) 
                         AS do_total, d.IdUnidadMedida, p.pr_descripcion, ISNULL(SUM(i.dm_cantidad_sinConversion), 0) AS CantidadIngresada, d.do_Cantidad - ISNULL(SUM(i.dm_cantidad_sinConversion), 0) AS Saldo, 
                         dbo.in_UnidadMedida.Descripcion AS NomUnidadMedida, dbo.in_categorias.IdCtaCtble_Inve
FROM            dbo.in_categorias RIGHT OUTER JOIN
                         dbo.in_Producto AS p ON dbo.in_categorias.IdEmpresa = p.IdEmpresa AND dbo.in_categorias.IdCategoria = p.IdCategoria RIGHT OUTER JOIN
                         dbo.in_UnidadMedida INNER JOIN
                         dbo.com_ordencompra_local AS C INNER JOIN
                         dbo.com_ordencompra_local_det AS d ON C.IdEmpresa = d.IdEmpresa AND C.IdSucursal = d.IdSucursal AND C.IdOrdenCompra = d.IdOrdenCompra ON 
                         dbo.in_UnidadMedida.IdUnidadMedida = d.IdUnidadMedida LEFT OUTER JOIN
                         dbo.in_Ing_Egr_Inven_det AS i ON d.IdEmpresa = i.IdEmpresa_oc AND d.IdSucursal = i.IdSucursal_oc AND d.IdOrdenCompra = i.IdOrdenCompra AND d.Secuencia = i.Secuencia_oc ON p.IdEmpresa = d.IdEmpresa AND 
                         p.IdProducto = d.IdProducto
GROUP BY d.IdEmpresa, d.IdSucursal, d.IdOrdenCompra, C.IdProveedor, C.oc_fecha, C.oc_observacion, C.Estado, C.IdEstadoAprobacion_cat, C.IdEstado_cierre, d.IdProducto, d.IdCod_Impuesto, d.Por_Iva, d.do_Cantidad, 
                         d.do_precioCompra, d.do_porc_des, d.do_descuento, d.do_precioFinal, d.do_subtotal, d.do_iva, d.do_total, d.IdUnidadMedida, p.pr_descripcion, d.Secuencia, dbo.in_UnidadMedida.Descripcion, 
                         dbo.in_categorias.IdCtaCtble_Inve
HAVING        (C.IdEstadoAprobacion_cat = 'APRO') AND (C.IdEstado_cierre <> 'CERR') AND (C.Estado = 'A')
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcom_ordencompra_local_x_ingresar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'= 1500
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
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcom_ordencompra_local_x_ingresar';




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
         Begin Table = "in_categorias"
            Begin Extent = 
               Top = 168
               Left = 830
               Bottom = 335
               Right = 1027
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 288
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_UnidadMedida"
            Begin Extent = 
               Top = 10
               Left = 780
               Bottom = 140
               Right = 975
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "C"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 271
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 0
               Left = 407
               Bottom = 303
               Right = 686
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "i"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 532
               Right = 317
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
      Begin ColumnWidths = 28
         Width = 284
         Width = 1500
         Width = 1500
         Width ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcom_ordencompra_local_x_ingresar';



