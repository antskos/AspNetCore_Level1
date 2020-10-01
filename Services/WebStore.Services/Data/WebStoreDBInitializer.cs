using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<WebStoreDBInitializer> _logger;

        public WebStoreDBInitializer(WebStoreDB db, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<WebStoreDBInitializer> logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        public void Initialize()
        {
            _logger.LogInformation("Инициализация БД");

            var db = _db.Database;

            // проверка создаётся ли новая БД
            //if (db.EnsureDeleted())
            //    if (!db.EnsureCreated())
            //        throw new InvalidOperationException("Ошибка при создании базы данных!");
            try 
            {
                _logger.LogInformation("Проведение миграций БД");
                db.Migrate();

                _logger.LogInformation("Инициализация каталога товаров");
                InitializeProducts();

                _logger.LogInformation("Инициализация каталога сотрудников");
                InitializeEmployees();

                _logger.LogInformation("Инициализация данных системы Identity");
                InitializeIdentityAsync().Wait();
            }
            catch(Exception e) 
            {
                _logger.LogCritical(new EventId(0), e, "Ошибка процесса инициализации БД");

                throw;
            }

            _logger.LogInformation("Инициализация БД выполнена успешно");
        } //Initialize() method

        private void InitializeProducts()
        {
            var db = _db.Database;

            // если есть товары в базе, то она проинициализирована
            if (_db.Products.Any())
            {
                _logger.LogInformation("Каталог товаров проинициализирован");

                return;
            }

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
            if (_db.Employees.Any())
            {
                _logger.LogInformation("Раздел сотрудников проинициализирован");

                return;
            }
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
            {
                _logger.LogInformation("Добавление роли {0}", Role.Administrator);

                await _roleManager.CreateAsync(new Role { Name = Role.Administrator });
            }

            if (!await _roleManager.RoleExistsAsync(Role.User))
            {
                _logger.LogInformation("Добавление роли {0}", Role.User);

                await _roleManager.CreateAsync(new Role { Name = Role.User });
            }

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User() { UserName = User.Administrator };

                var create_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (create_result.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} добавлен", User.Administrator);

                    await _userManager.AddToRoleAsync(admin, Role.Administrator);

                    _logger.LogInformation("Пользователю {0} добавлена роль {1}", User.Administrator, Role.Administrator);
                }
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
