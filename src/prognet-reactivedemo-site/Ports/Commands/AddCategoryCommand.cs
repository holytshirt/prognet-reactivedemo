using System;
using paramore.brighter.commandprocessor;

namespace reactivedemosite.Ports.Commands
{
    public class AddCategoryCommand : ICommand
    {
        public AddCategoryCommand(string name)
        {
            Name = name;
        }

        public int CategoryId { get; set; }

        public string Name { get; private set; }

        public Guid Id { get; set; }
    }
}