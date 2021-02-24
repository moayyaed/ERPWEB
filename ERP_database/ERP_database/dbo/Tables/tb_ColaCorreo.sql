CREATE TABLE [dbo].[tb_ColaCorreo] (
    [IdEmpresa]         INT            NOT NULL,
    [IdCorreo]          NUMERIC (18)   NOT NULL,
    [Codigo]            VARCHAR (50)   COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Destinatarios]     VARCHAR (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Asunto]            VARCHAR (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Cuerpo]            VARCHAR (MAX)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Parametros]        VARCHAR (200)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [IdUsuarioCreacion] VARCHAR (50)   COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [FechaCreacion]     DATE           NOT NULL,
    [FechaEnvio]        DATETIME       NULL,
    [Error]             VARCHAR (MAX)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    CONSTRAINT [PK_tb_ColaCorreo] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdCorreo] ASC)
);

