using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            try
            {
                List<ro_rdep_Info> Lista = new List<ro_rdep_Info>();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = Context.vwro_rdep.Where(q => q.IdEmpresa == IdEmpresa 
                    && q.IdSucursal >= IdSucursalInicio
                    && q.IdSucursal <= IdSucursalFin
                    && q.IdNomina_Tipo == IdNomina_Tipo
                    && q.pe_anio == IdAnio).Select(q => new ro_rdep_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        IdNomina_Tipo = q.IdNomina_Tipo,
                        Id_Rdep =  q.Id_Rdep,
                        pe_anio = q.pe_anio,
                        Estado = q.Estado,
                        Su_Descripcion = q.Su_Descripcion,
                        Descripcion = q.Descripcion,
                        Observacion = q.Observacion                        
                    }).ToList();
                }
                
                return Lista;
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
                using (Entities_rrhh db = new Entities_rrhh())
                {
                    var Lista = db.ro_rdep.Where(q => q.IdEmpresa == IdEmpresa).Select(q => q.Id_Rdep);

                    if (Lista.Count() > 0)
                        ID = Lista.Max() + 1;
                }
                return ID;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool GenerarRDEP(int IdEmpresa, int IdSucursal, int Id_Rdep, int IdAnio, int IdNomina_Tipo, decimal IdEmpleado, string Observacion, string IdUsuario)
        {
            int IdSucursalInicio = IdSucursal;
            int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

            decimal IdEmpleadoInicio = IdEmpleado;
            decimal IdEmpleadoFin = IdEmpleado == 0 ? 999999999 : IdEmpleado;

            if (Id_Rdep == 0)
            {
                Id_Rdep = get_id(IdEmpresa);
            }

            try
            {
                List<ro_rdep_Info> Lista = new List<ro_rdep_Info>();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Context.GenerarRDEP(IdEmpresa, Id_Rdep, IdAnio, IdNomina_Tipo, IdSucursalInicio, IdSucursalFin, IdEmpleadoInicio, IdEmpleadoFin, Observacion, IdUsuario);
                }
                
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ro_rdep_Info GetInfo(int IdEmpresa, int Id_Rdep)
        {
            try
            {
                ro_rdep_Info info = new ro_rdep_Info();
                ro_rdep_det_Info info_det = new ro_rdep_det_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_rdep Entity = Context.ro_rdep.Where(q => q.IdEmpresa == IdEmpresa && q.Id_Rdep == Id_Rdep).FirstOrDefault();
                    List<ro_rdep_det> Entity_Det = Context.ro_rdep_det.Where(q => q.IdEmpresa == IdEmpresa && q.Id_Rdep == Id_Rdep).OrderBy(q => q.pe_apellido).ThenBy(q => q.pe_nombre).ToList();

                    if (Entity == null) return null;
                    info = new ro_rdep_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        Id_Rdep = Entity.Id_Rdep,
                        pe_anio = Entity.pe_anio,                        
                        IdSucursal = Entity.IdSucursal,
                        IdNomina_Tipo = Entity.IdNomina_Tipo,
                        Estado = Entity.Estado,
                        Su_CodigoEstablecimiento = Entity.Su_CodigoEstablecimiento,
                        Observacion = Entity.Observacion
                    };

                    info.Lista_Rdep_Det = new List<ro_rdep_det_Info>();

                    foreach (var item in Entity_Det)
                    {
                        info_det = new ro_rdep_det_Info
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdEmpleado = item.IdEmpleado,
                            Id_Rdep = item.Id_Rdep,
                            Secuencia = item.Secuencia,
                            pe_cedulaRuc = item.pe_cedulaRuc,
                            Empleado = item.pe_apellido + " "+ item.pe_nombre,
                            pe_nombre = item.pe_nombre,
                            pe_apellido = item.pe_apellido,
                            Sueldo = item.Sueldo,
                            FondosReserva = item.FondosReserva,
                            DecimoTercerSueldo = item.DecimoTercerSueldo,
                            DecimoCuartoSueldo = item.DecimoCuartoSueldo,
                            Vacaciones = item.Vacaciones,
                            AportePErsonal = item.AportePErsonal,
                            GastoAlimentacion = item.GastoAlimentacion,
                            GastoEucacion = item.GastoEucacion,
                            GastoSalud = item.GastoSalud,
                            GastoVestimenta = item.GastoVestimenta,
                            GastoVivienda = item.GastoVivienda,
                            Utilidades = item.Utilidades,
                            IngresoVarios = item.IngresoVarios,
                            IngresoPorOtrosEmpleaodres = item.IngresoPorOtrosEmpleaodres,
                            IessPorOtrosEmpleadores = item.IessPorOtrosEmpleadores,
                            ValorImpuestoPorEsteEmplador = item.ValorImpuestoPorEsteEmplador,
                            ValorImpuestoPorOtroEmplador = item.ValorImpuestoPorOtroEmplador,
                            ExoneraionPorDiscapacidad = item.ExoneraionPorDiscapacidad,
                            ExoneracionPorTerceraEdad = item.ExoneracionPorTerceraEdad,
                            OtrosIngresosRelacionDependencia = item.OtrosIngresosRelacionDependencia,
                            ImpuestoRentaCausado = item.ImpuestoRentaCausado,
                            ValorImpuestoRetenidoTrabajador = item.ValorImpuestoRetenidoTrabajador,
                            ImpuestoRentaAsumidoPorEsteEmpleador = item.ImpuestoRentaAsumidoPorEsteEmpleador,
                            BaseImponibleGravada = item.BaseImponibleGravada,
                            IngresosGravadorPorEsteEmpleador = item.IngresosGravadorPorEsteEmpleador
                        };

                        info.Lista_Rdep_Det.Add(info_det);
                    }                    
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ro_rdep_det_Info GetInfo_x_Empleado(int IdEmpresa, int Id_Rdep, int Secuencia)
        {
            try
            {
                ro_rdep_det_Info info_det = new ro_rdep_det_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_rdep_det Entity_Det = Context.ro_rdep_det.Where(q => q.IdEmpresa == IdEmpresa && q.Id_Rdep == Id_Rdep && q.Secuencia == Secuencia).FirstOrDefault();

                    info_det.IdEmpresa = Entity_Det.IdEmpresa;
                    info_det.Id_Rdep = Entity_Det.Id_Rdep;
                    info_det.IdEmpleado = Entity_Det.IdEmpleado;
                    info_det.Secuencia = Entity_Det.Secuencia;
                    info_det.pe_cedulaRuc = Entity_Det.pe_cedulaRuc;
                    info_det.Empleado = Entity_Det.pe_apellido + " " + Entity_Det.pe_nombre;
                    info_det.pe_nombre = Entity_Det.pe_nombre;
                    info_det.pe_apellido = Entity_Det.pe_apellido;
                    info_det.Sueldo = Entity_Det.Sueldo;
                    info_det.FondosReserva = Entity_Det.FondosReserva;
                    info_det.DecimoTercerSueldo = Entity_Det.DecimoTercerSueldo;
                    info_det.DecimoCuartoSueldo = Entity_Det.DecimoCuartoSueldo;
                    info_det.Vacaciones = Entity_Det.Vacaciones;
                    info_det.AportePErsonal = Entity_Det.AportePErsonal;
                    info_det.GastoAlimentacion = Entity_Det.GastoAlimentacion;
                    info_det.GastoEucacion = Entity_Det.GastoEucacion;
                    info_det.GastoSalud = Entity_Det.GastoSalud;
                    info_det.GastoVestimenta = Entity_Det.GastoVestimenta;
                    info_det.GastoVivienda = Entity_Det.GastoVivienda;
                    info_det.Utilidades = Entity_Det.Utilidades;
                    info_det.IngresoVarios = Entity_Det.IngresoVarios;
                    info_det.IngresoPorOtrosEmpleaodres = Entity_Det.IngresoPorOtrosEmpleaodres;
                    info_det.IessPorOtrosEmpleadores = Entity_Det.IessPorOtrosEmpleadores;
                    info_det.ValorImpuestoPorEsteEmplador = Entity_Det.ValorImpuestoPorEsteEmplador;
                    info_det.ValorImpuestoPorOtroEmplador = Entity_Det.ValorImpuestoPorOtroEmplador;
                    info_det.ExoneraionPorDiscapacidad = Entity_Det.ExoneraionPorDiscapacidad;
                    info_det.ExoneracionPorTerceraEdad = Entity_Det.ExoneracionPorTerceraEdad;
                    info_det.OtrosIngresosRelacionDependencia = Entity_Det.OtrosIngresosRelacionDependencia;
                    info_det.ImpuestoRentaCausado = Entity_Det.ImpuestoRentaCausado;
                    info_det.ValorImpuestoRetenidoTrabajador = Entity_Det.ValorImpuestoRetenidoTrabajador;
                    info_det.ImpuestoRentaAsumidoPorEsteEmpleador = Entity_Det.ImpuestoRentaAsumidoPorEsteEmpleador;
                    info_det.BaseImponibleGravada = Entity_Det.BaseImponibleGravada;
                    info_det.IngresosGravadorPorEsteEmpleador = Entity_Det.IngresosGravadorPorEsteEmpleador;
                }

                return info_det;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarBD(ro_rdep_det_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_rdep entity_rdep = Context.ro_rdep.Where(q => q.IdEmpresa == info.IdEmpresa && q.Id_Rdep == info.Id_Rdep).FirstOrDefault();
                    ro_rdep_det entity = Context.ro_rdep_det.Where(q => q.IdEmpresa == info.IdEmpresa && q.Id_Rdep == info.Id_Rdep && q.Secuencia == info.Secuencia).FirstOrDefault();

                    if (entity_rdep == null)
                    {
                        return false;
                    }

                    entity_rdep.IdUsuarioUltMod = info.IdUsuario;
                    entity_rdep.Fecha_UltMod = DateTime.Now;

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
                    entity.ImpuestoRentaAsumidoPorEsteEmpleador = info.ImpuestoRentaAsumidoPorEsteEmpleador;
                    entity.BaseImponibleGravada = info.BaseImponibleGravada;
                    entity.IngresosGravadorPorEsteEmpleador = info.IngresosGravadorPorEsteEmpleador;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularBD(ro_rdep_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_rdep entity = Context.ro_rdep.Where(q => q.IdEmpresa == info.IdEmpresa && q.Id_Rdep == info.Id_Rdep).FirstOrDefault();
                    if (entity == null)
                    {
                        return false;
                    }

                    entity.Estado = false;
                    entity.MotiAnula = info.MotiAnula;
                    entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    entity.Fecha_UltAnu = DateTime.Now;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /**************** XML ************/
        public List<ro_rdep_det_Info> get_info_xml(int IdEmpresa, int Id_Rdep)
        {            
            try
            {
                List<ro_rdep_det_Info> Lista = new List<ro_rdep_det_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    command.CommandText = "SELECT EntidadRegulatoria.ro_rdep.pe_anio FROM EntidadRegulatoria.ro_rdep WHERE IdEmpresa = " + IdEmpresa.ToString() + " and Id_Rdep = " + Id_Rdep.ToString();
                    SqlDataReader reader = command.ExecuteReader();
                    int Anio = 0;
                    while (reader.Read())
                    {
                        Anio = Convert.ToInt32(reader["pe_anio"]);
                    }
                    reader.Close();

                    command.CommandText = "SELECT        EntidadRegulatoria.ro_rdep.IdEmpresa, EntidadRegulatoria.ro_rdep.Id_Rdep, EntidadRegulatoria.ro_rdep.IdSucursal, EntidadRegulatoria.ro_rdep.pe_anio, EntidadRegulatoria.ro_rdep.IdNomina_Tipo, "
                                        + " EntidadRegulatoria.ro_rdep.Su_CodigoEstablecimiento, EntidadRegulatoria.ro_rdep.Observacion, EntidadRegulatoria.ro_rdep.Estado, EntidadRegulatoria.ro_rdep_det.Secuencia, EntidadRegulatoria.ro_rdep_det.IdEmpleado, "
                                        + " EntidadRegulatoria.ro_rdep_det.pe_cedulaRuc, EntidadRegulatoria.ro_rdep_det.pe_apellido +' '+ EntidadRegulatoria.ro_rdep_det.pe_nombre as Empleado, EntidadRegulatoria.ro_rdep_det.pe_apellido, EntidadRegulatoria.ro_rdep_det.pe_nombre, EntidadRegulatoria.ro_rdep_det.Sueldo, "
                                        + " EntidadRegulatoria.ro_rdep_det.FondosReserva, EntidadRegulatoria.ro_rdep_det.DecimoTercerSueldo, EntidadRegulatoria.ro_rdep_det.DecimoCuartoSueldo, EntidadRegulatoria.ro_rdep_det.Vacaciones, "
                                        + " EntidadRegulatoria.ro_rdep_det.AportePErsonal, EntidadRegulatoria.ro_rdep_det.GastoAlimentacion, EntidadRegulatoria.ro_rdep_det.GastoEucacion, EntidadRegulatoria.ro_rdep_det.GastoSalud, "
                                        + " EntidadRegulatoria.ro_rdep_det.GastoVestimenta, EntidadRegulatoria.ro_rdep_det.GastoVivienda, EntidadRegulatoria.ro_rdep_det.Utilidades, EntidadRegulatoria.ro_rdep_det.IngresoVarios, "
                                        + " EntidadRegulatoria.ro_rdep_det.IngresoPorOtrosEmpleaodres, EntidadRegulatoria.ro_rdep_det.IessPorOtrosEmpleadores, EntidadRegulatoria.ro_rdep_det.ValorImpuestoPorEsteEmplador, "
                                        + " EntidadRegulatoria.ro_rdep_det.ValorImpuestoPorOtroEmplador, EntidadRegulatoria.ro_rdep_det.ExoneraionPorDiscapacidad, EntidadRegulatoria.ro_rdep_det.ExoneracionPorTerceraEdad, "
                                        + " EntidadRegulatoria.ro_rdep_det.OtrosIngresosRelacionDependencia, EntidadRegulatoria.ro_rdep_det.ImpuestoRentaCausado, EntidadRegulatoria.ro_rdep_det.ValorImpuestoRetenidoTrabajador, "
                                        + " EntidadRegulatoria.ro_rdep_det.ImpuestoRentaAsumidoPorEsteEmpleador, EntidadRegulatoria.ro_rdep_det.BaseImponibleGravada, EntidadRegulatoria.ro_rdep_det.IngresosGravadorPorEsteEmpleador"
                                        + " FROM            EntidadRegulatoria.ro_rdep INNER JOIN"
                                        + " EntidadRegulatoria.ro_rdep_det ON EntidadRegulatoria.ro_rdep.IdEmpresa = EntidadRegulatoria.ro_rdep_det.IdEmpresa AND EntidadRegulatoria.ro_rdep.Id_Rdep = EntidadRegulatoria.ro_rdep_det.Id_Rdep"
                                        + " WHERE EntidadRegulatoria.ro_rdep.IdEmpresa = " + IdEmpresa.ToString() + " and (EntidadRegulatoria.ro_rdep.Estado = 1) and EntidadRegulatoria.ro_rdep.pe_anio = " + Anio.ToString()
                                        + " order by EntidadRegulatoria.ro_rdep_det.pe_apellido +' '+ EntidadRegulatoria.ro_rdep_det.pe_nombre";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ro_rdep_det_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            Id_Rdep = Convert.ToInt32(reader["Id_Rdep"]),
                            pe_anio = Convert.ToInt32(reader["pe_anio"]),
                            Su_CodigoEstablecimiento = reader["Su_CodigoEstablecimiento"].ToString(),
                            Observacion = reader["Observacion"].ToString(),
                            Secuencia = Convert.ToInt32(reader["Secuencia"]),
                            pe_cedulaRuc = reader["pe_cedulaRuc"].ToString(),
                            Empleado = reader["Empleado"].ToString(),
                            pe_nombre = reader["pe_nombre"].ToString(),
                            pe_apellido = reader["pe_apellido"].ToString(),
                            Sueldo = reader["Sueldo"] == DBNull.Value ? null : (double?)reader["Sueldo"],
                            FondosReserva = reader["FondosReserva"] == DBNull.Value ? null : (double?)reader["FondosReserva"],
                            DecimoTercerSueldo = reader["DecimoTercerSueldo"] == DBNull.Value ? null : (double?)reader["DecimoTercerSueldo"],
                            DecimoCuartoSueldo = reader["DecimoCuartoSueldo"] == DBNull.Value ? null : (double?)reader["DecimoCuartoSueldo"],
                            Vacaciones = reader["Vacaciones"] == DBNull.Value ? null : (double?)reader["Vacaciones"],
                            AportePErsonal = reader["AportePErsonal"] == DBNull.Value ? null : (double?)reader["AportePErsonal"],
                            GastoAlimentacion = reader["GastoAlimentacion"] == DBNull.Value ? null : (double?)reader["GastoAlimentacion"],
                            GastoEucacion = reader["GastoEucacion"] == DBNull.Value ? null : (double?)reader["GastoEucacion"],
                            GastoSalud = reader["GastoSalud"] == DBNull.Value ? null : (double?)reader["GastoSalud"],
                            GastoVestimenta = reader["GastoVestimenta"] == DBNull.Value ? null : (double?)reader["GastoVestimenta"],
                            GastoVivienda = reader["GastoVivienda"] == DBNull.Value ? null : (double?)reader["GastoVivienda"],
                            Utilidades = reader["Utilidades"] == DBNull.Value ? null : (double?)reader["Utilidades"],
                            IngresoVarios = reader["IngresoVarios"] == DBNull.Value ? null : (double?)reader["IngresoVarios"],
                            IngresoPorOtrosEmpleaodres = reader["IngresoPorOtrosEmpleaodres"] == DBNull.Value ? null : (double?)reader["IngresoPorOtrosEmpleaodres"],
                            IessPorOtrosEmpleadores = reader["IessPorOtrosEmpleadores"] == DBNull.Value ? null : (double?)reader["IessPorOtrosEmpleadores"],
                            ValorImpuestoPorEsteEmplador = reader["ValorImpuestoPorEsteEmplador"] == DBNull.Value ? null : (double?)reader["ValorImpuestoPorEsteEmplador"],
                            ValorImpuestoPorOtroEmplador = reader["ValorImpuestoPorOtroEmplador"] == DBNull.Value ? null : (double?)reader["ValorImpuestoPorOtroEmplador"],
                            ExoneraionPorDiscapacidad = reader["ExoneraionPorDiscapacidad"] == DBNull.Value ? null : (double?)reader["ExoneraionPorDiscapacidad"],
                            ExoneracionPorTerceraEdad = reader["ExoneracionPorTerceraEdad"] == DBNull.Value ? null : (double?)reader["ExoneracionPorTerceraEdad"],
                            OtrosIngresosRelacionDependencia = reader["OtrosIngresosRelacionDependencia"] == DBNull.Value ? null : (double?)reader["OtrosIngresosRelacionDependencia"],
                            ImpuestoRentaCausado = reader["ImpuestoRentaCausado"] == DBNull.Value ? null : (double?)reader["ImpuestoRentaCausado"],
                            ValorImpuestoRetenidoTrabajador = reader["ValorImpuestoRetenidoTrabajador"] == DBNull.Value ? null : (double?)reader["ValorImpuestoRetenidoTrabajador"],
                            ImpuestoRentaAsumidoPorEsteEmpleador = reader["ImpuestoRentaAsumidoPorEsteEmpleador"] == DBNull.Value ? null : (double?)reader["ImpuestoRentaAsumidoPorEsteEmpleador"],
                            BaseImponibleGravada = reader["BaseImponibleGravada"] == DBNull.Value ? null : (double?)reader["BaseImponibleGravada"],
                            IngresosGravadorPorEsteEmpleador = reader["IngresosGravadorPorEsteEmpleador"] == DBNull.Value ? null : (double?)reader["IngresosGravadorPorEsteEmpleador"]
                        });
                    }
                    reader.Close();
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
