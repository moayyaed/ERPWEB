CREATE TABLE [dbo].[fa_ProbabilidadCobroDet] (
    [IdEmpresa]      INT          NOT NULL,
    [IdProbabilidad] INT          NOT NULL,
    [Secuencia]      INT          NOT NULL,
    [IdSucursal]     INT          NOT NULL,
    [IdBodega]       INT          NOT NULL,
    [IdCbteVta]      NUMERIC (18) NOT NULL,
    [vt_tipoDoc]     VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_fa_ProbabilidadCobroDet] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdProbabilidad] ASC, [Secuencia] ASC),
    CONSTRAINT [FK_fa_ProbabilidadCobroDet_fa_ProbabilidadCobro] FOREIGN KEY ([IdEmpresa], [IdProbabilidad]) REFERENCES [dbo].[fa_ProbabilidadCobro] ([IdEmpresa], [IdProbabilidad])
);

