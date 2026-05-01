using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace FeedQuestoes.Client.Modelos;

[Table("assunto")]
public class Assunto : BaseModel
{
    [PrimaryKey("id", false)] // false: o banco (gen_random_uuid) é quem gera, não o C#
    public Guid Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("descricao")]
    public string? Descricao { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; }
}