using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Web.Helps;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    [SessionTimeout]
    public class PeriodoContableController : Controller
    {
        #region Variables
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        ct_anio_fiscal_Bus bus_anio = new ct_anio_fiscal_Bus();
        tb_mes_Bus bus_mes = new tb_mes_Bus();
        tb_modulo_Bus bus_modulo = new tb_modulo_Bus();
        ct_periodo_x_tb_modulo_Bus bus_periodo_x_modulo = new ct_periodo_x_tb_modulo_Bus();
        ct_periodo_x_tb_modulo_List Lista_periodo_x_modulo = new ct_periodo_x_tb_modulo_List();
        string mensaje = string.Empty;
        #endregion

        #region Index
        public ActionResult Index(int IdanioFiscal = 0)
        {
            ViewBag.IdanioFiscal = IdanioFiscal;

            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_periodocontable()
        {
            List<ct_periodo_Info> model = new List<ct_periodo_Info>();
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model = bus_periodo.get_list(IdEmpresa, true);
            
            return PartialView("_GridViewPartial_periodocontable", model);
        }

        private void cargar_combos()
        {
            var lst_anio = bus_anio.get_list(false);
            ViewBag.lst_anio_fiscal = lst_anio;

            var lst_mes = bus_mes.get_list();
            ViewBag.lst_Mes = lst_mes;
        }

        #endregion

        #region Metodos del detalle

        [ValidateInput(false)]
        public ActionResult GridViewPartial_Cierre_x_Modulo()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var lst_periodo_x_modulo = Lista_periodo_x_modulo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Cierre_x_Modulo", lst_periodo_x_modulo);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ct_periodo_x_tb_modulo_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            if (info_det != null)
                Lista_periodo_x_modulo.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_periodo_x_modulo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Cierre_x_Modulo", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ct_periodo_x_tb_modulo_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            info_det.IdEmpresa = IdEmpresa;
            if (info_det != null)
                Lista_periodo_x_modulo.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_periodo_x_modulo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Cierre_x_Modulo", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            Lista_periodo_x_modulo.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_periodo_x_modulo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Cierre_x_Modulo", model);
        }

        private bool Validar(ct_periodo_Info i_validar, ref string msg)
        {
            i_validar.lst_periodo_x_modulo = Lista_periodo_x_modulo.get_list(i_validar.IdTransaccionSession);

            foreach (var item1 in i_validar.lst_periodo_x_modulo)
            {
                var contador = 0;
                foreach (var item2 in i_validar.lst_periodo_x_modulo)
                {
                    if (item1.IdModulo == item2.IdModulo)
                    {
                        contador++;
                    }

                    if (contador > 1)
                    {
                        mensaje = "Existen módulos repetidos en el detalle";
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion        

        #region Acciones

        public ActionResult Nuevo(int IdPeriodo = 0)
        {
            cargar_combos();
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            ct_periodo_Info model = new ct_periodo_Info
            {
                IdEmpresa = IdEmpresa,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdPeriodo = IdPeriodo
            };
            Lista_periodo_x_modulo.set_list(new List<ct_periodo_x_tb_modulo_Info>(), model.IdTransaccionSession);

            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ct_periodo_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdUsuario = SessionFixed.IdUsuario;
            model.lst_periodo_x_modulo = Lista_periodo_x_modulo.get_list(model.IdTransaccionSession);

            if (!bus_periodo.guardarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Modificar(int IdPeriodo = 0)
        {
            ct_periodo_Info model = bus_periodo.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdPeriodo);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_periodo_x_modulo = bus_periodo_x_modulo.GetList(model.IdEmpresa, Convert.ToInt32(model.IdPeriodo));
            Lista_periodo_x_modulo.set_list(model.lst_periodo_x_modulo, model.IdTransaccionSession);

            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_periodo_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdUsuario = SessionFixed.IdUsuario;
            model.lst_periodo_x_modulo = Lista_periodo_x_modulo.get_list(model.IdTransaccionSession);

            if (!bus_periodo.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Anular(int IdPeriodo = 0)
        {
            ct_periodo_Info model = bus_periodo.get_info(Convert.ToInt32(SessionFixed.IdEmpresa), IdPeriodo);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_periodo_x_modulo = bus_periodo_x_modulo.GetList(model.IdEmpresa, Convert.ToInt32(model.IdPeriodo));
            Lista_periodo_x_modulo.set_list(model.lst_periodo_x_modulo, model.IdTransaccionSession);

            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_periodo_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdUsuario = SessionFixed.IdUsuario;
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_periodo_x_modulo = Lista_periodo_x_modulo.get_list(model.IdTransaccionSession);

            if (!bus_periodo.anularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class ct_periodo_x_tb_modulo_List
    {
        string Variable = "ct_periodo_x_tb_modulo_Info";
        public List<ct_periodo_x_tb_modulo_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ct_periodo_x_tb_modulo_Info> list = new List<ct_periodo_x_tb_modulo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ct_periodo_x_tb_modulo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ct_periodo_x_tb_modulo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ct_periodo_x_tb_modulo_Info info_det, decimal IdTransaccionSession)
        {
            List<ct_periodo_x_tb_modulo_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            info_det.IdModulo = info_det.IdModulo;
            info_det.IdUsuario = SessionFixed.IdUsuario;
            info_det.IdUsuarioUltModi = SessionFixed.IdUsuario;

            list.Add(info_det);
        }

        public void UpdateRow(ct_periodo_x_tb_modulo_Info info_det, decimal IdTransaccionSession)
        {
            ct_periodo_x_tb_modulo_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.IdModulo = info_det.IdModulo;
            edited_info.IdUsuarioUltModi = SessionFixed.IdUsuario;
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<ct_periodo_x_tb_modulo_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).FirstOrDefault());
        }
    }
}