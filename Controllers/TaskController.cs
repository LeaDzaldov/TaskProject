using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Services;
using Tasks.Interfaces;
using Tasks.Models;
using Microsoft.AspNetCore.Authorization;  

namespace Tasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "Agent")]
    public class TaskController : ControllerBase
    {
       
        private long userId;
        private ITaskService TaskService;
        public TaskController(ITaskService taskService){
            this.TaskService=taskService;
             this.userId = long.Parse(User.FindFirst("UserId")?.Value ?? "");
        }

       [HttpGet("{id}")]
      public ActionResult<Task> Get(int id)
        {
            var taskks=TaskService.Get(userId,id);
            if (taskks == null)
                return NotFound();

            return taskks;
      
        }

       [HttpGet]
       public ActionResult<List<Task>> GetAll() 
       {   
          return  TaskService.GetAll(userId);
    
       }   

       [HttpPut ("{id}")]
      public IActionResult Put(int id, Task newTask)
    {
        if (id != newTask.id)
                return BadRequest();

            var existingTask = TaskService.Get(userId, id);
            if (existingTask is null)
                return  NotFound();
                
             TaskService.Update(userId, newTask);

            return NoContent();
      
    } 
    [HttpPost]
    public IActionResult Post(Task newTask)
    {
        TaskService.Add(userId,newTask);
           return CreatedAtAction(nameof(Post), new {id=newTask.id}, newTask);
        
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {  var task = TaskService.Get(userId, id);
            if (task is null)
                return  NotFound();

         TaskService .Delete(userId, id);

            return Content(TaskService.Count(userId).ToString());
    }       
     }

}
