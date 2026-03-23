using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
        }

        public async Task<Payment?> GetByPayosOrderCodeAsync(long payosOrderCode, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.PayosOrderCode == payosOrderCode, cancellationToken);
        }

        public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            await _context.Payments.AddAsync(payment, cancellationToken);
        }

        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
        }

        public void Delete(Payment payment)
        {
            _context.Payments.Remove(payment);
        }
    }
}
