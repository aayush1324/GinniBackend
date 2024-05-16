using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IOrderRepository
    {
        Task<string> CreateOrder(Guid userId);

        Task<string> CreateOrder(Guid userId, Guid productId);

        Task<ActionResult<List<OrdersDTO>>> GetOrders(Guid userId);

        Task<ActionResult<List<OrderDetailDTO>>> GetOrderByOrderId(string orderId);

        Task<ActionResult<List<OrderListDTO>>> GetAllOrders();

    }
}
