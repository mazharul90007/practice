using System;
namespace InheritanceExample
{
    class Animal
    {
        public string Name {get; set;}
        public Animal(string name)
        {
            this.Name = name;
        }
        
    }

    class Dog : Animal
    {

        public Dog(string name) : base(name)
        {
            
        }

        public void DogName()
        {
            Console.WriteLine("Dog's name is: " + Name);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dog dog = new Dog("Buddy");
            dog.DogName();
        }
    }

}