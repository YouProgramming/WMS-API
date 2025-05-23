using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_Repository_Data_Layer.Repository.IRepos
{
    public interface ICategoryRepo
    {
        public Task<List<Category>> GetAllCategories();

        public Task<Category> GetCategoryById(int id);

        public Task<Category> GetCategoryByName(string name);

        public Task<int> InsertCategory(Category category);
        
        public Task<int> UpdateCategory(Category category);

        public Task<int> DeleteCategory(int id);
    }
}
