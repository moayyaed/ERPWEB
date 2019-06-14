CREATE VIEW vwin_Ing_Egr_InvenPorOrdenCompra
AS
SELECT        in_Ing_Egr_Inven.IdEmpresa, in_Ing_Egr_Inven.IdSucursal, in_Ing_Egr_Inven.IdMovi_inven_tipo, in_Ing_Egr_Inven.IdNumMovi, in_movi_inven_tipo.tm_descripcion, tb_persona.pe_nombreCompleto, in_Ing_Egr_Inven.signo, 
                         in_Ing_Egr_Inven.cm_fecha, in_Ing_Egr_Inven.cm_observacion, in_Ing_Egr_Inven.Estado, in_Ing_Egr_Inven.IdBodega, tb_bodega.bo_Descripcion, tb_sucursal.Su_Descripcion, in_Ing_Egr_Inven.CodMoviInven
FROM            tb_sucursal INNER JOIN
                         tb_bodega ON tb_sucursal.IdEmpresa = tb_bodega.IdEmpresa AND tb_sucursal.IdSucursal = tb_bodega.IdSucursal RIGHT OUTER JOIN
                         tb_persona INNER JOIN
                         cp_proveedor ON tb_persona.IdPersona = cp_proveedor.IdPersona INNER JOIN
                         in_Ing_Egr_Inven INNER JOIN
                         com_parametro ON in_Ing_Egr_Inven.IdEmpresa = com_parametro.IdEmpresa AND in_Ing_Egr_Inven.IdMovi_inven_tipo = com_parametro.IdMovi_inven_tipo_OC ON cp_proveedor.IdEmpresa = in_Ing_Egr_Inven.IdEmpresa AND
                          cp_proveedor.IdProveedor = in_Ing_Egr_Inven.IdResponsable INNER JOIN
                         in_movi_inven_tipo ON in_Ing_Egr_Inven.IdEmpresa = in_movi_inven_tipo.IdEmpresa AND in_Ing_Egr_Inven.IdMovi_inven_tipo = in_movi_inven_tipo.IdMovi_inven_tipo ON 
                         tb_bodega.IdEmpresa = in_Ing_Egr_Inven.IdEmpresa AND tb_bodega.IdSucursal = in_Ing_Egr_Inven.IdSucursal AND tb_bodega.IdBodega = in_Ing_Egr_Inven.IdBodega