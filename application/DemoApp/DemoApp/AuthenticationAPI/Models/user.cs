using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Models;

[Table("user")]
[MySqlCollation("utf8mb4_0900_ai_ci")]
public partial class user
{
    [Key]
    public int iduser { get; set; }

    [StringLength(45)]
    public string username { get; set; } = null!;

    [StringLength(45)]
    public string password { get; set; } = null!;
}
