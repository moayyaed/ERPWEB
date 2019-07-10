CREATE TABLE [dbo].[fa_guia_remision_det] (
    [IdEmpresa]        INT           NOT NULL,
    [IdSucursal]       INT           NOT NULL,
    [IdBodega]         INT           NOT NULL,
    [IdGuiaRemision]   NUMERIC (18)  NOT NULL,
    [Secuencia]        INT           NOT NULL,
    [IdProducto]       NUMERIC (18)  NOT NULL,
    [gi_cantidad]      FLOAT (53)    NOT NULL,
    [gi_detallexItems] VARCHAR (MAX) NULL,
    [gi_precio]        FLOAT (53)    NOT NULL,
    [gi_por_desc]      FLOAT (53)    NOT NULL,
    [gi_descuentoUni]  FLOAT (53)    NOT NULL,
    [gi_PrecioFinal]   FLOAT (53)    NOT NULL,
    [gi_Subtotal]      FLOAT (53)    NOT NULL,
    [IdCod_Impuesto]   VARCHAR (25)  NOT NULL,
    [gi_por_iva]       FLOAT (53)    NOT NULL,
    [gi_Iva]           FLOAT (53)    NOT NULL,
    [gi_Total]         FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_fa_guia_remision_det] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSucursal] ASC, [IdBodega] ASC, [IdGuiaRemision] ASC, [Secuencia] ASC),
    CONSTRAINT [FK_fa_guia_remision_det_fa_guia_remision] FOREIGN KEY ([IdEmpresa], [IdSucursal], [IdBodega], [IdGuiaRemision]) REFERENCES [dbo].[fa_guia_remision] ([IdEmpresa], [IdSucursal], [IdBodega], [IdGuiaRemision]),
    CONSTRAINT [FK_fa_guia_remision_det_in_Producto] FOREIGN KEY ([IdEmpresa], [IdProducto]) REFERENCES [dbo].[in_Producto] ([IdEmpresa], [IdProducto]),
    CONSTRAINT [FK_fa_guia_remision_det_tb_sis_Impuesto] FOREIGN KEY ([IdCod_Impuesto]) REFERENCES [dbo].[tb_sis_Impuesto] ([IdCod_Impuesto])
);



