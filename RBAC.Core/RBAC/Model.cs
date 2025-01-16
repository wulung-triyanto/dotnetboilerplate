using Common.Core.Enum;

namespace Security.Core.RBAC;

public class RBACModel
{
    public int permissionId { get; set; }
    public int parentFeatureId { get; set; }
    public string featureName { get; set; }
    public string attributeId { get; set; }
    public int active { get; set; }
    public int userId { get; set; }
}

public class RBACPermission
{
    public int UserId { get; set; }
    public PermissionStatus PermissionStatus { get; set; }
}

public class RBACPOA
{
    public int roleId { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public int value { get; set; }
}

public class DRBACModel
{
    public List<int> companies { get; set; }
    public List<int> businessUnits { get; set; }
    public List<int> branches { get; set; }
    public List<int> locations { get; set; }
}

public class DRBACPOAModel
{
    public List<int> companies { get; set; }
    public List<int> businessUnits { get; set; }
    public List<int> branches { get; set; }
    public List<int> locations { get; set; }
    public List<int> roles { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
}