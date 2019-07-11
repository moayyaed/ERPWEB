CREATE TABLE [dbo].[cp_ConciliacionAnticipo] (
    [IdEmpresa]             INT           NOT NULL,
    [IdConciliacion]        NUMERIC (18)  NOT NULL,
    [Fecha]                 DATE          NOT NULL,
    [Observacion]           VARCHAR (MAX) NULL,
    [Estado]                BIT           NOT NULL,
    [Idcancelacion]         NUMERIC (18)  NULL,
    [IdTipoCbte]            INT           NULL,
    [IdCbteCble]            NUMERIC (18)  NULL,
    [IdUsuarioCreacion]     VARCHAR (50)  NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (50)  NULL,
    [FechaModificacion]     DATETIME      NULL,
    [IdUsuarioAnulacion]    VARCHAR (50)  NULL,
    [FechaAnulacion]        DATETIME      NULL,
    [MotivoAnulacion]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_cp_conciliacion] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdConciliacion] ASC),
    CONSTRAINT [FK_cp_ConciliacionAnticipo_ct_cbtecble] FOREIGN KEY ([IdEmpresa], [IdTipoCbte], [IdCbteCble]) REFERENCES [dbo].[ct_cbtecble] ([IdEmpresa], [IdTipoCbte], [IdCbteCble])
);

