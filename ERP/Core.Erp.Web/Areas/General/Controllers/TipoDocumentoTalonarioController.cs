using Core.Erp.Bus.General;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Info.General;
using Core.Erp.Web.Helps;
using System;
using System.Web.Mvc;
using Core.Erp.Info.Helps;
using System.Collections.Generic;
using System.Web;

namespace Core.Erp.Web.Areas.General.Controllers
{
    [SessionTimeout]
    public class TipoDocumentoTalonarioController : Controller
    {
        #region Variables
        tb_sis_Documento_Tipo_Talonario_Bus bus_talonario = new tb_sis_Documento_Tipo_Talonario_Bus();
        tb_sis_Documento_Tipo_Bus bus_tipodoc = new tb_sis_Documento_Tipo_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        tb_sis_Documento_Tipo_Talonario_List ListaTalonario = new tb_sis_Documento_Tipo_Talonario_List();
        #endregion

        #region Index / Metodos
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                CodDocumentoTipo = ""
            };
            cargar_combos_consulta();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            ViewBag.IdSucursal = model.IdSucursal;
            ViewBag.CodDocumentoTipo = model.CodDocumentoTipo;
            cargar_combos_consulta();
            return View(model);
        }
        private void cargar_combos_consulta()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, Convert.ToString(SessionFixed.IdUsuario), false);
            lst_sucursal.Add(new tb_sucursal_Info
            {
                IdEmpresa = IdEmpresa,
                IdSucursal = 0,
                Su_Descripcion = "Todos"
            });
            ViewBag.lst_sucursal = lst_sucursal;

            tb_sis_Documento_Tipo_Bus bus_tipo = new tb_sis_Documento_Tipo_Bus();
            var lst_doc = bus_tipo.get_list(false);
            lst_doc.Add(new tb_sis_Documento_Tipo_Info
            {
                codDocumentoTipo = "",
                descripcion = "Todos"
            });
            ViewBag.lst_doc = lst_doc;
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipodocumentotal(int IdSucursal = 0, string CodDocumentoTipo = "")
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            ViewBag.CodDocumentoTipo = CodDocumentoTipo;

            var model = bus_talonario.get_list(IdEmpresa, IdSucursal , CodDocumentoTipo, true);
            return PartialView("_GridViewPartial_tipodocumentotal", model);
        }
        private void cargar_combos(int IdEmpresa)
        {
            var lst_talonario = bus_tipodoc.get_list(false);
            ViewBag.lst_talonario = lst_talonario;
            
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        #endregion

        #region GridActualizacion
        [ValidateInput(false)]
        public ActionResult GridViewPartial_ActualizacionMasiva(int IdSucursal = 0, string CodDocumentoTipo = "", string Establecimiento="", string PuntoEmision="", int NumInicio=0, int NumFin=0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            ViewBag.CodDocumentoTipo = CodDocumentoTipo;
            ViewBag.Establecimiento = Establecimiento;
            ViewBag.PuntoEmision = PuntoEmision;
            ViewBag.NumInicio = NumInicio;
            ViewBag.NumFin = NumFin;
            decimal IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            //var model = bus_talonario.get_list_actualizacion_masiva(IdEmpresa, IdSucursal, CodDocumentoTipo, Establecimiento, PuntoEmision, NumInicio, NumFin);

            var model = ListaTalonario.get_list(IdTransaccionSession);
            //ListaTalonario.set_list(model, IdTransaccionSession);
            return PartialView("_GridViewPartial_ActualizacionMasiva", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0 )
        {
            tb_sis_Documento_Tipo_Talonario_Info model = new tb_sis_Documento_Tipo_Talonario_Info
            {
                IdEmpresa = IdEmpresa,
                FechaCaducidad = DateTime.Now
            };
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(tb_sis_Documento_Tipo_Talonario_Info model)
        {
            decimal documento_inicial = Convert.ToDecimal(model.NumDocumento);
            decimal documento_final = Convert.ToDecimal(model.Documentofinal);
            //for (decimal i = documento_inicial; i < documento_final; i++)
            //{
            //    tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info
            //    {
            //        IdEmpresa = model.IdEmpresa,
            //        CodDocumentoTipo = model.CodDocumentoTipo,
            //        Establecimiento = model.Establecimiento,
            //        PuntoEmision = model.PuntoEmision,
            //        NumDocumento = i.ToString("000000000"),
            //       es_Documento_Electronico = model.es_Documento_Electronico,
            //       FechaCaducidad = model.FechaCaducidad,
            //       IdSucursal = model.IdSucursal,
            //       NumAutorizacion = model.NumAutorizacion,
            //       Usado = model.Usado,                    
            //    };
            //    if (!bus_talonario.guardarDB(info))
            //    {
            //        cargar_combos(model.IdEmpresa);
            //        return View(model);
            //    }
            //}           
            int length = model.NumDocumento.Length;
            string relleno = string.Empty;
            for (int i = 0; i < length; i++)
            {
                relleno += "0";
            }
            decimal secuencia = documento_inicial;
            for (decimal i = documento_inicial; i < documento_final + 1; i++)
            {

                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info
                {
                    IdEmpresa = model.IdEmpresa,
                    CodDocumentoTipo = model.CodDocumentoTipo,
                    NumDocumento = secuencia.ToString(relleno),
                    Establecimiento = model.Establecimiento,
                    PuntoEmision = model.PuntoEmision,
                    EstadoBool = model.EstadoBool,
                    Estado = model.EstadoBool == true ? "A" : "I",
                    Usado = model.Usado,
                    es_Documento_Electronico = model.es_Documento_Electronico,
                    FechaCaducidad = model.FechaCaducidad,
                    IdSucursal = model.IdSucursal,
                    NumAutorizacion = model.NumAutorizacion,
                };
                if (!bus_talonario.guardarDB(info))
                {
                    cargar_combos(model.IdEmpresa);
                    return View(model);
                }
                secuencia++;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Modificar(int IdEmpresa = 0 , string CodDocumentoTipo = "", string Establecimiento = "", string PuntoEmision = "", string NumDocumento = "")
        {
            tb_sis_Documento_Tipo_Talonario_Info model = bus_talonario.get_info(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision, NumDocumento);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(tb_sis_Documento_Tipo_Talonario_Info model)
        {
            if (!bus_talonario.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");

        }

        public ActionResult Anular(int IdEmpresa = 0 , string CodDocumentoTipo = "", string Establecimiento = "", string PuntoEmision = "", string NumDocumento = "")
        {
            tb_sis_Documento_Tipo_Talonario_Info model = bus_talonario.get_info(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision, NumDocumento);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(tb_sis_Documento_Tipo_Talonario_Info model)
        {
            if (!bus_talonario.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ActualizacionMasiva()
        {
            cl_filtros_talonario_Info model = new cl_filtros_talonario_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                CodDocumentoTipo = "",
                NumInicio = 0,
                NumFin =0,
                Establecimiento="",
                PuntoEmision="",
                NumAutorizacion = "",
                FechaCaducidad = DateTime.Now,
                Usado =false,
                EsElectronico = false,
                Anulado = false
            };
            cargar_combos(model.IdEmpresa);
            ListaTalonario.set_list(new List<tb_sis_Documento_Tipo_Talonario_Info>(), model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult ActualizacionMasiva(cl_filtros_talonario_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            var Lista = bus_talonario.get_list_actualizacion_masiva(model.IdEmpresa, model.IdSucursal, model.CodDocumentoTipo, model.Establecimiento, model.PuntoEmision, model.NumInicio, model.NumFin);

            ListaTalonario.set_list(Lista, model.IdTransaccionSession);
            cargar_combos(model.IdEmpresa);
            
            return View(model);

        }
        #endregion

        #region Json
        public JsonResult get_NumeroDocumentoInicial (int IdEmpresa = 0 , string CodDocumentoTipo="", string Establecimiento="", string PuntoEmision = "")
        {
            var NumeroDocumento = bus_talonario.get_NumeroDocumentoInicial(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision);

            return Json(NumeroDocumento, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInfoEstablecimiento(int IdEmpresa = 0, int IdSucursal = 0)
        {
            var resultado = bus_sucursal.get_info(IdEmpresa, IdSucursal);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActualizarRegistros(DateTime FechaCaducidad, int IdSucursal = 0, string NumAutorizacion = "", bool EsElectronico = false, bool Usado = false, bool Anulado = false)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            var resultado = "";
            var ListaActualizacion = ListaTalonario.get_list(IdTransaccionSession);


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class tb_sis_Documento_Tipo_Talonario_List
    {
        string Variable = "tb_sis_Documento_Tipo_Talonario_Info";

        public List<tb_sis_Documento_Tipo_Talonario_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_sis_Documento_Tipo_Talonario_Info> list = new List<tb_sis_Documento_Tipo_Talonario_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_sis_Documento_Tipo_Talonario_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_sis_Documento_Tipo_Talonario_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}