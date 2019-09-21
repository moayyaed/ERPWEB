CREATE TABLE [web].[ct_CONTA_009] (
    [IdUsuario]            VARCHAR (50)    NOT NULL,
    [IdEmpresa]            INT             NOT NULL,
    [IdCtaCble]            VARCHAR (50)    NOT NULL,
    [IdCentroCosto]        VARCHAR (200)   NOT NULL,
    [pc_Cuenta]            VARCHAR (500)   NOT NULL,
    [IdCtaCblePadre]       VARCHAR (50)    NULL,
    [EsCtaUtilidad]        BIT             NOT NULL,
    [IdNivelCta]           INT             NOT NULL,
    [IdGrupoCble]          VARCHAR (5)     NOT NULL,
    [gc_GrupoCble]         VARCHAR (50)    NOT NULL,
    [gc_estado_financiero] VARCHAR (50)    NOT NULL,
    [gc_Orden]             INT             NOT NULL,
    [SaldoFinal]           NUMERIC (18, 2) NOT NULL,
    [SaldoFinalNaturaleza] NUMERIC (18, 2) NOT NULL,
    [EsCuentaMovimiento]   BIT             NOT NULL,
    [Naturaleza]           VARCHAR (2)     NOT NULL,
    [cc_Descripcion]       VARCHAR (500)   NULL,
    [pc_cuenta_padre]      VARCHAR (500)   NULL,
    CONSTRAINT [PK_ct_CONTA_009] PRIMARY KEY CLUSTERED ([IdUsuario] ASC, [IdEmpresa] ASC, [IdCtaCble] ASC, [IdCentroCosto] ASC)
);

