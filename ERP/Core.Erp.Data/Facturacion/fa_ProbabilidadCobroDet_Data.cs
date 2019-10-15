using Core.Erp.Data.Facturacion.Base;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Facturacion
{
    public class fa_ProbabilidadCobroDet_Data
    {
        public List<fa_ProbabilidadCobroDet_Info> get_list(int IdEmpresa, int IdProbabilidad)
        {
            try
            {
                List<fa_ProbabilidadCobroDet_Info> Lista;
                var Secuencia = 1;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = Context.vwfa_ProbabilidadCobroDet.Where(q => q.IdEmpresa == IdEmpresa && q.IdProbabilidad == IdProbabilidad).Select(q => new fa_ProbabilidadCobroDet_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdProbabilidad = q.IdProbabilidad??0,
                        IdSucursal = q.IdSucursal,
                        IdBodega = q.IdBodega,
                        IdCbteVta = q.IdCbteVta,
                        vt_tipoDoc = q.vt_tipoDoc,
                        vt_NumFactura = q.vt_NumFactura,
                        vt_fecha = q.vt_fecha,
                        DiasVencido = q.DiasVencido ?? 0,
                        Saldo = q.Saldo ?? 0,
                        Total = q.Total,
                        vt_Observacion = q.vt_Observacion,
                        pe_nombreCompleto = q.pe_nombreCompleto
                    }).ToList();
                    Lista.ForEach(q => q.Secuencia = Secuencia++);
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<fa_ProbabilidadCobroDet_Info> get_list_x_ingresar(int IdEmpresa)
        {
            try
            {
                List<fa_ProbabilidadCobroDet_Info> Lista;
                var Secuencia = 1;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = Context.vwfa_ProbabilidadCobroDet.Where(q => q.IdEmpresa == IdEmpresa && q.IdProbabilidad == null).Select(q => new fa_ProbabilidadCobroDet_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdProbabilidad = q.IdProbabilidad??0,
                        IdSucursal = q.IdSucursal,
                        IdBodega = q.IdBodega,
                        IdCbteVta = q.IdCbteVta,
                        vt_tipoDoc = q.vt_tipoDoc,
                        vt_NumFactura = q.vt_NumFactura,
                        vt_fecha =q.vt_fecha,
                        DiasVencido = q.DiasVencido??0,
                        Saldo = q.Saldo??0,
                        Total =q.Total,
                        vt_Observacion =q.vt_Observacion,
                        pe_nombreCompleto = q.pe_nombreCompleto
                    }).ToList();
                    Lista.ForEach(q => q.IdString = q.IdEmpresa.ToString("0000") + q.IdSucursal.ToString("0000") + q.IdBodega.ToString("0000") + q.IdCbteVta.ToString("0000000000"));
                    Lista.ForEach(q => q.Secuencia = Secuencia++);
                }

                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool guardarDB(fa_ProbabilidadCobroDet_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_ProbabilidadCobroDet Entity = new fa_ProbabilidadCobroDet
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdProbabilidad = info.IdProbabilidad,
                        Secuencia = info.Secuencia,
                        IdSucursal = info.IdSucursal,
                        IdBodega = info.IdBodega,
                        IdCbteVta = info.IdCbteVta,
                        vt_tipoDoc = info.vt_tipoDoc
                    };
                    Context.fa_ProbabilidadCobroDet.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception EX)
            {

                throw;
            }
        }

        public bool eliminarDB(fa_ProbabilidadCobroDet_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var sql = "delete from fa_ProbabilidadCobroDet where IdEmpresa =" + info.IdEmpresa + " and IdProbabilidad = " + info.IdProbabilidad + " and Secuencia = " + info.Secuencia;
                    Context.Database.ExecuteSqlCommand(sql);
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
