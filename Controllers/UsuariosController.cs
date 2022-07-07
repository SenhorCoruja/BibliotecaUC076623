using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult ListaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View(new UsuariosService().Listar());
        }

        public IActionResult editarUsuario(int id)
        {
            Usuario u = new UsuariosService().Listar(id);

            return View(u);
        }

        [HttpPost]
        public IActionResult editarUsuario(Usuario userEditado)
        {
            UsuariosService us = new UsuariosService();
            us.editarUsuario(userEditado);

            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult RegistrarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuarios(Usuario novoUser)
        {
           Autenticacao.CheckLogin(this);
           Autenticacao.verificaSeUsuarioEAdmin(this);

           novoUser.senha = Criptografo.TextoCriptografado(novoUser.senha);

           UsuariosService us = new UsuariosService();
           us.incluirUsuario(novoUser);

           return RedirectToAction("cadastroRealizado");
        }

        public IActionResult ExcluirUsuario(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View(new UsuariosService().Listar(id));
        }

        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao, int id)
        {
            
            if(decisao=="EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do usuario "+new UsuariosService().Listar(id).Nome+" realizada com sucesso";
                new UsuariosService().excluirUsuario(id);
                return View("ListaDeUsuarios" ,new UsuariosService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão cancelada";
                return View("ListaDeUsuarios", new UsuariosService().Listar());
            }


        }
        public IActionResult cadastroRealizado()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }

        public IActionResult NeedAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}