# Tryitter

O projeto consiste numa aplicação em C# que funciona como o back end de uma rede social, com o banco de dados em SQL Server. Nessa aplicação, é possível manipular (criar, ler, atualizar e apagar) as informações da tabela de users e posts. Além disso, para realizar as operações, o usuário necessita estar logado, sendo possível alterar apenas os seus posts (Autenticação e Autorização).

## Inicialização

Para rodar a aplicação, utilize os seguintes comandos:

- Na pasta raiz: dotnet restore

- Na pasta raiz: docker compose up

- Na pasta Tryitter: dotnet watch run

## Testes

Testes foram desenvolvidos utilizando XUnit e FluentAssertions. Para rodar, utilize os seguintes comandos:

- Na pasta raiz: dotnet test

## Componentes

O projeto foi desenvolvido em grupo com a participação dos seguintes integrantes:

- [Fabiana Martins](https://www.linkedin.com/in/martinsfaah/)

- [Paulo Bomfim](https://www.linkedin.com/in/paulopbomfim/)

- [Rafael de Jesus](https://www.linkedin.com/in/rafael-de-jesus-lima/)
