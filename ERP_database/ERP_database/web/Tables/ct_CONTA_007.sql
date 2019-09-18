CREATE TABLE [web].[ct_CONTA_007] (
    [IdEmpresa]          INT             NOT NULL,
    [IdUsuario]          VARCHAR (50)    NOT NULL,
    [Secuencia]          VARCHAR (500)   NOT NULL,
    [IdCtaCble]          VARCHAR (200)   NULL,
    [pc_cuenta]          VARCHAR (500)   NOT NULL,
    [Tipo]               VARCHAR (10)    NOT NULL,
    [TipoOrden]          INT             NOT NULL,
    [Clasificacion]      VARCHAR (50)    NOT NULL,
    [ClasificacionOrden] INT             NOT NULL,
    [Valor]              NUMERIC (18, 2) NOT NULL,
    CONSTRAINT [PK_ct_SPCONTA_007] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdUsuario] ASC, [Secuencia] ASC)
);

