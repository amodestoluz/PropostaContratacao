#!/bin/bash

echo "Executando script de inicialização do LocalStack..."

awslocal sqs create-queue --queue-name contratacao-proposta-queue --region us-east-1 --endpoint-url http://localstack:4566

echo "Fila SQS 'contratacao-proposta-queue' criada com sucesso."