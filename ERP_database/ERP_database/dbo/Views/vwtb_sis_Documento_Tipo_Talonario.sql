CREATE view vwtb_sis_Documento_Tipo_Talonario as
select IdEmpresa, IdSucursal, CodDocumentoTipo,Establecimiento, PuntoEmision,NumDocumento, CAST(NumDocumento as int) as NumDocumentoInt, 
FechaCaducidad, Usado, Estado, NumAutorizacion, es_Documento_Electronico from tb_sis_Documento_Tipo_Talonario a