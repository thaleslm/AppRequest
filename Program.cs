using AppRequest.Controllers.Categories;
using AppRequest.Repository.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:SqlServer"]);

//Estamos ultilizando o serviço do identity com aquele aplicationDbCotext assim ele
//vai entender como acessar o banco de dados;
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options=>{
    //alterando a segurança da senha
    options.Password.RequireNonAlphanumeric =false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapMethods(CategoryPost.Template,CategoryPost.Methods,CategoryPost.Handle);
app.MapMethods(CategoryGetAll.Template,CategoryGetAll.Methods,CategoryGetAll.Handle);

app.MapMethods(CategoryPut.Template,CategoryPut.Methods,CategoryPut.Handle);

app.Run();
