using Core.Erp.Info.Banco;
using Core.Erp.Info.CuentasPorPagar;
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

        public List<ba_Archivo_Transferencia_Det_Info> get_list_con_saldo(int IdEmpresa, decimal IdPersona, string IdTipo_Persona, decimal IdEntidad, string IdEstado_Aprobacion, string IdUsuario, int IdSucursal, bool mostrar_saldo_0)
        {
            try
            {
                decimal IdPersona_ini = IdPersona;
                decimal IdPersona_fin = IdPersona == 0 ? 99999 : IdPersona;

                decimal IdEntidad_ini = IdEntidad;
                decimal IdEntidad_fin = IdEntidad == 0 ? 99999 : IdEntidad;

                List<ba_Archivo_Transferencia_Det_Info> Lista;

                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    Lista = (from q in Context.spcp_Get_Data_orden_pago_con_cancelacion_data(IdEmpresa, IdPersona_ini, IdPersona_fin, IdTipo_Persona, IdEntidad_ini, IdEntidad_fin, IdEstado_Aprobacion, IdUsuario, IdSucursal, mostrar_saldo_0)
                             select new ba_Archivo_Transferencia_Det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 Secuencia = q.Secuencia_OP,
                                 IdOrdenPago = q.IdOrdenPago,
                                 Nom_Beneficiario =  q.Nom_Beneficiario,
                                 Valor = q.Saldo_x_Pagar_OP,
                                 Referencia = q.Referencia,
                                 IdTipoPersona = q.IdTipoPersona,
                                 Fecha_Factura = q.Fecha_Fa_Prov        
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
