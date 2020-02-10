![](https://raw.githubusercontent.com/jlnpinheiro/bufunfa-net-core-3.1/master/docs/banner-readme.png)
# O que é?
Em 2009, com a necessidade de aprender e praticar a utilização dos componentes da [EXT.NET](https://ext.net/) (pois a empresa onde eu trabalhava iria adquirir para utilizar em seus produtos), pensei em criar um *"sisteminha"* (simples e de escopo pequeno), que me permitisse aprender e também me auxiliasse em alguma tarefa cotidiana. Por que não, criar um sistema Web, que me auxiliasse na minha gestão financeira (controle das minhas receitas e despesas), na gestão da minha **bufunfa**? Pronto! E assim nasceu esse projeto!

Desde então, esse projeto já foi contruído em ASP.NET 2.0 - webforms com EXT.NET, passou para ASP.NET MVC + Bootstrap, utilizou banco de dados SQL Server, architetura *BOLOVO* (sim, porque não!?), persistência utilizando *POCOs*, Entity Framework (nos primórdios...), integração com webservices, passou por algumas arquiteturas, repositórios, etc...até chegarmos na arquitetura atual: *Frontend* ([Metronic](https://keenthemes.com/metronic/) + ASP.NET MVC Core 3.1 com C#) e *Backend* (API ASP.NET Core + MySQL).

![Frontend](https://raw.githubusercontent.com/jlnpinheiro/bufunfa-net-core-3.1/master/docs/tela-01.png)
![Backend](https://raw.githubusercontent.com/jlnpinheiro/bufunfa-net-core-3.1/master/docs/tela-backend-01.png)

Em "produção" atualmente, existe uma versão em http://bufunfa.jnogueira.net.br, rodando em uma máquina com 1 GB de memória, 25 GB de disco com Ubuntu 18.04 na [Digital Ocean](https://www.digitalocean.com/), com [Docker](https://www.docker.com)!

## Algumas características...
* Backend com documentação utilizando [Swashbuckle (Swagger)](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio) (exemplo em http://bufunfa.jnogueira.net.br/api)
* Integração com o [Google Drive API](https://developers.google.com/drive), para armazenamento de arquivos.
* Integração com a API [Alpha Vantage](https://www.alphavantage.co/), para obtenção de indicadores financeiros.
* Integração com o [Discord](https://discordapp.com/), como repositório do log.
* Geração de PDF utilizando o componente [Rotativa](https://github.com/webgio/Rotativa.AspNetCore).
