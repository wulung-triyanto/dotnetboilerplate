using Common.Core.Interface.Facade.Scheduler;

namespace Common.Core.Abstract.Scheduler;

/// <summary>
/// Abstract handler for scheduler
/// </summary>
/// <param name="facade"></param>
public abstract class AbstractHandler(IAbstractFacade facade)
{
    public readonly IAbstractFacade facade = facade;
}
