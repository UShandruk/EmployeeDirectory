using System;

namespace CommonClasses
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="birthDate">Дата рождения</param>
        public Employee(string surname, string name, string patronymic, DateTime birthDate)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            BirthDate = birthDate;
        }
    }
}
