using System.Collections;
using System.Collections.Generic;

namespace ClassicalCryptography.Transposition;

internal class LinkedList
{
    private class Node
    {
        public ushort Value;
        public Node? Next;
        public Node(ushort value) => Value = value;
    }

    private readonly Node first;
    private Node last;
    private Node current;
    public bool IsEnd => current.Next == null;
    public LinkedList(ushort[] arr)
    {
        if (arr == null || arr.Length == 0)
            throw new ArgumentException("数组不能为空", nameof(arr));
        current = first = new Node(arr[0]);
        for (int i = 1; i < arr.Length; i++)
            current = current.Next = new Node(arr[i]);
        last = current;
        current = first;
    }
    private LinkedList(Node first, Node last)
    {
        this.first = first;
        this.last = last;
        current = first;
    }
    public bool MoveStep(int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (IsEnd) return false;
            current = current.Next!;
        }
        return true;
    }

    public LinkedList? SubList()
    {
        if (IsEnd) return null;
        return new(current.Next!, last);
    }

    public void LinkLast(LinkedList list)
    {
        last.Next = list.first;
    }

    public void EndCurrent(LinkedList list)
    {
        last = list.current;
        last.Next = null;
    }

    public void LinkCurrent(LinkedList list)
    {
        current.Next = list.first;
    }

    public ushort[] ToArray()
    {
        var result = new List<ushort>();
        var node = first;
        while (node != null)
        {
            result.Add(node.Value);
            node = node.Next;
        }
        return result.ToArray();
    }
}
