using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_013_Data
    {
        public List<CONTA_013_Info> get_list(int IdEmpresa, int IdPunto_cargo_grupo, int IdPunto_cargo, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                int IdPunto_cargo_grupo_ini = IdPunto_cargo_grupo;
                int IdPunto_cargo_grupo_fin = (IdPunto_cargo_grupo==0 ? 999999 : IdPunto_cargo_grupo);
                List<CONTA_013_Info> Lista = new List<CONTA_013_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT        dbo.ct_cbtecble_det.IdEmpresa, dbo.ct_cbtecble_det.IdTipoCbte, dbo.ct_cbtecble_det.IdCbteCble, dbo.ct_cbtecble_det.secuencia, dbo.ct_cbtecble_det.IdCtaCble, dbo.ct_plancta.pc_Cuenta, dbo.ct_cbtecble_det.dc_Valor, "
                                        + " dbo.ct_cbtecble.cb_Observacion, dbo.ct_cbtecble_det.IdPunto_cargo_grupo, dbo.ct_cbtecble_det.IdPunto_cargo, dbo.ct_punto_cargo.nom_punto_cargo, dbo.ct_punto_cargo_grupo.nom_punto_cargo_grupo, "
                                        + " dbo.ct_cbtecble.cb_Fecha, dbo.ct_cbtecble_tipo.tc_TipoCbte, '[' + dbo.ct_cbtecble_det.IdCtaCble + '] ' + dbo.ct_plancta.pc_Cuenta AS TituloGrupo, 'TOTAL ' + dbo.ct_plancta.pc_Cuenta AS TituloTotalGrupo,"
                                        + " '[' + ISNULL(CAST(dbo.ct_cbtecble_det.IdPunto_cargo_grupo AS varchar), '') + '] ' + dbo.ct_punto_cargo_grupo.nom_punto_cargo_grupo AS nom_punto_cargo_grupoFiltro,"
                                        + " 'TOTAL ' + dbo.ct_punto_cargo_grupo.nom_punto_cargo_grupo AS TotalFinal"
                                        + " FROM dbo.ct_cbtecble_det INNER JOIN"
                                        + " dbo.ct_cbtecble ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_cbtecble.IdEmpresa AND dbo.ct_cbtecble_det.IdTipoCbte = dbo.ct_cbtecble.IdTipoCbte AND dbo.ct_cbtecble_det.IdCbteCble = dbo.ct_cbtecble.IdCbteCble INNER JOIN"
                                        + " dbo.ct_plancta ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_plancta.IdEmpresa AND dbo.ct_cbtecble_det.IdCtaCble = dbo.ct_plancta.IdCtaCble INNER JOIN"
                                        + " dbo.ct_cbtecble_tipo ON dbo.ct_cbtecble.IdEmpresa = dbo.ct_cbtecble_tipo.IdEmpresa AND dbo.ct_cbtecble.IdTipoCbte = dbo.ct_cbtecble_tipo.IdTipoCbte INNER JOIN"
                                        + " dbo.ct_punto_cargo ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_punto_cargo.IdEmpresa AND dbo.ct_cbtecble_det.IdPunto_cargo = dbo.ct_punto_cargo.IdPunto_cargo INNER JOIN"
                                        + " dbo.ct_punto_cargo_grupo ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_punto_cargo_grupo.IdEmpresa AND dbo.ct_cbtecble_det.IdPunto_cargo_grupo = dbo.ct_punto_cargo_grupo.IdPunto_cargo_grupo"
                                        + " WHERE ct_cbtecble_det.IdEmpresa = " + IdEmpresa.ToString() + " AND (dbo.ct_cbtecble_det.IdPunto_cargo_grupo IS NOT NULL) and ct_cbtecble_det.IdPunto_cargo_grupo = " + IdPunto_cargo_grupo.ToString()
                                        + " AND ct_cbtecble.cb_Fecha BETWEEN DATEFROMPARTS(" + fechaIni.Year.ToString() + "," + fechaIni.Month.ToString() + "," + fechaIni.Day.ToString() + ") AND DATEFROMPARTS(" + fechaFin.Year.ToString() + "," + fechaFin.Month.ToString() + "," + fechaFin.Day.ToString() + ")";

                    if (IdPunto_cargo > 0)
                        command.CommandText += " AND ct_cbtecble_det.IdPunto_cargo = " + IdPunto_cargo.ToString();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new CONTA_013_Info
                        {
                            cb_Fecha = Convert.ToDateTime(reader["cb_Fecha"]),
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            cb_Observacion = reader["cb_Observacion"].ToString(),
                            dc_Valor = Convert.ToDouble(reader["dc_Valor"]),
                            IdCbteCble = Convert.ToDecimal(reader["IdCbteCble"]),
                            IdCtaCble = reader["IdCtaCble"].ToString(),
                            IdPunto_cargo = reader["IdPunto_cargo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPunto_cargo"]),
                            IdPunto_cargo_grupo = Convert.ToInt32(reader["IdPunto_cargo_grupo"]),
                            IdTipoCbte = Convert.ToInt32(reader["IdTipoCbte"]),
                            nom_punto_cargo = reader["nom_punto_cargo"].ToString(),
                            nom_punto_cargo_grupo = reader["nom_punto_cargo_grupo"].ToString(),
                            nom_punto_cargo_grupoFiltro = reader["nom_punto_cargo_grupoFiltro"].ToString(),
                            pc_Cuenta = reader["pc_Cuenta"].ToString(),
                            secuencia = Convert.ToInt32(reader["secuencia"]),
                            tc_TipoCbte = reader["tc_TipoCbte"].ToString(),
                            TituloGrupo = reader["TituloGrupo"].ToString(),
                            TituloTotalGrupo = reader["TituloTotalGrupo"].ToString(),
                            TotalFinal = reader["TotalFinal"].ToString()
                        });
                    }
                    reader.Close();
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
