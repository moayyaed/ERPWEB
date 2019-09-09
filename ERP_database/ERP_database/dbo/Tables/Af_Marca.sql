CREATE TABLE [dbo].[Af_Marca] (
    [IdEmpresa]             INT           NOT NULL,
    [IdMarca]               INT           NOT NULL,
    [ma_Descripcion]        VARCHAR (MAX) NOT NULL,
    [Estado]                BIT           NOT NULL,
    [IdUsuarioCreacion]     VARCHAR (50)  NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (50)  NULL,
    [FechaModificacion]     DATETIME      NULL,
    [IdUsuarioAnulacion]    VARCHAR (50)  NULL,
    [FechaAnulacion]        DATETIME      NULL,
    [MotivoAnulacion]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Af_Marca] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdMarca] ASC)
);

