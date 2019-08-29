CREATE VIEW web.VWCXC_003
AS
SELECT dbo.fa_factura.IdEmpresa, dbo.fa_factura.IdSucursal, dbo.fa_factura.IdBodega, dbo.fa_factura.IdCbteVta, dbo.fa_factura.vt_serie1 + '-' + dbo.fa_factura.vt_serie2 + '-' + dbo.fa_factura.vt_NumFactura AS vt_NumFactura, 
                  dbo.tb_persona.pe_nombreCompleto, dbo.tb_persona.pe_cedulaRuc, dbo.cxc_cobro.cr_fecha, dbo.cxc_cobro.cr_NumDocumento, dbo.cxc_cobro_det.IdCobro_tipo, dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro, 
                  dbo.cxc_cobro_tipo.PorcentajeRet, 
                  CASE WHEN cxc_cobro_tipo.ESRetenFTE = 'S' THEN fa_factura_resumen.SubtotalConDscto WHEN cxc_cobro_tipo.ESRetenIVA = 'S' THEN fa_factura_resumen.ValorIVA ELSE 0 END AS Base, 
                  dbo.cxc_cobro_tipo.ESRetenIVA, dbo.cxc_cobro_tipo.ESRetenFTE, CASE WHEN cxc_cobro.cr_EsElectronico = 1 THEN 'SI' ELSE 'NO' END AS cr_EsElectronico, dbo.cxc_cobro_tipo.tc_descripcion, 
                  CASE WHEN cxc_cobro_tipo.ESRetenFTE IS NULL 
                  THEN 'COMPROBANTE SIN RETENCION' WHEN cxc_cobro_tipo.ESRetenFTE = 'S' THEN 'RETENCION DE FUENTE' WHEN cxc_cobro_tipo.ESRetenIVA = 'S' THEN 'RETENCION DE IVA' END AS TipoRetencion, 
                  dbo.fa_factura.IdCliente, dbo.fa_factura.vt_fecha, dbo.cxc_cobro_det.dc_ValorPago
FROM     dbo.fa_cliente INNER JOIN
                  dbo.fa_factura ON dbo.fa_cliente.IdEmpresa = dbo.fa_factura.IdEmpresa AND dbo.fa_cliente.IdCliente = dbo.fa_factura.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.fa_factura_resumen ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_resumen.IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_resumen.IdSucursal AND 
                  dbo.fa_factura.IdBodega = dbo.fa_factura_resumen.IdBodega AND dbo.fa_factura.IdCbteVta = dbo.fa_factura_resumen.IdCbteVta LEFT OUTER JOIN
                  dbo.cxc_cobro_tipo INNER JOIN
                  dbo.cxc_cobro_det ON dbo.cxc_cobro_tipo.IdCobro_tipo = dbo.cxc_cobro_det.IdCobro_tipo AND dbo.cxc_cobro_det.estado = 'A' INNER JOIN
                  dbo.cxc_cobro ON dbo.cxc_cobro_det.IdEmpresa = dbo.cxc_cobro.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.cxc_cobro.IdSucursal AND dbo.cxc_cobro_det.IdCobro = dbo.cxc_cobro.IdCobro AND 
                  dbo.cxc_cobro.cr_estado <> 'I' ON dbo.fa_factura.IdEmpresa = dbo.cxc_cobro_det.IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.cxc_cobro_det.IdSucursal AND 
                  dbo.fa_factura.IdBodega = dbo.cxc_cobro_det.IdBodega_Cbte AND dbo.fa_factura.IdCbteVta = dbo.cxc_cobro_det.IdCbte_vta_nota AND dbo.fa_factura.vt_tipoDoc = dbo.cxc_cobro_det.dc_TipoDocumento
WHERE  (dbo.fa_factura.Estado = 'A') AND (ISNULL(dbo.cxc_cobro_tipo.IdMotivo_tipo_cobro, 'RET') = 'RET')
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_003';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[36] 4[12] 2[27] 3) )"
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
         Top = -720
         Left = 0
      End
      Begin Tables = 
         Begin Table = "fa_cliente"
            Begin Extent = 
               Top = 16
               Left = 595
               Bottom = 408
               Right = 811
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_factura"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 419
               Right = 237
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 61
               Left = 900
               Bottom = 191
               Right = 1132
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_factura_resumen"
            Begin Extent = 
               Top = 406
               Left = 48
               Bottom = 569
               Right = 306
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_tipo"
            Begin Extent = 
               Top = 574
               Left = 48
               Bottom = 737
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cxc_cobro_det"
            Begin Extent = 
               Top = 742
               Left = 48
               Bottom = 905
               Right = 273
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "cxc_cobro"
            Begin Extent = 
               Top = 910
               Left = 48
               Bottom = 1073
               Right = 271
          ', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_003';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  End
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXC_003';

