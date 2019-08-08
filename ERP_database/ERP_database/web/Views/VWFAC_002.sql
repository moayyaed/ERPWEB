CREATE VIEW [web].[VWFAC_002]
AS
SELECT dbo.fa_notaCreDeb_det.IdEmpresa, dbo.fa_notaCreDeb_det.IdSucursal, dbo.fa_notaCreDeb_det.IdBodega, dbo.fa_notaCreDeb_det.IdNota, dbo.fa_notaCreDeb_det.Secuencia, dbo.fa_notaCreDeb.Serie1, dbo.fa_notaCreDeb.Serie2, 
                  dbo.fa_notaCreDeb.Serie1+'-'+dbo.fa_notaCreDeb.Serie2+'-'+ dbo.fa_notaCreDeb.NumNota_Impresa NumNota_Impresa, dbo.tb_persona.pe_nombreCompleto, dbo.tb_persona.pe_cedulaRuc, dbo.fa_notaCreDeb.no_fecha, ISNULL(cr_fa.vt_fecha, cr_nd.no_fecha) AS FechaDocumentoAplica, 
                  dbo.fa_notaCreDeb.sc_observacion, dbo.fa_notaCreDeb_det.IdProducto, dbo.in_Producto.pr_codigo, dbo.in_Producto.pr_descripcion, dbo.fa_notaCreDeb_det.sc_observacion AS DetalleAdicional, dbo.fa_notaCreDeb_det.sc_precioFinal, 
                  dbo.fa_cliente_contactos.Telefono, dbo.fa_cliente_contactos.Celular, dbo.fa_cliente_contactos.Correo, dbo.fa_cliente_contactos.Direccion, 
                  CASE WHEN fa_notaCreDeb_det.vt_por_iva > 0 THEN fa_notaCreDeb_det.sc_cantidad * fa_notaCreDeb_det.sc_Precio ELSE 0 END AS SubtotalIva, 
                  CASE WHEN fa_notaCreDeb_det.vt_por_iva = 0 THEN fa_notaCreDeb_det.sc_cantidad * fa_notaCreDeb_det.sc_Precio ELSE 0 END AS SubtotalSinIva, 
                  dbo.fa_notaCreDeb_det.sc_cantidad * dbo.fa_notaCreDeb_det.sc_Precio AS SubtotalAntesDescuento, dbo.fa_notaCreDeb_det.sc_subtotal, dbo.fa_notaCreDeb_det.sc_cantidad * dbo.fa_notaCreDeb_det.sc_descUni AS TotalDescuento, 
                  dbo.fa_notaCreDeb_det.sc_iva, dbo.fa_notaCreDeb_det.sc_total, CASE WHEN cr_fa.vt_NumFactura IS NULL 
                  THEN CASE WHEN cr_nd.NaturalezaNota = 'SRI' THEN cr_nd.Serie1 + '-' + cr_nd.Serie2 + '-' + cr_nd.NumNota_Impresa ELSE ISNULL(cr_nd.CodNota, 
                  CAST(cr_nd.IdNota AS varchar(20))) END ELSE cr_fa.vt_serie1 + '-' + cr_fa.vt_serie2 + '-' + cr_fa.vt_NumFactura END AS DocumentoAplicado, dbo.fa_notaCreDeb.NumAutorizacion, dbo.fa_notaCreDeb.Fecha_Autorizacion, 
                  dbo.fa_notaCreDeb.CreDeb, dbo.fa_notaCreDeb.CodDocumentoTipo, fa_notaCreDeb_det.sc_cantidad
FROM     dbo.fa_notaCreDeb INNER JOIN
                  dbo.fa_notaCreDeb_det ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_notaCreDeb_det.IdEmpresa AND dbo.fa_notaCreDeb.IdSucursal = dbo.fa_notaCreDeb_det.IdSucursal AND 
                  dbo.fa_notaCreDeb.IdBodega = dbo.fa_notaCreDeb_det.IdBodega AND dbo.fa_notaCreDeb.IdNota = dbo.fa_notaCreDeb_det.IdNota INNER JOIN
                  dbo.fa_cliente ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_notaCreDeb.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.fa_notaCreDeb_x_fa_factura_NotaDeb ON dbo.fa_notaCreDeb.IdEmpresa = dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_nt AND 
                  dbo.fa_notaCreDeb.IdSucursal = dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_nt AND dbo.fa_notaCreDeb.IdBodega = dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_nt AND 
                  dbo.fa_notaCreDeb.IdNota = dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdNota_nt INNER JOIN
                  dbo.in_Producto ON dbo.fa_notaCreDeb_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.fa_notaCreDeb_det.IdProducto = dbo.in_Producto.IdProducto INNER JOIN
                  dbo.fa_cliente_contactos ON dbo.fa_cliente.IdEmpresa = dbo.fa_cliente_contactos.IdEmpresa AND dbo.fa_cliente.IdCliente = dbo.fa_cliente_contactos.IdCliente LEFT OUTER JOIN
                  dbo.fa_factura AS cr_fa ON dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_fac_nd_doc_mod = cr_fa.IdEmpresa AND dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_fac_nd_doc_mod = cr_fa.IdSucursal AND 
                  dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_fac_nd_doc_mod = cr_fa.IdBodega AND dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdCbteVta_fac_nd_doc_mod = cr_fa.IdCbteVta AND 
                  dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = cr_fa.vt_tipoDoc LEFT OUTER JOIN
                  dbo.fa_notaCreDeb AS cr_nd ON dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdEmpresa_fac_nd_doc_mod = cr_nd.IdEmpresa AND dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdSucursal_fac_nd_doc_mod = cr_nd.IdSucursal AND 
                  dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdBodega_fac_nd_doc_mod = cr_nd.IdBodega AND dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.IdCbteVta_fac_nd_doc_mod = cr_nd.IdNota AND 
                  dbo.fa_notaCreDeb_x_fa_factura_NotaDeb.vt_tipoDoc = cr_nd.CodDocumentoTipo
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_002';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'     Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cr_fa"
            Begin Extent = 
               Top = 1183
               Left = 48
               Bottom = 1346
               Right = 299
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cr_nd"
            Begin Extent = 
               Top = 1351
               Left = 48
               Bottom = 1514
               Right = 278
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_002';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[83] 4[3] 2[3] 3) )"
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
               Top = 7
               Left = 48
               Bottom = 170
               Right = 278
            End
            DisplayFlags = 280
            TopColumn = 27
         End
         Begin Table = "fa_notaCreDeb_det"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 284
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_cliente"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 304
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 322
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_notaCreDeb_x_fa_factura_NotaDeb"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 330
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Producto"
            Begin Extent = 
               Top = 847
               Left = 48
               Bottom = 1010
               Right = 323
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_cliente_contactos"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
          ', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_002';

