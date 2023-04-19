using Microsoft.AspNetCore.Mvc;
using MyEmp.Context;
using MyEmp.Models;
using MyEmp.ViewModel;

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

        public async Task<IActionResult> GetAllOnlyTechDetails()
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
                    return NotFound("No Data Found");
                else
                    return Ok(employeeDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
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
                    return NotFound($"Employee id {Id} not Found");
                else
                    return Ok(employeeDetails);

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
                    return NotFound("No Data Found");
                else
                    return Ok(employeeDetails);
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
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                var employeeDetails = (from emp in _contextAccessor.CoreEmployees
                                       join state in _contextAccessor.CoreStates on emp.StateId equals state.Id
                                       join empTech in _contextAccessor.CoreEmpTeches on emp.Id equals empTech.Employee_Id
                                       join tech in _contextAccessor.CoreTechNames on empTech.Tech_ID equals tech.Id
                                       where emp.Id == id && empTech.Employee_Id == id && empTech.Is_active == true
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

                if (employeeDetails != null && employeeDetails.Count > 0)
                    return Ok(employeeDetails);
                else
                    return NotFound($"Employee {id} is Not Found");
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

        public async Task<IActionResult> UpdateCoreEmplyee(CrudPostViewModel addEmp)
        {
            using (var transaction = _contextAccessor.Database.BeginTransaction())
            {
                string responseMessage = "";
                List<string> invalidTechs = new List<string>();
                try
                {
                    bool updateSuccessful = true;
                    var employee = _contextAccessor.CoreEmployees.FirstOrDefault(e => e.Id == addEmp.Id);
                    if (employee == null)
                    {
                        string errorMsg = $"Sorry unable to find Employee Id {addEmp.Id}";
                        updateSuccessful = false;
                        responseMessage = errorMsg;
                        throw new InvalidOperationException(errorMsg);
                    }
                    var state = _contextAccessor.CoreStates.FirstOrDefault(x => x.StateName == addEmp.StateName);
                    if (state != null)
                        employee.StateId = state.Id;
                    else
                    {
                        updateSuccessful = false;
                        responseMessage = $"Invailed State entered {addEmp.StateName}";
                        throw new InvalidOperationException($"Invailed State entered {addEmp.StateName}");
                    }
                    if (updateSuccessful)
                    {
                        employee.Name = addEmp.Name;
                        employee.Gender = addEmp.gender;
                        employee.DOJ = addEmp.DOJ;
                        _contextAccessor.SaveChanges();
                        _contextAccessor.CoreEmpTeches.Where(e => e.Employee_Id == addEmp.Id).ToList().ForEach(e => _contextAccessor.CoreEmpTeches.Remove(e));
                    }
                    if (addEmp.TechNames.Length != 0)
                    {
                        foreach (var techName in addEmp.TechNames)
                        {
                            var techId = _contextAccessor.CoreTechNames.FirstOrDefault(x => x.Tech_Name == techName);
                            if (techId != null && updateSuccessful)
                            {
                                var techObj = new CoreEmpTech { Employee_Id = employee.Id, Tech_ID = techId.Id, Is_active = true };
                                _contextAccessor.Add(techObj);
                                _contextAccessor.SaveChanges();
                                invalidTechs.Add(techName);
                                responseMessage = $"Employee {addEmp.Id} Details Updated";
                            }
                            else
                            {
                                var remainingTech = addEmp.TechNames.Except(invalidTechs);
                                string remainingTechnames = string.Join(", ", remainingTech);
                                if (techId == null)
                                    responseMessage = $"Your entered wrong tech details {remainingTechnames} ";
                                throw new InvalidOperationException($"Wrong tech details {remainingTechnames}");
                            }

                        }
                    }
                    else
                    {
                        //  _contextAccessor.CoreEmpTeches.Where(e => e.Employee_Id == addEmp.Id).ToList().ForEach(e => _contextAccessor.CoreEmpTeches.Remove(e));
                        _contextAccessor.SaveChanges();
                        responseMessage = $"Employee {addEmp.Id} Details Updated";
                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        Message = responseMessage,
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new
                    {
                        Message = responseMessage,
                    });

                }
            }
        }

        [HttpPost]

        public async Task<IActionResult> AddCoreEmplyee(CrudPostViewModel addEmp)
        {
            using (var transaction = _contextAccessor.Database.BeginTransaction())
            {
                string responseMessage = "";
                List<string> invalidTechs = new List<string>();
                try
                {
                    var employee = new CoreEmployee();
                    bool addSuccessfully = true;
                    
                    var state = _contextAccessor.CoreStates.FirstOrDefault(x => x.StateName == addEmp.StateName);
                    if (state != null) employee.StateId = state.Id;
                    else
                    {
                        addSuccessfully = false;
                        responseMessage = $"Invailed State entered {addEmp.StateName} ";
                        throw new InvalidOperationException($"Invailed State entered {addEmp.StateName} ");
                    }
                    if (addSuccessfully)
                    {
                        employee.Name = addEmp.Name;
                        employee.DOJ = addEmp.DOJ;
                        employee.Gender = addEmp.gender;
                        _contextAccessor.Add(employee);
                        _contextAccessor.SaveChanges();
                    }
                    if (addEmp.TechNames.Length > 0)
                    {
                        foreach (var techName in addEmp.TechNames)
                        {
                            var techId = _contextAccessor.CoreTechNames.FirstOrDefault(x => x.Tech_Name == techName);
                            if (techId != null && addSuccessfully)
                            {
                                var techObj = new CoreEmpTech { Employee_Id = employee.Id, Tech_ID = techId.Id, Is_active = true };
                                _contextAccessor.Add(techObj);
                                _contextAccessor.SaveChanges();
                                invalidTechs.Add(techName);
                                responseMessage = "Details Added";
                            }
                            else
                            {
                                var remainingTech = addEmp.TechNames.Except(invalidTechs);
                                string remainingtechnameString = string.Join(", ", remainingTech);
                                if (techId == null)
                                {
                                    responseMessage = $"Your entered wrong tech details {remainingtechnameString}";
                                }
                                throw new InvalidOperationException($"Wrong tech details {remainingtechnameString}");
                            }
                        }
                    }
                    else
                    {
                        responseMessage = "Details Added.";
                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        Message = responseMessage,
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new
                    {
                        Message = responseMessage,
                    });
                }
            }
        }
    }
}
