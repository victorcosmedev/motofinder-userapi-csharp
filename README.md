

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
Para rodar o projeto, é necessário inserir as credenciais do banco de dados Oracle da FIAP no arquivo `appsettings.Development.json`.
#### Aplicação das entidades em tabelas no banco de dados
Após inserir as credenciais, deve-se abrir o Packet Manager Console (Tools > NuGet Package Manager > Package Manager Console) e inserir o comando: `update-database` de modo que as entidades sejam refletidas em banco de dados.
#### Rodar o projeto
Feito isso, basta inicializar o projeto via **HTTP** (não HTTPS) e o Swagger da API será aberto automaticamente. Caso isso não ocorra, ele pode ser acessado através da URL `http://localhost:5045/swagger/index.html`.

## Justificativa da arquitetura
- Seguimos as diretrizes de aula durante a construção do nosso projeto no 1º semestre. Já neste segundo, tomamos a liberdade de fazer algumas alterações para encapsular elementos como o **HATEOAS** e o **PageResultModel** para a paginação.
- Colocamos ambos dentro do pacote Models, sendo que o HATEOAS está em Models/Hateoas e PageResultModel está em Models/PageResultModel. 

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
### Endereco
- *Obs.: Esta entidade não tem um método "GetAll" pois na nossa visão isso não fazia sentido para esta entidade.*

- `GET /api/endereco/{id}` → Busca Endereço por ID
- `POST /api/endereco` → Cadastra novo endereço
- `DELETE /api/endereco/{id}` → Remove endereço
- `PUT /api/endereco/{id}` → Atualiza endereço

## Roteiro de testes
Aqui estão disponibilizados os JSONs para teste da API.

---
### Motoqueiro
**POST**
**URL:** `localhost:5045/api/motoqueiro/`
```
{
  "id": 0,
  "nome": "João da Silva",
  "cpf": "12345678901",
  "enderecoId": null,
  "endereco": null,
  "dataNascimento": "1990-05-15T00:00:00",
  "motoId": null
}
```
**POST - CRIADO APENAS PARA DELEÇÃO**
```
{
  "id": 0,
  "nome": "Maria Oliveira",
  "cpf": "10987654321",
  "enderecoId": null,
  "endereco": null,
  "dataNascimento": "1985-10-20T00:00:00",
  "motoId": null
}
```
---
**GET por ID**  
**URL:** `localhost:5045/api/motoqueiro/{id}`

---
**GET por CPF**  
**URL:** `localhost:5045/api/motoqueiro/cpf/12345678901`

---

**Buscar todos**
**URL:** `localhost:5045/api/motoqueiro/`

---
**PUT**  
**URL:** `localhost:5045/api/motoqueiro/{id}`
```
{
  "id": 1,
  "nome": "João da Silva Atualizado",
  "cpf": "12345678901",
  "enderecoId": null,
  "endereco": null,
  "dataNascimento": "1990-05-15T00:00:00",
  "motoId": null
}
```
---
**DELETE**
**URL:** `localhost:5045/api/motoqueiro/{id-da-delecao}`


### Moto
**POST**
**URL:** `localhost:5045/api/motoqueiro/`
```
{
  "id": 0,
  "modelo": "Honda CG 160",
  "anoDeFabricacao": 2020,
  "chassi": "9BWZZZ377VT004251",
  "placa": "ABC1D23",
  "motoqueiroId": "0" // insira aqui o ID retornado na criação do motoqueiro
}
```
**POST - CRIADO APENAS PARA DELEÇÃO**
```
{
  "id": 0,
  "modelo": "Yamaha Fazer 250",
  "anoDeFabricacao": 2019,
  "chassi": "1HGCM82633A123456",
  "placa": "XYZ9E87",
  "motoqueiroId": "0" // insira aqui o ID retornado na criação do motoqueiro
}
```
---
### GET por ID
**URL**: `localhost:5045/api/moto/{id}`

---
### GET por Placa
**URL**: `localhost:5045/api/moto/placa/ABC1D23`

---
### GET por Chassi
**URL**: `localhost:5045/api/moto/chassi/9BWZZZ377VT004251`

---
### PUT
**URL**: `localhost:5045/api/moto/{id}`
```
{
  "id": 1,
  "modelo": "Honda CG 160 Fan",
  "anoDeFabricacao": 2021,
  "chassi": "9BWZZZ377VT004251",
  "placa": "ABC1D23",
  "motoqueiroId": "{id}" // insira aqui o ID retornado na criação do motoqueiro
}
```
---
### Delete
**URL**: `localhost:5045/api/moto/{id-da-delecao}`

---
### Endereco
### POST
**URL:** `localhost:5045/api/endereco/`

```
{
  "id": 0,
  "logradouro": "Avenida Paulista, Bela Vista",
  "complemento": "Apartamento 101",
  "uf": "SP",
  "numero": "1000",
  "municipio": "São Paulo",
  "motoqueiroId": 0, // insira aqui o ID do motoqueiro cadastrado na primeira fase
  "cep": "01311000"
}
```
**GET por ID**
**URL:** `localhost:5045/api/endereco/{id}`

---
**PUT**
**URL:** `localhost:5045/api/endereco/{id}`
```
{
  "id": 3,
  "logradouro": "Avenida Paulista, Bela Vista",
  "complemento": "Apartamento 102",
  "uf": "SP",
  "numero": "1000",
  "municipio": "São Paulo",
  "motoqueiroId": 0, // insira aqui o ID do motoqueiro cadastrado na primeira fase
  "cep": "01311000"
}
```
**DELETE**
**URL:** `localhost:5045/api/endereco/{id}`
