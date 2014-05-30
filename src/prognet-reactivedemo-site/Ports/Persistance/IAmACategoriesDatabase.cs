using reactivedemosite.Domain;

namespace reactivedemosite.Ports.Persistance
{
    public interface IAmACategoriesDatabase
    {
        Category Add(Category category);
        void Reset();
        Category Get(int categoryId);
    }
}