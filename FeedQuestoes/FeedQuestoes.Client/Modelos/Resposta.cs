using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace FeedQuestoes.Client.Modelos;

[Table("resposta")]
public class Resposta : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("respondido_por")]
    public Guid RespondidoPor { get; set; }

    [Column("questao_id")]
    public Guid QuestaoId { get; set; }

    [Column("correta")]
    public bool Correta { get; set; }

    [Column("respondido_em")]
    public DateTime RespondidoEm { get; set; } = DateTime.UtcNow;
}