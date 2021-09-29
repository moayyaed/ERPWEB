CREATE TABLE [dbo].[ro_empleado_x_jefes_inmediatos] (
    [IdEmpresa]          INT          NOT NULL,
    [IdEmpleado]         NUMERIC (18) NOT NULL,
    [Secuencia]          INT          NOT NULL,
    [IdEmpleado_aprueba] NUMERIC (18) NOT NULL,
    [Aprueba_vacaciones] BIT          NOT NULL,
    [Aprueba_prestamo]   BIT          NOT NULL,
    CONSTRAINT [PK_ro_empleado_x_jefes_inmediatos] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdEmpleado] ASC, [Secuencia] ASC)
);

