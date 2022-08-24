using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoWebApi.Models;
using Microsoft.EntityFrameworkCore;
using DemoWebApi.ViewModel;

//Class is Created in ViewModel
namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        DB1045Context db = new DB1045Context();
        [HttpGet]
        [Route("ListDept")]
        public IActionResult GetDept() {
            //var data = db.Depts.ToList();
            var data = from obj in db.Depts select obj;
            //var data = from dept in db.Depts select new { Id=dept.Id, Name=dept.Name, Location=dept.Location};
            return Ok(data);
        }
        [HttpGet]
        [Route("ListDept/{id}")]
        public IActionResult GetDept(int? id) {
            if (id == null)
                return BadRequest("ID cannot be NULL");
            //var data = db.Depts.Where(d => d.Id == id).Select(d => new { id = d.Id, Name = d.Name, Location = d.Location }).FirstOrDefault();
            var data = (from dept in db.Depts where dept.Id == id select new { Id = dept.Id, Name = dept.Name, Location = dept.Location }).FirstOrDefault();
            //var data = db.Depts.Find(id);
            if (data == null)
                return NotFound($"Department {id} not present");
            return Ok(data);
        }
        [HttpGet]
        [Route("ListCity")]
        //....api/dept/Listcity?city=Mumbai
        public IActionResult GetCity([FromQuery] string city) {
            var data = db.Depts.Where(d => d.Location == city);
            if (data.Count() == 0) return NotFound($"City {city} is not found in Database");
            return Ok(data);
        }
        [HttpGet]
        [Route("ShowDept")]
        public IActionResult GetDeptInfo() {
            var data = db.DeptInfo_VMs.FromSqlInterpolated<DeptInfo_VM>($"DeptInfo");
            return Ok(data);
        }
        [HttpPost]
        [Route("AddDept")]
        public IActionResult PostDept(Dept dept) {
            if (ModelState.IsValid) {
                try
                {
                    //db.Depts.Add(dept);
                    //db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"deptadd {dept.Id},{dept.Name},{dept.Location}");       //For calling StoredProc deptadd from database
                }
                catch(Exception ex) {
                    return BadRequest(ex.InnerException.Message);
                }
            }
            return Created("Record Successfully Added",dept);
        }
        [HttpPut]                                                                                               //For Editing the record
        [Route("EditDept/{id}")]
        public IActionResult PutDept(int id, Dept dept) {
            if (ModelState.IsValid) {
                Dept obj = db.Depts.Find(id);
                obj.Name = dept.Name;
                obj.Location = dept.Location;
                db.SaveChanges();
                return Ok();
            }
            return BadRequest("Unable to Edit Record..!");
        }
        [HttpDelete]
        [Route("DeleteDept/{id}")]
        public IActionResult DeleteDept(int id) {
            var data = db.Depts.Find(id);
            db.Depts.Remove(data);
            db.SaveChanges();
            return Ok();
        }
    }
}
