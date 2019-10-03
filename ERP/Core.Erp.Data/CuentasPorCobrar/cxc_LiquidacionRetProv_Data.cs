using Core.Erp.Data.General;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorCobrar
{
    public class cxc_LiquidacionRetProv_Data
    {
        public List<cxc_LiquidacionRetProv_Info> get_list(int IdEmpresa, DateTime Fecha_ini, DateTime Fecha_fin)
        {
            try
            {
                List<cxc_LiquidacionRetProv_Info> Lista;

                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    Lista = (from q in Context.cxc_LiquidacionRetProv
                             where q.IdEmpresa == IdEmpresa
                             && Fecha_ini <= q.li_Fecha && q.li_Fecha <= Fecha_fin
                             orderby q.IdLiquidacion descending
                             select new cxc_LiquidacionRetProv_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdLiquidacion = q.IdLiquidacion,
                                 li_Fecha = q.li_Fecha,
                                 Observacion = q.Observacion,
                                 Estado = q.Estado,
                                 IdTipoCbte = q.IdTipoCbte
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private decimal get_id(int IdEmpesa)
        {
            try
            {
                decimal ID = 1;

                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var lst = from q in Context.cxc_LiquidacionRetProv
                              where q.IdEmpresa == IdEmpesa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdLiquidacion) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public cxc_LiquidacionRetProv_Info get_info(int IdEmpresa, decimal IdLiquidacion)
        {
            try
            {
                cxc_LiquidacionRetProv_Info info;

                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var Entity = Context.cxc_LiquidacionRetProv.Where(q => q.IdEmpresa == IdEmpresa && q.IdLiquidacion == IdLiquidacion).FirstOrDefault();
                    if (Entity == null) return null;
                    info = new cxc_LiquidacionRetProv_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdLiquidacion = Entity.IdLiquidacion,
                        li_Fecha = Entity.li_Fecha,
                        Observacion = Entity.Observacion,
                        Estado = Entity.Estado,
                        IdTipoCbte = Entity.IdTipoCbte
                    };
                }

                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(cxc_LiquidacionRetProv_Info info)
        {
            try
            {
                int secuencia = 1;
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    cxc_LiquidacionRetProv Entity = new cxc_LiquidacionRetProv
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdLiquidacion = info.IdLiquidacion = get_id(info.IdEmpresa),
                        li_Fecha = info.li_Fecha,
                        Observacion = info.Observacion,
                        Estado = info.Estado,
                        IdTipoCbte = info.IdTipoCbte
                    };

                    Context.cxc_LiquidacionRetProv.Add(Entity);

                    foreach (var item in info.lst_detalle)
                    {
                        Context.cxc_LiquidacionRetProvDet.Add(new cxc_LiquidacionRetProvDet
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdLiquidacion = info.IdLiquidacion,
                            IdSucursal = item.IdSucursal,
                            IdCobro = item.IdCobro,
                            Secuencia = secuencia++,
                            IdCobro_tipo = item.IdCobro_tipo,
                            Valor = item.dc_ValorPago,
                        });

                        secuencia++;
                    }

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_guia_remision_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuarioCreacion });
                return false;
            }
        }
        public bool modificarDB(cxc_LiquidacionRetProv_Info info)
        {
            try
            {
                int secuencia = 1;
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var Entity = Context.cxc_LiquidacionRetProv.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdLiquidacion == info.IdLiquidacion).FirstOrDefault();
                    if (Entity == null) return false;

                    Entity.li_Fecha = info.li_Fecha;
                    Entity.Observacion = info.Observacion;
                    Entity.IdTipoCbte = info.IdTipoCbte;

                    Context.cxc_LiquidacionRetProv.Add(Entity);

                    var lst = Context.cxc_LiquidacionRetProvDet.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdLiquidacion == info.IdLiquidacion).ToList();
                    Context.cxc_LiquidacionRetProvDet.RemoveRange(lst);

                    foreach (var item in info.lst_detalle)
                    {
                        Context.cxc_LiquidacionRetProvDet.Add(new cxc_LiquidacionRetProvDet
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdLiquidacion = info.IdLiquidacion,
                            IdSucursal = item.IdSucursal,
                            IdCobro = item.IdCobro,
                            Secuencia = secuencia++,
                            IdCobro_tipo = item.IdCobro_tipo,
                            Valor = item.dc_ValorPago,
                        });

                        secuencia++;
                    }

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_guia_remision_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuarioCreacion });
                return false;
            }
        }

        public bool anularDB(cxc_LiquidacionRetProv_Info info)
        {
            try
            {
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var Entity = Context.cxc_LiquidacionRetProv.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdLiquidacion == info.IdLiquidacion).FirstOrDefault();
                    if (Entity == null) return false;

                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    Entity.FechaAnulacion = DateTime.Now;
                    Entity.MotivoAnulacion = info.MotivoAnulacion;
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
