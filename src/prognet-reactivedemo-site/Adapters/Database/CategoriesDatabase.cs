using System.Web;
using reactivedemosite.Domain;
using reactivedemosite.Ports.Persistance;

namespace reactivedemosite.Adapters.Database
{
    public class CategoriesDatabase : IAmACategoriesDatabase
    {
        private readonly dynamic _database;

        public CategoriesDatabase()
        {
            string path = HttpContext.Current.Server.MapPath("~\\App_Data\\howmuchyouspend.sdf");
            _database = Simple.Data.Database.Opener.OpenFile(path);
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