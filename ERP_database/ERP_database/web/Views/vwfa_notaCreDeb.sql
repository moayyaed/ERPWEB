CREATE VIEW [web].[vwfa_notaCreDeb]
AS
SELECT dbo.fa_notaCreDeb.IdEmpresa, dbo.fa_notaCreDeb.IdSucursal, dbo.fa_notaCreDeb.IdBodega, dbo.fa_notaCreDeb.IdNota, dbo.fa_notaCreDeb.CreDeb, 
                  dbo.fa_notaCreDeb.Serie1 + '-' + dbo.fa_notaCreDeb.Serie2 + '-' + dbo.fa_notaCreDeb.NumNota_Impresa AS NumNota_Impresa, dbo.fa_notaCreDeb.no_fecha, P.pe_nombreCompleto AS Nombres, 
                  CAST(fa_notaCreDeb_resumen.SubtotalConDscto AS FLOAT) AS sc_subtotal, CAST(fa_notaCreDeb_resumen.ValorIVA AS FLOAT) AS sc_iva, CAST(fa_notaCreDeb_resumen.Total AS FLOAT) AS sc_total, dbo.fa_notaCreDeb.Estado, dbo.fa_notaCreDeb.CodNota, 
                  dbo.fa_notaCreDeb.Fecha_Autorizacion, dbo.fa_notaCreDeb.NumAutorizacion, dbo.fa_notaCreDeb.NaturalezaNota, dbo.fa_notaCreDeb_x_ct_cbtecble.ct_IdTipoCbte, dbo.fa_notaCreDeb_x_ct_cbtecble.ct_IdCbteCble
FROM     dbo.fa_notaCreDeb LEFT OUTER JOIN
                  dbo.fa_notaCreDeb_resumen ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_notaCreDeb_resumen.IdEmpresa AND dbo.fa_notaCreDeb.IdSucursal = dbo.fa_notaCreDeb_resumen.IdSucursal AND 
                  dbo.fa_notaCreDeb.IdBodega = dbo.fa_notaCreDeb_resumen.IdBodega AND dbo.fa_notaCreDeb.IdNota = dbo.fa_notaCreDeb_resumen.IdNota INNER JOIN
                  dbo.fa_cliente AS C ON C.IdEmpresa = dbo.fa_notaCreDeb.IdEmpresa AND C.IdCliente = dbo.fa_notaCreDeb.IdCliente INNER JOIN
                  dbo.tb_persona AS P ON P.IdPersona = C.IdPersona LEFT OUTER JOIN
                  dbo.fa_notaCreDeb_x_ct_cbtecble ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_notaCreDeb_x_ct_cbtecble.no_IdEmpresa AND dbo.fa_notaCreDeb.IdSucursal = dbo.fa_notaCreDeb_x_ct_cbtecble.no_IdSucursal AND 
                  dbo.fa_notaCreDeb.IdBodega = dbo.fa_notaCreDeb_x_ct_cbtecble.no_IdBodega AND dbo.fa_notaCreDeb.IdNota = dbo.fa_notaCreDeb_x_ct_cbtecble.no_IdNota

--where fa_notaCreDeb.NumNota_Impresa = 2746
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'vwfa_notaCreDeb';


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
         Begin Table = "fa_notaCreDeb"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 252
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "fa_cliente_contactos"
            Begin Extent = 
               Top = 6
               Left = 290
               Bottom = 136
               Right = 476
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_notaCreDeb_det"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 257
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
      Begin ColumnWidths = 12
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'vwfa_notaCreDeb';

