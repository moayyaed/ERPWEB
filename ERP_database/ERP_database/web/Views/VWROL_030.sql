CREATE VIEW web.VWROL_030
AS
SELECT        dbo.ro_rol.IdEmpresa, dbo.ro_rol.IdRol, dbo.ro_rol.IdSucursal, dbo.ro_rol.IdNominaTipo, dbo.ro_rol.IdNominaTipoLiqui, dbo.ro_rol.IdPeriodo, dbo.ro_rol_detalle.IdEmpleado, dbo.ro_rol_detalle.IdRubro, 
                         dbo.ro_cargo.ca_descripcion, dbo.tb_persona.pe_cedulaRuc, dbo.tb_persona.pe_nombreCompleto, dbo.ro_rubro_tipo.rub_codigo, dbo.ro_rubro_tipo.ru_codRolGen, dbo.ro_rubro_tipo.ru_orden, dbo.ro_rubro_tipo.ru_descripcion, 
                         dbo.ro_rol_detalle.Orden, dbo.ro_rol_detalle.Valor
FROM            dbo.ro_empleado INNER JOIN
                         dbo.ro_rol_detalle ON dbo.ro_empleado.IdEmpresa = dbo.ro_rol_detalle.IdEmpresa AND dbo.ro_empleado.IdEmpleado = dbo.ro_rol_detalle.IdEmpleado INNER JOIN
                         dbo.tb_persona ON dbo.ro_empleado.IdPersona = dbo.tb_persona.IdPersona LEFT OUTER JOIN
                         dbo.ro_rubro_tipo ON dbo.ro_rol_detalle.IdEmpresa = dbo.ro_rubro_tipo.IdEmpresa AND dbo.ro_rol_detalle.IdRubro = dbo.ro_rubro_tipo.IdRubro RIGHT OUTER JOIN
                         dbo.ro_rol ON dbo.ro_rol_detalle.IdEmpresa = dbo.ro_rol.IdEmpresa AND dbo.ro_rol_detalle.IdRol = dbo.ro_rol.IdRol LEFT OUTER JOIN
                         dbo.ro_cargo ON dbo.ro_empleado.IdEmpresa = dbo.ro_cargo.IdEmpresa AND dbo.ro_empleado.IdCargo = dbo.ro_cargo.IdCargo
GO



GO



GO


