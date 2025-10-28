namespace DrustvenaMrezaAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Group> Groups { get; set; }

        public User(int id, string username, string firstName, string lastName, DateTime birthDate)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Groups = new List<Group>();
        }
    }
}
