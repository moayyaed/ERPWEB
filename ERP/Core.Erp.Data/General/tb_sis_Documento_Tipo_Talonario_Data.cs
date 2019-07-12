using Core.Erp.Info.General;
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
                    if (mostrar_anulados)
                    {
                        if (!string.IsNullOrEmpty(CodDocumentoTipo))
                            Lista = (from q in Context.tb_sis_Documento_Tipo_Talonario
                                     where q.IdEmpresa == IdEmpresa
                                      && IdSucursal_ini <= q.IdSucursal
                                      && q.IdSucursal <= IdSucursal_fin
                                      && q.CodDocumentoTipo == CodDocumentoTipo
                                     select new tb_sis_Documento_Tipo_Talonario_Info
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
                        else
                            Lista = (from q in Context.tb_sis_Documento_Tipo_Talonario
                                     where q.IdEmpresa == IdEmpresa
                                      && IdSucursal_ini <= q.IdSucursal
                                      && q.IdSucursal <= IdSucursal_fin
                                      && q.CodDocumentoTipo == CodDocumentoTipo
                                     select new tb_sis_Documento_Tipo_Talonario_Info
                                     {
                                         CodDocumentoTipo = q.CodDocumentoTipo,
                                         IdSucursal = q.IdSucursal,
                                         IdEmpresa = q.IdEmpresa,
                                         Establecimiento = q.Establecimiento,
                                         Estado = q.Estado,
                                         FechaCaducidad = q.FechaCaducidad,
                                         NumAutorizacion = q.NumAutorizacion,
                                         NumDocumento = q.NumDocumento,
                                         PuntoEmision = q.PuntoEmision,

                                         EstadoBool = q.Estado == "A" ? true : false

                                     }).ToList();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CodDocumentoTipo))
                            Lista = (from q in Context.tb_sis_Documento_Tipo_Talonario
                                     where q.IdEmpresa == IdEmpresa
                                      && IdSucursal_ini <= q.IdSucursal
                                      && q.IdSucursal <= IdSucursal_fin
                                      && q.CodDocumentoTipo == CodDocumentoTipo
                                     select new tb_sis_Documento_Tipo_Talonario_Info
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
                        else
                            Lista = (from q in Context.tb_sis_Documento_Tipo_Talonario
                                     where q.IdEmpresa == IdEmpresa
                                     && q.Estado == "A"
                                     select new tb_sis_Documento_Tipo_Talonario_Info
                                     {
                                         CodDocumentoTipo = q.CodDocumentoTipo,
                                         IdSucursal = q.IdSucursal,
                                         IdEmpresa = q.IdEmpresa,
                                         Establecimiento = q.Establecimiento,
                                         Estado = q.Estado,
                                         FechaCaducidad = q.FechaCaducidad,
                                         NumAutorizacion = q.NumAutorizacion,
                                         NumDocumento = q.NumDocumento,
                                         PuntoEmision = q.PuntoEmision,

                                         EstadoBool = q.Estado == "A" ? true : false

                                     }).ToList();

                    }
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
        public tb_sis_Documento_Tipo_Talonario_Info get_info_ultimo_no_usado(int IdEmpresa, string CodDocumentoTipo, int IdSucursal, string Su_CodigoEstablecimiento)
        {            
            try
            {
                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info();
                using (Entities_general Context = new Entities_general())
                {
                    var q = (from A in Context.tb_sis_Documento_Tipo_Talonario
                             where A.IdEmpresa == IdEmpresa
                             && A.CodDocumentoTipo == CodDocumentoTipo
                             && A.IdSucursal==IdSucursal
                             && A.Establecimiento == Su_CodigoEstablecimiento
                             && A.Usado == false
                             && A.Estado == "A"
                             select A.NumDocumento).Min();
                    if (q != null)
                    {
                        string UltRegistro = q.ToString();
                        var querry = from A in Context.tb_sis_Documento_Tipo_Talonario
                                     where A.IdEmpresa == IdEmpresa
                                     //&& A.PuntoEmision == puntoemision 
                                     && A.Establecimiento == Su_CodigoEstablecimiento
                                     && A.CodDocumentoTipo == CodDocumentoTipo
                                     && A.Usado == false
                                     && A.NumDocumento == UltRegistro
                                     select A;

                        foreach (var item in querry)
                        {
                            info.IdEmpresa = item.IdEmpresa;
                            info.IdSucursal = item.IdSucursal;
                            info.CodDocumentoTipo = item.CodDocumentoTipo;
                            info.Establecimiento = item.Establecimiento;
                            info.Estado = item.Estado;
                            info.FechaCaducidad = item.FechaCaducidad;
                            info.NumAutorizacion = item.NumAutorizacion;
                            info.NumDocumento = item.NumDocumento;
                            info.PuntoEmision = item.PuntoEmision;
                            info.Usado =(bool) item.Usado;
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
        public tb_sis_Documento_Tipo_Talonario_Info get_info_ultimo_no_usado(int IdEmpresa, string Establecimiento, string PuntoEmision, string CodDocumentoTipo)
        {
            try
            {
                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info();
                using (Entities_general Context = new Entities_general())
                {
                    var q = (from A in Context.tb_sis_Documento_Tipo_Talonario
                             where A.IdEmpresa == IdEmpresa
                             && A.CodDocumentoTipo == CodDocumentoTipo
                             && A.Establecimiento == Establecimiento
                             && A.PuntoEmision == PuntoEmision
                             && A.Usado == false
                             && A.Estado == "A"
                             select A.NumDocumento).Min();
                    if (q != null)
                    {
                        string UltRegistro = q.ToString();
                        var Entity = Context.tb_sis_Documento_Tipo_Talonario.Where(v => v.IdEmpresa == IdEmpresa && v.CodDocumentoTipo == CodDocumentoTipo && v.Establecimiento == Establecimiento && v.PuntoEmision == PuntoEmision && v.Estado == "A" && v.Usado == false).FirstOrDefault();
                        if (Entity != null)
                            info = new tb_sis_Documento_Tipo_Talonario_Info
                            {
                                IdEmpresa = Entity.IdEmpresa,
                                Establecimiento = Entity.Establecimiento,
                                PuntoEmision = Entity.PuntoEmision,
                                NumDocumento = Entity.NumDocumento                                
                            };
                    }
                }
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public tb_sis_Documento_Tipo_Talonario_Info get_info_ultimo_no_usado_electronico(int IdEmpresa, string Establecimiento, string PuntoEmision, string CodDocumentoTipo)
        {
            try
            {
                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info();
                using (Entities_general Context = new Entities_general())
                {
                    var q = (from A in Context.tb_sis_Documento_Tipo_Talonario
                             where A.IdEmpresa == IdEmpresa
                             && A.CodDocumentoTipo == CodDocumentoTipo
                             && A.Establecimiento == Establecimiento
                             && A.PuntoEmision == PuntoEmision
                             && A.Usado == false
                             && A.Estado == "A"
                             && A.es_Documento_Electronico == true
                             select A.NumDocumento).Min();
                    if (q != null)
                    {
                        string UltRegistro = q.ToString();
                        var Entity = Context.tb_sis_Documento_Tipo_Talonario.Where(v => v.IdEmpresa == IdEmpresa && v.CodDocumentoTipo == CodDocumentoTipo && v.Establecimiento == Establecimiento && v.PuntoEmision == PuntoEmision && v.Estado == "A" && v.Usado == false && v.es_Documento_Electronico == true).FirstOrDefault();
                        if (Entity != null)
                            info = new tb_sis_Documento_Tipo_Talonario_Info
                            {
                                IdEmpresa = Entity.IdEmpresa,
                                Establecimiento = Entity.Establecimiento,
                                PuntoEmision = Entity.PuntoEmision,
                                NumDocumento = Entity.NumDocumento
                            };
                    }
                }
                return info;
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
        
        public tb_sis_Documento_Tipo_Talonario_Info GetUltimoNoUsadoFacElec(int IdEmpresa, string CodDocumentoTipo, string Establecimiento, string PuntoEmision)
        {
            try
            {
                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info();

                using (Entities_general db = new Entities_general())
                {
                    var q = (from A in db.tb_sis_Documento_Tipo_Talonario
                             where A.IdEmpresa == IdEmpresa
                             && A.CodDocumentoTipo == CodDocumentoTipo
                             && A.Establecimiento == Establecimiento
                             && A.PuntoEmision == PuntoEmision
                             && A.Usado == false
                             && A.Estado == "A"
                             //&& A.es_Documento_Electronico == true
                             select A.NumDocumento).Min();
                    if (q != null)
                    {
                        string UltRegistro = q.ToString();
                        var Entity = db.tb_sis_Documento_Tipo_Talonario.Where(v => v.IdEmpresa == IdEmpresa && v.CodDocumentoTipo == CodDocumentoTipo && v.Establecimiento == Establecimiento && v.PuntoEmision == PuntoEmision && v.Estado == "A" && v.Usado == false).FirstOrDefault();
                        if (Entity != null)
                        {
                            Entity.Usado = true;
                            db.SaveChanges();
                            info = new tb_sis_Documento_Tipo_Talonario_Info
                            {
                                IdEmpresa = Entity.IdEmpresa,
                                Establecimiento = Entity.Establecimiento,
                                PuntoEmision = Entity.PuntoEmision,
                                NumDocumento = Entity.NumDocumento,
                                CodDocumentoTipo = Entity.CodDocumentoTipo,                                
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
    }
}
