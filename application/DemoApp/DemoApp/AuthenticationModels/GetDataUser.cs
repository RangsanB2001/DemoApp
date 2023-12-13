
namespace AuthenticationModels
{
    public class GetDataUser
    {
        public int userId { get; set; }
        public string userName { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}
