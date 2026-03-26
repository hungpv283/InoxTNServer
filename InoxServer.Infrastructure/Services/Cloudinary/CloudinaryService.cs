using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InoxServer.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace InoxServer.Infrastructure.Services.Cloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var section = configuration.GetSection("Cloudinary");
        var account = new Account(
            section["CloudName"]!,
            section["ApiKey"]!,
            section["ApiSecret"]!
        );
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        _cloudinary.Api.Secure = true; // dùng HTTPS
    }

    public async Task<string> UploadImageAsync(
        Stream fileStream,
        string fileName,
        string folder,
        CancellationToken cancellationToken = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = folder,
            // Tự động crop về hình vuông, lấy khuôn mặt làm trung tâm
            Transformation = new Transformation()
                .Width(400).Height(400)
                .Crop("fill").Gravity("face")
        };

        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (result.Error is not null)
            throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");

        return result.SecureUrl.ToString();
    }

    public async Task DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}
