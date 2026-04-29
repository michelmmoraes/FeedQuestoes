using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;

namespace FeedQuestoes.Client.Modelos
{
    [Table("questao")]
    public class Questao : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("criador_id")]
        public string CriadorId { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("enunciado")]
        public string Enunciado { get; set; }

        [Column("alternativas")]
        public List<Alternativa> Alternativas { get; set; } = new List<Alternativa>();

        [Column("disciplinas")]
        public List<string> Disciplinas { get; set; } = new();

        [Column("assuntos")]
        public List<string> Assuntos { get; set; } = new();

        [Column("criado_em", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public DateTime CriadoEm { get; set; }
    }

    // Estrutura que mapeia o nosso JSONB de alternativas
    public class Alternativa
    {
        public string Id { get; set; }
        public string Texto { get; set; }
        public bool IsCorreta { get; set; }
    }
}