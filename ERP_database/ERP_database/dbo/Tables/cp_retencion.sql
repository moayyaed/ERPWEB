CREATE TABLE [dbo].[cp_retencion] (
    [IdEmpresa]           INT           NOT NULL,
    [IdRetencion]         NUMERIC (18)  NOT NULL,
    [CodDocumentoTipo]    VARCHAR (20)  NULL,
    [serie1]              VARCHAR (3)   NULL,
    [serie2]              VARCHAR (3)   NULL,
    [NumRetencion]        VARCHAR (20)  NULL,
    [NAutorizacion]       VARCHAR (50)  NULL,
    [Fecha_Autorizacion]  DATETIME      NULL,
    [fecha]               DATE          NOT NULL,
    [observacion]         VARCHAR (500) NOT NULL,
    [re_Tiene_RTiva]      CHAR (1)      NOT NULL,
    [re_Tiene_RFuente]    CHAR (1)      NOT NULL,
    [IdEmpresa_Ogiro]     INT           NULL,
    [IdCbteCble_Ogiro]    NUMERIC (18)  NULL,
    [IdTipoCbte_Ogiro]    INT           NULL,
    [ct_IdEmpresa_Anu]    INT           NULL,
    [ct_IdTipoCbte_Anu]   INT           NULL,
    [ct_IdCbteCble_Anu]   NUMERIC (18)  NULL,
    [re_EstaImpresa]      CHAR (1)      NOT NULL,
    [Estado]              CHAR (1)      NOT NULL,
    [IdUsuario]           VARCHAR (20)  NOT NULL,
    [Fecha_Transac]       DATETIME      NOT NULL,
    [IdUsuarioUltMod]     VARCHAR (20)  NULL,
    [Fecha_UltMod]        DATETIME      NULL,
    [IdUsuarioUltAnu]     VARCHAR (20)  NULL,
    [nom_pc]              VARCHAR (50)  NULL,
    [ip]                  VARCHAR (50)  NULL,
    [Fecha_UltAnu]        DATETIME      NULL,
    [MotivoAnulacion]     VARCHAR (200) NULL,
    [aprobada_enviar_sri] BIT           NOT NULL,
    [Generado]            BIT           NULL,
    CONSTRAINT [PK_cp_retencion_2] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdRetencion] ASC),
    CONSTRAINT [FK_cp_retencion_cp_orden_giro] FOREIGN KEY ([IdEmpresa_Ogiro], [IdCbteCble_Ogiro], [IdTipoCbte_Ogiro]) REFERENCES [dbo].[cp_orden_giro] ([IdEmpresa], [IdCbteCble_Ogiro], [IdTipoCbte_Ogiro]),
    CONSTRAINT [FK_cp_retencion_ct_cbtecble] FOREIGN KEY ([ct_IdEmpresa_Anu], [ct_IdTipoCbte_Anu], [ct_IdCbteCble_Anu]) REFERENCES [dbo].[ct_cbtecble] ([IdEmpresa], [IdTipoCbte], [IdCbteCble]),
    CONSTRAINT [FK_cp_retencion_tb_empresa] FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tb_empresa] ([IdEmpresa]),
    CONSTRAINT [FK_cp_retencion_tb_sis_Documento_Tipo_Talonario] FOREIGN KEY ([IdEmpresa], [CodDocumentoTipo], [serie2], [serie1], [NumRetencion]) REFERENCES [dbo].[tb_sis_Documento_Tipo_Talonario] ([IdEmpresa], [CodDocumentoTipo], [PuntoEmision], [Establecimiento], [NumDocumento])
);





