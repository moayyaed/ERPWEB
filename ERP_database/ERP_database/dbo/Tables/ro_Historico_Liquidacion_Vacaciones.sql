﻿CREATE TABLE [dbo].[ro_Historico_Liquidacion_Vacaciones] (
    [IdEmpresa]       INT           NOT NULL,
    [IdEmpleado]      NUMERIC (18)  NOT NULL,
    [IdLiquidacion]   INT           NOT NULL,
    [IdOrdenPago]     NUMERIC (9)   NOT NULL,
    [IdEmpresa_OP]    INT           NOT NULL,
    [ValorCancelado]  FLOAT (53)    NOT NULL,
    [FechaPago]       DATE          NOT NULL,
    [Observaciones]   VARCHAR (500) NULL,
    [IdUsuario]       VARCHAR (25)  NULL,
    [Estado]          VARCHAR (1)   NULL,
    [Fecha_Transac]   DATETIME      NULL,
    [Fecha_UltMod]    DATETIME      NULL,
    [IdUsuarioUltMod] VARCHAR (25)  NULL,
    [FechaHoraAnul]   DATETIME      NULL,
    [MotiAnula]       VARCHAR (200) NULL,
    [IdUsuarioUltAnu] VARCHAR (25)  NULL,
    [EstadoContrato]  VARCHAR (10)  NULL,
    [Iess]            FLOAT (53)    NULL,
    [IdTipo_op]       VARCHAR (20)  NULL,
    [Gozadas_Pagadas] VARCHAR (10)  NULL,
    [IdSolicitud]     INT           NOT NULL,
    CONSTRAINT [PK_ro_Historico_Liquidacion_Vacaciones] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdEmpleado] ASC, [IdLiquidacion] ASC),
    CONSTRAINT [FK_ro_Historico_Liquidacion_Vacaciones_ro_empleado] FOREIGN KEY ([IdEmpresa], [IdEmpleado]) REFERENCES [dbo].[ro_empleado] ([IdEmpresa], [IdEmpleado]),
    CONSTRAINT [FK_ro_Historico_Liquidacion_Vacaciones_tb_empresa] FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tb_empresa] ([IdEmpresa]),
    CONSTRAINT [FK_ro_Historico_Liquidacion_Vacaciones_tb_empresa1] FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tb_empresa] ([IdEmpresa])
);



