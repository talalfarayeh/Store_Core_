using Store.Core.Application.DTOs;
using Store.Infrastructure.Models;
 

namespace Store.Core.Application.Sarvice.ISarvice
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetSuppliersAllAsync();
        Task<SupplierDto> GetSupplierByIdAsync(int id);
        Task AddSupplierAsync(SupplierDto supplierDto);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(int id);

    }
}
