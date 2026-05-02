using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;

namespace FeedQuestoes.Client.Modelos;

[Table("referencia_questao")]
public class ReferenciaQuestao : BaseModel
{
    [PrimaryKey("id", true)]
    public Guid Id { get; set; }

    [Column("questao_id")]
    public Guid QuestaoId { get; set; }

    [Column("recurso_id")]
    public Guid RecursoId { get; set; }

    // O C# transforma esse dicionário perfeitamente no jsonb do PostgreSQL
    [Column("trecho")]
    public Dictionary<string, string> Trecho { get; set; } = new();

    [Column("ordem")]
    public int Ordem { get; set; } = 0;
}