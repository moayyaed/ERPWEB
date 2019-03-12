using Core.Erp.Data.RRHH;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.RRHH
{
    public class ro_rdep_Bus
    {
        ro_rdep_Data oData = new ro_rdep_Data();

        public List<ro_rdep_Info> GetList(int IdEmpresa, int IdSucursal, int IdNomina_Tipo, int IdAnio)
        {
            try
            {
                return oData.GetList(IdEmpresa, IdSucursal, IdNomina_Tipo, IdAnio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ro_rdep_Info GetInfo(int IdEmpresa, int Id_Rdep )
        {
            try
            {
                return oData.GetInfo(IdEmpresa, Id_Rdep);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ro_rdep_det_Info GetInfo_x_Empleado(int IdEmpresa, int Id_Rdep, int Secuencia)
        {
            try
            {
                return oData.GetInfo_x_Empleado(IdEmpresa, Id_Rdep, Secuencia);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GenerarRDEP(int IdEmpresa, int IdSucursal, int Id_Rdep, int IdAnio, int IdNomina_Tipo, int IdEmpleado, string Observacion, string IdUsuario)
        {
            try
            {
                return oData.GenerarRDEP(IdEmpresa, IdSucursal, Id_Rdep, IdAnio, IdNomina_Tipo, IdEmpleado, Observacion, IdUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModificarBD(ro_rdep_det_Info info)
        {
            try
            {
                return oData.ModificarBD(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /********************************************************/
        /*Rdep_Data odata = new Rdep_Data();
        rdep rdp = new rdep();
        List<ro_tabla_Impu_Renta_Info> list_base_calculo = new List<ro_tabla_Impu_Renta_Info>();
        ro_tabla_Impu_Renta_Data odata_base_Calculo = new ro_tabla_Impu_Renta_Data();
        tb_empresa_Info info_empresa = new tb_empresa_Info();
        tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
        public rdep get_list(int IdEmpresa, int Anio, decimal IdEmpleado)
        {
            try
            {
                info_empresa = bus_empresa.get_info(IdEmpresa);
                list_base_calculo = odata_base_Calculo.get_list(Anio).OrderByDescending(v => v.Secuencia).ToList();
                rdp.anio = Anio.ToString();
                rdp.numRuc = info_empresa.em_ruc;
                datRetRelDepTyp detalle = new datRetRelDepTyp();
                rdp.retRelDep = new List<datRetRelDepTyp>();
                var lis = odata.gett_list(IdEmpresa, Anio, IdEmpleado);
                lis.ForEach(item =>
                {
                    if (item.pe_cedulaRuc == "0927181131")
                    {

                    }
                    datRetRelDepTyp info_det = new datRetRelDepTyp();
                    info_det.empleado = new datEmpTyp();
                    info_det.empleado.benGalpg = benGalpgType.NO;
                    info_det.empleado.tipIdRet = datEmpTypTipIdRet.C;
                    info_det.empleado.idRet = item.pe_cedulaRuc;
                    info_det.empleado.apellidoTrab = item.pe_apellido.Replace("Ñ", "N").Trim();
                    info_det.empleado.nombreTrab = item.pe_nombre.Replace("Ñ", "N").Trim();

                    info_det.empleado.apellidoTrab = info_det.empleado.apellidoTrab.Replace("  ", " ").Trim();
                    info_det.empleado.nombreTrab = info_det.empleado.nombreTrab.Replace("  ", " ").Trim();


                    info_det.empleado.estab = item.Su_CodigoEstablecimiento;
                    info_det.empleado.residenciaTrab = resciTyp.Item01;
                    info_det.empleado.paisResidencia = "593";
                    info_det.empleado.aplicaConvenio = convImposTyp.NA;
                    info_det.empleado.tipIdDiscap = tipIdDiscapTyp.N;
                    info_det.empleado.tipoTrabajDiscap = discapTyp.Item01;
                    info_det.empleado.porcentajeDiscap = "0";
                    info_det.empleado.idDiscap = "999";
                    info_det.deducAliementSpecified = true;
                    info_det.deducVestimSpecified = true;
                    info_det.deducSaludSpecified = true;
                    info_det.deducEducaSpecified = true;
                    info_det.deducViviendaSpecified = true;
                    info_det.deducArtycultSpecified = true;
                    info_det.deducArtycult = 0;
                    info_det.suelSal = (item.Sueldo) == null ? Convert.ToDecimal(0.00) : Convert.ToDecimal(item.Sueldo);
                    info_det.sobSuelComRemu = (item.IngresoVarios) == null ? Convert.ToDecimal(0.00) : Convert.ToDecimal(item.IngresoVarios);
                    info_det.partUtil = Convert.ToDecimal(item.Utilidades);
                    info_det.intGrabGen = 0;
                    info_det.impRentEmpl = 0;
                    info_det.decimTer = (item.DecimoTercerSueldo) == null ? Convert.ToDecimal(0.00) : Convert.ToDecimal(item.DecimoTercerSueldo);
                    info_det.decimCuar = (item.DecimoCuartoSueldo) == null ? Convert.ToDecimal(0.00) : Convert.ToDecimal(item.DecimoCuartoSueldo);
                    info_det.fondoReserva = (item.FondosReserva) == null ? Convert.ToDecimal(0.00) : Convert.ToDecimal(item.FondosReserva);
                    info_det.salarioDigno = 0;
                    info_det.otrosIngRenGrav = 0;
                    info_det.sisSalNet = "1";
                    info_det.apoPerIess = Convert.ToDecimal(item.AportePErsonal);
                    info_det.aporPerIessConOtrosEmpls = 0;
                    info_det.deducVivienda = Convert.ToDecimal(item.GastoVivienda);
                    info_det.deducSalud = Convert.ToDecimal(item.GastoSalud);
                    info_det.deducEduca = Convert.ToDecimal(item.GastoEucacion);
                    info_det.deducAliement = Convert.ToDecimal(item.GastoAlimentacion);
                    info_det.deducVestim = Convert.ToDecimal(item.GastoVestimenta);
                    info_det.deducArtycult = 0;
                    info_det.exoDiscap = 0;
                    info_det.exoTerEd = 0;
                    info_det.basImp = Convert.ToDecimal((info_det.suelSal + info_det.sobSuelComRemu) - (info_det.deducVivienda + info_det.deducSalud + info_det.deducEduca + info_det.deducAliement + info_det.deducVestim + info_det.apoPerIess));
                    info_det.impRentCaus = CalcularImpuestoRenta(info_det);
                    info_det.valRetAsuOtrosEmpls = 0;
                    info_det.valImpAsuEsteEmpl = 0;
                    info_det.valRet = 0;
                    info_det.ingGravConEsteEmpl = info_det.suelSal + info_det.sobSuelComRemu;
                    info_det.intGrabGen = 0;

                    rdp.retRelDep.Add(info_det);


                });
                return rdp;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public decimal CalcularImpuestoRenta(datRetRelDepTyp item)
        {
            try
            {


                double BaseImponible = 0;
                BaseImponible = Convert.ToDouble(item.suelSal + item.decimCuar + item.decimCuar + item.fondoReserva + item.partUtil + item.otrosIngRenGrav) -
                    Convert.ToDouble(item.deducSalud + item.deducAliement + item.deducVivienda + item.deducEduca + item.deducVestim);
                double exedente = 0;
                double fraccionBasica = 0;
                double porcentajeFraccionExedente = 0;
                double impuestoFraccionBasica = 0;
                double valorIR = 0;
                foreach (ro_tabla_Impu_Renta_Info item_ in list_base_calculo)
                {
                    if (BaseImponible >= item_.FraccionBasica)
                    {
                        fraccionBasica = Convert.ToDouble(item_.FraccionBasica);
                        porcentajeFraccionExedente = Convert.ToDouble(item_.Por_ImpFraccion_Exce);
                        impuestoFraccionBasica = Convert.ToDouble(item_.ImpFraccionBasica);
                        break;
                    }
                }
                exedente = BaseImponible - fraccionBasica;
                valorIR = impuestoFraccionBasica + (exedente * porcentajeFraccionExedente * 0.01);
                return Convert.ToDecimal(valorIR);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public List<Rdep_Info> get_list_rdep(int IdEmpresa, int Anio, decimal IdEmpleado)
        {
            try
            {
                return odata.gett_list(IdEmpresa, Anio, IdEmpleado);
            }
            catch (Exception)
            {

                throw;
            }
        }*/
    }
}
