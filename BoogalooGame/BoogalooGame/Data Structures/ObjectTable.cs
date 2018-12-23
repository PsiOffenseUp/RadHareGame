using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoogalooGame
{
    /// <summary>
    /// Stores a maximum of 512 existing objects in an array. Will be used to keep track of all of the instances of enemies.
    /// </summary>
    class ObjectTable
    {
        const int MAX_LENGTH = 512; //Maximum length of the GameObject array
        public GameObject[] object_array = new GameObject[MAX_LENGTH]; //Array of instances currently in use
        public bool[] filled_array = new bool[MAX_LENGTH]; //Tells wheter a position is filled or not. Basically, does the object exist or not?
        private int count;
       
        ObjectTable()
        {
            for (int i = 0; i < MAX_LENGTH; i++)
            {
                filled_array[i] = false; //Initialize the array to False to signify no added object 
            }

            this.count = 0;
        }

        public int Count
        {
            get { return count; }

            set { this.count = value; }
        }

        public int insert(ref GameObject add) //Puts add into the object_array, and returns the objects id after creation
        {
            for (int i = 0; i < MAX_LENGTH; i++)
            {
                if (filled_array[i] == false)
                {
                    object_array[i] = add; //Put the object into its space in memory
                    filled_array[i] = true; //Tells whether each spot is taken or not
                    this.Count++; //Increment count for new object
                    return i;
                }
            }
            //If all of the spots are added, return a -1 as an error. If this happens, something has gone horribly wrong with items on screen.
            return -1;
        }
        
        public void remove(ref int id) //Removes an object at the index of the id passed in as an argument.
        {
            if (id > MAX_LENGTH || id < 0) //Do not remove any object if the user accidentally passed in a value greater than the id
                return;
            filled_array[id] = false;
            object_array[id] = null; //DEBUG Hopefully this is a thing in C#. Clear it from the array since C# does Garbage Collection
            
            this.Count--; //Decerement the count
        }
    }
}
