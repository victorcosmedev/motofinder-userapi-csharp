using System.Security.Cryptography.X509Certificates;

namespace MotoFindrUserAPI.Utils
{
    public static class ApiDoc
    {
        public const string GetMotoByIdSummary = "Obter moto por ID";
        public const string GetMotoByIdDescription = "Retorna uma moto específica com base no ID fornecido";

        public const string GetByPlacaSummary = "Obter moto por placa";
        public const string GetByPlacaDescription = "Retorna uma moto específica com base na placa fornecida";

        public const string GetMotoByChassiSummary = "Obter moto por chassi";
        public const string GetMotoByChassiDescription = "Retorna uma moto específica com base no chassi fornecido";

        public const string SalvarMotoSummary = "Criar nova moto";
        public const string SalvarMotoDescription = "Cadastra uma nova moto no sistema";

        public const string AtualizarMotoSummary = "Atualizar moto existente";
        public const string AtualizarMotoDescription = "Atualiza os dados de uma moto existente";

        public const string DeletarMotoSummary = "Remover moto";
        public const string DeletarMotoDescription = "Exclui permanentemente uma moto do sistema";

        public const string BuscarTodasMotosSummary = "Obter todas as motos cadastradas";
        public const string BuscarTodasMotosDescription = "Buscar todas as motos salvas no sistema.";

        public const string BuscarMotoqueiroPorIdSummary = "Obtém um motoqueiro pelo ID";
        public const string BuscarMotoqueiroPorIdDescription = "Retorna os detalhes do motoqueiro com o ID especificado";

        public const string BuscarMotoqueiroPorCpfSummary = "Busca motoqueiro por CPF";
        public const string BuscarMotoqueiroPorCpfDescription = "Realiza a busca de um motoqueiro utilizando o CPF como filtro";

        public const string SalvarMotoqueiroSummary = "Cria um novo motoqueiro";
        public const string SalvarMotoqueiroDescription = "Cadastra um novo motoqueiro no sistema";

        public const string AtualizarMotoqueiroSummary = "Atualiza um motoqueiro existente";
        public const string AtualizarMotoqueiroDescription = "Atualiza os dados do motoqueiro com o ID especificado";

        public const string DeletarMotoqueiroSummary = "Remove um motoqueiro";
        public const string DeletarMotoqueiroDescription = "Exclui permanentemente o motoqueiro com o ID especificado";

        public const string BuscarTodosMotoqueirosSummary = "Obter todos os motoqueiros cadastrados";
        public const string BuscarTodosMotoqueirosDescription = "Buscar todos os motoqueiros salvos no sistema.";
    }
}
