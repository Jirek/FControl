using FControl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace FControl.Controllers
{
    
    public class CuentaController : Controller
    {
        private FinancialControlEntities db;
        public CuentaController()
            {
            db =  new FinancialControlEntities();
            }

        public ActionResult Index(int? id)
        {
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            
            var cuenta = db.Cuenta.Where(d => d.UsuarioID ==userId|| d.CompartidoID == userId).Include("Divisa").Include("Transaccion").ToList();

            return View(cuenta);
        }
        public ActionResult RegistroCuenta(int? id)
        {
            
            var transaccion = db.Transaccion.Where(d => d.CuentaID ==  id ).Include("Cuenta");

            return View(transaccion.ToList());
        }
        public ActionResult Opciones(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuenta.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            return PartialView(cuenta);
        }
        public ActionResult Detalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuenta.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            return PartialView(cuenta);
        }
        public ActionResult CrearRegistro()
        {
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            ViewBag.BenefeciarioID = new SelectList(db.Beneficiario.Where(d => d.UsuarioID == userId), "BeneficiarioID", "Nombre");

            ViewBag.CuentaID = db.Cuenta.Where(d => d.UsuarioID == userId || d.CompartidoID == userId).ToList();

            ViewBag.DivisaID = new SelectList(db.Divisa, "DivisaID", "Nombre");
            return View();
        }
        public ActionResult Crear()
        {
            ViewBag.DivisaID = db.Divisa.ToList();

            return View();
        }
        public ActionResult CrearDebito()
        {
            ViewBag.DivisaID = db.Divisa.ToList();
            ViewBag.EntidadEmisoraID = db.EntidadEmisora.ToList();
            return View();
        }
        public ActionResult CrearCredito()
        {
            ViewBag.DivisaID = db.Divisa.ToList();
            ViewBag.EntidadEmisoraID = db.EntidadEmisora.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearRegistro( Transaccion transaccion, Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                object fc = Session["UsuarioID"].ToString();

                transaccion.FechaTransaccion = DateTime.Now;
                transaccion.UsuarioID = Convert.ToInt16(fc);
                transaccion.Concepto = " : " + transaccion.Concepto;

                if (transaccion.TipoTransaccion == 1 && transaccion.DivisaID == 1 && cuenta.DivisaID==2)
                {
                    transaccion.ImporteOtraDivisa = transaccion.Importe /3.25;
                    transaccion.Importe = transaccion.Importe /3.25;
                }

                if (transaccion.TipoTransaccion == 2 && transaccion.DivisaID == 1 )
                {
                    transaccion.ImporteOtraDivisa = transaccion.Importe*-1;
                    transaccion.Importe= transaccion.Importe * -1;
                }
                if (transaccion.TipoTransaccion == 2 && transaccion.DivisaID == 2)
                {
                    transaccion.ImporteOtraDivisa = transaccion.Importe * -1*3.25;
                    transaccion.Importe = transaccion.Importe * -1;
                }
                if (transaccion.TipoTransaccion == 1 && transaccion.DivisaID == 1)
                {
                    transaccion.ImporteOtraDivisa = transaccion.Importe;
                    
                }
                if (transaccion.TipoTransaccion == 1 && transaccion.DivisaID == 2)
                {
                    transaccion.ImporteOtraDivisa = transaccion.Importe*3.25;
                    
                }

                db.Transaccion.Add(transaccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            ViewBag.BenefeciarioID = new SelectList(db.Transaccion.Where(d => d.UsuarioID == userId), "BeneficiarioID", "Nombre", transaccion.BenefeciarioID);

            ViewBag.CuentaID = db.Cuenta.Where(d => d.UsuarioID == userId || d.CompartidoID == userId).ToList();

            ViewBag.DivisaID = new SelectList(db.Divisa, "DivisaID", "Nombre", transaccion.CuentaID);


            return View(transaccion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Cuenta cuenta, Transaccion transaccion)
        {
            ValidarEfectivo(cuenta);
            if (ModelState.IsValid)
            {
                object fc = Session["UsuarioID"].ToString();
                cuenta.FechaCuenta = DateTime.Now;
                cuenta.UsuarioID = Convert.ToInt16(fc);
                cuenta.TipoCuentaID = 1;
                if (cuenta.SaldoInicial >= 0)
                {
                    transaccion.Concepto = "Saldo inicial (" + cuenta.Nombre + ")";
                    transaccion.Importe = cuenta.SaldoInicial;
                    if (cuenta.DivisaID ==2 )
                    {
                        transaccion.ImporteOtraDivisa = cuenta.SaldoInicial * 3.25;
                    }

                    if (cuenta.DivisaID == 1)
                    {
                        transaccion.ImporteOtraDivisa = cuenta.SaldoInicial * 1;
                    }
                    transaccion.CuentaID = cuenta.CuentaID;
                    transaccion.UsuarioID = Convert.ToInt16(fc);

                    transaccion.FechaTransaccion = DateTime.Now;
                    db.Transaccion.Add(transaccion);
                    
                }
                db.Cuenta.Add(cuenta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisaID = db.Divisa.ToList();
            return View(cuenta);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearDebito( Cuenta cuenta, Transaccion transaccion)
        {
            ValidarTarjeta(cuenta);
            if (ModelState.IsValid)
            {
                object fc = Session["UsuarioID"].ToString();

                cuenta.FechaCuenta = DateTime.Now;
                cuenta.UsuarioID = Convert.ToInt16(fc);
                cuenta.TipoCuentaID = 2;

                if (cuenta.SaldoInicial >= 0)
                {
                    transaccion.Concepto = "Saldo inicial (" + cuenta.Nombre + ")";
                    transaccion.Importe = cuenta.SaldoInicial;
                    transaccion.CuentaID = cuenta.CuentaID;
                    if (cuenta.DivisaID == 2)
                    {
                        transaccion.ImporteOtraDivisa = cuenta.SaldoInicial * 3.25;
                    }

                    if (cuenta.DivisaID == 1)
                    {
                        transaccion.ImporteOtraDivisa = cuenta.SaldoInicial * 1;
                    }
                    transaccion.UsuarioID = Convert.ToInt16(fc);
                    transaccion.FechaTransaccion = DateTime.Now;
                    db.Transaccion.Add(transaccion);
                }
                db.Cuenta.Add(cuenta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisaID = db.Divisa.ToList();
            ViewBag.EntidadEmisoraID = db.EntidadEmisora.ToList();
            return View(cuenta);

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCredito(Cuenta cuenta, Transaccion transaccion)
        {
            ValidarTarjetaCredito(cuenta);
            if (ModelState.IsValid)
            {
                object fc = Session["UsuarioID"].ToString();

                cuenta.FechaCuenta = DateTime.Now;
                cuenta.UsuarioID = Convert.ToInt16(fc);
                cuenta.TipoCuentaID = 3;
                if (cuenta.SaldoInicial >= 0)
                {

                    transaccion.Concepto = "Salario inicial (" + cuenta.Nombre + ")";
                    transaccion.Importe = cuenta.SaldoInicial;
                    transaccion.CuentaID = cuenta.CuentaID;
                    transaccion.UsuarioID = Convert.ToInt16(fc);
                    transaccion.FechaTransaccion = DateTime.Now;
                    db.Transaccion.Add(transaccion);
                }
                db.Cuenta.Add(cuenta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisaID = db.Divisa.ToList();
            ViewBag.EntidadEmisoraID = db.EntidadEmisora.ToList();

            return View(cuenta);

        }
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuenta.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.DivisaID = db.Divisa.ToList();
            return PartialView(cuenta);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                Cuenta cu = db.Cuenta.Find(cuenta.CuentaID);
                cu.Nombre = cuenta.Nombre;
                cu.DivisaID = cuenta.DivisaID;
                cu.Concepto = cuenta.Concepto;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisaID = db.Divisa.ToList();
            return View(cuenta);
        }
        public ActionResult CompartirCuenta(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuenta.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioID = db.Usuario.ToList();
            return PartialView(cuenta);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompartirCuenta(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                Cuenta cu = db.Cuenta.Find(cuenta.CuentaID);
                cu.CompartidoID = cuenta.CompartidoID;         
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = db.Usuario.ToList();
            return View(cuenta);
        }
       
       
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuenta.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuenta cuenta = db.Cuenta.Find(id);
            db.Cuenta.Remove(cuenta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private void ValidarTarjeta(Cuenta cuenta)
        {
            if (cuenta.Nombre == null)
            
                ModelState.AddModelError("Nombre", "Ingrese Nombre");
             
            if (cuenta.DivisaID == null)
            
                ModelState.AddModelError("Divisa", "Seleccione Divisa");

            if (cuenta.EntidadEmisoraID == null)

                ModelState.AddModelError("EntidadEmisoraID", "Seleccione Entidad");

        }
        private void ValidarTarjetaCredito(Cuenta cuenta)
        {
            if (cuenta.Nombre == null)

                ModelState.AddModelError("Nombre", "Ingrese Nombre");

            if (cuenta.DivisaID == null)

                ModelState.AddModelError("Divisa", "Seleccione Divisa");

            if (cuenta.EntidadEmisoraID == null)

                ModelState.AddModelError("EntidadEmisoraID", "Seleccione Entidad");
            if(cuenta.DiaCierre == null)
                ModelState.AddModelError("DiaCierre", "Seleccione día de Cierre");
            if (cuenta.DiaPago == null)
                ModelState.AddModelError("DiaPago", "Seleccione día de Pago");
        }
        private void ValidarEfectivo(Cuenta cuenta)
        {
            if (cuenta.Nombre == null)

                ModelState.AddModelError("Nombre", "Ingrese Nombre");

            if (cuenta.DivisaID == null)

                ModelState.AddModelError("Divisa", "Seleccione Divisa");

           
        }

    }
}