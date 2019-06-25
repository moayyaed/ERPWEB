CREATE VIEW vwcp_orden_giro_det_ing_x_oc
AS
SELECT cp_orden_giro_det_ing_x_oc.IdEmpresa, cp_orden_giro_det_ing_x_oc.IdCbteCble_Ogiro, cp_orden_giro_det_ing_x_oc.IdTipoCbte_Ogiro, cp_orden_giro_det_ing_x_oc.Secuencia, cp_orden_giro_det_ing_x_oc.inv_IdSucursal, 
                  cp_orden_giro_det_ing_x_oc.inv_IdMovi_inven_tipo, cp_orden_giro_det_ing_x_oc.inv_IdNumMovi, cp_orden_giro_det_ing_x_oc.inv_Secuencia, cp_orden_giro_det_ing_x_oc.oc_IdSucursal, 
                  cp_orden_giro_det_ing_x_oc.oc_IdOrdenCompra, cp_orden_giro_det_ing_x_oc.oc_Secuencia, cp_orden_giro_det_ing_x_oc.IdCtaCble, cp_orden_giro_det_ing_x_oc.dm_cantidad, cp_orden_giro_det_ing_x_oc.do_porc_des, 
                  cp_orden_giro_det_ing_x_oc.do_descuento, cp_orden_giro_det_ing_x_oc.do_precioFinal, cp_orden_giro_det_ing_x_oc.do_subtotal, cp_orden_giro_det_ing_x_oc.IdCod_Impuesto, cp_orden_giro_det_ing_x_oc.do_iva, 
                  cp_orden_giro_det_ing_x_oc.Por_Iva, cp_orden_giro_det_ing_x_oc.do_total, cp_orden_giro_det_ing_x_oc.IdUnidadMedida, cp_orden_giro_det_ing_x_oc.IdProducto, in_UnidadMedida.Descripcion AS NomUnidadMedida, 
                  in_Producto.pr_descripcion, ct_plancta.pc_Cuenta
FROM     in_UnidadMedida INNER JOIN
                  cp_orden_giro_det_ing_x_oc ON in_UnidadMedida.IdUnidadMedida = cp_orden_giro_det_ing_x_oc.IdUnidadMedida INNER JOIN
                  ct_plancta ON cp_orden_giro_det_ing_x_oc.IdEmpresa = ct_plancta.IdEmpresa AND cp_orden_giro_det_ing_x_oc.IdCtaCble = ct_plancta.IdCtaCble INNER JOIN
                  in_Producto ON cp_orden_giro_det_ing_x_oc.IdProducto = in_Producto.IdProducto AND cp_orden_giro_det_ing_x_oc.IdEmpresa = in_Producto.IdEmpresa