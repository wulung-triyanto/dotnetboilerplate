using Common.Core.Enum;

namespace Common.Core.Function;

[ExcludeFromCodeCoverage]
public class Message
{
    public static string Accepted(string module)
    {
        return $"Data {module} has been successfully submited, please refresh your page periodically.";
    }

    public static string AllBranch()
    {
        return $"The data branch in this contract is all branch in database.";
    }

    public static string ALLBranchValidation(string module)
    {
        return $"Material {module} should not be all branch if there is more than one contract item.";
    }

    public static string BatchInsertExceed(string entity, int count)
    {
        return $"Cannot create more than {count} items into {entity} at once";
    }

    public static string BeingCreated(string module)
    {
        return $"{module} is being created";
    }

    public static string BranchDuplicate(string module)
    {
        return $"Branch {module} is duplicated.";
    }

    public static string CantBeUsed(string module)
    {
        return $"{module} Allow Duplicate is true, cant Be Used at Start or End Rental";
    }

    public static string CanAddContract()
    {
        return $"Can not add more than 10 contract.";
    }

    public static string CanAddvariant()
    {
        return $"Can not add more than 1 variant.";
    }

    public static string CategoryGreaterZero()
    {
        return $"One of the categories must be greater than zero.";
    }

    public static string Created(string module)
    {
        return $"Data {module} has been created.";
    }

    public static string DeleteFail(string module)
    {
        return $"{module} data failed to delete.";
    }

    public static string DeleteSuccess(string module)
    {
        return $"{module} data deleted successfully.";
    }

    public static string DriverDuplicate(string module)
    {
        return $"Driver {module} with this detail is duplicated.";
    }

    public static string DriverDuplicateInDB()
    {
        return $"Driver with this detail already exists.";
    }

    public static string Empty(string module)
    {
        return $"{module} data cannot be empty.";
    }

    public static string EmptyData(string module)
    {
        return $"{module} data 0";
    }

    public static string ExpenseDuplicate(string module)
    {
        return $"Expense {module} with this detail is duplicated.";
    }

    public static string ExpenseDuplicateInDB(string module)
    {
        return $"Expense {module} with this detail already exists.";
    }

    public static string Exist(string module, string key)
    {
        return $"{module} data with Id {key} is already exists.";
    }

    public static string Exist(string module)
    {
        return $"{module} data already exists.";
    }

    public static string Exception()
    {
        return "There is an error. Please contact your administrator.";
    }

    public static string Exception(string process)
    {
        return $"There is an error on process {process}.";
    }

    public static string Fail(string process)
    {
        return $"{process} is failed.";
    }

    public static string FailAddContractItem()
    {
        return $"Can not add more than 10 contract item.";
    }

    public static string Found(string module)
    {
        return $"{module} data found.";
    }

    public static string Found(int count, string module)
    {
        if (count <= 0)
        {
            return NotFound(module);
        }

        return $"{count} data {module} found.";
    }

    public static string FoundIf(bool condition, string module)
    {
        return condition ? Found(module) : NotFound(module);
    }

    public static string FmsValidation(string module)
    {
        return $"You cannot delete {module} data Source From Not FMS";
    }

    public static string hasBeenValidated(string module)
    {
        return $"Data {module} has been validated";
    }

    public static string Insert(string module)
    {
        return $"Insert data {module}";
    }

    public static string InsertFail(string module)
    {
        return $"{module} data failed to save.";
    }

    public static string InsertSuccess(string module)
    {
        return $"{module} data saved successfully.";
    }

    public static string InvalidEvent()
    {
        return $"Invalid or empty event method.";
    }

    public static string InvalidReferenceName()
    {
        return $"Invalid vehicle reference name, allowed values is BRAND, CATEGORY, MODEL, COLOR, FUELTYPE";
    }

    public static string InvalidRegexCode()
    {
        return $"Numbers and letters only please.";
    }

    public static string InvalidRegexName()
    {
        return $"Numbers, letters, space, point, comma, dash and parentheses only please.";
    }

    public static string InvalidValue(string module)
    {
        return $"Data {module} has Invalid Value";
    }

    public static string LocationIsUsed()
    {
        return $"This location has been used in another service, only Operational Hours and Address Note can be update.";
    }

    public static string lowerThan(string module)
    {
        return $"{module} value cannot be lower than previous value";
    }

    public static string MaterialDuplicate(string module)
    {
        return $"Material {module} with this detail is duplicated.";
    }

    public static string MaterialDuplicateInDB(string module)
    {
        return $"Material {module} with this detail already exists.";
    }

    public static string MultiInvalid(string module)
    {
        return $"One or more {module} has invalid";
    }

    public static string MultiNotFound(string module)
    {
        return $"One or more {module} not found";
    }

    public static string mustBePool(string module)
    {
        return $"{module} must be Location Type Pool";
    }

    public static string NotAllowDuplicated(string module)
    {
        return $"{module} Not Allow To Be Duplicated";
    }

    public static string NotCriteria(string module)
    {
        return $"Data {module} is not match requirment";
    }

    public static string NotEmpty(string module)
    {
        return $"{module} should not be empty.";
    }

    public static string NotExist(string module)
    {
        return $"Could not find {module}.";
    }

    public static string NotFound(string module)
    {
        return $"{module} data not found.";
    }

    public static string NotFoundInList(string module)
    {
        return $"{module} not found in detail lists";
    }

    public static string NotificationTitle(string module)
    {
        return $"Data {module} has been successfully received.";
    }

    public static string ProsesInprogress(string module)
    {
        return $"{module} data on progress";
    }

    public static string ResourceLocking(string module)
    {
        return $"{module} data still in progress on another process or have been deleted";
    }

    public static string RouteVariantLimit(string module)
    {
        return $"This route plan id of {module} only have 1 variant route, reached limit of deletion.";
    }

    public static string SAGACompensationFailed(string module, string transactionId)
    {
        return $"SAGA Compensation in {module} with {transactionId} Has Failed Because Last Status is not {(int)EventStatus.COMPENSATION_FAILED}";
    }

    public static string StartLowerOrEqualThanEnd()
    {
        return $"Start Rental Activity Sequence cant lower or equal than End Rental Activity Sequence";
    }

    public static string Success(string process)
    {
        return $"{process} is successful.";
    }

    public static string syncStatusProgress(string module)
    {
        return $"{module} data still in sync status inprogress or failed";
    }

    public static string SyncFail(string module)
    {
        return $"{module} data failed to synced.";
    }

    public static string SyncSuccess(string module)
    {
        return $"{module} data synced successfully.";
    }

    public static string ToBeValidated(string module)
    {
        return $"Data {module} to be validated.";
    }

    public static string Unauthorized()
    {
        return $"User unauthorized.";
    }

    public static string UnauthorizedDelete(string moduleName)
    {
        return $"User don't have access to delete {moduleName}.";
    }

    public static string UnathorizedDRBAC(string module)
    {
        return $"You do not have authorization to access this {module} data.";
    }

    public static string Unverified()
    {
        return $"User is not verified.";
    }

    public static string UsedValidation(string module)
    {
        return $"Can't update or delete because {module} has been used in another service.";
    }

    public static string VariantNameDuplicate(string module)
    {
        return $"Variant Name {module} with this detail is duplicated.";
    }

    public static string VendorContractItemInvalidPeriod()
    {
        return $"Contract item schedule invalid period input.";
    }

    public static string VendorContractItemInvalidStartDate()
    {
        return $"Contract item schedule invalid availability start date";
    }

    public static string VMValidation(string module)
    {
        return $"You cannot delete {module} data originated from VM";
    }
}
