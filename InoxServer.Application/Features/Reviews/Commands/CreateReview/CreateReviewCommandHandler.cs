using InoxServer.Application.Features.Reviews.DTOs;
using InoxServer.Domain.Entities;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra sản phẩm có tồn tại không
            var productExists = await _productRepository.ExistsAsync(request.ProductId, cancellationToken);
            if (!productExists)
                throw new DomainException(ReviewErrors.ProductNotFound);

            // 2. Kiểm tra user đã đánh giá sản phẩm này chưa
            var existingReview = await _reviewRepository.GetByUserAndProductAsync(
                request.UserId, request.ProductId, cancellationToken);
            if (existingReview != null)
                throw new DomainException(ReviewErrors.AlreadyReviewed);

            // 3. Nếu có OrderId, kiểm tra đơn hàng phải Completed mới cho đánh giá
            if (request.OrderId.HasValue)
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId.Value, cancellationToken);
                if (order == null)
                    throw new DomainException(OrderErrors.NotFound);

                if (order.UserId != request.UserId)
                    throw new DomainException(ReviewErrors.Unauthorized);

                // Quan trọng: Chỉ cho đánh giá khi order status là Completed
                if (order.Status != OrderStatus.Completed)
                    throw new DomainException(ReviewErrors.OrderNotCompleted);

                // Kiểm tra sản phẩm có trong đơn hàng không
                var productInOrder = order.OrderItems.Any(oi => oi.ProductId == request.ProductId);
                if (!productInOrder)
                    throw new DomainException(ReviewErrors.NotPurchased);
            }

            // 4. Tạo Review
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                UserId = request.UserId,
                OrderId = request.OrderId,
                Rating = request.Rating,
                Comment = request.Comment,
                IsApproved = true, // Cần admin duyệt
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}
