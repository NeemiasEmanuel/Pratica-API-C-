using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using loja.models;
using loja.data;
using loja.services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<FornecedorService>();
builder.Services.AddScoped<EstoqueService>();
builder.Services.AddScoped<DepositoService>();
builder.Services.AddScoped<VendaService>();
builder.Services.AddScoped<ServiceService>();
builder.Services.AddScoped<ContratoService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("abcabcabcabcabcabcabcabcabcabcabc"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar as requisições HTTP

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var json = JsonDocument.Parse(body);
    var username = json.RootElement.GetProperty("username").GetString();
    var email = json.RootElement.GetProperty("email").GetString();
    var senha = json.RootElement.GetProperty("senha").GetString();

    var token = string.Empty;
    if (senha == "1029")
    {
        token = GenerateToken(email);
    }
    await context.Response.WriteAsync(token);
});

string GenerateToken(string email)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes("abcabcabcabcabcabcabcabcabcabcabc");
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}

app.MapGet("/rotaSegura", async (HttpContext context) =>
{
    if (!context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Token não fornecido");
        return;
    }

    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes("abcabcabcabcabcabcabcabcabcabcabc");

    try
    {
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        }, out SecurityToken validatedToken);
    }
    catch (Exception)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Token inválido");
        return;
    }

    await context.Response.WriteAsync("Autorizado");
});

// produtos

app.MapGet("/produtos", async ([FromServices] ProductService productService) =>
{
    var produtos = await productService.GetAllProductsAsync();
    return Results.Ok(produtos);
}).RequireAuthorization();

app.MapGet("/produtos/{id}", async (int id, [FromServices] ProductService productService) =>
{
    var produto = await productService.GetProductByIdAsync(id);
    if (produto == null) return Results.NotFound();
    return Results.Ok(produto);
}).RequireAuthorization();

app.MapPost("/produtos", async ([FromBody] Produto produto, [FromServices] ProductService productService) =>
{
    await productService.AddProductAsync(produto);
    return Results.Created($"/produtos/{produto.Id}", produto);
}).RequireAuthorization();

