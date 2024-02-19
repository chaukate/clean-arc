using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Initializers
{
    public abstract class BaseInitializer
    {
        protected readonly ModelBuilder _modelBuilder;
        public BaseInitializer(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }
    }
}
