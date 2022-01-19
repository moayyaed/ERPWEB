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
    public class CONTA_014_Data
    {
        public List<CONTA_014_Info> GetList(int IdEmpresa, DateTime FechaDesde, DateTime FechaHasta, bool MostrarAcumulado, bool ValorOPorcentaje)
        {
            List<CONTA_014_Info> Lista = new List<CONTA_014_Info>();

            using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (ValorOPorcentaje)
                {
                    #region Valor
                    command.CommandText = "declare @IdEmpresa int = " + IdEmpresa.ToString() + ","
                                + " @FechaDesde date = datefromparts(" + FechaDesde.Year.ToString() + ", " + FechaDesde.Month.ToString() + ", " + FechaDesde.Day.ToString() + "),"
                                + " @FechaHasta date = datefromparts(" + FechaHasta.Year.ToString() + ", " + FechaHasta.Month.ToString() + ", " + FechaHasta.Day.ToString() + "),"
                                + " @MostrarAcumulado bit = " + (MostrarAcumulado ? "1" : "0")
                                + " declare @Year int = year(@FechaDesde)"
                                + " select a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then '001' else c.IdCentroCosto end IdCentroCosto, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end cc_Descripcion, "
                                + " case when b.IdCtaCble like '4%' then 'Ventas' when b.IdCtaCble like '51%' then 'Materia Prima' when b.IdCtaCble like '52%' then 'Mano de Obra Directa' when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 'Gastos Indirectos de Fabricación' END gc_GrupoCble"
                                + " ,case when b.IdCtaCble like '4%' then 4 when b.IdCtaCble like '51%' then 4.1 when b.IdCtaCble like '52%' then 4.2 when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 4.3 END gc_Orden,"
                                + " sum(dc_Valor * e.gc_signo_operacion) ValorMostrar, sum(dc_Valor) ValorReal"
                                + " from ct_cbtecble as a"
                                + " join"
                                + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                                + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like(c.IdCentroCosto + '%')  left join"
                                + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                                + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                                + " where e.gc_estado_financiero = 'ER' and"
                                + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                                + " and a.IdEmpresa = @IdEmpresa"
                                + " and year(a.cb_Fecha) = @Year"
                                + " and(b.IdCtaCble like '4%' or b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                                + " group by a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('', '004', '005','007') then '001' else c.IdCentroCosto end, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end, case when b.IdCtaCble like '4%' then 'Ventas' when b.IdCtaCble like '51%' then 'Materia Prima' when b.IdCtaCble like '52%' then 'Mano de Obra Directa' when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 'Gastos Indirectos de Fabricación' END, case when b.IdCtaCble like '4%' then 4 when b.IdCtaCble like '51%' then 4.1 when b.IdCtaCble like '52%' then 4.2 when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 4.3 END"
                                    + " UNION ALL"
                                    + " select a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then '001' else c.IdCentroCosto end IdCentroCosto, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end cc_Descripcion, 'Utilidad' gc_GrupoCble, 9 gc_Orden, sum(dc_Valor) * -1 ValorMostrar, sum(dc_Valor) ValorReal"
                                      + " from ct_cbtecble as a"
                                + " join"
                                + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                                + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like(c.IdCentroCosto + '%')  left join"
                                + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                                + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                                + " where e.gc_estado_financiero = 'ER' AND E.IdGrupoCble IN('INGRE', 'GS_OP', 'CTS_P') and"
                                + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                                + " and a.IdEmpresa = @IdEmpresa"
                                + " and year(a.cb_Fecha) = @Year"
                                + " and(b.IdCtaCble like '4%' or b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                                + " group by a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('', '004', '005','007') then '001' else c.IdCentroCosto end, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end"
                                + " UNION ALL"
                                + " select a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then '001' else c.IdCentroCosto end IdCentroCosto, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end cc_Descripcion, 'Total Costos' gc_GrupoCble, 5 gc_Orden,sum(dc_Valor) ValorMostrar, 0"
                                + " from ct_cbtecble as a"
                                + " join"
                                + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                                + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like (c.IdCentroCosto + '%')  left join"
                                + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                                + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                                + " where e.gc_estado_financiero = 'ER' AND E.IdGrupoCble IN('INGRE', 'GS_OP', 'CTS_P') and"
                                + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                                + " and a.IdEmpresa = @IdEmpresa"
                                + " and year(a.cb_Fecha) = @Year"
                                + " and(b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                                + " group by a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('', '004', '005','007') then '001' else c.IdCentroCosto end, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end";
                    #endregion
                }else
                {
                    #region Porcentaje
                    command.CommandText = "declare @IdEmpresa int = " + IdEmpresa.ToString() + ","
                               + " @FechaDesde date = datefromparts(" + FechaDesde.Year.ToString() + ", " + FechaDesde.Month.ToString() + ", " + FechaDesde.Day.ToString() + "),"
                               + " @FechaHasta date = datefromparts(" + FechaHasta.Year.ToString() + ", " + FechaHasta.Month.ToString() + ", " + FechaHasta.Day.ToString() + "),"
                               + " @MostrarAcumulado bit = " + (MostrarAcumulado ? "1" : "0")
                               + " declare @Year int = year(@FechaDesde)"
                               + " select a.IdEmpresa, case when isnull(c.IdCentroCosto,'') in ('','004','005','007') then '001' else c.IdCentroCosto end IdCentroCosto, case when isnull(c.IdCentroCosto,'') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end cc_Descripcion, '% Utilidad' gc_GrupoCble, 9.1 gc_Orden,ROUND(CASE WHEN(sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end)) != 0 THEN(sum(dc_Valor)*-1) / (sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end)*-1) ELSE 0 END, 2) ValorMostrar, 0 ValorReal"
                                + " from ct_cbtecble as a"
                                + " join"
                                + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                                + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like (c.IdCentroCosto + '%')  left join"
                                + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                                + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                                + " where e.gc_estado_financiero = 'ER' AND E.IdGrupoCble IN('INGRE', 'GS_OP', 'CTS_P') and"
                                + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                                + " and a.IdEmpresa = @IdEmpresa"
                                + " and year(a.cb_Fecha) = @Year"
                                + " and(b.IdCtaCble like '4%' or b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                                + " group by a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('', '004', '005','007') then '001' else c.IdCentroCosto end, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end"
                                + " UNION ALL"
                                + " select a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then '001' else c.IdCentroCosto end IdCentroCosto, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end cc_Descripcion, '% Costo de Venta' gc_GrupoCble, 9.3 gc_Orden,ROUND(CASE WHEN(sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end)) != 0 THEN(sum(case when b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then dc_valor else 0 end) * -1) / (sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end)) ELSE 0 END, 2) ValorMostrar, 0 ValorReal"
                                + " from ct_cbtecble as a"
                                + " join"
                                + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                                + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like (c.IdCentroCosto + '%')  left join"
                                + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                                + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                                + " where e.gc_estado_financiero = 'ER' AND E.IdGrupoCble IN('INGRE', 'GS_OP', 'CTS_P') and"
                                + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                                + " and a.IdEmpresa = @IdEmpresa"
                                + " and year(a.cb_Fecha) = @Year"
                                + " and(b.IdCtaCble like '4%' or b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                                + " group by a.IdEmpresa, case when isnull(c.IdCentroCosto, '') in ('', '004', '005','007') then '001' else c.IdCentroCosto end, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end"
                                + " UNION ALL"
                                + " select a.IdEmpresa, '010' IdCentroCost, 'Total' cc_Descripcion, '% Costo de Venta' gc_GrupoCble, 9.3 gc_Orden,"
                                + " ROUND(CASE WHEN(sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end) * -1) != 0 THEN(sum(case when b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then dc_valor else 0 end)*-1) / (sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end)) ELSE 0 END, 2) ValorMostrar, 0 ValorReal"
                                + " from ct_cbtecble as a join"
                                + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                                + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like (c.IdCentroCosto + '%')  left join"
                                + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                                + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                                + " where e.gc_estado_financiero = 'ER' AND E.IdGrupoCble IN('INGRE', 'GS_OP', 'CTS_P') and"
                                + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                                + " and a.IdEmpresa = @IdEmpresa"
                                + " and year(a.cb_Fecha) = @Year"
                                + " and(b.IdCtaCble like '4%' or b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                                + " group by a.IdEmpresa"
                                + " UNION ALL"
                                + " select a.IdEmpresa, '010' IdCentroCosto, 'Total' cc_Descripcion, '% Utilidad' gc_GrupoCble, 9.1 gc_Orden, ROUND(CASE WHEN(sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end)) != 0 THEN(sum(dc_Valor)*-1) / (sum(case when b.IdCtaCble like '4%' then dc_valor else 0 end)*-1) ELSE 0 END, 2) ValorMostrar, 0 ValorReal"
                                + " from ct_cbtecble as a"
                                + " join"
                                + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                                + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like(c.IdCentroCosto + '%')  left join"
                                + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                                + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                                + " where e.gc_estado_financiero = 'ER' AND E.IdGrupoCble IN('INGRE', 'GS_OP', 'CTS_P') and"
                                + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                                + " and a.IdEmpresa = @IdEmpresa"
                                + " and year(a.cb_Fecha) = @Year"
                                + " and(b.IdCtaCble like '4%' or b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                                + " group by a.IdEmpresa";
                    #endregion
                }


                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Lista.Add(new CONTA_014_Info
                    {
                        IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                        IdCentroCosto = reader["IdCentroCosto"].ToString(),
                        cc_Descripcion = reader["cc_Descripcion"].ToString(),
                        gc_GrupoCble = reader["gc_GrupoCble"].ToString(),
                        gc_Orden = reader["gc_Orden"] == DBNull.Value ? 0 : Convert.ToDouble(reader["gc_Orden"]),
                        ValorMostrar = reader["ValorMostrar"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValorMostrar"]),
                        ValorReal = reader["ValorReal"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValorReal"]),
                    });
                }
                reader.Close();
            }

            return Lista;
        }

        public List<CONTA_014_Detalle_Info> GetList_Detalle(int IdEmpresa, DateTime FechaDesde, DateTime FechaHasta, bool MostrarAcumulado)
        {
            List<CONTA_014_Detalle_Info> Lista = new List<CONTA_014_Detalle_Info>();

            using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                #region Query
                command.CommandText = "declare @IdEmpresa int = " + IdEmpresa.ToString() + ","
                            + " @FechaDesde date = datefromparts(" + FechaDesde.Year.ToString() + ", " + FechaDesde.Month.ToString() + ", " + FechaDesde.Day.ToString() + "),"
                            + " @FechaHasta date = datefromparts(" + FechaHasta.Year.ToString() + ", " + FechaHasta.Month.ToString() + ", " + FechaHasta.Day.ToString() + "),"
                            + " @MostrarAcumulado bit = " + (MostrarAcumulado ? "1" : "0")
                            + " declare @Year int = year(@FechaDesde)"
                            + " select a.IdEmpresa, b.IdCtaCble, d.pc_Cuenta, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then '001' else c.IdCentroCosto end IdCentroCosto, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end cc_Descripcion,"
                            + " case when b.IdCtaCble like '4%' then 'Ventas' when b.IdCtaCble like '51%' then 'Materia Prima' when b.IdCtaCble like '52%' then 'Mano de Obra Directa' when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 'Gastos Indirectos de Fabricación' END gc_GrupoCble"
                            + " ,case when b.IdCtaCble like '4%' then 4 when b.IdCtaCble like '51%' then 4.1 when b.IdCtaCble like '52%' then 4.2 when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 4.3 END gc_Orden,"
                            + " sum(dc_Valor * e.gc_signo_operacion) ValorMostrar, sum(dc_Valor) ValorReal,"
                            + " case when b.IdCtaCble like '4%' then 'Ventas' else 'Costos' END GrupoPrincipal"
                            + " from ct_cbtecble as a"
                            + " join"
                            + " ct_cbtecble_det as b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble left join"
                            + " ct_CentroCosto as c on c.IdNivel = 1 and c.IdEmpresa = b.IdEmpresa and b.IdCentroCosto like(c.IdCentroCosto + '%')  left join"
                            + " ct_plancta as d on b.IdEmpresa = d.IdEmpresa and b.IdCtaCble = d.IdCtaCble left join"
                            + " ct_grupocble as e on d.IdGrupoCble = e.IdGrupoCble"
                            + " where e.gc_estado_financiero = 'ER' and"
                            + " a.cb_Fecha between @FechaDesde and @FechaHasta"
                            + " and a.IdEmpresa = @IdEmpresa"
                            + " and year(a.cb_Fecha) = @Year"
                            + " and(b.IdCtaCble like '4%' or b.IdCtaCble like '51%' or b.IdCtaCble like '52%' or b.IdCtaCble like '53%' or b.IdCtaCble like '540%')"
                            + " group by a.IdEmpresa, b.IdCtaCble, d.pc_Cuenta, case when isnull(c.IdCentroCosto, '') in ('', '004', '005', '007') then '001' else c.IdCentroCosto end, case when isnull(c.IdCentroCosto, '') in ('','004','005','007') then 'Offset' else c.cc_Descripcion end, case when b.IdCtaCble like '4%' then 'Ventas' when b.IdCtaCble like '51%' then 'Materia Prima' when b.IdCtaCble like '52%' then 'Mano de Obra Directa' when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 'Gastos Indirectos de Fabricación' END, case when b.IdCtaCble like '4%' then 4 when b.IdCtaCble like '51%' then 4.1 when b.IdCtaCble like '52%' then 4.2 when b.IdCtaCble like '53%' or b.IdCtaCble like '540%' then 4.3 END,"
                            + " case when b.IdCtaCble like '4%' then 'Ventas' else 'Costos' END ";
                #endregion

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Lista.Add(new CONTA_014_Detalle_Info
                    {
                        IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                        IdCtaCble = reader["IdCtaCble"].ToString(),
                        pc_Cuenta = reader["pc_Cuenta"].ToString(),
                        IdCentroCosto = reader["IdCentroCosto"].ToString(),
                        cc_Descripcion = reader["cc_Descripcion"].ToString(),
                        gc_GrupoCble = reader["gc_GrupoCble"].ToString(),
                        gc_Orden = reader["gc_Orden"] == DBNull.Value ? 0 : Convert.ToDouble(reader["gc_Orden"]),
                        ValorMostrar = reader["ValorMostrar"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValorMostrar"]),
                        ValorReal = reader["ValorReal"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValorReal"]),
                        GrupoPrincipal = reader["GrupoPrincipal"].ToString(),
                    });
                }
                reader.Close();
            }

            return Lista;
        }
    }
}
