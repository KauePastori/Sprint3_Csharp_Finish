using System.Text;
using System.Text.Json;
using StudentsApp.Domain;

namespace StudentsApp.Services
{
    public static class FileService
    {
        public static void ExportJson(string path, IEnumerable<Student> students)
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(path, JsonSerializer.Serialize(students, opts), Encoding.UTF8);
        }

        public static List<Student> ImportJson(string path)
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Student>>(json) ?? new();
        }

        public static void ExportTxt(string path, IEnumerable<Student> students)
        {
            var sb = new StringBuilder();
            foreach (var s in students)
                sb.AppendLine(s.ToString());
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
