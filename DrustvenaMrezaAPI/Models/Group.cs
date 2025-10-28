namespace DrustvenaMrezaAPI.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FoundedDate { get; set; }
        public List<int> MemberIds { get; set; }

        public Group(int id, string name, DateTime foundedDate)
        {
            Id = id;
            Name = name;
            FoundedDate = foundedDate;
            MemberIds = new List<int>();
        }
    }
}
