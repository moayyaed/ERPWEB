using Core.Erp.Data.General;
using Core.Erp.Info.CuentasPorPagar;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Erp.Data.CuentasPorPagar
{
    public class cp_proveedor_Data
    {
        public List<cp_proveedor_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<cp_proveedor_Info> Lista = new List<cp_proveedor_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    string query = "select a.IdEmpresa, a.IdProveedor, a.pr_estado, c.descripcion_clas_prove, a.IdPersona, b.pe_nombreCompleto, b.pe_cedulaRuc, case when a.pr_estado = 'A' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS EstadoBool"
                                +" from cp_proveedor as a inner join"
                                +" tb_persona as b on a.IdPersona = b.IdPersona left join"
                                +" cp_proveedor_clase as c on a.IdEmpresa = c.IdEmpresa and a.IdClaseProveedor = c.IdClaseProveedor"
                                +" where a.IdEmpresa = "+IdEmpresa.ToString()+" "+(mostrar_anulados ? "" : "and a.pr_estado = 'A'")+" order by b.pe_nombreCompleto";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new cp_proveedor_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdProveedor = Convert.ToDecimal(reader["IdProveedor"]),
                            pr_estado = Convert.ToString(reader["pr_estado"]),
                            descripcion_clas_prove = Convert.ToString(reader["descripcion_clas_prove"]),
                            IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                            EstadoBool = Convert.ToBoolean(reader["EstadoBool"]),
                            info_persona = new Info.General.tb_persona_Info
                            {
                                IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                                pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                                pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"])
                            }
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

        public cp_proveedor_Info get_info(int IdEmpresa, decimal IdProveedor)
        {
            try
            {
                cp_proveedor_Info info = new cp_proveedor_Info();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    string query = "select a.IdEmpresa, a.IdProveedor,a.IdPersona, a.IdClaseProveedor, a.IdCiudad, a.IdBanco_acreditacion,"
                                +" a.IdCtaCble_CXP, a.IdCtaCble_Gasto,a.IdTipoCta_acreditacion_cat, case when a.pr_contribuyenteEspecial = 'S' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END pr_contribuyenteEspecial_bool,"
                                +" a.es_empresa_relacionada, a.num_cta_acreditacion, a.pr_codigo, a.pr_plazo, a.pr_estado, a.pr_correo, a.pr_direccion,"
                                +" a.pr_telefonos, a.pr_celular, a.IdCtaCble_Anticipo,"
                                +" b.pe_apellido, b.pe_nombre, b.pe_cedulaRuc, b.pe_nombreCompleto, b.pe_razonSocial, b.pe_Naturaleza, b.IdTipoDocumento,"
                                +" case when c.Ruc is null then 0 else 1 end as EsMicroEmpresa"
                                +" from cp_proveedor as a inner join"
                                +" tb_persona as b on a.IdPersona = b.IdPersona left join"
                                +" cp_proveedor_microempresa as c on b.pe_cedulaRuc = c.Ruc"
                                +" where a.IdEmpresa = "+IdEmpresa.ToString()+" and a.IdProveedor = "+IdProveedor.ToString();
                    SqlCommand command = new SqlCommand(query,connection);
                    var ValidateValue = command.ExecuteScalar();
                    if (ValidateValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        info = new cp_proveedor_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdProveedor = Convert.ToDecimal(reader["IdProveedor"]),
                            IdClaseProveedor = Convert.ToInt32(reader["IdClaseProveedor"]),
                            IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                            IdCiudad = Convert.ToString(reader["IdCiudad"]),
                            IdBanco_acreditacion = string.IsNullOrEmpty(reader["IdBanco_acreditacion"].ToString()) ? null : (int?)(reader["IdBanco_acreditacion"]),
                            IdCtaCble_CXP = Convert.ToString(reader["IdCtaCble_CXP"]),
                            IdCtaCble_Gasto = Convert.ToString(reader["IdCtaCble_Gasto"]),
                            IdTipoCta_acreditacion_cat = Convert.ToString(reader["IdTipoCta_acreditacion_cat"]),
                            pr_contribuyenteEspecial_bool = Convert.ToBoolean(reader["pr_contribuyenteEspecial_bool"]),
                            es_empresa_relacionada = Convert.ToBoolean(reader["es_empresa_relacionada"]),
                            num_cta_acreditacion = Convert.ToString(reader["num_cta_acreditacion"]),
                            pr_codigo = Convert.ToString(reader["pr_codigo"]),
                            pr_plazo = Convert.ToInt32(reader["pr_plazo"]),
                            pr_estado = Convert.ToString(reader["pr_estado"]),
                            pr_correo = Convert.ToString(reader["pr_correo"]),
                            pr_direccion = Convert.ToString(reader["pr_direccion"]),
                            pr_telefonos = Convert.ToString(reader["pr_telefonos"]),
                            pr_celular = Convert.ToString(reader["pr_celular"]),
                            IdCtaCble_Anticipo = Convert.ToString(reader["IdCtaCble_Anticipo"]),
                            EsMicroEmpresa = Convert.ToInt32(reader["EsMicroEmpresa"]),
                            info_persona = new Info.General.tb_persona_Info
                            {
                                pe_apellido = Convert.ToString(reader["pe_apellido"]),
                                pe_nombre = Convert.ToString(reader["pe_nombre"]),
                                pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"]),
                                pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                                pe_razonSocial = Convert.ToString(reader["pe_razonSocial"]),
                                pe_Naturaleza = Convert.ToString(reader["pe_Naturaleza"]),
                                IdTipoDocumento = Convert.ToString(reader["IdTipoDocumento"])
                            }
                        };
                    }
                    reader.Close();
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private decimal get_id(int IdEmpresa)
        {
            try
            {
                decimal ID = 1;
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    var lst = from q in Context.cp_proveedor
                              where q.IdEmpresa == IdEmpresa
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdProveedor) + 1;
                }
                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(cp_proveedor_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar() )
                {
                    cp_proveedor Entity = new cp_proveedor
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdProveedor = info.IdProveedor=get_id(info.IdEmpresa),
                        IdPersona = info.IdPersona,
                        IdClaseProveedor = info.IdClaseProveedor,
                        IdCiudad = info.IdCiudad,
                        IdBanco_acreditacion = info.IdBanco_acreditacion,
                        IdCtaCble_CXP = info.IdCtaCble_CXP,
                        IdCtaCble_Gasto = info.IdCtaCble_Gasto,
                        IdTipoCta_acreditacion_cat = info.IdTipoCta_acreditacion_cat,
                        num_cta_acreditacion = info.num_cta_acreditacion,
                        pr_codigo = info.pr_codigo,
                        pr_plazo = info.pr_plazo,
                        pr_estado = info.pr_estado="A",
                        pr_contribuyenteEspecial = info.pr_contribuyenteEspecial_bool == true ? "S" : "N",
                        es_empresa_relacionada = info.es_empresa_relacionada,
                        pr_celular = info.pr_celular,
                        pr_telefonos = info.pr_telefonos,
                        pr_direccion = info.pr_direccion,
                        pr_correo = info.pr_correo,
                        IdCtaCble_Anticipo = info.IdCtaCble_Anticipo,

                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = DateTime.Now
                    };
                    Context.cp_proveedor.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(cp_proveedor_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_proveedor Entity = Context.cp_proveedor.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdProveedor == info.IdProveedor);
                    if (Entity == null) return false;

                    //Entity.IdPersona = info.IdPersona;
                    Entity.IdClaseProveedor = info.IdClaseProveedor;
                    Entity.IdCiudad = info.IdCiudad;
                    Entity.IdBanco_acreditacion = info.IdBanco_acreditacion;
                    Entity.IdCtaCble_CXP = (info.IdCtaCble_CXP)== "== Seleccione =="?null: info.IdCtaCble_CXP;
                    Entity.IdCtaCble_Gasto =  (info.IdCtaCble_Gasto) == "== Seleccione ==" ? null : info.IdCtaCble_Gasto;
                    Entity.IdTipoCta_acreditacion_cat = info.IdTipoCta_acreditacion_cat;
                    Entity.num_cta_acreditacion = info.num_cta_acreditacion;
                    Entity.pr_codigo = info.pr_codigo;
                    Entity.pr_plazo = info.pr_plazo;
                    Entity.pr_contribuyenteEspecial = info.pr_contribuyenteEspecial_bool == true ? "S" : "N";
                    Entity.es_empresa_relacionada = info.es_empresa_relacionada;
                    Entity.pr_correo = info.pr_correo;
                    Entity.pr_direccion = info.pr_direccion;
                    Entity.pr_telefonos = info.pr_telefonos;
                    Entity.pr_celular = info.pr_celular;
                    Entity.IdCtaCble_Anticipo = info.IdCtaCble_Anticipo;

                    Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    Entity.Fecha_UltMod = DateTime.Now;
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(cp_proveedor_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_proveedor Entity = Context.cp_proveedor.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdProveedor == info.IdProveedor);
                    if (Entity == null) return false;

                    Entity.pr_estado = info.pr_estado="I";

                    Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
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

        public cp_proveedor_Info get_info_x_num_cedula(int IdEmpresa, string pe_cedulaRuc)
        {
            try
            {
                cp_proveedor_Info info = new cp_proveedor_Info
                {
                    info_persona = new Info.General.tb_persona_Info()
                };

                Entities_general Context_general = new Entities_general();
                tb_persona Entity_p = Context_general.tb_persona.Where(q => q.pe_cedulaRuc == pe_cedulaRuc).FirstOrDefault();
                if (Entity_p == null)
                {
                    Context_general.Dispose();
                    return info;
                }
                Entities_cuentas_por_pagar Context_cxp = new Entities_cuentas_por_pagar();
                cp_proveedor Entity_c = Context_cxp.cp_proveedor.Where(q => q.IdEmpresa == IdEmpresa && q.IdPersona == Entity_p.IdPersona).FirstOrDefault();
                if (Entity_c == null)
                {
                    info.IdPersona = Entity_p.IdPersona;
                    info.info_persona = new Info.General.tb_persona_Info
                    {
                        IdPersona = Entity_p.IdPersona,
                        pe_apellido = Entity_p.pe_apellido,
                        pe_nombre = Entity_p.pe_nombre,
                        pe_cedulaRuc = Entity_p.pe_cedulaRuc,
                        pe_nombreCompleto = Entity_p.pe_nombreCompleto,
                        pe_razonSocial = Entity_p.pe_razonSocial,
                        pe_celular = Entity_p.pe_celular,
                        pe_telfono_Contacto = Entity_p.pe_telfono_Contacto,
                        pe_correo = Entity_p.pe_correo,
                        pe_direccion = Entity_p.pe_direccion
                    };
                    Context_general.Dispose();
                    Context_cxp.Dispose();
                    return info;
                }
                info = new cp_proveedor_Info
                {
                    IdEmpresa = Entity_c.IdEmpresa,
                    IdProveedor = Entity_c.IdProveedor,
                    IdPersona = Entity_p.IdPersona,
                    info_persona = new Info.General.tb_persona_Info
                    {
                        IdPersona = Entity_p.IdPersona,
                        pe_apellido = Entity_p.pe_apellido,
                        pe_nombre = Entity_p.pe_nombre,
                        pe_cedulaRuc = Entity_p.pe_cedulaRuc,
                        pe_nombreCompleto = Entity_p.pe_nombreCompleto,
                        pe_razonSocial = Entity_p.pe_razonSocial,
                        pe_celular = Entity_p.pe_celular,
                        pe_telfono_Contacto = Entity_p.pe_telfono_Contacto,
                        pe_correo = Entity_p.pe_correo,
                        pe_direccion = Entity_p.pe_direccion
                    }
                };

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }



        #region metodo baja demanda

        public List<cp_proveedor_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<cp_proveedor_Info> Lista = new List<cp_proveedor_Info>();
            Lista = get_list(IdEmpresa, skip, take, args.Filter);

            return Lista;
        }

        public cp_proveedor_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, int IdEmpresa)
        {
            decimal id;
            if (args.Value == null || !decimal.TryParse(args.Value.ToString(), out id))
                return null;
            return get_info_demanda(IdEmpresa, Convert.ToDecimal(args.Value));
        }

        public cp_proveedor_Info get_info_demanda(int IdEmpresa, decimal IdProducto)
        {
            cp_proveedor_Info info = new cp_proveedor_Info();

            using (Entities_cuentas_por_pagar Contex = new Entities_cuentas_por_pagar())
            {
                info = (from q in Contex.vwcp_proveedor_consulta
                      
                        select new cp_proveedor_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdPersona = q.IdPersona,
                            IdProveedor = q.IdProveedor,
                            pr_codigo = q.pr_codigo,
                            info_persona = new Info.General.tb_persona_Info
                            {
                                pe_cedulaRuc=q.pe_cedulaRuc,
                                pe_nombreCompleto=q.pe_nombreCompleto
                            }
                           
                        }).FirstOrDefault();

            }
          
            return info;
        }

        public List<cp_proveedor_Info> get_list(int IdEmpresa, int skip, int take, string filter)
        {
            try
            {
                List<cp_proveedor_Info> Lista = new List<cp_proveedor_Info>();

                Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar();

                var lst = (from
                          p in Context.vwcp_proveedor_consulta
                          
                           where
                            p.IdEmpresa == IdEmpresa
                           
                            && (p.IdProveedor.ToString() + " " + p.pe_nombreCompleto).Contains(filter)
                           select new
                           {
                               p.IdEmpresa,
                               p.IdPersona,
                               p.IdProveedor,
                               p.pe_cedulaRuc,
                               p.pr_codigo,
                               p.pe_nombreCompleto
                             
                           })
                             .OrderBy(p => p.IdProveedor)
                             .Skip(skip)
                             .Take(take)
                             .ToList();


                foreach (var q in lst)
                {
                    Lista.Add(new cp_proveedor_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdPersona = q.IdPersona,
                        IdProveedor = q.IdProveedor,
                        pr_codigo=q.pr_codigo,
                        info_persona=new Info.General.tb_persona_Info
                        {
                            pe_cedulaRuc=q.pe_cedulaRuc,
                            pe_nombreCompleto=q.pe_nombreCompleto,
                        }
                        
                    });
                }

                Context.Dispose();
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion



    }
}
