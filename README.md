![](https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/docs/github-social-preview.png)
# O que é?
Em 2009, com a necessidade de aprender e praticar a utilização dos componentes da [EXT.NET](https://ext.net/) (pois a empresa onde eu trabalhava iria adquirir esse componentes para utilizar em seus produtos), pensei em criar um *"sisteminha"* (simples e de escopo pequeno), que me permitisse aprender e também me auxiliasse em alguma tarefa cotidiana (para não ter que criar mais um sistema de locador, padaria ou algo do tipo...). Então pensei: "Por que não, criar um sistema Web, que me auxilie na minha gestão financeira (controle das minhas receitas e despesas), na gestão da minha **bufunfa**?". :moneybag: :dollar: :credit_card: :money_with_wings:

Pronto...assim nasceu esse projeto!

Desde então, esse projeto já foi contruído em ASP.NET 2.0 - webforms com EXT.NET, passou para ASP.NET MVC + Bootstrap, utilizou banco de dados SQL Server, architetura *BOLOVO* (sim, porque não!? :sob:), persistência utilizando *POCOs*, Entity Framework (nos primórdios...), integração com webservices, passou por algumas arquiteturas, repositórios, etc...até chegarmos na arquitetura atual: 

**Frontend** ([Metronic](https://keenthemes.com/metronic/) + ASP.NET MVC Core 3.1)
<p align="center">
  <img src="https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/docs/cena-1.gif">
  <img src="https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/docs/cena-2.gif">
  <img src="https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/docs/cena-3.gif">
</p>

**Backend** (API ASP.NET Core + MySQL)
![Backend](https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/docs/tela-backend-01.png)

Em "produção" atualmente, existe uma versão em http://bufunfa.jnogueira.net.br, rodando em uma máquina com 1 GB de memória, 25 GB de disco com Ubuntu 18.04 na [Digital Ocean](https://www.digitalocean.com/), com [Docker](https://www.docker.com)! :heart_eyes:

## Algumas funcionalidades
* Cadastro de contas, cartões de créditos e ativos
* Cadastro de agendamentos (compras a prestação, débitos ou créditos mensais)
* Cadastro de lançamentos em conta
* Cadastro de categorias (permitem categorizar lançamentos e agendamentos)
* Gestão de faturas de cartão de crédito
* Emissão de extrato de uma conta

## Algumas características...
* Backend com documentação utilizando [Swashbuckle (Swagger)](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio) (exemplo em http://bufunfa.jnogueira.net.br/api)
* Integração com o [Google Drive API](https://developers.google.com/drive), para armazenamento de arquivos.
* Integração com a API [Alpha Vantage](https://www.alphavantage.co/), para obtenção de indicadores financeiros.
* Utilização do [Discord](https://discordapp.com/) como repositório do log (utilizando o componente [Discord .NET Logger Provider](https://github.com/jlnpinheiro/logger-discord-provider)).
* Geração de PDF utilizando o componente [Rotativa](https://github.com/webgio/Rotativa.AspNetCore).

## Alguns conceitos utilizados
* Repository pattern
* "Unit of Work" pattern
* Injeção de dependência
* Domains notifications (utilizando o componente [Notifique me!](https://github.com/jlnpinheiro/notifique-me-csharp))

# Colocando pra funcionar...
É possível subir um ambiente necessário para o funcionamento do Bufunfa de duas maneiras: 
* Utilizando containers
* Publicando individualmente os projetos

## Configurações necessárias (ou não...)
Para o correto funcionamento do sistema, é necessário configurar algumas variáveis de ambientes. Dependendo da maneira escolhida para subir o ambiente, essas variáveis deverão ser informadas:
* Container => variáveis definidas no arquivo **docker-compose.yml** ou no comando *docker run*;
* Publicando individualmente os projetos => variáveis definidas no arquivo **appSettings.json**;

Exemplo de arquivo *appSettings.json*:
```json
{
    "BUFUNFA_BANCO_DADOS_CONNECTION_STRING": "server=localhost;Port=3306;user id=bufunfa;password=p@ssw0rd;database=db_bufunfa;SslMode=none;",
    "BUFUNFA_DISCORD_WEBHOOK_URL": "https://discordapp.com/api/webhooks/xpto",
    "BUFUNFA_GOOGLE_DRIVE_ID_PASTA_ANEXO": "23651283768687s6sa",
    "BUFUNFA_API_ALPHA_VANTAGE_KEY": "abcd"
}
```
Exemplo de arquivo *docker-compose.yml*:
```docker
...

# Backend - API ASP.NET Core 3.1
  bufunfa-backend:
    depends_on:
        - bufunfa-db
    image: jlnpinheiro/bufunfa:1.0.0-backend
    container_name: bufunfa-backend
    environment:
        # String de conexão para acesso ao banco
        BUFUNFA_BANCO_DADOS_CONNECTION_STRING: "server=localhost;Port=3306;user id=bufunfa;password=p@ssw0rd;database=db_bufunfa;SslMode=none;"
        # URL do webhook do Discord (não obrigatório)
        BUFUNFA_DISCORD_WEBHOOK_URL: "https://discordapp.com/api/webhooks/xpto" 
        # ID da pasta no Google Drive onde os anexos serão armazenados (não obrigatório)
        BUFUNFA_GOOGLE_DRIVE_ID_PASTA_ANEXO: "23651283768687s6sa"  
        # Key para utilizar a API da Alpha Vantage (consulta de indicadores financeiros de ativos)
        BUFUNFA_API_ALPHA_VANTAGE_KEY: "abcd" 
    restart: always
    networks:
        - bufunfa-network

...
```

As variáveis de ambiente são:

* **BUFUNFA_BANCO_DADOS_CONNECTION_STRING** (obrigatória): string de conexão para acesso ao banco de dados MySQL. Caso o ambiente do sistema seja criado utilizando containers, na string de conexão, o parâmetro *server* deve conter o nome do container contendo o banco de dados MySQL.

* **BUFUNFA_DISCORD_WEBHOOK_URL** (opcional): URL do webhook do Discord, que será utilizado para o envio das mensagens. Para maiores informações de como criar um webhook do Discord, acesse [aqui](https://support.discordapp.com/hc/pt-br/articles/228383668-Usando-Webhooks).

* **BUFUNFA_API_ALPHA_VANTAGE_KEY** (opcional): o Bufunfa pode exibir os valores de alguns ativos. Para isso ele utiliza a integração com a API da [Alpha Vantage](https://www.alphavantage.co/). Para utilizar essa API, você deve criar uma chave. Para obter a sua chave grátis, acesse https://www.alphavantage.co/support/#api-key

* **"BUFUNFA_BACKEND_URL** (obrigatoria): URL para acesso ao *backend*.

* **BUFUNFA_GOOGLE_DRIVE_ID_PASTA_ANEXO** (opcional): o Bufunfa permite para um lançamento em um conta, anexar arquivos (comprovantes de pagamentos, recibos, boleto, etc). Esses arquivos são armazenados em uma pasta do Google Drive. O ID dessa pasta, deverá ser armazenado nessa variável.

No Google Drive, basta criar ou acessar um pasta já existente. Na URL, o código final se refere ao ID da pasta. Veja o exemplo:

```
https://drive.google.com/drive/u/0/folders/1SIlmDfZBepgzZ4qOEdiuy879y7d12908 (1SIlmDfZBepgzZ4qOEdiuy879y7d12908 é o ID da pasta no Google Drive)
```

Além de definir o ID da pasta, para integração com o Google Drive, é preciso criar um arquivo (denominado *google_credentials.json*) contendo as credenciais de acesso para utilização da API do Google Drive. Esse arquivo deverá existir na pasta root do *backend*. Para mais informações de como criar o arquivo *google_credentials.json* clique [aqui](https://cloud.google.com/iam/docs/creating-managing-service-account-keys?hl=pt-br). Veja um exemplo desse arquivo:

```json
{
    "type": "service_account",
    "project_id": "[PROJECT-ID]",
    "private_key_id": "[KEY-ID]",
    "private_key": "-----BEGIN PRIVATE KEY-----\n[PRIVATE-KEY]\n-----END PRIVATE KEY-----\n",
    "client_email": "[SERVICE-ACCOUNT-EMAIL]",
    "client_id": "[CLIENT-ID]",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://accounts.google.com/o/oauth2/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_x509_cert_url": "https://www.googleapis.com/robot/v1/metadata/x509/[SERVICE-ACCOUNT-EMAIL]"
}
```

## Utilizando containers
O Bufunfa, em sua arquitetura, foi divido em 3 containers: 
1. *Backend* (API)
2. *Frontend* (MVC)
3. Banco de dados *MySQL* 

As imagens para geração desses containers podem ser encontradas no [meu perfil](https://hub.docker.com/repository/docker/jlnpinheiro/bufunfa) do Docker Hub!

No arquivo [docker-compose.yml](https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/src/_docker/docker-compose.yml) está as parametrizações necessárias para subir o ambiente. Para subir o ambiente com todos os containers, basta executar o comando:

```
docker-compose -f docker-compose.yml -p bufunfa up -d
```

Nas pastas onde estão os fontes do *backend* e do *frontend* existem os arquivos **dockerfile** para geração dessas mesmas imagens disponibilizadas no Docker Hub. Para criar as imagens, basta executar o comando:

```
 docker build -t <nome da imagem> . --no-cache
```
**Observação:** se no comando *docker build* você der outro nome para a imagem e for subir o ambiente utilizando o arquivo *docker-compose.yml*, lembre-se de alterar o nome das imagens lá referenciadas.

A imagem **jlnpinheiro/bufunfa:1.0.0-database** já está configurada para quando o container subir, executar um script de geração do banco de dados utilizado pelo sistema. Esse script pode ser visualizado [aqui](https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/src/_docker/docker-image-bufunfa-mysql/_script/create-database.sql).

Ao executar o comando *docker-compose*, você pode acessar os container pelas seguintes URLs:

* Backend: http://localhost:5000
* Frontend: http://localhost:5001

## Publicando individualmente os projetos

Individualmente, você pode publicar os fontes do *backend* e do *frontend* e executá-los. Levantando o ambiente nesse modelo, não esqueça de definir corretamente as variáveis de ambiente no arquivo **appSettings.json**.

Para criar o banco de dados MySQL utilizado pelo sistema, você deve utilizar o seguinte [script](https://raw.githubusercontent.com/jlnpinheiro/bufunfa/master/src/_docker/docker-image-bufunfa-mysql/_script/create-database.sql).

## Acessando o sistema

Após montar e levantar o ambiente escolhido por você, acesse a URL do *frontend* e informe as seguintes credenciais:

```
Login: administrador@bufunfa.net
Senha: admin
```

Se tudo estiver ok...bem-vindo e faça bom uso do Bufunfa!


## Feedback...sempre muito bem-vindo!

 Como já falei anteriormente, criei esse sistema com o objetivo de aprender e praticar meu conhecimento! A maioria dos conceitos aqui utilizados, utilizo bastante também em projetos profissionais.

 Por isso, toda e qualquer sugestão, crítica, dúvida, um PR (por que não!?) é sempre muito bem-vindo! :+1: :facepunch: :pray:
