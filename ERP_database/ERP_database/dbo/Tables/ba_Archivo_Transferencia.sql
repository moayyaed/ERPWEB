CREATE TABLE [dbo].[ba_Archivo_Transferencia] (
    [IdEmpresa]          INT           NOT NULL,
    [IdArchivo]          NUMERIC (18)  NOT NULL,
    [cod_archivo]        VARCHAR (50)  NULL,
    [IdBanco]            INT           NOT NULL,
    [IdProceso_bancario] INT           NOT NULL,
    [Cod_Empresa]        VARCHAR (30)  NOT NULL,
    [Nom_Archivo]        VARCHAR (200) NOT NULL,
    [Fecha]              DATETIME      NOT NULL,
    [Estado]             BIT           NOT NULL,
    [Observacion]        VARCHAR (MAX) NOT NULL,
    [IdUsuario]          VARCHAR (50)  NULL,
    [Fecha_Transac]      DATETIME      NULL,
    [IdUsuarioUltMod]    VARCHAR (50)  NULL,
    [Fecha_UltMod]       DATETIME      NULL,
    [IdUsuarioUltAnu]    VARCHAR (50)  NULL,
    [Fecha_UltAnu]       DATETIME      NULL,
    [Motivo_anulacion]   VARCHAR (MAX) NULL,
    [Fecha_Proceso]      DATETIME      NULL,
    [Contabilizado]      BIT           NULL,
    [IdSucursal]         INT           NOT NULL,
    CONSTRAINT [PK_ba_Archivo_Transferencia] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdArchivo] ASC),
    CONSTRAINT [FK_ba_Archivo_Transferencia_ba_Banco_Cuenta] FOREIGN KEY ([IdEmpresa], [IdBanco]) REFERENCES [dbo].[ba_Banco_Cuenta] ([IdEmpresa], [IdBanco])
);












GO
CREATE NONCLUSTERED INDEX [IX_ba_Archivo_Transferencia]
    ON [dbo].[ba_Archivo_Transferencia]([IdEmpresa] ASC, [IdArchivo] ASC, [IdBanco] ASC, [IdProceso_bancario] ASC);

