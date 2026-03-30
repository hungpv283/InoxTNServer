using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.UploadAvatar;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;

    public UploadAvatarCommandHandler(
        IUserRepository userRepository,
        ICloudinaryService cloudinaryService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _cloudinaryService = cloudinaryService;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new DomainException(UserErrors.NotFound);

        // Upload ảnh mới lên Cloudinary, lưu vào folder "avatars"
        var avatarUrl = await _cloudinaryService.UploadImageAsync(
            request.FileStream,
            request.FileName,
            folder: "avatars",
            cancellationToken);

        user.AvatarUrl = avatarUrl;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return avatarUrl;
    }
}
