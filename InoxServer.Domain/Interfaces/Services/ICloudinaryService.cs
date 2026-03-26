namespace InoxServer.Domain.Interfaces.Services;

public interface ICloudinaryService
{
    /// <summary>
    /// Upload ảnh lên Cloudinary, trả về URL public của ảnh.
    /// </summary>
    /// <param name="fileStream">Stream của file ảnh</param>
    /// <param name="fileName">Tên file gốc (dùng để đặt tên trên Cloudinary)</param>
    /// <param name="folder">Thư mục trên Cloudinary (vd: "avatars", "products")</param>
    Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa ảnh khỏi Cloudinary theo publicId.
    /// </summary>
    Task DeleteImageAsync(string publicId, CancellationToken cancellationToken = default);
}