app.MapPut("/produtos/{id}", async (int id, [FromBody] Produto produto, [FromServices] ProductService productService) =>
{
    var result = await productService.UpdateProductAsync(id, produto);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/produtos/{id}", async (int id, [FromServices] ProductService productService) =>
{
    var result = await productService.DeleteProductAsync(id);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

// clientes

app.MapGet("/clientes", async ([FromServices] ClientService clientService) =>
{
    var clientes = await clientService.GetAllClientsAsync();
    return Results.Ok(clientes);
}).RequireAuthorization();

app.MapGet("/clientes/{id}", async (int id, [FromServices] ClientService clientService) =>
{
    var cliente = await clientService.GetClientByIdAsync(id);
    if (cliente == null) return Results.NotFound();
    return Results.Ok(cliente);
}).RequireAuthorization();

app.MapPost("/clientes", async ([FromBody] Cliente cliente, [FromServices] ClientService clientService) =>
{
    await clientService.AddClientAsync(cliente);
    return Results.Created($"/clientes/{cliente.Id}", cliente);
}).RequireAuthorization();

app.MapPut("/clientes/{id}", async (int id, [FromBody] Cliente cliente, [FromServices] ClientService clientService) =>
{
    var result = await clientService.UpdateClientAsync(id, cliente);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/clientes/{id}", async (int id, [FromServices] ClientService clientService) =>
{
    var result = await clientService.DeleteClientAsync(id);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

// fornecedores

app.MapGet("/fornecedores", async ([FromServices] FornecedorService fornecedorService) =>
{
    var fornecedores = await fornecedorService.GetAllFornecedoresAsync();
    return Results.Ok(fornecedores);
}).RequireAuthorization();

app.MapGet("/fornecedores/{id}", async (int id, [FromServices] FornecedorService fornecedorService) =>
{
    var fornecedor = await fornecedorService.GetFornecedorByIdAsync(id);
    if (fornecedor == null) return Results.NotFound();
    return Results.Ok(fornecedor);
}).RequireAuthorization();

app.MapPost("/fornecedores", async ([FromBody] Fornecedor fornecedor, [FromServices] FornecedorService fornecedorService) =>
{
    await fornecedorService.AddFornecedorAsync(fornecedor);
    return Results.Created($"/fornecedores/{fornecedor.Id}", fornecedor);
}).RequireAuthorization();

app.MapPut("/fornecedores/{id}", async (int id, [FromBody] Fornecedor fornecedor, [FromServices] FornecedorService fornecedorService) =>
{
    var result = await fornecedorService.UpdateFornecedorAsync(id, fornecedor);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/fornecedores/{id}", async (int id, [FromServices] FornecedorService fornecedorService) =>
{
    var result = await fornecedorService.DeleteFornecedorAsync(id);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

// Estoques

app.MapGet("/estoques", async ([FromServices] EstoqueService estoqueService) =>
{
    var estoques = await estoqueService.GetAllEstoquesAsync();
    return Results.Ok(estoques);
}).RequireAuthorization();

app.MapGet("/estoques/{id}", async (int id, [FromServices] EstoqueService estoqueService) =>
{
    var estoque = await estoqueService.GetEstoqueByIdAsync(id);
    if (estoque == null) return Results.NotFound();
    return Results.Ok(estoque);
}).RequireAuthorization();

app.MapPost("/estoques", async ([FromBody] Estoque estoque, [FromServices] EstoqueService estoqueService) =>
{
    await estoqueService.AddEstoqueAsync(estoque);
    return Results.Created($"/estoques/{estoque.Id}", estoque);
}).RequireAuthorization();

app.MapPut("/estoques/{id}", async (int id, [FromBody] Estoque estoque, [FromServices] EstoqueService estoqueService) =>
{
    var result = await estoqueService.UpdateEstoqueAsync(id, estoque);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/estoques/{id}", async (int id, [FromServices] EstoqueService estoqueService) =>
{
    var result = await estoqueService.DeleteEstoqueAsync(id);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

// Vendas

app.MapGet("/vendas", async ([FromServices] VendaService vendaService) =>
{
    var vendas = await vendaService.GetAllVendasAsync();
    return Results.Ok(vendas);
}).RequireAuthorization();

app.MapGet("/vendas/{id}", async (int id, [FromServices] VendaService vendaService) =>
{
    var venda = await vendaService.GetVendaByIdAsync(id);
    if (venda == null) return Results.NotFound();
    return Results.Ok(venda);
}).RequireAuthorization();

app.MapPost("/vendas", async ([FromBody] Venda venda, [FromServices] VendaService vendaService) =>
{
    await vendaService.AddVendaAsync(venda);
    return Results.Created($"/vendas/{venda.Id}", venda);
}).RequireAuthorization();

app.MapPut("/vendas/{id}", async (int id, [FromBody] Venda venda, [FromServices] VendaService vendaService) =>
{
    var result = await vendaService.UpdateVendaAsync(id, venda);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/vendas/{id}", async (int id, [FromServices] VendaService vendaService) =>
{
    var result = await vendaService.DeleteVendaAsync(id);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapGet("/depositos", async ([FromServices] DepositoService depositoService) =>
{
    var depositos = await depositoService.GetAllDepositosAsync();
    return Results.Ok(depositos);
}).RequireAuthorization();

app.MapGet("/depositos/{id}", async (int id, [FromServices] DepositoService depositoService) =>
{
    var deposito = await depositoService.GetDepositoByIdAsync(id);
    if (deposito == null) return Results.NotFound();
    return Results.Ok(deposito);
}).RequireAuthorization();

app.MapPost("/depositos", async ([FromBody] Deposito deposito, [FromServices] DepositoService depositoService) =>
{
    await depositoService.AddDepositoAsync(deposito);
    return Results.Created($"/depositos/{deposito.Id}", deposito);
}).RequireAuthorization();

app.MapPut("/depositos/{id}", async (int id, [FromBody] Deposito deposito, [FromServices] DepositoService depositoService) =>
{
    var result = await depositoService.UpdateDepositoAsync(id, deposito);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/depositos/{id}", async (int id, [FromServices] DepositoService depositoService) =>
{
    var result = await depositoService.DeleteDepositoAsync(id);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();


// Endpoints para vendas detalhadas e sumarizadas por produto
app.MapGet("/vendas/produto/detalhada/{produtoId}", async (int produtoId, [FromServices] VendaService vendaService) =>
{
    var vendas = await vendaService.GetVendasPorProdutoDetalhadaAsync(produtoId);
    return Results.Ok(vendas);
}).RequireAuthorization();

app.MapGet("/vendas/produto/sumarizada/{produtoId}", async (int produtoId, [FromServices] VendaService vendaService) =>
{
    var vendas = await vendaService.GetVendasPorProdutoSumarizadaAsync(produtoId);
    return Results.Ok(vendas);
}).RequireAuthorization();

// Endpoints para vendas detalhadas e sumarizadas por cliente
app.MapGet("/vendas/cliente/detalhada/{clienteId}", async (int clienteId, [FromServices] VendaService vendaService) =>
{
    var vendas = await vendaService.GetVendasPorClienteDetalhadaAsync(clienteId);
    return Results.Ok(vendas);
}).RequireAuthorization();

app.MapGet("/vendas/cliente/sumarizada/{clienteId}", async (int clienteId, [FromServices] VendaService vendaService) =>
{
    var vendas = await vendaService.GetVendasPorClienteSumarizadaAsync(clienteId);
    return Results.Ok(vendas);
}).RequireAuthorization();

// Endpoints para consultar produtos no depósito/estoque
app.MapGet("/depositos/{depositoId}/produtos", async (int depositoId, [FromServices] DepositoService depositoService) =>
{
    var produtosQuantidades = await depositoService.GetProdutosNoDepositoSumarizadaAsync(depositoId);
    if (produtosQuantidades == null || produtosQuantidades.Count == 0) 
        return Results.NotFound();

    return Results.Ok(produtosQuantidades);
}).RequireAuthorization();

// Endpoint para consultar quantidade de um produto no depósito/estoque
app.MapGet("/depositos/produto/{produtoId}", async (int produtoId, [FromServices] DepositoService depositoService) =>
{
    var produtoQuantidade = await depositoService.GetQuantidadeProdutoNoDepositoAsync(produtoId);
    if (produtoQuantidade == null) return Results.NotFound();

    return Results.Ok(produtoQuantidade);
}).RequireAuthorization();


app.MapGet("/servico", async ([FromServices] ServiceService ServiceService) =>
{
    var Services = await ServiceService.GetAllServicesAsync();
    return Results.Ok(Services);
}).RequireAuthorization();

app.MapGet("/servico/{id}", async (int id, [FromServices] ServiceService ServiceService) =>
{
    var Service = await ServiceService.GetServiceByIdAsync(id);
    if (Service == null) return Results.NotFound();
    return Results.Ok(Service);
}).RequireAuthorization();

app.MapPost("/servico", async ([FromBody] Service Service, [FromServices] ServiceService ServiceService) =>
{
    await ServiceService.AddServiceAsync(Service);
    return Results.Created($"/servico/{Service.id}", Service);
}).RequireAuthorization();

app.MapPut("/servico/{id}", async (int id, [FromBody] Service Service, [FromServices] ServiceService ServiceService) =>
{
    var result = await ServiceService.UpdateServiceAsync(id, Service);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/servico/{id}", async (int id, [FromServices] ServiceService ServiceService) =>
{
    var result = await ServiceService.DeleteServiceAsync(id);
    if (!result) return Results.NotFound();
    return Results.NoContent();
}).RequireAuthorization();

app.MapPost("/contratos", async ([FromBody] Contrato contrato, [FromServices] ContratoService contratoService) =>
{
    await contratoService.AddContratoAsync(contrato);
    return Results.Created($"/contratos/{contrato.Id}", contrato);
}).RequireAuthorization();

app.MapGet("/clientes/{clienteId}/servicos", async (int clienteId, [FromServices] ContratoService contratoService) =>
{
    var servicos = await contratoService.GetServicosByClienteIdAsync(clienteId);
    if (!servicos.Any()) return Results.NotFound("No services found for the given client.");
    return Results.Ok(servicos);
}).RequireAuthorization();



app.Run();
