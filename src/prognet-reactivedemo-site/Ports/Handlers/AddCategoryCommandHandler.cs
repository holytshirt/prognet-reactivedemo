using Common.Logging;
using paramore.brighter.commandprocessor;
using reactivedemosite.Domain;
using reactivedemosite.Ports.Commands;
using reactivedemosite.Ports.Persistance;

namespace reactivedemosite.Ports.Handlers
{
    public class AddCategoryCommandHandler : RequestHandler<AddCategoryCommand>
    {
        private readonly IAmACategoriesDatabase _categoriesDatabase;

        public AddCategoryCommandHandler(IAmACategoriesDatabase categoriesDatabase, ILog logger) : base(logger)
        {
            _categoriesDatabase = categoriesDatabase;
        }

        public override AddCategoryCommand Handle(AddCategoryCommand addCategoryCommand)
        {
            Category category = _categoriesDatabase.Add(new Category(addCategoryCommand.Name));

            addCategoryCommand.CategoryId = category.Id;

            return addCategoryCommand;
        }
    }
}