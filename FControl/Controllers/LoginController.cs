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
    public class LoginController : Controller
    {

        private FinancialControlEntities db = new FinancialControlEntities();

        // GET: Login
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                using (FinancialControlEntities db = new FinancialControlEntities())
                {
                    var obj = db.Usuario.Where(a => a.Name.Equals(usuario.Name) && a.Password.Equals(usuario.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UsuarioID"] = obj.UsuarioID.ToString();
                        Session["Name"] = obj.Name.ToString();
                        Session["Imagen"] = obj.Imagen.ToString();
                        Session["Nombre"] = obj.Nombre.ToString();
                        Session["Apellido"] = obj.Apellido.ToString();
                        return RedirectToAction("UserDashFC");
                    }
                }
            }
            return View(usuario);
        }
        public ActionResult Registrar()
        {
  
            return View(new Usuario());
        }
        public ActionResult Guardar(Usuario usuario)
        {
            Validar(usuario);
            if (ModelState.IsValid)
            {
                FinancialControlEntities db = new FinancialControlEntities();
                usuario.Imagen = "/assets/img/avatar.jpg";
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return View("Login", usuario);
            }
            return View("Registrar", usuario);
        }
        public ActionResult UserDashFC()
        {
            if (Session["UsuarioID"] != null)
            {
                return RedirectToAction("Index","Cuenta");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
       
        private void Validar(Usuario usuario)
        {
            if (usuario.Name == null)

               ModelState.AddModelError("Name", "Ingrese Nombre de Usaurio");
            if (usuario.Password == null)

                ModelState.AddModelError("Password", "Ingrese una contraseña");
            if (usuario.Nombre == null)

                ModelState.AddModelError("Nombre", "Ingrese Nombre");
            if (usuario.Apellido == null)

                ModelState.AddModelError("Apellido", "Ingrese Apellido");


        }



    }

}