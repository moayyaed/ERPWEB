CREATE TABLE [web].[in_SPINV_022] (
    [IdEmpresa]           INT          NOT NULL,
    [IdSucursal]          INT          NOT NULL,
    [IdBodega]            INT          NOT NULL,
    [IdProducto]          NUMERIC (18) NOT NULL,
    [Stock]               FLOAT (53)   NOT NULL,
    [Costo_promedio]      FLOAT (53)   NOT NULL,
    [Costo_total]         FLOAT (53)   NOT NULL,
    [IdCategoria]         VARCHAR (20) NOT NULL,
    [IdLinea]             INT          NOT NULL,
    [IdGrupo]             INT          NOT NULL,
    [IdSubGrupo]          INT          NOT NULL,
    [IdMarca]             INT          NOT NULL,
    [FechaUltCompra]      DATE         NULL,
    [CostoUltCompra]      FLOAT (53)   NULL,
    [CostoTotalUltCompra] FLOAT (53)   NULL,
    [DiasEnInventario]    INT          NULL,
    [VariacionNIC]        FLOAT (53)   NULL,
    CONSTRAINT [PK_in_SPINV_022] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSucursal] ASC, [IdBodega] ASC, [IdProducto] ASC)
);

