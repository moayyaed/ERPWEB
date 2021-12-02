﻿using Core.Erp.Data.Caja;
using Core.Erp.Data.Contabilidad;
using Core.Erp.Data.Facturacion.Base;
using Core.Erp.Data.General;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Erp.Data.CuentasPorCobrar
{
    public class cxc_cobro_Data
    {
        caj_Caja_Movimiento_Data data_caj_movi = new caj_Caja_Movimiento_Data();
        public List<cxc_cobro_Info> get_list(int IdEmpresa, int IdSucursal, DateTime Fecha_ini, DateTime Fecha_fin)
        {
            try
            {
                int IdSucursal_ini = IdSucursal;
                int IdSucursal_fin = IdSucursal == 0 ? 9999 : IdSucursal;
                List<cxc_cobro_Info> Lista;

                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    Lista = (from q in Context.vwcxc_cobro
                             where q.IdEmpresa == IdEmpresa
                             && IdSucursal_ini <= q.IdSucursal && q.IdSucursal <= IdSucursal_fin
                             && Fecha_ini <= q.cr_fecha && q.cr_fecha <= Fecha_fin
                             orderby q.IdCobro descending
                             select new cxc_cobro_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdCobro = q.IdCobro,
                                 IdCliente = q.IdCliente,
                                 pe_nombreCompleto = q.pe_nombreCompleto,
                                 IdCobro_tipo = q.IdCobro_tipo,
                                 cr_fecha = q.cr_fecha,
                                 cr_TotalCobro = q.cr_TotalCobro,
                                 cr_estado = q.cr_estado,
                                 Su_Descripcion = q.Su_Descripcion,
                                 cr_observacion = q.cr_observacion,
                                 nom_Motivo_tipo_cobro = q.nom_Motivo_tipo_cobro,
                                 cr_NumDocumento = q.cr_NumDocumento,
                                 cr_NumDocumento_Dep = q.cr_NumDocumento,
                                 EstadoBool = q.cr_estado == "A" ? true : false
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private decimal get_id(int IdEmpesa, int IdSucursal)
        {
            try
            {
                decimal ID = 1;

                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var lst = from q in Context.cxc_cobro
                              where q.IdEmpresa == IdEmpesa
                              && q.IdSucursal == IdSucursal
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdCobro) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public cxc_cobro_Info get_info(int IdEmpresa, int IdSucursal, decimal IdCobro)
        {
            try
            {
                cxc_cobro_Info info;

                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var Entity = Context.cxc_cobro.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdCobro == IdCobro).FirstOrDefault();
                    if (Entity == null) return null;
                    info = new cxc_cobro_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdCobro = Entity.IdCobro,
                        cr_Codigo = Entity.cr_Codigo,
                        IdCobro_tipo = Entity.IdCobro_tipo,
                        IdCliente = Entity.IdCliente,
                        cr_TotalCobro = Entity.cr_TotalCobro,
                        cr_fecha = Entity.cr_fecha,
                        cr_fechaDocu = Entity.cr_fechaDocu,
                        cr_fechaCobro = Entity.cr_fechaCobro,
                        cr_observacion = Entity.cr_observacion,
                        cr_Banco = Entity.cr_Banco,
                        cr_cuenta = Entity.cr_cuenta,
                        cr_NumDocumento = Entity.cr_NumDocumento,
                        cr_NumDocumento_Dep = Entity.cr_NumDocumento,
                        cr_Tarjeta = Entity.cr_Tarjeta,
                        cr_propietarioCta = Entity.cr_propietarioCta,
                        cr_estado = Entity.cr_estado,
                        cr_recibo = Entity.cr_recibo,
                        cr_EsProvision = Entity.cr_EsProvision,
                        cr_Excedente = Entity.cr_Excedente,
                        IdBanco = Entity.IdBanco,
                        IdCaja = Entity.IdCaja
                    };
                }

                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(cxc_cobro_Info info)
        {
            Entities_cuentas_por_cobrar Context_cxc = new Entities_cuentas_por_cobrar();
            Entities_facturacion Context_fac = new Entities_facturacion();
            Entities_caja Context_caj = new Entities_caja();
            Entities_contabilidad Context_ct = new Entities_contabilidad();
            Entities_general Context_gen = new Entities_general();
            ct_cbtecble_Data data_ct = new ct_cbtecble_Data();
            try
            {
                #region Variables
                int Secuencia = 1;
                bool generar_diario = true;
                
                string IdCtaCble_haber = string.Empty;
                string IdCtaCble_Anticipo = string.Empty;
                int IdTipoCbte = 0;
                int IdTipoMoviCaja = 0;
                cxc_Parametro param_cxc;
                #endregion

                #region Consultas para generar diario

                param_cxc = Context_cxc.cxc_Parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                if (param_cxc == null)
                    return false;

                #region CtaCble debe
                var cliente = Context_fac.fa_cliente.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCliente == info.IdCliente).FirstOrDefault();
                if (cliente == null)
                    return false;
                IdCtaCble_haber = cliente.IdCtaCble_cxc_Credito;
                IdCtaCble_Anticipo = cliente.IdCtaCble_Anticipo;
                #endregion
                
                #region CtaCble Haber
                if (info.IdCobro_tipo != null)
                {
                    var tipo_cobro = Context_cxc.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == info.IdCobro_tipo).FirstOrDefault();
                    if (tipo_cobro == null)
                        return false;


                    if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.CAJA.ToString())
                    {
                        var caja = Context_caj.caj_Caja.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == info.IdCaja).FirstOrDefault();
                        if (caja == null)
                            return false;
                        info.lst_det.ForEach(q => q.IdCtaCble = caja.IdCtaCble);
                    }
                    else
                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.TIP_COBRO.ToString())
                    {
                        foreach (var item in info.lst_det)
                        {
                            var cta_x_tipo = Context_cxc.cxc_cobro_tipo_Param_conta_x_sucursal.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdCobro_tipo == item.IdCobro_tipo_det && q.IdSucursal == item.IdSucursal).FirstOrDefault();
                            if (cta_x_tipo != null)
                                item.IdCtaCble = cta_x_tipo.IdCtaCble;
                        }
                    }
                }
                else
                {
                    foreach (var item in info.lst_det)
                    {
                        var tipo_cobro = Context_cxc.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == item.IdCobro_tipo_det).FirstOrDefault();
                        if (tipo_cobro == null)
                            return false;

                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.CAJA.ToString())
                        {
                            var caja = Context_caj.caj_Caja.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == info.IdCaja).FirstOrDefault();
                            if (caja == null)
                                return false;
                            item.IdCtaCble = caja.IdCtaCble;
                        }
                        else
                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.TIP_COBRO.ToString())
                        {
                            if (info.IdCobro_tipo == null)
                            {
                                var cta_x_tipo = Context_cxc.cxc_cobro_tipo_Param_conta_x_sucursal.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdCobro_tipo == item.IdCobro_tipo_det && q.IdSucursal == item.IdSucursal).FirstOrDefault();
                                if (cta_x_tipo != null)
                                    item.IdCtaCble = cta_x_tipo.IdCtaCble;
                            }
                        }
                    }
                }
               
                if (info.lst_det.Where(q=> string.IsNullOrEmpty(q.IdCtaCble)).Count() > 0 || string.IsNullOrEmpty(IdCtaCble_haber))
                    generar_diario = false;                
                #endregion

                if(generar_diario)
                {
                    #region TipoCbte
                    
                    var d = info.lst_det[0];
                    var tipo_cobro = Context_cxc.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == d.IdCobro_tipo_det).FirstOrDefault();
                    if (tipo_cobro == null)
                        return false;
                    if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.TIP_COBRO.ToString())
                    {
                        IdTipoCbte = param_cxc.pa_IdTipoCbteCble_CxC;
                    }
                    else
                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.CAJA.ToString())
                    {
                        var param_caja = Context_caj.caj_parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                        if (param_caja == null)
                            return false;
                        IdTipoCbte = param_caja.IdTipoCbteCble_MoviCaja_Ing;
                        IdTipoMoviCaja = param_cxc.pa_IdTipoMoviCaja_x_Cobros_x_cliente;
                    }

                    if (IdTipoCbte == 0)
                        generar_diario = false;
                    
                    #endregion
                }
                #endregion

                #region Cabecera cobro
                cxc_cobro cab = new cxc_cobro
                {
                    IdEmpresa = info.IdEmpresa,
                    IdSucursal = info.IdSucursal,
                    IdCobro = info.IdCobro = get_id(info.IdEmpresa, info.IdSucursal),
                    cr_Codigo = info.cr_Codigo,
                    IdCobro_tipo = info.IdCobro_tipo,
                    IdCliente = info.IdCliente,
                    cr_TotalCobro = info.cr_TotalCobro,
                    cr_fecha = info.cr_fecha.Date,
                    cr_fechaDocu = info.cr_fechaDocu,
                    cr_fechaCobro = info.cr_fechaCobro,
                    cr_observacion = info.cr_observacion,

                    cr_Banco = info.cr_Banco,
                    cr_cuenta = info.cr_cuenta,
                    cr_NumDocumento = info.cr_NumDocumento,
                    cr_Tarjeta = info.cr_Tarjeta,
                    cr_propietarioCta = info.cr_propietarioCta,
                    cr_estado = "A",
                    cr_EsProvision = info.cr_EsProvision,
                    cr_EsElectronico = info.cr_EsElectronico,
                    cr_Excedente = info.cr_Excedente,
                    IdBanco = info.IdBanco,
                    IdCaja = info.IdCaja,
                    NumeroAutorizacion = info.NumeroAutorizacion,

                    Fecha_Transac = DateTime.Now,
                    IdUsuario = info.IdUsuario
                };
                Context_cxc.cxc_cobro.Add(cab);
                #endregion

                #region Detalle cobro
                foreach (var item in info.lst_det)
                {
                    cxc_cobro_det det = new cxc_cobro_det
                    {
                        IdEmpresa = item.IdEmpresa = cab.IdEmpresa,
                        IdSucursal = item.IdSucursal = cab.IdSucursal,
                        IdCobro = item.IdCobro = cab.IdCobro,
                        secuencial = item.secuencial = Secuencia++,
                        dc_TipoDocumento = item.dc_TipoDocumento,
                        IdBodega_Cbte = item.IdBodega_Cbte,
                        IdCbte_vta_nota = item.IdCbte_vta_nota,
                        dc_ValorPago = item.dc_ValorPago,
                        IdUsuario = cab.IdUsuario,
                        Fecha_Transac = DateTime.Now,
                        estado = "A",
                        IdCobro_tipo = item.IdCobro_tipo_det
                    };
                    Context_cxc.cxc_cobro_det.Add(det);
                }
                #endregion               

                if (generar_diario)
                {
                    var persona = Context_gen.tb_persona.Where(q => q.IdPersona == cliente.IdPersona).FirstOrDefault();
                    if (persona == null) persona = new tb_persona();

                    #region Diario
                    ct_cbtecble diario = new ct_cbtecble
                    {
                        IdEmpresa = cab.IdEmpresa,
                        IdTipoCbte = IdTipoCbte,
                        IdCbteCble = data_ct.get_id(info.IdEmpresa, IdTipoCbte),
                        cb_Fecha = cab.cr_fecha.Date,
                        IdSucursal = info.IdSucursal,
                        IdPeriodo = Convert.ToInt32(cab.cr_fecha.ToString("yyyyMM")),
                        cb_Observacion = "COBRO #" + info.IdCobro + " " + cab.cr_observacion + " CLIENTE: "+persona.pe_nombreCompleto,
                        cb_Estado = "A",

                        IdUsuario = cab.IdUsuario,
                        cb_FechaTransac = DateTime.Now,
                        cb_Valor = Math.Round(info.cr_TotalCobro, 2, MidpointRounding.AwayFromZero),
                    };
                    Context_ct.ct_cbtecble.Add(diario);
                    Secuencia = 1;
                    if (IdTipoMoviCaja != 0)
                    {
                        ct_cbtecble_det Debe = new ct_cbtecble_det
                        {
                            IdEmpresa = diario.IdEmpresa,
                            IdTipoCbte = diario.IdTipoCbte,
                            IdCbteCble = diario.IdCbteCble,
                            secuencia = Secuencia++,
                            IdCtaCble = info.lst_det[0].IdCtaCble,
                            dc_Valor = Math.Round(Convert.ToDouble(diario.cb_Valor), 2, MidpointRounding.AwayFromZero),
                        };
                        Context_ct.ct_cbtecble_det.Add(Debe);
                    }
                    else
                    {
                        foreach (var item in info.lst_det)
                        {
                            Context_ct.ct_cbtecble_det.Add(new ct_cbtecble_det
                            {
                                IdEmpresa = diario.IdEmpresa,
                                IdTipoCbte = diario.IdTipoCbte,
                                IdCbteCble = diario.IdCbteCble,
                                secuencia = Secuencia++,
                                dc_Valor = Math.Round(Convert.ToDouble(item.dc_ValorPago), 2, MidpointRounding.AwayFromZero),
                                IdCtaCble = info.cr_EsProvision == true ? (item.IdCobro_tipo_det.Contains("RTFTE") ? param_cxc.IdCtaCble_ProvisionFuente : param_cxc.IdCtaCble_ProvisionIva ) : item.IdCtaCble,                                
                                IdPunto_cargo = info.cr_EsProvision == true ? (item.IdCobro_tipo_det.Contains("RTFTE") ? param_cxc.IdPunto_cargo_Fte : param_cxc.IdPunto_cargo_Iva) : null,
                                IdPunto_cargo_grupo = info.cr_EsProvision == true ? (item.IdCobro_tipo_det.Contains("RTFTE") ? param_cxc.IdPunto_cargo_grupo_Fte : param_cxc.IdPunto_cargo_grupo_Iva) : null
                            });
                        }
                    }

                    ct_cbtecble_det Haber = new ct_cbtecble_det
                    {
                        IdEmpresa = diario.IdEmpresa,
                        IdTipoCbte = diario.IdTipoCbte,
                        IdCbteCble = diario.IdCbteCble,
                        secuencia = Secuencia++,
                        IdCtaCble = IdCtaCble_haber,
                        dc_Valor = Math.Round(Convert.ToDouble(info.lst_det.Sum(q=> q.dc_ValorPago)), 2, MidpointRounding.AwayFromZero) * -1,
                    };
                    Context_ct.ct_cbtecble_det.Add(Haber);

                    if ((info.cr_Excedente ?? 0) > 0)
                    {
                        Context_ct.ct_cbtecble_det.Add(new ct_cbtecble_det
                        {
                            IdEmpresa = diario.IdEmpresa,
                            IdTipoCbte = diario.IdTipoCbte,
                            IdCbteCble = diario.IdCbteCble,
                            secuencia = Secuencia++,
                            IdCtaCble = IdCtaCble_Anticipo,
                            dc_Valor = Math.Round(Convert.ToDouble(info.cr_Excedente ?? 0), 2, MidpointRounding.AwayFromZero) * -1,
                        });
                    }
                    #endregion

                    #region Relacion cobro - diario
                    cxc_cobro_x_ct_cbtecble relacion_diario_cobro = new cxc_cobro_x_ct_cbtecble
                    {
                        cbr_IdEmpresa = info.IdEmpresa,
                        cbr_IdCobro = cab.IdCobro,
                        cbr_IdSucursal = cab.IdSucursal,
                        ct_IdEmpresa = diario.IdEmpresa,
                        ct_IdTipoCbte = diario.IdTipoCbte,
                        ct_IdCbteCble = diario.IdCbteCble
                    };
                    Context_cxc.cxc_cobro_x_ct_cbtecble.Add(relacion_diario_cobro);
                    #endregion
                    
                    if (IdTipoMoviCaja != 0)
                    {
                        #region Movimiento de caja
                        caj_Caja_Movimiento mov_caja = new caj_Caja_Movimiento
                        {
                            IdEmpresa = diario.IdEmpresa,
                            IdTipocbte = diario.IdTipoCbte,
                            IdCbteCble = diario.IdCbteCble,
                            IdTipoMovi = IdTipoMoviCaja,
                            cm_fecha = diario.cb_Fecha,
                            cm_valor = diario.cb_Valor,
                            cm_Signo = "+",
                            cm_observacion = diario.cb_Observacion,
                            Estado = "A",
                            IdPeriodo = diario.IdPeriodo,
                            IdCaja = info.IdCaja,
                            IdTipo_Persona = cl_enumeradores.eTipoPersona.CLIENTE.ToString(),
                            IdEntidad = info.IdEntidad,
                            IdPersona = cliente.IdPersona,
                            SecuenciaCaja = data_caj_movi.get_Secuencia(diario.IdEmpresa,info.IdCaja,"+"),

                            IdUsuario = info.IdUsuario,
                            Fecha_Transac = DateTime.Now
                        };
                        Context_caj.caj_Caja_Movimiento.Add(mov_caja);
                        caj_Caja_Movimiento_det mov_caja_det = new caj_Caja_Movimiento_det
                        {
                            IdEmpresa = diario.IdEmpresa,
                            IdTipocbte = diario.IdTipoCbte,
                            IdCbteCble = diario.IdCbteCble,
                            IdCobro_tipo = info.IdCobro_tipo,
                            cr_Valor = diario.cb_Valor,
                            Secuencia = 1
                        };
                        Context_caj.caj_Caja_Movimiento_det.Add(mov_caja_det);
                        #endregion
                    }
                }

                Context_ct.SaveChanges();
                Context_cxc.SaveChanges();
                Context_caj.SaveChanges();               

                Context_cxc.Dispose();
                Context_fac.Dispose();
                Context_caj.Dispose();
                Context_ct.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Context_cxc.Dispose();
                Context_fac.Dispose();
                Context_caj.Dispose();
                Context_ct.Dispose();
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "cxc_cobro_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuario});
                return false;
            }
        }

        public bool modificarDB(cxc_cobro_Info info)
        {
            Entities_cuentas_por_cobrar Context_cxc = new Entities_cuentas_por_cobrar();
            Entities_facturacion Context_fac = new Entities_facturacion();
            Entities_caja Context_caj = new Entities_caja();
            Entities_contabilidad Context_ct = new Entities_contabilidad();
            ct_cbtecble_Data data_ct = new ct_cbtecble_Data();
            Entities_general Context_gen = new Entities_general();
            try
            {
                #region Variables
                int Secuencia = 1;
                bool generar_diario = true;
                string IdCtaCble_haber = string.Empty;
                string IdCtaCble_Anticipo = string.Empty;
                int IdTipoCbte = 0;
                int IdTipoMoviCaja = 0;
                cxc_Parametro param_cxc;
                #endregion

                param_cxc = Context_cxc.cxc_Parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                if (param_cxc == null)
                    return false;

                #region Cabecera cobro
                var Entity = Context_cxc.cxc_cobro.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdCobro == info.IdCobro).FirstOrDefault();
                if (Entity == null) return false;
                Entity.cr_Codigo = info.cr_Codigo;
                Entity.IdCobro_tipo = info.IdCobro_tipo;
                Entity.IdCliente = info.IdCliente;
                Entity.cr_TotalCobro = info.cr_TotalCobro;
                Entity.cr_fecha = info.cr_fecha.Date;
                Entity.cr_fechaDocu = info.cr_fechaDocu;
                Entity.cr_fechaCobro = info.cr_fechaCobro;
                Entity.cr_observacion = info.cr_observacion;
                Entity.cr_Banco = info.cr_Banco;
                Entity.cr_cuenta = info.cr_cuenta;
                Entity.cr_NumDocumento = info.cr_NumDocumento;
                Entity.cr_Tarjeta = info.cr_Tarjeta;
                Entity.cr_propietarioCta = info.cr_propietarioCta;
                Entity.IdBanco = info.IdBanco;
                Entity.IdCaja = info.IdCaja;
                Entity.cr_EsElectronico = info.cr_EsElectronico;
                Entity.cr_Excedente = info.cr_Excedente;
                Entity.cr_EsProvision = info.cr_EsProvision;
                Entity.cr_EsProvision = info.cr_EsProvision;
                Entity.NumeroAutorizacion = info.NumeroAutorizacion;
                Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                Entity.Fecha_UltMod = DateTime.Now;
                #endregion

                #region Consultas para generar diario
                #region CtaCble debe
                var cliente = Context_fac.fa_cliente.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCliente == info.IdCliente).FirstOrDefault();
                if (cliente == null)
                    return false;
                IdCtaCble_haber = cliente.IdCtaCble_cxc_Credito;
                IdCtaCble_Anticipo = cliente.IdCtaCble_Anticipo;
                #endregion

                #region CtaCble Haber
                if (info.IdCobro_tipo != null)
                {
                    var tipo_cobro = Context_cxc.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == info.IdCobro_tipo).FirstOrDefault();
                    if (tipo_cobro == null)
                        return false;


                    if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.CAJA.ToString())
                    {
                        var caja = Context_caj.caj_Caja.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == info.IdCaja).FirstOrDefault();
                        if (caja == null)
                            return false;
                        info.lst_det.ForEach(q => q.IdCtaCble = caja.IdCtaCble);
                    }
                    else
                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.TIP_COBRO.ToString())
                    {
                        if (info.IdCobro_tipo == null)
                        {
                            foreach (var item in info.lst_det)
                            {
                                var cta_x_tipo = Context_cxc.cxc_cobro_tipo_Param_conta_x_sucursal.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdCobro_tipo == item.IdCobro_tipo_det && q.IdSucursal == item.IdSucursal).FirstOrDefault();
                                if (cta_x_tipo != null)
                                    item.IdCtaCble = cta_x_tipo.IdCtaCble;
                            }
                        }else
                        {
                            var cta_x_tipo = Context_cxc.cxc_cobro_tipo_Param_conta_x_sucursal.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCobro_tipo == info.IdCobro_tipo && q.IdSucursal == info.IdSucursal).FirstOrDefault();
                            if (cta_x_tipo != null)
                                info.lst_det.ForEach(q=>q.IdCtaCble = cta_x_tipo.IdCtaCble);
                        }
                    }
                }
                else
                {
                    foreach (var item in info.lst_det)
                    {
                        var tipo_cobro = Context_cxc.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == item.IdCobro_tipo_det).FirstOrDefault();
                        if (tipo_cobro == null)
                            return false;

                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.CAJA.ToString())
                        {
                            var caja = Context_caj.caj_Caja.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == info.IdCaja).FirstOrDefault();
                            if (caja == null)
                                return false;
                            item.IdCtaCble = caja.IdCtaCble;
                        }
                        else
                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.TIP_COBRO.ToString())
                        {
                            if (info.IdCobro_tipo == null)
                            {
                                var cta_x_tipo = Context_cxc.cxc_cobro_tipo_Param_conta_x_sucursal.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdCobro_tipo == item.IdCobro_tipo_det && q.IdSucursal == item.IdSucursal).FirstOrDefault();
                                if (cta_x_tipo != null)
                                    item.IdCtaCble = cta_x_tipo.IdCtaCble;
                            }
                        }
                    }
                }

                if (info.lst_det.Where(q => string.IsNullOrEmpty(q.IdCtaCble)).Count() > 0 || string.IsNullOrEmpty(IdCtaCble_haber))
                    generar_diario = false;
                #endregion

                if (generar_diario)
                {
                    #region TipoCbte
                    
                    var d = info.lst_det[0];
                    var tipo_cobro = Context_cxc.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == d.IdCobro_tipo_det).FirstOrDefault();
                    if (tipo_cobro == null)
                        return false;
                    if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.TIP_COBRO.ToString())
                    {
                        IdTipoCbte = param_cxc.pa_IdTipoCbteCble_CxC;
                    }
                    else
                        if (tipo_cobro.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.CAJA.ToString())
                    {
                        var param_caja = Context_caj.caj_parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                        if (param_caja == null)
                            return false;
                        IdTipoCbte = param_caja.IdTipoCbteCble_MoviCaja_Ing;
                        IdTipoMoviCaja = param_cxc.pa_IdTipoMoviCaja_x_Cobros_x_cliente;
                    }

                    if (IdTipoCbte == 0)
                        generar_diario = false;

                    #endregion
                }
                #endregion

                #region Detalle cobro
                var cobros_det = Context_cxc.cxc_cobro_det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdCobro == info.IdCobro).ToList();
                foreach (var item in cobros_det)
                {
                    Context_cxc.cxc_cobro_det.Remove(item);
                }
                foreach (var item in info.lst_det)
                {
                    cxc_cobro_det det = new cxc_cobro_det
                    {
                        IdEmpresa = item.IdEmpresa = info.IdEmpresa,
                        IdSucursal = item.IdSucursal = info.IdSucursal,
                        IdCobro = item.IdCobro = info.IdCobro,
                        secuencial = item.secuencial = Secuencia++,
                        dc_TipoDocumento = item.dc_TipoDocumento,
                        IdBodega_Cbte = item.IdBodega_Cbte,
                        IdCbte_vta_nota = item.IdCbte_vta_nota,
                        dc_ValorPago = item.dc_ValorPago,
                        IdUsuarioUltMod = info.IdUsuario,
                        Fecha_UltMod = DateTime.Now,
                        estado = "A",
                        IdCobro_tipo = item.IdCobro_tipo_det
                    };
                    Context_cxc.cxc_cobro_det.Add(det);
                }
                #endregion

                #region Contabilizacion
                if (info.lst_det.Sum(q => q.dc_ValorPago) > 0)
                {
                    var relacion = Context_cxc.cxc_cobro_x_ct_cbtecble.Where(q => q.cbr_IdEmpresa == info.IdEmpresa && q.cbr_IdSucursal == info.IdSucursal && q.cbr_IdCobro == info.IdCobro).FirstOrDefault();
                    var persona = Context_gen.tb_persona.Where(q => q.IdPersona == cliente.IdPersona).FirstOrDefault();
                    if (persona == null) persona = new tb_persona();
                    #region Si no tiene relacion creada
                    if (generar_diario && relacion == null)
                    {
                        #region Diario
                        ct_cbtecble diario = new ct_cbtecble
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdTipoCbte = IdTipoCbte,
                            IdCbteCble = data_ct.get_id(info.IdEmpresa, IdTipoCbte),
                            cb_Fecha = info.cr_fecha.Date,
                            IdSucursal = info.IdSucursal,
                            IdPeriodo = Convert.ToInt32(info.cr_fecha.ToString("yyyyMM")),
                            cb_Observacion = "COBRO #" + info.IdCobro + " " + info.cr_observacion + " CLIENTE: " + persona.pe_nombreCompleto,
                            cb_Estado = "A",

                            IdUsuario = info.IdUsuario,
                            cb_FechaTransac = DateTime.Now,
                            cb_Valor = Math.Round(info.cr_TotalCobro, 2, MidpointRounding.AwayFromZero),
                        };
                        Context_ct.ct_cbtecble.Add(diario);
                        Secuencia = 1;
                        if (IdTipoMoviCaja != 0)
                        {
                            ct_cbtecble_det Debe = new ct_cbtecble_det
                            {
                                IdEmpresa = diario.IdEmpresa,
                                IdTipoCbte = diario.IdTipoCbte,
                                IdCbteCble = diario.IdCbteCble,
                                secuencia = Secuencia++,
                                IdCtaCble = info.lst_det[0].IdCtaCble,
                                dc_Valor = Math.Round(Convert.ToDouble(diario.cb_Valor), 2, MidpointRounding.AwayFromZero),
                            };
                            Context_ct.ct_cbtecble_det.Add(Debe);
                        }
                        else
                        {
                            foreach (var item in info.lst_det)
                            {
                                ct_cbtecble_det Debe = new ct_cbtecble_det
                                {
                                    IdEmpresa = diario.IdEmpresa,
                                    IdTipoCbte = diario.IdTipoCbte,
                                    IdCbteCble = diario.IdCbteCble,
                                    secuencia = Secuencia++,
                                    dc_Valor = Math.Round(Convert.ToDouble(item.dc_ValorPago), 2, MidpointRounding.AwayFromZero),
                                    IdCtaCble = info.cr_EsProvision == true ? (item.IdCobro_tipo_det.Contains("RTFTE") ? param_cxc.IdCtaCble_ProvisionFuente : param_cxc.IdCtaCble_ProvisionIva) : item.IdCtaCble,
                                    IdPunto_cargo = info.cr_EsProvision == true ? (item.IdCobro_tipo_det.Contains("RTFTE") ? param_cxc.IdPunto_cargo_Fte : param_cxc.IdPunto_cargo_Iva) : null,
                                    IdPunto_cargo_grupo = info.cr_EsProvision == true ? (item.IdCobro_tipo_det.Contains("RTFTE") ? param_cxc.IdPunto_cargo_grupo_Fte : param_cxc.IdPunto_cargo_grupo_Iva) : null
                                };
                                Context_ct.ct_cbtecble_det.Add(Debe);
                            }
                        }

                        ct_cbtecble_det Haber = new ct_cbtecble_det
                        {
                            IdEmpresa = diario.IdEmpresa,
                            IdTipoCbte = diario.IdTipoCbte,
                            IdCbteCble = diario.IdCbteCble,
                            secuencia = Secuencia++,
                            IdCtaCble = IdCtaCble_haber,
                            dc_Valor = Math.Round(Convert.ToDouble(diario.cb_Valor), 2, MidpointRounding.AwayFromZero) * -1,
                        };
                        Context_ct.ct_cbtecble_det.Add(Haber);

                        if ((info.cr_Excedente ?? 0) > 0)
                        {
                            Context_ct.ct_cbtecble_det.Add(new ct_cbtecble_det
                            {
                                IdEmpresa = diario.IdEmpresa,
                                IdTipoCbte = diario.IdTipoCbte,
                                IdCbteCble = diario.IdCbteCble,
                                secuencia = Secuencia++,
                                IdCtaCble = IdCtaCble_Anticipo,
                                dc_Valor = Math.Round(Convert.ToDouble(info.cr_Excedente ?? 0), 2, MidpointRounding.AwayFromZero) * -1,
                            });
                        }

                        
                        #endregion

                        #region Relacion cobro - diario
                        cxc_cobro_x_ct_cbtecble relacion_diario_cobro = new cxc_cobro_x_ct_cbtecble
                        {
                            cbr_IdEmpresa = info.IdEmpresa,
                            cbr_IdCobro = info.IdCobro,
                            cbr_IdSucursal = info.IdSucursal,
                            ct_IdEmpresa = diario.IdEmpresa,
                            ct_IdTipoCbte = diario.IdTipoCbte,
                            ct_IdCbteCble = diario.IdCbteCble
                        };
                        Context_cxc.cxc_cobro_x_ct_cbtecble.Add(relacion_diario_cobro);
                        #endregion

                        if (IdTipoMoviCaja != 0)
                        {
                            #region Movimiento de caja
                            caj_Caja_Movimiento mov_caja = new caj_Caja_Movimiento
                            {
                                IdEmpresa = diario.IdEmpresa,
                                IdTipocbte = diario.IdTipoCbte,
                                IdCbteCble = diario.IdCbteCble,
                                IdTipoMovi = IdTipoMoviCaja,
                                cm_fecha = diario.cb_Fecha,
                                cm_valor = diario.cb_Valor,
                                cm_Signo = "+",
                                SecuenciaCaja = data_caj_movi.get_Secuencia(diario.IdEmpresa, info.IdCaja, "+"),
                                cm_observacion = diario.cb_Observacion,
                                Estado = "A",
                                IdPeriodo = diario.IdPeriodo,
                                IdCaja = info.IdCaja,
                                IdTipo_Persona = cl_enumeradores.eTipoPersona.CLIENTE.ToString(),
                                IdEntidad = info.IdEntidad,
                                IdPersona = cliente.IdPersona,

                                IdUsuario = info.IdUsuario,
                                Fecha_Transac = DateTime.Now
                            };
                            Context_caj.caj_Caja_Movimiento.Add(mov_caja);
                            caj_Caja_Movimiento_det mov_caja_det = new caj_Caja_Movimiento_det
                            {
                                IdEmpresa = diario.IdEmpresa,
                                IdTipocbte = diario.IdTipoCbte,
                                IdCbteCble = diario.IdCbteCble,
                                IdCobro_tipo = info.IdCobro_tipo,
                                cr_Valor = diario.cb_Valor,
                                Secuencia = 1
                            };
                            Context_caj.caj_Caja_Movimiento_det.Add(mov_caja_det);
                            #endregion
                        }
                    }
                    #endregion

                    else

                    #region Si tiene diario creado
                    if (generar_diario && relacion != null)
                    {
                        var personac = Context_gen.tb_persona.Where(q => q.IdPersona == cliente.IdPersona).FirstOrDefault();
                        if (persona == null) persona = new tb_persona();
                        #region Diario
                        var diario = Context_ct.ct_cbtecble.Where(q => q.IdEmpresa == relacion.ct_IdEmpresa && q.IdTipoCbte == relacion.ct_IdTipoCbte && q.IdCbteCble == relacion.ct_IdCbteCble).FirstOrDefault();
                        if (diario == null)
                            return false;
                        diario.cb_Fecha = info.cr_fecha.Date;
                        diario.IdSucursal = info.IdSucursal;
                        diario.IdPeriodo = Convert.ToInt32(info.cr_fecha.ToString("yyyyMM"));
                        diario.cb_Observacion = "COBRO #" + info.IdCobro + " "+ info.cr_observacion + " CLIENTE: " + personac.pe_nombreCompleto;
                        diario.IdUsuarioUltModi = info.IdUsuarioUltMod;
                        diario.cb_FechaUltModi = DateTime.Now;
                        diario.cb_Valor = Math.Round(info.cr_TotalCobro, 2, MidpointRounding.AwayFromZero);

                        var diario_det = Context_ct.ct_cbtecble_det.Where(q => q.IdEmpresa == relacion.ct_IdEmpresa && q.IdTipoCbte == relacion.ct_IdTipoCbte && q.IdCbteCble == relacion.ct_IdCbteCble).ToList();
                        foreach (var item in diario_det)
                        {
                            Context_ct.ct_cbtecble_det.Remove(item);
                        }

                        Secuencia = 1;
                        if (IdTipoMoviCaja != 0)
                        {
                            ct_cbtecble_det Debe = new ct_cbtecble_det
                            {
                                IdEmpresa = diario.IdEmpresa,
                                IdTipoCbte = diario.IdTipoCbte,
                                IdCbteCble = diario.IdCbteCble,
                                secuencia = Secuencia++,
                                IdCtaCble = info.lst_det[0].IdCtaCble,
                                dc_Valor = Math.Round(Convert.ToDouble(diario.cb_Valor), 2, MidpointRounding.AwayFromZero),
                            };
                            Context_ct.ct_cbtecble_det.Add(Debe);
                        }
                        else
                        {
                            foreach (var item in info.lst_det)
                            {
                                Context_ct.ct_cbtecble_det.Add(new ct_cbtecble_det
                                {
                                    IdEmpresa = diario.IdEmpresa,
                                    IdTipoCbte = diario.IdTipoCbte,
                                    IdCbteCble = diario.IdCbteCble,
                                    secuencia = Secuencia++,
                                    IdCtaCble = item.IdCtaCble,
                                    dc_Valor = Math.Round(Convert.ToDouble(item.dc_ValorPago), 2, MidpointRounding.AwayFromZero),
                                });                               
                            }
                        }

                        ct_cbtecble_det Haber = new ct_cbtecble_det
                        {
                            IdEmpresa = relacion.ct_IdEmpresa,
                            IdTipoCbte = relacion.ct_IdTipoCbte,
                            IdCbteCble = relacion.ct_IdCbteCble,
                            secuencia = Secuencia++,
                            IdCtaCble = IdCtaCble_haber,
                            dc_Valor = Math.Round(Convert.ToDouble(info.lst_det.Sum(q=> q.dc_ValorPago)), 2, MidpointRounding.AwayFromZero) * -1,
                        };
                        Context_ct.ct_cbtecble_det.Add(Haber);

                        if ((info.cr_Excedente ?? 0) > 0)
                        {
                            Context_ct.ct_cbtecble_det.Add(new ct_cbtecble_det
                            {
                                IdEmpresa = diario.IdEmpresa,
                                IdTipoCbte = diario.IdTipoCbte,
                                IdCbteCble = diario.IdCbteCble,
                                secuencia = Secuencia++,
                                IdCtaCble = IdCtaCble_Anticipo,
                                dc_Valor = Math.Round(Convert.ToDouble(info.cr_Excedente ?? 0), 2, MidpointRounding.AwayFromZero) * -1,
                            });
                        }
                        #endregion

                        if (IdTipoMoviCaja != 0)
                        {
                            #region Movimiento de caja
                            caj_Caja_Movimiento mov_caja = Context_caj.caj_Caja_Movimiento.Where(q => q.IdEmpresa == relacion.ct_IdEmpresa && q.IdTipocbte == relacion.ct_IdTipoCbte && q.IdCbteCble == relacion.ct_IdCbteCble).FirstOrDefault();
                            if (mov_caja != null)
                            {
                                mov_caja.IdTipoMovi = IdTipoMoviCaja;
                                mov_caja.cm_fecha = diario.cb_Fecha;
                                mov_caja.cm_valor = diario.cb_Valor;
                                mov_caja.cm_observacion = diario.cb_Observacion;
                                mov_caja.IdPeriodo = diario.IdPeriodo;
                                mov_caja.IdCaja = info.IdCaja;
                                mov_caja.IdTipo_Persona = cl_enumeradores.eTipoPersona.CLIENTE.ToString();
                                mov_caja.IdEntidad = info.IdEntidad;
                                mov_caja.IdPersona = cliente.IdPersona;

                                mov_caja.IdUsuarioUltMod = info.IdUsuario;
                                mov_caja.Fecha_UltMod = DateTime.Now;
                            }
                            else
                            {
                                mov_caja = new caj_Caja_Movimiento
                                {
                                    IdEmpresa = diario.IdEmpresa,
                                    IdTipocbte = diario.IdTipoCbte,
                                    IdCbteCble = diario.IdCbteCble,
                                    IdTipoMovi = IdTipoMoviCaja,
                                    cm_fecha = diario.cb_Fecha,
                                    cm_valor = diario.cb_Valor,
                                    cm_Signo = "+",
                                    SecuenciaCaja = data_caj_movi.get_Secuencia(diario.IdEmpresa, info.IdCaja, "+"),
                                    cm_observacion = diario.cb_Observacion,
                                    Estado = "A",
                                    IdPeriodo = diario.IdPeriodo,
                                    IdCaja = info.IdCaja,
                                    IdTipo_Persona = cl_enumeradores.eTipoPersona.CLIENTE.ToString(),
                                    IdEntidad = info.IdEntidad,
                                    IdPersona = cliente.IdPersona,

                                    IdUsuario = info.IdUsuario,
                                    Fecha_Transac = DateTime.Now
                                };
                                Context_caj.caj_Caja_Movimiento.Add(mov_caja);
                            }

                            caj_Caja_Movimiento_det mov_caja_det = Context_caj.caj_Caja_Movimiento_det.Where(q => q.IdEmpresa == relacion.ct_IdEmpresa && q.IdTipocbte == relacion.ct_IdTipoCbte && q.IdCbteCble == relacion.ct_IdCbteCble).FirstOrDefault();
                            if (mov_caja_det != null)
                            {
                                mov_caja_det.IdEmpresa = diario.IdEmpresa;
                                mov_caja_det.IdTipocbte = diario.IdTipoCbte;
                                mov_caja_det.IdCbteCble = diario.IdCbteCble;
                                mov_caja_det.IdCobro_tipo = info.IdCobro_tipo;
                                mov_caja_det.cr_Valor = diario.cb_Valor;
                                mov_caja_det.Secuencia = 1;
                            }
                            else
                            {
                                mov_caja_det = new caj_Caja_Movimiento_det
                                {
                                    IdEmpresa = diario.IdEmpresa,
                                    IdTipocbte = diario.IdTipoCbte,
                                    IdCbteCble = diario.IdCbteCble,
                                    IdCobro_tipo = info.IdCobro_tipo,
                                    cr_Valor = diario.cb_Valor,
                                    Secuencia = 1
                                };
                                Context_caj.caj_Caja_Movimiento_det.Add(mov_caja_det);
                            }

                            #endregion
                        }
                    }
                    #endregion

                }
                #endregion

                Context_ct.SaveChanges();
                Context_cxc.SaveChanges();
                Context_caj.SaveChanges();

                Context_cxc.Dispose();
                Context_fac.Dispose();
                Context_caj.Dispose();
                Context_ct.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Context_cxc.Dispose();
                Context_fac.Dispose();
                Context_caj.Dispose();
                Context_ct.Dispose();
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "cxc_cobro_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }

        public bool anularDB(cxc_cobro_Info info)
        {
            try
            {
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var Entity = Context.cxc_cobro.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdCobro == info.IdCobro).FirstOrDefault();
                    if (Entity == null) return false;

                    if (Entity.cr_estado == "I") return true;

                    Entity.cr_estado = "I";
                    var cobros_det = Context.cxc_cobro_det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdCobro == info.IdCobro).ToList();
                    foreach (var item in cobros_det)
                    {
                        item.estado = "I";
                    }

                    var relacion = Context.cxc_cobro_x_ct_cbtecble.Where(q => q.cbr_IdEmpresa == info.IdEmpresa && q.cbr_IdSucursal == info.IdSucursal && q.cbr_IdCobro == info.IdCobro).FirstOrDefault();
                    if (relacion != null)
                    {
                        ct_cbtecble_Data odata_ct = new ct_cbtecble_Data();
                        if (odata_ct.anularDB(new ct_cbtecble_Info
                        {
                            IdEmpresa = relacion.ct_IdEmpresa,
                            IdTipoCbte = relacion.ct_IdTipoCbte,
                            IdCbteCble = relacion.ct_IdCbteCble,
                            IdUsuario = info.IdUsuarioUltAnu,
                            IdUsuarioAnu = info.IdUsuarioUltAnu
                        }))
                        {
                            if (Entity.IdCobro_tipo != null)
                            {
                                var cobro_tipo = Context.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == Entity.IdCobro_tipo).FirstOrDefault();

                                if (cobro_tipo.tc_Tomar_Cta_Cble_De == cl_enumeradores.eTipoCobroTomaCuentaDe.CAJA.ToString())
                                {
                                    caj_Caja_Movimiento_Data odata_caj = new caj_Caja_Movimiento_Data();
                                    odata_caj.anularDB(new Info.Caja.caj_Caja_Movimiento_Info
                                    {
                                        IdEmpresa = relacion.ct_IdEmpresa,
                                        IdTipocbte = relacion.ct_IdTipoCbte,
                                        IdCbteCble = relacion.ct_IdCbteCble,
                                        IdUsuario = info.IdUsuarioUltAnu,
                                        IdUsuario_Anu = info.IdUsuarioUltAnu
                                    });
                                }
                            }
                        }
                    }             

                    Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    Entity.MotiAnula = info.MotiAnula;
                    Entity.Fecha_UltAnu = DateTime.Now;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cxc_cobro_Info> get_list_para_retencion(int IdEmpresa, int IdSucursal, DateTime fecha_ini, DateTime fecha_fin, bool TieneRetencion)
        {
            try
            {
                List<cxc_cobro_Info> Lista;
                double Saldo = TieneRetencion ? -1 : 0;
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    Lista = (from q in Context.vwcxc_cobro_para_retencion
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && fecha_ini <= q.vt_fecha && q.vt_fecha <= fecha_fin
                             && q.TieneRetencion == TieneRetencion
                             && q.Saldo > Saldo
                             select new cxc_cobro_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdCbteVta = q.IdCbteVta,
                                 vt_tipoDoc = q.vt_tipoDoc,
                                 pe_nombreCompleto = q.Nombres,
                                 cr_fecha = q.vt_fecha,
                                 vt_NumFactura = q.vt_NumFactura,
                                 cr_observacion = q.vt_Observacion,
                                 vt_fecha = q.vt_fecha,
                                 vt_fech_venc = q.vt_fech_venc,
                                 vt_Subtotal = q.vt_Subtotal,
                                 vt_Iva = q.vt_iva,
                                 vt_Total = q.vt_total,
                                 Su_Descripcion = q.Su_Descripcion,
                                 Saldo = q.Saldo
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public cxc_cobro_Info get_info_para_retencion(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, string vt_tipoDoc)
        {
            try
            {
                cxc_cobro_Info info = new cxc_cobro_Info();
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    vwcxc_cobro_para_retencion Entity = Context.vwcxc_cobro_para_retencion.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdCbteVta == IdCbteVta && q.vt_tipoDoc == vt_tipoDoc);
                    if (Entity == null) return null;
                    info = new cxc_cobro_Info
                    {

                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdBodega = Entity.IdBodega,
                        IdCbteVta = Entity.IdCbteVta,
                        vt_Iva = Entity.vt_iva,
                        vt_Total =  Entity.vt_total,
                        pe_nombreCompleto = Entity.Nombres,
                        cr_fecha = Entity.vt_fecha,
                        vt_NumFactura = Entity.vt_NumFactura,
                        cr_observacion = Entity.vt_Observacion,
                        vt_Subtotal = Entity.vt_Subtotal,
                        IdCliente = Entity.IdCliente,
                        IdEntidad = Entity.IdCliente,
                        vt_tipoDoc = Entity.vt_tipoDoc,
                        cr_EsElectronico = Entity.cr_EsElectronico,
                        cr_EsProvision = Entity.cr_EsProvision,
                        NumeroAutorizacion = Entity.NumeroAutorizacion
                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ValidarSaldoDocumento(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, string CodDocumentoTipo, double ValorCobrado, double ValorAnterior)
        {
            try
            {
                string Retorno = string.Empty;

                using (Entities_cuentas_por_cobrar db = new Entities_cuentas_por_cobrar())
                {
                    var Documento = db.vwcxc_cartera_x_cobrar.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdComprobante == IdCbteVta && q.vt_tipoDoc == CodDocumentoTipo).FirstOrDefault();
                    if (Documento != null)
                    {
                        if (Math.Round((Documento.Saldo ?? 0) + ValorAnterior - ValorCobrado, 2, MidpointRounding.AwayFromZero) < 0)
                        {
                            Retorno += ("Está intentando aplicar " + ValorCobrado.ToString("c2") + " al documento " + Documento.vt_NunDocumento + " cuyo saldo es de " + (Documento.Saldo ?? 0 + ValorAnterior).ToString("c2"));
                        }
                    }
                }

                return Retorno;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<cxc_cobro_Info> get_Saldo(int IdEmpresa, decimal IdCliente)
        {
            try
            {
                cxc_cobro_Info info = new cxc_cobro_Info();
                List<cxc_cobro_Info> Lista = new List<cxc_cobro_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT cabfac.IdEmpresa, cabfac.IdSucursal, cabfac.IdBodega, cabfac.vt_tipoDoc, cabfac.vt_serie1 + '-' + cabfac.vt_serie2 + '-' + cabfac.vt_NumFactura AS vt_NunDocumento, cabfac.vt_Observacion AS Referencia, "
                    + " cabfac.IdCbteVta AS IdComprobante, cabfac.CodCbteVta AS CodComprobante, Sucu.Su_Descripcion, cabfac.IdCliente, cabfac.vt_fecha, CAST(detfac.Total AS FLOAT) AS vt_total, "
                    + " ROUND(detfac.Total - ROUND(ISNULL(vwcxc_total_cobros_x_Docu.dc_ValorPago, 0), 2), 2) AS Saldo, ISNULL(vwcxc_total_cobros_x_Docu.dc_ValorPago, 0) AS TotalxCobrado, Bod.bo_Descripcion AS Bodega, "
                    + " CAST(detfac.SubtotalConDscto AS FLOAT) AS vt_Subtotal, CAST(detfac.ValorIVA AS FLOAT) AS vt_iva, cabfac.vt_fech_venc, Cli.Codigo AS CodCliente, tb_persona.pe_nombreCompleto AS NomCliente, tb_empresa.em_nombre, "
                    + " cabfac.Estado "
                    + " FROM     fa_factura_resumen AS detfac INNER JOIN "
                    + " fa_factura AS cabfac ON detfac.IdBodega = cabfac.IdBodega AND detfac.IdSucursal = cabfac.IdSucursal AND detfac.IdEmpresa = cabfac.IdEmpresa AND detfac.IdCbteVta = cabfac.IdCbteVta INNER JOIN "
                    + " tb_sucursal AS Sucu ON cabfac.IdEmpresa = Sucu.IdEmpresa AND cabfac.IdSucursal = Sucu.IdSucursal INNER JOIN "
                    + " tb_bodega AS Bod ON cabfac.IdEmpresa = Bod.IdEmpresa AND cabfac.IdSucursal = Bod.IdSucursal AND cabfac.IdBodega = Bod.IdBodega AND Sucu.IdEmpresa = Bod.IdEmpresa AND Sucu.IdSucursal = Bod.IdSucursal INNER JOIN "
                    + " fa_cliente AS Cli ON cabfac.IdEmpresa = Cli.IdEmpresa AND cabfac.IdCliente = Cli.IdCliente INNER JOIN "
                    + " tb_persona ON Cli.IdPersona = tb_persona.IdPersona INNER JOIN "
                    + " tb_empresa ON cabfac.IdEmpresa = tb_empresa.IdEmpresa LEFT OUTER JOIN "
                    + " vwcxc_total_cobros_x_Docu ON cabfac.IdEmpresa = vwcxc_total_cobros_x_Docu.IdEmpresa AND cabfac.IdSucursal = vwcxc_total_cobros_x_Docu.IdSucursal AND cabfac.IdBodega = vwcxc_total_cobros_x_Docu.IdBodega_Cbte AND "
                    + " cabfac.vt_tipoDoc = vwcxc_total_cobros_x_Docu.dc_TipoDocumento AND cabfac.IdCbteVta = vwcxc_total_cobros_x_Docu.IdCbte_vta_nota "
                    + " WHERE cabfac.IdEmpresa = " + IdEmpresa.ToString() + " and cabfac.IdCliente = " + IdCliente.ToString() + " and cabfac.Estado = 'A' "
                    + " UNION "
                    + " SELECT A.IdEmpresa, A.IdSucursal, A.IdBodega, 'NTDB' AS CreDeb, CASE WHEN A.NaturalezaNota = 'SRI' THEN A.Serie1 + '-' + A.Serie2 + '-' + A.NumNota_Impresa ELSE ISNULL(A.CodNota, cast(a.IdNota AS varchar(20))) END AS Documento, "
                    + " A.sc_observacion, A.IdNota, A.CodNota, su.Su_Descripcion, A.IdCliente, A.no_fecha, ROUND(B.Total, 2) AS sc_total, ROUND(B.Total - ISNULL(CB.dc_ValorPago, 0), 2) AS Saldo, ISNULL(CB.dc_ValorPago, 0) AS totalCobrado, "
                    + " Bo.bo_Descripcion, ROUND(B.Total, 2) AS sc_subtotal, ROUND(B.ValorIVA, 2) AS sc_iva, A.no_fecha_venc, Cli.Codigo AS CodCliente, tb_persona.pe_nombreCompleto, tb_empresa.em_nombre, A.Estado "
                    + " FROM     fa_notaCreDeb AS A INNER JOIN "
                    + " fa_notaCreDeb_resumen AS B ON A.IdEmpresa = B.IdEmpresa AND A.IdSucursal = B.IdSucursal AND A.IdBodega = B.IdBodega AND A.IdNota = B.IdNota INNER JOIN "
                    + " tb_bodega AS Bo ON A.IdEmpresa = Bo.IdEmpresa AND A.IdSucursal = Bo.IdSucursal AND A.IdBodega = Bo.IdBodega INNER JOIN "
                    + " tb_sucursal AS su ON Bo.IdEmpresa = su.IdEmpresa AND Bo.IdSucursal = su.IdSucursal INNER JOIN "
                    + " fa_cliente AS Cli ON A.IdEmpresa = Cli.IdEmpresa AND A.IdCliente = Cli.IdCliente INNER JOIN "
                    + " tb_persona ON Cli.IdPersona = tb_persona.IdPersona INNER JOIN "
                    + " tb_empresa ON A.IdEmpresa = tb_empresa.IdEmpresa LEFT OUTER JOIN "
                    + " vwcxc_total_cobros_x_Docu AS CB ON A.IdEmpresa = CB.IdEmpresa AND A.IdSucursal = CB.IdSucursal AND A.IdBodega = CB.IdBodega_Cbte AND A.IdNota = CB.IdCbte_vta_nota AND "
                    + " A.CodDocumentoTipo = CB.dc_TipoDocumento "
                    + " WHERE A.IdEmpresa = " + IdEmpresa.ToString() + " and A.IdCliente = " + IdCliente.ToString() + " and A.Estado = 'A' AND A.CreDeb = 'D' AND NOT EXISTS "
                          + " (SELECT * "
                          + " FROM      fa_notaCreDeb_x_fa_factura_NotaDeb Cruce "
                          + " WHERE   Cruce.IdEmpresa_nt = A.IdEmpresa AND Cruce.IdSucursal_nt = A.IdSucursal AND Cruce.IdBodega_nt = A.IdBodega AND Cruce.IdNota_nt = A.IdNota AND Cruce.Valor_Aplicado <> 0)";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new cxc_cobro_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdCliente = Convert.ToDecimal(reader["IdCliente"]),
                            Saldo = Convert.ToDouble(reader["Saldo"]),
                            vt_Total = Convert.ToDecimal(reader["vt_Total"])
                        });
                    }
                    reader.Close();
                    return Lista;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public fa_Dashboard_Info CobrosPorDia(int IdEmpresa, DateTime Fecha)
        {
            try
            {
                fa_Dashboard_Info info = new fa_Dashboard_Info();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    //connection.Open();
                    //SqlCommand command = new SqlCommand("", connection);
                    //command.CommandText = "declare @IdEmpresa int = " + IdEmpresa + ", "
                    //+ " @Fecha date = DATEFROMPARTS(" + Fecha + ", " + Fecha.Month + ", " + Fecha.Day + "),"
                    //+ " select SUM(cr_TotalCobro) Total"
                    //+ " from cxc_cobro a"
                    //+ " where a.IdEmpresa = @IdEmpresa and a.vt_fecha = @Fecha and a.Estado = 'A' ";

                    //var ResultValue = command.ExecuteScalar();

                    //if (ResultValue == null)
                    //    return null;

                    //SqlDataReader reader = command.ExecuteReader();

                    //while (reader.Read())
                    //{
                    //    info = new fa_Dashboard_Info
                    //    {
                    //        Total = Convert.ToDecimal(reader["Total"]),
                    //    };
                    //}
                }

                //info.Total_String = info.Total.ToString("C2");
                return info;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "get_list_UltimasVentasAnio", IdUsuario = "consulta" });
                return new fa_Dashboard_Info();
            }
        }
    }
}
