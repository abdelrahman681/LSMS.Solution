using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Enum
{
    public enum CashMovementType
    {
        Sale = 1,
        SaleReturn = 2,
        Expense = 3,
        SupplierPayment = 4,
        OpeningBalance = 5,
        CashIn = 6,
        CashOut = 7
    }
}
