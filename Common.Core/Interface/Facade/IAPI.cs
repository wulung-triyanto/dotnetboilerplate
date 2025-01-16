using Common.Core.Interface.Message;
using Common.Core.Interface.NoSQL;
using Microsoft.AspNetCore.Http;

namespace Common.Core.Interface.Facade.API;

public interface IAbstractFacade
{
    ICosmosContext cosmosContext { get; }
    IFirebaseContext firebaseContext { get; }
    IHttpContextAccessor httpContext { get; }
    IMessageContext messageContext { get; }
}
