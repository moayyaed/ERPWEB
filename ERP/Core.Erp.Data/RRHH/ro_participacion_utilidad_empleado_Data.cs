using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
namespace Core.Erp.Data.RRHH
{
  public  class ro_participacion_utilidad_empleado_Data
    {
        public List<ro_participacion_utilidad_empleado_Info> get_list(int IdEmpresa, DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                List<ro_participacion_utilidad_empleado_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = (from q in Context.spro_nomina_x_pago_utilidad(IdEmpresa, 1, FechaInicio, FechaFin)
                             where q.IdEmpresa == IdEmpresa
                             select new ro_participacion_utilidad_empleado_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdEmpleado=q.IdEmpleado,
                                 em_status=q.em_status,
                                 ca_descripcion=q.ca_descripcion,
                                 pe_nombre=q.pe_nombre,
                                 pe_apellido=q.pe_apellido+" "+q.pe_nombre,
                                 pe_cedulaRuc=q.pe_cedulaRuc,
                                 CargasFamiliares=q.num_cargas,
                                 num_contratos=q.num_contratos,
                                 em_fechaIngaRol=q.em_fechaIngaRol,
                                 em_fechaSalida=q.em_fechaSalida,
                                 IdSucursal=q.IdSucursal
                                  

                             }).ToList();

                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ro_participacion_utilidad_empleado_Info> get_list(int IdEmpresa, int IdUtilidad)
        {
            try
            {
                List<ro_participacion_utilidad_empleado_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = (from q in Context.vwro_participacion_utilidad_empleado
                             where q.IdEmpresa == IdEmpresa
                             && q.IdUtilidad== IdUtilidad
                             select new ro_participacion_utilidad_empleado_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdUtilidad = q.IdUtilidad,
                                 IdPeriodo=q.IdPeriodo,
                                 IdSucursal=q.IdSucursal,
                                 IdEmpleado = q.IdEmpleado,
                                 em_status = q.em_status,
                                 ca_descripcion = q.ca_descripcion,
                                 pe_apellido = q.pe_apellido + " " + q.pe_nombre,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 CargasFamiliares = q.CargasFamiliares,
                                 ValorCargaFamiliar=q.ValorCargaFamiliar,
                                 ValorIndividual=q.ValorIndividual,
                                 ValorTotal=q.ValorTotal,
                                 DiasTrabajados=q.DiasTrabajados,
                                 em_fechaIngaRol=q.em_fechaIngaRol,
                                 em_fechaSalida=q.em_fechaSalida,
                                 em_fecha_ingreso=q.em_fecha_ingreso,
                                 UtilidadDerechoIndividual=q.UtilidadDerechoIndividual,
                                 UtilidadCargaFamiliar=q.UtilidadCargaFamiliar,
                                 Descuento=q.Descuento,
                                 NetoRecibir=q.NetoRecibir

                             }).ToList();

                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ro_participacion_utilidad_empleado_Info> get_list(int IdEmpresa, int IdNomina, int IdNominaTipo, int IdPeriodo)
        {
            try
            {
                List<ro_participacion_utilidad_empleado_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = (from q in Context.vwro_participacion_utilidad_empleado
                             where q.IdEmpresa == IdEmpresa
                             && q.IdPeriodo==IdPeriodo
                             select new ro_participacion_utilidad_empleado_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdPeriodo=q.IdPeriodo,
                                 IdUtilidad = q.IdUtilidad,
                                 IdEmpleado = q.IdEmpleado,
                                 em_status = q.em_status,
                                 ca_descripcion = q.ca_descripcion,
                                 pe_nombre = q.pe_nombre,
                                 pe_apellido = q.pe_apellido,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 CargasFamiliares = q.CargasFamiliares,
                                 ValorCargaFamiliar = q.ValorCargaFamiliar,
                                 ValorIndividual = q.ValorIndividual,
                                 ValorTotal = q.ValorTotal,
                                 DiasTrabajados = q.DiasTrabajados,
                                 em_fechaIngaRol = q.em_fechaIngaRol,
                                 em_fechaSalida = q.em_fechaSalida,
                                 em_fecha_ingreso = q.em_fecha_ingreso

                             }).ToList();

                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

    
   }
}
