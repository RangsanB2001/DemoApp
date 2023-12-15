
namespace AuthenticationModels
{
    public class GetDataUser
    {
        public int iduser { get; set; }
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}
