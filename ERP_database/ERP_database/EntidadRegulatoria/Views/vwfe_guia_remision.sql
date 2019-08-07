

ALTER view [EntidadRegulatoria].[vwfe_guia_remision] as

SELECT        dbo.fa_guia_remision.IdEmpresa, dbo.fa_guia_remision.IdSucursal, dbo.fa_guia_remision.IdBodega, dbo.fa_guia_remision.IdGuiaRemision, dbo.fa_guia_remision.CodGuiaRemision, dbo.fa_guia_remision.CodDocumentoTipo, 
                         dbo.fa_guia_remision.Serie1, dbo.fa_guia_remision.Serie2, dbo.fa_guia_remision.NumGuia_Preimpresa, dbo.fa_guia_remision.NUAutorizacion, dbo.fa_guia_remision.Fecha_Autorizacion, dbo.fa_guia_remision.gi_fecha, 
                         dbo.fa_guia_remision.gi_plazo, dbo.fa_guia_remision.gi_fech_venc, dbo.fa_guia_remision.gi_Observacion, dbo.fa_guia_remision.gi_FechaFinTraslado, dbo.fa_guia_remision.gi_FechaInicioTraslado, 
                         dbo.fa_guia_remision.placa, '' AS ruta, dbo.fa_guia_remision.Direccion_Origen, dbo.fa_guia_remision.Direccion_Destino, dbo.tb_transportista.Cedula, dbo.tb_transportista.Nombre, dbo.fa_cliente_contactos.Nombres, 
                         dbo.fa_cliente_contactos.Telefono, dbo.fa_cliente_contactos.Celular, dbo.fa_cliente_contactos.Correo, dbo.fa_cliente_contactos.Direccion, dbo.tb_persona.pe_cedulaRuc, dbo.tb_persona.pe_nombreCompleto, 
                         dbo.tb_persona.IdTipoDocumento, dbo.tb_persona.pe_Naturaleza, dbo.tb_empresa.em_nombre, dbo.tb_empresa.RazonSocial, dbo.tb_empresa.NombreComercial, dbo.tb_empresa.em_ruc, dbo.tb_empresa.em_telefonos, 
                         dbo.tb_empresa.ContribuyenteEspecial, 'SI' AS ObligadoAllevarConta, dbo.tb_empresa.em_Email, dbo.tb_empresa.em_direccion, dbo.fa_factura.vt_serie1, dbo.fa_factura.vt_serie2, dbo.fa_factura.vt_NumFactura, 
                         dbo.fa_factura.vt_fecha, dbo.fa_MotivoTraslado.tr_Descripcion
FROM            dbo.tb_transportista INNER JOIN
                         dbo.fa_guia_remision ON dbo.tb_transportista.IdEmpresa = dbo.fa_guia_remision.IdEmpresa AND dbo.tb_transportista.IdTransportista = dbo.fa_guia_remision.IdTransportista INNER JOIN
                         dbo.fa_cliente_contactos ON dbo.fa_guia_remision.IdEmpresa = dbo.fa_cliente_contactos.IdEmpresa AND dbo.fa_guia_remision.IdCliente = dbo.fa_cliente_contactos.IdCliente AND 
                         dbo.fa_cliente_contactos.IdContacto = 1 INNER JOIN
                         dbo.tb_persona INNER JOIN
                         dbo.fa_cliente ON dbo.tb_persona.IdPersona = dbo.fa_cliente.IdPersona ON dbo.fa_cliente_contactos.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_cliente_contactos.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                         dbo.tb_empresa ON dbo.fa_cliente.IdEmpresa = dbo.tb_empresa.IdEmpresa AND dbo.fa_guia_remision.Estado = 1 AND dbo.fa_guia_remision.aprobada_enviar_sri = 1 AND NOT EXISTS
                             (SELECT        ID_REGISTRO, FECHA_CARGA, ESTADO
                               FROM            EntidadRegulatoria.fa_elec_registros_generados
                               WHERE        (ID_REGISTRO = SUBSTRING(dbo.tb_empresa.em_nombre, 0, 4) + '-' + 'GUI' + '-' + dbo.fa_guia_remision.Serie1 + '-' + dbo.fa_guia_remision.Serie2 + '-' + dbo.fa_guia_remision.NumGuia_Preimpresa)) INNER JOIN
                         dbo.fa_factura_x_fa_guia_remision ON dbo.fa_guia_remision.IdEmpresa = dbo.fa_factura_x_fa_guia_remision.gi_IdEmpresa AND dbo.fa_guia_remision.IdSucursal = dbo.fa_factura_x_fa_guia_remision.gi_IdSucursal AND 
                         dbo.fa_guia_remision.IdBodega = dbo.fa_factura_x_fa_guia_remision.gi_IdBodega AND dbo.fa_guia_remision.IdGuiaRemision = dbo.fa_factura_x_fa_guia_remision.gi_IdGuiaRemision INNER JOIN
                         dbo.fa_factura ON dbo.fa_factura_x_fa_guia_remision.fa_IdEmpresa = dbo.fa_factura.IdEmpresa AND dbo.fa_factura_x_fa_guia_remision.fa_IdSucursal = dbo.fa_factura.IdSucursal AND 
                         dbo.fa_factura_x_fa_guia_remision.fa_IdBodega = dbo.fa_factura.IdBodega AND dbo.fa_factura_x_fa_guia_remision.fa_IdCbteVta = dbo.fa_factura.IdCbteVta INNER JOIN
                         dbo.fa_MotivoTraslado ON dbo.fa_guia_remision.IdEmpresa = dbo.fa_MotivoTraslado.IdEmpresa AND dbo.fa_guia_remision.IdMotivoTraslado = dbo.fa_MotivoTraslado.IdMotivoTraslado
WHERE        (dbo.fa_guia_remision.NUAutorizacion IS NULL) AND (dbo.fa_guia_remision.Generado IS NULL)