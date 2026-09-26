using System.Collections;

public class LinkedList : IEnumerable<int>
{
    private Node? _head;
    private Node? _tail;

    /// <summary>
    /// Insert a new node at the front (i.e. the head) of the linked list.
    /// </summary>
    public void InsertHead(int value)
    {
        // Create new node
        Node newNode = new(value);
        // If the list is empty, then point both head and tail to the new node.
        if (_head is null)
        {
            _head = newNode;
            _tail = newNode;
        }
        // If the list is not empty, then only head will be affected.
        else
        {
            newNode.Next = _head; // Connect new node to the previous head
            _head.Prev = newNode; // Connect the previous head to the new node
            _head = newNode; // Update the head to point to the new node
        }
    }

    /// <summary>
    /// Insert a new node at the back (i.e. the tail) of the linked list.
    /// </summary>
    public void InsertTail(int value)
    {
        // PLAN (mirrors InsertHead, but working from the tail end):
        // 1. Create the new node.
        // 2. If the list is empty (_tail is null), point both head and tail to the new node.
        // 3. Otherwise, connect the new node after the current tail:
        //    - new node's Prev points to the old tail
        //    - old tail's Next points to the new node
        //    - update _tail to point to the new node
        Node newNode = new(value);

        if (_tail is null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            newNode.Prev = _tail; // Connect new node to the previous tail
            _tail.Next = newNode; // Connect the previous tail to the new node
            _tail = newNode; // Update the tail to point to the new node
        }
    }


    /// <summary>
    /// Remove the first node (i.e. the head) of the linked list.
    /// </summary>
    public void RemoveHead()
    {
        // If the list has only one item in it, then set head and tail 
        // to null resulting in an empty list.  This condition will also
        // cover an empty list.  Its okay to set to null again.
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        // If the list has more than one item in it, then only the head
        // will be affected.
        else if (_head is not null)
        {
            _head.Next!.Prev = null; // Disconnect the second node from the first node
            _head = _head.Next; // Update the head to point to the second node
        }
    }


    /// <summary>
    /// Remove the last node (i.e. the tail) of the linked list.
    /// </summary>
    public void RemoveTail()
    {
        // PLAN (mirrors RemoveHead, but working from the tail end):
        // 1. If the list has only one item (_head == _tail), set both to null - this also
        //    safely covers the empty-list case since null == null.
        // 2. Otherwise, disconnect the tail from the second-to-last node:
        //    - the node before the tail (_tail.Prev) gets its Next set to null
        //    - update _tail to point to that previous node
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        else if (_tail is not null)
        {
            _tail.Prev!.Next = null; // Disconnect the second-to-last node from the last node
            _tail = _tail.Prev; // Update the tail to point to the second-to-last node
        }
    }

    /// <summary>
    /// Insert 'newValue' after the first occurrence of 'value' in the linked list.
    /// </summary>
    public void InsertAfter(int value, int newValue)
    {
        // Search for the node that matches 'value' by starting at the 
        // head of the list.
        Node? curr = _head;
        while (curr is not null)
        {
            if (curr.Data == value)
            {
                // If the location of 'value' is at the end of the list,
                // then we can call insert_tail to add 'new_value'
                if (curr == _tail)
                {
                    InsertTail(newValue);
                }
                // For any other location of 'value', need to create a 
                // new node and reconnect the links to insert.
                else
                {
                    Node newNode = new(newValue);
                    newNode.Prev = curr; // Connect new node to the node containing 'value'
                    newNode.Next = curr.Next; // Connect new node to the node after 'value'
                    curr.Next!.Prev = newNode; // Connect node after 'value' to the new node
                    curr.Next = newNode; // Connect the node containing 'value' to the new node
                }

                return; // We can exit the function after we insert
            }

            curr = curr.Next; // Go to the next node to search for 'value'
        }
    }

    /// <summary>
    /// Remove the first node that contains 'value'.
    /// </summary>
    public void Remove(int value)
    {
        // PLAN:
        // 1. Search for the node containing 'value', starting at the head, just like InsertAfter does.
        // 2. Once found, figure out where it is in the list:
        //    - If it's the head node, reuse RemoveHead() to handle it.
        //    - If it's the tail node, reuse RemoveTail() to handle it.
        //    - Otherwise (a middle node), reconnect its neighbors directly:
        //      the previous node's Next should skip over this node to point to the next node,
        //      and the next node's Prev should skip back to point to the previous node.
        // 3. Stop searching immediately once the first match is found and removed (return).
        Node? curr = _head;
        while (curr is not null)
        {
            if (curr.Data == value)
            {
                if (curr == _head)
                {
                    RemoveHead();
                }
                else if (curr == _tail)
                {
                    RemoveTail();
                }
                else
                {
                    curr.Prev!.Next = curr.Next; // Skip over curr from the previous node's side
                    curr.Next!.Prev = curr.Prev; // Skip over curr from the next node's side
                }

                return; // Stop searching after the first match is removed
            }

            curr = curr.Next;
        }
    }

    /// <summary>
    /// Search for all instances of 'oldValue' and replace the value to 'newValue'.
    /// </summary>
    public void Replace(int oldValue, int newValue)
    {
        // PLAN:
        // 1. Unlike Remove, we do NOT stop after the first match - we need to check every node.
        // 2. Start at the head and walk through the entire list.
        // 3. For each node, if its Data matches 'oldValue', update it to 'newValue'.
        // 4. Continue to the next node regardless of whether a match was found, until
        //    the end of the list (curr becomes null) is reached.
        Node? curr = _head;
        while (curr is not null)
        {
            if (curr.Data == oldValue)
            {
                curr.Data = newValue;
            }

            curr = curr.Next;
        }
    }

    /// <summary>
    /// Yields all values in the linked list
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        // call the generic version of the method
        return this.GetEnumerator();
    }

    /// <summary>
    /// Iterate forward through the Linked List
    /// </summary>
    public IEnumerator<int> GetEnumerator()
    {
        var curr = _head; // Start at the beginning since this is a forward iteration.
        while (curr is not null)
        {
            yield return curr.Data; // Provide (yield) each item to the user
            curr = curr.Next; // Go forward in the linked list
        }
    }

    /// <summary>
    /// Iterate backward through the Linked List
    /// </summary>
    public IEnumerable Reverse()
    {
        // PLAN (mirrors GetEnumerator, but starting from the opposite end):
        // 1. Start at the tail instead of the head, since we're going backward.
        // 2. Loop while the current node is not null.
        // 3. Yield the current node's Data.
        // 4. Move to curr.Prev instead of curr.Next to go backward through the list.
        var curr = _tail; // Start at the end since this is a backward iteration.
        while (curr is not null)
        {
            yield return curr.Data; // Provide (yield) each item to the user
            curr = curr.Prev; // Go backward in the linked list
        }
    }

    public override string ToString()
    {
        return "<LinkedList>{" + string.Join(", ", this) + "}";
    }

    // Just for testing.
    public Boolean HeadAndTailAreNull()
    {
        return _head is null && _tail is null;
    }

    // Just for testing.
    public Boolean HeadAndTailAreNotNull()
    {
        return _head is not null && _tail is not null;
    }
}

public static class IntArrayExtensionMethods {
    public static string AsString(this IEnumerable array) {
        return "<IEnumerable>{" + string.Join(", ", array.Cast<int>()) + "}";
    }
}