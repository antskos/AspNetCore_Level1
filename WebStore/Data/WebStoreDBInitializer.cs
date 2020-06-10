using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Contetxt;

namespace WebStore.Data
{
    

    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;

        public WebStoreDBInitializer(WebStoreDB db) => _db = db;
        
        public void Initialize() 
        {
            var db = _db.Database;

            // проверка создаётся ли новая БД
            //if (db.EnsureDeleted())
            //    if (!db.EnsureCreated())
            //        throw new InvalidOperationException("Ошибка при создании базы данных товаров");

            db.Migrate();

            // если есть товары в базе, то она проинициализирована
            if (_db.Products.Any()) return;
        }
    }
}
