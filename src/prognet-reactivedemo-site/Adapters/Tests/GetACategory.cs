using Machine.Specifications;
using reactivedemosite.Adapters.API.Resources;
using reactivedemosite.Adapters.Database;
using reactivedemosite.Domain;
using reactivedemosite.Ports.ViewModelRetrievers;

namespace reactivedemosite.Adapters.Tests
{
    [Subject("Category")]
    public class GetACategory
    {
        public class When_I_get_a_category
        {
            private static CategoryViewModelRetriever _categoryViewModelRetriever;
            private static CategoryViewModel _category;
            private static Category _expectedCategory;

            private Establish context = () =>
            {
                var database = new TestCategoriesDatabase();

                _expectedCategory = database.Add(new Category("coffee"));

                _categoryViewModelRetriever = new CategoryViewModelRetriever(database);
            };

            private Because of = () => { _category = _categoryViewModelRetriever.Get(_expectedCategory.Id); };

            private It should_return_the_correct_category =
                () => { _category.Name.ShouldEqual(_expectedCategory.Name); };
        }
    }
}