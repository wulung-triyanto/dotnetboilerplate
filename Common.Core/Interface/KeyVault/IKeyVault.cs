namespace Common.Core.Interface.KeyVault;

public interface IKeyVault
{
    Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken);
    string GetSecret(string secretName);
}
