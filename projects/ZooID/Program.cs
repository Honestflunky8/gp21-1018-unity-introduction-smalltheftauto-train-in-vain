﻿using System;

namespace ZooID
{
    class Program
    {
        static void Main()
        {
            {
                Zoo<Fish> fishZoo = new Zoo<Fish>();
                fishZoo.AddAnimal(new Fish()); // OKAY
                fishZoo.AddAnimal(new Clownfish()); // OKAY
            }
            {
                Zoo<Animal> animalZoo = new Zoo<Animal>();
                animalZoo.AddAnimal(new Fish()); // OKAY
                animalZoo.AddAnimal(new Clownfish()); // OKAY
                animalZoo.AddAnimal(new Lion()); // OKAY
                animalZoo.AddAnimal(new Donkey()); // OKAY
                
                Zoo<Lion> lionZoo = new Zoo<Lion>();
                animalZoo.AddAnimal(new Lion()); // OKAY
                animalZoo.AddAnimal(new Lion()); // OKAY
                animalZoo.AddAnimal(new Lion()); // OKAY
            }
            {
                Zoo<Salmon> salmonZoo = new Zoo<Salmon>();
                salmonZoo.HasAnimal<Lion>(); // ERROR!
            }
            {
                Zoo<Student> studentZoo = new StudentZoo(); // ERROR!
            }
            {
                Zoo<Fish> fishZoo = new Zoo<Fish>();
                fishZoo.AddAnimal(new Lion()); // ERROR!
            }
            {
                Zoo<Salmon> salmonZoo = new Zoo<Salmon>();
                salmonZoo.AddAnimal(new Fish()); // ERROR!
            }
            {
                Zoo<Fish> fishZoo = new Zoo<Fish>();
                fishZoo.AddAnimal(new Salmon());
                fishZoo.AddAnimal(new Salmon());
                Console.WriteLine("This should be False: "+fishZoo.HasAnimal<Clownfish>());
            }
            
        }
    }
    class Animal 
    {
            
    }

    class Mammal : Animal
    {
        
    }

    class Bear : Mammal
    {
        
    }

    class Donkey : Mammal
    {
        
    }

    class Lion : Mammal
    {
        
    }

    class Fish : Animal
    {
        
    }

    class Salmon : Fish
    {
        
    }

    class Clownfish : Fish
    {
        
    }

    class Student 
    {
        
    }

    class Zoo <TAnimals>  where TAnimals : Animal
    {
        private TAnimals[] Animals = new TAnimals[0];

        public void AddAnimal(TAnimals animal) 
        { 
            Array.Resize(ref Animals, Animals.Length+1);
            Animals[Animals.Length - 1] = animal;
        }

        public void HasAnimal<TSpecies>() where TSpecies : TAnimals
        {
            throw new NotImplementedException();
        }
        
    }
}
