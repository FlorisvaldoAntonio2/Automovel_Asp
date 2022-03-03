using Automovel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Automovel.Controllers
{
    public class AutomovelController : Controller
    {
        //inico - injeção de dependencia
        private readonly BancoContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AutomovelController(BancoContext contexto, IWebHostEnvironment hostEnvironment)
        {
            _context = contexto;
            webHostEnvironment = hostEnvironment;
        }
        //fim - da injeção de dependencia

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id.HasValue)
            {

                var automovel = await _context.Automoveis.FindAsync(id);
                if (automovel == null)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Automovel não encontrada.");
                    return RedirectToAction("Catalogo");
                }
                return View(automovel);
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
            var automovel = await _context.Automoveis.FindAsync(id);
            if (automovel != null)
            {
                string caminhoCompleto = Path.Combine(webHostEnvironment.WebRootPath, "Imagens") + "\\" + automovel.Foto;
               
                _context.Automoveis.Remove(automovel);

                if (await _context.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Automovel excluído com sucesso.");
                    if(automovel.Foto != null)
                        System.IO.File.Delete(caminhoCompleto);
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir o automovel.", TipoMensagem.Erro);
                }
                return RedirectToAction(nameof(Catalogo));
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Automovel não encontrada.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Catalogo));
            }

        }
        //com o termo ASYNC definimos que o metodo vai ultilizar o recurso assincrono
        //TASK<TIPO RETORNO> defimos que o metodo retorna uma tarefa do tipo estabelecido no parametro generico
        public async Task<IActionResult> Catalogo()
        {
            //await determina que vamos esperar o retorno, porém não na linha principal de execução, assim podemos execultar 
            //outras ações
            return View(await _context.Automoveis.OrderBy(x => x.Modelo).Include(x => x.Montadora).AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int? id)
        {
            SelectMontadoras();

            if (id.HasValue)
            {
                var auto = await _context.Automoveis.FindAsync(id);
                if (auto == null)
                {
                    return NotFound();
                }
                AutomovelViewModel employee = new AutomovelViewModel
                {
                    IdAutomovel = auto.IdAutomovel,
                    Modelo = auto.Modelo,
                    Cor = auto.Cor,
                    Portas = auto.Portas,
                    Ano = auto.Ano,
                    Quilometragem = auto.Quilometragem,
                    IdMontadora = auto.IdMontadora
                };
                return View(employee);
            }
            return View(new AutomovelViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(int? id, [FromForm] AutomovelViewModel auto)
        {
            //modelo é valido
            if (ModelState.IsValid)
            {
                if (!ValidaExtensao(auto.Foto))
                {
                    SelectMontadoras();
                    TempData["mensagem"] = MensagemModel.Serializar("Extensão INVALIDA!!.", TipoMensagem.Erro);
                    return View(auto);
                }
                //existe algum valor no parametro id
                //caso sim é uma atualização
                if (id.HasValue)
                {
                    if (AutomovelExiste(id.Value))
                    {
                        var nomeUnicoArquivo = UploadedFile(auto);
                        AutomovelModel employee = new AutomovelModel
                        {
                            IdAutomovel = auto.IdAutomovel,
                            Modelo = auto.Modelo,
                            Cor = auto.Cor,
                            Portas = auto.Portas,
                            Ano = auto.Ano,
                            Quilometragem = auto.Quilometragem,
                            Foto = nomeUnicoArquivo,
                            IdMontadora = auto.IdMontadora
                        };
                        _context.Automoveis.Update(employee);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Automovel alterado com sucesso.");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar o Automovel.", TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Automovel não encontrada.", TipoMensagem.Erro);
                    }
                }
                //caso negativo é uma inserção
                else
                {
                    var nomeUnicoArquivo = UploadedFile(auto);
                    AutomovelModel employee = new AutomovelModel
                    {
                        Modelo = auto.Modelo,
                        Cor = auto.Cor,
                        Portas = auto.Portas,
                        Ano = auto.Ano,
                        Quilometragem = auto.Quilometragem,
                        Foto = nomeUnicoArquivo,
                        IdMontadora = auto.IdMontadora
                    };
                    _context.Automoveis.Add(employee);
                    if(await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Automovel adicionado com sucesso!");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Automovel adicionado com sucesso!",TipoMensagem.Erro);
                    }
                    
                }
                return RedirectToAction("Catalogo");
            }
            //caso não seja valido, retornamos o objeto com os dados de erro
            else
            {
                SelectMontadoras();
                return View(auto);
            }

        }

        public async Task<IActionResult> Exibir(int? id)
        {
            if (id.HasValue)
            {

                var automovel = await _context.Automoveis.FindAsync(id);
                if (automovel == null)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Automovel não encontrado.");
                    return RedirectToAction("Catalogo");
                }
                return View(automovel);
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Valor não informado.");
                return RedirectToAction("Catalogo");

            }
        }

        //metodos de helper
        private void SelectMontadoras()
        {
            var montadoras = _context.Montadoras.OrderBy(x => x.Name).AsNoTracking().ToList();
            //um object Select list, recebe 3 parametros, SelectList(fonte de dados, propriedade que vamos usar como valor, e valor para a exibição)
            var montadorasSelectList = new SelectList(montadoras,
                nameof(MontadoraModel.IdMontadora), nameof(MontadoraModel.Name));
            ViewBag.Montadoras = montadorasSelectList;
        }
        private string UploadedFile(AutomovelViewModel model)
        {
            string nomeUnicoArquivo = null;
            if (model.Foto != null)
            {
                string pastaFotos = Path.Combine(webHostEnvironment.WebRootPath, "Imagens");
                nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                string caminhoArquivo = Path.Combine(pastaFotos, nomeUnicoArquivo);
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                }
            }
            return nomeUnicoArquivo;
        }
        private bool ValidaExtensao(IFormFile uploadedFileName)
        {
            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };

            if(uploadedFileName != null)
            {
                var ext = Path.GetExtension(uploadedFileName.FileName).ToLowerInvariant();

                if (!permittedExtensions.Contains(ext))
                {
                    return false;
                }
            }

            return true;
        }
        private bool AutomovelExiste(int id)
        {
            return _context.Automoveis.Any(x => x.IdAutomovel == id);
        }
    }
}
