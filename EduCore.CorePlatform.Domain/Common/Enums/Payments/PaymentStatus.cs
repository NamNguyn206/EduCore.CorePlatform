using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.Payments;

public enum PaymentStatus
{
    Unpaid = 0, // Chưa thanh toán
    Pending = 1, // Đang chờ xác nhận thanh toán
    Paid = 2, // Đã thanh toán
    Failed = 3, // Thanh toán thất bại
    Refunded = 4 // Đã hoàn tiền
}
