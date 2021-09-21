using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_015_Data
    {
        public List<CONTA_015_Info> GetList(int IdEmpresa, DateTime FechaIni, DateTime FechaFin)
        {
            List<CONTA_015_Info> Lista = new List<CONTA_015_Info>();

            using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                #region Query
                command.CommandText = "declare @IdEmpresa int = "+IdEmpresa+","
                + " @FechaDesde date = DATEFROMPARTS(" + FechaIni.Year.ToString() + ", " + FechaIni.Month.ToString() + ", " + FechaIni.Day.ToString() + "),"
                + " @FechaHasta date = DATEFROMPARTS(" + FechaFin.Year.ToString() + ", " + FechaFin.Month.ToString() + ", " + FechaFin.Day.ToString() + ")"
                + " select x.IdEmpresa, x.em_ruc, x.em_nombre, right('00' + cast(Month(@FechaDesde) as varchar), 2) as Mes, year(@FechaDesde) Anio,"
                + " sum(case when c.codigoSRI = '303' then b.re_baseRetencion else 0 end) c303,"
                + " sum(case when c.codigoSRI = '303' then b.re_valor_retencion else 0 end) c353,"
                + " sum(case when c.codigoSRI = '304' then b.re_baseRetencion else 0 end) c304,"
                + " sum(case when c.codigoSRI = '304' then b.re_valor_retencion else 0 end) c354,"
                + " sum(case when c.codigoSRI = '307' then b.re_baseRetencion else 0 end) c307,"
                + " sum(case when c.codigoSRI = '307' then b.re_valor_retencion else 0 end) c357,"
                + " sum(case when c.codigoSRI = '308' then b.re_baseRetencion else 0 end) c308,"
                + " sum(case when c.codigoSRI = '308' then b.re_valor_retencion else 0 end) c358,"
                + " sum(case when c.codigoSRI = '309' then b.re_baseRetencion else 0 end) c309,"
                + " sum(case when c.codigoSRI = '309' then b.re_valor_retencion else 0 end) c359,"
                + " sum(case when c.codigoSRI = '310' then b.re_baseRetencion else 0 end) c310,"
                + " sum(case when c.codigoSRI = '310' then b.re_valor_retencion else 0 end) c360,"
                + " sum(case when c.codigoSRI = '311' then b.re_baseRetencion else 0 end) c311,"
                + " sum(case when c.codigoSRI = '311' then b.re_valor_retencion else 0 end) c361,"
                + " sum(case when c.codigoSRI = '312' then b.re_baseRetencion else 0 end) c312,"
                + " sum(case when c.codigoSRI = '312' then b.re_valor_retencion else 0 end) c362,"
                + " sum(case when c.codigoSRI = '312A' then b.re_baseRetencion else 0 end) c3120,"
                + " sum(case when c.codigoSRI = '312A' then b.re_valor_retencion else 0 end) c3620,"
                + " sum(case when c.codigoSRI in ('314A', '314B', '314C', '314D') then b.re_baseRetencion else 0 end) c314,"
                + " sum(case when c.codigoSRI in ('314A', '314B', '314C', '314D') then b.re_valor_retencion else 0 end) c364,"
                + " sum(case when c.codigoSRI = '319' then b.re_baseRetencion else 0 end) c319,"
                + " sum(case when c.codigoSRI = '319' then b.re_valor_retencion else 0 end) c369,"
                + " sum(case when c.codigoSRI = '320' then b.re_baseRetencion else 0 end) c320,"
                + " sum(case when c.codigoSRI = '320' then b.re_valor_retencion else 0 end) c370,"
                + " sum(case when c.codigoSRI = '322' then b.re_baseRetencion else 0 end) c322,"
                + " sum(case when c.codigoSRI = '322' then b.re_valor_retencion else 0 end) c372,"
                + " sum(case when c.codigoSRI LIKE '323%' then b.re_baseRetencion else 0 end) c323,"
                + " sum(case when c.codigoSRI LIKE '323%' then b.re_valor_retencion else 0 end) c373,"
                + " sum(case when c.codigoSRI LIKE '324%' then b.re_baseRetencion else 0 end) c324,"
                + " sum(case when c.codigoSRI LIKE '324%' then b.re_valor_retencion else 0 end) c374,"
                + " sum(case when c.codigoSRI LIKE '325%' then b.re_baseRetencion else 0 end) c325,"
                + " sum(case when c.codigoSRI LIKE '325%' then b.re_valor_retencion else 0 end) c375,"
                + " sum(case when c.codigoSRI LIKE '326%' then b.re_baseRetencion else 0 end) c326,"
                + " sum(case when c.codigoSRI LIKE '326%' then b.re_valor_retencion else 0 end) c376,"
                + " sum(case when c.codigoSRI LIKE '327%' then b.re_baseRetencion else 0 end) c327,"
                + " sum(case when c.codigoSRI LIKE '327%' then b.re_valor_retencion else 0 end) c377,"
                + " sum(case when c.codigoSRI LIKE '328%' then b.re_baseRetencion else 0 end) c328,"
                + " sum(case when c.codigoSRI LIKE '328%' then b.re_valor_retencion else 0 end) c378,"
                + " sum(case when c.codigoSRI LIKE '329%' then b.re_baseRetencion else 0 end) c329,"
                + " sum(case when c.codigoSRI LIKE '329%' then b.re_valor_retencion else 0 end) c379,"
                + " sum(case when c.codigoSRI LIKE '330%' then b.re_baseRetencion else 0 end) c330,"
                + " sum(case when c.codigoSRI LIKE '330%' then b.re_valor_retencion else 0 end) c380,"
                + " sum(case when c.codigoSRI LIKE '331%' then b.re_baseRetencion else 0 end) c331,"
                + " sum(case when c.codigoSRI LIKE '332%' then b.re_baseRetencion else 0 end) c332,"
                + " sum(case when c.codigoSRI LIKE '333%' then b.re_baseRetencion else 0 end) c333,"
                + " sum(case when c.codigoSRI LIKE '333%' then b.re_valor_retencion else 0 end) c383,"
                + " sum(case when c.codigoSRI LIKE '334%' then b.re_baseRetencion else 0 end) c334,"
                + " sum(case when c.codigoSRI LIKE '334%' then b.re_valor_retencion else 0 end) c384,"
                + " sum(case when c.codigoSRI LIKE '335%' then b.re_baseRetencion else 0 end) c335,"
                + " sum(case when c.codigoSRI LIKE '335%' then b.re_valor_retencion else 0 end) c385,"
                + " sum(case when c.codigoSRI LIKE '336%' then b.re_baseRetencion else 0 end) c336,"
                + " sum(case when c.codigoSRI LIKE '336%' then b.re_valor_retencion else 0 end) c386,"
                + " sum(case when c.codigoSRI LIKE '337%' then b.re_baseRetencion else 0 end) c337,"
                + " sum(case when c.codigoSRI LIKE '337%' then b.re_valor_retencion else 0 end) c387,"
                + " sum(case when c.codigoSRI LIKE '338%' then b.re_baseRetencion else 0 end) c338,"
                + " sum(case when c.codigoSRI LIKE '338%' then b.re_valor_retencion else 0 end) c388,"
                + " sum(case when c.codigoSRI LIKE '339%' then b.re_baseRetencion else 0 end) c339,"
                + " sum(case when c.codigoSRI LIKE '339%' then b.re_valor_retencion else 0 end) c389,"
                + " sum(case when c.codigoSRI LIKE '340%' then b.re_baseRetencion else 0 end) c340,"
                + " sum(case when c.codigoSRI LIKE '340%' then b.re_valor_retencion else 0 end) c390,"
                + " sum(case when c.codigoSRI LIKE '341%' then b.re_baseRetencion else 0 end) c341,"
                + " sum(case when c.codigoSRI LIKE '341%' then b.re_valor_retencion else 0 end) c391,"
                + " sum(case when c.codigoSRI LIKE '342%' then b.re_baseRetencion else 0 end) c342,"
                + " sum(case when c.codigoSRI LIKE '342%' then b.re_valor_retencion else 0 end) c392,"
                + " sum(case when c.codigoSRI LIKE '343%' then b.re_baseRetencion else 0 end) c343,"
                + " sum(case when c.codigoSRI LIKE '343%' then b.re_valor_retencion else 0 end) c393,"
                + " sum(case when c.codigoSRI LIKE '344%' then b.re_baseRetencion else 0 end) c344,"
                + " sum(case when c.codigoSRI LIKE '344%' then b.re_valor_retencion else 0 end) c394,"
                + " sum(case when c.codigoSRI LIKE '345%' then b.re_baseRetencion else 0 end) c345,"
                + " sum(case when c.codigoSRI LIKE '345%' then b.re_valor_retencion else 0 end) c395,"
                + " sum(case when c.codigoSRI LIKE '346%' then b.re_baseRetencion else 0 end) c346,"
                + " sum(case when c.codigoSRI LIKE '346%' then b.re_valor_retencion else 0 end) c396,"
                + " isnull(sum(e.BaseImponibleExt), 0) c423"
                 + " from tb_empresa as x with(nolock) left join"
                + " cp_retencion as a with(nolock)on a.IdEmpresa = x.IdEmpresa and a.fecha between @FechaDesde and @FechaHasta and a.Estado = 'A' LEFT join"
                + " cp_retencion_det as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdRetencion = b.IdRetencion LEFT join"
                + " cp_codigo_SRI as c with(nolock) on b.IdCodigo_SRI = c.IdCodigo_SRI left join"
                + " cp_orden_giro as d with(nolock) on a.IdEmpresa_Ogiro = d.IdEmpresa and a.IdTipoCbte_Ogiro = d.IdTipoCbte_Ogiro and a.IdCbteCble_Ogiro = d.IdCbteCble_Ogiro left join"
                + " ("
                   + " select A.IdEmpresa, SUM(A.co_baseImponible) BaseImponibleExt from cp_orden_giro as a  left join"
                   + " cp_retencion as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte_Ogiro = b.IdTipoCbte_Ogiro and a.IdCbteCble_Ogiro = b.IdCbteCble_Ogiro"
                   + " where a.IdEmpresa = @IdEmpresa and a.PagoLocExt = 'EXT' AND A.co_FechaFactura BETWEEN @FechaDesde AND @FechaHasta and b.IdRetencion is null"
                   + " GROUP BY A.IdEmpresa"
                + " ) as e on x.IdEmpresa = e.IdEmpresa"
                + " where x.IdEmpresa = @IdEmpresa"
                + " group by x.IdEmpresa, x.em_ruc, x.em_nombre";
                #endregion

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Lista.Add(new CONTA_015_Info
                    {
                        IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                        em_ruc = string.IsNullOrEmpty(reader["em_ruc"].ToString()) ? null: reader["em_ruc"].ToString(),
                        em_nombre = string.IsNullOrEmpty(reader["em_nombre"].ToString()) ? null : reader["em_nombre"].ToString(),
                        Mes = string.IsNullOrEmpty(reader["Mes"].ToString()) ? null : reader["Mes"].ToString(),
                        Anio = string.IsNullOrEmpty(reader["Anio"].ToString()) ? (int?)null : Convert.ToInt32(reader["Anio"]),
                        c303 = string.IsNullOrEmpty(reader["c303"].ToString()) ? (double?)null : Convert.ToInt32(reader["c303"]),
                        c353 = string.IsNullOrEmpty(reader["c353"].ToString()) ? (double?)null : Convert.ToDouble(reader["c353"]),
                        c304 = string.IsNullOrEmpty(reader["c304"].ToString()) ? (double?)null : Convert.ToDouble(reader["c304"]),
                        c354 = string.IsNullOrEmpty(reader["c354"].ToString()) ? (double?)null : Convert.ToDouble(reader["c354"]),
                        c307 = string.IsNullOrEmpty(reader["c307"].ToString()) ? (double?)null : Convert.ToDouble(reader["c307"]),
                        c357 = string.IsNullOrEmpty(reader["c357"].ToString()) ? (double?)null : Convert.ToDouble(reader["c357"]),
                        c308 = string.IsNullOrEmpty(reader["c308"].ToString()) ? (double?)null : Convert.ToInt32(reader["c308"]),
                        c358 = string.IsNullOrEmpty(reader["c358"].ToString()) ? (double?)null : Convert.ToDouble(reader["c358"]),
                        c309 = string.IsNullOrEmpty(reader["c309"].ToString()) ? (double?)null : Convert.ToDouble(reader["c309"]),
                        c359 = string.IsNullOrEmpty(reader["c359"].ToString()) ? (double?)null : Convert.ToDouble(reader["c359"]),
                        c310 = string.IsNullOrEmpty(reader["c310"].ToString()) ? (double?)null : Convert.ToDouble(reader["c310"]),
                        c360 = string.IsNullOrEmpty(reader["c360"].ToString()) ? (double?)null : Convert.ToDouble(reader["c360"]),
                        c311 = string.IsNullOrEmpty(reader["c311"].ToString()) ? (double?)null : Convert.ToInt32(reader["c311"]),
                        c361 = string.IsNullOrEmpty(reader["c361"].ToString()) ? (double?)null : Convert.ToDouble(reader["c361"]),
                        c312 = string.IsNullOrEmpty(reader["c312"].ToString()) ? (double?)null : Convert.ToDouble(reader["c312"]),
                        c362 = string.IsNullOrEmpty(reader["c362"].ToString()) ? (double?)null : Convert.ToDouble(reader["c362"]),
                        c3120 = string.IsNullOrEmpty(reader["c3120"].ToString()) ? (double?)null : Convert.ToDouble(reader["c3120"]),
                        c3620 = string.IsNullOrEmpty(reader["c3620"].ToString()) ? (double?)null : Convert.ToDouble(reader["c3620"]),
                        c314 = string.IsNullOrEmpty(reader["c314"].ToString()) ? (double?)null : Convert.ToInt32(reader["c314"]),
                        c364 = string.IsNullOrEmpty(reader["c364"].ToString()) ? (double?)null : Convert.ToDouble(reader["c364"]),
                        c319 = string.IsNullOrEmpty(reader["c319"].ToString()) ? (double?)null : Convert.ToDouble(reader["c319"]),
                        c369 = string.IsNullOrEmpty(reader["c369"].ToString()) ? (double?)null : Convert.ToDouble(reader["c369"]),
                        c320 = string.IsNullOrEmpty(reader["c320"].ToString()) ? (double?)null : Convert.ToDouble(reader["c320"]),
                        c370 = string.IsNullOrEmpty(reader["c370"].ToString()) ? (double?)null : Convert.ToDouble(reader["c370"]),
                        c322 = string.IsNullOrEmpty(reader["c322"].ToString()) ? (double?)null : Convert.ToInt32(reader["c322"]),
                        c372 = string.IsNullOrEmpty(reader["c372"].ToString()) ? (double?)null : Convert.ToDouble(reader["c372"]),
                        c323 = string.IsNullOrEmpty(reader["c323"].ToString()) ? (double?)null : Convert.ToDouble(reader["c323"]),
                        c373 = string.IsNullOrEmpty(reader["c373"].ToString()) ? (double?)null : Convert.ToDouble(reader["c373"]),
                        c324 = string.IsNullOrEmpty(reader["c324"].ToString()) ? (double?)null : Convert.ToDouble(reader["c324"]),
                        c374 = string.IsNullOrEmpty(reader["c374"].ToString()) ? (double?)null : Convert.ToDouble(reader["c374"]),
                        c325 = string.IsNullOrEmpty(reader["c325"].ToString()) ? (double?)null : Convert.ToDouble(reader["c325"]),
                        c375 = string.IsNullOrEmpty(reader["c375"].ToString()) ? (double?)null : Convert.ToDouble(reader["c375"]),
                        c326 = string.IsNullOrEmpty(reader["c326"].ToString()) ? (double?)null : Convert.ToDouble(reader["c326"]),
                        c376 = string.IsNullOrEmpty(reader["c376"].ToString()) ? (double?)null : Convert.ToDouble(reader["c376"]),
                        c327 = string.IsNullOrEmpty(reader["c327"].ToString()) ? (double?)null : Convert.ToDouble(reader["c327"]),
                        c377 = string.IsNullOrEmpty(reader["c377"].ToString()) ? (double?)null : Convert.ToDouble(reader["c377"]),
                        c328 = string.IsNullOrEmpty(reader["c328"].ToString()) ? (double?)null : Convert.ToDouble(reader["c328"]),
                        c378 = string.IsNullOrEmpty(reader["c378"].ToString()) ? (double?)null : Convert.ToDouble(reader["c378"]),
                        c329 = string.IsNullOrEmpty(reader["c329"].ToString()) ? (double?)null : Convert.ToDouble(reader["c329"]),
                        c379 = string.IsNullOrEmpty(reader["c379"].ToString()) ? (double?)null : Convert.ToDouble(reader["c379"]),
                        c330 = string.IsNullOrEmpty(reader["c330"].ToString()) ? (double?)null : Convert.ToDouble(reader["c330"]),
                        c380 = string.IsNullOrEmpty(reader["c380"].ToString()) ? (double?)null : Convert.ToDouble(reader["c380"]),
                        c331 = string.IsNullOrEmpty(reader["c331"].ToString()) ? (double?)null : Convert.ToDouble(reader["c331"]),
                        c332 = string.IsNullOrEmpty(reader["c332"].ToString()) ? (double?)null : Convert.ToDouble(reader["c332"]),
                        c333 = string.IsNullOrEmpty(reader["c333"].ToString()) ? (double?)null : Convert.ToDouble(reader["c333"]),
                        c383 = string.IsNullOrEmpty(reader["c383"].ToString()) ? (double?)null : Convert.ToDouble(reader["c383"]),
                        c334 = string.IsNullOrEmpty(reader["c334"].ToString()) ? (double?)null : Convert.ToDouble(reader["c334"]),
                        c384 = string.IsNullOrEmpty(reader["c384"].ToString()) ? (double?)null : Convert.ToDouble(reader["c384"]),
                        c335 = string.IsNullOrEmpty(reader["c335"].ToString()) ? (double?)null : Convert.ToDouble(reader["c335"]),
                        c385 = string.IsNullOrEmpty(reader["c385"].ToString()) ? (double?)null : Convert.ToDouble(reader["c385"]),
                        c336 = string.IsNullOrEmpty(reader["c336"].ToString()) ? (double?)null : Convert.ToDouble(reader["c336"]),
                        c386 = string.IsNullOrEmpty(reader["c386"].ToString()) ? (double?)null : Convert.ToDouble(reader["c386"]),
                        c337 = string.IsNullOrEmpty(reader["c337"].ToString()) ? (double?)null : Convert.ToDouble(reader["c337"]),
                        c387 = string.IsNullOrEmpty(reader["c387"].ToString()) ? (double?)null : Convert.ToDouble(reader["c387"]),
                        c338 = string.IsNullOrEmpty(reader["c338"].ToString()) ? (double?)null : Convert.ToDouble(reader["c338"]),
                        c388 = string.IsNullOrEmpty(reader["c388"].ToString()) ? (double?)null : Convert.ToDouble(reader["c388"]),
                        c339 = string.IsNullOrEmpty(reader["c339"].ToString()) ? (double?)null : Convert.ToDouble(reader["c339"]),
                        c389 = string.IsNullOrEmpty(reader["c389"].ToString()) ? (double?)null : Convert.ToDouble(reader["c389"]),
                        c340 = string.IsNullOrEmpty(reader["c340"].ToString()) ? (double?)null : Convert.ToDouble(reader["c340"]),
                        c390 = string.IsNullOrEmpty(reader["c390"].ToString()) ? (double?)null : Convert.ToDouble(reader["c390"]),
                        c341 = string.IsNullOrEmpty(reader["c341"].ToString()) ? (double?)null : Convert.ToDouble(reader["c341"]),
                        c391 = string.IsNullOrEmpty(reader["c391"].ToString()) ? (double?)null : Convert.ToDouble(reader["c391"]),
                        c342 = string.IsNullOrEmpty(reader["c342"].ToString()) ? (double?)null : Convert.ToDouble(reader["c342"]),
                        c392 = string.IsNullOrEmpty(reader["c392"].ToString()) ? (double?)null : Convert.ToDouble(reader["c392"]),
                        c343 = string.IsNullOrEmpty(reader["c343"].ToString()) ? (double?)null : Convert.ToDouble(reader["c343"]),
                        c393 = string.IsNullOrEmpty(reader["c393"].ToString()) ? (double?)null : Convert.ToDouble(reader["c393"]),
                        c344 = string.IsNullOrEmpty(reader["c344"].ToString()) ? (double?)null : Convert.ToDouble(reader["c344"]),
                        c394 = string.IsNullOrEmpty(reader["c394"].ToString()) ? (double?)null : Convert.ToDouble(reader["c394"]),
                        c345 = string.IsNullOrEmpty(reader["c345"].ToString()) ? (double?)null : Convert.ToDouble(reader["c345"]),
                        c395 = string.IsNullOrEmpty(reader["c395"].ToString()) ? (double?)null : Convert.ToDouble(reader["c395"]),
                        c346 = string.IsNullOrEmpty(reader["c346"].ToString()) ? (double?)null : Convert.ToDouble(reader["c346"]),
                        c396 = string.IsNullOrEmpty(reader["c396"].ToString()) ? (double?)null : Convert.ToDouble(reader["c396"]),
                        c423 = Convert.ToDouble(reader["c423"])
                    });
                }
                reader.Close();
            }

            return Lista;
        }
    }
}
