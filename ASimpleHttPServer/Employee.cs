﻿namespace ASimpleHttpServer
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

        public short Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

    }
}