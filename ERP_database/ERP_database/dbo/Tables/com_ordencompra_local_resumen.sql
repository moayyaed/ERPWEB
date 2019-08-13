CREATE TABLE [dbo].[com_ordencompra_local_resumen] (
    [IdEmpresa]              INT             NOT NULL,
    [IdSucursal]             INT             NOT NULL,
    [IdOrdenCompra]          NUMERIC (18, 2) NOT NULL,
    [SubtotalIVASinDscto]    NUMERIC (18, 2) NOT NULL,
    [SubtotalSinIVASinDscto] NUMERIC (18, 2) NOT NULL,
    [SubtotalSinDscto]       NUMERIC (18, 2) NOT NULL,
    [Descuento]              NUMERIC (18, 2) NOT NULL,
    [SubtotalIVAConDscto]    NUMERIC (18, 2) NOT NULL,
    [SubtotalSinIVAConDscto] NUMERIC (18, 2) NOT NULL,
    [SubtotalConDscto]       NUMERIC (18, 2) NOT NULL,
    [ValorIVA]               NUMERIC (18, 2) NOT NULL,
    [Total]                  NUMERIC (18, 2) NOT NULL,
    CONSTRAINT [PK_com_ordencompra_local_resumen] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSucursal] ASC, [IdOrdenCompra] ASC)
);

