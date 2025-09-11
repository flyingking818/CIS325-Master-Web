using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIS325_Master_Web.Pages.Demos.Module2
{
    public class StudentsModel : PageModel
    {
        public string Query { get; set; }

        public List<Student> Results { get; set; } = new();

        //Create a Student Class
        public class Student
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public Student(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        //Create an in-memory array for the student list (hard-coded)
        private Student[] Students =
        {
            new("S101", "Jeremy Wang"),
            new("S102", "Jacob Corder"),
            new("S103", "John Dowd"),
            new("S104", "Jay Hawkins"),
            new("S105", "Molly Sloan"),
            new("S106", "Griffin Heinberg")

        };


        //Handler method for HTTP GET requests
        //? is an optional, because we don't know whether there's a query!
        public void OnGet(string? query)
        {
            //This is where we need to do the heavy-lifting! Logic/algorithm, etc.
            Query = query;
        }
    }
}
