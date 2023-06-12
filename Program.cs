using System.Text;
using AppRequest.Controllers.Categories;
using AppRequest.Controllers.Employee;
using AppRequest.Controllers.Security;
using AppRequest.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:SqlServer"]);

//Estamos ultilizando o serviço do identity com aquele aplicationDbCotext assim ele
//vai entender como acessar o banco de dados;
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options=>{
    //alterando a segurança da senha
    options.Password.RequireNonAlphanumeric =false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllers();

//serviço e tudo aquilo que eu digo que esta disponivel para o asp net usar
builder.Services.AddAuthorization(options =>{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
        //adicionando a politica de empregados 
        options.AddPolicy("EmployeePolicy", p=>
        p.RequireAuthenticatedUser().RequireClaim("EmployeeCode"));
        //somente aquele usuario pode acessar aquela rota
        options.AddPolicy("Employee005Policy", p=>
        p.RequireAuthenticatedUser().RequireClaim("EmployeeCode","005"));
         
}
);//adiciona a parte de autorização do asp.net
builder.Services.AddAuthentication(x =>{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,//assinatura
        ValidateAudience = true,//audience
        ValidateLifetime = true,//tempo de vida
        ValidateIssuerSigningKey = true,//secretKey
        ClockSkew = TimeSpan.Zero,//zero tempo de bonus após a expiração do token, default 60min
        ValidIssuer = builder.Configuration["Jwr:Issuer"],
        ValidAudience = builder.Configuration["Jwr:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))   //Chave de assinatura do emissor

    };
});
//Enquanto durar a requisição essa classe estara na memoria;
builder.Services.AddScoped<QueryAllUsersWithClaimName>();

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
//aqui estou habilitando para o aplicativo usar
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapMethods(CategoryPost.Template,CategoryPost.Methods,CategoryPost.Handle);
app.MapMethods(CategoryGetAll.Template,CategoryGetAll.Methods,CategoryGetAll.Handle);

app.MapMethods(CategoryPut.Template,CategoryPut.Methods,CategoryPut.Handle);
app.MapMethods(EmployeeGetAll.Template,EmployeePost.Methods,EmployeePost.Handle);
app.MapMethods(EmployeeGetAll.Template,EmployeeGetAll.Methods,EmployeeGetAll.Handle);
app.MapMethods(TokenPost.Template,TokenPost.Methods,TokenPost.Handle);



app.Run();
