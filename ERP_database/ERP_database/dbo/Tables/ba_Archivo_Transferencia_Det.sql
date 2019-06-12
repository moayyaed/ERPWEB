CREATE TABLE [dbo].[ba_Archivo_Transferencia_Det] (
    [IdEmpresa]                INT             NOT NULL,
    [IdArchivo]                NUMERIC (18)    NOT NULL,
    [Secuencia]                INT             NOT NULL,
    [Id_Item]                  VARCHAR (50)    NULL,
    [IdEmpresa_OP]             INT             NULL,
    [IdOrdenPago]              NUMERIC (18)    NULL,
    [Secuencia_OP]             INT             NULL,
    [Estado]                   BIT             NOT NULL,
    [Valor]                    NUMERIC (18, 2) NOT NULL,
    [Secuencial_reg_x_proceso] NUMERIC (18)    NOT NULL,
    [Contabilizado]            BIT             NULL,
    [Fecha_proceso]            DATETIME        NULL,
    CONSTRAINT [PK_ba_Archivo_Transferencia_Det] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdArchivo] ASC, [Secuencia] ASC),
    CONSTRAINT [FK_ba_Archivo_Transferencia_Det_ba_Archivo_Transferencia1] FOREIGN KEY ([IdEmpresa], [IdArchivo]) REFERENCES [dbo].[ba_Archivo_Transferencia] ([IdEmpresa], [IdArchivo]),
    CONSTRAINT [FK_ba_Archivo_Transferencia_Det_cp_orden_pago_det] FOREIGN KEY ([IdEmpresa_OP], [IdOrdenPago], [Secuencia_OP]) REFERENCES [dbo].[cp_orden_pago_det] ([IdEmpresa], [IdOrdenPago], [Secuencia])
);




GO
CREATE NONCLUSTERED INDEX [IX_ba_Archivo_Transferencia_Det_1]
    ON [dbo].[ba_Archivo_Transferencia_Det]([IdEmpresa_OP] ASC, [IdOrdenPago] ASC, [Secuencia_OP] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ba_Archivo_Transferencia_Det]
    ON [dbo].[ba_Archivo_Transferencia_Det]([IdEmpresa] ASC, [IdArchivo] ASC, [Secuencia] ASC);

