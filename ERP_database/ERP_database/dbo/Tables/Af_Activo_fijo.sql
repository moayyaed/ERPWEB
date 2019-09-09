CREATE TABLE [dbo].[Af_Activo_fijo] (
    [IdEmpresa]             INT           NOT NULL,
    [IdActivoFijo]          INT           NOT NULL,
    [CodActivoFijo]         VARCHAR (50)  NULL,
    [Af_Nombre]             VARCHAR (MAX) NOT NULL,
    [IdActivoFijoTipo]      INT           NOT NULL,
    [IdCategoriaAF]         INT           NOT NULL,
    [IdSucursal]            INT           NOT NULL,
    [IdDepartamento]        NUMERIC (18)  NOT NULL,
    [IdArea]                NUMERIC (18)  NOT NULL,
    [Af_NumSerie]           VARCHAR (100) NULL,
    [Af_fecha_compra]       DATE          NOT NULL,
    [Af_fecha_ini_depre]    DATE          NOT NULL,
    [Af_fecha_fin_depre]    DATE          NOT NULL,
    [Af_costo_compra]       FLOAT (53)    NOT NULL,
    [Af_Depreciacion_acum]  FLOAT (53)    NOT NULL,
    [Af_Vida_Util]          INT           NOT NULL,
    [Af_Meses_depreciar]    INT           NOT NULL,
    [Af_porcentaje_deprec]  FLOAT (53)    NOT NULL,
    [Af_observacion]        VARCHAR (MAX) NULL,
    [Estado]                CHAR (1)      NOT NULL,
    [IdEmpleadoEncargado]   NUMERIC (18)  NOT NULL,
    [IdEmpleadoCustodio]    NUMERIC (18)  NOT NULL,
    [Af_Codigo_Barra]       VARCHAR (50)  NULL,
    [Estado_Proceso]        VARCHAR (35)  NOT NULL,
    [Af_ValorSalvamento]    FLOAT (53)    NOT NULL,
    [Cantidad]              INT           NOT NULL,
    [IdModelo]              INT           NOT NULL,
    [IdMarca]               INT           NOT NULL,
    [IdUsuarioCreacion]     VARCHAR (50)  NULL,
    [FechaCreacion]         DATETIME      NULL,
    [IdUsuarioModificacion] VARCHAR (50)  NULL,
    [FechaModificacion]     DATETIME      NULL,
    [IdUsuarioAnulacion]    VARCHAR (50)  NULL,
    [FechaAnulacion]        DATETIME      NULL,
    [MotivoAnulacion]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Af_Activo_fijo_1] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdActivoFijo] ASC),
    CONSTRAINT [FK_Af_Activo_fijo_Af_Activo_fijo_Categoria] FOREIGN KEY ([IdEmpresa], [IdCategoriaAF]) REFERENCES [dbo].[Af_Activo_fijo_Categoria] ([IdEmpresa], [IdCategoriaAF]),
    CONSTRAINT [FK_Af_Activo_fijo_Af_Activo_fijo_tipo] FOREIGN KEY ([IdEmpresa], [IdActivoFijoTipo]) REFERENCES [dbo].[Af_Activo_fijo_tipo] ([IdEmpresa], [IdActivoFijoTipo]),
    CONSTRAINT [FK_Af_Activo_fijo_Af_Area] FOREIGN KEY ([IdEmpresa], [IdArea]) REFERENCES [dbo].[Af_Area] ([IdEmpresa], [IdArea]),
    CONSTRAINT [FK_Af_Activo_fijo_Af_Catalogo9] FOREIGN KEY ([Estado_Proceso]) REFERENCES [dbo].[Af_Catalogo] ([IdCatalogo]),
    CONSTRAINT [FK_Af_Activo_fijo_Af_Departamento] FOREIGN KEY ([IdEmpresa], [IdDepartamento]) REFERENCES [dbo].[Af_Departamento] ([IdEmpresa], [IdDepartamento]),
    CONSTRAINT [FK_Af_Activo_fijo_Af_Marca] FOREIGN KEY ([IdEmpresa], [IdMarca]) REFERENCES [dbo].[Af_Marca] ([IdEmpresa], [IdMarca]),
    CONSTRAINT [FK_Af_Activo_fijo_Af_Modelo] FOREIGN KEY ([IdEmpresa], [IdModelo]) REFERENCES [dbo].[Af_Modelo] ([IdEmpresa], [IdModelo]),
    CONSTRAINT [FK_Af_Activo_fijo_tb_empresa] FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tb_empresa] ([IdEmpresa]),
    CONSTRAINT [FK_Af_Activo_fijo_tb_persona] FOREIGN KEY ([IdEmpleadoEncargado]) REFERENCES [dbo].[tb_persona] ([IdPersona]),
    CONSTRAINT [FK_Af_Activo_fijo_tb_persona1] FOREIGN KEY ([IdEmpleadoCustodio]) REFERENCES [dbo].[tb_persona] ([IdPersona]),
    CONSTRAINT [FK_Af_Activo_fijo_tb_sucursal] FOREIGN KEY ([IdEmpresa], [IdSucursal]) REFERENCES [dbo].[tb_sucursal] ([IdEmpresa], [IdSucursal])
);















