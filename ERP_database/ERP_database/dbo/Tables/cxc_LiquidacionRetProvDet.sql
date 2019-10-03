CREATE TABLE [dbo].[cxc_LiquidacionRetProvDet] (
    [IdEmpresa]     INT          NOT NULL,
    [IdLiquidacion] NUMERIC (18) NOT NULL,
    [Secuencia]     INT          NOT NULL,
    [IdSucursal]    INT          NOT NULL,
    [IdCobro]       NUMERIC (18) NOT NULL,
    [secuencial]    INT          NOT NULL,
    [IdCobro_tipo]  VARCHAR (20) NOT NULL,
    [Valor]         FLOAT (53)   NOT NULL,
    CONSTRAINT [PK_cxc_LiquidacionRetProvDet_1] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdLiquidacion] ASC, [Secuencia] ASC, [IdSucursal] ASC),
    CONSTRAINT [FK_cxc_LiquidacionRetProvDet_cxc_cobro] FOREIGN KEY ([IdEmpresa], [IdSucursal], [IdCobro]) REFERENCES [dbo].[cxc_cobro] ([IdEmpresa], [IdSucursal], [IdCobro]),
    CONSTRAINT [FK_cxc_LiquidacionRetProvDet_cxc_cobro_det] FOREIGN KEY ([IdEmpresa], [IdSucursal], [IdCobro], [secuencial]) REFERENCES [dbo].[cxc_cobro_det] ([IdEmpresa], [IdSucursal], [IdCobro], [secuencial]),
    CONSTRAINT [FK_cxc_LiquidacionRetProvDet_cxc_LiquidacionRetProv] FOREIGN KEY ([IdEmpresa], [IdSucursal], [IdLiquidacion]) REFERENCES [dbo].[cxc_LiquidacionRetProv] ([IdEmpresa], [IdSucursal], [IdLiquidacion])
);



