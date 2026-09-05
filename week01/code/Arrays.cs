public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // TODO Problem 1 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.

        // PLAN:
        // 1. Since we already know the final size of the result (it's given to us as 'length'),
        //    we can create an array of that exact size up front instead of growing it dynamically.
        // 2. We need to loop from 1 to 'length' (inclusive) - the loop counter represents which
        //    multiple we are on (1st multiple, 2nd multiple, etc).
        // 3. For each position in the loop, the value to store is 'number' multiplied by the
        //    current multiple counter (e.g. 1st multiple = number * 1, 2nd multiple = number * 2).
        // 4. Store that calculated value into the array at the correct index. Since arrays are
        //    zero-indexed but our multiple counter starts at 1, the index will be (counter - 1).
        // 5. Once the loop finishes, return the completed array.

        var result = new double[length];
        for (var i = 1; i <= length; i++)
        {
            result[i - 1] = number * i;
        }

        return result;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // TODO Problem 2 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.

        // PLAN:
        // 1. Rotating right by 'amount' means: the LAST 'amount' items in the list need to move to
        //    the FRONT of the list, and everything else shifts back to fill in behind them.
        //    Example: {1,2,3,4,5,6,7,8,9} rotate right by 3
        //             -> last 3 items {7,8,9} move to front
        //             -> remaining items {1,2,3,4,5,6} come after
        //             -> result: {7,8,9,1,2,3,4,5,6}
        // 2. To do this with List methods, we can split the list into two pieces (slices):
        //      - the "tail" piece: the last 'amount' items (these become the new front)
        //      - the "front" piece: everything before the tail (these become the new back)
        // 3. We can get the split index as (data.Count - amount). Everything from that index
        //    to the end is the tail; everything before that index is the front.
        // 4. Use GetRange to pull out the tail piece as its own list:
        //      tail = data.GetRange(splitIndex, amount)
        // 5. Remove that same tail piece from the original list, leaving just the front piece behind:
        //      data.RemoveRange(splitIndex, amount)
        // 6. Insert the tail piece back at the very beginning (index 0) of what remains.
        //      data.InsertRange(0, tail)
        // 7. Because we modified 'data' directly with RemoveRange/InsertRange, there is no need
        //    to return anything - the caller's original list is now rotated in place.

        var splitIndex = data.Count - amount;
        var tail = data.GetRange(splitIndex, amount);
        data.RemoveRange(splitIndex, amount);
        data.InsertRange(0, tail);
    }
}