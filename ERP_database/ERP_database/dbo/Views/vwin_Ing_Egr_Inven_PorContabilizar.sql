CREATE VIEW [dbo].[vwin_Ing_Egr_Inven_PorContabilizar]
AS
SELECT dbo.in_Ing_Egr_Inven_det.IdEmpresa, dbo.in_Ing_Egr_Inven_det.IdSucursal, dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo, dbo.in_Ing_Egr_Inven_det.IdNumMovi, dbo.in_Ing_Egr_Inven_det.Secuencia, 
                  dbo.in_Ing_Egr_Inven_det.IdEmpresa_inv, dbo.in_Ing_Egr_Inven_det.IdSucursal_inv, dbo.in_Ing_Egr_Inven_det.IdBodega_inv, dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo_inv, dbo.in_Ing_Egr_Inven_det.IdNumMovi_inv, 
                  dbo.in_Ing_Egr_Inven_det.secuencia_inv, dbo.in_producto_x_tb_bodega.IdCtaCble_Inven IdCtaCtble_Inve, dbo.in_categorias.IdCtaCtble_Costo, dbo.in_Motivo_Inven.IdCtaCble AS IdCtaCble_Motivo, dbo.in_parametro.P_IdCtaCble_transitoria_transf_inven, 
                  dbo.in_Ing_Egr_Inven_det.dm_cantidad * dbo.in_Ing_Egr_Inven_det.mv_costo AS Valor, dbo.in_Ing_Egr_Inven.Estado, CASE WHEN dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = in_parametro.IdMovi_inven_tipo_egresoBodegaOrigen OR
                  dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = in_parametro.IdMovi_inven_tipo_ingresoBodegaDestino THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS EsTransferencia, dbo.in_Ing_Egr_Inven_det.IdCentroCosto, 
                  in_Motivo_Inven_1.IdCtaCble AS IdCtaCble_MotivoDet, dbo.in_producto_x_tb_bodega.IdCtaCble_Costo AS IdCtaCble_CostoProducto
FROM     dbo.in_Ing_Egr_Inven_det INNER JOIN
                  dbo.in_Producto ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdProducto = dbo.in_Producto.IdProducto INNER JOIN
                  dbo.in_categorias ON dbo.in_Producto.IdEmpresa = dbo.in_categorias.IdEmpresa AND dbo.in_Producto.IdCategoria = dbo.in_categorias.IdCategoria INNER JOIN
                  dbo.tb_bodega ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.tb_bodega.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdBodega = dbo.tb_bodega.IdBodega INNER JOIN
                  dbo.in_Ing_Egr_Inven ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Ing_Egr_Inven.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.in_Ing_Egr_Inven.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo AND dbo.in_Ing_Egr_Inven_det.IdNumMovi = dbo.in_Ing_Egr_Inven.IdNumMovi INNER JOIN
                  dbo.in_Motivo_Inven ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.in_Motivo_Inven.IdEmpresa AND dbo.in_Ing_Egr_Inven.IdMotivo_Inv = dbo.in_Motivo_Inven.IdMotivo_Inv INNER JOIN
                  dbo.in_parametro ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_parametro.IdEmpresa LEFT OUTER JOIN
                  dbo.in_producto_x_tb_bodega ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_producto_x_tb_bodega.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.in_producto_x_tb_bodega.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdBodega = dbo.in_producto_x_tb_bodega.IdBodega AND dbo.in_Ing_Egr_Inven_det.IdProducto = dbo.in_producto_x_tb_bodega.IdProducto LEFT OUTER JOIN
                  dbo.in_Motivo_Inven AS in_Motivo_Inven_1 ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = in_Motivo_Inven_1.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdMotivo_Inv = in_Motivo_Inven_1.IdMotivo_Inv
WHERE  (dbo.in_Ing_Egr_Inven_det.IdSucursal_inv IS NOT NULL)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwin_Ing_Egr_Inven_PorContabilizar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Motivo_Inven_1"
            Begin Extent = 
               Top = 1183
               Left = 48
               Bottom = 1346
               Right = 270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_producto_x_tb_bodega"
            Begin Extent = 
               Top = 47
               Left = 965
               Bottom = 357
               Right = 1211
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwin_Ing_Egr_Inven_PorContabilizar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[16] 4[77] 2[3] 3) )"
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
         Begin Table = "in_Ing_Egr_Inven_det"
            Begin Extent = 
               Top = 69
               Left = 538
               Bottom = 645
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Producto"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 323
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_categorias"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 257
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_bodega"
            Begin Extent = 
               Top = 450
               Left = 94
               Bottom = 613
               Right = 406
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Ing_Egr_Inven"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Motivo_Inven"
            Begin Extent = 
               Top = 847
               Left = 48
               Bottom = 1010
               Right = 270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_parametro"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
               Right = 488', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwin_Ing_Egr_Inven_PorContabilizar';

