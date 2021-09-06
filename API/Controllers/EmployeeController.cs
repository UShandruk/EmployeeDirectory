using CommonClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
//using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получить полный список сотрудников
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public List<Employee> GetEmployees()
        {        
            List<Employee> listEmployees = DAL.GetEmployees();
            return listEmployees;
        }

        /// <summary>
        /// Получить сотрудника по его id
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public Employee GetEmployee(int id)
        {
            Employee employee = DAL.GetEmployee(id);
            return employee;
        }
    }
}