using SqlSugar;

namespace Hello0
{
    public class users
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
