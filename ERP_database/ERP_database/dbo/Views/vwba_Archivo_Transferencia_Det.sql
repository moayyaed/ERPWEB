CREATE VIEW dbo.vwba_Archivo_Transferencia_Det
AS
SELECT        dbo.ba_Archivo_Transferencia_Det.IdEmpresa, dbo.ba_Archivo_Transferencia_Det.IdArchivo, dbo.ba_Archivo_Transferencia_Det.Secuencia, dbo.ba_Archivo_Transferencia_Det.Id_Item, 
                         dbo.ba_Archivo_Transferencia_Det.IdEmpresa_OP, dbo.ba_Archivo_Transferencia_Det.IdOrdenPago, dbo.ba_Archivo_Transferencia_Det.Secuencia_OP, dbo.ba_Archivo_Transferencia_Det.Estado, 
                         dbo.ba_Archivo_Transferencia_Det.Valor, dbo.ba_Archivo_Transferencia_Det.Secuencial_reg_x_proceso, dbo.ba_Archivo_Transferencia_Det.Contabilizado, dbo.ba_Archivo_Transferencia_Det.Fecha_proceso, 
                         dbo.cp_proveedor.IdTipoCta_acreditacion_cat, dbo.cp_proveedor.num_cta_acreditacion, dbo.cp_proveedor.IdBanco_acreditacion, dbo.cp_proveedor.pr_direccion, dbo.cp_proveedor.pr_correo, dbo.tb_persona.IdTipoDocumento, 
                         dbo.tb_persona.pe_cedulaRuc, dbo.tb_persona.pe_nombreCompleto, dbo.tb_banco.CodigoLegal AS CodigoLegalBanco
FROM            dbo.ba_Archivo_Transferencia INNER JOIN
                         dbo.ba_Archivo_Transferencia_Det INNER JOIN
                         dbo.cp_orden_pago_det ON dbo.ba_Archivo_Transferencia_Det.IdEmpresa_OP = dbo.cp_orden_pago_det.IdEmpresa AND dbo.ba_Archivo_Transferencia_Det.IdOrdenPago = dbo.cp_orden_pago_det.IdOrdenPago AND 
                         dbo.ba_Archivo_Transferencia_Det.Secuencia_OP = dbo.cp_orden_pago_det.Secuencia INNER JOIN
                         dbo.cp_orden_pago ON dbo.cp_orden_pago_det.IdEmpresa = dbo.cp_orden_pago.IdEmpresa AND dbo.cp_orden_pago_det.IdOrdenPago = dbo.cp_orden_pago.IdOrdenPago INNER JOIN
                         dbo.tb_persona INNER JOIN
                         dbo.cp_proveedor ON dbo.tb_persona.IdPersona = dbo.cp_proveedor.IdPersona ON dbo.cp_orden_pago.IdPersona = dbo.cp_proveedor.IdPersona AND dbo.cp_orden_pago.IdEntidad = dbo.cp_proveedor.IdProveedor AND 
                         dbo.cp_orden_pago.IdEmpresa = dbo.cp_proveedor.IdEmpresa ON dbo.ba_Archivo_Transferencia.IdEmpresa = dbo.ba_Archivo_Transferencia_Det.IdEmpresa AND 
                         dbo.ba_Archivo_Transferencia.IdArchivo = dbo.ba_Archivo_Transferencia_Det.IdArchivo INNER JOIN
                         dbo.tb_banco ON dbo.cp_proveedor.IdBanco_acreditacion = dbo.tb_banco.IdBanco
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwba_Archivo_Transferencia_Det';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' Right = 531
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwba_Archivo_Transferencia_Det';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[51] 4[9] 2[5] 3) )"
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
         Top = -384
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ba_Archivo_Transferencia_Det"
            Begin Extent = 
               Top = 0
               Left = 9
               Bottom = 264
               Right = 233
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_orden_pago_det"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 244
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_orden_pago"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 239
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 532
               Right = 270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_proveedor"
            Begin Extent = 
               Top = 534
               Left = 38
               Bottom = 664
               Right = 270
            End
            DisplayFlags = 280
            TopColumn = 14
         End
         Begin Table = "tb_banco"
            Begin Extent = 
               Top = 510
               Left = 399
               Bottom = 640
               Right = 633
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "ba_Archivo_Transferencia"
            Begin Extent = 
               Top = 0
               Left = 339
               Bottom = 300
              ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwba_Archivo_Transferencia_Det';

