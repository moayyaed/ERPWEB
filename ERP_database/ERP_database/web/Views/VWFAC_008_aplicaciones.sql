CREATE VIEW web.VWFAC_008_aplicaciones
AS
SELECT  fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_nt, fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_nt, fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_nt, fa_notaCreDeb_x_fa_factura_NotaDeb.IdNota_nt, 
fa_notaCreDeb_x_fa_factura_NotaDeb.secuencia, fa_notaCreDeb_x_fa_factura_NotaDeb.IdCbteVta_fac_nd_doc_mod, fa_notaCreDeb_x_fa_factura_NotaDeb.Valor_Aplicado, 
IIF(fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = 'FACT', (fa_factura.vt_tipoDoc + '-' + fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura), 
ISNULL(
(fa_notaCreDeb.CodDocumentoTipo + '-' + fa_notaCreDeb.Serie1 + '-' + fa_notaCreDeb.Serie2 + '-' + fa_notaCreDeb.NumNota_Impresa), fa_notaCreDeb.CodNota) ) AS NumFactura, IIF(fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = 'FACT', 
fa_factura.vt_fecha, fa_notaCreDeb.no_fecha) vt_fecha
FROM            fa_notaCreDeb_x_fa_factura_NotaDeb LEFT OUTER JOIN
fa_notaCreDeb ON fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_fac_nd_doc_mod = fa_notaCreDeb.IdEmpresa AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_fac_nd_doc_mod = fa_notaCreDeb.IdSucursal AND 
fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_fac_nd_doc_mod = fa_notaCreDeb.IdBodega AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdCbteVta_fac_nd_doc_mod = fa_notaCreDeb.IdNota AND 
fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = fa_notaCreDeb.CodDocumentoTipo LEFT OUTER JOIN
fa_factura ON fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_fac_nd_doc_mod = fa_factura.IdEmpresa AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_fac_nd_doc_mod = fa_factura.IdSucursal AND 
fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_fac_nd_doc_mod = fa_factura.IdBodega AND fa_notaCreDeb_x_fa_factura_NotaDeb.IdCbteVta_fac_nd_doc_mod = fa_factura.IdCbteVta AND 
fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = fa_factura.vt_tipoDoc
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_008_aplicaciones';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[9] 2[15] 3) )"
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
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 3060
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_008_aplicaciones';

