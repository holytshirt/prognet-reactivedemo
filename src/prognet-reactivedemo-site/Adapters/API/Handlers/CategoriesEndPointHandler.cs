using OpenRasta.Web;
using paramore.brighter.commandprocessor;
using reactivedemosite.Adapters.API.Resources;
using reactivedemosite.Ports.Commands;
using reactivedemosite.Ports.ViewModelRetrievers;

namespace reactivedemosite.Adapters.API.Handlers
{
    public class CategoriesEndPointHandler
    {
        private readonly IAmACategoryViewModelRetriever _categoryViewModelRetriever;
        private readonly IAmACommandProcessor _commandProcessor;

        public CategoriesEndPointHandler(IAmACommandProcessor commandProcessor,
            IAmACategoryViewModelRetriever categoryViewModelRetriever)
        {
            _commandProcessor = commandProcessor;
            _categoryViewModelRetriever = categoryViewModelRetriever;
        }

        public OperationResult Get(int id)
        {
            CategoryViewModel categoryViewModel = _categoryViewModelRetriever.Get(id);

            if (categoryViewModel == null)
                return new OperationResult.NotFound();

            return new OperationResult.OK
            {
                ResponseResource = categoryViewModel
            };
        }

        public OperationResult Post(CategoryViewModel categoryViewModel)
        {
            var createNewCategoryCommand = new AddCategoryCommand(categoryViewModel.Name);
            _commandProcessor.Send(createNewCategoryCommand);

            CategoryViewModel returnedCategoryViewModel =
                _categoryViewModelRetriever.Get(createNewCategoryCommand.CategoryId);

            return new OperationResult.Created
            {
                ResponseResource = returnedCategoryViewModel,
                CreatedResourceUrl = returnedCategoryViewModel.CreateUri()
            };
        }
    }
}