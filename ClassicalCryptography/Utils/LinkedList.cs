using System.Collections;
using System.Collections.Generic;

namespace ClassicalCryptography.Utils;

internal class LinkedList
{
    public class Node
    {
        public ushort Value;
        public Node? Next;
        public Node(ushort value) => Value = value;
    }

    public Node First;
    public Node Last;
    public Node Current;
    public bool IsEnd => Current.Next == null;
    public LinkedList(ushort[] arr)
    {
        if (arr == null || arr.Length == 0)
            throw new ArgumentException("数组不能为空", nameof(arr));
        Current = First = new Node(arr[0]);
        for (int i = 1; i < arr.Length; i++)
            Current = Current.Next = new Node(arr[i]);
        Last = Current;
        Current = First;
    }
    private LinkedList(Node first, Node last)
    {
        this.First = first;
        this.Last = last;
        Current = first;
    }
    public bool MoveStep(int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (IsEnd) return false;
            Current = Current.Next!;
        }
        return true;
    }

    public LinkedList? SubList()
    {
        if (IsEnd) return null;
        return new(Current.Next!, Last);
    }

    public void LinkLast(LinkedList list)
    {
        Last.Next = list.First;
    }

    public void EndCurrent(LinkedList list)
    {
        Last = list.Current;
        Last.Next = null;
    }

    public void LinkCurrent(LinkedList list)
    {
        Current.Next = list.First;
    }

    public ushort[] ToArray()
    {
        var result = new List<ushort>();
        var node = First;
        while (node != null)
        {
            result.Add(node.Value);
            node = node.Next;
        }
        return result.ToArray();
    }
}
