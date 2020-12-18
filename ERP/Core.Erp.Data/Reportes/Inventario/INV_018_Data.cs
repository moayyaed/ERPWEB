using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
using System.Data.SqlClient;

namespace Core.Erp.Data.Reportes.Inventario
{
  public class INV_018_Data
    {
        public List<INV_018_Info> GetList(int IdEmpresa, decimal IdAjuste)
        {
            try
            {
                List<INV_018_Info> Lista = new List<INV_018_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT        d.IdEmpresa, d.IdAjuste, d.Secuencia, c.IdSucursal, c.IdBodega, c.IdMovi_inven_tipo_ing, c.IdMovi_inven_tipo_egr, c.IdNumMovi_ing, c.IdNumMovi_egr, c.IdCatalogo_Estado, c.Estado, c.Fecha, c.Observacion, "
                                        +" s.Su_Descripcion, b.bo_Descripcion, movi_ing.tm_descripcion AS tm_descripcion_ing, movi_egr.tm_descripcion AS tm_descripcion_egr, d.IdProducto, d.IdUnidadMedida, d.StockSistema, d.StockFisico, d.Ajuste, d.Costo, "
                                        + " p.pr_descripcion, u.Descripcion AS NomUnidadMedida, ca.ca_Categoria, l.nom_linea, d.Ajuste* d.Costo AS Total, cat.Nombre AS NombreEstado, p.IdCategoria, p.IdLinea, p.IdGrupo, p.IdSubGrupo, g.nom_grupo, sg.nom_subgrupo"
                                        + " FROM            dbo.in_UnidadMedida AS u INNER JOIN"
                                        +" dbo.in_Ajuste AS c INNER JOIN"
                                        +" dbo.in_AjusteDet AS d ON c.IdEmpresa = d.IdEmpresa AND c.IdAjuste = d.IdAjuste INNER JOIN"
                                        +" dbo.tb_bodega AS b ON c.IdEmpresa = b.IdEmpresa AND c.IdEmpresa = b.IdEmpresa AND c.IdSucursal = b.IdSucursal AND c.IdSucursal = b.IdSucursal AND c.IdBodega = b.IdBodega AND c.IdBodega = b.IdBodega INNER JOIN"
                                        +" dbo.tb_sucursal AS s ON b.IdEmpresa = s.IdEmpresa AND b.IdSucursal = s.IdSucursal INNER JOIN"
                                        +" dbo.in_Producto AS p ON d.IdEmpresa = p.IdEmpresa AND d.IdProducto = p.IdProducto INNER JOIN"
                                        +" dbo.in_linea AS l INNER JOIN"
                                        +" dbo.in_categorias AS ca ON l.IdEmpresa = ca.IdEmpresa AND l.IdCategoria = ca.IdCategoria ON p.IdEmpresa = l.IdEmpresa AND p.IdCategoria = l.IdCategoria AND p.IdLinea = l.IdLinea ON"
                                        +" u.IdUnidadMedida = d.IdUnidadMedida INNER JOIN"
                                        +" dbo.in_Catalogo AS cat ON c.IdCatalogo_Estado = cat.IdCatalogo LEFT OUTER JOIN"
                                        +" dbo.in_movi_inven_tipo AS movi_ing ON c.IdEmpresa = movi_ing.IdEmpresa AND c.IdMovi_inven_tipo_ing = movi_ing.IdMovi_inven_tipo LEFT OUTER JOIN"
                                        +" dbo.in_movi_inven_tipo AS movi_egr ON c.IdEmpresa = movi_egr.IdEmpresa AND c.IdMovi_inven_tipo_egr = movi_egr.IdMovi_inven_tipo LEFT OUTER JOIN"
                                        +" dbo.in_grupo AS g ON g.IdEmpresa = p.IdEmpresa AND g.IdCategoria = p.IdCategoria AND g.IdLinea = p.IdLinea AND g.IdGrupo = p.IdGrupo LEFT JOIN"
                                        + " dbo.in_subgrupo as sg on sg.IdEmpresa = p.IdEmpresa AND sg.IdCategoria = p.IdCategoria AND sg.IdLinea = p.IdLinea AND sg.IdGrupo = p.IdGrupo AND p.IdSubGrupo = sg.IdSubGrupo"
                                        + " where c.IdEmpresa = "+IdEmpresa.ToString()+" and c.IdAjuste = "+IdAjuste.ToString();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new INV_018_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAjuste = Convert.ToDecimal(reader["IdAjuste"]),
                            Ajuste = Convert.ToDouble(reader["Ajuste"]),
                            bo_Descripcion = Convert.ToString(reader["bo_Descripcion"]),
                            ca_Categoria = Convert.ToString(reader["ca_Categoria"]),
                            Costo = Convert.ToDouble(reader["Costo"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdCatalogo_Estado = Convert.ToString(reader["IdCatalogo_Estado"]),
                            IdMovi_inven_tipo_egr = Convert.ToInt32(reader["IdMovi_inven_tipo_egr"]),
                            IdMovi_inven_tipo_ing = Convert.ToInt32(reader["IdMovi_inven_tipo_ing"]),
                            IdNumMovi_egr = reader["IdNumMovi_egr"] == DBNull.Value ? null : (decimal?)reader["IdNumMovi_egr"],
                            IdNumMovi_ing = reader["IdNumMovi_ing"] == DBNull.Value ? null : (decimal?)reader["IdNumMovi_ing"],
                            IdProducto = Convert.ToDecimal(reader["IdProducto"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdUnidadMedida = Convert.ToString(reader["IdUnidadMedida"]),
                            NomUnidadMedida = Convert.ToString(reader["NomUnidadMedida"]),
                            nom_linea = Convert.ToString(reader["nom_linea"]),
                            Observacion = Convert.ToString(reader["Observacion"]),
                            pr_descripcion = Convert.ToString(reader["pr_descripcion"]),
                            Secuencia = Convert.ToInt32(reader["Secuencia"]),
                            StockFisico = Convert.ToDouble(reader["StockFisico"]),
                            StockSistema = Convert.ToDouble(reader["StockSistema"]),
                            Su_Descripcion = Convert.ToString(reader["Su_Descripcion"]),
                            tm_descripcion_egr = Convert.ToString(reader["tm_descripcion_egr"]),
                            tm_descripcion_ing = Convert.ToString(reader["tm_descripcion_ing"]),
                            Total = Convert.ToDouble(reader["Total"]),
                            NombreEstado = Convert.ToString(reader["NombreEstado"]),
                            IdCategoria = Convert.ToString(reader["IdCategoria"]),
                            IdLinea = Convert.ToInt32(reader["IdLinea"]),
                            IdGrupo = Convert.ToInt32(reader["IdGrupo"]),
                            IdSubGrupo = Convert.ToInt32(reader["IdSubGrupo"]),
                            nom_grupo = Convert.ToString(reader["nom_grupo"]),
                            nom_subgrupo = Convert.ToString(reader["ca_Categoria"]) + " - " + Convert.ToString(reader["nom_linea"]) + " - " + Convert.ToString(reader["nom_grupo"]) + " - " + Convert.ToString(reader["nom_subgrupo"])
                        });
                    }
                    reader.Close();
                }
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
