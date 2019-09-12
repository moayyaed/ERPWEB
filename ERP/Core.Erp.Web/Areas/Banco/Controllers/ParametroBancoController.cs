using Core.Erp.Bus.Banco;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Info.Banco;
using Core.Erp.Web.Helps;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Banco.Controllers
{
    [SessionTimeout]
    public class ParametroBancoController : Controller
    {
        #region Variables
        ba_parametros_Bus bus_parametro = new ba_parametros_Bus();
        ct_cbtecble_tipo_Bus bus_tipo_comprobante = new ct_cbtecble_tipo_Bus();
        ba_Cbte_Ban_tipo_Bus bus_ban_cbte = new ba_Cbte_Ban_tipo_Bus();
        ct_plancta_Bus bus_cta = new ct_plancta_Bus();
        tb_ciudad_Bus bus_ciudad = new tb_ciudad_Bus();
        ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Bus bus_cbteban_x_cbtecble = new ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Bus();
        ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_List Lista_CbteBan_CbteCble = new ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_List();
        #endregion

        #region Index
        public ActionResult Index()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ba_parametros_Info model = bus_parametro.get_info(IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            if (model == null)
            {
                model = new ba_parametros_Info { IdEmpresa = IdEmpresa };
                model.Lista_CbteBan_x_CbteCble = new List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info>();
                Lista_CbteBan_CbteCble.set_list(model.Lista_CbteBan_x_CbteCble, model.IdTransaccionSession);
            }
            else
            {
                model.Lista_CbteBan_x_CbteCble = bus_cbteban_x_cbtecble.GetList(model.IdEmpresa);
                Lista_CbteBan_CbteCble.set_list(model.Lista_CbteBan_x_CbteCble, model.IdTransaccionSession);
            }
                
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(ba_parametros_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            model.Lista_CbteBan_x_CbteCble = Lista_CbteBan_CbteCble.get_list(model.IdTransaccionSession);
            if (!bus_parametro.guardarDB(model))
                ViewBag.mensaje = "No se pudieron actualizar los registros";
            cargar_combos(model.IdEmpresa);
            return View(model);
        }

        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            var lst_tipo_comprobante = bus_tipo_comprobante.get_list(IdEmpresa, false);
            ViewBag.lst_tipo_comprobante = lst_tipo_comprobante;

            var lst_ban_tipo_cbte = bus_ban_cbte.get_list();
            ViewBag.lst_ban_tipo_cbte = lst_ban_tipo_cbte;

            var lst_cta = bus_cta.get_list(IdEmpresa, false, false);
            ViewBag.lst_cta = lst_cta;

            var lst_ciudad = bus_ciudad.get_list("", false);
            ViewBag.lst_ciudad = lst_ciudad;

            Dictionary<string, string> lst_DebCre = new Dictionary<string, string>();
            lst_DebCre.Add("D", "Débito");
            lst_DebCre.Add("C", "Crédito");
            ViewBag.lst_DebCre = lst_DebCre;
        }

        #endregion

        #region Detalle
        [ValidateInput(false)]
        public ActionResult GridViewPartial_CbteBan_x_CbteCble()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            ba_parametros_Info model = new ba_parametros_Info();
            model.IdEmpresa = IdEmpresa;
            model.Lista_CbteBan_x_CbteCble = Lista_CbteBan_CbteCble.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos(model.IdEmpresa);
            return PartialView("_GridViewPartial_CbteBan_x_CbteCble", model);
        }

        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info info_det)
        {
            if (ModelState.IsValid)
                Lista_CbteBan_CbteCble.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ba_parametros_Info model = new ba_parametros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa); 
            model.Lista_CbteBan_x_CbteCble = Lista_CbteBan_CbteCble.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos(model.IdEmpresa);
            return PartialView("_GridViewPartial_CbteBan_x_CbteCble", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info info_det)
        {

            if (ModelState.IsValid)
                Lista_CbteBan_CbteCble.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ba_parametros_Info model = new ba_parametros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.Lista_CbteBan_x_CbteCble = Lista_CbteBan_CbteCble.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos(model.IdEmpresa);
            return PartialView("_GridViewPartial_CbteBan_x_CbteCble", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            Lista_CbteBan_CbteCble.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ba_parametros_Info model = new ba_parametros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.Lista_CbteBan_x_CbteCble = Lista_CbteBan_CbteCble.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos(model.IdEmpresa);
            return PartialView("_GridViewPartial_CbteBan_x_CbteCble", model);
        }
        #endregion
    }

    public class ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_List
    {
        string variable = "ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info";
        public List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info> list = new List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);

            List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            list.Add(info_det);
        }

        public void UpdateRow(ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);

            ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.CodTipoCbteBan = info_det.CodTipoCbteBan;
            edited_info.IdTipoCbteCble = info_det.IdTipoCbteCble;
            edited_info.IdTipoCbteCble_Anu = info_det.IdTipoCbteCble_Anu;
            edited_info.Tipo_DebCred = info_det.Tipo_DebCred;
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<ba_Cbte_Ban_tipo_x_ct_CbteCble_tipo_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(q => q.Secuencia == Secuencia).FirstOrDefault());
        }
    }
}