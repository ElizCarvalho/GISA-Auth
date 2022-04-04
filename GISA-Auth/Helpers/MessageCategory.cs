namespace GISA_Auth.Auth
{
    public class MessageCategory
    {
        private MessageCategory(string value) { Value = value; }
        public string Value { get; private set; }

        public static MessageCategory UserCreated { get { return new MessageCategory("User created successfully!"); } }
        public static MessageCategory UserCreationFailed { get { return new MessageCategory("User creation failed! Please check user details and try again."); } }
        public static MessageCategory UserAlreadyExists { get { return new MessageCategory("User already exists!"); } }

    }
}
