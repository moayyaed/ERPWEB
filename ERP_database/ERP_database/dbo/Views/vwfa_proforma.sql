CREATE VIEW dbo.vwfa_proforma
AS
SELECT        dbo.fa_proforma.IdEmpresa, dbo.fa_proforma.IdSucursal, dbo.fa_proforma.IdProforma, dbo.fa_proforma.IdCliente, dbo.fa_proforma.IdTerminoPago, dbo.fa_proforma.pf_plazo, dbo.fa_proforma.pf_codigo, 
                         dbo.fa_proforma.pf_observacion, dbo.fa_proforma.pf_fecha, dbo.fa_proforma.pf_fecha_vcto, dbo.tb_sucursal.Su_Descripcion, dbo.tb_persona.pe_nombreCompleto, ISNULL(pdet.pd_subtotal, 0) AS pd_subtotal, 
                         ISNULL(pdet.pd_iva, 0) AS pd_iva, ISNULL(pdet.pd_total, 0) AS pd_total, dbo.fa_proforma.estado, dbo.fa_proforma.IdUsuario_creacion, dbo.fa_proforma.fecha_creacion, dbo.fa_proforma.IdUsuario_modificacion, 
                         dbo.fa_proforma.fecha_modificacion, dbo.fa_proforma.IdUsuario_anulacion, dbo.fa_proforma.fecha_anulacion, dbo.fa_proforma.IdBodega, dbo.fa_proforma.IdVendedor, dbo.fa_proforma.pr_dias_entrega, 
                         CASE WHEN pdet.anulado = 1 THEN 'CERRADO' ELSE IIF((pd_cantidad - ISNULL(vt_cantidad, 0)) != pd_cantidad, 'FACTURADO', 'ABIERTO') END AS EstadoCierre
FROM            dbo.fa_proforma INNER JOIN
                         dbo.fa_cliente ON dbo.fa_proforma.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_proforma.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                         dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona INNER JOIN
                         dbo.tb_sucursal ON dbo.fa_proforma.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.fa_proforma.IdSucursal = dbo.tb_sucursal.IdSucursal LEFT JOIN
                             (SELECT        d .IdEmpresa, d .IdSucursal, d .IdProforma, sum(d .pd_subtotal) pd_subtotal, sum(d .pd_iva) pd_iva, sum(d .pd_total) pd_total, max(CAST(d .anulado AS INT)) anulado, sum(d .pd_cantidad) pd_cantidad
                               FROM            fa_proforma_det d
                               GROUP BY d .IdEmpresa, d .IdSucursal, d .IdProforma) AS pdet ON dbo.fa_proforma.IdEmpresa = pdet.IdEmpresa AND dbo.fa_proforma.IdSucursal = pdet.IdSucursal AND dbo.fa_proforma.IdProforma = pdet.IdProforma LEFT 
                         JOIN
                             (SELECT        fd.IdEmpresa_pf, fd.IdSucursal_pf, fd.IdProforma, sum(fd.vt_cantidad) vt_cantidad
                               FROM            fa_factura_det AS fd
                               GROUP BY fd.IdEmpresa_pf, fd.IdSucursal_pf, fd.IdProforma) AS dfet ON dbo.fa_proforma.IdEmpresa = dfet.IdEmpresa_pf AND dbo.fa_proforma.IdSucursal = dfet.IdSucursal_pf AND 
                         dbo.fa_proforma.IdProforma = dfet.IdProforma
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
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 27
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwfa_proforma';




GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwfa_proforma';



