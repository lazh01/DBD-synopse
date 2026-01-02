using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cqrs.Domain.Entities;
using Cqrs.Application.Dtos;

namespace Cqrs.Application.Mappers;

public static class DtoMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            UnitPrice = product.UnitPrice
        };
    }

    public static OrderLineDto ToDto(this OrderLine line)
    {
        return new OrderLineDto
        {
            Id = line.Id,
            ProductId = line.ProductId,
            ProductName = line.Product.Name,
            Quantity = line.Quantity,
            UnitPrice = line.UnitPrice
        };
    }

    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            User = order.User.ToDto(),
            CreatedAt = order.CreatedAt,
            Lines = order.Lines.Select(l => l.ToDto()).ToList(),
            TotalAmount = order.Lines.Sum(l => l.Quantity * l.UnitPrice)
        };
    }
}
