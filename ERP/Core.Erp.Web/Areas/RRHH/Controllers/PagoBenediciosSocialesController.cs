using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using Core.Erp.Web.Helps;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Core.Erp.Info.Helps;
using Core.Erp.Info.General;
using Core.Erp.Bus.General;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class PagoBenediciosSocialesController : Controller
    {
        #region Variables
        int IdEmpresa = 0;
        ro_rol_Bus bus_rol = new ro_rol_Bus();
        ro_rol_List Lista_Benenificios = new ro_rol_List();
        #endregion

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            List<ro_rol_Info> lista = bus_rol.get_list_decimos(model.IdEmpresa, model.IdSucursal);
            Lista_Benenificios.set_list(lista, model.IdTransaccionSession);
            cargar_combo_consulta(model.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            cargar_combo_consulta(model.IdEmpresa);
            List<ro_rol_Info> lista = bus_rol.get_list_decimos(model.IdEmpresa, model.IdSucursal);
            Lista_Benenificios.set_list(lista, model.IdTransaccionSession);

            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_pago_beneficios(int IdEmpresa=0, int IdSucursal=0)
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                var model = Lista_Benenificios.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_pago_beneficios", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_rol_Info info)
        {
            try
            {
                    info.UsuarioIngresa = SessionFixed.IdUsuario;
                    info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    if (!bus_rol.Decimos(info))
                        return View(info);
                    else
                        return RedirectToAction("Index");
         
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Nuevo()
        {
            try
            {
                cargar_combos(0, 0);
                ro_rol_Info info = new ro_rol_Info();
                info.Anio = DateTime.Now.Year;
                return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_rol_Info info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    info.UsuarioIngresa = SessionFixed.IdUsuario;
                    if (!bus_rol.Decimos(info))
                    {
                        cargar_combos(info.IdNomina_Tipo, info.IdNomina_TipoLiqui);
                        return View(info);
                    }
                    else
                        return RedirectToAction("Index");
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Modificar(int IdNomina_Tipo = 0, int IdNomina_TipoLiqui = 0, int IdPeriodo = 0, decimal IdRol=0)
        {
            try
            {
                ro_rol_Info model = new ro_rol_Info();
                cargar_combos(IdNomina_Tipo, IdNomina_TipoLiqui);
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                model=bus_rol.get_info(IdEmpresa, IdNomina_Tipo, IdNomina_TipoLiqui, IdPeriodo, IdRol);
                if (model.IdNomina_TipoLiqui == 3)
                    model.decimoIII = true;
                else
                    model.decimoIV = true;
                return View(model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cargar_combos(int IdNomina_Tipo, int IdNomina_Tipo_Liqui)
        {
            try
            {
                tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
                ro_nomina_tipo_Bus bus_nomina = new ro_nomina_tipo_Bus();
                var lst_sucursal = bus_sucursal.GetList(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, false);
                ViewBag.lst_sucursal = lst_sucursal;

              var  lista_nomina = bus_nomina.get_list(Convert.ToInt32(SessionFixed.IdEmpresa), false);
                ViewBag.lst_nomina = lista_nomina;

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void cargar_combo_consulta(int IdEmpresa)
        {
            try
            {
                tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
                var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
                ViewBag.lst_sucursal = lst_sucursal;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public FileResult GetCSV(int IdRol=0,int IdNomina_TipoLiqui = 0)
        {
            ro_archivosCSV_Bus bus_archivos = new ro_archivosCSV_Bus();
            string archivo = "";
            string NombreFile = "";

            var listado = bus_archivos.get_lis(Convert.ToInt32(SessionFixed.IdEmpresa), IdRol, 24);
            if (IdNomina_TipoLiqui == 3)
            {
                NombreFile = "Decimo III";
                archivo = bus_archivos.Get_decimoIII(listado);
            }
            else
            { 
                NombreFile = "Decimo IV";

                archivo = bus_archivos.Get_decimoIII(listado);
            }
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(archivo);
            return File(byteArray, "application/xml", NombreFile + ".csv");
        }
    }

    public class ro_rol_List
    {
        string variable = "ro_rol_Info";
        public List<ro_rol_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_rol_Info> list = new List<ro_rol_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_rol_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_rol_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }
    }
}