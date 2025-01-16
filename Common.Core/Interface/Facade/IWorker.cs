using Common.Core.Interface.Message;

namespace Common.Core.Interface.Facade.Worker;

public interface IAbstractFacade
{
    IMessageContext messageContext { get; }
}
