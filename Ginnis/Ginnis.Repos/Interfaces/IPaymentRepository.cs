using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IActionResult> CreateOrder(int amount, string orderId, Guid UserID);

        Task<IActionResult> ConfirmPayment(JsonElement data, string orderID, Guid userID);

        Task<IActionResult> FailurePayment(JsonElement data);

        Task<IActionResult> RefundOrder(JsonElement data);



        //Task<List<RazorpayPayment>> GetOrderByUserID(Guid userId);
        //Task<RazorpayPayment> GetOrderByID(string orderID);
    }
}
