using reactivedemosite.Adapters.API.Resources;
using reactivedemosite.Domain;
using reactivedemosite.Ports.Persistance;

namespace reactivedemosite.Ports.ViewModelRetrievers
{
    public class CategoryViewModelRetriever : IAmACategoryViewModelRetriever
    {
        private readonly IAmACategoriesDatabase _categoriesDatabase;

        public CategoryViewModelRetriever(IAmACategoriesDatabase categoriesDatabase)
        {
            _categoriesDatabase = categoriesDatabase;
        }

        public CategoryViewModel Get(int categoryId)
        {
            Category category = _categoriesDatabase.Get(categoryId);

            if (category == null) return null;

            return new CategoryViewModel(category.Id, category.Name);
        }
    }
}