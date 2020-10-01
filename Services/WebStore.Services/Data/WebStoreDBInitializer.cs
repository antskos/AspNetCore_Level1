using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{


    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public WebStoreDBInitializer(WebStoreDB db, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public void Initialize()
        {
            var db = _db.Database;

            // проверка создаётся ли новая БД
            //if (db.EnsureDeleted())
            //    if (!db.EnsureCreated())
            //        throw new InvalidOperationException("Ошибка при создании базы данных!");
            db.Migrate();

            InitializeProducts();
            InitializeEmployees();
            InitializeIdentityAsync().Wait();

        } //Initialize() method

        private void InitializeProducts()
        {
            var db = _db.Database;

            // если есть товары в базе, то она проинициализирована
            if (_db.Products.Any()) return;

            using (db.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSection] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSection] OFF");

                db.CommitTransaction();
            }

            using (var transaction = db.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrand] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrand] OFF");

                transaction.Commit();
            }

            using (var transaction = db.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                transaction.Commit();
            }
        }  //InitializeProducts() method

        private void InitializeEmployees()
        {
            if (_db.Employees.Any()) return;

            using (var transaction = _db.Database.BeginTransaction())
            {
                var employees = TestData.Employees.ToList();

                employees.ForEach(emp => emp.Id = 0);
                _db.Employees.AddRange(employees);

                _db.SaveChanges();
                transaction.Commit();
            }
        }   //InitializeEmployees() method

        private async Task InitializeIdentityAsync()
        {
            // проверка определены ли в БД "обязательные" роли и если нет создаём их
            if (!await _roleManager.RoleExistsAsync(Role.Administrator))
                await _roleManager.CreateAsync(new Role { Name = Role.Administrator });

            if (!await _roleManager.RoleExistsAsync(Role.User))
                await _roleManager.CreateAsync(new Role { Name = Role.User });

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User() { UserName = User.Administrator };

                var create_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (create_result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, Role.Administrator);
                else
                {
                    var errors = create_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при создании пользователя с правами администратора: " +
                        $"{string.Join(",", errors)}");
                }
            }


        }   //InitializePdentity() method
    }
}
