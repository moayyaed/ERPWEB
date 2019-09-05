using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Contabilidad
{
    public class Dinardap_Data
    {
        ct_periodo_Data data_perido = new ct_periodo_Data();
        public List<DinardapData_Info> get_info(int IdEmpresa, int IdPeriodo, int IdSucursal)
        {
            try
            {
                var perido_info = data_perido.get_info(IdEmpresa, IdPeriodo);
                int IdSucursalInicio = Convert.ToInt32(IdSucursal);
                int IdSucursalFin = Convert.ToInt32(IdSucursal) == 0 ? 9999 : Convert.ToInt32(IdSucursal);
                List<DinardapData_Info> Lista = new List<DinardapData_Info>();

                using (Entities_contabilidad Context = new Entities_contabilidad())
                {

                    Lista = Context.GenerarDINARDAP(IdEmpresa, IdSucursalInicio, IdSucursalFin, perido_info.pe_FechaIni, perido_info.pe_FechaFin).Select(q => new DinardapData_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        IdBodega = q.IdBodega,
                        IdCliente = q.IdCliente,
                        Codigo = q.Codigo,
                        IdCbteVta = q.IdCbteVta,
                        CodCbteVta = q.CodCbteVta,
                        vt_fecha = q.vt_fecha,
                        vt_NumFactura = q.vt_NumFactura,
                        vt_serie1 = q.vt_serie1,
                        vt_serie2 = q.vt_serie2,
                        vt_tipoDoc = q.vt_tipoDoc,
                        Su_Descripcion = q.Su_Descripcion,
                        pe_cedulaRuc = q.pe_cedulaRuc,
                        pe_telefonoOfic = q.pe_telefonoOfic,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        Dias_Vencidos = q.Dias_Vencidos,
                        Idtipo_cliente = q.Idtipo_cliente,
                        Total_Pagado = q.Total_Pagado,
                        Valor_Original = q.Valor_Original,
                        Valor_x_Vencer = q.Valor_x_Vencer,
                        x_Vencer_1_30_Dias= q.x_Vencer_1_30_Dias,
                        x_Vencer_31_90_Dias = q.x_Vencer_31_90_Dias,
                        x_Vencer_91_180_Dias=q.x_Vencer_91_180_Dias,
                        x_Vencer_181_360_Dias = q.x_Vencer_181_360_Dias,
                        x_Vencer_Mayor_a_360Dias = q.x_Vencer_Mayor_a_360Dias,
                        Valor_Vencido = q.Valor_Vencido,
                        Vencido_1_30_Dias = q.Vencido_1_30_Dias,
                        Vencido_31_90_Dias = q.Vencido_31_90_Dias,
                        Vencido_91_180_Dias = q.Vencido_91_180_Dias,
                        Vencido_181_360_Dias = q.Vencido_181_360_Dias,
                        Vencido_Mayor_a_360Dias = q.Vencido_Mayor_a_360Dias,
                        vt_fech_venc=q.vt_fech_venc,
                        Total = q.Total,
                        Cod_Provincia = q.Cod_Provincia,
                        Cod_Ciudad= q.Cod_Ciudad,
                        cod_parroquia = q.cod_parroquia,
                        pe_Naturaleza = q.pe_Naturaleza,
                        pe_sexo = q.pe_sexo,
                        IdTipoDocumento = q.IdTipoDocumento,
                        IdEstadoCivil = q.IdEstadoCivil,
                        Plazo = q.Plazo,
                        cod_entidad_dinardap = q.cod_entidad_dinardap,
                        cr_fechaCobro = q.cr_fechaCobro,
                        Valor_cobrado= q.Valor_cobrado

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
