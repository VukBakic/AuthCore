
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o => {
		var Key = Encoding.UTF8.GetBytes(builder.Configuration["JsonWebTokenSetting:Key"]);
		o.SaveToken = true;
		o.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false, 
			ValidateAudience = false, 
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["JsonWebTokenSetting:Issuer"],
			ValidAudience = builder.Configuration["JsonWebTokenSetting:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Key),
			ClockSkew = TimeSpan.Zero
		};
	});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
