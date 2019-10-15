using Core.Erp.Bus.Facturacion;
using Core.Erp.Info.Facturacion;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class ProbabilidadCobroController : Controller
    {
        #region Variables
        fa_ProbabilidadCobro_Bus bus_probabilidad = new fa_ProbabilidadCobro_Bus();
        fa_ProbabilidadCobro_List Lista_ProbabilidadCobro = new fa_ProbabilidadCobro_List();
        fa_ProbabilidadCobroDet_List Lista_ProbabilidadCobroDet = new fa_ProbabilidadCobroDet_List();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
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

            fa_ProbabilidadCobro_Info model = new fa_ProbabilidadCobro_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            var lst = bus_probabilidad.get_list(model.IdEmpresa, true);
            Lista_ProbabilidadCobro.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ProbabilidadCobro()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<fa_ProbabilidadCobro_Info> model = Lista_ProbabilidadCobro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_ProbabilidadCobro", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion

                fa_ProbabilidadCobro_Info model = new fa_ProbabilidadCobro_Info
                {
                    IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                    IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                };

                model.lst_detalle = new List<fa_ProbabilidadCobroDet_Info>();
                Lista_ProbabilidadCobroDet.set_list(model.lst_detalle, model.IdTransaccionSession);

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(fa_ProbabilidadCobro_Info info)
        {
            try
            {
                info.IdUsuarioCreacion = SessionFixed.IdUsuario;
                info.lst_detalle = Lista_ProbabilidadCobroDet.get_list(info.IdTransaccionSession);

                if (ModelState.IsValid)
                {
                    if (!bus_probabilidad.GuardarDB(info))
                    {
                        ViewBag.mensaje = "No se ha podido guardar el registro";
                        SessionFixed.IdTransaccionSessionActual = info.IdTransaccionSession.ToString();
                        return View(info);
                    }
                    else
                        return RedirectToAction("Modificar", new { IdEmpresa=info.IdEmpresa, IdProbabilidad=info.IdProbabilidad, Exito = true });
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Detalle
        [ValidateInput(false)]
        public ActionResult GridViewPartial_Facturas()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            var model = Lista_ProbabilidadCobroDet.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Facturas", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ProbabilidadCobroDet()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            var model = Lista_ProbabilidadCobroDet.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_ProbabilidadCobroDet", model);
        }
        #endregion

        #region Json
        public JsonResult GetFacturas(int IdEmpresa)
        {
            var resultado = "";

            //var list = bus_factura.get_list(IdEmpresa, IdSucursal, IdCliente, IdProforma);
            //if (list.Count() == 0)
            //    resultado = false;
            //var detalle_factura = List_det.get_list(IdTransaccionSession);
            //if (detalle_factura.Where(v => v.IdProforma == IdProforma).Count() == 0)
            //{
            //    int Secuencia = detalle_factura.Count == 0 ? 1 : detalle_factura.Max(q => q.Secuencia) + 1;
            //    list.ForEach(q => q.Secuencia = Secuencia++);
            //    detalle_factura.AddRange(list);
            //}
            //List_det.set_list(detalle_factura, IdTransaccionSession);



            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class fa_ProbabilidadCobro_List
    {
        string Variable = "fa_ProbabilidadCobro_Info";
        public List<fa_ProbabilidadCobro_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_ProbabilidadCobro_Info> list = new List<fa_ProbabilidadCobro_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_ProbabilidadCobro_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_ProbabilidadCobro_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class fa_ProbabilidadCobroDet_List
    {
        string Variable = "fa_ProbabilidadCobroDet_Info";
        public List<fa_ProbabilidadCobroDet_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_ProbabilidadCobroDet_Info> list = new List<fa_ProbabilidadCobroDet_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_ProbabilidadCobroDet_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_ProbabilidadCobroDet_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}