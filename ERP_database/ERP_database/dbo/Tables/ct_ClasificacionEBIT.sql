CREATE TABLE [dbo].[ct_ClasificacionEBIT] (
    [IdClasificacionEBIT] INT           NOT NULL,
    [ebit_Codigo]         VARCHAR (100) NOT NULL,
    [ebit_Descripcion]    VARCHAR (MAX) NOT NULL,
    [AplicaEBIT]          BIT           NOT NULL,
    [AplicaEBITDA]        BIT           NOT NULL,
    CONSTRAINT [PK_ct_ClasificacionEBIT] PRIMARY KEY CLUSTERED ([IdClasificacionEBIT] ASC)
);

