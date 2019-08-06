CREATE VIEW dbo.vwfa_proforma_det
AS
SELECT dbo.fa_proforma_det.IdSucursal, dbo.fa_proforma_det.IdProforma, dbo.fa_proforma_det.Secuencia, dbo.fa_proforma_det.IdProducto, dbo.fa_proforma_det.pd_cantidad, dbo.fa_proforma_det.pd_precio, 
                  dbo.fa_proforma_det.pd_por_descuento_uni, dbo.fa_proforma_det.pd_descuento_uni, dbo.fa_proforma_det.pd_precio_final, dbo.fa_proforma_det.pd_subtotal, dbo.fa_proforma_det.IdCod_Impuesto, dbo.fa_proforma_det.pd_por_iva, 
                  dbo.fa_proforma_det.pd_iva, dbo.fa_proforma_det.pd_total, dbo.fa_proforma_det.anulado, dbo.in_Producto.pr_descripcion, dbo.in_presentacion.nom_presentacion, dbo.in_Producto.lote_num_lote, dbo.in_Producto.lote_fecha_vcto, 
                  dbo.fa_proforma_det.IdCentroCosto, dbo.ct_CentroCosto.cc_Descripcion, dbo.fa_proforma_det.IdEmpresa, dbo.fa_proforma_det.NumCotizacion, dbo.fa_proforma_det.NumOPr, dbo.fa_proforma_det.pd_DetalleAdicional
FROM     dbo.ct_CentroCosto RIGHT OUTER JOIN
                  dbo.fa_proforma_det ON dbo.ct_CentroCosto.IdEmpresa = dbo.fa_proforma_det.IdEmpresa AND dbo.ct_CentroCosto.IdCentroCosto = dbo.fa_proforma_det.IdCentroCosto LEFT OUTER JOIN
                  dbo.in_presentacion INNER JOIN
                  dbo.in_Producto ON dbo.in_presentacion.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_presentacion.IdPresentacion = dbo.in_Producto.IdPresentacion ON dbo.fa_proforma_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND 
                  dbo.fa_proforma_det.IdProducto = dbo.in_Producto.IdProducto
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
         Begin Table = "ct_CentroCosto"
            Begin Extent = 
               Top = 13
               Left = 28
               Bottom = 166
               Right = 237
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_proforma_det"
            Begin Extent = 
               Top = 8
               Left = 376
               Bottom = 211
               Right = 584
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "in_presentacion"
            Begin Extent = 
               Top = 151
               Left = 700
               Bottom = 281
               Right = 887
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Producto"
            Begin Extent = 
               Top = 0
               Left = 660
               Bottom = 130
               Right = 894
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
      Begin ColumnWidths = 23
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
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwfa_proforma_det';








GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwfa_proforma_det';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwfa_proforma_det';



