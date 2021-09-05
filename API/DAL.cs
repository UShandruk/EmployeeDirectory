using CommonClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public static class DAL
    {
        static string connectionString = @"Data source = (LocalDB)\MSSQLLocalDB;Initial Catalog = DbEmployees; Integrated Security = True";

        /// <summary>
        /// Получить список сотрудников
        /// </summary>
        /// <returns></returns>
        public static List<Employee> GetEmployees()
        {
            List<Employee> listEmployee = new List<Employee>();

            // название хранимой процедуры
            string sqlExpression = "sp_SelectAllEmployees";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string surname = reader.GetString(1);
                        string name = reader.GetString(2);                        
                        string patronymic = reader.GetString(3);
                        DateTime birthDate = DateTime.Parse(reader.GetString(4));
                        List<string> phoneNumbers = reader.GetString(5).Split(';').ToList();

                        Employee employee = new Employee(id, surname, name, patronymic, birthDate, phoneNumbers);
                        listEmployee.Add(employee);
                    }
                }
                reader.Close();
            }

            return listEmployee;
        }

        /// <summary>
        /// Получить запись о сотруднике
        /// </summary>
        public static Employee LoadEmployee(int id)
        {
            // название хранимой процедуры
            string sqlExpression = "sp_SelectEmployee";

            Employee employee = new Employee();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = id
                };
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult);
                while (reader.Read())
                {
                    employee.Id = reader.GetInt32(0);                    
                    employee.Surname = reader.GetString(1);
                    employee.Name = reader.GetString(2);
                    employee.Patronymic = reader.GetString(3);
                    employee.BirthDate = DateTime.Parse(reader.GetString(4));
                    employee.PhoneNumbers = reader.GetString(5).Split(';').ToList();
                }
            }
            return employee;
        }

        /// <summary>
        /// Добавить запись о сотруднике
        /// </summary>
        public static void AddEmployee(Employee newEmployee)
        {
            // название хранимой процедуры
            string sqlExpression = "sp_InsertEmployee";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter paramSurname = new SqlParameter
                {
                    ParameterName = "@Surname",
                    Value = newEmployee.Surname
                };

                SqlParameter paramName = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = newEmployee.Name
                };

                SqlParameter paramPatronymic = new SqlParameter
                {
                    ParameterName = "@Patronymic",
                    Value = newEmployee.Patronymic
                };

                SqlParameter paramBirthDate = new SqlParameter
                {
                    ParameterName = "@BirthDate",
                    Value = newEmployee.BirthDate
                };

                SqlParameter paramPersonnelNumber = new SqlParameter
                {
                    ParameterName = "@PhoneNumbers",
                    Value = newEmployee.PhoneNumbers
                };

                command.Parameters.Add(paramSurname);
                command.Parameters.Add(paramName);
                command.Parameters.Add(paramPatronymic);
                command.Parameters.Add(paramBirthDate);
                command.Parameters.Add(paramPersonnelNumber);

                var result = command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Удалить запись о сотруднике
        /// </summary>
        public static void DeleteEmployee(int personnelNumber)
        {
            // название хранимой процедуры
            string sqlExpression = "sp_DeleteEmployee";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter paramId = new SqlParameter
                {
                    ParameterName = "@PersonnelNumber",
                    Value = personnelNumber
                };

                command.Parameters.Add(paramId);

                var result = command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Обновить запись о сотруднике
        /// </summary>
        //[HttpPost]
        public static void UpdateEmployee(Employee employee)
        {
            // название хранимой процедуры
            string sqlExpression = "sp_UpdateEmployee";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter paramSurname = new SqlParameter
                {
                    ParameterName = "@Surname",
                    Value = employee.Surname
                };

                SqlParameter paramName = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = employee.Name
                };

                SqlParameter paramPatronymic = new SqlParameter
                {
                    ParameterName = "@Patronymic",
                    Value = employee.Patronymic
                };

                SqlParameter paramBirthDate = new SqlParameter
                {
                    ParameterName = "@BirthDate",
                    Value = employee.BirthDate
                };

                SqlParameter paramPersonnelNumber = new SqlParameter
                {
                    ParameterName = "@PhoneNumbers",
                    Value = employee.PhoneNumbers
                };

                command.Parameters.Add(paramSurname);
                command.Parameters.Add(paramName);
                command.Parameters.Add(paramPatronymic);
                command.Parameters.Add(paramBirthDate);
                command.Parameters.Add(paramPersonnelNumber);

                var result = command.ExecuteNonQuery();
            }
            return;
        }
    }
}
