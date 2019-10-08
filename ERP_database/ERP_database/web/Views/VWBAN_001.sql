CREATE VIEW web.VWBAN_001
AS
SELECT dbo.ba_Cbte_Ban_tipo.CodTipoCbteBan, dbo.ba_Cbte_Ban.IdEmpresa, dbo.ba_Cbte_Ban.IdCbteCble, dbo.ba_Cbte_Ban.IdTipocbte, dbo.ba_Cbte_Ban.IdBanco, dbo.ba_Banco_Cuenta.ba_descripcion, dbo.ba_Cbte_Ban.cb_Fecha, 
                  dbo.ba_Cbte_Ban.cb_Observacion, dbo.ba_Cbte_Ban.Estado, dbo.ba_Cbte_Ban.IdTipoNota, dbo.ba_tipo_nota.Descripcion AS Descripcion_TipoNota, 'Ninguno' AS NomBeneficiario, dbo.ct_cbtecble_det.IdCtaCble, 
                  dbo.ct_plancta.pc_Cuenta, dbo.ct_cbtecble_det.dc_Valor, CASE WHEN ct_cbtecble_det.dc_Valor > 0 THEN ct_cbtecble_det.dc_Valor ELSE 0 END AS dc_Valor_Debe, 
                  CASE WHEN ct_cbtecble_det.dc_Valor < 0 THEN ABS(ct_cbtecble_det.dc_Valor) ELSE 0 END AS dc_Valor_Haber, dbo.seg_usuario.Nombre, dbo.tb_sucursal.Su_Descripcion, dbo.ct_punto_cargo.nom_punto_cargo, 
                  dbo.ct_punto_cargo_grupo.nom_punto_cargo_grupo, dbo.ct_cbtecble_det.IdPunto_cargo_grupo, dbo.ct_cbtecble_det.IdPunto_cargo
FROM     dbo.ct_punto_cargo RIGHT OUTER JOIN
                  dbo.ba_Cbte_Ban_tipo INNER JOIN
                  dbo.ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo ON dbo.ba_Cbte_Ban_tipo.CodTipoCbteBan = dbo.ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo.CodTipoCbteBan INNER JOIN
                  dbo.ba_Cbte_Ban ON dbo.ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo.IdEmpresa = dbo.ba_Cbte_Ban.IdEmpresa AND dbo.ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo.IdTipoCbteCble = dbo.ba_Cbte_Ban.IdTipocbte INNER JOIN
                  dbo.ba_Banco_Cuenta ON dbo.ba_Cbte_Ban.IdEmpresa = dbo.ba_Banco_Cuenta.IdEmpresa AND dbo.ba_Cbte_Ban.IdBanco = dbo.ba_Banco_Cuenta.IdBanco INNER JOIN
                  dbo.ct_cbtecble_det INNER JOIN
                  dbo.ct_plancta ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_plancta.IdEmpresa AND dbo.ct_cbtecble_det.IdCtaCble = dbo.ct_plancta.IdCtaCble ON dbo.ba_Cbte_Ban.IdEmpresa = dbo.ct_cbtecble_det.IdEmpresa AND 
                  dbo.ba_Cbte_Ban.IdTipocbte = dbo.ct_cbtecble_det.IdTipoCbte AND dbo.ba_Cbte_Ban.IdCbteCble = dbo.ct_cbtecble_det.IdCbteCble ON dbo.ct_punto_cargo.IdEmpresa = dbo.ct_cbtecble_det.IdEmpresa AND 
                  dbo.ct_punto_cargo.IdPunto_cargo = dbo.ct_cbtecble_det.IdPunto_cargo LEFT OUTER JOIN
                  dbo.ct_punto_cargo_grupo ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_punto_cargo_grupo.IdEmpresa AND dbo.ct_cbtecble_det.IdPunto_cargo_grupo = dbo.ct_punto_cargo_grupo.IdPunto_cargo_grupo LEFT OUTER JOIN
                  dbo.ba_tipo_nota ON dbo.ba_Cbte_Ban.IdEmpresa = dbo.ba_tipo_nota.IdEmpresa AND dbo.ba_Cbte_Ban.IdTipoNota = dbo.ba_tipo_nota.IdTipoNota LEFT OUTER JOIN
                  dbo.seg_usuario ON dbo.ba_Cbte_Ban.IdUsuario = dbo.seg_usuario.IdUsuario LEFT OUTER JOIN
                  dbo.tb_sucursal ON dbo.ba_Cbte_Ban.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.ba_Cbte_Ban.IdSucursal = dbo.tb_sucursal.IdSucursal
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWBAN_001';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'           Right = 268
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "seg_usuario"
            Begin Extent = 
               Top = 1183
               Left = 48
               Bottom = 1346
               Right = 342
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_sucursal"
            Begin Extent = 
               Top = 1351
               Left = 48
               Bottom = 1514
               Right = 320
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_punto_cargo_grupo"
            Begin Extent = 
               Top = 1013
               Left = 1192
               Bottom = 1176
               Right = 1452
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_punto_cargo"
            Begin Extent = 
               Top = 746
               Left = 1354
               Bottom = 909
               Right = 1599
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWBAN_001';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[90] 4[3] 2[3] 3) )"
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
         Begin Table = "ba_tipo_nota"
            Begin Extent = 
               Top = 128
               Left = 670
               Bottom = 291
               Right = 864
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ba_Cbte_Ban_tipo"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 256
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ba_Cbte_Ban"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 309
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ba_Banco_Cuenta"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 338
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_cbtecble_det"
            Begin Extent = 
               Top = 864
               Left = 786
               Bottom = 1181
               Right = 1022
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_plancta"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
    ', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWBAN_001';

