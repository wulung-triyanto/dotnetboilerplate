using Common.Core.Interface.Message;

namespace Common.Core.Interface.Facade.Scheduler;

public interface IAbstractFacade
{
    IMessageContext messageContext { get; }
}
