using Automovel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Automovel.Controllers
{
    public class MontadoraController : Controller
    {
        private readonly BancoContext _context;

        public MontadoraController(BancoContext contexto)
        {
            _context = contexto;
        }

        public IActionResult Index()
        {
            string datetime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            ViewBag.DataHora = datetime;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if(id.HasValue)
            {

                var montadora = await _context.Montadoras.FindAsync(id);
                if (montadora == null)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Montadora não encontrada.");
                    return RedirectToAction("Catalogo");
                }
                return View(montadora);
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Valor não informado.");
                return RedirectToAction("Catalogo");

            }
            
            
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var montadora = await _context.Montadoras.FindAsync(id);
            if(montadora != null)
            {
                _context.Montadoras.Remove(montadora);
                if (await _context.SaveChangesAsync() > 0)
                    TempData["mensagem"] = MensagemModel.Serializar("Montadora excluída com sucesso.");
                else
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir a Montadora.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Catalogo));
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Montadora não encontrada.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Catalogo));
            }

        }

        public async Task<IActionResult> Catalogo()
        {
            return View(await _context.Montadoras.OrderBy(x => x.Name).AsNoTracking().ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Cadastro(int? id)
        {
            if (id.HasValue)
            {
                var montadora = await _context.Montadoras.FindAsync(id);
                if(montadora == null)
                {
                    return NotFound();
                }
                return View(montadora);
            }
            return View(new MontadoraModel());
        }

        private bool MontadoraExiste(int id)
        {
            return _context.Montadoras.Any(x => x.IdMontadora == id);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(int? id, [FromForm] MontadoraModel montadora)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (MontadoraExiste(id.Value))
                    {
                        _context.Montadoras.Update(montadora);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Montadora alterada com sucesso.");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar Montadora.", TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Montadora não encontrada.", TipoMensagem.Erro);
                    }
                }
                else
                {
                    _context.Montadoras.Add(montadora);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Montadora cadastrada com sucesso.");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro ao cadastrar Montadora.", TipoMensagem.Erro);
                    }
                }
                return RedirectToAction("Catalogo");
            }
            else
            {
                return View(montadora);
            }
        }

    }
}
