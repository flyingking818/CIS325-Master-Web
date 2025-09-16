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
        //Let's use the Contains() method to search for a substring
        //which is equivalent to SQL's LIKE '%query%'
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
        public void OnGet(string? query)  //This MUST match the name of the query string parameter
        {
            //This is where we need to do the heavy-lifting! Logic/algorithm, etc.
            Query = query;
            if (string.IsNullOrEmpty(query)){
                //Show all students
                Results = Students.ToList();
            }
            else
            {
                //Refresh - SQL = Structured Query Language
                //LINQ = Language Integrated Query, SQL like syntax in C#, which can be used 
                //to query in-memory collections (e.g., arrays, lists, collections,
                //databases, xml, JSON, etc.)
                Results = Students.Where(
                        s=>s.Id.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        s.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                        )
                        .ToList();

            }

        }
    }
}
