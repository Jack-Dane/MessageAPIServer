using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MessageAppServerTests.MockData
{
    public class TestDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T>
        where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public TestDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public override EntityEntry<T> Add(T item)
        {
            _data.Add(item);
            return item as EntityEntry<T>;
        }

        public override EntityEntry<T> Remove(T item)
        {
            _data.Remove(item);
            return item as EntityEntry<T>;
        }

        public override EntityEntry<T> Attach(T item)
        {
            _data.Add(item);
            return item as EntityEntry<T>;
        }

        public override LocalView<T> Local
        {
            get { return new ObservableCollection<T>(_data) as LocalView<T>; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}