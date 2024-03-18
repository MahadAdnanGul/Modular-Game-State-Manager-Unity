using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateSystem
{
    public class OrderedAction<T, T2>
    {
        public delegate void Del(T args1, T2 args2);

        private List<ActionPriority<Del>> m_delegates;

        public void Add(Del del, int priority = 0)
        {
            if (m_delegates == null)
            {
                m_delegates = new List<ActionPriority<Del>>();
            }
            
            m_delegates.AddSorted(new ActionPriority<Del>{Action = del, Priority = priority});
        }

        public void Remove(Del del)
        {
            if (m_delegates == null)
            {
                return;
            }
            
            for (int i = m_delegates.Count - 1; i >= 0; i--)
            {
                if (del == m_delegates[i].Action)
                {
                    m_delegates.RemoveAt(i);
                    break;
                }
            }

            if (m_delegates.IsEmpty())
            {
                m_delegates = null;
            }
        }

        public void Invoke(T args1, T2 args2)
        {
            if (m_delegates == null)
            {
                return;
            }
            
            for (int i = m_delegates.Count - 1; i >= 0; i--)
            {
                try
                {
                    m_delegates[i].Action.Invoke(args1, args2);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

    internal struct ActionPriority<T> : IComparable<ActionPriority<T>>
    {
        public int Priority;
        public T Action;
        
        public int CompareTo(ActionPriority<T> other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}