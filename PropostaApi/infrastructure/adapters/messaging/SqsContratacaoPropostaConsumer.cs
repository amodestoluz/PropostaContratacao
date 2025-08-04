using Amazon.SQS;
using Amazon.SQS.Model;
using application.dto;
using application.ports;
using domain.@enum;
using infrastructure.adapters.messaging.request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace infrastructure.adapters.messaging
{
    public class SqsContratacaoPropostaConsumer : BackgroundService
    {
        private readonly ILogger<SqsContratacaoPropostaConsumer> _logger;
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;
        private readonly IContratacaoPropostaConsumer _eventConsumerPort;

        public SqsContratacaoPropostaConsumer(
            ILogger<SqsContratacaoPropostaConsumer> logger,
            IAmazonSQS sqsClient,
            IContratacaoPropostaConsumer eventConsumerPort,
            IConfiguration configuration)
        {
            _logger = logger;
            _sqsClient = sqsClient;
            _eventConsumerPort = eventConsumerPort;
            _queueUrl = configuration["SQS:ContratacaoPropostaQueueUrl"];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consumidor SQS de Contratação de Proposta iniciado.");

            int retries = 10;
            while (retries > 0 && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _sqsClient.GetQueueAttributesAsync(new GetQueueAttributesRequest
                    {
                        QueueUrl = _queueUrl
                    }, stoppingToken);

                    _logger.LogInformation("Conexão com a fila SQS estabelecida. Começando a consumir mensagens...");
                    break;
                }
                catch (QueueDoesNotExistException)
                {
                    retries--;
                    _logger.LogWarning("Fila SQS não encontrada. Tentando novamente em 3 segundos...");
                    await Task.Delay(3000, stoppingToken);
                    if (retries == 0)
                    {
                        _logger.LogError("Falha ao encontrar a fila SQS após várias tentativas. Encerrando o serviço.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado durante a verificação da fila SQS.");
                    retries--;
                    await Task.Delay(3000, stoppingToken);
                    if (retries == 0)
                    {
                        _logger.LogError("Falha ao encontrar a fila SQS após várias tentativas. Encerrando o serviço.");
                        return;
                    }
                }
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var receiveMessageRequest = new ReceiveMessageRequest
                    {
                        QueueUrl = _queueUrl,
                        MaxNumberOfMessages = 10,
                        WaitTimeSeconds = 20,
                        VisibilityTimeout = 30
                    };

                    var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

                    foreach (var message in receiveMessageResponse.Messages)
                    {
                        _logger.LogInformation($"Mensagem SQS recebida: {message.Body}");

                        try
                        {
                            var contratacaoPropostaEventRequest = JsonSerializer.Deserialize<ContratacaoPropostaEventRequest>(message.Body);

                            if (contratacaoPropostaEventRequest != null)
                            {
                                await _eventConsumerPort.Consume(new AlterarStatusPropostaCommand(contratacaoPropostaEventRequest.IdProposta, StatusPropostaEnum.Contratada));
                                await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
                                {
                                    QueueUrl = _queueUrl,
                                    ReceiptHandle = message.ReceiptHandle
                                }, stoppingToken);

                                _logger.LogInformation($"Mensagem SQS processada e excluída: {message.MessageId}");
                            }
                            else
                            {
                                _logger.LogWarning($"Não foi possível deserializar a mensagem SQS: {message.Body}");
                            }
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError(ex, $"Erro de deserialização JSON na mensagem SQS: {message.Body}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Erro ao processar mensagem SQS: {message.Body}");
                        }
                    }
                }
                catch (QueueDoesNotExistException ex)
                {
                    _logger.LogError($"A fila SQS '{_queueUrl}' não existe. Verifique a configuração. Erro: {ex.Message}");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (AmazonSQSException ex)
                {
                    _logger.LogError(ex, "Erro ao interagir com o SQS. Verifique as credenciais e permissões.");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado no consumidor SQS.");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
            _logger.LogInformation("Consumidor SQS de Contratação de Proposta encerrado.");
        }
    }
}
