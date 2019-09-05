using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class DinardapController : Controller
    {
        #region Variables
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        Dinardap_Bus bus_dinardap = new Dinardap_Bus();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        string rutafile = System.IO.Path.GetTempPath();
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            cl_filtros_contabilidad_Info model = new cl_filtros_contabilidad_Info();
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public FileResult Nuevo(cl_filtros_contabilidad_Info model)
        {
            string nombre_file = "Dinardap-"+model.IdPeriodoIni.ToString();
            if (model.IdPeriodoIni.ToString().Length == 6)
            {
                nombre_file = "Dinardap-" + model.IdPeriodoIni.ToString().Substring(4, 2) + model.IdPeriodoIni.ToString().Substring(0, 4);
            }

            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            byte[] archivo;
            List<DinardapData_Info> lst_archivo = new List<DinardapData_Info>();
            List<Dinardap_Info> ListInfoDinardap = new List<Dinardap_Info>();

            if (model.IntArray.Count()>0 && model.IdPeriodoIni != 0)
            {
                foreach (var item in model.IntArray)
                {
                    lst_archivo.AddRange(bus_dinardap.get_info(IdEmpresa, model.IdPeriodoIni, Convert.ToInt32(item)));
                }
            }

            var lst_dinardarp = set_dinardap_info(lst_archivo, model.IdPeriodoIni);

            archivo = GetArchivo(lst_dinardarp, nombre_file);
            return File(archivo, "application/xml", nombre_file + ".txt");
        }
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
            var lst_periodos = bus_periodo.get_list(IdEmpresa, false);
            ViewBag.lst_periodos = lst_periodos;

            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            lst_sucursal.Where(q => q.IdSucursal == IdSucursal).FirstOrDefault().Seleccionado = true;
            ViewBag.lst_sucursal = lst_sucursal;
        }

        private List<Dinardap_Info> set_dinardap_info(List<DinardapData_Info> lst_archivo, int IdPeriodo = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            List<Dinardap_Info> ListInfoDinardap = new List<Dinardap_Info>();

            foreach (var item in lst_archivo)
            {
                Dinardap_Info InfoDinardap = new Dinardap_Info();
                ct_periodo_Info InfoPeriodo = bus_periodo.get_info(IdEmpresa, IdPeriodo);
                InfoDinardap.FechaDatos = InfoPeriodo.pe_FechaFin.ToString("dd/MM/yyyy");

                switch (item.IdTipoDocumento)
                {
                    case "CED": InfoDinardap.TipoIden = "C"; break;
                    case "PAS": InfoDinardap.TipoIden = "E"; break;
                    case "RUC": InfoDinardap.TipoIden = "R"; break;
                    default:
                        InfoDinardap.TipoIden = "C"; break;
                }
                InfoDinardap.Identificacion = item.pe_cedulaRuc.Trim();
                InfoDinardap.Nom_apellido = item.pe_nombreCompleto.Trim();

                switch (item.pe_Naturaleza)
                {
                    case "JURI": InfoDinardap.clase_suje = "J"; InfoDinardap.sexo = ""; InfoDinardap.estado_civil = ""; InfoDinardap.Origen_Ing = ""; break;
                    case "NATU":
                        InfoDinardap.clase_suje = "N";
                        InfoDinardap.sexo = (item.pe_sexo == "SEXO_MAS") ? "M" : "F";
                        InfoDinardap.Origen_Ing = "V";
                        switch (item.IdEstadoCivil)
                        {
                            case "CASAD": InfoDinardap.estado_civil = "C"; break;
                            case "DIVOR": InfoDinardap.estado_civil = "D"; break;
                            case "SOLTE": InfoDinardap.estado_civil = "S"; break;
                            case "UNILI": InfoDinardap.estado_civil = "U"; break;
                            case "VIUD": InfoDinardap.estado_civil = "V"; break;
                            default: InfoDinardap.estado_civil = "S"; break;
                        }
                        break;

                    case "OTRO": InfoDinardap.clase_suje = "N"; InfoDinardap.sexo = ""; InfoDinardap.estado_civil = ""; InfoDinardap.Origen_Ing = ""; break;
                    case "RISE": InfoDinardap.clase_suje = "N"; InfoDinardap.sexo = ""; InfoDinardap.estado_civil = ""; InfoDinardap.Origen_Ing = ""; break;
                }


                InfoDinardap.Provincia = item.Cod_Provincia;
                InfoDinardap.canton = item.Cod_Ciudad;
                InfoDinardap.parroquia = item.cod_parroquia;

                InfoDinardap.num_operacion = item.vt_NumFactura != null ? /*item.vt_serie1 + "-" + item.vt_serie2 + "-" +*/ item.vt_NumFactura : item.CodCbteVta;

                InfoDinardap.valor_ope = Math.Round(Math.Abs(Convert.ToDecimal(item.Valor_Original)), 2, MidpointRounding.AwayFromZero);

                InfoDinardap.saldo_ope = Math.Round(Math.Abs(Convert.ToDecimal(item.Valor_Original)), 2, MidpointRounding.AwayFromZero) - Math.Round(Math.Abs(Convert.ToDecimal(item.Total_Pagado)), 2, MidpointRounding.AwayFromZero);


                InfoDinardap.fecha_conse = item.vt_fecha.ToString("dd/MM/yyyy");
                InfoDinardap.fecha_vct = Convert.ToDateTime(item.vt_fech_venc).ToString("dd/MM/yyyy");
                InfoDinardap.fecha_exigi = Convert.ToDateTime(item.vt_fech_venc).ToString("dd/MM/yyyy");

                //
                InfoDinardap.Plazo_op = Convert.ToInt32(((DateTime)item.vt_fech_venc - (DateTime)item.vt_fecha).TotalDays); //item.Plazo;
                InfoDinardap.Periodicidad_pago = Convert.ToDecimal(item.Plazo);

                if (InfoDinardap.Plazo_op <= 0) { InfoDinardap.Plazo_op = 1; }
                if (InfoDinardap.Plazo_op > 99999) { InfoDinardap.Plazo_op = 99999; }

                if (InfoDinardap.Periodicidad_pago <= 0) { InfoDinardap.Periodicidad_pago = 1; }
                if (InfoDinardap.Periodicidad_pago > 99999) { InfoDinardap.Periodicidad_pago = 99999; }

                InfoDinardap.dias_morosidad = Math.Abs(Convert.ToDecimal(item.Dias_Vencidos));
                InfoDinardap.monto_morosidad = InfoDinardap.dias_morosidad == 0 ? 0 : Math.Round(Math.Abs(Convert.ToDecimal(item.Valor_Vencido)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.monto_inte_mora = 0;
                if (item.x_Vencer_1_30_Dias != 0 || item.x_Vencer_31_90_Dias != 0 || item.x_Vencer_91_180_Dias != 0 || item.x_Vencer_181_360_Dias != 0 || item.x_Vencer_Mayor_a_360Dias != 0
                    || item.Vencido_1_30_Dias != 0 || item.Vencido_31_90_Dias != 0 || item.Vencido_91_180_Dias != 0 || item.Vencido_181_360_Dias != 0 || item.Vencido_Mayor_a_360Dias != 0)
                {
                    //VALORES POR VENCER SE DEBEN CONSIDERAR EN MORA?
                }

                InfoDinardap.valor_x_vencer_1_30 = Math.Round(Math.Abs(Convert.ToDecimal(item.x_Vencer_1_30_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_x_vencer_31_90 = Math.Round(Math.Abs(Convert.ToDecimal(item.x_Vencer_31_90_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_x_vencer_91_180 = Math.Round(Math.Abs(Convert.ToDecimal(item.x_Vencer_91_180_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_x_vencer_181_360 = Math.Round(Math.Abs(Convert.ToDecimal(item.x_Vencer_181_360_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_x_vencer_mas_360 = Math.Round(Math.Abs(Convert.ToDecimal(item.x_Vencer_Mayor_a_360Dias)), 2, MidpointRounding.AwayFromZero);

                InfoDinardap.valor_vencido_1_30 = Math.Round(Math.Abs(Convert.ToDecimal(item.Vencido_1_30_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_vencido_31_90 = Math.Round(Math.Abs(Convert.ToDecimal(item.Vencido_31_90_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_vencido_91_180 = Math.Round(Math.Abs(Convert.ToDecimal(item.Vencido_91_180_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_vencido_181_360 = Math.Round(Math.Abs(Convert.ToDecimal(item.Vencido_181_360_Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_vencido_mas_360 = Math.Round(Math.Abs(Convert.ToDecimal(item.Vencido_Mayor_a_360Dias)), 2, MidpointRounding.AwayFromZero);
                InfoDinardap.valor_en_demand_judi = 0;
                InfoDinardap.cartera_castigada = 0;
                InfoDinardap.couta_credito = Math.Round(Math.Abs(Convert.ToDecimal(item.Valor_Vencido == 0 ? item.Valor_x_Vencer : item.Valor_Vencido)), 2, MidpointRounding.AwayFromZero);
                //Fecha de cobro
                InfoDinardap.fecha_cancela = "";//item.cr_fechaCobro == null || item.Total_Pagado == 0 ? "" : Convert.ToDateTime(item.cr_fechaCobro).ToString("dd/MM/yyyy");
                InfoDinardap.forma_cance = ""; //item.cr_fechaCobro == null ? "" : "E";
                InfoDinardap.CodEntidad = item.cod_entidad_dinardap;

                ListInfoDinardap.Add(InfoDinardap);
            }

            return ListInfoDinardap;
        }
        #endregion

        #region Archivo
        private byte[] GetMulticash(List<Dinardap_Info> lst_dinardarp, string NombreArchivo)
        {
            try
            {
                string sLinea = "";
                System.IO.File.Delete(rutafile + NombreArchivo + ".txt");

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(rutafile + NombreArchivo + ".txt", true))
                {
                    foreach (Dinardap_Info InfoData in lst_dinardarp)
                    {
                        //cabecera

                        sLinea = "";

                        sLinea = InfoData.CodEntidad + "|" + InfoData.FechaDatos + "|" + InfoData.TipoIden;
                        sLinea = sLinea + "|" + InfoData.Identificacion + "|" + InfoData.Nom_apellido + "|" + InfoData.clase_suje;
                        sLinea = sLinea + "|" + InfoData.Provincia + "|" + InfoData.canton + "|" + InfoData.parroquia;
                        sLinea = sLinea + "|" + InfoData.sexo + "|" + InfoData.estado_civil + "|" + InfoData.Origen_Ing;
                        sLinea = sLinea + "|" + InfoData.num_operacion + "|" + string.Format("{0:f2}", InfoData.valor_ope) + "|" + string.Format("{0:f2}", InfoData.saldo_ope);
                        sLinea = sLinea + "|" + InfoData.fecha_conse + "|" + InfoData.fecha_vct + "|" + InfoData.fecha_exigi;
                        sLinea = sLinea + "|" + InfoData.Plazo_op + "|" + InfoData.Periodicidad_pago + "|" + InfoData.dias_morosidad;
                        sLinea = sLinea + "|" + string.Format("{0:f2}", InfoData.monto_morosidad) + "|" + string.Format("{0:f2}", InfoData.monto_inte_mora) + "|" + string.Format("{0:f2}", InfoData.valor_x_vencer_1_30);
                        sLinea = sLinea + "|" + string.Format("{0:f2}", InfoData.valor_x_vencer_31_90) + "|" + string.Format("{0:f2}", InfoData.valor_x_vencer_91_180) + "|" + string.Format("{0:f2}", InfoData.valor_x_vencer_181_360);
                        sLinea = sLinea + "|" + string.Format("{0:f2}", InfoData.valor_x_vencer_mas_360) + "|" + string.Format("{0:f2}", InfoData.valor_vencido_1_30) + "|" + string.Format("{0:f2}", InfoData.valor_vencido_31_90);
                        sLinea = sLinea + "|" + string.Format("{0:f2}", InfoData.valor_vencido_91_180) + "|" + string.Format("{0:f2}", InfoData.valor_vencido_181_360) + "|" + string.Format("{0:f2}", InfoData.valor_vencido_mas_360);
                        sLinea = sLinea + "|" + string.Format("{0:f2}", InfoData.valor_en_demand_judi) + "|" + string.Format("{0:f2}", InfoData.cartera_castigada) + "|" + string.Format("{0:f2}", InfoData.couta_credito);
                        sLinea = sLinea + "|" + InfoData.fecha_cancela + "|" + InfoData.forma_cance;

                        file.WriteLine(sLinea);
                    }
                }
                byte[] filebyte = System.IO.File.ReadAllBytes(rutafile + NombreArchivo + ".txt");
                return filebyte;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public byte[] GetArchivo(List<Dinardap_Info> info, string nombre_file)
        {
            try
            {
                return GetMulticash(info, nombre_file);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}