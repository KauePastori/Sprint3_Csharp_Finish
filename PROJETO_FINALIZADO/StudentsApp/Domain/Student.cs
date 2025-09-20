using System;

namespace StudentsApp.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RM { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(RM))
                throw new ArgumentException("RM é obrigatório.");
            if (string.IsNullOrWhiteSpace(CPF) || !((CPF.Length == 11) || (CPF.Length == 14)))
                throw new ArgumentException("CPF é obrigatório (11 dígitos ou com pontuação, 14 chars).");
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
                throw new ArgumentException("Email inválido.");
        }

        public override string ToString()
            => $"{Id} | {Name} | RM:{RM} | CPF:{CPF} | {Email}";
    }
}
