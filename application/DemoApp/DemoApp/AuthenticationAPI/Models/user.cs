using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
