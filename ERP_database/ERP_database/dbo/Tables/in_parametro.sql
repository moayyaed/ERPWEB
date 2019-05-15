CREATE TABLE [dbo].[in_parametro] (
    [IdEmpresa]                                         INT          NOT NULL,
    [IdMovi_inven_tipo_egresoBodegaOrigen]              INT          NOT NULL,
    [IdMovi_inven_tipo_ingresoBodegaDestino]            INT          NOT NULL,
    [IdMovi_Inven_tipo_x_Dev_Inv_x_Ing]                 INT          NOT NULL,
    [IdMovi_Inven_tipo_x_Dev_Inv_x_Erg]                 INT          NOT NULL,
    [P_Al_Conta_CtaInven_Buscar_en]                     VARCHAR (15) NULL,
    [P_Al_Conta_CtaCosto_Buscar_en]                     VARCHAR (15) NULL,
    [P_IdCtaCble_transitoria_transf_inven]              VARCHAR (20) NULL,
    [P_IdProductoTipo_para_lote_0]                      INT          NOT NULL,
    [P_se_crea_lote_0_al_crear_producto_matriz]         BIT          NOT NULL,
    [IdMovi_inven_tipo_x_distribucion_ing]              INT          NOT NULL,
    [IdMovi_inven_tipo_x_distribucion_egr]              INT          NOT NULL,
    [P_IdMovi_inven_tipo_default_ing]                   INT          NOT NULL,
    [P_IdMovi_inven_tipo_default_egr]                   INT          NOT NULL,
    [P_IdMovi_inven_tipo_ingreso_x_compra]              INT          NOT NULL,
    [P_Dias_menores_alerta_desde_fecha_actual_rojo]     INT          NOT NULL,
    [P_Dias_menores_alerta_desde_fecha_actual_amarillo] INT          NOT NULL,
    [DiasTransaccionesAFuturo]                          INT          NOT NULL,
    [IdMovi_inven_tipo_Cambio]                          INT          NOT NULL,
    [IdMovi_inven_tipo_Consignacion]                    INT          NOT NULL,
    [IdMovi_inven_tipo_elaboracion_egr]                 INT          NOT NULL,
    [IdMovi_inven_tipo_elaboracion_ing]                 INT          NOT NULL,
    [IdMotivo_Inv_elaboracion_ing]                      INT          NULL,
    [IdMotivo_Inv_elaboracion_egr]                      INT          NULL,
    CONSTRAINT [PK_in_parametro] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC),
    CONSTRAINT [FK_in_parametro_ct_plancta2] FOREIGN KEY ([IdEmpresa], [P_IdCtaCble_transitoria_transf_inven]) REFERENCES [dbo].[ct_plancta] ([IdEmpresa], [IdCtaCble]),
    CONSTRAINT [FK_in_parametro_in_Catalogo] FOREIGN KEY ([P_Al_Conta_CtaInven_Buscar_en]) REFERENCES [dbo].[in_Catalogo] ([IdCatalogo]),
    CONSTRAINT [FK_in_parametro_in_Catalogo1] FOREIGN KEY ([P_Al_Conta_CtaCosto_Buscar_en]) REFERENCES [dbo].[in_Catalogo] ([IdCatalogo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo] FOREIGN KEY ([IdEmpresa], [IdMovi_inven_tipo_Cambio]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo1] FOREIGN KEY ([IdEmpresa], [IdMovi_inven_tipo_elaboracion_egr]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo10] FOREIGN KEY ([IdEmpresa], [P_IdMovi_inven_tipo_default_ing]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo11] FOREIGN KEY ([IdEmpresa], [P_IdMovi_inven_tipo_default_egr]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo12] FOREIGN KEY ([IdEmpresa], [P_IdMovi_inven_tipo_ingreso_x_compra]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo2] FOREIGN KEY ([IdEmpresa], [IdMovi_inven_tipo_egresoBodegaOrigen]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo3] FOREIGN KEY ([IdEmpresa], [IdMovi_inven_tipo_egresoBodegaOrigen]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo4] FOREIGN KEY ([IdEmpresa], [IdMovi_inven_tipo_elaboracion_ing]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo6] FOREIGN KEY ([IdEmpresa], [IdMovi_Inven_tipo_x_Dev_Inv_x_Ing]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo7] FOREIGN KEY ([IdEmpresa], [IdMovi_Inven_tipo_x_Dev_Inv_x_Erg]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo8] FOREIGN KEY ([IdEmpresa], [IdMovi_inven_tipo_x_distribucion_ing]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_movi_inven_tipo9] FOREIGN KEY ([IdEmpresa], [IdMovi_inven_tipo_x_distribucion_egr]) REFERENCES [dbo].[in_movi_inven_tipo] ([IdEmpresa], [IdMovi_inven_tipo]),
    CONSTRAINT [FK_in_parametro_in_ProductoTipo] FOREIGN KEY ([IdEmpresa], [P_IdProductoTipo_para_lote_0]) REFERENCES [dbo].[in_ProductoTipo] ([IdEmpresa], [IdProductoTipo])
);

















