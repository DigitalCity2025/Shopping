namespace Shopping.DAL.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsImportant { get; set; }
    }
}
