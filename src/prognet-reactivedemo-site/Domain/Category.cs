namespace reactivedemosite.Domain
{
    public class Category
    {
        public Category()
        {
        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Category(string name)
        {
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}