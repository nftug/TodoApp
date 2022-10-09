using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class VirtualizationTestPage : ComponentBase
{
    public List<Person> People = Enumerable
        .Range(1, 100)
        .Select(x => new Person(x.ToString(), x % 60))
        .ToList();

    public class Person
    {
        public string Name { get; }
        public int Age { get; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
