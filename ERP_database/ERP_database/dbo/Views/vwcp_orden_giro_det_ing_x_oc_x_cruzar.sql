CREATE VIEW dbo.vwcp_orden_giro_det_ing_x_oc_x_cruzar
AS
SELECT dbo.in_Ing_Egr_Inven_det.IdEmpresa AS inv_IdEmpresa, dbo.in_Ing_Egr_Inven_det.IdSucursal AS inv_IdSucursal, dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo AS inv_IdMovi_inven_tipo, 
                  dbo.in_Ing_Egr_Inven_det.IdNumMovi AS inv_IdNumMovi, dbo.in_Ing_Egr_Inven_det.Secuencia AS inv_Secuencia, dbo.in_Ing_Egr_Inven_det.IdSucursal_oc AS oc_IdSucursal, 
                  dbo.in_Ing_Egr_Inven_det.IdOrdenCompra AS oc_IdOrdenCompra, dbo.in_Ing_Egr_Inven_det.Secuencia_oc AS oc_Secuencia, dbo.in_Producto.pr_descripcion, ISNULL(dbo.in_producto_x_tb_bodega.IdCtaCble_Inven, 
                  dbo.tb_bodega.IdCtaCtble_Inve) AS IdCtaCtble_Inve, dbo.in_Ing_Egr_Inven_det.dm_cantidad_sinConversion, dbo.com_ordencompra_local_det.do_precioCompra, dbo.com_ordencompra_local_det.do_porc_des, 
                  dbo.com_ordencompra_local_det.do_descuento, dbo.com_ordencompra_local_det.do_precioFinal, 
                  dbo.com_ordencompra_local_det.do_precioFinal * dbo.in_Ing_Egr_Inven_det.dm_cantidad_sinConversion AS do_subtotal, 
                  (dbo.com_ordencompra_local_det.do_precioFinal * dbo.in_Ing_Egr_Inven_det.dm_cantidad_sinConversion) * (dbo.com_ordencompra_local_det.Por_Iva / 100) AS do_iva, 
                  dbo.com_ordencompra_local_det.do_precioFinal * dbo.in_Ing_Egr_Inven_det.dm_cantidad_sinConversion + (dbo.com_ordencompra_local_det.do_precioFinal * dbo.in_Ing_Egr_Inven_det.dm_cantidad_sinConversion) 
                  * (dbo.com_ordencompra_local_det.Por_Iva / 100) AS do_total, dbo.com_ordencompra_local_det.IdUnidadMedida, dbo.com_ordencompra_local_det.Por_Iva, dbo.com_ordencompra_local_det.IdCod_Impuesto, 
                  dbo.in_UnidadMedida.Descripcion AS NomUnidadMedida, dbo.com_ordencompra_local.IdProveedor, dbo.com_ordencompra_local_det.IdProducto, ISNULL(ct_plancta_1.pc_Cuenta, dbo.ct_plancta.pc_Cuenta) 
                  AS pc_Cuenta, dbo.com_ordencompra_local.SecuenciaTipo
