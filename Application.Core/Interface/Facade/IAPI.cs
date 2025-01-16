using Application.Core.Interface.Database;
using Common.Core.Interface.Facade.API;

namespace Application.Core.Interface.Facade;

public interface IAPIFacade : IAbstractFacade
{
    ISQLContext sqlContext { get; }
}
