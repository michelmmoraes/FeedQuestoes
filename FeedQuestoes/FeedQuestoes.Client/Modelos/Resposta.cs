using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace FeedQuestoes.Client.Modelos
{
    [Table("resposta")]
    public class Resposta : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("respondido_por")]
        public string RespondidoPor { get; set; }

        [Column("questao_id")]
        public string QuestaoId { get; set; }

        [Column("correta")]
        public bool Correta { get; set; }

        [Column("respondido_em", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public DateTime RespondidoEm { get; set; }
    }
}