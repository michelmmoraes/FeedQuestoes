using FeedQuestoes.Components;
using Supabase;
using FeedQuestoes.Client.Modelos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => {
    var navManager = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navManager.BaseUri) };
});

// --- INÍCIO DA CONFIGURAÇÃO DO SUPABASE (A RETAGUARDA) ---
var url = builder.Configuration["Supabase:Url"];
var key = builder.Configuration["Supabase:Key"];
var options = new SupabaseOptions { AutoConnectRealtime = true };
builder.Services.AddSingleton(new Supabase.Client(url, key, options));
// --- FIM DA CONFIGURAÇÃO ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FeedQuestoes.Client._Imports).Assembly);

// --- INÍCIO DA PONTE SEGURA (MINIMAL APIS) ---

// 1. Doca de Missões (Questões) - GET
app.MapGet("/api/questoes", async (Supabase.Client db) => {
    var response = await db.From<Questao>().Get();
    // Empacota usando o transportador robusto
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

// 2. Doca de Missões (Questões) - POST (Blindado com Newtonsoft e Alarme)
app.MapPost("/api/questoes", async (HttpContext ctx, Supabase.Client db) => {
    try
    {
        using var reader = new StreamReader(ctx.Request.Body);
        var novaQuestao = Newtonsoft.Json.JsonConvert.DeserializeObject<Questao>(await reader.ReadToEndAsync());
        await db.From<Questao>().Insert(novaQuestao!);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Erro na fábrica de questões: {ex.Message}");
    }
});

// 3. Doca de Lastro (Referências) - POST (Blindado com Newtonsoft e Alarme)
app.MapPost("/api/referencias", async (HttpContext ctx, Supabase.Client db) => {
    try
    {
        using var reader = new StreamReader(ctx.Request.Body);
        var refQ = Newtonsoft.Json.JsonConvert.DeserializeObject<ReferenciaQuestao>(await reader.ReadToEndAsync());
        await db.From<ReferenciaQuestao>().Insert(refQ!);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Erro no vínculo do lastro: {ex.Message}");
    }
});

// 4. Doca de Respostas - POST
app.MapPost("/api/respostas", async (HttpContext ctx, Supabase.Client db) => {
    try
    {
        using var reader = new StreamReader(ctx.Request.Body);
        var novaResposta = Newtonsoft.Json.JsonConvert.DeserializeObject<Resposta>(await reader.ReadToEndAsync());
        await db.From<Resposta>().Insert(novaResposta!);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// 5. Doca de Respostas - GET
app.MapGet("/api/respostas/{idTrabalhador}", async (Supabase.Client db, Guid idTrabalhador) => {
    var response = await db.From<Resposta>().Where(r => r.RespondidoPor == idTrabalhador).Get();
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

// 6. Doca de Territórios e Assuntos
app.MapGet("/api/territorios/{id:guid}", async (Supabase.Client db, Guid id) => {
    var response = await db.From<Assunto>().Where(t => t.Id == id).Single();
    if (response == null) return Results.NotFound();
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response), "application/json");
});

app.MapGet("/api/territorios", async (Supabase.Client db) => {
    var response = await db.From<Assunto>().Get();
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

// 7. Hierarquias
app.MapGet("/api/territorios/hierarquia", async (Supabase.Client db) => {
    var response = await db.From<AssuntoHierarquia>().Get();
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

// 8. Recursos dos Territórios
app.MapGet("/api/territorios/{id:guid}/recursos", async (Supabase.Client db, Guid id) => {
    var relacoes = await db.From<RecursoAssunto>().Where(r => r.AssuntoId == id).Get();
    var recursoIds = relacoes.Models.Select(r => r.RecursoId).ToList();

    if (!recursoIds.Any()) return Results.Ok(new List<Recurso>());

    var recursos = await db.From<Recurso>()
                           .Filter("id", Supabase.Postgrest.Constants.Operator.In, recursoIds)
                           .Get();

    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(recursos.Models), "application/json");
});

// --- FIM DA PONTE SEGURA ---

// --- ROTA FORÇADA PARA O MOTOR PWA ---
app.MapGet("/service-worker.js", (IWebHostEnvironment env) =>
{
    var filePath = Path.Combine(env.WebRootPath, "service-worker.js");
    if (File.Exists(filePath))
    {
        return Results.File(filePath, "application/javascript");
    }
    return Results.NotFound("Arquivo do motor não encontrado.");
});

app.Run();