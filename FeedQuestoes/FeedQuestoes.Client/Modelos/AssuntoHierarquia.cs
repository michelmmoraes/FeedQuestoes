using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace FeedQuestoes.Client.Modelos;

[Table("assunto_hierarquia")]
public class AssuntoHierarquia : BaseModel
{
    // No Supabase C#, tabelas de junção precisam que as colunas 
    // sejam marcadas como chaves para que o mapeamento funcione.
    [PrimaryKey("pai_id", false)]
    public Guid PaiId { get; set; }

    [PrimaryKey("filho_id", false)]
    public Guid FilhoId { get; set; }
}