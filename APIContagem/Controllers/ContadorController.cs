using Microsoft.AspNetCore.Mvc;
using APIContagem.Models;
using APIContagem.Logging;
using Groffe.AspNetCore.Indisponibilidade;

namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class ContadorController : ControllerBase
{
    private static readonly Contador _CONTADOR_MIDDLEWAREFILTER = new Contador();
    private static readonly Contador _CONTADOR = new Contador();
    private readonly ILogger<ContadorController> _logger;
    private readonly IConfiguration _configuration;

    public ContadorController(ILogger<ContadorController> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    [MiddlewareFilter(typeof(IndisponibilidadePipeline))]
    public ResultadoContador Get()
    {
        int valorAtualContador;

        lock (_CONTADOR_MIDDLEWAREFILTER)
        {
            _CONTADOR_MIDDLEWAREFILTER.Incrementar();
            valorAtualContador = _CONTADOR_MIDDLEWAREFILTER.ValorAtual;
        }

        _logger.LogValorAtual(nameof(Get), valorAtualContador);

        return new()
        {
            ValorAtual = valorAtualContador,
            Producer = _CONTADOR_MIDDLEWAREFILTER.Local,
            Kernel = _CONTADOR_MIDDLEWAREFILTER.Kernel,
            Framework = _CONTADOR_MIDDLEWAREFILTER.Framework,
            Mensagem = $"*** Com Filter *** {_configuration["MensagemVariavel"]}"
        };
    }

    [HttpGet("semfilter")]
    public ResultadoContador GetContagemSemFilter()
    {
        int valorAtualContador;

        lock (_CONTADOR)
        {
            _CONTADOR.Incrementar();
            valorAtualContador = _CONTADOR.ValorAtual;
        }

        _logger.LogValorAtual(nameof(GetContagemSemFilter), valorAtualContador);

        return new()
        {
            ValorAtual = valorAtualContador,
            Producer = _CONTADOR.Local,
            Kernel = _CONTADOR.Kernel,
            Framework = _CONTADOR.Framework,
            Mensagem = $"*** Sem Filter *** {_configuration["MensagemVariavel"]}"
        };
    }
}