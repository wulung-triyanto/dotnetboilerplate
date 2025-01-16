namespace Common.Core.Interface.BlobStorage;

public interface IBlobStorage
{
    string FileName { get; }

    Task<string> CopyFileAsync(string containerName, string oldFile, string newFile, CancellationToken cancellationToken);
    Task<string> ReadFileAsync(string containerName, string prefix, CancellationToken cancellationToken);
    Task<List<string>> ReadLineFileAsync(string containerName, string prefix, CancellationToken cancellationToken);
    Task<bool> RemoveFileAsync(string containerName, string fileName, CancellationToken cancellationToken);
    string Upload(string containerName, string blobName, bool overwrite, Stream content);
    Task<string> UploadAsync(string containerName, string blobName, bool overwrite, Stream content, CancellationToken cancellationToken);
}
