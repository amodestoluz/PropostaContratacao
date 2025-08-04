using Amazon.SQS;
using Amazon.SQS.Model;
using application.dto;
using application.ports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace infrastructure.adapters.messaging
{
    public class SqsContratarPropostaMessageProducer : IContratarPropostaMessageProduce
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;
        private readonly ILogger<SqsContratarPropostaMessageProducer> _logger;

        public SqsContratarPropostaMessageProducer(IAmazonSQS sqsClient, IConfiguration configuration, ILogger<SqsContratarPropostaMessageProducer> logger)
        {
            _sqsClient = sqsClient;
            _logger = logger;
            _queueUrl = configuration["SQS:ContratacaoPropostaQueueUrl"] ??
                        throw new ArgumentNullException("SQS:ContratacaoPropostaQueueUrl não configurada.");
        }

      
        public async Task ProduceMessageAsync(ContratarPropostaCommand message)
        {
            try
            {
                var messageBody = JsonSerializer.Serialize(message);
                _logger.LogInformation($"Tentando enviar mensagem SQS para fila {_queueUrl}. Corpo: {messageBody}");

                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MessageBody = messageBody,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        { "EventType", new MessageAttributeValue { DataType = "String", StringValue = "AlterarStatusProposta" } }
                    }
                };

                var response = await _sqsClient.SendMessageAsync(sendMessageRequest);
                _logger.LogInformation($"Mensagem SQS enviada com sucesso. MessageId: {response.MessageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao enviar mensagem SQS para fila {_queueUrl}: {ex.Message}");
                // Em um ambiente real podemos logar e por em uma dlq
                throw new ApplicationException("Falha ao enviar mensagem de alteração de status de proposta para a fila SQS.", ex);
            }
        }
    }
}
