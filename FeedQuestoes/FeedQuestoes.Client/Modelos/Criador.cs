using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace FeedQuestoes.Client.Modelos;

[Table("criador")]
public class Criador : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [Column("auth_user_id")]
    public Guid? AuthUserId { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}