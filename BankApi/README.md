# 💰 Bank API - Simulação de Sistema Bancário

## 📌 Sobre o projeto

Este projeto é uma API simples desenvolvida em C# que simula operações básicas de um sistema bancário.

A ideia é demonstrar, de forma prática, como funcionam:

* criação de contas
* depósitos e saques
* controle de saldo
* histórico de transações (extrato)

Mesmo sendo um projeto simples, ele segue conceitos usados em sistemas reais.

---

## 🎯 Objetivo

Mostrar conhecimentos básicos de desenvolvimento backend, incluindo:

* criação de APIs REST
* manipulação de banco de dados
* regras de negócio
* organização de código

---

## ⚙️ O que o sistema faz

### 👤 1. Criação de usuário

Permite cadastrar um usuário que será dono de uma conta.

---

### 🏦 2. Criação de conta

Uma conta bancária é criada vinculada a um usuário existente.

* Cada conta começa com saldo **0**
* A conta é identificada por um ID único

---

### 💵 3. Depósito

Permite adicionar dinheiro à conta.

Regras:

* o valor deve ser maior que zero
* a conta precisa existir

---

### 💸 4. Saque

Permite retirar dinheiro da conta.

Regras:

* o valor deve ser maior que zero
* não é possível sacar mais do que o saldo disponível

---

### 📄 5. Extrato (histórico)

Mostra tudo que aconteceu na conta:

* saldo atual
* nome do usuário
* lista de transações

  * tipo (depósito ou saque)
  * valor (+ ou -)
  * data

---

## 🧠 Como o saldo funciona

O sistema utiliza duas abordagens:

1. **Saldo armazenado**

   * atualizado a cada operação (mais rápido)

2. **Saldo calculado pelo histórico**

   * soma todas as transações (mais seguro)

Isso simula como sistemas reais lidam com **performance vs consistência**.

---

## 🔄 Transações

Cada operação (depósito ou saque) gera um registro no histórico.

Isso permite:

* rastrear todas as movimentações
* validar o saldo
* gerar extratos confiáveis

---

## 🏗️ Tecnologias utilizadas

* C#
* ASP.NET Core
* Entity Framework Core
* SQL Server
* Docker (para rodar o banco)

---

## 📂 Estrutura do projeto

* **Controllers** → endpoints da API
* **Models** → entidades do sistema (Conta, Usuario, Transacao)
* **DTOs** → objetos usados para entrada de dados
* **Enums** → tipos fixos (ex: tipo de transação)
* **Data** → configuração do banco

---

## 🧪 Exemplos de uso

### Criar conta

```json
{
  "usuarioId": 1
}
```

---

### Depositar

```json
{
  "contaId": 1,
  "valor": 100
}
```

---

### Sacar

```json
{
  "contaId": 1,
  "valor": 50
}
```

---

## 🚀 O que este projeto demonstra

* criação de APIs REST
* validação de dados
* uso de banco relacional
* controle de regras de negócio
* separação de responsabilidades (DTOs, Models, Controllers)

---

## 📌 Possíveis melhorias futuras

* autenticação de usuários
* múltiplas contas por usuário
* transferências entre contas
* uso de transações do banco (garantia de consistência)
* arquitetura em camadas (Service Layer)

---

## 🧠 Observação final

Este projeto foi construído com foco em aprendizado e evolução prática, aplicando conceitos fundamentais usados no desenvolvimento backend profissional.
