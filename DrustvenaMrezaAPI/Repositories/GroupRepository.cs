using DrustvenaMrezaAPI.Models;
using System.Globalization;

namespace DrustvenaMrezaAPI.Repositories
{
    public class GroupRepository
    {
        private const string filePath = "data/grupe.csv";
        public static Dictionary<int, Group> Data;

        public GroupRepository()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Group>();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                int id = int.Parse(parts[0]);
                string name = parts[1];
                DateTime foundedDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Group group = new Group(id, name, foundedDate);
                Data[id] = group;
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Group g in Data.Values)
            {
                string date = g.FoundedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                lines.Add($"{g.Id},{g.Name},{date}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
