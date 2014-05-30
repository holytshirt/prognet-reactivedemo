using Machine.Specifications;
using reactivedemosite.Adapters.Database;
using reactivedemosite.Ports.Commands;
using reactivedemosite.Ports.Handlers;

namespace reactivedemosite.Adapters.Tests
{
    [Subject("Category")]
    public class AddACategory
    {
        public class When_I_add_category
        {
            private static AddCategoryCommandHandler _addCategoryCommandHandler;
            private static AddCategoryCommand _addCategoryCommand;
            private static AddCategoryCommand _returnedAddCategoryCommand;
            private static TestCategoriesDatabase _categoriesDatabase;

            private Establish context = () =>
            {
                _categoriesDatabase = new TestCategoriesDatabase();

                _addCategoryCommand = new AddCategoryCommand("eating out");

                _addCategoryCommandHandler = new AddCategoryCommandHandler(_categoriesDatabase, null);
            };

            private Because of =
                () => { _returnedAddCategoryCommand = _addCategoryCommandHandler.Handle(_addCategoryCommand); };

            private It should_have_the_category_in_the_database =
                () =>
                {
                    _categoriesDatabase.Get(_returnedAddCategoryCommand.CategoryId)
                        .Name.ShouldEqual(_addCategoryCommand.Name);
                };
        }
    }
}