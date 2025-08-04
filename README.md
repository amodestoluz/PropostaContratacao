# PropostaContratacao
aplicação .net em ambiente docker com sqs, clean arch, hexagonal simples para criar e contratar uma proposta

rodar docker compose up --build 
ao subir http://localhost:5000/swagger/index.html
para acessar o swagger 

contratacao api na 5002
e proposta api na 5000

ultimo migration executado dotnet ef migrations add AddDataContratacaoToProposta --project PropostaApi\infrastructure\infrastructure.csproj --startup-project PropostaApi\api\api.csproj