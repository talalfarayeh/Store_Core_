using Mapster;
using Store.Core.Application.DTOs;
using Store.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Supplier, SupplierDto>.NewConfig()
                .Map(dest=>dest.SupplierID,src =>src.SupplierID)
                .Map(dest => dest.SupplierName, src => src.SupplierName)
                .Map(dest => dest.Phone, src => src.Phone);

            TypeAdapterConfig<Product, ProductDto>.NewConfig()
          .Map(dest => dest.ProductID, src => src.ProductID)
          .Map(dest => dest.ProductName, src => src.ProductName)
          .Map(dest => dest.QuantityInStock, src => src.QuantityInStock)
          .Map(dest => dest.UnitPrice, src => src.UnitPrice)
          .Map(dest => dest.SupplierName, src => src.Supplier != null ? src.Supplier.SupplierName : "No Supplier");

            TypeAdapterConfig<Product, ProductCreateUpdateDto>.NewConfig()
                 .Map(dest => dest.ProductName, src => src.ProductName)
                 .Map(dest => dest.QuantityInStock, src => src.QuantityInStock)
                 .Map(dest => dest.UnitPrice, src => src.UnitPrice)
                 .Map(dest => dest.SupplierID, src => src.Supplier.SupplierID  );

            TypeAdapterConfig<Order, OrderDto>.NewConfig()
                 .Map(dest => dest.OrderID, src => src.OrderID)
                 .Map(dest => dest.OrderDate, src => src.OrderDate)
                 .Map(dest => dest.CustomerName, src => src.CustomerName)
                 .Map(dest => dest.Quantity, src => src.Quantity)
                 .Map(dest => dest.ProductName, src => src.Product.ProductName)
                    .Map(dest => dest.UserId, src => src.UserId)
             .Map(dest => dest.ProductID, src => src.Product.ProductID);

            TypeAdapterConfig<OrderDto, Order>.NewConfig()
                  .Ignore(dest => dest.Product);

            TypeAdapterConfig<StockMovement,StockMovementDto>.NewConfig ()
                .Map(dest => dest.StockMovementID, src => src.StockMovementID)
                .Map(dest => dest.ProductID, src => src.ProductID)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.MovementDate, src => src.MovementDate)
                .Map(dest => dest.MovementType, src => src.MovementType)
                .Map(dest => dest.ProductName, src => src.Product.ProductName);

            TypeAdapterConfig<StockMovementDto, StockMovement>.NewConfig()
                  .Ignore(dest => dest.Product);

            




        }
    }
}
