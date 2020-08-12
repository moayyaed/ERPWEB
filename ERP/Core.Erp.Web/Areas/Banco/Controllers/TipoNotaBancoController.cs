using Core.Erp.Bus.Banco;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Banco;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Banco.Controllers
{
    [SessionTimeout]
    public class TipoNotaBancoController : Controller
    {
        #region Variables

        ba_tipo_nota_Bus bus_tipo = new ba_tipo_nota_Bus();
        ct_plancta_Bus bus_cuenta = new ct_plancta_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ba_tipo_nota_List Lista_TipoNota = new ba_tipo_nota_List();
        #endregion

        #region Index

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoNotaBanco", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ba_tipo_nota_Info model = new ba_tipo_nota_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            var lst = bus_tipo.get_list(model.IdEmpresa, "", true);
            Lista_TipoNota.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipo_nota(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_tipo.get_list(IdEmpresa,"", true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TipoNota.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_tipo_nota", model);
        }
        #endregion

        #region Metodos

        private void cargar_combos(int IdEmpresa)
        {
            Dictionary<string, string> lst_tipo_nota = new Dictionary<string, string>();
            lst_tipo_nota.Add("CHEQ", "Cheque");
            lst_tipo_nota.Add("DEPO", "Depósito");
            lst_tipo_nota.Add("NCBA", "Nota de crédito");
            lst_tipo_nota.Add("NDBA", "Nota de débito");
            ViewBag.lst_tipo_nota = lst_tipo_nota;
            
            var lst_cuenta = bus_cuenta.get_list(IdEmpresa, false, true);
            ViewBag.lst_cuenta = lst_cuenta;
            
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo(int IdEmpresa =0 )
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoNotaBanco", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ba_tipo_nota_Info model = new ba_tipo_nota_Info
            {
                IdEmpresa = IdEmpresa
            };
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ba_tipo_nota_Info model)
        {
            if(!bus_tipo.guardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoNota = model.IdTipoNota, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdTipoNota = 0, bool Exito = false)
        {
            ba_tipo_nota_Info model = bus_tipo.get_info(IdEmpresa, IdTipoNota);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoNotaBanco", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdTipoNota = 0, bool Exito=false)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoNotaBanco", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ba_tipo_nota_Info model = bus_tipo.get_info(IdEmpresa, IdTipoNota);
            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ba_tipo_nota_Info model)
        {
            if(!bus_tipo.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdTipoNota = model.IdTipoNota, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdTipoNota = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TipoNotaBanco", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ba_tipo_nota_Info model = bus_tipo.get_info(IdEmpresa, IdTipoNota);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(ba_tipo_nota_Info model)
        {
            if (!bus_tipo.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

    }

    public class ba_tipo_nota_List
    {
        string Variable = "ba_tipo_nota_Info";
        public List<ba_tipo_nota_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_tipo_nota_Info> list = new List<ba_tipo_nota_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_tipo_nota_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_tipo_nota_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}