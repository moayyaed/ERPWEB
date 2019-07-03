CREATE VIEW vwct_cbtecble_det
AS
SELECT        dbo.ct_cbtecble_det.IdEmpresa, dbo.ct_cbtecble_det.IdTipoCbte, dbo.ct_cbtecble_det.IdCbteCble, dbo.ct_cbtecble_det.secuencia, dbo.ct_cbtecble_det.IdCtaCble, dbo.ct_cbtecble_det.dc_Valor, 
                         dbo.ct_cbtecble_det.dc_Observacion, dbo.ct_cbtecble_det.dc_para_conciliar, dbo.ct_cbtecble_det.IdCtaCble +' - '+ dbo.ct_plancta.pc_Cuenta pc_Cuenta
FROM            dbo.ct_cbtecble_det INNER JOIN
                         dbo.ct_plancta ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_plancta.IdEmpresa AND dbo.ct_cbtecble_det.IdCtaCble = dbo.ct_plancta.IdCtaCble
GO



GO


