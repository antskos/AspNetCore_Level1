using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Models.Employee> Employees { get; } = new List<Models.Employee>
        {
            new Models.Employee
            {
                Id = 1,
                Name = "Ivan",
                Surname = "Ivanov",
                Patronymic = "Ivanovich",
                Age = 26
            },
            new Models.Employee
            {
                Id = 2,
                Name = "Aleksei",
                Surname = "Alekseev",
                Patronymic = "Alekseevich",
                Age = 35
            },
            new Models.Employee
            {
                Id = 3,
                Name = "Konstantin",
                Surname = "Konstantinov",
                Patronymic = "Konstantinovich",
                Age = 41
            },
        };
    }
}
