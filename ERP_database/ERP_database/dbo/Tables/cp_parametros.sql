﻿CREATE TABLE [dbo].[cp_parametros] (
    [IdEmpresa]                        INT          NOT NULL,
    [pa_TipoCbte_OG]                   INT          NULL,
    [pa_ctacble_iva]                   VARCHAR (20) NULL,
    [pa_IdTipoCbte_x_Retencion]        INT          NULL,
    [IdUsuario]                        VARCHAR (20) NULL,
    [IdUsuarioUltMod]                  VARCHAR (20) NULL,
    [FechaUltMod]                      DATETIME     NULL,
    [pa_TipoCbte_NC]                   INT          NULL,
    [pa_TipoCbte_ND]                   INT          NULL,
    [pa_TipoCbte_para_conci_x_antcipo] INT          NULL,
    [DiasTransaccionesAFuturo]         INT          NOT NULL,
    [SeValidaCtaGasto]                 BIT          NULL,
    CONSTRAINT [PK_cp_parametros] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC),
    CONSTRAINT [FK_cp_parametros_ct_cbtecble_tipo] FOREIGN KEY ([IdEmpresa], [pa_TipoCbte_OG]) REFERENCES [dbo].[ct_cbtecble_tipo] ([IdEmpresa], [IdTipoCbte]),
    CONSTRAINT [FK_cp_parametros_ct_cbtecble_tipo2] FOREIGN KEY ([IdEmpresa], [pa_IdTipoCbte_x_Retencion]) REFERENCES [dbo].[ct_cbtecble_tipo] ([IdEmpresa], [IdTipoCbte]),
    CONSTRAINT [FK_cp_parametros_ct_cbtecble_tipo4] FOREIGN KEY ([IdEmpresa], [pa_TipoCbte_NC]) REFERENCES [dbo].[ct_cbtecble_tipo] ([IdEmpresa], [IdTipoCbte]),
    CONSTRAINT [FK_cp_parametros_ct_cbtecble_tipo6] FOREIGN KEY ([IdEmpresa], [pa_TipoCbte_ND]) REFERENCES [dbo].[ct_cbtecble_tipo] ([IdEmpresa], [IdTipoCbte]),
    CONSTRAINT [FK_cp_parametros_ct_cbtecble_tipo8] FOREIGN KEY ([IdEmpresa], [pa_TipoCbte_para_conci_x_antcipo]) REFERENCES [dbo].[ct_cbtecble_tipo] ([IdEmpresa], [IdTipoCbte]),
    CONSTRAINT [FK_cp_parametros_ct_plancta1] FOREIGN KEY ([IdEmpresa], [pa_ctacble_iva]) REFERENCES [dbo].[ct_plancta] ([IdEmpresa], [IdCtaCble]),
    CONSTRAINT [FK_cp_parametros_tb_empresa] FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tb_empresa] ([IdEmpresa])
);





