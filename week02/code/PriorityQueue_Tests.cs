using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue three items with different priorities (Low=1, High=5, Medium=3), then dequeue all three.
    // Expected Result: Items come out in priority order regardless of the order they were enqueued in: High, Medium, Low.
    // Defect(s) Found: The loop in Dequeue skipped the last item in the list (index < _queue.Count - 1), so the
    // highest-priority item could be missed if it was the last one enqueued.
    public void TestPriorityQueue_DequeueHighestPriorityFirst()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("High", 5);
        priorityQueue.Enqueue("Medium", 3);

        Assert.AreEqual("High", priorityQueue.Dequeue());
        Assert.AreEqual("Medium", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue two items that share the same highest priority (First then Second, both priority 5),
    // plus a lower priority item, then dequeue all three.
    // Expected Result: Among tied highest-priority items, the one closest to the front (enqueued first) is
    // dequeued first: First, Second, Third.
    // Defect(s) Found: Dequeue used ">=" instead of ">" when comparing priorities, so on a tie the later
    // (more recently enqueued) item was incorrectly selected instead of the earlier one.
    public void TestPriorityQueue_TieBreaksToFrontOfQueue()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("First", 5);
        priorityQueue.Enqueue("Second", 5);
        priorityQueue.Enqueue("Third", 2);

        Assert.AreEqual("First", priorityQueue.Dequeue());
        Assert.AreEqual("Second", priorityQueue.Dequeue());
        Assert.AreEqual("Third", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue a single item, dequeue it, then attempt to dequeue again from what should now be an
    // empty queue.
    // Expected Result: The first Dequeue returns the item and removes it from the queue. The second Dequeue
    // call throws an InvalidOperationException with the message "The queue is empty."
    // Defect(s) Found: Dequeue never removed the item from the internal list (missing _queue.RemoveAt), so the
    // same item could be dequeued repeatedly and the queue never emptied.
    public void TestPriorityQueue_DequeueRemovesItemFromQueue()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Only", 1);

        Assert.AreEqual("Only", priorityQueue.Dequeue());

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
    }

    [TestMethod]
    // Scenario: Call Dequeue on a PriorityQueue that has never had anything enqueued.
    // Expected Result: An InvalidOperationException is thrown with the message "The queue is empty."
    // Defect(s) Found: None - this case worked correctly already.
    public void TestPriorityQueue_EmptyQueueThrowsException()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
    }

    // Add more test cases as needed below.
}