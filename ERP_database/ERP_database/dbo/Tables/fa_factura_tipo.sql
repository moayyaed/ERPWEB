CREATE TABLE [dbo].[fa_factura_tipo] (
    [IdEmpresa]             INT            NOT NULL,
    [IdFacturaTipo]         INT            NOT NULL,
    [Codigo]                VARCHAR (50)   NOT NULL,
    [Descripcion]           VARCHAR (1000) NOT NULL,
    [Estado]                BIT            NOT NULL,
    [IdUsuarioCreacion]     VARCHAR (200)  NULL,
    [FechaCreacion]         DATETIME       NULL,
    [IdUsuarioModificacion] VARCHAR (200)  NULL,
    [FechaModificacion]     DATETIME       NULL,
    [IdUsuarioAnulacion]    VARCHAR (200)  NULL,
    [FechaAnulacion]        DATETIME       NULL,
    [MotivoAnulacion]       VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_fa_factura_tipo] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdFacturaTipo] ASC)
);

