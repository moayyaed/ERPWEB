using Core.Erp.Data.Contabilidad;
using Core.Erp.Info.Contabilidad;
using DevExpress.Web;
using System;
using System.Collections.Generic;
namespace Core.Erp.Bus.Contabilidad
{
    public class ct_punto_cargo_Bus
    {
         ct_punto_cargo_Data odata = new  ct_punto_cargo_Data();
        public List<ct_punto_cargo_Info> GetList(int IdEmpresa, int IdPunto_cargo_grupo, bool mostrar_anulados, bool NoMostrarTodos)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdPunto_cargo_grupo, mostrar_anulados, NoMostrarTodos);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ct_punto_cargo_Info GetInfo(int IdEmpresa, int IdPunto_cargo)
        {
            try
            {
                return odata.GetInfo(IdEmpresa, IdPunto_cargo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool GuardarDB( ct_punto_cargo_Info info)
        {
            try
            {
                return odata.GuardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarDB( ct_punto_cargo_Info info)
        {
            try
            {
                return odata.ModificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularDB( ct_punto_cargo_Info info)
        {
            try
            {
                return odata.AnularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ct_punto_cargo_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa, int IdPuntoCargoGrupo)
        {
            try
            {
                return odata.get_list_bajo_demanda(args, IdEmpresa, IdPuntoCargoGrupo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ct_punto_cargo_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, int IdEmpresa)
        {
            try
            {
                return odata.get_info_bajo_demanda(args, IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
