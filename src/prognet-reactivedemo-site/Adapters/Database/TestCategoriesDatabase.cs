using System.IO;
using System.Reflection;
using reactivedemosite.Domain;
using reactivedemosite.Ports.Persistance;

namespace reactivedemosite.Adapters.Database
{
    public class TestCategoriesDatabase : IAmACategoriesDatabase
    {
        private readonly dynamic _database;

        public TestCategoriesDatabase()
        {
            string path =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)),
                    @"App_Data\howmuchyouspend.sdf");

            _database = Simple.Data.Database.Opener.OpenFile(path);

            Reset();
        }

        public Category Add(Category category)
        {
            return _database.Categories.Insert(category);
        }

        public void Reset()
        {
            _database.Categories.DeleteAll();
        }


        public Category Get(int categoryId)
        {
            return _database.Categories.FindById(categoryId);
        }
    }
}