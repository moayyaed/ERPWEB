CREATE TABLE [dbo].[ct_cbtecble_det] (
    [IdEmpresa]          INT           NOT NULL,
    [IdTipoCbte]         INT           NOT NULL,
    [IdCbteCble]         NUMERIC (18)  NOT NULL,
    [secuencia]          INT           NOT NULL,
    [IdCtaCble]          VARCHAR (20)  NOT NULL,
    [dc_Valor]           FLOAT (53)    NOT NULL,
    [dc_Observacion]     VARCHAR (MAX) NULL,
    [dc_para_conciliar]  BIT           NULL,
    CONSTRAINT [PK_ct_cbtecble_det_1] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdTipoCbte] ASC, [IdCbteCble] ASC, [secuencia] ASC),
    CONSTRAINT [FK_ct_cbtecble_det_ct_cbtecble] FOREIGN KEY ([IdEmpresa], [IdTipoCbte], [IdCbteCble]) REFERENCES [dbo].[ct_cbtecble] ([IdEmpresa], [IdTipoCbte], [IdCbteCble]),
    CONSTRAINT [FK_ct_cbtecble_det_ct_plancta] FOREIGN KEY ([IdEmpresa], [IdCtaCble]) REFERENCES [dbo].[ct_plancta] ([IdEmpresa], [IdCtaCble])
);




GO
CREATE NONCLUSTERED INDEX [IX_ct_cbtecble_det]
    ON [dbo].[ct_cbtecble_det]([IdEmpresa] ASC, [IdTipoCbte] ASC, [IdCbteCble] ASC, [secuencia] ASC, [IdCtaCble] ASC);

