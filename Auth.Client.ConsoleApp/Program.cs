using Auth.Client.ConsoleApp.Consts;
using Auth.Client.ConsoleApp.Interfaces.Api;
using Auth.Client.ConsoleApp.Services.Actions;
using Auth.Client.ConsoleApp.Services.Api;
using Auth.Client.ConsoleApp.Services.Configurations;
using Auth.Client.ConsoleApp.Tools;

string uri = "";
IUnitOfWorkApi _uowApi = null;
ConfigurationService configuration = new();
BaseActionService _actions = null;

Console.WriteLine("Chose server type: \n 1) Local \n 2) Server");
var type = Console.ReadLine();
switch (type)
{
    default:
        uri = AuthConsts.LOCAL_SEREVER_URI; break;
}

try
{
    _uowApi = new UnitOfWorkServerApi(uri);
    await _uowApi.Account().TryLoginAsync();

    _actions = new BaseActionService(
        new ChatActionService(_uowApi, uri),
        new RabbitMQActionService(
            configuration.Get<RabbitMQOptions>(AppConsts.RABBITMQ_SECTION_NAME))
        );
    await _actions.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
finally
{
    _actions?.Dispose();
    _uowApi?.Dispose();
}