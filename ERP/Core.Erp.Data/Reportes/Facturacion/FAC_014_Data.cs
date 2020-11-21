using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Facturacion
{
    public class FAC_014_Data
    {
        public List<FAC_014_Info> GetList(int IdEmpresa, decimal IdCliente, string IdCentroCosto, DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                List<FAC_014_Info> Lista = new List<FAC_014_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select d.IdEmpresa, d.IdSucursal, d.IdBodega, d.IdCbteVta, a.vt_fecha, a.IdCliente, f.NumCotizacion, f.NumOPr,a.vt_serie1+'-'+ a.vt_serie2+'-'+ a.vt_NumFactura vt_NumFactura,"
                                        + " c.pe_nombreCompleto, e.pr_descripcion, d.IdCentroCosto, g.cc_Descripcion, dbo.BankersRounding(d.vt_Subtotal, 2) vt_Subtotal, a.vt_Observacion"
                                        + " from fa_factura as a join"
                                        + " fa_cliente as b on a.IdEmpresa = b.IdEmpresa and a.IdCliente = b.IdCliente join"
                                        + " tb_persona as c on b.IdPersona = c.IdPersona join"
                                        + " fa_factura_det as d on d.IdEmpresa = a.IdEmpresa and d.IdSucursal = a.IdSucursal and d.IdBodega = a.IdBodega and a.IdCbteVta = d.IdCbteVta join"
                                        + " in_Producto as e on d.IdEmpresa = e.IdEmpresa and d.IdProducto = e.IdProducto left join"
                                        + " fa_proforma_det as f on d.IdEmpresa_pf = f.IdEmpresa and d.IdSucursal_pf = f.IdSucursal and d.IdProforma = f.IdProforma and d.Secuencia_pf = f.Secuencia left join"
                                        + " ct_CentroCosto as g on d.IdEmpresa = g.IdEmpresa and d.IdCentroCosto = g.IdCentroCosto"
                                        + " where a.IdEmpresa = " + IdEmpresa.ToString() + " and a.Estado = 'A'"
                                        + " and a.vt_fecha between DATEFROMPARTS(" + FechaIni.Year.ToString() + "," + FechaIni.Month.ToString() + ", " + FechaIni.Day.ToString() + ") and DATEFROMPARTS(" + FechaFin.Year.ToString() + "," + FechaFin.Month.ToString() + ", " + FechaFin.Day.ToString() + ") ";

                    if (!string.IsNullOrEmpty(IdCentroCosto))
                        command.CommandText += " and d.IdCentroCosto = '" + IdCentroCosto + "'";

                    if (IdCliente > 0)
                        command.CommandText += " and a.IdCliente = " + IdCliente.ToString();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new FAC_014_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdCbteVta = Convert.ToDecimal(reader["IdCbteVta"]),
                            vt_fecha = Convert.ToDateTime(reader["vt_fecha"]),
                            IdCliente = Convert.ToDecimal(reader["IdCliente"]),
                            NumCotizacion = Convert.ToString(reader["NumCotizacion"]),
                            NumOPr = Convert.ToString(reader["NumOPr"]),
                            vt_NumFactura = Convert.ToString(reader["vt_NumFactura"]),
                            pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                            pr_descripcion = Convert.ToString(reader["pr_descripcion"]),
                            IdCentroCosto = Convert.ToString(reader["IdCentroCosto"]),
                            cc_Descripcion = Convert.ToString(reader["cc_Descripcion"]),
                            vt_Subtotal = Convert.ToDouble(reader["vt_Subtotal"]),
                            vt_Observacion = Convert.ToString(reader["vt_Observacion"])
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
