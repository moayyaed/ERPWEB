using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.CuentasPorPagar;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.CuentasPorPagar;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorPagar.Controllers
{
    [SessionTimeout]
    public class CodigoSRIController : Controller
    {
        #region Variables
        cp_codigo_SRI_Bus bus_codigo_sri = new cp_codigo_SRI_Bus();
        cp_codigo_SRI_tipo_Bus bus_tipo_codigo = new cp_codigo_SRI_tipo_Bus();
        cp_codigo_SRI_x_CtaCble_Bus bus_codigo_ctacble = new cp_codigo_SRI_x_CtaCble_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        cp_CodigoSRI_List Lista_CodigoSRI = new cp_CodigoSRI_List();
        #endregion

        #region Index
        public ActionResult Index(string IdTipoSRI = "")
        {
            ViewBag.IdTipoSRI = IdTipoSRI;
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cp_codigo_SRI_Info model = new cp_codigo_SRI_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdTipoSRI = IdTipoSRI
            };

            var lst = bus_codigo_sri.get_list(model.IdTipoSRI,true);
            Lista_CodigoSRI.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_codigo_sri(int IdTipoSRI = 0, bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_codigo_sri.get_list(IdTipoSRI, true);
            ViewBag.IdTipoSRI = IdTipoSRI;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CodigoSRI.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_codigo_sri", model);
        }

        private void cargar_combos()
        {
            var lst_codigo_tipo = bus_tipo_codigo.get_list(true);
            ViewBag.lst_tipo = lst_codigo_tipo;

            var lst_ctacble = bus_plancta.get_list(Convert.ToInt32(SessionFixed.IdEmpresa), false, false);
            ViewBag.lst_cuentas = lst_ctacble;

        }


        #endregion

        #region Acciones
        public ActionResult Nuevo(string IdTipoSRI = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cp_codigo_SRI_Info model = new cp_codigo_SRI_Info
            {
                co_f_valides_desde = DateTime.Now.AddYears(-100),
                co_f_valides_hasta = DateTime.Now.AddYears(100),
                info_codigo_ctacble = new cp_codigo_SRI_x_CtaCble_Info()

            };
            ViewBag.IdTipoSRI = IdTipoSRI;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cp_codigo_SRI_Info model)
        {
            if (!bus_codigo_sri.guardarDB(model))
            {
                if (!string.IsNullOrEmpty(model.info_codigo_ctacble.IdCtaCble))
                {
                    model.info_codigo_ctacble.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                    model.info_codigo_ctacble.idCodigo_SRI = model.IdCodigo_SRI;
                    bus_codigo_ctacble.guardarDB(model.info_codigo_ctacble);
                }                
                return RedirectToAction("Index", new { IdTipoSRI = model.IdTipoSRI });
            }
            return RedirectToAction("Index", new { IdTipoSRI = model.IdTipoSRI });
        }

        public ActionResult Consultar(int IdCodigo_SRI = 0, bool Exito=false)
        {
            cp_codigo_SRI_Info model = bus_codigo_sri.get_info(IdCodigo_SRI);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdTipoSRI = model.IdTipoSRI });
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            if (model.co_estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            model.info_codigo_ctacble = bus_codigo_ctacble.get_info(IdCodigo_SRI, Convert.ToInt32(SessionFixed.IdEmpresa));
            if (model.info_codigo_ctacble == null)
                model.info_codigo_ctacble = new cp_codigo_SRI_x_CtaCble_Info
                {
                    IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                    idCodigo_SRI = model.IdCodigo_SRI
                };
            ViewBag.IdTipoSRI = model.IdTipoSRI;
            cargar_combos();

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(int IdCodigo_SRI = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            cp_codigo_SRI_Info model = bus_codigo_sri.get_info(IdCodigo_SRI);
            if (model == null)
            {
                return RedirectToAction("Index", new { IdTipoSRI = model.IdTipoSRI });
            }
            model.info_codigo_ctacble = bus_codigo_ctacble.get_info(IdCodigo_SRI, Convert.ToInt32(SessionFixed.IdEmpresa));
            if (model.info_codigo_ctacble == null)
                model.info_codigo_ctacble = new cp_codigo_SRI_x_CtaCble_Info
                {
                    IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                    idCodigo_SRI = model.IdCodigo_SRI
                };
            ViewBag.IdTipoSRI = model.IdTipoSRI;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cp_codigo_SRI_Info model)
        {
            if(!bus_codigo_sri.modificarDB(model))
            {
                cargar_combos();
                ViewBag.IdTipoSRI = model.IdTipoSRI;
                return View(model);
            }
            bus_codigo_ctacble.eliminarDB(model.IdCodigo_SRI, model.info_codigo_ctacble.IdEmpresa);
            if (!string.IsNullOrEmpty(model.info_codigo_ctacble.IdCtaCble))
                bus_codigo_ctacble.guardarDB(model.info_codigo_ctacble);
            return RedirectToAction("Index", new { IdTipoSRI = model.IdTipoSRI });
        }

        public ActionResult Anular(int IdCodigo_SRI = 0, string IdTipoSRI = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorPagar", "TipoCodigoSRI", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            cp_codigo_SRI_Info model = bus_codigo_sri.get_info(IdCodigo_SRI);
            if(model == null)
            {
                return RedirectToAction("Index", new { IdTipoSRI = IdTipoSRI });
            }
            model.info_codigo_ctacble = bus_codigo_ctacble.get_info(IdCodigo_SRI, Convert.ToInt32(SessionFixed.IdEmpresa));
            if(model.info_codigo_ctacble == null)
                model.info_codigo_ctacble = new cp_codigo_SRI_x_CtaCble_Info
                {
                    IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                    idCodigo_SRI = model.IdCodigo_SRI
                };
            ViewBag.IdTipoSRI = IdTipoSRI;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cp_codigo_SRI_Info model)
        {
            if (!bus_codigo_sri.anularDB(model))
            {
                cargar_combos();
                ViewBag.IdTipoSRI = model.IdTipoSRI;
                return View(model);
            }
            return RedirectToAction("Index", new { IdTipoSRI = model.IdTipoSRI });
        }

        #endregion

        #region Metodos ComboBox bajo demanda
        public ActionResult CmbCuenta_Codigo()
        {
            string model = "";
            return PartialView("_CmbCuenta_Codigo", model);
        }

        public List<ct_plancta_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

    }
    public class cp_CodigoSRI_List
    {
        string Variable = "cp_CodigoSRI_Info";
        public List<cp_codigo_SRI_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cp_codigo_SRI_Info> list = new List<cp_codigo_SRI_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cp_codigo_SRI_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cp_codigo_SRI_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}