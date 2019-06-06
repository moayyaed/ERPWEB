using Core.Erp.Info.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Banco
{
   public class ba_Archivo_Transferencia_Det_Data
    {
        public List<ba_Archivo_Transferencia_Det_Info> GetList(int IdEmpresa, decimal IdArchivo)
        {
            try
            {
                List<ba_Archivo_Transferencia_Det_Info> Lista;
                using (Entities_banco Context = new Entities_banco())
                {
                    Lista = Context.ba_Archivo_Transferencia_Det.Where(q => q.IdEmpresa == IdEmpresa && q.IdArchivo == IdArchivo).Select(q => new ba_Archivo_Transferencia_Det_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdArchivo = q.IdArchivo,
                        Contabilizado = q.Contabilizado,
                        Estado = q.Estado,
                        Fecha_proceso = q.Fecha_proceso,
                        IdEmpresa_OP = q.IdEmpresa_OP,
                        IdOrdenPago = q.IdOrdenPago,
                        Id_Item = q.Id_Item,
                        Secuencia = q.Secuencia,
                        Secuencial_reg_x_proceso = q.Secuencial_reg_x_proceso,
                        Secuencia_OP = q.Secuencia_OP,
                        Valor = q.Valor
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
