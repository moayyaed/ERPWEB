
CREATE VIEW [dbo].[vwfa_fa_notaCreDeb_x_empresa]
AS
SELECT        dbo.fa_notaCreDeb.IdEmpresa, dbo.fa_notaCreDeb.Serie1, dbo.fa_notaCreDeb.Serie2, dbo.tb_empresa.em_ruc, dbo.fa_notaCreDeb.NumNota_Impresa
FROM            dbo.tb_empresa INNER JOIN
                         dbo.fa_notaCreDeb ON dbo.tb_empresa.IdEmpresa = dbo.fa_notaCreDeb.IdEmpresa