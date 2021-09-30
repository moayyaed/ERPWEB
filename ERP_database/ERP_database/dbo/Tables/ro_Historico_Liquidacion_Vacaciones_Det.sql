CREATE TABLE [dbo].[ro_Historico_Liquidacion_Vacaciones_Det] (
    [IdEmpresa]          INT        NOT NULL,
    [IdLiquidacion]      INT        NOT NULL,
    [Secuencia]          INT        NOT NULL,
    [Anio]               INT        NOT NULL,
    [Mes]                INT        NOT NULL,
    [Total_Remuneracion] FLOAT (53) NOT NULL,
    [Total_Vacaciones]   FLOAT (53) NOT NULL,
    [Valor_Cancelar]     FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ro_Historico_Liquidacion_Vacaciones_Det] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdLiquidacion] ASC, [Secuencia] ASC)
);



