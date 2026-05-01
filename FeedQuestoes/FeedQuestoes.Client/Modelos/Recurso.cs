using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace FeedQuestoes.Client.Modelos;

[Table("recurso")]
public class Recurso : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("criador_id")]
    public Guid CriadorId { get; set; }

    [Column("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [Column("conteudo_markdown")]
    public string ConteudoMarkdown { get; set; } = string.Empty;

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; }
}