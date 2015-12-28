using System;
using System.Collections.Generic;
using System.Web.Http;
using Exercise_ASP.NET_Web_API.Models;

namespace Exercise_ASP.NET_Web_API.Controllers
{
    public class StudentController : ApiController
    {
        public IHttpActionResult GetStudent()
        {
            var students = new List<Student>()
            {
                new Student()
                {
                    Name = "Konstantin Kirchev",
                    BirthDay = new DateTime(1982, 3, 21),
                    Username = "BlueEagle"
                },
                new Student()
                {
                    Name = "Dodo Didov",
                    BirthDay = new DateTime(1947, 6, 3),
                    Username = "Don Didon"
                },
                new Student()
                {
                    Name = "Ralitsa Raicheva",
                    BirthDay = new DateTime(1975, 8, 10),
                    Username = "KRalitsa"
                },

            };
            return this.Ok(students);
        }
    }
}
