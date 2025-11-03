

namespace MotoFindrUserAPI.Utils.Doc
{
    public static class ApiDoc
    {
        #region MotoDoc
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
        #endregion
        #region MotoqueiroDoc
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
        #endregion
        #region EnderecoDoc
        public const string BuscarEnderecoPorIdSummary = "Buscar endereço por ID";
        public const string BuscarEnderecoPorIdDescription = "Retorna um endereço específico com base no ID informado.";

        public const string SalvarEnderecoSummary = "Cria um novo endereço";
        public const string SalvarEnderecoDescription = "Cadastra um novo endereço no sistema";

        public const string AtualizarEnderecoSummary = "Atualiza um endereço existente";
        public const string AtualizarEnderecoDescription = "Atualiza os dados do endereço com o ID especificado";

        public const string DeletarEnderecoSummary = "Remove um endereço";
        public const string DeletarEnderecoDescription = "Exclui permanentemente o endereço com o ID especificado";

        public const string BuscarTodosEnderecosSummary = "Obter todos os endereços cadastrados";
        public const string BuscarTodosEnderecosDescription = "Buscar todos os endereços salvos no sistema.";
        #endregion
        #region UserDoc
        public const string RegisterUserSummary = "Registra um novo usuário";
        public const string RegisterUserDescription = "Cria uma nova conta de usuário no sistema com nome de usuário e senha.";

        public const string LoginUserSummary = "Autentica um usuário";
        public const string LoginUserDescription = "Realiza o login do usuário, validando credenciais e retornando um token JWT.";
        #endregion
    }
}
