CREATE VIEW [dbo].[vwcom_ordencompra_local_detPorIngresar]
AS
SELECT ocd.IdEmpresa, ocd.IdSucursal, ocd.IdOrdenCompra, ocd.Secuencia, oc.Tipo, oc.SecuenciaTipo, ocd.IdProducto, p.pr_descripcion, ocd.do_Cantidad, ocd.do_precioFinal, ISNULL(SUM(invd.dm_cantidad_sinConversion), 0) 
                  AS CantidadIngresada, ocd.do_Cantidad - ISNULL(SUM(invd.dm_cantidad_sinConversion), 0) AS Saldo, ocd.IdUnidadMedida, per.pe_nombreCompleto, oc.oc_observacion, oc.oc_fecha, oc.IdProveedor, ocd.IdCentroCosto, 
                  ocd.IdPunto_cargo_grupo, ocd.IdPunto_cargo, dbo.ct_CentroCosto.cc_Descripcion
FROM     dbo.com_ordencompra_local_det AS ocd LEFT OUTER JOIN
                  dbo.in_Ing_Egr_Inven_det AS invd ON ocd.IdEmpresa = invd.IdEmpresa_oc AND ocd.IdSucursal = invd.IdSucursal_oc AND ocd.IdOrdenCompra = invd.IdOrdenCompra AND ocd.Secuencia = invd.Secuencia_oc INNER JOIN
                  dbo.in_Producto AS p ON p.IdEmpresa = ocd.IdEmpresa AND p.IdProducto = ocd.IdProducto INNER JOIN
                  dbo.com_ordencompra_local AS oc ON oc.IdEmpresa = ocd.IdEmpresa AND oc.IdSucursal = ocd.IdSucursal AND oc.IdOrdenCompra = ocd.IdOrdenCompra INNER JOIN
                  dbo.cp_proveedor AS pro ON pro.IdEmpresa = oc.IdEmpresa AND pro.IdProveedor = oc.IdProveedor INNER JOIN
                  dbo.tb_persona AS per ON per.IdPersona = pro.IdPersona LEFT OUTER JOIN
                  dbo.ct_CentroCosto ON ocd.IdEmpresa = dbo.ct_CentroCosto.IdEmpresa AND ocd.IdCentroCosto = dbo.ct_CentroCosto.IdCentroCosto
WHERE  (oc.Estado = 'A') and oc.IdEstadoAprobacion_cat = 'APRO' AND (oc.Tipo = 'OC')
GROUP BY ocd.IdEmpresa, ocd.IdSucursal, ocd.IdOrdenCompra, ocd.Secuencia, ocd.IdProducto, p.pr_descripcion, ocd.do_Cantidad, ocd.do_precioFinal, ocd.IdUnidadMedida, per.pe_nombreCompleto, oc.oc_observacion, oc.oc_fecha, 
                  oc.IdProveedor, oc.SecuenciaTipo, oc.Tipo, ocd.IdCentroCosto, ocd.IdPunto_cargo_grupo, ocd.IdPunto_cargo, dbo.ct_CentroCosto.cc_Descripcion
HAVING (ocd.do_Cantidad - ISNULL(SUM(invd.dm_cantidad_sinConversion), 0) <> 0)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcom_ordencompra_local_detPorIngresar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'umn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 18
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcom_ordencompra_local_detPorIngresar';




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
         Begin Table = "ocd"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 369
               Right = 317
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "invd"
            Begin Extent = 
               Top = 43
               Left = 477
               Bottom = 173
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 1
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
         Begin Table = "oc"
            Begin Extent = 
               Top = 323
               Left = 498
               Bottom = 628
               Right = 731
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pro"
            Begin Extent = 
               Top = 341
               Left = 983
               Bottom = 471
               Right = 1231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "per"
            Begin Extent = 
               Top = 666
               Left = 38
               Bottom = 796
               Right = 286
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_CentroCosto"
            Begin Extent = 
               Top = 184
               Left = 541
               Bottom = 435
               Right = 802
            End
            DisplayFlags = 280
            TopCol', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcom_ordencompra_local_detPorIngresar';



