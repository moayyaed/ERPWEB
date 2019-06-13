using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.CuentasPorPagar;
namespace Core.Erp.Data.CuentasPorPagar
{
   public class cp_cuotas_x_doc_Data
    {
        public List<cp_cuotas_x_doc_Info> get_list(int IdEmpresa)
        {
            try
            {
                List<cp_cuotas_x_doc_Info> Lista ;

                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    Lista =( from q in Context.cp_cuotas_x_doc
                              where IdEmpresa == q.IdEmpresa
                              select new cp_cuotas_x_doc_Info 
                              {
                                  IdEmpresa = q.IdEmpresa,
                                  IdCuota = q.IdCuota,
                                  Observacion = q.Observacion,
                                  Estado = q.Estado,
                                  Fecha_inicio = q.Fecha_inicio,
                                  Dias_plazo = q.Dias_plazo,
                                  Num_cuotas = q.Num_cuotas,
                                  Total_a_pagar = q.Total_a_pagar,
                                  IdCbteCble = q.IdCbteCble,
                                  IdTipoCbte = q.IdTipoCbte,
                                  IdEmpresa_ct = q.IdEmpresa_ct
                              }).ToList();
            }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public cp_cuotas_x_doc_Info get_info(int IdEmpresa, decimal IdCuota)
        {
            try
            {
                cp_cuotas_x_doc_Info info = new cp_cuotas_x_doc_Info();

                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    var lst = from q in Context.cp_cuotas_x_doc
                              where IdEmpresa == q.IdEmpresa
                              && q.IdCuota == IdCuota
                              select q;

                    foreach (var item in lst)
                    {
                        info.IdEmpresa = item.IdEmpresa;
                        info.IdCuota = item.IdCuota;
                        info.Observacion = item.Observacion;
                        info.Estado = item.Estado;
                        info.Fecha_inicio = item.Fecha_inicio;
                        info.Dias_plazo = item.Dias_plazo;
                        info.Num_cuotas = item.Num_cuotas;
                        info.Total_a_pagar = item.Total_a_pagar;
                        info.IdCbteCble = item.IdCbteCble;
                        info.IdTipoCbte = item.IdTipoCbte;
                        info.IdEmpresa_ct = item.IdEmpresa_ct;

                    }
                }

                return info;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public cp_cuotas_x_doc_Info get_info(int IdEmpresa, int IdTipoCbte, decimal IdCbteCble)
        {
            try
            {
                cp_cuotas_x_doc_Info info = new cp_cuotas_x_doc_Info();

                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_cuotas_x_doc Entity = Context.cp_cuotas_x_doc.FirstOrDefault(q => q.IdEmpresa == IdEmpresa&& q.IdCbteCble==IdCbteCble&& q.IdTipoCbte==IdTipoCbte);
                    if (Entity == null) return null;
                    info = new cp_cuotas_x_doc_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdCuota = Entity.IdCuota,
                       IdEmpresa_ct = Entity.IdEmpresa_ct,
                        IdTipoCbte = Entity.IdTipoCbte,
                        IdCbteCble = Entity.IdCbteCble,
                        Total_a_pagar = Entity.Total_a_pagar,
                        Num_cuotas = Entity.Num_cuotas,
                        Dias_plazo = Entity.Dias_plazo,
                        Fecha_inicio = Entity.Fecha_inicio,
                        Estado=Entity.Estado,
                        Observacion=Entity.Observacion,
                    };
                    //info.lst_cuotas_det
                    //      = (from q in Context.cp_cuotas_x_doc_det
                    //         where q.IdCuota ==info.IdCuota
                    //         && q.IdEmpresa==IdEmpresa
                    //              select new cp_cuotas_x_doc_det_Info
                    //              {
                    //                  IdEmpresa = q.IdEmpresa,
                    //                  IdCuota = q.IdCuota,
                    //                  Secuencia = q.Secuencia,
                    //                  Num_cuota = q.Num_cuota,
                    //                  Fecha_vcto_cuota = q.Fecha_vcto_cuota,
                    //                  Valor_cuota = q.Valor_cuota,
                    //                  Observacion = q.Observacion,
                    //                  Estado = q.Estado,
                    //              }).ToList();
                }

                return info;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public decimal get_id(int IdEmpresa)
        {
            try
            {
                decimal ID = 0;

                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    var lst = from q in Context.cp_cuotas_x_doc
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() == 0)
                    {
                        ID = 1;
                    }
                    else
                    {
                        ID = lst.Max(q => q.IdCuota) + 1;
                    }
                }

                return ID;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public Boolean guardarDB(cp_cuotas_x_doc_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    var lst = from q in Context.cp_cuotas_x_doc
                              where q.IdEmpresa == info.IdEmpresa
                              && q.IdCuota == info.IdCuota
                              select q;
                    if (lst.Count() == 0)
                    {
                        cp_cuotas_x_doc Entity = new cp_cuotas_x_doc
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdCuota = info.IdCuota = get_id(info.IdEmpresa),
                            Observacion = info.Observacion,
                            Estado = true,
                            Fecha_inicio = info.Fecha_inicio.Date,
                            Dias_plazo = info.Dias_plazo,
                            Num_cuotas = info.Num_cuotas,
                            Total_a_pagar = info.Total_a_pagar,
                            IdCbteCble = info.IdCbteCble,
                            IdTipoCbte = info.IdTipoCbte,
                            IdEmpresa_ct = info.IdEmpresa_ct
                        };
                        Context.cp_cuotas_x_doc.Add(Entity);
                        Context.SaveChanges();

                        //cp_cuotas_x_doc_det_Data oData_det = new cp_cuotas_x_doc_det_Data();
                        //foreach (var item in info.lst_cuotas_det)
                        //{
                        //    item.IdCuota = info.IdCuota;
                        //    item.IdEmpresa = info.IdEmpresa;

                        //}
                        //oData_det.GuardarDB(info.lst_cuotas_det);
                    }
                    else
                    {
                        modificarDB(info);
                    }
                }

                return true;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public Boolean modificarDB(cp_cuotas_x_doc_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_cuotas_x_doc Entity = Context.cp_cuotas_x_doc.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCuota == info.IdCuota);
                    if (Entity != null)
                    {
                        Entity.Observacion = info.Observacion;
                        Entity.Fecha_inicio = info.Fecha_inicio;
                        Entity.Dias_plazo = info.Dias_plazo;
                        Entity.Num_cuotas = info.Num_cuotas;
                        Entity.Total_a_pagar = info.Total_a_pagar;
                        Context.SaveChanges();

                        //var lst = from q in Context.cp_cuotas_x_doc_det
                        //          where q.IdEmpresa == info.IdEmpresa
                        //          && q.IdCuota == info.IdCuota
                        //          && q.Estado == true
                        //          select q;
                        //cp_cuotas_x_doc_det_Data oData = new cp_cuotas_x_doc_det_Data();
                        //if (lst.Count() == 0)
                        //{
                        //    oData.EliminarDB(info.IdEmpresa, info.IdCuota);
                        //    foreach (var item in info.lst_cuotas_det)
                        //    {
                        //        item.IdCuota = info.IdCuota;
                        //        item.IdEmpresa = info.IdEmpresa;
                        //    }
                        //    oData.GuardarDB(info.lst_cuotas_det);
                        //}
                        //else
                        //{
                        //    foreach (var item in info.lst_cuotas_det)
                        //    {
                        //        oData.ModificarDB_campos_op(item);
                        //    }
                        //}
                    }

                }

                return true;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public Boolean anularDB(cp_cuotas_x_doc_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_cuotas_x_doc Entity = Context.cp_cuotas_x_doc.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCuota == info.IdCuota);
                    if (Entity != null)
                    {
                        Entity.Estado = false;
                        Entity.IdEmpresa_ct = null;
                        Entity.IdTipoCbte = null;
                        Entity.IdCbteCble = null;
                    }
                }

                return true;
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}
