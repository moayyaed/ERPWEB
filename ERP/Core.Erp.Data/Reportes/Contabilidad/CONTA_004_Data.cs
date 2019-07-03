using Core.Erp.Data.General;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
   public class CONTA_004_Data
    {

        tb_sucursal_Data data_sucursal = new tb_sucursal_Data();
        public List<CONTA_004_Info> get_list(int IdEmpresa, int IdAnio, DateTime fechaIni1, DateTime fechaFin1, string IdUsuario, int IdNivel, bool mostrarSaldo0, string balance, bool MostrarSaldoAcumulado, DateTime fechaIni2, DateTime fechaFin2)
        {
            try
            {
                List<CONTA_004_Info> Lista;
                List<CONTA_003_balances_Info> Lista1;

                Entities_contabilidad db = new Entities_contabilidad();
                Lista = db.ct_plancta.Where(q => q.IdEmpresa == IdEmpresa && q.pc_Estado == "A").Select(q => new CONTA_004_Info
                {
                    IdEmpresa = q.IdEmpresa,
                    IdCtaCble = q.IdCtaCble,
                    IdCtaCblePadre = q.IdCtaCblePadre,
                    pc_Cuenta = q.pc_Cuenta
                }).ToList();

                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista1 = (from q in Context.SPCONTA_003_balances(IdEmpresa, IdAnio, fechaIni1, fechaFin1, IdUsuario, IdNivel, mostrarSaldo0, balance,MostrarSaldoAcumulado)
                             select new CONTA_003_balances_Info
                             {
                                 IdUsuario = q.IdUsuario,
                                 IdEmpresa = q.IdEmpresa,
                                 IdCtaCble = q.IdCtaCble,
                                 pc_Cuenta = q.pc_Cuenta,
                                 IdCtaCblePadre = q.IdCtaCblePadre,
                                 EsCtaUtilidad = q.EsCtaUtilidad,
                                 IdNivelCta = q.IdNivelCta,
                                 IdGrupoCble = q.IdGrupoCble,
                                 gc_estado_financiero = q.gc_estado_financiero,
                                 gc_GrupoCble = q.gc_GrupoCble,
                                 gc_Orden = q.gc_Orden,
                                 Debitos = q.Debitos,
                                 DebitosSaldoInicial = q.DebitosSaldoInicial,
                                 SaldoDebitos = q.SaldoDebitos,
                                 SaldoDebitosCreditos = q.SaldoDebitosCreditos,
                                 Creditos = q.Creditos,
                                 CreditosSaldoInicial = q.CreditosSaldoInicial,
                                 SaldoCreditos = q.SaldoCreditos,
                                 SaldoFinal = q.SaldoFinal,
                                 SaldoInicial = q.SaldoInicial,
                                 EsCuentaMovimiento = q.EsCuentaMovimiento,
                                 Naturaleza = q.Naturaleza,
                                 SaldoCreditosNaturaleza = q.SaldoCreditosNaturaleza,
                                 SaldoDebitosCreditosNaturaleza = q.SaldoDebitosCreditosNaturaleza,
                                 SaldoDebitosNaturaleza = q.SaldoDebitosNaturaleza,
                                 SaldoFinalNaturaleza = q.SaldoFinalNaturaleza,
                                 SaldoInicialNaturaleza = q.SaldoInicialNaturaleza                                 
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
