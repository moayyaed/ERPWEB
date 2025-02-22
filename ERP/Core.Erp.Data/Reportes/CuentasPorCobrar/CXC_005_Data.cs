﻿using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.CuentasPorCobrar
{
    public class CXC_005_Data
    {
        public List<CXC_005_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdCLiente,int Idtipo_cliente,  DateTime? fecha_corte, bool mostrarSaldo0)
        {
            try
            {
                int IdSucursal_ini = IdSucursal;
                int IdSucursal_fin = IdSucursal == 0 ? 99999999 : IdSucursal;

                decimal IdCliente_ini = IdCLiente;
                decimal IdCliente_fin = IdCLiente == 0 ? 99999999 : IdCLiente;

                int Idtipo_clienteIni = Idtipo_cliente;
                int Idtipo_clienteFin = Idtipo_cliente == 0 ? 999999999 : Idtipo_cliente;
                List<CXC_005_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.SPCXC_005(IdEmpresa, IdSucursal_ini, IdSucursal_fin, Idtipo_clienteIni, Idtipo_clienteFin, IdCliente_ini, IdCliente_fin, fecha_corte, mostrarSaldo0)
                             select new CXC_005_Info
                             {
                                IdEmpresa = q.IdEmpresa,
                                IdSucursal = q.IdSucursal,
                                IdBodega = q.IdBodega,
                                IdCbteVta = q.IdCbteVta,
                                IdCliente = q.IdCliente,
                                Cobrado = q.Cobrado,
                                NotaCredito = q.NotaCredito,
                                IVA = q.IVA,
                                NomCliente = q.NomCliente,
                                Saldo = q.Saldo,
                                Subtotal = q.Subtotal,
                                Total = q.Total,
                                vt_fecha = q.vt_fecha,
                                vt_fech_venc = q.vt_fech_venc,
                                vt_NumFactura = q.vt_NumFactura,
                                vt_tipoDoc = q.vt_tipoDoc,
                                Su_Descripcion = q.Su_Descripcion,
                                Descripcion_tip_cliente = q.Descripcion_tip_cliente,
                                Idtipo_cliente = q.Idtipo_cliente
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
