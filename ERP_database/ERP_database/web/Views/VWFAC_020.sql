CREATE VIEW web.VWFAC_020
AS
SELECT dbo.fa_factura_x_fa_guia_remision.fa_IdEmpresa, dbo.fa_factura_x_fa_guia_remision.fa_IdSucursal, dbo.fa_factura_x_fa_guia_remision.fa_IdBodega, dbo.fa_factura_x_fa_guia_remision.fa_IdCbteVta, 
                  dbo.fa_factura_x_fa_guia_remision.gi_IdEmpresa, dbo.fa_factura_x_fa_guia_remision.gi_IdSucursal, dbo.fa_factura_x_fa_guia_remision.gi_IdBodega, dbo.fa_factura_x_fa_guia_remision.gi_IdGuiaRemision, 
                  dbo.fa_guia_remision_det.Secuencia, dbo.fa_guia_remision_det.IdProducto, dbo.in_Producto.pr_codigo, dbo.in_Producto.pr_descripcion, dbo.fa_guia_remision_det.gi_cantidad, dbo.fa_guia_remision_det.gi_detallexItems, 
                  dbo.tb_persona.pe_nombreCompleto, dbo.tb_persona.pe_cedulaRuc, dbo.fa_guia_remision.CodDocumentoTipo, 
                  dbo.fa_guia_remision.Serie1 + '-' + dbo.fa_guia_remision.Serie2 + '-' + dbo.fa_guia_remision.NumGuia_Preimpresa AS NumGuia_Preimpresa, dbo.fa_guia_remision.CodGuiaRemision, dbo.fa_guia_remision.NUAutorizacion, 
                  dbo.fa_guia_remision.Fecha_Autorizacion, dbo.fa_guia_remision.IdCliente, dbo.fa_guia_remision.IdTransportista, dbo.fa_guia_remision.gi_fecha, dbo.fa_guia_remision.gi_FechaFinTraslado, dbo.fa_guia_remision.gi_FechaInicioTraslado, 
                  dbo.fa_guia_remision.gi_Observacion, dbo.fa_guia_remision.placa, dbo.fa_guia_remision.Direccion_Origen, dbo.fa_guia_remision.Direccion_Destino, dbo.fa_guia_remision.Estado, dbo.fa_MotivoTraslado.tr_Descripcion, 
                  dbo.fa_factura.vt_serie1 + '-' + dbo.fa_factura.vt_serie2 + '' + dbo.fa_factura.vt_NumFactura AS NumComprobanteVenta, dbo.tb_transportista.Cedula AS CedulaTransportista, dbo.tb_transportista.Nombre AS NombreTransportista, 
                  dbo.fa_factura.vt_fecha, dbo.fa_factura.vt_autorizacion, dbo.tb_sucursal.Su_Descripcion, dbo.tb_sucursal.Su_Direccion
FROM     dbo.tb_sucursal INNER JOIN
                  dbo.fa_guia_remision INNER JOIN
                  dbo.fa_guia_remision_det ON dbo.fa_guia_remision.IdEmpresa = dbo.fa_guia_remision_det.IdEmpresa AND dbo.fa_guia_remision.IdSucursal = dbo.fa_guia_remision_det.IdSucursal AND 
                  dbo.fa_guia_remision.IdBodega = dbo.fa_guia_remision_det.IdBodega AND dbo.fa_guia_remision.IdGuiaRemision = dbo.fa_guia_remision_det.IdGuiaRemision INNER JOIN
                  dbo.fa_cliente ON dbo.fa_guia_remision.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_guia_remision.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                  dbo.tb_transportista ON dbo.fa_guia_remision.IdEmpresa = dbo.tb_transportista.IdEmpresa AND dbo.fa_guia_remision.IdTransportista = dbo.tb_transportista.IdTransportista INNER JOIN
                  dbo.fa_MotivoTraslado ON dbo.fa_guia_remision.IdEmpresa = dbo.fa_MotivoTraslado.IdEmpresa AND dbo.fa_guia_remision.IdMotivoTraslado = dbo.fa_MotivoTraslado.IdMotivoTraslado INNER JOIN
                  dbo.fa_cliente_contactos ON dbo.fa_cliente.IdEmpresa = dbo.fa_cliente_contactos.IdEmpresa AND dbo.fa_cliente.IdCliente = dbo.fa_cliente_contactos.IdCliente INNER JOIN
                  dbo.in_Producto ON dbo.fa_guia_remision_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.fa_guia_remision_det.IdProducto = dbo.in_Producto.IdProducto ON dbo.tb_sucursal.IdEmpresa = dbo.fa_guia_remision.IdEmpresa AND 
                  dbo.tb_sucursal.IdSucursal = dbo.fa_guia_remision.IdSucursal LEFT OUTER JOIN
                  dbo.fa_factura INNER JOIN
                  dbo.fa_factura_x_fa_guia_remision ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_x_fa_guia_remision.fa_IdEmpresa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_x_fa_guia_remision.fa_IdSucursal AND 
                  dbo.fa_factura.IdBodega = dbo.fa_factura_x_fa_guia_remision.fa_IdBodega AND dbo.fa_factura.IdCbteVta = dbo.fa_factura_x_fa_guia_remision.fa_IdCbteVta ON 
                  dbo.fa_guia_remision.IdEmpresa = dbo.fa_factura_x_fa_guia_remision.gi_IdEmpresa AND dbo.fa_guia_remision.IdSucursal = dbo.fa_factura_x_fa_guia_remision.gi_IdSucursal AND 
                  dbo.fa_guia_remision.IdBodega = dbo.fa_factura_x_fa_guia_remision.gi_IdBodega AND dbo.fa_guia_remision.IdGuiaRemision = dbo.fa_factura_x_fa_guia_remision.gi_IdGuiaRemision
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_020';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'   Right = 265
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_MotivoTraslado"
            Begin Extent = 
               Top = 1183
               Left = 48
               Bottom = 1346
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_cliente_contactos"
            Begin Extent = 
               Top = 1351
               Left = 48
               Bottom = 1514
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "in_Producto"
            Begin Extent = 
               Top = 1519
               Left = 48
               Bottom = 1682
               Right = 323
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_sucursal"
            Begin Extent = 
               Top = 314
               Left = 347
               Bottom = 645
               Right = 619
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_020';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[78] 4[3] 2[0] 3) )"
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
         Top = -120
         Left = 0
      End
      Begin Tables = 
         Begin Table = "fa_factura"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 299
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_factura_x_fa_guia_remision"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 266
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_guia_remision"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_guia_remision_det"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 284
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fa_cliente"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 304
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 847
               Left = 48
               Bottom = 1010
               Right = 322
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_transportista"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
            ', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWFAC_020';

