using Amazon.S3;
using Amazon.S3.Transfer;

namespace Bicycle.Services;

public class AmazonS3Service
{
    public AmazonS3Service(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:BucketName"];
    }
    
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    
    public async Task<string> UploadFileAsync(IFormFile file, string key)
    {
        var fileTransferUtility = new TransferUtility(_s3Client);

        using (var newMemoryStream = new MemoryStream())
        {
            await file.CopyToAsync(newMemoryStream);
            await fileTransferUtility.UploadAsync(newMemoryStream, _bucketName, key);
        }

        return $"https://{_bucketName}.s3.amazonaws.com/{key}";
    }

    public async Task DeleteFileAsync(string? key)
    {
        if (!string.IsNullOrEmpty(key))
            await _s3Client.DeleteObjectAsync(_bucketName, key);
    }
}