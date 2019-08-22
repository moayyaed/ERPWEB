CREATE TABLE [web].[in_SPINV_009] (
    [IdEmpresa]       INT           NOT NULL,
    [IdUsuario]       VARCHAR (50)  NOT NULL,
    [IdProducto]      NUMERIC (18)  NOT NULL,
    [IdSucursal]      INT           NOT NULL,
    [IdBodega]        INT           NOT NULL,
    [IdCategoria]     INT           NOT NULL,
    [IdLinea]         INT           NOT NULL,
    [IdGrupo]         INT           NOT NULL,
    [IdSubGrupo]      INT           NOT NULL,
    [pr_codigo]       VARCHAR (200) NULL,
    [pr_descripcion]  VARCHAR (MAX) NOT NULL,
    [IdUnidadMedida]  VARCHAR (50)  NOT NULL,
    [NomUnidadMedida] VARCHAR (500) NOT NULL,
    [NomCategoria]    VARCHAR (500) NOT NULL,
    [NomLinea]        VARCHAR (500) NOT NULL,
    [NomGrupo]        VARCHAR (500) NOT NULL,
    [NomSubGrupo]     VARCHAR (500) NOT NULL,
    [CantidadInicial] FLOAT (53)    NOT NULL,
    [CostoInicial]    FLOAT (53)    NOT NULL,
    [CantidadIngreso] FLOAT (53)    NOT NULL,
    [CostoIngreso]    FLOAT (53)    NOT NULL,
    [CantidadEgreso]  FLOAT (53)    NOT NULL,
    [CostoEgreso]     FLOAT (53)    NOT NULL,
    [CantidadFinal]   FLOAT (53)    NOT NULL,
    [CostoFinal]      FLOAT (53)    NOT NULL,
    [Su_Descripcion]  VARCHAR (MAX) NULL,
    [bo_Bodega]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_in_SPINV_009] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdUsuario] ASC, [IdProducto] ASC, [IdSucursal] ASC, [IdBodega] ASC)
);



