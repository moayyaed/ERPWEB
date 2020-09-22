CREATE TABLE [dbo].[cxc_cobro] (
    [IdEmpresa]          INT           NOT NULL,
    [IdSucursal]         INT           NOT NULL,
    [IdCobro]            NUMERIC (18)  NOT NULL,
    [IdCobro_a_aplicar]  NUMERIC (18)  NULL,
    [cr_Codigo]          VARCHAR (10)  NULL,
    [IdCobro_tipo]       VARCHAR (20)  NULL,
    [IdCliente]          NUMERIC (18)  NOT NULL,
    [cr_TotalCobro]      FLOAT (53)    NOT NULL,
    [cr_Excedente]       FLOAT (53)    NULL,
    [cr_EsElectronico]   BIT           NULL,
    [cr_fecha]           DATE          NOT NULL,
    [cr_fechaDocu]       DATE          NOT NULL,
    [cr_fechaCobro]      DATE          NOT NULL,
    [cr_observacion]     VARCHAR (MAX) NOT NULL,
    [cr_Banco]           VARCHAR (500) NULL,
    [cr_cuenta]          VARCHAR (500) NULL,
    [cr_NumDocumento]    VARCHAR (100) NULL,
    [cr_Tarjeta]         VARCHAR (50)  NULL,
    [cr_propietarioCta]  VARCHAR (100) NULL,
    [cr_estado]          NVARCHAR (1)  NULL,
    [cr_recibo]          NUMERIC (18)  NULL,
    [Fecha_Transac]      DATETIME      NULL,
    [IdUsuario]          VARCHAR (20)  NULL,
    [IdUsuarioUltMod]    VARCHAR (20)  NULL,
    [Fecha_UltMod]       DATETIME      NULL,
    [IdUsuarioUltAnu]    VARCHAR (20)  NULL,
    [Fecha_UltAnu]       DATETIME      NULL,
    [IdBanco]            INT           NULL,
    [IdCaja]             INT           NOT NULL,
    [MotiAnula]          VARCHAR (50)  NULL,
    [IdTipoNotaCredito]  INT           NULL,
    [cr_EsProvision]     BIT           NULL,
    [NumeroAutorizacion] VARCHAR (500) NULL,
    CONSTRAINT [PK_cxc_cobro] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSucursal] ASC, [IdCobro] ASC),
    CONSTRAINT [FK_cxc_cobro_ba_Banco_Cuenta] FOREIGN KEY ([IdEmpresa], [IdBanco]) REFERENCES [dbo].[ba_Banco_Cuenta] ([IdEmpresa], [IdBanco]),
    CONSTRAINT [FK_cxc_cobro_caj_Caja] FOREIGN KEY ([IdEmpresa], [IdCaja]) REFERENCES [dbo].[caj_Caja] ([IdEmpresa], [IdCaja]),
    CONSTRAINT [FK_cxc_cobro_cxc_cobro_tipo] FOREIGN KEY ([IdCobro_tipo]) REFERENCES [dbo].[cxc_cobro_tipo] ([IdCobro_tipo]),
    CONSTRAINT [FK_cxc_cobro_fa_cliente] FOREIGN KEY ([IdEmpresa], [IdCliente]) REFERENCES [dbo].[fa_cliente] ([IdEmpresa], [IdCliente]),
    CONSTRAINT [FK_cxc_cobro_tb_sucursal] FOREIGN KEY ([IdEmpresa], [IdSucursal]) REFERENCES [dbo].[tb_sucursal] ([IdEmpresa], [IdSucursal])
);













