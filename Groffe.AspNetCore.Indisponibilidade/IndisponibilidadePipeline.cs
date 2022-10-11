using Microsoft.AspNetCore.Builder;

namespace Groffe.AspNetCore.Indisponibilidade;

public class IndisponibilidadePipeline
{
    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseChecagemIndisponibilidade();
    }
}