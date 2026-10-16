using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.Payments;

public enum PaymentMethod
{
    Wallet = 0, // Thanh toán bằng ví nội bộ
    COD = 1, // Thu tiền mặt
    BankTransfer = 2 // Chuyển khoản điện tử 
}
