using System;
using System.Threading.Tasks;
using System.Web;
using Common.Logging;
using OpenRasta.DI;
using OpenRasta.Pipeline;
using OpenRasta.Web;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.ioccontainers.Adapters;
using paramore.brighter.commandprocessor.messaginggateway.rmq;
using Polly;
using reactivedemosite.Adapters.Database;
using reactivedemosite.Ports.Commands;
using reactivedemosite.Ports.Handlers;
using reactivedemosite.Ports.Persistance;
using reactivedemosite.Ports.ViewModelRetrievers;
using TinyIoC;

namespace reactivedemosite.Adapters.API.Configuration
{
    public class DependencyPipelineContributor : IPipelineContributor
    {
        private readonly IDependencyResolver resolver;

        public DependencyPipelineContributor(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(InitializeContainer)
                .Before<KnownStages.IOperationExecution>();
        }

        private PipelineContinuation InitializeContainer(ICommunicationContext arg)
        {
            var container = new TinyIoCAdapter(new TinyIoCContainer());
            //HACK! For now dependencies may need to be in both containers to allow resolution
            container.Register<IHandleRequests<AddCategoryCommand>, AddCategoryCommandHandler>().AsMultiInstance();
            container.Register<IAmACategoryViewModelRetriever, CategoryViewModelRetriever>().AsMultiInstance();
            container.Register<IAmACategoriesDatabase, CategoriesDatabase>().AsMultiInstance();
            ILog logger = LogManager.GetLogger("Categories");
            container.Register<ILog, ILog>(logger);
            container.Register<IAmARequestContextFactory, InMemoryRequestContextFactory>().AsMultiInstance();

            container.Register<IAmAMessageStore<Message>, SqlCe40MessageStore>().AsSingleton();
            container.Register<IAmAMessagingGateway, RMQMessagingGateway>().AsSingleton();

            container.Register(CommandProcessor.RETRYPOLICY, GetRetryPolicy());
            container.Register(CommandProcessor.CIRCUITBREAKER, GetCircuitBreakerPolicy());

            CommandProcessor commandProcessor = new CommandProcessorFactory(container).Create();
            container.Register<IAmACommandProcessor, IAmACommandProcessor>(commandProcessor);

            resolver.AddDependencyInstance<IAdaptAnInversionOfControlContainer>(container, DependencyLifetime.Singleton);
            resolver.AddDependencyInstance<IAmARequestContextFactory>(new InMemoryRequestContextFactory(),
                DependencyLifetime.PerRequest);
            resolver.AddDependencyInstance<IAmACommandProcessor>(commandProcessor, DependencyLifetime.Singleton);
            resolver.AddDependency<IAmACategoryViewModelRetriever, CategoryViewModelRetriever>(
                DependencyLifetime.Singleton);
            resolver.AddDependency<IAmACategoriesDatabase, CategoriesDatabase>(DependencyLifetime.PerRequest);


            return PipelineContinuation.Continue;
        }

        private Policy GetCircuitBreakerPolicy()
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreaker(2, TimeSpan.FromMilliseconds(500));
        }

        private Policy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                }, (exception, timeSpan) =>
                {
                    ILog logger = LogManager.GetLogger("RetryPolicy");
                    logger.Error(
                        m => m("Error during decoupled invocation attempt: {0}, retrying in {1)", exception, timeSpan));
                });
        }
    }

    public class SqlCe40MessageStore : IAmAMessageStore<Message>
    {
        private readonly dynamic _database;

        public SqlCe40MessageStore()
        {
            string path = HttpContext.Current.Server.MapPath("~\\App_Data\\howmuchyouspend.sdf");
            _database = Simple.Data.Database.Opener.OpenFile(path);
        }

        public Task Add(Message message)
        {
            return new Task(() => _database.Messages.Insert(message));
        }

        public Task<Message> Get(Guid messageId)
        {
            return new Task<Message>(() => _database.Messages.FindById(messageId));
        }
    }
}