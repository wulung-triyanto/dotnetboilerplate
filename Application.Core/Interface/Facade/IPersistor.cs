using Application.Core.Interface.Database;
using Common.Core.Interface.Facade.Worker;

namespace Application.Core.Interface.Facade;

public interface IPersistorFacade : IAbstractFacade
{
    ISQLContext sqlContext { get; }
}
