CREATE view [dbo].[vwcp_Orden_Giro_Pendiente_Conciliar]
as
SELECT        og.IdEmpresa, og.IdCbteCble_Ogiro, og.IdTipoCbte_Ogiro, og.IdOrden_giro_Tipo, og.IdProveedor, pe_nombreCompleto AS nombreProveedor, og.co_fechaOg, 
                         og.co_factura, og.co_FechaFactura, og.co_observacion, og.co_subtotal_iva, og.co_subtotal_siniva, og.co_baseImponible, og.co_total, og.Estado, 
                         rete.IdEmpresa AS IdEmpresa_ret, rete.IdRetencion, rete.serie1 + '-' + rete.serie2 AS re_serie, rete.NumRetencion AS re_NumRetencion, rete.re_EstaImpresa, 
                         '' AS TipoFlujo, og.IdIden_credito, (CASE WHEN (rtrim(ltrim(og.co_serie)) = '' OR
                         rtrim(ltrim(og.co_serie)) IS NULL) THEN '000' ELSE (substring(og.co_serie, 1, 3)) END) AS Serie, (CASE WHEN (rtrim(ltrim(og.co_serie)) = '' OR
                         rtrim(ltrim(og.co_serie)) IS NULL) THEN '000' ELSE (substring(og.co_serie, 5, 3)) END) AS Serie2, og.co_factura AS numDocFactura, og.Num_Autorizacion, 
                         og.Num_Autorizacion_Imprenta, 0 co_OtroValor_a_descontar, og.co_Por_iva, og.co_valoriva, og.fecha_autorizacion, dbo.vwct_cbtecble_con_ctacble_acreedora.IdCtaCble_Acreedora IdCtaCble_Gasto, '' IdCtaCble_IVA
                         
FROM            dbo.cp_orden_giro AS og INNER JOIN
                         dbo.cp_proveedor AS pro ON og.IdEmpresa = pro.IdEmpresa AND og.IdProveedor = pro.IdProveedor INNER JOIN
                         dbo.vwct_cbtecble_con_ctacble_acreedora ON og.IdEmpresa = dbo.vwct_cbtecble_con_ctacble_acreedora.IdEmpresa AND 
                         og.IdTipoCbte_Ogiro = dbo.vwct_cbtecble_con_ctacble_acreedora.IdTipoCbte AND 
                         og.IdCbteCble_Ogiro = dbo.vwct_cbtecble_con_ctacble_acreedora.IdCbteCble LEFT OUTER JOIN
                         dbo.cp_retencion AS rete ON og.IdEmpresa = rete.IdEmpresa_Ogiro AND og.IdCbteCble_Ogiro = rete.IdCbteCble_Ogiro AND 
                         og.IdTipoCbte_Ogiro = rete.IdTipoCbte_Ogiro LEFT OUTER JOIN
                         
                         dbo.tb_sucursal AS su ON og.IdSucursal = su.IdSucursal AND og.IdEmpresa = su.IdEmpresa inner join tb_persona as per
						 on per.IdPersona = pro.IdPersona
WHERE        (NOT EXISTS
                             (SELECT        IdEmpresa, IdAprobacion, Fecha_aprobacion, IdEmpresa_Ogiro, IdCbteCble_Ogiro, IdTipoCbte_Ogiro, IdOrden_giro_Tipo, IdIden_credito, IdProveedor, 
                                                         Observacion, Serie, Serie2, num_documento, num_auto_Proveedor, num_auto_Imprenta, Fecha_Factura, co_subtotal_iva, co_subtotal_siniva, 
                                                         Descuento, co_baseImponible, co_Por_iva, co_valoriva, co_total
                               FROM            dbo.cp_Aprobacion_Ing_Bod_x_OC AS apro
                               WHERE        (IdEmpresa_Ogiro = og.IdEmpresa) AND (IdCbteCble_Ogiro = og.IdCbteCble_Ogiro) AND (IdTipoCbte_Ogiro = og.IdTipoCbte_Ogiro) AND 
                                                         (IdOrden_giro_Tipo = og.IdOrden_giro_Tipo)))
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
         Begin Table = "og"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pro"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 267
               Right = 259
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rete"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 399
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "flu"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 531
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "su"
            Begin Extent = 
               Top = 534
               Left = 38
               Bottom = 663
               Right = 268
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcp_Orden_Giro_Pendiente_Conciliar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcp_Orden_Giro_Pendiente_Conciliar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwcp_Orden_Giro_Pendiente_Conciliar';

