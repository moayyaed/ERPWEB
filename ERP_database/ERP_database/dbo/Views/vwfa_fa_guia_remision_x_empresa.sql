CREATE VIEW [dbo].[vwfa_fa_guia_remision_x_empresa]
AS
SELECT        dbo.fa_guia_remision.IdEmpresa, dbo.tb_empresa.em_ruc, dbo.fa_guia_remision.Serie1, dbo.fa_guia_remision.Serie2, 
                         dbo.fa_guia_remision.NumGuia_Preimpresa
FROM            dbo.tb_empresa INNER JOIN
                         dbo.fa_guia_remision ON dbo.tb_empresa.IdEmpresa = dbo.fa_guia_remision.IdEmpresa