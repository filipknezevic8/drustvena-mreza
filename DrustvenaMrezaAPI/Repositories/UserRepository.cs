using DrustvenaMrezaAPI.Models;
using System.Globalization;

namespace DrustvenaMrezaAPI.Repositories
{
    public class UserRepository
    {
        private const string usersFile = "data/korisnici.csv";
        private const string membershipFile = "data/clanstva.csv";

        public static Dictionary<int, User> Data;

        public UserRepository()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, User>();
            string[] userLines = File.ReadAllLines(usersFile);
            foreach (string line in userLines)
            {
                string[] parts = line.Split(',');
                int id = int.Parse(parts[0]);
                string username = parts[1];
                string firstName = parts[2];
                string lastName = parts[3];
                DateTime birthDate = DateTime.ParseExact(parts[4], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                User user = new User(id, username, firstName, lastName, birthDate);
                Data[id] = user;
            }

            string[] membershipLines = File.ReadAllLines(membershipFile);
            foreach (string line in membershipLines)
            {
                string[] parts = line.Split(',');
                int userId = int.Parse(parts[0]);
                int groupId = int.Parse(parts[1]);

                if (Data.ContainsKey(userId) && GroupRepository.Data.ContainsKey(groupId))
                {
                    Data[userId].Groups.Add(GroupRepository.Data[groupId]);
                }
            }
        }

        public void Save()
        {
            List<string> userLines = new List<string>();
            foreach (User u in Data.Values)
            {
                string date = u.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                userLines.Add($"{u.Id},{u.Username},{u.FirstName},{u.LastName},{date}");
            }
            File.WriteAllLines(usersFile, userLines);

            List<string> membershipLines = new List<string>();
            foreach (User u in Data.Values)
            {
                foreach (Group g in u.Groups)
                {
                    membershipLines.Add($"{u.Id},{g.Id}");
                }
            }
            File.WriteAllLines(membershipFile, membershipLines);
        }
    }
}
