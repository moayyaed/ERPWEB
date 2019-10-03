CREATE TABLE [dbo].[cxc_Parametro] (
    [IdEmpresa]                            INT          NOT NULL,
    [pa_IdCaja_x_cobros_x_CXC]             INT          NOT NULL,
    [pa_IdTipoMoviCaja_x_Cobros_x_cliente] INT          NOT NULL,
    [pa_IdTipoCbteCble_CxC]                INT          NOT NULL,
    [DiasTransaccionesAFuturo]             INT          NOT NULL,
    [IdUsuario]                            VARCHAR (20) NULL,
    [FechaTransac]                         DATETIME     NULL,
    [IdUsuarioUltMod]                      VARCHAR (20) NULL,
    [FechaUltMod]                          DATETIME     NULL,
    [IdCtaCble_ProvisionFuente]            VARCHAR (20) NULL,
    [IdCtaCble_ProvisionIva]               VARCHAR (20) NULL,
    [IdPunto_cargo_grupo_Fte]              INT          NULL,
    [IdPunto_cargo_Fte]                    INT          NULL,
    [IdPunto_cargo_grupo_Iva]              INT          NULL,
    [IdPunto_cargo_Iva]                    INT          NULL,
    [IdTipoCbte_LiquidacionRet]            INT          NULL,
    CONSTRAINT [PK_cxc_Parametro] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC),
    CONSTRAINT [FK_cxc_Parametro_caj_Caja] FOREIGN KEY ([IdEmpresa], [pa_IdCaja_x_cobros_x_CXC]) REFERENCES [dbo].[caj_Caja] ([IdEmpresa], [IdCaja]),
    CONSTRAINT [FK_cxc_Parametro_caj_Caja_Movimiento_Tipo] FOREIGN KEY ([IdEmpresa], [pa_IdTipoMoviCaja_x_Cobros_x_cliente]) REFERENCES [dbo].[caj_Caja_Movimiento_Tipo] ([IdEmpresa], [IdTipoMovi]),
    CONSTRAINT [FK_cxc_Parametro_ct_cbtecble_tipo] FOREIGN KEY ([IdEmpresa], [pa_IdTipoCbteCble_CxC]) REFERENCES [dbo].[ct_cbtecble_tipo] ([IdEmpresa], [IdTipoCbte]),
    CONSTRAINT [FK_cxc_Parametro_ct_cbtecble_tipo1] FOREIGN KEY ([IdEmpresa], [IdTipoCbte_LiquidacionRet]) REFERENCES [dbo].[ct_cbtecble_tipo] ([IdEmpresa], [IdTipoCbte]),
    CONSTRAINT [FK_cxc_Parametro_ct_plancta] FOREIGN KEY ([IdEmpresa], [IdCtaCble_ProvisionFuente]) REFERENCES [dbo].[ct_plancta] ([IdEmpresa], [IdCtaCble]),
    CONSTRAINT [FK_cxc_Parametro_ct_plancta1] FOREIGN KEY ([IdEmpresa], [IdCtaCble_ProvisionIva]) REFERENCES [dbo].[ct_plancta] ([IdEmpresa], [IdCtaCble]),
    CONSTRAINT [FK_cxc_Parametro_ct_punto_cargo] FOREIGN KEY ([IdEmpresa], [IdPunto_cargo_Iva]) REFERENCES [dbo].[ct_punto_cargo] ([IdEmpresa], [IdPunto_cargo]),
    CONSTRAINT [FK_cxc_Parametro_ct_punto_cargo_grupo] FOREIGN KEY ([IdEmpresa], [IdPunto_cargo_grupo_Iva]) REFERENCES [dbo].[ct_punto_cargo_grupo] ([IdEmpresa], [IdPunto_cargo_grupo]),
    CONSTRAINT [FK_cxc_Parametro_ct_punto_cargo_grupo1] FOREIGN KEY ([IdEmpresa], [IdPunto_cargo_grupo_Fte]) REFERENCES [dbo].[ct_punto_cargo_grupo] ([IdEmpresa], [IdPunto_cargo_grupo]),
    CONSTRAINT [FK_cxc_Parametro_ct_punto_cargo1] FOREIGN KEY ([IdEmpresa], [IdPunto_cargo_Fte]) REFERENCES [dbo].[ct_punto_cargo] ([IdEmpresa], [IdPunto_cargo]),
    CONSTRAINT [FK_cxc_Parametro_tb_empresa] FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tb_empresa] ([IdEmpresa])
);







