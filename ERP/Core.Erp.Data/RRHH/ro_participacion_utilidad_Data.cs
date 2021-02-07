using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
namespace Core.Erp.Data.RRHH
{
  public  class ro_participacion_utilidad_Data
    {
        public List<ro_participacion_utilidad_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<ro_participacion_utilidad_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.vwro_participacion_utilidad
                                 where q.IdEmpresa == IdEmpresa
                                 select new ro_participacion_utilidad_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdUtilidad=q.IdUtilidad,
                                     IdPeriodo = q.IdPeriodo,
                                     UtilidadDerechoIndividual=q.UtilidadDerechoIndividual,
                                     UtilidadCargaFamiliar=q.UtilidadCargaFamiliar,
                                     pe_FechaIni=q.pe_FechaIni,
                                     pe_FechaFin=q.pe_FechaFin,
                                     Estado=q.Estado,

                                     EstadoBool = q.Estado == "A" ? true : false

                                 }).ToList();
                    else
                        Lista = (from q in Context.vwro_participacion_utilidad
                                 where q.IdEmpresa == IdEmpresa
                                 select new ro_participacion_utilidad_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdPeriodo = q.IdPeriodo,
                                     pe_FechaIni = q.pe_FechaIni,
                                     pe_FechaFin = q.pe_FechaFin,

                                     EstadoBool = q.Estado == "A" ? true : false
                                 }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ro_participacion_utilidad_Info get_info(int IdEmpresa, int IdPeriodo)
        {
            try
            {
                ro_participacion_utilidad_Info info = new ro_participacion_utilidad_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_participacion_utilidad Entity = Context.ro_participacion_utilidad.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdUtilidad == IdPeriodo);
                    if (Entity == null) return null;

                    info = new ro_participacion_utilidad_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdUtilidad = Entity.IdUtilidad,
                        IdPeriodo=Entity.IdPeriodo,
                        UtilidadCargaFamiliar=Entity.UtilidadCargaFamiliar,
                        UtilidadDerechoIndividual=Entity.UtilidadDerechoIndividual,
                        Utilidad=Entity.Utilidad,
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int get_id(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    var lst = from q in Context.ro_participacion_utilidad
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdUtilidad) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(ro_participacion_utilidad_Info info)
        {
            try
            {
                int IdNomina_General;
                int IdNomina_TipoLiqui_PagoUtilidad;

                using (Entities_rrhh Context = new Entities_rrhh())
                {

                    ro_rubros_calculados RubrosCalculados = Context.ro_rubros_calculados.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa);
                    if (RubrosCalculados == null | RubrosCalculados.IdRubro_utilidad==null)
                        return false;
                    #region Nomina y periodo
                    DateTime fi =new DateTime(info.IdPeriodo, 1, 1);
                    DateTime ff =new DateTime(info.IdPeriodo, 12, 31);

                    ro_Parametros param = Context.ro_Parametros.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa);
                    if (param == null || param.IdNomina_General == null || param.IdNomina_TipoLiqui_PagoUtilidad == null)
                        return false;
                    IdNomina_General = Convert.ToInt32(param.IdNomina_General);
                    IdNomina_TipoLiqui_PagoUtilidad = Convert.ToInt32(param.IdNomina_TipoLiqui_PagoUtilidad);

                    ro_periodo Entity_periodo = Context.ro_periodo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdPeriodo == info.IdPeriodo);
                    if (Entity_periodo == null)
                    {
                        Context.ro_periodo.Add(new ro_periodo
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdPeriodo = info.IdPeriodo,
                            pe_FechaIni = new DateTime(info.IdPeriodo, 1, 1),
                            pe_FechaFin = new DateTime(info.IdPeriodo, 12, 31),
                            pe_anio = info.IdPeriodo,
                            pe_estado = "A",
                            IdUsuario = info.UsuarioIngresa,
                            Fecha_Transac = DateTime.Now
                        });
                        
                        ro_periodo_x_ro_Nomina_TipoLiqui Entity_periodo_nom = Context.ro_periodo_x_ro_Nomina_TipoLiqui.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdPeriodo == info.IdPeriodo
                        && q.IdNomina_Tipo == IdNomina_General && q.IdNomina_TipoLiqui == IdNomina_TipoLiqui_PagoUtilidad);

                        Context.ro_periodo_x_ro_Nomina_TipoLiqui.Add(new ro_periodo_x_ro_Nomina_TipoLiqui
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdPeriodo = info.IdPeriodo,
                            IdNomina_Tipo = IdNomina_General,
                            IdNomina_TipoLiqui = IdNomina_TipoLiqui_PagoUtilidad,
                            Cerrado = "N",
                            Procesado = "S",
                            Contabilizado = "N"
                        });
                    }

                    #endregion
                    #region Utilidad cabecera
                    
                    ro_participacion_utilidad Entity = new ro_participacion_utilidad
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdUtilidad = info.IdUtilidad = get_id(info.IdEmpresa),
                        Utilidad = info.Utilidad,
                        UtilidadCargaFamiliar = info.UtilidadCargaFamiliar,
                        UtilidadDerechoIndividual = info.UtilidadDerechoIndividual,
                        IdPeriodo = info.IdPeriodo,
                        FechaIngresa = DateTime.Now,
                        UsuarioIngresa = info.UsuarioIngresa,
                        Estado = "A"
                    };
                    Context.ro_participacion_utilidad.Add(Entity);
                    var det = Context.ro_participacion_utilidad_empleado.Where(s => s.IdEmpresa == info.IdEmpresa && info.IdUtilidad == info.IdUtilidad);
                    Context.ro_participacion_utilidad_empleado.RemoveRange(det);

                    #endregion

                    #region Rol
                    ro_rol Entity_rol = Context.ro_rol.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                    && q.IdNominaTipo == IdNomina_General
                     && q.IdNominaTipoLiqui == IdNomina_TipoLiqui_PagoUtilidad);

                    decimal rolId = 1;
                    if (Entity_rol == null)
                    {
                        var lst = from q in Context.ro_rol
                                  where q.IdEmpresa == info.IdEmpresa
                                  select q;
                        if(lst.Count()!=0)
                        rolId = lst.Max(q => q.IdRol) + 1;
                        
                        Context.ro_rol.Add(new ro_rol
                        {
                            IdRol = rolId,
                            IdEmpresa = info.IdEmpresa,
                            IdSucursal=info.IdSucursal,
                            IdPeriodo=info.IdPeriodo,
                            IdNominaTipo = IdNomina_General,
                            IdNominaTipoLiqui = IdNomina_TipoLiqui_PagoUtilidad,
                            Cerrado = "N",
                            Descripcion = "PAGO DE UTILIDA " + info.IdPeriodo.ToString(),
                            UsuarioIngresa = info.UsuarioIngresa,
                            FechaIngresa = DateTime.Now
                        });
                    }
                    else
                    {
                        Entity_rol.Observacion = "PAGO DE UTILIDA " + info.IdPeriodo.ToString();
                        Entity_rol.FechaModifica = DateTime.Now;
                        rolId = Entity_rol.IdRol;
                        var rol_detalle = Context.ro_rol_detalle.Where(s => s.IdEmpresa == info.IdEmpresa && s.IdRol == Entity_rol.IdRol);
                        Context.ro_rol_detalle.RemoveRange(rol_detalle);
                    }

                    #region INSERTANDO ROL DETALLE
                    foreach (var item in info.detalle)// LISTADO DE EMPLEADOS CON BENEFICIOS DE UTILIDAS
                    {
                        double TotalEgreso = 0;
                        // DIAS TRABAJADOS
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_dias_trabajados,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.DiasTrabajados
                        });
                        // TOTAL DE UTILIDAD
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_utilidad,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.ValorTotal
                        });
                        // TOTAL DE INGRESO
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_tot_ing,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.ValorTotal
                        });

                        #region Novedades y prestamos POR EMPLEADO
                        string sql = "";
                        sql = "  SELECT  n.IdEmpresa, n.IdEmpleado,IdSucursal, nd.IdRubro,sum( nd.Valor)Valor " +
                                "FROM dbo.ro_empleado_Novedad AS n INNER JOIN " +
                                 "dbo.ro_empleado_novedad_det AS nd ON n.IdEmpresa = nd.IdEmpresa AND n.IdNovedad = nd.IdNovedad " +
                                "where nd.EstadoCobro = 'PEN' and n.IdEmpresa='" + info.IdEmpresa + "' and IdNomina_Tipo=" + param.IdNomina_General + " and n.IdNomina_TipoLiqui=" + param.IdNomina_TipoLiqui_PagoUtilidad + "" +
                                "and  nd.FechaPago between '" + fi.ToString("yyyy-MM-dd") + "'  and '" + ff.ToString("yyyy-MM-dd") + "'" +
                                "GROUP BY n.IdEmpresa, n.IdEmpleado,IdSucursal, nd.IdRubro, nd.Valor " +
                                "union all " +
                                "SELECT p.IdEmpresa, p.IdEmpleado,IdSucursal, p.IdRubro, pd.TotalCuota " +
                                "FROM dbo.ro_prestamo AS p INNER JOIN " +
                                 " dbo.ro_prestamo_detalle AS pd ON p.IdEmpresa = pd.IdEmpresa AND p.IdPrestamo = pd.IdPrestamo INNER JOIN " +
                                 " dbo.ro_empleado AS e ON p.IdEmpresa = e.IdEmpresa AND p.IdEmpleado = e.IdEmpleado " +
                                 " where pd.EstadoPago = 'PEN' and p.IdEmpresa='" + info.IdEmpresa + "' and descuento_ben_soc='1' and pd.IdNominaTipoLiqui=" + param.IdNomina_TipoLiqui_PagoUtilidad + "" +
                                 " and  pd.FechaPago between '" + fi.ToString("yyyy-MM-dd") + "'  and '" + ff.ToString("yyyy-MM-dd") + "'" +
                                 " GROUP BY p.IdEmpresa, p.IdEmpleado,e.IdSucursal, pd.TotalCuota, p.IdRubro";
                        var lstNovedades = Context.Database.SqlQuery<ro_empleado_novedad_det_Info>(sql).ToList();
                        foreach (var item_ in lstNovedades)
                        {
                            Context.ro_rol_detalle.Add(new ro_rol_detalle
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdRol = rolId,
                                IdEmpleado = item_.IdEmpleado,
                                IdRubro = item_.IdRubro,
                                Observacion = " ",
                                IdSucursal = item_.IdSucursal,
                                rub_visible_reporte = true,
                                Orden = 0,
                                Valor = item_.Valor
                            });
                        }
                        #endregion

                        #region Total de egreso por empleado
                        var lst_total_egreso = lstNovedades.GroupBy(c => new
                        {
                            c.IdEmpleado,
                            c.IdSucursal
                        })
                           .Select(x => new
                           {
                               IdEmpleado = x.Key.IdEmpleado,
                               IdSucursal = x.Key.IdSucursal,
                               Valor = x.Sum(y => y.Valor)
                           }).ToList();
                        foreach (var item_egr in lst_total_egreso)
                        {
                            TotalEgreso = item_egr.Valor;
                            Context.ro_rol_detalle.Add(new ro_rol_detalle
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdRol = rolId,
                                IdEmpleado = item_egr.IdEmpleado,
                                IdRubro = RubrosCalculados.IdRubro_tot_egr,
                                Observacion = " ",
                                IdSucursal = item_egr.IdSucursal,
                                rub_visible_reporte = true,
                                Orden = 0,
                                Valor = item_egr.Valor
                            });
                        }
                        #endregion

                        #region liquido a recibir en rol
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_tot_egr,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.ValorTotal - TotalEgreso
                        });
                        #endregion

                        #region detalle de utilidad
                        ro_participacion_utilidad_empleado Entity_det = new ro_participacion_utilidad_empleado
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdUtilidad = info.IdUtilidad,
                            IdEmpleado = item.IdEmpleado,
                            CargasFamiliares = item.CargasFamiliares,
                            ValorCargaFamiliar = item.ValorCargaFamiliar,
                            ValorIndividual = item.ValorIndividual,
                            ValorTotal = item.ValorTotal,
                            DiasTrabajados = item.DiasTrabajados,
                            Descuento = TotalEgreso,
                            NetoRecibir = item.ValorTotal - TotalEgreso
                        };
                        Context.ro_participacion_utilidad_empleado.Add(Entity_det);
                        #endregion



                        #endregion



                    }


                    #endregion


                    Context.SaveChanges();
                }
                
                return true;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public bool modificarDB(ro_participacion_utilidad_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    int IdNomina_General;
                    int IdNomina_TipoLiqui_PagoUtilidad;

                    ro_rubros_calculados RubrosCalculados = Context.ro_rubros_calculados.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa);
                    if (RubrosCalculados == null | RubrosCalculados.IdRubro_utilidad == null)
                        return false;
                     #region Nomina y periodo
                    DateTime fi = new DateTime(info.IdPeriodo, 1, 1);
                    DateTime ff = new DateTime(info.IdPeriodo, 12, 31);

                    ro_Parametros param = Context.ro_Parametros.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa);
                    if (param == null || param.IdNomina_General == null || param.IdNomina_TipoLiqui_PagoUtilidad == null)
                        return false;
                    IdNomina_General = Convert.ToInt32(param.IdNomina_General);
                    IdNomina_TipoLiqui_PagoUtilidad = Convert.ToInt32(param.IdNomina_TipoLiqui_PagoUtilidad);

                    ro_periodo Entity_periodo = Context.ro_periodo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdPeriodo == info.IdPeriodo);
                    if (Entity_periodo == null)
                    {
                        Context.ro_periodo.Add(new ro_periodo
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdPeriodo = info.IdPeriodo,
                            pe_FechaIni = new DateTime(info.IdPeriodo, 1, 1),
                            pe_FechaFin = new DateTime(info.IdPeriodo, 12, 31),
                            pe_anio = info.IdPeriodo,
                            pe_estado = "A",
                            IdUsuario = info.UsuarioIngresa,
                            Fecha_Transac = DateTime.Now
                        });
                        ro_periodo_x_ro_Nomina_TipoLiqui Entity_periodo_nom = Context.ro_periodo_x_ro_Nomina_TipoLiqui.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdPeriodo == info.IdPeriodo
                        && q.IdNomina_Tipo == IdNomina_General && q.IdNomina_TipoLiqui == IdNomina_TipoLiqui_PagoUtilidad);

                        Context.ro_periodo_x_ro_Nomina_TipoLiqui.Add(new ro_periodo_x_ro_Nomina_TipoLiqui
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdPeriodo = info.IdPeriodo,
                            IdNomina_Tipo =IdNomina_General,
                            IdNomina_TipoLiqui = IdNomina_TipoLiqui_PagoUtilidad,
                            Cerrado = "N",
                            Procesado = "S",
                            Contabilizado = "N"
                        });
                    }

                    #endregion
                     #region Utilidad cabecera

                    ro_participacion_utilidad Entity = Context.ro_participacion_utilidad.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdUtilidad == info.IdUtilidad);
                    if (Entity == null)
                        return false;
                    Entity.IdUsuarioModifica = info.IdUsuarioModifica;
                    Entity.UtilidadDerechoIndividual = info.UtilidadDerechoIndividual;
                    Entity.UtilidadCargaFamiliar = info.UtilidadCargaFamiliar;
                    Entity.Utilidad = info.Utilidad;
                    Entity.Fecha_ultima_modif = DateTime.Now;



                    #endregion

                     #region Rol
                    ro_rol Entity_rol = Context.ro_rol.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa 
                    && q.IdNominaTipo ==IdNomina_General
                     && q.IdNominaTipoLiqui == IdNomina_TipoLiqui_PagoUtilidad);

                    decimal rolId = 0;
                    if(Entity_rol==null)
                    {
                        var lst = from q in Context.ro_rol
                                  where q.IdEmpresa == info.IdEmpresa
                                  select q;
                        rolId = lst.Max(q => q.IdRol) + 1;
                        Context.ro_rol.Add(new ro_rol
                        {
                            IdRol = rolId,
                            IdSucursal=info.IdSucursal,
                            IdEmpresa = info.IdEmpresa,
                            IdNominaTipo =IdNomina_General,
                            IdNominaTipoLiqui = IdNomina_TipoLiqui_PagoUtilidad,
                            Cerrado = "N",
                            Descripcion = "PAGO DE UTILIDA " + info.IdPeriodo.ToString(),
                            UsuarioIngresa = info.UsuarioIngresa,
                            FechaIngresa = DateTime.Now
                        });
                    }
                    else
                    {
                        Entity_rol.Observacion = "PAGO DE UTILIDA " + info.IdPeriodo.ToString();
                        Entity_rol.FechaModifica = DateTime.Now;
                        rolId = Entity_rol.IdRol;
                        var rol_detalle = Context.ro_rol_detalle.Where(s => s.IdEmpresa == info.IdEmpresa && s.IdRol== Entity_rol.IdRol);
                        Context.ro_rol_detalle.RemoveRange(rol_detalle);
                    }

                    #region INSERTANDO ROL DETALLE
                    var utilidad_detalle = Context.ro_participacion_utilidad_empleado.Where(s => s.IdEmpresa == info.IdEmpresa && s.IdUtilidad == info.IdUtilidad);
                    Context.ro_participacion_utilidad_empleado.RemoveRange(utilidad_detalle);


                    foreach (var item in info.detalle)// LISTADO DE EMPLEADOS CON BENEFICIOS DE UTILIDAS
                    {
                        double TotalEgreso = 0;
                        // DIAS TRABAJADOS
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_dias_trabajados,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.DiasTrabajados
                        });
                        // TOTAL DE UTILIDAD
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_utilidad,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.ValorTotal
                        });
                        // TOTAL DE INGRESO
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_tot_ing,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.ValorTotal
                        });

                        #region Novedades y prestamos POR EMPLEADO
                        string sql = "";
                        sql = "  SELECT  n.IdEmpresa, n.IdEmpleado,IdSucursal, nd.IdRubro,sum( nd.Valor)Valor " +
                                "FROM dbo.ro_empleado_Novedad AS n INNER JOIN " +
                                 "dbo.ro_empleado_novedad_det AS nd ON n.IdEmpresa = nd.IdEmpresa AND n.IdNovedad = nd.IdNovedad " +
                                "where nd.EstadoCobro = 'PEN' and n.IdEmpresa='" + info.IdEmpresa + "' and IdNomina_Tipo=" + param.IdNomina_General + " and n.IdNomina_TipoLiqui=" + param.IdNomina_TipoLiqui_PagoUtilidad + "" +
                                "and  nd.FechaPago between '" + fi.ToString("yyyy-MM-dd") + "'  and '" + ff.ToString("yyyy-MM-dd") + "'" +
                                " and n.IdEmpleado='"+item.IdEmpleado+"'"+
                                "GROUP BY n.IdEmpresa, n.IdEmpleado,IdSucursal, nd.IdRubro, nd.Valor " +
                                "union all " +
                                "SELECT p.IdEmpresa, p.IdEmpleado,IdSucursal, p.IdRubro, pd.TotalCuota " +
                                "FROM dbo.ro_prestamo AS p INNER JOIN " +
                                 " dbo.ro_prestamo_detalle AS pd ON p.IdEmpresa = pd.IdEmpresa AND p.IdPrestamo = pd.IdPrestamo INNER JOIN " +
                                 " dbo.ro_empleado AS e ON p.IdEmpresa = e.IdEmpresa AND p.IdEmpleado = e.IdEmpleado " +
                                 " where pd.EstadoPago = 'PEN' and p.IdEmpresa='" + info.IdEmpresa + "' and descuento_ben_soc='1' and pd.IdNominaTipoLiqui=" + param.IdNomina_TipoLiqui_PagoUtilidad + "" +
                                 " and  pd.FechaPago between '" + fi.ToString("yyyy-MM-dd") + "'  and '" + ff.ToString("yyyy-MM-dd") + "'" +
                                 " and p.IdEmpleado='" + item.IdEmpleado + "'"+
                                 " GROUP BY p.IdEmpresa, p.IdEmpleado,e.IdSucursal, pd.TotalCuota, p.IdRubro";
                        var lstNovedades = Context.Database.SqlQuery<ro_empleado_novedad_det_Info>(sql).ToList();
                        foreach (var item_ in lstNovedades)
                        {
                            Context.ro_rol_detalle.Add(new ro_rol_detalle
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdRol = rolId,
                                IdEmpleado = item_.IdEmpleado,
                                IdRubro = item_.IdRubro,
                                Observacion = " ",
                                IdSucursal = item_.IdSucursal,
                                rub_visible_reporte = true,
                                Orden = 0,
                                Valor = item_.Valor
                            });
                        }
                      #endregion

                        #region Total de egreso por empleado
                        var lst_total_egreso = lstNovedades.GroupBy(c => new
                        {
                            c.IdEmpleado,
                            c.IdSucursal
                        })
                           .Select(x => new
                           {
                               IdEmpleado = x.Key.IdEmpleado,
                               IdSucursal = x.Key.IdSucursal,
                               Valor = x.Sum(y => y.Valor)
                           }).ToList();
                        foreach (var item_egr in lst_total_egreso)
                        {
                            TotalEgreso = item_egr.Valor;
                            Context.ro_rol_detalle.Add(new ro_rol_detalle
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdRol = rolId,
                                IdEmpleado = item_egr.IdEmpleado,
                                IdRubro = RubrosCalculados.IdRubro_tot_egr,
                                Observacion = " ",
                                IdSucursal = item_egr.IdSucursal,
                                rub_visible_reporte = true,
                                Orden = 0,
                                Valor = item_egr.Valor
                            });
                        }
                        #endregion

                        #region liquido a recibir en rol
                        Context.ro_rol_detalle.Add(new ro_rol_detalle
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdRol = rolId,
                            IdEmpleado = item.IdEmpleado,
                            IdRubro = RubrosCalculados.IdRubro_tot_pagar,
                            Observacion = " ",
                            IdSucursal = item.IdSucursal,
                            rub_visible_reporte = true,
                            Orden = 0,
                            Valor = item.ValorTotal - TotalEgreso
                        });
                        #endregion

                        #region detalle de utilidad
                        ro_participacion_utilidad_empleado Entity_det = new ro_participacion_utilidad_empleado
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdUtilidad = info.IdUtilidad,
                            IdEmpleado = item.IdEmpleado,
                            CargasFamiliares = item.CargasFamiliares,
                            ValorCargaFamiliar = item.ValorCargaFamiliar,
                            ValorIndividual = item.ValorIndividual,
                            ValorTotal = item.ValorTotal,
                            DiasTrabajados = item.DiasTrabajados,
                            Descuento = TotalEgreso,
                            NetoRecibir = (item.ValorTotal - TotalEgreso)
                        };
                        Context.ro_participacion_utilidad_empleado.Add(Entity_det);
                        #endregion



                        #endregion



                    }


                    #endregion

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public bool anularDB(ro_participacion_utilidad_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_participacion_utilidad Entity = Context.ro_participacion_utilidad.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdUtilidad == info.IdUtilidad);
                    if (Entity == null)
                        return false;
                    Entity.Estado  = "I";

                    Entity.IdUsuario_anula = info.IdUsuario_anula;
                    Entity.Fecha_anulacion  = DateTime.Now;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
