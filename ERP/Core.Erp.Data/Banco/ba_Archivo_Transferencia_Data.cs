using Core.Erp.Info.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Banco
{
  public  class ba_Archivo_Transferencia_Data
    {
        public List<ba_Archivo_Transferencia_Info> GetList(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<ba_Archivo_Transferencia_Info> Lista;
                using (Entities_banco Context = new Entities_banco())
                {
                    if(mostrar_anulados)
                    {
                        Lista = Context.ba_Archivo_Transferencia.Where(q => q.IdEmpresa == IdEmpresa).Select(q => new ba_Archivo_Transferencia_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            cod_archivo = q.cod_archivo,
                            Cod_Empresa = q.Cod_Empresa,
                            Contabilizado = q.Contabilizado,
                            Estado = q.Estado,
                            Fecha = q.Fecha,
                            Fecha_Proceso = q.Fecha_Proceso,
                            IdArchivo = q.IdArchivo,
                            IdBanco = q.IdBanco,
                            IdProceso_bancario = q.IdProceso_bancario,
                            Nom_Archivo = q.Nom_Archivo,
                            Observacion = q.Observacion,
                            IdSucursal = q.IdSucursal
                        }).ToList();

                    }
                    else
                    {
                        Lista = Context.ba_Archivo_Transferencia.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == true).Select(q => new ba_Archivo_Transferencia_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            cod_archivo = q.cod_archivo,
                            Cod_Empresa = q.Cod_Empresa,
                            Contabilizado = q.Contabilizado,
                            Estado = true,
                            Fecha = q.Fecha,
                            Fecha_Proceso = q.Fecha_Proceso,
                            IdArchivo = q.IdArchivo,
                            IdBanco = q.IdBanco,
                            IdProceso_bancario = q.IdProceso_bancario,
                            Nom_Archivo = q.Nom_Archivo,
                            Observacion = q.Observacion,
                            IdSucursal = q.IdSucursal
                        }).ToList();
                    }
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ba_Archivo_Transferencia_Info GetInfo(int IdEmpresa, decimal IdArchivo)
        {
            try
            {
                ba_Archivo_Transferencia_Info info = new ba_Archivo_Transferencia_Info();
                using (Entities_banco Context = new Entities_banco())
                {
                    ba_Archivo_Transferencia Entity = Context.ba_Archivo_Transferencia.Where(q => q.IdEmpresa == IdEmpresa && q.IdArchivo == IdArchivo).FirstOrDefault();
                    if (Entity == null) return null;
                    info = new ba_Archivo_Transferencia_Info
                    {
                            IdEmpresa = Entity.IdEmpresa,
                            cod_archivo = Entity.cod_archivo,
                            Cod_Empresa = Entity.Cod_Empresa,
                            Contabilizado = Entity.Contabilizado,
                            Estado = Entity.Estado,
                            Fecha = Entity.Fecha,
                            Fecha_Proceso = Entity.Fecha_Proceso,
                            IdArchivo = Entity.IdArchivo,
                            IdBanco = Entity.IdBanco,
                            IdProceso_bancario = Entity.IdProceso_bancario,
                            Nom_Archivo = Entity.Nom_Archivo,
                            Observacion = Entity.Observacion,
                            IdSucursal = Entity.IdSucursal,
                            SecuencialInicial = Entity.SecuencialInicial
                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private decimal GetId(int IdEmpresa)
        {
            try
            {
                decimal Id = 1;
                using (Entities_banco Context = new Entities_banco())
                {
                    var lst = from q in Context.ba_Archivo_Transferencia
                              where q.IdEmpresa == IdEmpresa
                              select q;
                    if (lst.Count() > 0)
                        Id = lst.Max(q => q.IdArchivo) + 1;
                }
                return Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(ba_Archivo_Transferencia_Info info)
        {
            try
            {
                info.Nom_Archivo = "PAGOS_MULTICASH_"+info.Fecha.ToString("yyyyMMdd")+"_01";
                using (Entities_banco Context = new Entities_banco())
                {
                    Context.ba_Archivo_Transferencia.Add(new ba_Archivo_Transferencia
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdArchivo = info.IdArchivo=GetId(info.IdEmpresa),
                        cod_archivo = info.cod_archivo,
                        Cod_Empresa = info.Cod_Empresa = "",
                        Contabilizado = info.Contabilizado,
                        Estado = true,
                        Fecha = info.Fecha.Date,
                        IdBanco = info.IdBanco,
                        IdProceso_bancario = info.IdProceso_bancario,
                        Nom_Archivo = info.Nom_Archivo,
                        Observacion = info.Observacion,
                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = DateTime.Now,
                        IdSucursal = info.IdSucursal,
                        SecuencialInicial = info.SecuencialInicial
                    });

                    int Secuencia = 1;
                    if(info.Lst_det.Count()>0)
                    {
                        foreach (var item in info.Lst_det)
                        {
                            Context.ba_Archivo_Transferencia_Det.Add(new ba_Archivo_Transferencia_Det
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdArchivo = info.IdArchivo,
                                Contabilizado = item.Contabilizado,
                                Estado = item.Estado,
                                Fecha_proceso = item.Fecha_proceso,
                                IdOrdenPago = item.IdOrdenPago,
                                IdEmpresa_OP = info.IdEmpresa,
                                Secuencia = Secuencia++,
                                Secuencial_reg_x_proceso = item.Secuencial_reg_x_proceso,
                                Secuencia_OP = item.Secuencia_OP,
                                Referencia = item.Referencia,
                                
                                Valor = item.Valor
                            });
                        }
                    }


                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarDB(ba_Archivo_Transferencia_Info info)
        {
            try
            {
                using (Entities_banco Context = new Entities_banco())
                {
                    ba_Archivo_Transferencia Entity = Context.ba_Archivo_Transferencia.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdArchivo == info.IdArchivo).FirstOrDefault();
                    if (Entity == null) return false;

                    Entity.Nom_Archivo = "PAGOS_MULTICASH_" + info.Fecha.ToString("yyyyMMdd") + "_01";
                    Entity.Observacion = info.Observacion;
                    Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    Entity.Fecha_UltMod = DateTime.Now;

                    var Lst_det = Context.ba_Archivo_Transferencia_Det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdArchivo == info.IdArchivo).ToList();
                    Context.ba_Archivo_Transferencia_Det.RemoveRange(Lst_det);
                    if (info.Lst_det.Count() > 0)
                    {
                        foreach (var item in info.Lst_det)
                        {
                            Context.ba_Archivo_Transferencia_Det.Add(new ba_Archivo_Transferencia_Det
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdArchivo = info.IdArchivo,
                                Contabilizado = item.Contabilizado,
                                Estado = item.Estado,
                                Fecha_proceso = item.Fecha_proceso,
                                IdOrdenPago = item.IdOrdenPago,
                                IdEmpresa_OP = item.IdEmpresa_OP,
                                Secuencia = item.Secuencia,
                                Secuencial_reg_x_proceso = item.Secuencial_reg_x_proceso,
                                Secuencia_OP = item.Secuencia_OP,
                                Valor = item.Valor,
                                Referencia = item.Referencia
                            });
                        }
                    }

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularDB(ba_Archivo_Transferencia_Info info)
        {
            try
            {
                using (Entities_banco Context = new Entities_banco())
                {
                    var Lst_det = Context.ba_Archivo_Transferencia_Det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdArchivo == info.IdArchivo).ToList();
                    Context.ba_Archivo_Transferencia_Det.RemoveRange(Lst_det);
                    Context.ba_Archivo_Transferencia.Remove(Context.ba_Archivo_Transferencia.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdArchivo == info.IdArchivo).FirstOrDefault());
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
