CREATE TABLE [dbo].[cxc_SeguimientoCartera] (
    [IdEmpresa]             INT           NOT NULL,
    [IdSeguimiento]         INT           NOT NULL,
    [IdCliente]             NUMERIC (18)  NOT NULL,
    [CorreoEnviado]         BIT           NOT NULL,
    [Fecha]                 DATETIME      NOT NULL,
    [Observacion]           VARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Estado]                BIT           NOT NULL,
    [IdUsuarioCreacion]     VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaModificacion]     DATETIME      NULL,
    [IdUsuarioAnulacion]    VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaAnulacion]        DATETIME      NULL,
    [MotivoAnulacion]       VARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    CONSTRAINT [PK_cxc_SeguimientoCartera] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSeguimiento] ASC),
    CONSTRAINT [FK_cxc_SeguimientoCartera_fa_cliente] FOREIGN KEY ([IdEmpresa], [IdCliente]) REFERENCES [dbo].[fa_cliente] ([IdEmpresa], [IdCliente])
);

