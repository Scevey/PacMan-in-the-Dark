using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListExperimizzle
{
    // Mike Teixeira
    public class LinkedList<T>:ICollection<T>
    {
        internal class Node
        {
            //variable to store data of type T
            T data;
            internal T Data
            {
                get
                {
                    return data;
                }
                set
                {
                    data = value;
                }
            }

            //reference to the previous node in the chain
            Node prev;
            internal Node Prev
            {
                get
                {
                    return prev;
                }
                set
                {
                    prev = value;
                }
            }

            //reference to the next node in the chain
            Node next;
            internal Node Next
            {
                get
                {
                    return next;
                }
                set
                {
                    next = value;
                }
            }

            //constructs node with given data, connects both ends to itself
            internal Node(T t)
            {
                prev = this;
                data = t;
                next = this;
            }

            //constructs node with given data, connects ends to specified nodes
            //not used
            internal Node(T t, Node _prev, Node _next)
            {
                prev = _prev;
                next = _next;
                data = t;
            }
        }

        //an enumerator class for this implementation of linked lists
        public class LinkedListEnumerator : IEnumerator<T>
        {
            //the linked list
            LinkedList<T> list;

            //the current node
            Node current;

            //the number of completed steps
            int index;

            //constructor, takes a linked list
            public LinkedListEnumerator(LinkedList<T> _list)
            {
                current = _list.head.Prev;
                list = _list;
            }

            //the stepping method
            //returns false when the end of the list is reached
            public bool MoveNext()
            {
                //the step
                current = current.Next;

                //ending condition
                return (index++ == list.Count) ? false : true;
            }

            //resets enumerator to initial conditions
            public void Reset()
            {
                index = -1;
                current = list.head.Prev;
            }

            //the current element
            public T Current
            {
                get
                {
                    //returns the data in the current node
                    return current.Data;
                }
            }

            //derp
            public void Dispose()
            {
            }

            //same as the other "Current"
            //I don't know why I need this
            object IEnumerator.Current
            {
                get { return current.Data; }
            }
        }

        //the currently active node
        Node head;

        //number of elements in the list
        int count;

        //constructor
        public LinkedList()
        {
            head = null;
            count = 0;
        }

        //Adds an element with given data in front of the current head
        public void Add(T t)
        {
            //creates a head if one doesn't exist
            if (head == null)
            {
                head = new Node(t);
                count++; //count increment
                return;
            }

            //inserts node between head and the element after the head if a head does exist
            //when only one element is present this will essentially place the new node between the head and itself
            Node newNode = new Node(t);
            newNode.Next = head.Next;
            newNode.Prev = head;
            head.Next.Prev = newNode;
            head.Next = newNode;

            //advances the head
            Step();

            //count increment
            count++;
        }

        //removes the current element and moves the head backwards
        public void Remove()
        {
            //count decrement
            count--;

            //links the element after the head to the element behind the head
            head.Next.Prev = head.Prev;

            //links the element behind the head to the element after the head
            head.Prev.Next = head.Next;

            //moves the head backwards
            StepBack();

            //the old head is now out of the chain
        }

        //empties the list
        public void Clear()
        {
            //by setting the head to null, initial conditions are recreated
            //all the old nodes are now inaccessible and will be picked up by garbage collection eventually
            head = null;
        }

        //returns true if the list contains a given element
        public bool Contains(T item)
        {
            foreach (T t in this)
            {
                if (t.Equals(item))
                    return true;
            }
            return false;
        }

        //copies the list to an array starting at a given element
        public void CopyTo(T[] array, int arrayIndex = 0)
        {
            //it's up to the user to handle exceptions from this
            foreach (T t in this)
            {
                array[arrayIndex] = t;
                arrayIndex++;
            }
        }

        //returns the count variable
        public int Count
        {
            get { return count; }
        }

        //this isn't read only
        public bool IsReadOnly
        {
            get { return false; }
        }

        //searches for the given element
        //removes it and returns true if found, otherwise returns false
        public bool Remove(T item)
        {
            foreach (T t in this)
            {
                if (t.Equals(item))
                {
                    Remove();
                    return true;
                }
            }
            return false;
        }

        //returns an enumerator for this list
        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListEnumerator(this);
        }

        //same
        //I dunno why I need both of these
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LinkedListEnumerator(this);
        }

        //steps forward and returns the element that was stepped into
        public void Step()
        {
            head = head.Next;
        }

        //steps backward and returns the element that was stepped into
        public void StepBack()
        {
            head = head.Prev;
        }

        //gets and sets the data in the active element
        public T Current
        {
            get
            {
                return head.Data;
            }
            set
            {
                head.Data = value;
            }
        }
    }
}
