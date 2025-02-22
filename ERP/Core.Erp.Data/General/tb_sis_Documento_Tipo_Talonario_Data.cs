﻿using Core.Erp.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.General
{
    public class tb_sis_Documento_Tipo_Talonario_Data
    {
        public List<tb_sis_Documento_Tipo_Talonario_Info> get_list(int IdEmpresa, int IdSucursal,string CodDocumentoTipo,  bool mostrar_anulados)
        {
            try
            {
                int IdSucursal_ini = IdSucursal;
                int IdSucursal_fin = IdSucursal == 0 ? 9999 : IdSucursal;
                
                List<tb_sis_Documento_Tipo_Talonario_Info> Lista;
                using (Entities_general Context = new Entities_general())
                {
                            Lista = Context.tb_sis_Documento_Tipo_Talonario.Where(q=> q.IdEmpresa == IdEmpresa
                                      && IdSucursal_ini <= q.IdSucursal
                                      && q.IdSucursal <= IdSucursal_fin
                                      && q.CodDocumentoTipo == (string.IsNullOrEmpty(CodDocumentoTipo) ? q.CodDocumentoTipo : CodDocumentoTipo) 
                                      && q.Estado ==(mostrar_anulados ? q.Estado : "A")).Select(q=> new tb_sis_Documento_Tipo_Talonario_Info
                                     {
                                         CodDocumentoTipo = q.CodDocumentoTipo,
                                         IdSucursal = q.IdSucursal,
                                         IdEmpresa = q.IdEmpresa,
                                         Establecimiento = q.Establecimiento,
                                         Estado = q.Estado,
                                         NumAutorizacion = q.NumAutorizacion,
                                         NumDocumento = q.NumDocumento,
                                         PuntoEmision = q.PuntoEmision,
                                         EstadoBool = q.Estado == "A" ? true : false

                                     }).ToList();                    
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tb_sis_Documento_Tipo_Talonario_Info get_info(int IdEmpresa, string CodDocumentoTipo, string Establecimiento, string PuntoEmision, string NumDocumento)
        {
            try
            {
                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info();
                using (Entities_general Context = new Entities_general())
                {
                    tb_sis_Documento_Tipo_Talonario Entity = Context.tb_sis_Documento_Tipo_Talonario.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.CodDocumentoTipo == CodDocumentoTipo && q.Establecimiento == Establecimiento && q.PuntoEmision == PuntoEmision && q.NumDocumento == NumDocumento);
                    if (Entity == null) return null;
                    info = new tb_sis_Documento_Tipo_Talonario_Info
                    {
                        CodDocumentoTipo = Entity.CodDocumentoTipo,
                        IdSucursal = Entity.IdSucursal,
                        IdEmpresa = Entity.IdEmpresa,
                        Establecimiento = Entity.Establecimiento,
                        Estado = Entity.Estado,
                        es_Documento_Electronico = Entity.es_Documento_Electronico==null ? false:Convert.ToBoolean(Entity.es_Documento_Electronico),
                        FechaCaducidad = Entity.FechaCaducidad,
                        NumAutorizacion = Entity.NumAutorizacion,
                        NumDocumento = Entity.NumDocumento,
                        PuntoEmision = Entity.PuntoEmision,
                        Usado = Entity.Usado == null ? false : Convert.ToBoolean(Entity.Usado)

                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(tb_sis_Documento_Tipo_Talonario_Info info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    tb_sis_Documento_Tipo_Talonario Entity = new tb_sis_Documento_Tipo_Talonario
                    {
                        CodDocumentoTipo = info.CodDocumentoTipo,
                        IdSucursal = info.IdSucursal,
                        IdEmpresa = info.IdEmpresa,
                        Establecimiento = info.Establecimiento,
                        Estado = info.Estado="A",
                        es_Documento_Electronico = info.es_Documento_Electronico,
                        FechaCaducidad = info.FechaCaducidad,
                        NumAutorizacion = info.NumAutorizacion,
                        NumDocumento = info.NumDocumento,
                        PuntoEmision = info.PuntoEmision,
                        Usado = info.Usado
                        
                    };
                    Context.tb_sis_Documento_Tipo_Talonario.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception )
            {

                throw;
            }
        }
        public bool modificarDB(tb_sis_Documento_Tipo_Talonario_Info info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    tb_sis_Documento_Tipo_Talonario Entity = Context.tb_sis_Documento_Tipo_Talonario.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.CodDocumentoTipo == info.CodDocumentoTipo && q.Establecimiento == info.Establecimiento && q.PuntoEmision == info.PuntoEmision && q.NumDocumento == info.NumDocumento);
                    if (Entity == null) return false;

                    
                    Entity.es_Documento_Electronico = info.es_Documento_Electronico;
                    Entity.FechaCaducidad = info.FechaCaducidad;
                    Entity.NumAutorizacion = info.NumAutorizacion;
                    Entity.Usado = info.Usado;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool anularDB(tb_sis_Documento_Tipo_Talonario_Info info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    tb_sis_Documento_Tipo_Talonario Entity = Context.tb_sis_Documento_Tipo_Talonario.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.CodDocumentoTipo == info.CodDocumentoTipo && q.Establecimiento == info.Establecimiento && q.PuntoEmision == info.PuntoEmision && q.NumDocumento == info.NumDocumento);
                    if (Entity == null) return false;
                    Entity.Estado = info.Estado = "I";
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// crear una funcion donde recibe parametro idEmpresa, CodDocumentotipo, establecimiento y pto emision, select a la base devuelve max.numdoc
        public string get_NumeroDocumentoInicial (int IdEmpresa, string CodDcumentoTipo, string Establecimiento, string PuntoEmision)
        {
            try
            {
                string NumeroDocumento = "000000001";
                using (Entities_general Context = new Entities_general())
                {
                    var lst = from q in Context.tb_sis_Documento_Tipo_Talonario
                              where q.IdEmpresa == IdEmpresa
                              && q.CodDocumentoTipo == CodDcumentoTipo
                              && q.PuntoEmision == PuntoEmision
                              && q.Establecimiento == Establecimiento
                              select q;
                    if (lst.Count() > 0)
                    {
                        NumeroDocumento = lst.Max(q => q.NumDocumento) + "";
                        double NumeroDocumento_double = Convert.ToDouble(NumeroDocumento) + 1;
                        NumeroDocumento = NumeroDocumento_double.ToString("000000000");
                    }         
                }
                return NumeroDocumento;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificar_estado_usadoDB(tb_sis_Documento_Tipo_Talonario_Info info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    tb_sis_Documento_Tipo_Talonario Entity = Context.tb_sis_Documento_Tipo_Talonario.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.CodDocumentoTipo == info.CodDocumentoTipo && q.Establecimiento == info.Establecimiento && q.PuntoEmision == info.PuntoEmision && q.NumDocumento == info.NumDocumento);
                    if (Entity == null) return false;
                    Entity.Usado = info.Usado;
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tb_sis_Documento_Tipo_Talonario_Info GetUltimoNoUsado(int IdEmpresa, string CodDocumentoTipo, string Establecimiento, string PuntoEmision, bool EsDocumentoElectronico, bool Actualizar)
        {
            try
            {
                DateTime Fecha = DateTime.Now.Date;
                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info();
                if (EsDocumentoElectronico)
                    Fecha = new DateTime(2000, 1, 1);
                using (Entities_general db = new Entities_general())
                {
                    var q = (from A in db.tb_sis_Documento_Tipo_Talonario
                             where A.IdEmpresa == IdEmpresa
                             && A.CodDocumentoTipo == CodDocumentoTipo
                             && A.Establecimiento == Establecimiento
                             && A.PuntoEmision == PuntoEmision
                             && A.Usado == false
                             && A.Estado == "A"
                             && A.es_Documento_Electronico == EsDocumentoElectronico
                             && Fecha <= A.FechaCaducidad
                             select A.NumDocumento).Min();
                    if (q != null)
                    {

                        string UltRegistro = q.ToString();
                        var Entity = db.tb_sis_Documento_Tipo_Talonario.Where(v => v.IdEmpresa == IdEmpresa && v.CodDocumentoTipo == CodDocumentoTipo && v.Establecimiento == Establecimiento && v.PuntoEmision == PuntoEmision && v.Estado == "A" && v.Usado == false && v.es_Documento_Electronico == EsDocumentoElectronico).FirstOrDefault();
                        if (Entity != null)
                        {
                            if (Actualizar)
                            {
                                Entity.Usado = true;
                                db.SaveChanges();
                            }
                            info = new tb_sis_Documento_Tipo_Talonario_Info
                            {
                                IdEmpresa = Entity.IdEmpresa,
                                Establecimiento = Entity.Establecimiento,
                                PuntoEmision = Entity.PuntoEmision,
                                NumDocumento = Entity.NumDocumento,
                                CodDocumentoTipo = Entity.CodDocumentoTipo,
                                es_Documento_Electronico = Entity.es_Documento_Electronico ?? false,
                                FechaCaducidad = Entity.FechaCaducidad,
                                NumAutorizacion = Entity.NumAutorizacion                               
                            };
                        }
                    }
                }

                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tb_sis_Documento_Tipo_Talonario_Info> get_list_actualizacion_masiva(int IdEmpresa, int IdSucursal, string CodDocumentoTipo, string Establecimiento, string PuntoEmision, int NumIni, int NumFin)
        {
            try
            {
                List<tb_sis_Documento_Tipo_Talonario_Info> Lista;
                using (Entities_general Context = new Entities_general())
                {
                    Lista = Context.vwtb_sis_Documento_Tipo_Talonario.Where(q => q.IdEmpresa == IdEmpresa
                              && q.IdSucursal == IdSucursal
                              && q.CodDocumentoTipo == CodDocumentoTipo
                              && q.Establecimiento == Establecimiento
                              && q.PuntoEmision == PuntoEmision
                              && q.NumDocumentoInt >= NumIni
                              && q.NumDocumentoInt <= NumFin).Select(q => new tb_sis_Documento_Tipo_Talonario_Info
                              {
                                  CodDocumentoTipo = q.CodDocumentoTipo,
                                  IdSucursal = q.IdSucursal,
                                  IdEmpresa = q.IdEmpresa,
                                  Establecimiento = q.Establecimiento,
                                  Estado = q.Estado,
                                  NumAutorizacion = q.NumAutorizacion,
                                  NumDocumento = q.NumDocumento,
                                  PuntoEmision = q.PuntoEmision,
                                  FechaCaducidad = q.FechaCaducidad,
                                  EstadoBool = q.Estado == "A" ? true : false

                              }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificacionMasivaDB(List<tb_sis_Documento_Tipo_Talonario_Info> info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    foreach (var item in info)
                    {
                        tb_sis_Documento_Tipo_Talonario Entity = Context.tb_sis_Documento_Tipo_Talonario.FirstOrDefault(q => q.IdEmpresa == item.IdEmpresa 
                        && q.CodDocumentoTipo == item.CodDocumentoTipo && q.Establecimiento == item.Establecimiento && q.PuntoEmision == item.PuntoEmision 
                        && q.NumDocumento == item.NumDocumento);
                        if (Entity == null) return false;

                        if (item.FechaCaducidad != null)
                        {
                            Entity.FechaCaducidad = item.FechaCaducidad;
                        }

                        if (item.NumAutorizacion != null || item.NumAutorizacion !="")
                        {
                            Entity.NumAutorizacion = item.NumAutorizacion;
                        }

                        Entity.es_Documento_Electronico = item.es_Documento_Electronico;
                        Entity.Usado = item.Usado;
                        Entity.Estado = item.Estado;

                        Context.SaveChanges();
                    }
                    
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <param name="fechaEmision">Fecha documento</param>
        /// <param name="tipoComprobante">01 Factura, 03 Liquidación, 04 Nota de credito, 05 Nota de debito, 06 Guia de remisión, 07 Retención</param>
        /// <param name="ruc">RUC Empresa</param>
        /// <param name="serie">Establecimiento y punto de emisión (6 dígitos)</param>
        /// <param name="numeroComprobante">9 digitos del secuencial</param>
        /// <param name="codigoNumerico">Puede no asignarse</param>
        /// <param name="tipoEmision">Puede no asignarse</param>
        /// <param name="ambiente">Puede no asignarse</param>
        /// <returns></returns>
        public string GeneraClaveAcceso(DateTime fechaEmision, string tipoComprobante, string ruc, string serie, string numeroComprobante, string codigoNumerico = "12345678", string tipoEmision = "1", string ambiente = "2")
        {
            try
            {
                if ((ruc != null) && (ruc.Length < 13))
                {
                    ruc = String.Format("%013d", new Object[] { ruc });
                }
                String fecha = fechaEmision.ToString("ddMMyyyy");
                StringBuilder clave = new StringBuilder(fecha);
                clave.Append(tipoComprobante);
                clave.Append(ruc);
                clave.Append(ambiente);
                clave.Append(serie);
                clave.Append(numeroComprobante);
                clave.Append(codigoNumerico);
                clave.Append(tipoEmision);

                int digitoVer = GeneraDigitoVerificadorModulo11(clave.ToString());
                
                return clave.ToString() + digitoVer.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GeneraDigitoVerificadorModulo11(string cadena)
        {
            try
            {
                int baseMultiplicador = 7;

                int[] aux = new int[cadena.Length];
                int multiplicador = 2;
                int total = 0;
                int verificador = 0;
                for (int i = aux.Length - 1; i >= 0; i--)
                {
                    aux[i] = Convert.ToInt32(cadena[i].ToString());
                    aux[i] *= multiplicador;
                    multiplicador++;
                    if (multiplicador > baseMultiplicador)
                    {
                        multiplicador = 2;
                    }
                    total += aux[i];
                }
                if ((total == 0) || (total == 1))
                {
                    verificador = 0;
                }
                else
                {
                    verificador = 11 - total % 11 == 11 ? 0 : 11 - total % 11;
                }
                if (verificador == 10)
                {
                    verificador = 1;
                }
                return verificador;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
