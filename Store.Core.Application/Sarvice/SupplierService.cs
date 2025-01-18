using Microsoft.EntityFrameworkCore;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories.IRepositories;
 

namespace Store.Core.Application.Sarvice
{
    public class SupplierService : ISupplierService
    {
        private readonly IGenericRepository<Supplier> _repository;
        public SupplierService(IGenericRepository<Supplier> repository)
        {
            _repository = repository;


        }
        public async Task AddSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = new Supplier
            {
                SupplierName = supplierDto.SupplierName,
                Phone = supplierDto.Phone
            };

            await _repository.AddAsync(supplier);
        }



        public async Task<SupplierDto> GetSupplierByIdAsync(int id)
        {

            var supplier = await _repository.GetByIdAsync(id);
            if (supplier == null)
                return null;

            return new SupplierDto
            {
                SupplierID = supplier.SupplierID,
                SupplierName = supplier.SupplierName,
                Phone = supplier.Phone
            };
           
        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliersAllAsync()
        {
            var suppliers = await _repository.GetTableNoTracking().ToListAsync();
            return suppliers.Select(supplier => new SupplierDto
            {
                SupplierID = supplier.SupplierID,
                SupplierName = supplier.SupplierName,
                Phone = supplier.Phone
            }).ToList();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            await _repository.UpdateAsync(supplier);
        }
        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);
            if (supplier != null)
            {
                await _repository.DeleteAsync(supplier);
            }
        }

    }



}
