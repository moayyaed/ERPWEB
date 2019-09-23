using Core.Erp.Data.General;
using Core.Erp.Info.General;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.RRHH
{
    public class ro_PrestamoMasivo_Data
    {
        ro_Parametros_Data odata = new ro_Parametros_Data();

        public List<ro_PrestamoMasivo_Info> get_list(int IdEmpresa, DateTime fecha_ini, DateTime fecha_fin, int IdSucursal, bool MostrarAnulados)
        {
            try
            {
                List<ro_PrestamoMasivo_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = (from q in Context.ro_PrestamoMasivo
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.Fecha_PriPago >= fecha_ini
                             && q.Fecha_PriPago <= fecha_fin
                             && q.Estado == (MostrarAnulados== true ? q.Estado : true)
                             select new ro_PrestamoMasivo_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdCarga = q.IdCarga,
                                 Fecha_PriPago = q.Fecha_PriPago,
                                 NumCuotas = q.NumCuotas,
                                 descuento_quincena = q.descuento_quincena,
                                 descuento_mensual = q.descuento_mensual,
                                 descuento_men_quin = q.descuento_men_quin,
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

        public ro_PrestamoMasivo_Info get_info(int IdEmpresa, decimal IdCarga)
        {
            try
            {
                ro_PrestamoMasivo_Info info = new ro_PrestamoMasivo_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_PrestamoMasivo Entity = Context.ro_PrestamoMasivo.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdCarga == IdCarga);
                    if (Entity == null) return null;

                    info = new ro_PrestamoMasivo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal= Entity.IdSucursal,
                        IdCarga = Entity.IdCarga,
                        Fecha_PriPago = Entity.Fecha_PriPago,
                        NumCuotas = Entity.NumCuotas,
                        descuento_mensual= Entity.descuento_mensual,
                        descuento_men_quin= Entity.descuento_men_quin,
                        descuento_quincena = Entity.descuento_quincena,
                        Observacion= Entity.Observacion
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public decimal get_id(int IdEmpresa)
        {
            try
            {
                decimal ID = 1;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    var lst = from q in Context.ro_PrestamoMasivo
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdCarga) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(ro_PrestamoMasivo_Info info)
        {
            try
            {
                var ro_parametro = odata.get_info(info.IdEmpresa);
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_PrestamoMasivo Entity = new ro_PrestamoMasivo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdCarga = info.IdCarga = get_id(info.IdEmpresa),
                        Fecha_PriPago = info.Fecha_PriPago,
                        NumCuotas = info.NumCuotas,
                        descuento_mensual = info.descuento_mensual,
                        descuento_men_quin = info.descuento_men_quin,
                        descuento_quincena = info.descuento_quincena,
                        Observacion = info.Observacion,
                        Estado = info.Estado = true,
                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = info.Fecha_Transac = DateTime.Now
                    };
                    Context.ro_PrestamoMasivo.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "ro_prestamo_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }
        public bool modificarDB(ro_PrestamoMasivo_Info info)
        {
            try
            {
                var ro_parametro = odata.get_info(info.IdEmpresa);
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_PrestamoMasivo Entity = Context.ro_PrestamoMasivo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCarga == info.IdCarga);
                    if (Entity == null)
                        return false;
                    Entity.Fecha_PriPago = info.Fecha_PriPago;
                    Entity.NumCuotas = info.NumCuotas;
                    Entity.descuento_mensual = info.descuento_mensual;
                    Entity.descuento_men_quin = info.descuento_men_quin;
                    Entity.descuento_quincena = info.descuento_quincena;
                    Entity.Observacion = info.Observacion;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "ro_prestamo_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }
        public bool anularDB(ro_PrestamoMasivo_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_PrestamoMasivo Entity = Context.ro_PrestamoMasivo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCarga == info.IdCarga);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = false;
                    Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    Entity.Fecha_UltAnu = info.Fecha_UltAnu = DateTime.Now;
                    Entity.MotiAnula = info.MotiAnula;
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
