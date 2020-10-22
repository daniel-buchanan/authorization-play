using System;
using System.Collections.Generic;

namespace authorization_play.Core
{
    public interface IStorage<T>
    {
        void Add(T item);
        IEnumerable<T> All();
        T FirstOrDefault(Func<T, bool> predicate);
        IEnumerable<T> Where(Func<T, bool> predicate);
        void Remove(T item);
        bool Exists(Func<T, bool> predicate);
    }
}
