CREATE VIEW [web].[VWCXP_009]
AS
SELECT dbo.cp_orden_giro.IdEmpresa, dbo.cp_orden_giro.IdTipoCbte_Ogiro, dbo.cp_orden_giro.IdCbteCble_Ogiro, ISNULL(dbo.cp_retencion_det.IdRetencion, 0) AS IdRetencion, ISNULL(dbo.cp_retencion_det.Idsecuencia, 0) AS Idsecuencia, 
                  dbo.cp_orden_giro.IdOrden_giro_Tipo, dbo.cp_orden_giro.IdProveedor, per.pe_nombreCompleto AS nom_proveedor, per.pe_cedulaRuc AS ced_proveedor, pro.pr_direccion AS dir_proveedor, 
                  dbo.cp_orden_giro.co_FechaFactura co_fechaOg, dbo.cp_orden_giro.co_serie, dbo.cp_orden_giro.co_factura AS num_factura, dbo.cp_orden_giro.co_FechaFactura, ISNULL(dbo.cp_retencion.Estado, dbo.cp_orden_giro.Estado) 
                  AS Estado, dbo.cp_TipoDocumento.Descripcion AS TipoDocumento, dbo.cp_orden_giro.co_FechaFactura AS fecha_retencion, YEAR(ISNULL(dbo.cp_retencion.fecha, dbo.cp_orden_giro.co_FechaContabilizacion)) AS ejercicio_fiscal, 
                  ISNULL(dbo.cp_retencion_det.re_tipoRet, 'RTF') AS Impuesto, ISNULL(dbo.cp_retencion_det.re_baseRetencion, dbo.cp_orden_giro.co_subtotal_iva + dbo.cp_orden_giro.co_subtotal_siniva) AS base_retencion, 
                  ISNULL(dbo.cp_retencion_det.IdCodigo_SRI, 0) AS IdCodigo_SRI, ISNULL(dbo.cp_codigo_SRI.codigoSRI, '332') AS cod_Impuesto_SRI, ISNULL(dbo.cp_codigo_SRI.co_porRetencion, 0) AS por_Retencion_SRI, 
                  ISNULL(dbo.cp_retencion_det.re_valor_retencion, 0) AS valor_Retenido, dbo.cp_orden_giro.IdEmpresa AS IdEmpresa_Ogiro, dbo.cp_retencion.serie1 + '-' + dbo.cp_retencion.serie2 AS serie, dbo.cp_retencion.NumRetencion, 
                  ISNULL(dbo.cp_codigo_SRI.co_descripcion, 'Retención de fuente 0%') AS co_descripcion, dbo.cp_codigo_SRI_x_CtaCble.IdCtaCble, dbo.cp_retencion_x_ct_cbtecble.ct_IdCbteCble AS IdCbteCbleRet, 
                  ISNULL(dbo.cp_orden_giro.co_observacion, dbo.cp_retencion.observacion) AS co_observacion, dbo.cp_orden_giro.IdSucursal, su.Su_Descripcion, CAST(ISNULL(dbo.tb_sis_Documento_Tipo_Talonario.es_Documento_Electronico, 0) 
                  AS bit) AS es_Documento_Electronico
FROM     dbo.cp_retencion_det INNER JOIN
                  dbo.cp_retencion ON dbo.cp_retencion_det.IdEmpresa = dbo.cp_retencion.IdEmpresa AND dbo.cp_retencion_det.IdRetencion = dbo.cp_retencion.IdRetencion INNER JOIN
                  dbo.cp_codigo_SRI ON dbo.cp_retencion_det.IdCodigo_SRI = dbo.cp_codigo_SRI.IdCodigo_SRI INNER JOIN
                  dbo.cp_proveedor AS pro INNER JOIN
                  dbo.cp_orden_giro INNER JOIN
                  dbo.cp_TipoDocumento ON dbo.cp_orden_giro.IdOrden_giro_Tipo = dbo.cp_TipoDocumento.CodTipoDocumento ON pro.IdEmpresa = dbo.cp_orden_giro.IdEmpresa AND pro.IdProveedor = dbo.cp_orden_giro.IdProveedor INNER JOIN
                  dbo.tb_persona AS per ON pro.IdPersona = per.IdPersona ON dbo.cp_retencion.IdCbteCble_Ogiro = dbo.cp_orden_giro.IdCbteCble_Ogiro AND dbo.cp_retencion.IdTipoCbte_Ogiro = dbo.cp_orden_giro.IdTipoCbte_Ogiro AND 
                  dbo.cp_retencion.IdEmpresa_Ogiro = dbo.cp_orden_giro.IdEmpresa LEFT OUTER JOIN
                  dbo.tb_sis_Documento_Tipo_Talonario ON dbo.cp_retencion.IdEmpresa = dbo.tb_sis_Documento_Tipo_Talonario.IdEmpresa AND dbo.cp_retencion.CodDocumentoTipo = dbo.tb_sis_Documento_Tipo_Talonario.CodDocumentoTipo AND 
                  dbo.cp_retencion.serie2 = dbo.tb_sis_Documento_Tipo_Talonario.PuntoEmision AND dbo.cp_retencion.serie1 = dbo.tb_sis_Documento_Tipo_Talonario.Establecimiento AND 
                  dbo.cp_retencion.NumRetencion = dbo.tb_sis_Documento_Tipo_Talonario.NumDocumento LEFT OUTER JOIN
                  dbo.cp_retencion_x_ct_cbtecble LEFT OUTER JOIN
                  dbo.ct_cbtecble ON dbo.cp_retencion_x_ct_cbtecble.ct_IdEmpresa = dbo.ct_cbtecble.IdEmpresa AND dbo.cp_retencion_x_ct_cbtecble.ct_IdTipoCbte = dbo.ct_cbtecble.IdTipoCbte AND 
                  dbo.cp_retencion_x_ct_cbtecble.ct_IdCbteCble = dbo.ct_cbtecble.IdCbteCble ON dbo.cp_retencion.IdEmpresa = dbo.cp_retencion_x_ct_cbtecble.rt_IdEmpresa AND 
                  dbo.cp_retencion.IdRetencion = dbo.cp_retencion_x_ct_cbtecble.rt_IdRetencion LEFT OUTER JOIN
                  dbo.cp_codigo_SRI_x_CtaCble ON dbo.cp_retencion_det.IdEmpresa = dbo.cp_codigo_SRI_x_CtaCble.IdEmpresa AND dbo.cp_retencion_det.IdCodigo_SRI = dbo.cp_codigo_SRI_x_CtaCble.idCodigo_SRI LEFT OUTER JOIN
                  dbo.tb_sucursal AS su ON dbo.cp_orden_giro.IdEmpresa = su.IdEmpresa AND dbo.cp_orden_giro.IdSucursal = su.IdSucursal
