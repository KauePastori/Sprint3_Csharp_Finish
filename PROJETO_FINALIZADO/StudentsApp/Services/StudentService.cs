using StudentsApp.Domain;
using StudentsApp.Infrastructure;

namespace StudentsApp.Services
{
    public class StudentService
    {
        private readonly StudentRepository _repo = new();

        public List<Student> GetAll() => _repo.ReadAll();

        public Student Add(Student s)
        {
            s.Validate();
            s.Id = _repo.Create(s);
            return s;
        }

        public void Update(Student s)
        {
            s.Validate();
            if (s.Id <= 0) throw new ArgumentException("Id inválido para atualização.");
            _repo.Update(s);
        }

        public void Remove(int id) => _repo.Delete(id);
    }
}
