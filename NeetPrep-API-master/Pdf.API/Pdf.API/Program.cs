using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pdf.API;
using System.Text;


// Add services to the container.============================================================
var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    services.AddControllers();
    //services.AddCors();
    services.AddSwaggerGen();

    services.AddSingleton<IConfiguration>(builder.Configuration);

    services.CustonServicesExtension(builder.Configuration);
    
    services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AuthDetails:SecurityKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
}



// Configure the HTTP request pipeline.=======================================================
var app = builder.Build();
{
    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseAuthentication();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
    });

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseCors(opt => opt
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

app.Run();
