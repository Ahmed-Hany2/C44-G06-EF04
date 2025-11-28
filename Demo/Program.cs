using Assignment;

namespace Demo
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
    }

    public class Instructor : Person
    {
        public string Specialization { get; set; }
    }

    public class Trainee : Person
    {
        public int Grade { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {

            // group join example
            using var context = new ITIContext();
            
                var result =
                    context.Departments
                    .GroupJoin(
                        context.Instructors,
                        dept => dept.ID,
                        inst => inst.ID,
                        (dept, instGroup) => new
                        {
                            Department = dept.Name,
                            Instructors = instGroup.Select(i => i.Name)
                        });

                foreach (var item in result)
                {
                    Console.WriteLine($"Department: {item.Department}");
                    foreach (var inst in item.Instructors)
                        Console.WriteLine($"  - {inst}");
                }

            var result2 =
                    context.Departments
                    .GroupJoin(
                        context.Courses,
                        d => d.ID,
                        c => c.ID,
                        (d, cs) => new
                        {
                            Department = d.Name,
                            Courses = cs.Select(c => c.Name)
                        });

                foreach (var item in result2)
                {
                    Console.WriteLine($"Department: {item.Department}");
                    foreach (var c in item.Department)
                        Console.WriteLine($"   - {c}");
                }

            // Left Join example
            var result3 =
               from d in context.Departments
               join i in context.Instructors
               on d.ID equals i.ID
               into gi
               from inst in gi.DefaultIfEmpty()
               select new
               {
                   Department = d.Name,
                   Instructor = inst != null ? inst.Name : "No Instructor"
               };

                    foreach (var r in result3)
                    {
                        Console.WriteLine($"{r.Department}");
                    }

            //Inheritance mapping
            context.Add(new Instructor
            {
                Name = "Ahmed",
                 Specialization = "C#"
            });

            context.Add(new Trainee
            {
                Name = "Mohamed",
                Grade = 95
            });

            context.SaveChanges();

            var people = context.Set<Person>().ToList();

            foreach (var p in people)
            {
                Console.WriteLine($"{p.PersonId} - {p.Name} - {p.GetType().Name}");
            }


        }

}


    }
