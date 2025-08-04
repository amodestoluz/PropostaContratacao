using Amazon.SQS;
using application.adapters;
using application.ports;
using application.usecases.implementation;
using application.usecases.interfaces;
using infrastructure.adapters.httpclients;
using infrastructure.adapters.messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IPropostaExternalService, PropostaApiHttpClient>(client =>
{
    var baseUrl = builder.Configuration["PropostaApi:BaseUrl"] ??
                  throw new ArgumentNullException("PropostaApi:BaseUrl não configurado em appsettings.json ou variáveis de ambiente.");
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddScoped<IContratacaoPropostaService, ContratacaoPropostaService>();
builder.Services.AddScoped<IContratarPropostaUseCase, ContratarPropostaUseCase>();
builder.Services.AddScoped<IContratarPropostaMessageProduce, SqsContratarPropostaMessageProducer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