FROM     dbo.in_producto_x_tb_bodega LEFT OUTER JOIN
                  dbo.ct_plancta AS ct_plancta_1 ON dbo.in_producto_x_tb_bodega.IdEmpresa = ct_plancta_1.IdEmpresa AND dbo.in_producto_x_tb_bodega.IdCtaCble_Inven = ct_plancta_1.IdCtaCble RIGHT OUTER JOIN
                  dbo.com_ordencompra_local_det INNER JOIN
                  dbo.in_Ing_Egr_Inven_det ON dbo.com_ordencompra_local_det.IdEmpresa = dbo.in_Ing_Egr_Inven_det.IdEmpresa_oc AND dbo.com_ordencompra_local_det.IdSucursal = dbo.in_Ing_Egr_Inven_det.IdSucursal_oc AND 
                  dbo.com_ordencompra_local_det.IdOrdenCompra = dbo.in_Ing_Egr_Inven_det.IdOrdenCompra AND dbo.com_ordencompra_local_det.Secuencia = dbo.in_Ing_Egr_Inven_det.Secuencia_oc INNER JOIN
                  dbo.com_ordencompra_local ON dbo.com_ordencompra_local_det.IdEmpresa = dbo.com_ordencompra_local.IdEmpresa AND dbo.com_ordencompra_local_det.IdSucursal = dbo.com_ordencompra_local.IdSucursal AND 
                  dbo.com_ordencompra_local_det.IdOrdenCompra = dbo.com_ordencompra_local.IdOrdenCompra INNER JOIN
                  dbo.in_Producto ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdProducto = dbo.in_Producto.IdProducto INNER JOIN
                  dbo.in_UnidadMedida ON dbo.com_ordencompra_local_det.IdUnidadMedida = dbo.in_UnidadMedida.IdUnidadMedida ON dbo.in_producto_x_tb_bodega.IdEmpresa = dbo.in_Ing_Egr_Inven_det.IdEmpresa AND 
                  dbo.in_producto_x_tb_bodega.IdSucursal = dbo.in_Ing_Egr_Inven_det.IdSucursal AND dbo.in_producto_x_tb_bodega.IdBodega = dbo.in_Ing_Egr_Inven_det.IdBodega AND 
                  dbo.in_producto_x_tb_bodega.IdProducto = dbo.in_Ing_Egr_Inven_det.IdProducto LEFT OUTER JOIN
                  dbo.ct_plancta INNER JOIN
                  dbo.tb_bodega ON dbo.ct_plancta.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.ct_plancta.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.ct_plancta.IdCtaCble = dbo.tb_bodega.IdCtaCtble_Inve ON 
                  dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.tb_bodega.IdSucursal AND dbo.in_Ing_Egr_Inven_det.IdBodega = dbo.tb_bodega.IdBodega AND 
                  dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.tb_bodega.IdEmpresa LEFT OUTER JOIN
                  dbo.cp_orden_giro_det_ing_x_oc ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.cp_orden_giro_det_ing_x_oc.IdEmpresa AND 
                  dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.cp_orden_giro_det_ing_x_oc.inv_IdSucursal AND dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = dbo.cp_orden_giro_det_ing_x_oc.inv_IdMovi_inven_tipo AND 
                  dbo.in_Ing_Egr_Inven_det.IdNumMovi = dbo.cp_orden_giro_det_ing_x_oc.inv_IdNumMovi AND dbo.in_Ing_Egr_Inven_det.Secuencia = dbo.cp_orden_giro_det_ing_x_oc.inv_Secuencia
WHERE  (dbo.cp_orden_giro_det_ing_x_oc.IdEmpresa IS NULL)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcp_orden_giro_det_ing_x_oc_x_cruzar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
               Right = 256
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_plancta"
            Begin Extent = 
               Top = 919
               Left = 563
               Bottom = 1082
               Right = 774
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_bodega"
            Begin Extent = 
               Top = 847
               Left = 48
               Bottom = 1010
               Right = 360
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_orden_giro_det_ing_x_oc"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
               Right = 288
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcp_orden_giro_det_ing_x_oc_x_cruzar';






GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[31] 4[5] 2[37] 3) )"
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
         Top = -360
         Left = 0
      End
      Begin Tables = 
         Begin Table = "in_producto_x_tb_bodega"
            Begin Extent = 
               Top = 607
               Left = 48
               Bottom = 770
               Right = 294
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_plancta_1"
            Begin Extent = 
               Top = 775
               Left = 48
               Bottom = 938
               Right = 259
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "com_ordencompra_local_det"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 357
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Ing_Egr_Inven_det"
            Begin Extent = 
               Top = 48
               Left = 619
               Bottom = 211
               Right = 928
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "com_ordencompra_local"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 305
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "in_Producto"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 323
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_UnidadMedida"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcp_orden_giro_det_ing_x_oc_x_cruzar';





