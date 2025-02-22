﻿CREATE TABLE [dbo].[ro_canasta_basica] (
    [Anio]                   INT             NOT NULL,
    [valorCanasta]           NUMERIC (18, 2) NOT NULL,
    [MultiploCanastaBasica]  FLOAT (53)      NOT NULL,
    [MultiploFraccionBasica] FLOAT (53)      NOT NULL,
    [Observacion]            VARCHAR (500)   NULL,
    [IdUsuario]              VARCHAR (50)    NULL,
    [IdUsuarioUltMod]        VARCHAR (50)    NULL,
    [FechaTansaccion]        DATETIME        NULL,
    [FechaUltModi]           DATETIME        NULL,
    CONSTRAINT [PK_ro_canasta_basica] PRIMARY KEY CLUSTERED ([Anio] ASC)
);



