using System;
using System.Threading.Tasks;
using Common;

public interface IPriceService
{
    Task<Price> GetPriceByProductId(int productId);
}