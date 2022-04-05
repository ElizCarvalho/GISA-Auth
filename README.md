<br />
<div align="center">
  <img src="images/logo-gisa.png" alt="Logo" width="351" height="120">
  <h3 align="center">GISA Auth API</h3>
</div>

<!-- ABOUT THE PROJECT -->
## Sobre o projeto
API de autenticação da POC desenvolvida como parte da entrega do TCC para obtenção do título de 'Especialista em Arquitetura de Software Distribuído' pela PUC-MG.

### Tecnologias
* [JWT](https://jwt.io/introduction)
* [Entity Framework](https://docs.microsoft.com/pt-br/ef/)
* [Open API](https://swagger.io/specification/)
* [Core 6](https://docs.microsoft.com/pt-br/dotnet/core/compatibility/6.0)

## Iniciando

### Pré requisito
Faz-se necessário ter o [Docker](https://docs.docker.com/get-docker/) instalado e funcionando corretamente.

### Instalação
1. Clone o repositório
   ```sh
   git clone https://github.com/ElizCarvalho/GISA-Auth.git
   ```
2. Acesse o diretório da aplicação, por exemplo:
   ```sh
   cd .\repos\GISA-Auth\GISA-Auth
   ```
3. Insira a senha do banco de dados em `docker-compose.yml` na propriedade `SA_PASSWORD`
   ```yml
   - SA_PASSWORD = PASSWORD_DATABASE
   ```
4. Repita a senha em `appsettings.json` na propriedade `DefaultConnection` em appsettings.json `Password` 
   ```json
   "DefaultConnection": "Initial Catalog=gisa-auth-db; Data Source=sqldb; Persist Security Info=True;User ID=SA;Password=PASSWORD_DATABASE;"
   ```
5. Ainda em `appsettings.json`, informe sua  `SecretKey` que será utilizada para gerar o token JWT 
   ```json
   "AppSettings": {
    "SecretKey": "MYSECRETSUPERSECRET"
    ...
    }
   ```
6. No terminal, gere a imagem dos containers
   ```sh
   docker-compose build
   ````
7. No terminal, suba os containers
   ```sh
   docker-compose up -d
   ````
8. Acesse a documentação da API através da url http://localhost:9000/swagger
