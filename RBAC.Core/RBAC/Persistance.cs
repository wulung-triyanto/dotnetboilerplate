namespace Security.Core.RBAC;

public interface IRBACPersistance
{
    Task<DRBACModel> GetDRBACPermissionAsync(string key, string userId, CancellationToken cancellationToken);
    Task<List<DRBACPOAModel>> GetDRBACPOAPermissionAsync(string key, string userId, CancellationToken cancellationToken);
    Task<RBACPermission> GetRBACPermissionAsync(string key, string feature, CancellationToken cancellationToken);
    Task<List<RBACPOA>> GetRBACPOAPermissionAsync(string key, string feature, CancellationToken cancellationToken);
    Task<TimeSpan?> GetTimeToLiveAsync(string key, CancellationToken cancellationToken);
}
