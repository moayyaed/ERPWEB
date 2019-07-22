using Core.Erp.Data.Contabilidad;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorPagar
{
    public class cp_ConciliacionAnticipo_Data
    {
        cp_parametros_Data data_cp_parametro = new cp_parametros_Data();
        ct_cbtecble_Data data_cbtecble = new ct_cbtecble_Data();
        ct_cbtecble_det_Data data_cbtecble_det = new ct_cbtecble_det_Data();

        public List<cp_conciliacionAnticipo_Info> getlist(int IdEmpresa, int IdSucursal, DateTime Fecha_ini, DateTime Fecha_fin, bool MostrarAnulados)
        {
            try
            {
                int IdSucursalInicio = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

                List<cp_conciliacionAnticipo_Info> Lista = new List<cp_conciliacionAnticipo_Info>();

                using (Entities_cuentas_por_pagar db = new Entities_cuentas_por_pagar())
                {
                    Lista = (from q in db.vwcp_ConciliacionAnticipo
                                where q.IdEmpresa == IdEmpresa
                                && q.IdSucursal >= IdSucursalInicio 
                                && q.IdSucursal <= IdSucursalFin
                                && q.Estado == (MostrarAnulados== true ? q.Estado : true)
                                select new cp_conciliacionAnticipo_Info
                                {
                                    IdEmpresa = q.IdEmpresa,
                                    IdConciliacion = q.IdConciliacion,
                                    IdSucursal = q.IdSucursal,
                                    IdProveedor = q.IdProveedor,
                                    Fecha = q.Fecha,
                                    Observacion = q.Observacion,
                                    Estado = q.Estado

                                }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public cp_conciliacionAnticipo_Info get_info(int IdEmpresa, int IdConciliacion)
        {
            try
            {
                cp_conciliacionAnticipo_Info info = new cp_conciliacionAnticipo_Info();
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_ConciliacionAnticipo Entity = Context.cp_ConciliacionAnticipo.Where(q => q.IdConciliacion == IdConciliacion && q.IdEmpresa == IdEmpresa).FirstOrDefault();

                    if (Entity == null) return null;
                    info = new cp_conciliacionAnticipo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdConciliacion = Entity.IdConciliacion,
                        IdSucursal = Entity.IdSucursal,
                        IdProveedor = Entity.IdProveedor,
                        Fecha = Entity.Fecha,
                        Observacion = Entity.Observacion,
                        Estado = Entity.Estado,
                        IdTipoCbte = Entity.IdTipoCbte,
                        IdCbteCble = Entity.IdCbteCble
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
                using (Entities_cuentas_por_pagar db = new Entities_cuentas_por_pagar())
                {
                    var Lista = db.cp_ConciliacionAnticipo.Where(q => q.IdEmpresa == IdEmpresa).Select(q => q.IdConciliacion);

                    if (Lista.Count() > 0)
                        ID = Convert.ToInt32(Lista.Max() + 1);
                }
                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarBD(cp_conciliacionAnticipo_Info info)
        {
            try
            {
                cp_parametros_Info info_parametros = new cp_parametros_Info();
                info_parametros = data_cp_parametro.get_info(info.IdEmpresa);

                using (Entities_contabilidad db_cont = new Entities_contabilidad())
                {                 
                    if(info_parametros!= null && info_parametros.pa_TipoCbte_para_conci_x_antcipo!= null)
                    {
                        ct_cbtecble_Info info_diario = armar_info(info.Lista_det_Cbte, info.IdEmpresa, info.IdSucursal, Convert.ToInt32(info_parametros.pa_TipoCbte_para_conci_x_antcipo), 0, info.Observacion, info.Fecha);

                        if (info_diario != null)
                        {
                            if (data_cbtecble.guardarDB(info_diario))
                            {
                                info.IdTipoCbte = info_diario.IdTipoCbte;
                                info.IdCbteCble = info_diario.IdCbteCble;
                            }
                        }
                    }
          
                }

                using (Entities_cuentas_por_pagar db = new Entities_cuentas_por_pagar())
                {
                    db.cp_ConciliacionAnticipo.Add(new cp_ConciliacionAnticipo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdConciliacion = info.IdConciliacion = get_id(info.IdEmpresa),
                        IdSucursal = info.IdSucursal,
                        IdProveedor = info.IdProveedor,
                        Fecha = info.Fecha,
                        Observacion = info.Observacion,
                        IdTipoCbte = info.IdTipoCbte,
                        IdCbteCble = info.IdCbteCble,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                    });



                    if (info.Lista_det_OP != null)
                    {
                        int SecuenciaOP = 1;

                        foreach (var item in info.Lista_det_OP)
                        {

                            db.cp_ConciliacionAnticipoDetAnt.Add(new cp_ConciliacionAnticipoDetAnt
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdConciliacion = info.IdConciliacion,
                                Secuencia = SecuenciaOP++,
                                IdOrdenPago = item.IdOrdenPago,
                                MontoAplicado = item.MontoAplicado
                            });

                        }
                    }

                    if (info.Lista_det_Fact != null)
                    {
                        int SecuenciaFact = 1;

                        foreach (var item in info.Lista_det_Fact)
                        {

                            db.cp_ConciliacionAnticipoDetCXP.Add(new cp_ConciliacionAnticipoDetCXP
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdConciliacion = info.IdConciliacion,
                                Secuencia = SecuenciaFact++,
                                IdOrdenPago = item.IdOrdenPago,
                                IdEmpresa_cxp = item.IdEmpresa,
                                IdTipoCbte_cxp = item.IdTipoCbte_cxp,
                                IdCbteCble_cxp = item.IdCbteCble_cxp,
                                MontoAplicado = item.MontoAplicado
                            });

                        }
                    }

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                eliminar(info.IdEmpresa, Convert.ToInt32(info.IdTipoCbte), Convert.ToDecimal(info.IdCbteCble));
                throw;                
            }
        }


        public bool ModificarBD(cp_conciliacionAnticipo_Info info)
        {
            try
            {
                using (Entities_contabilidad db_cont = new Entities_contabilidad())
                {
                    //ct_cbtecble_Info info_diario = armar_info(info.ListaDiario, info.IdEmpresa, info.IdSucursal, Convert.ToInt32(info.IdTipoCbte), 0, info.Observacion, info.Fecha);
                    ct_cbtecble Entity_cbte = db_cont.ct_cbtecble.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdTipoCbte == info.IdTipoCbte && q.IdCbteCble == info.IdCbteCble).FirstOrDefault();

                    Entity_cbte.IdEmpresa = info.InfoCbte.IdEmpresa;
                    Entity_cbte.IdSucursal = info.InfoCbte.IdSucursal;
                    Entity_cbte.IdTipoCbte = info.InfoCbte.IdTipoCbte;
                    Entity_cbte.IdCbteCble = info.InfoCbte.IdCbteCble;
                    Entity_cbte.CodCbteCble = info.InfoCbte.CodCbteCble;
                    Entity_cbte.IdPeriodo = info.InfoCbte.IdPeriodo;
                    Entity_cbte.cb_Fecha = info.InfoCbte.cb_Fecha;
                    Entity_cbte.cb_Valor = info.InfoCbte.cb_Valor;
                    Entity_cbte.cb_Observacion = info.InfoCbte.cb_Observacion;
                    Entity_cbte.cb_Estado = info.InfoCbte.cb_Estado;
                    Entity_cbte.IdUsuario = info.IdUsuarioCreacion;
                    Entity_cbte.cb_FechaTransac = DateTime.Now;

                    var lst_det_cbte = db_cont.ct_cbtecble_det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdTipoCbte == info.InfoCbte.IdTipoCbte && q.IdCbteCble == info.InfoCbte.IdCbteCble).ToList();
                    db_cont.ct_cbtecble_det.RemoveRange(lst_det_cbte);

                    if (info.Lista_det_Cbte != null)
                    {
                        int Secuencia = 1;

                        foreach (var item in info.Lista_det_Cbte)
                        {

                            db_cont.ct_cbtecble_det.Add(new ct_cbtecble_det
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdTipoCbte = item.IdTipoCbte,
                                IdCbteCble = item.IdCbteCble,
                                secuencia = Secuencia++,
                                IdCtaCble = item.IdCtaCble,
                                dc_Valor = item.dc_Valor,
                                dc_Observacion = item.dc_Observacion,
                                dc_para_conciliar = item.dc_para_conciliar,
                                IdPunto_cargo_grupo = item.IdPunto_cargo_grupo,
                                IdPunto_cargo = item.IdPunto_cargo,
                                IdCentroCosto = item.IdCentroCosto
                            });

                        }
                    }
                    db_cont.SaveChanges();
                }

                using (Entities_cuentas_por_pagar db = new Entities_cuentas_por_pagar())
                {
                    cp_ConciliacionAnticipo entity = db.cp_ConciliacionAnticipo.Where(q => q.IdConciliacion == info.IdConciliacion && q.IdEmpresa == info.IdEmpresa).FirstOrDefault();

                    if (entity == null)
                    {
                        return false;
                    }

                    entity.IdProveedor = info.IdProveedor;
                    entity.Fecha = info.Fecha;
                    entity.Observacion = info.Observacion;
                    entity.IdTipoCbte = info.IdTipoCbte;
                    entity.IdCbteCble = info.IdCbteCble;
                    entity.IdSucursal = info.IdSucursal;
                    entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    entity.FechaModificacion = DateTime.Now;

                    var lst_det_OP = db.cp_ConciliacionAnticipoDetAnt.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdConciliacion == info.IdConciliacion).ToList();
                    db.cp_ConciliacionAnticipoDetAnt.RemoveRange(lst_det_OP);

                    var lst_det_Fact = db.cp_ConciliacionAnticipoDetCXP.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdConciliacion == info.IdConciliacion).ToList();
                    db.cp_ConciliacionAnticipoDetCXP.RemoveRange(lst_det_Fact);

                    if (info.Lista_det_OP != null)
                    {
                        int SecuenciaOP = 1;

                        foreach (var item in info.Lista_det_OP)
                        {

                            db.cp_ConciliacionAnticipoDetAnt.Add(new cp_ConciliacionAnticipoDetAnt
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdConciliacion = info.IdConciliacion,
                                Secuencia = SecuenciaOP++,
                                IdOrdenPago = item.IdOrdenPago,
                                MontoAplicado = item.MontoAplicado
                            });

                        }
                    }

                    if (info.Lista_det_Fact != null)
                    {
                        int SecuenciaFact = 1;

                        foreach (var item in info.Lista_det_Fact)
                        {

                            db.cp_ConciliacionAnticipoDetCXP.Add(new cp_ConciliacionAnticipoDetCXP
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdConciliacion = info.IdConciliacion,
                                Secuencia = SecuenciaFact++,
                                IdOrdenPago = item.IdOrdenPago,
                                IdEmpresa_cxp = item.IdEmpresa,
                                IdTipoCbte_cxp = item.IdTipoCbte_cxp,
                                IdCbteCble_cxp = item.IdCbteCble_cxp,
                                MontoAplicado = item.MontoAplicado
                            });

                        }
                    }

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularBD(cp_conciliacionAnticipo_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar db = new Entities_cuentas_por_pagar())
                {
                    cp_ConciliacionAnticipo entity = db.cp_ConciliacionAnticipo.Where(q => q.IdConciliacion == info.IdConciliacion && q.IdEmpresa == info.IdEmpresa).FirstOrDefault();

                    if (entity == null)
                    {
                        return false;
                    }

                    entity.Estado = false;
                    entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    entity.FechaAnulacion = DateTime.Now;
                    entity.MotivoAnulacion = info.MotivoAnulacion;

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ct_cbtecble_Info armar_info(List<ct_cbtecble_det_Info> lista, int IdEmpresa, int IdSucursal, int IdTipoCbte, decimal IdCbteCble, string Observacion, DateTime Fecha)
        {
            try
            {
                ct_cbtecble_Info info = new ct_cbtecble_Info
                {
                    IdEmpresa = IdEmpresa,
                    IdTipoCbte = IdTipoCbte,
                    IdCbteCble = IdCbteCble,
                    IdSucursal = IdSucursal,
                    cb_Observacion = Observacion,
                    cb_Fecha = Fecha,
                    cb_Valor = lista.Where(q => q.dc_Valor > 0).Sum(q => q.dc_Valor)
                };

                info.lst_ct_cbtecble_det = lista;
                info.lst_ct_cbtecble_det.ForEach(q => { q.IdEmpresa = IdEmpresa; q.IdTipoCbte = IdTipoCbte; q.IdCbteCble = IdCbteCble; });

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool eliminar(int IdEmpresa, int IdTipoCbte, decimal IdCbtecble)
        {
            try
            {
                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    var lst_cbte_det = db.ct_cbtecble_det.Where(q => q.IdEmpresa == IdEmpresa && q.IdTipoCbte == IdTipoCbte && q.IdCbteCble == IdCbtecble).ToList();
                    db.ct_cbtecble_det.RemoveRange(lst_cbte_det);

                    var cbte = db.ct_cbtecble.Where(q => q.IdEmpresa == IdEmpresa && q.IdTipoCbte == IdTipoCbte && q.IdCbteCble == IdCbtecble).FirstOrDefault();
                    db.ct_cbtecble.Remove(cbte);

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
