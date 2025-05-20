
# MotoFindr - API de Usuários e Motos

### 💡Sobre o projeto

A ideia da nossa solução é elaborar um sistema de localização indoor para as motos dentro dos pátios da Mottu, já que a principal queixa dos stakeholders é justamente a ineficiência e imprecisão do sistema de GPS quando se trata de espaços tão curtos.

Desta forma, o **MotoFindr** surge como um software capaz de organizar, sistematizar e cadastrar as motos que chegam aos pátios de modo a sanar as dores da Mottu.

  

## Nome Integrantes

<div align="center">

| Nome | RM |  
| ------------- |:-------------:|  
| Arthur Eduardo Luna Pulini|554848|  
|Lucas Almeida Fernandes de Moraes| 557569 |  
|Victor Nascimento Cosme|558856|

</div>

  

## 🎯 Objetivo

Gerenciar o status das motos nos pátios, com foco no:

🔹 **Atrelamento**: Vinculação automática moto-motoqueiro na entrada

🔹 **Desatrelamento**: Liberação do vínculo na saída

🔹 **Auditoria**: Registro completo das movimentações

#### Funcionalidades-chave:

✅ **CRUD completo** de motos e motoqueiros

✅ **Atrelamento/Desatrelamento** automático das entidades:

- Quando uma moto **entra no pátio**, é vinculada ao motoqueiro responsável

- Quando **sai**, o vínculo é removido, liberando o veículo para nova alocação

✅ **Registro preciso** de movimentações (chassi, placa, horários e responsáveis)

  

#### Por quê a API não representa a aplicação completa?

- A API representa somente parte da aplicação pois será complementada com a API produzida na matéria **Java Advanced**.

## Instalação do projeto - Orientações

#### Credenciais
Para rodar o projeto, é necessário inserir as credenciais do banco de dados Oracle da FIAP no arquivo `appsettings.json`.
#### Aplicação das entidades em tabelas no banco de dados
Após inserir as credenciais, deve-se abrir o Packet Manager Console (Tools > NuGet Package Manager > Package Manager Console) e inserir o comando: `update-database` de modo que as entidades sejam refletidas em banco de dados.
#### Rodar o projeto
Feito isso, basta inicializar o projeto via **HTTP** (não HTTPS) e o Swagger da API será aberto automaticamente. Caso isso não ocorra, ele pode ser acessado através da URL `http://localhost:5045/swagger/index.html`.

## Endpoints da API

### **Motoqueiros**

  

-  `GET /api/motoqueiro/{id}` → Busca motoqueiro por ID

-  `GET /api/motoqueiro/cpf/{cpf}` → Busca motoqueiro por CPF

-  `POST /api/motoqueiro` → Cadastra novo motoqueiro

-  `PUT /api/motoqueiro/{id}` → Atualiza motoqueiro existente

-  `DELETE /api/motoqueiro/{id}` → Remove motoqueiro

-  `GET /api/motoqueiro` → Lista todos os motoqueiros

  

### **Motos**

  

-  `GET /api/moto/{id}` → Busca moto por ID

-  `GET /api/moto/placa/{placa}` → Busca moto por placa

-  `GET /api/moto/chassi/{chassi}` → Busca moto por chassi

-  `POST /api/moto` → Cadastra nova moto

-  `PUT /api/moto/{id}` → Atualiza moto existente

-  `DELETE /api/moto/{id}` → Remove moto

-  `GET /api/moto` → Lista todas as motos
