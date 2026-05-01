using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace FeedQuestoes.Client.Modelos;

[Table("questao_assunto")]
public class QuestaoAssunto : BaseModel
{
    // Em tabelas de chave composta, mapeamos as duas como PrimaryKey
    [PrimaryKey("questao_id", false)]
    public Guid QuestaoId { get; set; }

    [PrimaryKey("assunto_id", false)]
    public Guid AssuntoId { get; set; }
}