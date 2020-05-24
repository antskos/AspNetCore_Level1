using System;
namespace WebStore.Models
{
    public class Employee
    {
        public Employee()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }

    }
}
