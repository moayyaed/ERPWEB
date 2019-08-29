CREATE VIEW web.VWCXC_002
AS
SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, dbo.cxc_cobro_det.secuencial, dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_det.IdBodega_Cbte, 
                  dbo.cxc_cobro_det.IdCbte_vta_nota, dbo.cxc_cobro_tipo.tc_descripcion, dbo.cxc_cobro_det.dc_ValorPago, dbo.tb_sucursal.Su_Descripcion, ISNULL(dbo.tb_persona.pe_nombreCompleto, fa_per.pe_nombreCompleto) 
                  AS pe_nombreCompleto, dbo.cxc_cobro.cr_fecha, dbo.cxc_cobro.cr_TotalCobro, dbo.cxc_cobro.cr_observacion, dbo.cxc_cobro.cr_NumDocumento, CASE WHEN fa_factura.vt_NumFactura IS NULL 
                  THEN fa_notaCreDeb.CodDocumentoTipo + '-' + isnull(fa_notaCreDeb.NumNota_Impresa, fa_notaCreDeb.CodNota) ELSE fa_factura.vt_tipoDoc + '-' + CAST(CAST(fa_factura.vt_NumFactura AS numeric) AS varchar(20)) 
                  END AS vt_NumFactura, ISNULL(dbo.fa_factura.vt_fecha, dbo.fa_notaCreDeb.no_fecha) AS vt_fecha, CAST(ISNULL(fd.SubtotalConDscto, nd.vt_Subtotal) AS float) AS vt_Subtotal, CAST(ISNULL(fd.ValorIVA, nd.vt_iva) 
                  AS float) AS vt_iva, CAST(ISNULL(fd.Total, nd.vt_total) AS float) AS vt_total, dbo.cxc_cobro_tipo.PorcentajeRet
FROM     dbo.tb_persona INNER JOIN
                  dbo.fa_factura INNER JOIN
                  dbo.fa_cliente ON dbo.fa_factura.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_factura.IdCliente = dbo.fa_cliente.IdCliente ON dbo.tb_persona.IdPersona = dbo.fa_cliente.IdPersona RIGHT OUTER JOIN
                  dbo.cxc_cobro INNER JOIN
                  dbo.cxc_cobro_det ON dbo.cxc_cobro.IdEmpresa = dbo.cxc_cobro_det.IdEmpresa AND dbo.cxc_cobro.IdSucursal = dbo.cxc_cobro_det.IdSucursal AND dbo.cxc_cobro.IdCobro = dbo.cxc_cobro_det.IdCobro INNER JOIN
                  dbo.cxc_cobro_tipo ON dbo.cxc_cobro_det.IdCobro_tipo = dbo.cxc_cobro_tipo.IdCobro_tipo INNER JOIN
                  dbo.tb_sucursal ON dbo.cxc_cobro_det.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.tb_sucursal.IdSucursal ON dbo.fa_factura.IdEmpresa = dbo.cxc_cobro_det.IdEmpresa AND 
                  dbo.fa_factura.IdSucursal = dbo.cxc_cobro_det.IdSucursal AND dbo.fa_factura.IdBodega = dbo.cxc_cobro_det.IdBodega_Cbte AND dbo.fa_factura.IdCbteVta = dbo.cxc_cobro_det.IdCbte_vta_nota AND 
                  dbo.fa_factura.vt_tipoDoc = dbo.cxc_cobro_det.dc_TipoDocumento LEFT OUTER JOIN
                  dbo.fa_cliente AS fa_cli INNER JOIN
                  dbo.tb_persona AS fa_per ON fa_cli.IdPersona = fa_per.IdPersona INNER JOIN
                  dbo.fa_notaCreDeb ON fa_cli.IdEmpresa = dbo.fa_notaCreDeb.IdEmpresa AND fa_cli.IdCliente = dbo.fa_notaCreDeb.IdCliente ON dbo.cxc_cobro_det.IdEmpresa = dbo.fa_notaCreDeb.IdEmpresa AND 
                  dbo.cxc_cobro_det.IdSucursal = dbo.fa_notaCreDeb.IdSucursal AND dbo.cxc_cobro_det.IdBodega_Cbte = dbo.fa_notaCreDeb.IdBodega AND dbo.cxc_cobro_det.IdCbte_vta_nota = dbo.fa_notaCreDeb.IdNota AND 
                  dbo.cxc_cobro_det.dc_TipoDocumento = dbo.fa_notaCreDeb.CodDocumentoTipo LEFT OUTER JOIN
                  dbo.fa_factura_resumen AS fd ON dbo.fa_factura.IdEmpresa = fd.IdEmpresa AND dbo.fa_factura.IdSucursal = fd.IdSucursal AND dbo.fa_factura.IdBodega = fd.IdBodega AND 
                  dbo.fa_factura.IdCbteVta = fd.IdCbteVta LEFT OUTER JOIN
                      (SELECT IdEmpresa, IdSucursal, IdBodega, IdNota, SUM(sc_subtotal) AS vt_Subtotal, SUM(sc_iva) AS vt_iva, SUM(sc_total) AS vt_total
                       FROM      dbo.fa_notaCreDeb_det
                       GROUP BY IdEmpresa, IdSucursal, IdBodega, IdNota) AS nd ON dbo.fa_notaCreDeb.IdEmpresa = nd.IdEmpresa AND dbo.fa_notaCreDeb.IdSucursal = nd.IdSucursal AND 
                  dbo.fa_notaCreDeb.IdBodega = nd.IdBodega AND dbo.fa_notaCreDeb.IdNota = nd.IdNota
WHERE  (dbo.cxc_cobro.IdCobro_tipo IS NULL) AND (dbo.cxc_cobro.cr_estado = 'A')

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[21] 4[18] 2[45] 3) )"
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
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 798
               Left = 38
               Bottom = 928
               Right = 270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_factura"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 532
               Right = 237
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_cliente"
            Begin Extent = 
               Top = 534
               Left = 38
               Bottom = 664
               Right = 254
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_det"
            Begin Extent = 
               Top = 6
               Left = 270
               Bottom = 136
               Right = 464
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_tipo"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 283
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "tb_sucursal"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 268
            End
     ', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_002';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'       DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_cli"
            Begin Extent = 
               Top = 931
               Left = 48
               Bottom = 1094
               Right = 304
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_per"
            Begin Extent = 
               Top = 1099
               Left = 48
               Bottom = 1262
               Right = 322
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_notaCreDeb"
            Begin Extent = 
               Top = 666
               Left = 38
               Bottom = 796
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fd"
            Begin Extent = 
               Top = 402
               Left = 275
               Bottom = 532
               Right = 445
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "nd"
            Begin Extent = 
               Top = 534
               Left = 292
               Bottom = 664
               Right = 462
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
      Begin ColumnWidths = 22
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_002';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_002';

