using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.RRHH
{
    public class ro_rdep_Data
    {
        public List<ro_rdep_Info> GetList(int IdEmpresa, int IdSucursal, int IdNomina_Tipo, int IdAnio)
        {
            int IdSucursalInicio = IdSucursal;
            int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

            //int IdEmpleadoInicio = IdAnio;
            //int IdEmpleadoFin = IdAnio == 0 ? 99999999 : IdAnio;

            try
            {
                List<ro_rdep_Info> Lista = new List<ro_rdep_Info>();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = Context.ro_rdep.Where(q => q.IdEmpresa == IdEmpresa 
                    && q.IdSucursal >= IdSucursalInicio
                    && q.IdSucursal <= IdSucursalFin
                    && q.IdNomina_Tipo == IdNomina_Tipo
                    && q.pe_anio == IdAnio).Select(q => new ro_rdep_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        pe_anio = q.pe_anio,
                        IdEmpleado = q.IdEmpleado,
                        IdSucursal = q.IdSucursal,
                        IdNomina_Tipo = q.IdNomina_Tipo,
                        Su_CodigoEstablecimiento = q.Su_CodigoEstablecimiento,
                        Empleado = q.pe_apellido+' '+ q.pe_nombre,
                        pe_cedulaRuc = q.pe_cedulaRuc,
                        pe_nombre = q.pe_nombre,
                        pe_apellido = q.pe_apellido,
                        Sueldo = q.Sueldo,
                        FondosReserva = q.FondosReserva,
                        DecimoTercerSueldo = q.DecimoTercerSueldo,
                        DecimoCuartoSueldo = q.DecimoCuartoSueldo,
                        Vacaciones = q.Vacaciones,
                        AportePErsonal = q.AportePErsonal,
                        GastoAlimentacion = q.GastoAlimentacion,
                        GastoEucacion = q.GastoEucacion,
                        GastoSalud = q.GastoSalud,
                        GastoVestimenta = q.GastoVestimenta,
                        GastoVivienda = q.GastoVivienda,
                        Utilidades = q.Utilidades,
                        IngresoVarios = q.IngresoVarios,
                        IngresoPorOtrosEmpleaodres = q.IngresoPorOtrosEmpleaodres,
                        IessPorOtrosEmpleadores = q.IessPorOtrosEmpleadores,
                        ValorImpuestoPorEsteEmplador = q.ValorImpuestoPorEsteEmplador,
                        ValorImpuestoPorOtroEmplador = q.ValorImpuestoPorOtroEmplador,
                        ExoneraionPorDiscapacidad = q.ExoneraionPorDiscapacidad,
                        ExoneracionPorTerceraEdad = q.ExoneracionPorTerceraEdad,
                        OtrosIngresosRelacionDependencia = q.OtrosIngresosRelacionDependencia,
                        ImpuestoRentaCausado = q.ImpuestoRentaCausado,
                        ValorImpuestoRetenidoTrabajador = q.ValorImpuestoRetenidoTrabajador
                    }).ToList();
                }
                return Lista;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ro_rdep_Info> GenerarRDEP(int IdEmpresa, int IdSucursal, int IdAnio, int IdNomina_Tipo)
        {
            int IdSucursalInicio = IdSucursal;
            int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

            try
            {
                List<ro_rdep_Info> Lista = new List<ro_rdep_Info>();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Context.GenerarRDEP(IdEmpresa, IdAnio, IdNomina_Tipo, IdSucursalInicio, IdSucursalFin);
                    Lista = (from q in Context.ro_rdep
                           where q.IdEmpresa == IdEmpresa
                            && q.IdSucursal >= IdSucursalInicio
                            && q.IdSucursal <= IdSucursalFin
                            && q.IdNomina_Tipo == IdNomina_Tipo
                            && q.pe_anio == IdAnio
                             select new ro_rdep_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        pe_anio = q.pe_anio,
                        IdEmpleado = q.IdEmpleado,
                        IdSucursal = q.IdSucursal,
                        IdNomina_Tipo = q.IdNomina_Tipo,
                        Su_CodigoEstablecimiento = q.Su_CodigoEstablecimiento,
                        Empleado = q.pe_apellido + ' ' + q.pe_nombre,
                        pe_cedulaRuc = q.pe_cedulaRuc,
                        pe_nombre = q.pe_nombre,
                        pe_apellido = q.pe_apellido,
                        Sueldo = q.Sueldo,
                        FondosReserva = q.FondosReserva,
                        DecimoTercerSueldo = q.DecimoTercerSueldo,
                        DecimoCuartoSueldo = q.DecimoCuartoSueldo,
                        Vacaciones = q.Vacaciones,
                        AportePErsonal = q.AportePErsonal,
                        GastoAlimentacion = q.GastoAlimentacion,
                        GastoEucacion = q.GastoEucacion,
                        GastoSalud = q.GastoSalud,
                        GastoVestimenta = q.GastoVestimenta,
                        GastoVivienda = q.GastoVivienda,
                        Utilidades = q.Utilidades,
                        IngresoVarios = q.IngresoVarios,
                        IngresoPorOtrosEmpleaodres = q.IngresoPorOtrosEmpleaodres,
                        IessPorOtrosEmpleadores = q.IessPorOtrosEmpleadores,
                        ValorImpuestoPorEsteEmplador = q.ValorImpuestoPorEsteEmplador,
                        ValorImpuestoPorOtroEmplador = q.ValorImpuestoPorOtroEmplador,
                        ExoneraionPorDiscapacidad = q.ExoneraionPorDiscapacidad,
                        ExoneracionPorTerceraEdad = q.ExoneracionPorTerceraEdad,
                        OtrosIngresosRelacionDependencia = q.OtrosIngresosRelacionDependencia,
                        ImpuestoRentaCausado = q.ImpuestoRentaCausado,
                        ValorImpuestoRetenidoTrabajador = q.ValorImpuestoRetenidoTrabajador
                    }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ro_rdep_Info GetInfo(int IdEmpresa, int IdSucursal, int IdEmpleado)
        {
            try
            {
                ro_rdep_Info info = new ro_rdep_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_rdep Entity = Context.ro_rdep.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdEmpleado == IdEmpleado).FirstOrDefault();

                    if (Entity == null) return null;
                    info = new ro_rdep_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        pe_anio = Entity.pe_anio,
                        IdEmpleado = Entity.IdEmpleado,
                        IdSucursal = Entity.IdSucursal,
                        IdNomina_Tipo = Entity.IdNomina_Tipo,
                        Su_CodigoEstablecimiento = Entity.Su_CodigoEstablecimiento,
                        pe_cedulaRuc = Entity.pe_cedulaRuc,
                        pe_nombre = Entity.pe_nombre,
                        pe_apellido = Entity.pe_apellido,
                        Sueldo = Entity.Sueldo,
                        FondosReserva = Entity.FondosReserva,
                        DecimoTercerSueldo = Entity.DecimoTercerSueldo,
                        DecimoCuartoSueldo = Entity.DecimoCuartoSueldo,
                        Vacaciones = Entity.Vacaciones,
                        AportePErsonal = Entity.AportePErsonal,
                        GastoAlimentacion = Entity.GastoAlimentacion,
                        GastoEucacion = Entity.GastoEucacion,
                        GastoSalud = Entity.GastoSalud,
                        GastoVestimenta = Entity.GastoVestimenta,
                        GastoVivienda = Entity.GastoVivienda,
                        Utilidades = Entity.Utilidades,
                        IngresoVarios = Entity.IngresoVarios,
                        IngresoPorOtrosEmpleaodres = Entity.IngresoPorOtrosEmpleaodres,
                        IessPorOtrosEmpleadores = Entity.IessPorOtrosEmpleadores,
                        ValorImpuestoPorEsteEmplador = Entity.ValorImpuestoPorEsteEmplador,
                        ValorImpuestoPorOtroEmplador = Entity.ValorImpuestoPorOtroEmplador,
                        ExoneraionPorDiscapacidad = Entity.ExoneraionPorDiscapacidad,
                        ExoneracionPorTerceraEdad = Entity.ExoneracionPorTerceraEdad,
                        OtrosIngresosRelacionDependencia = Entity.OtrosIngresosRelacionDependencia,
                        ImpuestoRentaCausado = Entity.ImpuestoRentaCausado,
                        ValorImpuestoRetenidoTrabajador = Entity.ValorImpuestoRetenidoTrabajador
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarBD(ro_rdep_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Context.ro_rdep.Add(new ro_rdep
                    {
                        IdEmpresa = info.IdEmpresa,
                        pe_anio = info.pe_anio,
                        IdEmpleado = info.IdEmpleado,
                        IdSucursal = info.IdSucursal,
                        IdNomina_Tipo = info.IdNomina_Tipo,
                        Su_CodigoEstablecimiento = info.Su_CodigoEstablecimiento,
                        pe_cedulaRuc = info.pe_cedulaRuc,
                        pe_nombre = info.pe_nombre,
                        pe_apellido = info.pe_apellido,
                        Sueldo = info.Sueldo,
                        FondosReserva = info.FondosReserva,
                        DecimoTercerSueldo = info.DecimoTercerSueldo,
                        DecimoCuartoSueldo = info.DecimoCuartoSueldo,
                        Vacaciones = info.Vacaciones,
                        AportePErsonal = info.AportePErsonal,
                        GastoAlimentacion = info.GastoAlimentacion,
                        GastoEucacion = info.GastoEucacion,
                        GastoSalud = info.GastoSalud,
                        GastoVestimenta = info.GastoVestimenta,
                        GastoVivienda = info.GastoVivienda,
                        Utilidades = info.Utilidades,
                        IngresoVarios = info.IngresoVarios,
                        IngresoPorOtrosEmpleaodres = info.IngresoPorOtrosEmpleaodres,
                        IessPorOtrosEmpleadores = info.IessPorOtrosEmpleadores,
                        ValorImpuestoPorEsteEmplador = info.ValorImpuestoPorEsteEmplador,
                        ValorImpuestoPorOtroEmplador = info.ValorImpuestoPorOtroEmplador,
                        ExoneraionPorDiscapacidad = info.ExoneraionPorDiscapacidad,
                        ExoneracionPorTerceraEdad = info.ExoneracionPorTerceraEdad,
                        OtrosIngresosRelacionDependencia = info.OtrosIngresosRelacionDependencia,
                        ImpuestoRentaCausado = info.ImpuestoRentaCausado,
                        ValorImpuestoRetenidoTrabajador = info.ValorImpuestoRetenidoTrabajador
                    });

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModificarBD(ro_rdep_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_rdep entity = Context.ro_rdep.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdEmpleado == info.IdEmpleado).FirstOrDefault();

                    if (entity == null)
                    {
                        return false;
                    }

                    entity.IdEmpresa = info.IdEmpresa;
                    entity.pe_anio = info.pe_anio;
                    entity.IdEmpleado = info.IdEmpleado;
                    entity.IdSucursal = info.IdSucursal;
                    entity.IdNomina_Tipo = info.IdNomina_Tipo;
                    entity.Su_CodigoEstablecimiento = info.Su_CodigoEstablecimiento;
                    entity.pe_cedulaRuc = info.pe_cedulaRuc;
                    entity.pe_nombre = info.pe_nombre;
                    entity.pe_apellido = info.pe_apellido;
                    entity.Sueldo = info.Sueldo;
                    entity.FondosReserva = info.FondosReserva;
                    entity.DecimoTercerSueldo = info.DecimoTercerSueldo;
                    entity.DecimoCuartoSueldo = info.DecimoCuartoSueldo;
                    entity.Vacaciones = info.Vacaciones;
                    entity.AportePErsonal = info.AportePErsonal;
                    entity.GastoAlimentacion = info.GastoAlimentacion;
                    entity.GastoEucacion = info.GastoEucacion;
                    entity.GastoSalud = info.GastoSalud;
                    entity.GastoVestimenta = info.GastoVestimenta;
                    entity.GastoVivienda = info.GastoVivienda;
                    entity.Utilidades = info.Utilidades;
                    entity.IngresoVarios = info.IngresoVarios;
                    entity.IngresoPorOtrosEmpleaodres = info.IngresoPorOtrosEmpleaodres;
                    entity.IessPorOtrosEmpleadores = info.IessPorOtrosEmpleadores;
                    entity.ValorImpuestoPorEsteEmplador = info.ValorImpuestoPorEsteEmplador;
                    entity.ValorImpuestoPorOtroEmplador = info.ValorImpuestoPorOtroEmplador;
                    entity.ExoneraionPorDiscapacidad = info.ExoneraionPorDiscapacidad;
                    entity.ExoneracionPorTerceraEdad = info.ExoneracionPorTerceraEdad;
                    entity.OtrosIngresosRelacionDependencia = info.OtrosIngresosRelacionDependencia;
                    entity.ImpuestoRentaCausado = info.ImpuestoRentaCausado;
                    entity.ValorImpuestoRetenidoTrabajador = info.ValorImpuestoRetenidoTrabajador;

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
