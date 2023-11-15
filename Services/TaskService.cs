using Tasks.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
 using Tasks.Models;

namespace Tasks.Services
{


    public class TaskService:ITaskService
     {      
        List<Task> ttasks {get;}
        private IWebHostEnvironment  webHost;
        private string filePath;

        public TaskService(IWebHostEnvironment webHost)
        {
          
            this.webHost = webHost;

            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                ttasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
       }
       
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(ttasks));
        }

      public List<Task> GetAll(long userId)
       {
          return ttasks.Where(p => p.AgentId == userId).ToList();
       }

       public Task Get(long userId,int id)   
        {
            return ttasks.FirstOrDefault(p => p.AgentId == userId && p.id == id);
        }



        public void Add(long userId, Task tasks)
        {
            tasks.id = Count(userId) + 1;
            tasks.AgentId = userId;
            ttasks.Add(tasks);
            saveToFile();
        }

        public void Delete(long userId, int id)
        {
            var tasks = Get(userId, id);
            if (tasks is null)
                return;

            ttasks.Remove(tasks);
            saveToFile();
        }

        public void Update(long userId, Task task)
        {
            var index = ttasks.FindIndex(p => p.AgentId == userId &&  p.id == task.id);
            if (index == -1)
                return;

            ttasks[index] = task;
            saveToFile();
        }

        public int Count(long userId) 
        { 
            return GetAll(userId).Count();
        }
    }
    
}
