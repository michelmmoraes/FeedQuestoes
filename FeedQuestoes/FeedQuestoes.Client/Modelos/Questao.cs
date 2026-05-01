using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FeedQuestoes.Client.Modelos;

[Table("questao")]
public class Questao : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("criador_id")]
    public Guid CriadorId { get; set; }

    [Column("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [Column("enunciado")]
    public string Enunciado { get; set; } = string.Empty;

    [Column("alternativas")]
    public List<Alternativa> Alternativas { get; set; } = new();

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // --- PROPRIEDADES FANTASMAS (MEMÓRIA UI) ---
    // Não possuem a tag [Column], logo o Supabase as ignora no INSERT/UPDATE,
    // mas o Blazor pode usá-las livremente para desenhar a tela.
    public List<string> Disciplinas { get; set; } = new();
    public List<string> Assuntos { get; set; } = new();
}

public class Alternativa
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("texto")]
    public string Texto { get; set; } = string.Empty;

    [JsonProperty("correta")]
    public bool Correta { get; set; }
}