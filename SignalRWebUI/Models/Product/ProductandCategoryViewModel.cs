using System;
using SignalRWebUI.Dtos.CategoryDtos;
using SignalRWebUI.Dtos.ProductDtos;

namespace SignalRWebUI.Models.Product
{
    public class ProductandCategoryViewModel
    {
        public List<ResultProductWithCategoryDto> ProductsWithCategory { get; set; }
        public List<ResultCategoryDto> Categories { get; set; }

    }
}