using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// AQUI ESTÁ A ANTENA DO RÁDIO:
// Isso ensina o celular do estudante a fazer chamadas HTTP (buscar dados) 
// apontando para o próprio endereço onde o sistema está hospedado.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();