WHERE  (dbo.cp_orden_giro.Estado = 'A')
UNION ALL
SELECT OG.IdEmpresa, OG.IdTipoCbte_Ogiro, OG.IdCbteCble_Ogiro, 0, 0, OG.IdOrden_giro_Tipo, OG.IdProveedor, PER.pe_nombreCompleto, PER.pe_cedulaRuc, PRO.pr_direccion, OG.co_FechaFactura, OG.co_serie, OG.co_factura, OG.co_FechaFactura, 
                  OG.Estado, t .Descripcion, og.co_FechaFactura, year(og.co_FechaFactura), 'RTF', ROUND((og.co_subtotal_iva + og.co_subtotal_siniva) - ISNULL(R.re_valor_retencion, 0), 2), 0, '00F', 0, 0, og.IdEmpresa, NULL, NULL, 
                  'Retención de fuente 0%', NULL, NULL, og.co_observacion, og.IdSucursal, s.Su_Descripcion, cast(1 AS bit)
FROM     cp_orden_giro AS OG INNER JOIN
                  cp_proveedor AS PRO ON OG.IdEmpresa = PRO.IdEmpresa AND OG.IdProveedor = PRO.IdProveedor INNER JOIN
                  tb_persona AS PER ON PRO.IdPersona = PER.IdPersona INNER JOIN
                  cp_TipoDocumento AS T ON t .CodTipoDocumento = og.IdOrden_giro_Tipo INNER JOIN
                  tb_sucursal AS s ON og.IdEmpresa = s.IdEmpresa AND og.IdSucursal = s.IdSucursal LEFT JOIN
                      (SELECT C.IdEmpresa_Ogiro, C.IdTipoCbte_Ogiro, C.IdCbteCble_Ogiro, SUM(D .re_baseRetencion) re_valor_retencion
                       FROM      cp_retencion AS c INNER JOIN
                                         cp_retencion_det AS d ON c.IdEmpresa = d .IdEmpresa AND c.IdRetencion = d .IdRetencion
                       WHERE   ltrim(rtrim(d .re_tipoRet)) = 'RTF' AND c.Estado = 'A'
                       GROUP BY C.IdEmpresa_Ogiro, C.IdTipoCbte_Ogiro, C.IdCbteCble_Ogiro) R ON og.IdEmpresa = R.IdEmpresa_Ogiro AND og.IdTipoCbte_Ogiro = R.IdTipoCbte_Ogiro AND og.IdCbteCble_Ogiro = R.IdCbteCble_Ogiro
WHERE  ROUND((og.co_subtotal_iva + og.co_subtotal_siniva) - ISNULL(R.re_valor_retencion, 0), 2) > 0 AND OG.Estado = 'A'
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXP_009';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'          DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_retencion_x_ct_cbtecble"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct_cbtecble"
            Begin Extent = 
               Top = 1183
               Left = 48
               Bottom = 1346
               Right = 260
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_codigo_SRI_x_CtaCble"
            Begin Extent = 
               Top = 1015
               Left = 48
               Bottom = 1178
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "su"
            Begin Extent = 
               Top = 1351
               Left = 48
               Bottom = 1514
               Right = 320
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tb_sis_Documento_Tipo_Talonario"
            Begin Extent = 
               Top = 490
               Left = 1262
               Bottom = 814
               Right = 1535
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
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXP_009';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[35] 4[3] 2[20] 3) )"
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
         Top = -360
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cp_retencion_det"
            Begin Extent = 
               Top = 679
               Left = 48
               Bottom = 842
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_retencion"
            Begin Extent = 
               Top = 509
               Left = 829
               Bottom = 986
               Right = 1070
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_codigo_SRI"
            Begin Extent = 
               Top = 847
               Left = 48
               Bottom = 1010
               Right = 268
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pro"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 322
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp_orden_giro"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 355
            End
            DisplayFlags = 280
            TopColumn = 38
         End
         Begin Table = "cp_TipoDocumento"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 366
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "per"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 322
            End
  ', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWCXP_009';



