namespace Ong.Domain
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public User(Guid id, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio ou nulo.", nameof(email));
            if (!IsValidEmail(email))
                throw new ArgumentException("Formato de email é inválido", nameof(email));

            Id = id;
            Name = name;
            Email = email;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email não pode ser vazio ou nulo.", nameof(newEmail));
            if (!IsValidEmail(newEmail))
                throw new ArgumentException("Formato de email é inválido", nameof(newEmail));
            Email = newEmail;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
