CREATE view web.VWROL_007 as
SELECT        per.pe_apellido, per.pe_nombre, per.pe_cedulaRuc, e.IdEmpresa, e.IdEmpleado, sv.IdSolicitud, sv.Fecha, sv.Fecha_Desde, sv.Fecha_Hasta, sv.Fecha_Retorno, sv.Observacion, d.de_descripcion, e.em_fechaIngaRol, 
                         carg.ca_descripcion, sv_det.IdPeriodo_Inicio, sv_det.IdPeriodo_Fin, sv_det.Dias_tomados, hv.DiasGanado, ca_tl.ca_descripcion AS Tipo_liquidacion, ca_tv.ca_descripcion AS Tipo_vacacion, hv.FechaIni, hv.FechaFin
FROM            dbo.ro_Solicitud_Vacaciones_x_empleado AS sv INNER JOIN
                         dbo.ro_empleado AS e ON sv.IdEmpresa = e.IdEmpresa AND sv.IdEmpleado = e.IdEmpleado INNER JOIN
                         dbo.ro_Departamento AS d ON e.IdEmpresa = d.IdEmpresa AND e.IdDepartamento = d.IdDepartamento INNER JOIN
                         dbo.tb_persona AS per ON e.IdPersona = per.IdPersona INNER JOIN
                         dbo.ro_cargo AS carg ON e.IdEmpresa = carg.IdEmpresa AND e.IdCargo = carg.IdCargo INNER JOIN
                         dbo.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado AS sv_det ON sv.IdEmpresa = sv_det.IdEmpresa AND sv.IdSolicitud = sv_det.IdSolicitud INNER JOIN
                         dbo.ro_historico_vacaciones_x_empleado AS hv ON sv_det.IdEmpresa = hv.IdEmpresa AND sv_det.IdEmpleado = hv.IdEmpleado AND sv_det.IdPeriodo_Inicio = hv.IdPeriodo_Inicio AND 
                         sv_det.IdPeriodo_Fin = hv.IdPeriodo_Fin INNER JOIN
                         dbo.ro_catalogo AS ca_tl ON sv_det.Tipo_liquidacion = ca_tl.CodCatalogo INNER JOIN
                         dbo.ro_catalogo AS ca_tv ON sv_det.Tipo_vacacion = ca_tv.CodCatalogo