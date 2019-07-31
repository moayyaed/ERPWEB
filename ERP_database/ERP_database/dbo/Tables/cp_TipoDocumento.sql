CREATE TABLE [dbo].[cp_TipoDocumento] (
    [CodTipoDocumento]                VARCHAR (5)   NOT NULL,
    [Codigo]                          VARCHAR (10)  NOT NULL,
    [Descripcion]                     VARCHAR (250) NOT NULL,
    [Orden]                           INT           NOT NULL,
    [DeclaraSRI]                      VARCHAR (1)   NOT NULL,
    [CodSRI]                          VARCHAR (5)   NULL,
    [Estado]                          VARCHAR (1)   NOT NULL,
    [IdUsuario]                       VARCHAR (20)  NULL,
    [Fecha_Transac]                   DATETIME      NULL,
    [IdUsuarioUltMod]                 VARCHAR (20)  NULL,
    [Fecha_UltMod]                    DATETIME      NULL,
    [IdUsuarioUltAnu]                 VARCHAR (20)  NULL,
    [Fecha_UltAnu]                    DATETIME      NULL,
    [GeneraRetencion]                 VARCHAR (1)   NULL,
    [Codigo_Secuenciales_Transaccion] VARCHAR (250) NULL,
    [Sustento_Tributario]             VARCHAR (250) NULL,
    CONSTRAINT [PK_cp_TipoDocumento] PRIMARY KEY CLUSTERED ([CodTipoDocumento] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_cp_TipoDocumento]
    ON [dbo].[cp_TipoDocumento]([CodTipoDocumento] ASC);

