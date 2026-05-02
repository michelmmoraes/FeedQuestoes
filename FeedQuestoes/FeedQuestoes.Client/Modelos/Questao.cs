using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;

namespace FeedQuestoes.Client.Modelos;

[Table("questao")]
public class Questao : BaseModel
{
    // MANTIDO COMO TRUE: Nossa aplicação é soberana na geração de IDs
    [PrimaryKey("id", true)]
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
    // Usando o nome explícito de ambas as bibliotecas para evitar a "guerra dos sindicatos"
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public List<string> Disciplinas { get; set; } = new();

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public List<string> Assuntos { get; set; } = new();
}

public class Alternativa
{
    // Acordo diplomático: O passaporte das duas alfândegas
    [System.Text.Json.Serialization.JsonPropertyName("Id")]
    [Newtonsoft.Json.JsonProperty("Id")]
    public string Id { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("Texto")]
    [Newtonsoft.Json.JsonProperty("Texto")]
    public string Texto { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("IsCorreta")]
    [Newtonsoft.Json.JsonProperty("IsCorreta")]
    public bool IsCorreta { get; set; }
}