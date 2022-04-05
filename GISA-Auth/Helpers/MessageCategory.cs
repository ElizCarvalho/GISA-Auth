namespace GISA_Auth.Auth
{
    public class MessageCategory
    {
        private MessageCategory(string value) { Value = value; }
        public string Value { get; private set; }

        public static MessageCategory UserCreated { get { return new MessageCategory("Usuário criado com sucesso!"); } }
        public static MessageCategory UserCreationFailed { get { return new MessageCategory("Falha na criação do usuário! Verifique os detalhes do usuário e tente novamente."); } }
        public static MessageCategory UserAlreadyExists { get { return new MessageCategory("Usuário já existe!"); } }

    }
}