using Common.Core.Interface.Facade.Worker;

namespace Common.Core.Abstract.Worker;

/// <summary>
/// Abstract handler for message broker worker
/// </summary>
/// <param name="facade"></param>
public abstract class AbstractHandler(IAbstractFacade facade)
{
    public readonly IAbstractFacade facade = facade;
}