CREATE TABLE [web].[ct_CONTA_011] (
    [IdEmpresa]   INT           NOT NULL,
    [IdUsuario]   VARCHAR (200) NOT NULL,
    [Secuencia]   INT           NOT NULL,
    [Grupo]       INT           NOT NULL,
    [Descripcion] VARCHAR (500) NOT NULL,
    [NombreC1]    VARCHAR (500) NOT NULL,
    [NombreC2]    VARCHAR (500) NOT NULL,
    [NombreC3]    VARCHAR (500) NOT NULL,
    [NombreC4]    VARCHAR (500) NOT NULL,
    [NombreC5]    VARCHAR (500) NOT NULL,
    [NombreC6]    VARCHAR (500) NOT NULL,
    [NombreC7]    VARCHAR (500) NOT NULL,
    [Columna1]    FLOAT (53)    NOT NULL,
    [Columna2]    FLOAT (53)    NOT NULL,
    [Columna3]    FLOAT (53)    NOT NULL,
    [Columna4]    FLOAT (53)    NOT NULL,
    [Columna5]    FLOAT (53)    NOT NULL,
    [Columna6]    FLOAT (53)    NOT NULL,
    [Columna7]    FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_ct_SPCONTA_010] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdUsuario] ASC, [Secuencia] ASC)
);



