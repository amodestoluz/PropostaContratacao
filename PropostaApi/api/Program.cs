using Amazon.SQS;
using application.adapters;
using application.ports;
using application.usecases.implementations;
using application.usecases.interfaces;
using infrastructure.adapters.data;
using infrastructure.adapters.messaging;
using infrastructure.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPropostaRepository, PropostaRepository>();
builder.Services.AddScoped<IPropostaService, PropostaService>();
builder.Services.AddScoped<ICriarPropostaUseCase, CriarPropostaUseCase>();
builder.Services.AddScoped<IGetPropostaByIdUseCase, GetPropostaByIdUseCase>();
builder.Services.AddScoped<IListarPropostasUseCase, ListarPropostasUseCase>();
builder.Services.AddScoped<IAlterarStatusPropostaUseCase, AlterarStatusPropostaUseCase>();
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddScoped<IContratacaoPropostaConsumer, ContratacaoPropostaEventAdapter>();
builder.Services.AddHostedService<SqsContratacaoPropostaConsumer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        logger.LogInformation("Tentando aplicar migrações...");
        int retries = 5; 
        while (retries > 0)
        {
            try
            {
                dbContext.Database.Migrate();
                logger.LogInformation("Migrações aplicadas com sucesso!");
                break;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                retries--;
                logger.LogError(ex, "Falha na conexão com o banco de dados. Tentando novamente em 5 segundos...");
                Thread.Sleep(5000); 
                if (retries == 0)
                {
                    throw;
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro fatal ao aplicar migrações.");
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.Run();
