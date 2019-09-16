CREATE TABLE [dbo].[cxc_LiquidacionRetProv] (
    [IdEmpresa]             INT           NOT NULL,
    [IdLiquidacion]         NUMERIC (18)  NOT NULL,
    [li_Fecha]              DATE          NOT NULL,
    [Observacion]           VARCHAR (MAX) NULL,
    [Estado]                BIT           NOT NULL,
    [IdTipoCbte]            INT           NULL,
    [IdCbteCble]            NUMERIC (18)  NULL,
    [IdUsuarioCreacion]     VARCHAR (50)  NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (50)  NULL,
    [FechaModificacion]     DATETIME      NULL,
    [IdUsuarioAnulacion]    VARCHAR (50)  NULL,
    [FechaAnulacion]        DATETIME      NULL,
    [MotivoAnulacion]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_cxc_LiquidacionRetProv] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdLiquidacion] ASC),
    CONSTRAINT [FK_cxc_LiquidacionRetProv_ct_cbtecble] FOREIGN KEY ([IdEmpresa], [IdTipoCbte], [IdCbteCble]) REFERENCES [dbo].[ct_cbtecble] ([IdEmpresa], [IdTipoCbte], [IdCbteCble])
);

