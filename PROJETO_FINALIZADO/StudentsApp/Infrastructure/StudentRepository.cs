using Microsoft.Data.Sqlite;
using StudentsApp.Domain;

namespace StudentsApp.Infrastructure
{
    public class StudentRepository
    {
        public int Create(Student s)
        {
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Students(Name, RM, CPF, Email)
                VALUES ($n,$rm,$cpf,$e);
                SELECT last_insert_rowid();
            ";
            cmd.Parameters.AddWithValue("$n", s.Name);
            cmd.Parameters.AddWithValue("$rm", s.RM);
            cmd.Parameters.AddWithValue("$cpf", s.CPF);
            cmd.Parameters.AddWithValue("$e", s.Email);
            var id = (long)cmd.ExecuteScalar()!;
            return (int)id;
        }

        public List<Student> ReadAll()
        {
            var list = new List<Student>();
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name, RM, CPF, Email FROM Students ORDER BY Id DESC";
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new Student
                {
                    Id = rd.GetInt32(0),
                    Name = rd.GetString(1),
                    RM = rd.GetString(2),
                    CPF = rd.GetString(3),
                    Email = rd.GetString(4)
                });
            }
            return list;
        }

        public void Update(Student s)
        {
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Students
                SET Name=$n, RM=$rm, CPF=$cpf, Email=$e
                WHERE Id=$id;
            ";
            cmd.Parameters.AddWithValue("$n", s.Name);
            cmd.Parameters.AddWithValue("$rm", s.RM);
            cmd.Parameters.AddWithValue("$cpf", s.CPF);
            cmd.Parameters.AddWithValue("$e", s.Email);
            cmd.Parameters.AddWithValue("$id", s.Id);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Students WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
