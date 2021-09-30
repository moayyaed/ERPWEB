CREATE TABLE [dbo].[ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado] (
    [IdEmpresa]        INT           NOT NULL,
    [IdSolicitud]      INT           NOT NULL,
    [Secuencia]        INT           NOT NULL,
    [IdEmpleado]       NUMERIC (18)  NOT NULL,
    [IdPeriodo_Inicio] INT           NOT NULL,
    [IdPeriodo_Fin]    INT           NOT NULL,
    [Dias_tomados]     INT           NOT NULL,
    [Observacion]      VARCHAR (MAX) NOT NULL,
    [Tipo_liquidacion] VARCHAR (50)  NOT NULL,
    [Tipo_vacacion]    VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado_1] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSolicitud] ASC, [Secuencia] ASC),
    CONSTRAINT [FK_ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado_ro_Solicitud_Vacaciones_x_empleado] FOREIGN KEY ([IdEmpresa], [IdSolicitud]) REFERENCES [dbo].[ro_Solicitud_Vacaciones_x_empleado] ([IdEmpresa], [IdSolicitud])
);





