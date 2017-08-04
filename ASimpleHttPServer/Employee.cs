namespace ASimpleHttPServer
{
    internal class Employee
    {
        public Employee(short id, string name, string gender, string position, int age)
        {
            Id = id;
            Name = name;
            Position = position;
            Age = age;
            Gender = gender;
        }

        public short Id { get; }

        public string Name { get; }

        public string Position { get; }

        public int Age { get; }

        public string Gender { get; }

    }
}