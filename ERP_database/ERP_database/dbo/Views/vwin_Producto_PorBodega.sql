CREATE VIEW [dbo].[vwin_Producto_PorBodega]
AS
SELECT        dbo.in_producto_x_tb_bodega.IdEmpresa, dbo.in_producto_x_tb_bodega.IdSucursal, dbo.in_producto_x_tb_bodega.IdBodega, dbo.in_producto_x_tb_bodega.IdProducto, dbo.in_Producto.pr_codigo, 
                         dbo.in_Producto.pr_descripcion, SUM(ISNULL(dbo.in_Ing_Egr_Inven_det.dm_cantidad, 0)) AS Stock, dbo.in_categorias.ca_Categoria AS nom_categoria
FROM            dbo.in_ProductoTipo INNER JOIN
                         dbo.in_producto_x_tb_bodega INNER JOIN
                         dbo.in_Producto ON dbo.in_producto_x_tb_bodega.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_producto_x_tb_bodega.IdProducto = dbo.in_Producto.IdProducto ON 
                         dbo.in_ProductoTipo.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_ProductoTipo.IdProductoTipo = dbo.in_Producto.IdProductoTipo INNER JOIN
                         dbo.in_categorias ON dbo.in_Producto.IdEmpresa = dbo.in_categorias.IdEmpresa AND dbo.in_Producto.IdCategoria = dbo.in_categorias.IdCategoria LEFT OUTER JOIN
                         dbo.in_Ing_Egr_Inven INNER JOIN
                         dbo.in_Ing_Egr_Inven_det ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.in_Ing_Egr_Inven_det.IdEmpresa AND dbo.in_Ing_Egr_Inven.IdSucursal = dbo.in_Ing_Egr_Inven_det.IdSucursal AND 
                         dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo = dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo AND dbo.in_Ing_Egr_Inven.IdNumMovi = dbo.in_Ing_Egr_Inven_det.IdNumMovi and in_Ing_Egr_Inven.Estado = 'A' ON 
                         dbo.in_producto_x_tb_bodega.IdEmpresa = dbo.in_Ing_Egr_Inven_det.IdEmpresa AND dbo.in_producto_x_tb_bodega.IdSucursal = dbo.in_Ing_Egr_Inven_det.IdSucursal AND 
                         dbo.in_producto_x_tb_bodega.IdBodega = dbo.in_Ing_Egr_Inven_det.IdBodega AND dbo.in_producto_x_tb_bodega.IdProducto = dbo.in_Ing_Egr_Inven_det.IdProducto
WHERE  (dbo.in_Producto.Estado = 'A')
GROUP BY dbo.in_producto_x_tb_bodega.IdEmpresa, dbo.in_producto_x_tb_bodega.IdSucursal, dbo.in_producto_x_tb_bodega.IdBodega, dbo.in_producto_x_tb_bodega.IdProducto, dbo.in_Producto.pr_codigo, 
                         dbo.in_Producto.pr_descripcion, dbo.in_categorias.ca_Categoria
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwin_Producto_PorBodega';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' ColumnWidths = 12
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwin_Producto_PorBodega';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[89] 4[3] 2[3] 3) )"
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
         Begin Table = "in_ProductoTipo"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 331
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_producto_x_tb_bodega"
            Begin Extent = 
               Top = 337
               Left = 564
               Bottom = 704
               Right = 826
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Producto"
            Begin Extent = 
               Top = 17
               Left = 532
               Bottom = 228
               Right = 823
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_categorias"
            Begin Extent = 
               Top = 205
               Left = 53
               Bottom = 368
               Right = 278
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Ing_Egr_Inven"
            Begin Extent = 
               Top = 378
               Left = 1426
               Bottom = 541
               Right = 1666
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Ing_Egr_Inven_det"
            Begin Extent = 
               Top = 325
               Left = 918
               Bottom = 645
               Right = 1238
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
      Begin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwin_Producto_PorBodega';

