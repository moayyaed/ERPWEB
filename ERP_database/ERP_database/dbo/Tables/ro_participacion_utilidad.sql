CREATE TABLE [dbo].[ro_participacion_utilidad] (
    [IdEmpresa]                 INT           NOT NULL,
    [IdUtilidad]                INT           NOT NULL,
    [IdPeriodo]                 INT           NOT NULL,
    [BaseUtilidad]              FLOAT (53)    NOT NULL,
    [Utilidad]                  FLOAT (53)    NOT NULL,
    [UtilidadDerechoIndividual] FLOAT (53)    NOT NULL,
    [UtilidadCargaFamiliar]     FLOAT (53)    NOT NULL,
    [Observacion]               VARCHAR (100) NULL,
    [FechaIngresa]              DATETIME      NULL,
    [UsuarioIngresa]            VARCHAR (25)  NULL,
    [IdUsuarioModifica]         VARCHAR (25)  NULL,
    [Fecha_ultima_modif]        DATETIME      NULL,
    [IdUsuario_anula]           VARCHAR (25)  NULL,
    [Fecha_anulacion]           DATETIME      NULL,
    [Estado]                    VARCHAR (1)   NULL,
    CONSTRAINT [PK_ro_participacion_utilidad] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdUtilidad] ASC)
);





