using Mapster;
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
            var supplier = supplierDto.Adapt<Supplier>();

            await _repository.AddAsync(supplier);
        }



        public async Task<SupplierDto> GetSupplierByIdAsync(int id)
        {

            var supplier = await _repository.GetByIdAsync(id);
            if (supplier == null)
                return null;

            return supplier.Adapt<SupplierDto>();

        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliersAllAsync()
        {
            var suppliers = await _repository.GetTableNoTracking().ToListAsync();
            return suppliers.Adapt<List<SupplierDto>>();
        }

        public async Task UpdateSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = supplierDto.Adapt<Supplier>();
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
