using reactivedemosite.Adapters.API.Resources;

namespace reactivedemosite.Ports.ViewModelRetrievers
{
    public interface IAmACategoryViewModelRetriever
    {
        CategoryViewModel Get(int categoryId);
    }
}