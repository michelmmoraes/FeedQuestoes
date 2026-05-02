using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace FeedQuestoes.Client.Modelos;

[Table("recurso_assunto")]
public class RecursoAssunto : BaseModel
{
    [PrimaryKey("recurso_id", false)]
    public Guid RecursoId { get; set; }

    [PrimaryKey("assunto_id", false)]
    public Guid AssuntoId { get; set; }
}