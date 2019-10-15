CREATE TABLE [dbo].[fa_ProbabilidadCobro] (
    [IdEmpresa]             INT           NOT NULL,
    [IdProbabilidad]        INT           NOT NULL,
    [Descripcion]           VARCHAR (500) NOT NULL,
    [MostrarNoAsignadas]    BIT           NOT NULL,
    [Estado]                BIT           NOT NULL,
    [IdUsuarioCreacion]     VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaModificacion]     DATETIME      NULL,
    [IdUsuarioAnulacion]    VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaAnulacion]        DATETIME      NULL,
    [MotivoAnulacion]       VARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    CONSTRAINT [PK_fa_ProbabilidadCobro] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdProbabilidad] ASC)
);

