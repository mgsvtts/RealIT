using System.Net.Http.Headers;
using Infrastructure.HttpClients;
using Web;

var app = WebApplication.CreateBuilder(args)
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation()
    .Build();

app.AddMiddlewares().Run();
