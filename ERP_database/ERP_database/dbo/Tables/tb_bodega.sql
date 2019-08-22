CREATE TABLE [dbo].[tb_bodega] (
    [IdEmpresa]       INT           NOT NULL,
    [IdSucursal]      INT           NOT NULL,
    [IdBodega]        INT           NOT NULL,
    [cod_bodega]      VARCHAR (50)  NULL,
    [bo_Descripcion]  VARCHAR (MAX) NOT NULL,
    [bo_EsBodega]     BIT           NULL,
    [IdUsuario]       VARCHAR (20)  NULL,
    [Fecha_Transac]   DATETIME      NULL,
    [IdUsuarioUltMod] VARCHAR (20)  NULL,
    [Fecha_UltMod]    DATETIME      NULL,
    [IdUsuarioUltAnu] VARCHAR (20)  NULL,
    [Fecha_UltAnu]    DATETIME      NULL,
    [Estado]          CHAR (1)      NOT NULL,
    [IdCtaCtble_Inve] VARCHAR (20)  NULL,
    CONSTRAINT [PK_tb_bodega] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSucursal] ASC, [IdBodega] ASC),
    CONSTRAINT [FK_tb_bodega_ct_plancta] FOREIGN KEY ([IdEmpresa], [IdCtaCtble_Inve]) REFERENCES [dbo].[ct_plancta] ([IdEmpresa], [IdCtaCble]),
    CONSTRAINT [FK_tb_bodega_tb_sucursal] FOREIGN KEY ([IdEmpresa], [IdSucursal]) REFERENCES [dbo].[tb_sucursal] ([IdEmpresa], [IdSucursal])
);





