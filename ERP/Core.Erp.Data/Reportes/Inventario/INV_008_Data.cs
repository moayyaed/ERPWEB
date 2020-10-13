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
    public class INV_008_Data
    {
        public List<INV_008_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, DateTime fecha_ini, DateTime fecha_fin, string IdCentroCosto, string signo, int IdMovi_inven_tipo, int IdProductoTipo, string IdCategoria, int IdLinea, int IdGrupo, int IdSubGrupo)
        {
            try
            {
                List<INV_008_Info> Lista = new List<INV_008_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    string query = "SELECT        d.IdEmpresa, d.IdSucursal, d.IdMovi_inven_tipo, d.IdNumMovi, d.Secuencia, s.Su_Descripcion, b.bo_Descripcion, m.cm_tipo_movi, m.tm_descripcion, mot.Desc_mov_inv, per.pe_nombreCompleto, p.pr_codigo, "
                                    + " p.pr_descripcion, c.IdEstadoAproba, d.dm_cantidad, d.mv_costo, d.IdOrdenCompra, d.IdCentroCosto, d.IdMotivo_Inv, c.cm_fecha, c.Estado, d.IdBodega, d.IdProducto, cc.cc_Descripcion, pt.tp_descripcion"
                                    + " FROM            dbo.in_Producto AS p INNER JOIN"
                                    + " dbo.in_Ing_Egr_Inven_det AS d INNER JOIN"
                                    + " dbo.in_Ing_Egr_Inven AS c ON d.IdEmpresa = c.IdEmpresa AND d.IdSucursal = c.IdSucursal AND d.IdMovi_inven_tipo = c.IdMovi_inven_tipo AND d.IdNumMovi = c.IdNumMovi INNER JOIN"
                                    + " dbo.in_movi_inven_tipo AS m ON d.IdEmpresa = m.IdEmpresa AND d.IdMovi_inven_tipo = m.IdMovi_inven_tipo ON p.IdEmpresa = d.IdEmpresa AND p.IdProducto = d.IdProducto INNER JOIN"
                                    + " dbo.tb_bodega AS b ON d.IdEmpresa = b.IdEmpresa AND d.IdSucursal = b.IdSucursal AND d.IdBodega = b.IdBodega INNER JOIN"
                                    + " dbo.tb_sucursal AS s ON b.IdEmpresa = s.IdEmpresa AND b.IdSucursal = s.IdSucursal LEFT OUTER JOIN"
                                    + " dbo.in_Motivo_Inven AS mot ON d.IdMotivo_Inv = mot.IdMotivo_Inv AND d.IdEmpresa = mot.IdEmpresa LEFT OUTER JOIN"
                                    + " dbo.tb_persona AS per INNER JOIN"
                                    + " dbo.cp_proveedor AS pro ON per.IdPersona = pro.IdPersona ON c.IdEmpresa = pro.IdEmpresa AND c.IdResponsable = pro.IdProveedor LEFT OUTER JOIN"
                                    + " dbo.ct_CentroCosto AS cc ON d.IdEmpresa = cc.IdEmpresa AND d.IdCentroCosto = cc.IdCentroCosto LEFT JOIN"
                                    + " in_ProductoTipo as pt on p.IdEmpresa = pt.IdEmpresa and p.IdProductoTipo = pt.IdProductoTipo"
                                    + " WHERE(c.Estado = 'A') and c.cm_fecha between DATEFROMPARTS("+fecha_ini.Year.ToString()+","+fecha_ini.Month.ToString()+","+fecha_ini.Day.ToString()+ ") AND DATEFROMPARTS(" + fecha_fin.Year.ToString() + "," + fecha_fin.Month.ToString() + "," + fecha_fin.Day.ToString() + ")";

                    if (IdProducto != 0)
                        query += " AND d.IdProducto = "+IdProducto.ToString();

                    if (IdSucursal != 0)
                        query += " AND d.IdSucursal = "+IdSucursal.ToString();

                    if (IdBodega != 0)
                        query += " AND d.IdBodega = "+IdBodega.ToString();

                    if (IdMovi_inven_tipo != 0)
                        query += " AND d.IdMovi_inven_tipo = "+IdMovi_inven_tipo.ToString();

                    if (IdProductoTipo != 0)
                        query += " AND p.IdProductoTipo = "+IdProductoTipo.ToString();

                    if (!string.IsNullOrEmpty(IdCentroCosto))
                        query += " AND d.IdCentroCosto = '" + IdCentroCosto+"'";

                    if (!string.IsNullOrEmpty(signo))
                        query += " AND m.cm_tipo_movi = '"+signo+"'";

                    if (!string.IsNullOrEmpty(IdCategoria))
                        query += " AND p.IdCategoria = '" + IdCategoria + "'";

                    if (IdLinea != 0)
                        query += " AND p.IdLinea = " + IdLinea.ToString();

                    if (IdGrupo != 0)
                        query += " AND p.IdGrupo= " + IdGrupo.ToString();

                    if (IdSubGrupo != 0)
                        query += " AND p.IdSubGrupo= " + IdSubGrupo.ToString();

                    SqlCommand command = new SqlCommand(query,connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new INV_008_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            bo_Descripcion = Convert.ToString(reader["bo_Descripcion"]),
                            cm_fecha = Convert.ToDateTime(reader["cm_fecha"]),
                            cm_tipo_movi = Convert.ToString(reader["cm_tipo_movi"]),
                            Desc_mov_inv = Convert.ToString(reader["Desc_mov_inv"]),
                            dm_cantidad = Convert.ToDouble(reader["dm_cantidad"]),
                            Estado = Convert.ToString(reader["Estado"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdCentroCosto = Convert.ToString(reader["IdCentroCosto"]),
                            IdEstadoAproba = Convert.ToString(reader["IdEstadoAproba"]),
                            IdMotivo_Inv = string.IsNullOrEmpty(reader["IdMotivo_Inv"].ToString()) ? null : (int?)(reader["IdMotivo_Inv"]),
                            IdMovi_inven_tipo = Convert.ToInt32(reader["IdMovi_inven_tipo"]),
                            IdNumMovi = Convert.ToDecimal(reader["IdNumMovi"]),
                            IdOrdenCompra = string.IsNullOrEmpty(reader["IdOrdenCompra"].ToString()) ? null : (decimal?)(reader["IdOrdenCompra"]),
                            IdProducto = Convert.ToDecimal(reader["IdProducto"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            mv_costo = Convert.ToDouble(reader["mv_costo"]),
                            pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                            pr_codigo = Convert.ToString(reader["pr_codigo"]),
                            pr_descripcion = Convert.ToString(reader["pr_descripcion"]),
                            Secuencia = Convert.ToInt32(reader["Secuencia"]),
                            Su_Descripcion = Convert.ToString(reader["Su_Descripcion"]),
                            tm_descripcion = Convert.ToString(reader["tm_descripcion"]),
                            cc_Descripcion = Convert.ToString(reader["cc_Descripcion"]),
                            tp_descripcion = Convert.ToString(reader["tp_descripcion"])
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

        public List<INV_008_Info> GetListResumen(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, DateTime fecha_ini, DateTime fecha_fin, string IdCentroCosto, string signo, int IdMovi_inven_tipo, int IdProductoTipo, string IdCategoria, int IdLinea, int IdGrupo, int IdSubGrupo)
        {
            try
            {
                List<INV_008_Info> Lista = new List<INV_008_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    string query = "select b.IdEmpresa, b.IdSucursal, b.IdBodega, b.IdProducto, c.pr_codigo, c.pr_descripcion, c.IdCategoria, c.IdLinea, c.IdGrupo, c.IdSubGrupo, "
                                +" g.ca_Categoria, f.nom_linea, e.nom_grupo, d.nom_subgrupo, b.dm_cantidad dm_cantidad, b.dm_cantidad * b.mv_costo as mv_costo,"
                                +" CASE WHEN b.dm_cantidad > 0 then ' INGRESO' ELSE 'EGRESO' END AS Tipo"
                                +" from in_Ing_Egr_Inven as a inner join"
                                +" in_Ing_Egr_Inven_det as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdMovi_inven_tipo = b.IdMovi_inven_tipo and a.IdNumMovi = b.IdNumMovi left join"
                                +" in_Producto as c on c.IdEmpresa = b.IdEmpresa and c.IdProducto = b.IdProducto left join"
                                +" in_subgrupo as d on c.IdEmpresa = d.IdEmpresa and c.IdCategoria = d.IdCategoria and c.IdLinea = d.IdLinea and c.IdGrupo = d.IdGrupo and c.IdSubGrupo = d.IdSubgrupo left join"
                                +" in_grupo as e on c.IdEmpresa = e.IdEmpresa and c.IdCategoria = e.IdCategoria and c.IdLinea = e.IdLinea and c.IdGrupo = e.IdGrupo left join"
                                +" in_linea as f on c.IdEmpresa = f.IdEmpresa and c.IdCategoria = f.IdCategoria and c.IdLinea = f.IdLinea left join"
                                +" in_categorias as g on g.IdEmpresa = c.IdEmpresa and g.IdCategoria = c.IdCategoria left join"
                                + " in_movi_inven_tipo as h on h.IdEmpresa = a.IdEmpresa and h.IdMovi_inven_tipo = a.IdMovi_inven_tipo"
                                + " WHERE(a.Estado = 'A') and a.cm_fecha between DATEFROMPARTS(" + fecha_ini.Year.ToString() + "," + fecha_ini.Month.ToString() + "," + fecha_ini.Day.ToString() + ") AND DATEFROMPARTS(" + fecha_fin.Year.ToString() + "," + fecha_fin.Month.ToString() + "," + fecha_fin.Day.ToString() + ")";

                    if (IdProducto != 0)
                        query += " AND b.IdProducto = " + IdProducto.ToString();

                    if (IdSucursal != 0)
                        query += " AND b.IdSucursal = " + IdSucursal.ToString();

                    if (IdBodega != 0)
                        query += " AND b.IdBodega = " + IdBodega.ToString();

                    if (IdMovi_inven_tipo != 0)
                        query += " AND b.IdMovi_inven_tipo = " + IdMovi_inven_tipo.ToString();

                    if (IdProductoTipo != 0)
                        query += " AND b.IdProductoTipo = " + IdProductoTipo.ToString();

                    if (!string.IsNullOrEmpty(IdCentroCosto))
                        query += " AND b.IdCentroCosto = '" + IdCentroCosto + "'";

                    if (!string.IsNullOrEmpty(signo))
                        query += " AND h.cm_tipo_movi = '" + signo + "'";

                    if (!string.IsNullOrEmpty(IdCategoria))
                        query += " AND c.IdCategoria = '" + IdCategoria + "'";

                    if (IdLinea != 0)
                        query += " AND c.IdLinea = " + IdLinea.ToString();

                    if (IdGrupo != 0)
                        query += " AND c.IdGrupo= " + IdGrupo.ToString();

                    if (IdSubGrupo != 0)
                        query += " AND c.IdSubGrupo= " + IdSubGrupo.ToString();


                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new INV_008_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdProducto = Convert.ToDecimal(reader["IdProducto"]),
                            pr_codigo = Convert.ToString(reader["pr_codigo"]),
                            pr_descripcion = Convert.ToString(reader["pr_descripcion"]),
                            IdCategoria = Convert.ToString(reader["IdCategoria"]),
                            IdLinea = Convert.ToInt32(reader["IdLinea"]),
                            IdGrupo = Convert.ToInt32(reader["IdGrupo"]),
                            IdSubGrupo = Convert.ToInt32(reader["IdSubGrupo"]),
                            ca_Categoria = Convert.ToString(reader["ca_Categoria"]),
                            nom_linea = Convert.ToString(reader["nom_linea"]),
                            nom_grupo = Convert.ToString(reader["nom_grupo"]),
                            nom_subgrupo = Convert.ToString(reader["nom_subgrupo"]),
                            Tipo = Convert.ToString(reader["Tipo"]),
                            dm_cantidad = Convert.ToDouble(reader["dm_cantidad"]),
                            mv_costo = Convert.ToDouble(reader["mv_costo"])
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
