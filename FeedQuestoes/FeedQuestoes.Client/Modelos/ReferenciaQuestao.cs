using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Newtonsoft.Json;

namespace FeedQuestoes.Client.Modelos;

[Table("referencia_questao")]
public class ReferenciaQuestao : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("questao_id")]
    public Guid QuestaoId { get; set; }

    [Column("recurso_id")]
    public Guid RecursoId { get; set; }

    // Nosso JSONB com os offsets do texto
    [Column("trecho")]
    public TrechoAncora Trecho { get; set; } = new();

    [Column("ordem")]
    public int Ordem { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; }
}

// Classe auxiliar para mapear o JSON da âncora
public class TrechoAncora
{
    [JsonProperty("inicio")]
    public int Inicio { get; set; }

    [JsonProperty("fim")]
    public int Fim { get; set; }

    [JsonProperty("texto_original")]
    public string TextoOriginal { get; set; } = string.Empty;
}