CREATE TABLE [dbo].[tb_ColaCorreoParametros] (
    [IdEmpresa]             INT           NOT NULL,
    [Usuario]               VARCHAR (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Contrasenia]           VARCHAR (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Puerto]                INT           NOT NULL,
    [Host]                  VARCHAR (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [PermitirSSL]           BIT           NOT NULL,
    [CorreoCopia]           VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [IdUsuarioCreacion]     VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FechaModificacion]     DATETIME      NULL,
    CONSTRAINT [PK_tb_ColaCorreoParametros] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC)
);

