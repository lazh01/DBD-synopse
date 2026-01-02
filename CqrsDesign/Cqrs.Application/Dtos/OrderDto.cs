using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Application.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderLineDto> Lines { get; set; } = new();
    }
}
