using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_Repository_Data_Layer.Repository.IRepos
{
    public interface IProductRepo
    {
        public Task<List<Product>> GetAllProducts();

        public Task<Product> GetProductById(int id);

        public Task<Product> GetProductByName(string name);

        public Task<int> InsertProduct(Product product);

        public Task<int> UpdateProduct(Product product);

        public Task<int> DeleteProduct(int id);

        public Task<bool> SaveProductPicturePath(string RelativePath, int ProductId);
    }
}
