namespace Common.Core.Interface.Worker;

public interface IWorkerServiceRunner
{
    Task StartServiceAsync(CancellationToken cancellationToken);
    Task StopServiceAsync(CancellationToken cancellationToken);
}

public interface IServiceBusWorkerService
{
    Task StartWorkAsync(CancellationToken stoppingToken);
    Task StopWorkAsync(CancellationToken cancellationToken);
}

public interface IWorkerService
{
    Task RunWorkerAsync(IService service, CancellationToken cancellationToken);
    Task StopWorkerAsync(IService service, CancellationToken cancellationToken);
}

public interface IService
{
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}

#region Main Services
public interface ICosmosDLQService : IService { }
public interface ICosmosService : IService { }
public interface IFirebaseDLQService : IService { }
public interface IFirebaseService : IService { }
public interface IMSSQLDLQService : IService { }
public interface IMSSQLService : IService { }
#endregion