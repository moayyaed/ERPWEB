CREATE VIEW web.VWINV_008
AS
SELECT dbo.in_Ing_Egr_Inven_det.IdEmpresa, dbo.in_Ing_Egr_Inven_det.IdSucursal, dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo, dbo.in_Ing_Egr_Inven_det.IdNumMovi, dbo.in_Ing_Egr_Inven_det.Secuencia, dbo.tb_sucursal.Su_Descripcion, 
                  dbo.tb_bodega.bo_Descripcion, dbo.in_movi_inven_tipo.cm_tipo_movi, dbo.in_movi_inven_tipo.tm_descripcion, dbo.in_Motivo_Inven.Desc_mov_inv, dbo.tb_persona.pe_nombreCompleto, dbo.in_Producto.pr_codigo, 
                  dbo.in_Producto.pr_descripcion, dbo.in_Ing_Egr_Inven.IdEstadoAproba, dbo.in_Ing_Egr_Inven_det.dm_cantidad, dbo.in_Ing_Egr_Inven_det.mv_costo, dbo.in_Ing_Egr_Inven_det.IdOrdenCompra, dbo.in_Ing_Egr_Inven_det.IdCentroCosto, 
                  dbo.in_Ing_Egr_Inven_det.IdMotivo_Inv, dbo.in_Ing_Egr_Inven.cm_fecha, dbo.in_Ing_Egr_Inven.Estado, dbo.in_Ing_Egr_Inven_det.IdBodega, dbo.in_Ing_Egr_Inven_det.IdProducto, dbo.ct_CentroCosto.cc_Descripcion
FROM     dbo.in_Producto INNER JOIN
                  dbo.in_Ing_Egr_Inven_det INNER JOIN
                  dbo.in_Ing_Egr_Inven ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Ing_Egr_Inven.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.in_Ing_Egr_Inven.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo AND dbo.in_Ing_Egr_Inven_det.IdNumMovi = dbo.in_Ing_Egr_Inven.IdNumMovi INNER JOIN
                  dbo.in_movi_inven_tipo ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_movi_inven_tipo.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = dbo.in_movi_inven_tipo.IdMovi_inven_tipo ON 
                  dbo.in_Producto.IdEmpresa = dbo.in_Ing_Egr_Inven_det.IdEmpresa AND dbo.in_Producto.IdProducto = dbo.in_Ing_Egr_Inven_det.IdProducto INNER JOIN
                  dbo.tb_bodega ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.tb_bodega.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdBodega = dbo.tb_bodega.IdBodega INNER JOIN
                  dbo.tb_sucursal ON dbo.tb_bodega.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.tb_bodega.IdSucursal = dbo.tb_sucursal.IdSucursal LEFT OUTER JOIN
                  dbo.in_Motivo_Inven ON dbo.in_Ing_Egr_Inven_det.IdMotivo_Inv = dbo.in_Motivo_Inven.IdMotivo_Inv AND dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Motivo_Inven.IdEmpresa LEFT OUTER JOIN
                  dbo.tb_persona INNER JOIN
                  dbo.cp_proveedor ON dbo.tb_persona.IdPersona = dbo.cp_proveedor.IdPersona ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.cp_proveedor.IdEmpresa AND 
                  dbo.in_Ing_Egr_Inven.IdResponsable = dbo.cp_proveedor.IdProveedor LEFT OUTER JOIN
                  dbo.ct_CentroCosto ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.ct_CentroCosto.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdCentroCosto = dbo.ct_CentroCosto.IdCentroCosto
WHERE  (dbo.in_Ing_Egr_Inven.Estado = 'A')
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWINV_008';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' 270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 1183
               Left = 48
               Bottom = 1346
               Right = 322
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_proveedor"
            Begin Extent = 
               Top = 1351
               Left = 48
               Bottom = 1514
               Right = 322
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_CentroCosto"
            Begin Extent = 
               Top = 1519
               Left = 48
               Bottom = 1682
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 3
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
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWINV_008';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[70] 4[3] 2[9] 3) )"
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
         Top = -1320
         Left = 0
      End
      Begin Tables = 
         Begin Table = "in_Producto"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 323
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Ing_Egr_Inven_det"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 352
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Ing_Egr_Inven"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_movi_inven_tipo"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 303
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_bodega"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 256
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_sucursal"
            Begin Extent = 
               Top = 847
               Left = 48
               Bottom = 1010
               Right = 320
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Motivo_Inven"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
               Right =', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWINV_008';

