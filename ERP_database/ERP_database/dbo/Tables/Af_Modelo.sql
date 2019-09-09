CREATE TABLE [dbo].[Af_Modelo] (
    [IdEmpresa]             INT           NOT NULL,
    [IdModelo]              INT           NOT NULL,
    [mo_Descripcion]        VARCHAR (MAX) NOT NULL,
    [Estado]                BIT           NOT NULL,
    [IdUsuarioCreacion]     VARCHAR (50)  NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (50)  NULL,
    [FechaModificacion]     DATETIME      NULL,
    [IdUsuarioAnulacion]    VARCHAR (50)  NULL,
    [FechaAnulacion]        DATETIME      NULL,
    [MotivoAnulacion]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Af_Modelo] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdModelo] ASC)
);

