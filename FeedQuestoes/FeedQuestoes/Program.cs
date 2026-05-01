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

// --- INÍCIO DA PONTE SEGURA (MINIMAL APIS) ---

app.MapGet("/api/questoes", async (Supabase.Client db) => {
    var response = await db.From<Questao>().Get();
    // Empacota usando o transportador robusto
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

app.MapPost("/api/questoes", async (HttpContext ctx, Supabase.Client db) => {
    using var reader = new StreamReader(ctx.Request.Body);
    // Desempacota usando o transportador robusto
    var novaQuestao = Newtonsoft.Json.JsonConvert.DeserializeObject<Questao>(await reader.ReadToEndAsync());
    await db.From<Questao>().Insert(novaQuestao!);
    return Results.Ok();
});

app.MapPost("/api/respostas", async (HttpContext ctx, Supabase.Client db) => {
    using var reader = new StreamReader(ctx.Request.Body);
    var novaResposta = Newtonsoft.Json.JsonConvert.DeserializeObject<Resposta>(await reader.ReadToEndAsync());
    await db.From<Resposta>().Insert(novaResposta!);
    return Results.Ok();
});

app.MapGet("/api/respostas/{idTrabalhador}", async (Supabase.Client db, Guid idTrabalhador) => {
    var response = await db.From<Resposta>().Where(r => r.RespondidoPor == idTrabalhador).Get();
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

// --- FIM DA PONTE SEGURA ---
// --- ROTA FORÇADA PARA O MOTOR PWA ---
app.MapGet("/service-worker.js", (IWebHostEnvironment env) =>
{
    // Procura o arquivo fisicamente na pasta wwwroot do Servidor
    var filePath = Path.Combine(env.WebRootPath, "service-worker.js");

    if (File.Exists(filePath))
    {
        // Força a entrega com o carimbo correto (application/javascript)
        return Results.File(filePath, "application/javascript");
    }

    return Results.NotFound("Arquivo do motor não encontrado.");
});

app.MapGet("/api/territorios/{id:guid}", async (Supabase.Client db, Guid id) => {
    // Busca na base de dados o território específico
    var response = await db.From<Assunto>().Where(t => t.Id == id).Single();

    if (response == null) return Results.NotFound();

    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response), "application/json");
});

app.MapGet("/api/territorios", async (Supabase.Client db) => {
    // Busca todos os registros da tabela 'assunto'
    var response = await db.From<Assunto>().Get();
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

// Rota para buscar as relações de hierarquia
app.MapGet("/api/territorios/hierarquia", async (Supabase.Client db) => {
    var response = await db.From<AssuntoHierarquia>().Get();
    return Results.Text(Newtonsoft.Json.JsonConvert.SerializeObject(response.Models), "application/json");
});

app.Run();