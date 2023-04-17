using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEmp.Context;
using MyEmp.Models;
using MyEmp.ViewModel;
using NuGet.Protocol;
using System;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyEmp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoreEmpController : ControllerBase
    {
        private readonly MyCoreEmpContext _contextAccessor;

        public CoreEmpController(MyCoreEmpContext contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HttpGet("techenable")]

        public async Task<IActionResult> GetAllonlyTechDetails()
        {
            try
            {
                var employeeDetails = (from emp in _contextAccessor.CoreEmployees
                                       join state in _contextAccessor.CoreStates on emp.StateId equals state.Id
                                       join empTech in _contextAccessor.CoreEmpTeches on emp.Id equals empTech.Employee_Id
                                       join tech in _contextAccessor.CoreTechNames on empTech.Tech_ID equals tech.Id
                                       where empTech.Is_active == true
                                       group tech by new { emp.Id, emp.Name, emp.DOJ, emp.Gender, state.StateName } into g
                                       select new
                                       {
                                           g.Key.Id,
                                           Name = g.Key.Name,
                                           Doj = g.Key.DOJ,
                                           StateName = g.Key.StateName,
                                           gender = g.Key.Gender,
                                           TechNames = g.Select(t => t.Tech_Name).ToArray()
                                       }).ToList();

                if (employeeDetails.Count == 0)
                {
                    return NotFound("No Data Found");
                }
                else
                {
                    return Ok(employeeDetails);
                }
             
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetbyId(int Id)
        {
            try
            {


                Console.WriteLine(Id);
                var employeeDetails = (from emp in _contextAccessor.CoreEmployees
                                       join state in _contextAccessor.CoreStates on emp.StateId equals state.Id
                                       join empTech in _contextAccessor.CoreEmpTeches on emp.Id equals empTech.Employee_Id into empTechGroup
                                       from empTech in empTechGroup.DefaultIfEmpty()
                                       join tech in _contextAccessor.CoreTechNames on empTech.Tech_ID equals tech.Id into techGroup
                                       from tech in techGroup.DefaultIfEmpty()
                                       where emp.Id == Id
                                       group tech by new { emp.Id, emp.Name, emp.DOJ, emp.Gender, state.StateName } into g
                                       select new
                                       {
                                           g.Key.Id,
                                           Name = g.Key.Name,
                                           Doj = g.Key.DOJ,
                                           StateName = g.Key.StateName,
                                           gender = g.Key.Gender,
                                           TechNames = g.Where(t => t != null).Select(t => t.Tech_Name).ToArray(),
                                       }).ToList();

                if (employeeDetails.Count == 0)
                {
                    return NotFound($"Employee id {Id} not Found");
                }
                else
                {
                    return Ok(employeeDetails);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employeeDetails = (from emp in _contextAccessor.CoreEmployees
                                       join state in _contextAccessor.CoreStates on emp.StateId equals state.Id
                                       join empTech in _contextAccessor.CoreEmpTeches on emp.Id equals empTech.Employee_Id into empTechGroup
                                       from empTech in empTechGroup.DefaultIfEmpty()
                                       join tech in _contextAccessor.CoreTechNames on empTech.Tech_ID equals tech.Id into techGroup
                                       from tech in techGroup.DefaultIfEmpty()                         
                                       group tech by new { emp.Id, emp.Name, emp.DOJ, emp.Gender, state.StateName } into g
                                       select new
                                       {
                                           g.Key.Id,
                                           Name = g.Key.Name,
                                           Doj = g.Key.DOJ,
                                           StateName = g.Key.StateName,
                                           gender = g.Key.Gender,
                                           TechNames = g.Where(t => t != null).Select(t => t.Tech_Name).ToArray(),
                                       }).ToList();

                if (employeeDetails.Count == 0)
                {
                    return NotFound("No Data Found");
                }
                else
                {
                    return Ok(employeeDetails);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("techenable/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var res=new object();
                var employeeDetails = (from emp in _contextAccessor.CoreEmployees
                                                         join state in _contextAccessor.CoreStates on emp.StateId equals state.Id
                                                        join empTech in _contextAccessor.CoreEmpTeches on emp.Id equals empTech.Employee_Id
                                                        join tech in _contextAccessor.CoreTechNames on empTech.Tech_ID equals tech.Id
                                                        where emp.Id == id && empTech.Employee_Id == id && empTech.Is_active == true
                                                        group tech by new { emp.Id,emp.Name,emp.DOJ,emp.Gender,state.StateName } into g
                                       select new{
                                           g.Key.Id,
                                           Name=g.Key.Name,
                                           Doj=g.Key.DOJ,
                                           StateName = g.Key.StateName,
                                           gender = g.Key.Gender,
                                           TechNames = g.Select(t => t.Tech_Name).ToArray()
                                       }).ToList();


                if(employeeDetails!=null && employeeDetails.Count>0)
                {
                    return Ok(employeeDetails);

                }
                else
                {
                    return NotFound($"Employee {id} is Not Found");
                }

            }
            catch (Exception ex)
            {
                
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }


        [HttpPut]

        public async Task<IActionResult> UpdateCoreEmplyee([FromBody] CrudPostViewModel addEmp)
        {
            using (var transaction = _contextAccessor.Database.BeginTransaction())
            {
                string Res = "";
                string[] addedTechs = new string[] { };
                try
                {
                    var employee = _contextAccessor.CoreEmployees.FirstOrDefault(e => e.Id == addEmp.Id);
                    if (employee == null)
                    {
                        string ErrorMsg = $"Sorry unable to find Employee Id {addEmp.Id}";
                        Res = ErrorMsg;
                        return NotFound(ErrorMsg);
                        throw new InvalidOperationException(ErrorMsg);
                    }

                    var state = _contextAccessor.CoreStates.FirstOrDefault(x => x.StateName == addEmp.StateName);
                    employee.Name = addEmp.Name;
                    employee.DOJ = addEmp.DOJ;
                    employee.Gender = addEmp.gender;
                    if (state != null)
                    {
                        employee.StateId = state.Id;
                    }
                    else
                    {
                        Res = $"Invailed State entered {addEmp.StateName}";
                        throw new InvalidOperationException("Wrong State name Provied");

                        
                    }
                  //  _contextAccessor.Add(Employee);
                    var saved = _contextAccessor.SaveChanges();

                    //Console.WriteLine(saved);
                    //var lastEmpId = _contextAccessor.CoreEmployees.OrderByDescending(e => e.Id)
                    //     .Select(e => e.Id)
                    //       .FirstOrDefault();
                    _contextAccessor.CoreEmpTeches.Where(e => e.Employee_Id == addEmp.Id).ToList().ForEach(e=> _contextAccessor.CoreEmpTeches.Remove(e));
                    foreach (var techname in addEmp.TechNames)
                    {
                        var techId = _contextAccessor.CoreTechNames.FirstOrDefault(x => x.Tech_Name == techname);
                        if (techId != null)
                        {
                            var techObj = new CoreEmpTech { Employee_Id = employee.Id, Tech_ID = techId.Id, Is_active = true };

                            _contextAccessor.Add(techObj);
                            _contextAccessor.SaveChanges();
                            Array.Resize(ref addedTechs, addedTechs.Length + 1);
                            addedTechs[addedTechs.Length - 1] = techname;
                            Res = $"Employee {addEmp.Id} Details Updated";
                        }
                        else
                        {
                            var remainingData = addEmp.TechNames.Except(addedTechs);
                            string remainingDataString = string.Join(", ", remainingData);
                            if (techId == null )
                            {
                                Res = $"Your entered wrong tech details {remainingDataString} ";
                            }
                            else if (state == null)
                            {
                                Res = $"Your entered  wrong state {addEmp.StateName}";
                            }
                            else if (techId == null)
                            {
                                Res = $"Your entered wrong tech details {remainingDataString} ";

                            }
                            throw new InvalidOperationException($"Wrong tech details {remainingDataString}");
                        //   transaction.Rollback();

                        }

                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        Message = Res,
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return BadRequest(new
                    {
                        Message = Res,
                    });

                }
            }
        }


        [HttpPost]

        public async Task<IActionResult> AddCoreEmplyee(CrudPostViewModel addEmp)
        {
            using (var transaction = _contextAccessor.Database.BeginTransaction())
            {
                string Res = "";
                string[] addedTechs = new string[] { };
                try
                {
                    var employee = new CoreEmployee();

                    var state = _contextAccessor.CoreStates.FirstOrDefault(x => x.StateName == addEmp.StateName);
                    employee.Name = addEmp.Name;
                    employee.DOJ = addEmp.DOJ;
                    employee.Gender = addEmp.gender;
                    if (state != null)
                    {
                        employee.StateId = state.Id;
                    }
                    else
                    {
                        Res = $"Invailed State entered {addEmp.StateName}";
                        throw new InvalidOperationException("Wrong State name Provied");

                        ;
                    }
                    _contextAccessor.Add(employee);
                    _contextAccessor.SaveChanges();


                  
                    //Console.WriteLine(saved);
                   // var lastEmpId= _contextAccessor.CoreEmployees.OrderByDescending(e => e.Id)
                   //      .Select(e => e.Id)
                   //        .FirstOrDefault();

                    foreach (var techname in addEmp.TechNames)
                    {
                        var techId= _contextAccessor.CoreTechNames.FirstOrDefault(x => x.Tech_Name == techname);
                       
                        if (techId != null)
                        {
                            var techObj = new CoreEmpTech { Employee_Id = employee.Id, Tech_ID = techId.Id, Is_active = true };
                            _contextAccessor.Add(techObj);
                            _contextAccessor.SaveChanges();
                            Array.Resize(ref addedTechs, addedTechs.Length + 1);
                            addedTechs[addedTechs.Length - 1] = techname;
                            Res = "Details Added";
                        }
                        else
                        {
                            var remainingData = addEmp.TechNames.Except(addedTechs);
                            string remainingDataString = string.Join(", ", remainingData);
                            if (techId == null && state == null)
                            {
                                Res = $"Your entered wrong tech details {remainingDataString} and wrong state {addEmp.StateName}";
                            }else if(state == null)
                            {
                                Res = $"Your entered  wrong state {addEmp.StateName}";
                            }
                            else if (techId == null)
                            {
                                Res = $"Your entered wrong tech details {remainingDataString} and wrong state {addEmp.StateName}";

                            }
                            throw new InvalidOperationException($"Wrong tech details {remainingDataString}");

                        }

                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        Message = Res,
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return BadRequest(new
                    {
                        Message = Res,
                    });
                    
                }
            }
        }
    }
}
