using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace FeedQuestoes.Client.Modelos
{
    [Table("criador")]
    public class Criador : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("auth_user_id")]
        public string? AuthUserId { get; set; }

        [Column("criado_em", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public DateTime CriadoEm { get; set; }
    }
}