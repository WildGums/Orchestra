using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace TableView
{
  public class TableViewCellCollection : IList, ICollection, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
  {
    public int CopyCount { get; set; }

    private object _copyObject;
    public object CopyObject
    {
      get { return _copyObject; }
      set
      {
        if (value != _copyObject)
        {
          _copyObject = value;
          OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
      }
    }

    public TableViewCellCollection(object copyObject, int copyCount)
    {
      CopyCount = copyCount;
      CopyObject = copyObject;
    }

    public int Add(object value)
    {
      throw new NotImplementedException();
    }

    public void Clear()
    {
      throw new NotImplementedException();
    }

    public bool Contains(object value)
    {
      return value == CopyObject;
    }

    public int IndexOf(object value)
    {
      if (value == CopyObject)
        return 0;
      return -1;
    }

    public void Insert(int index, object value)
    {
      throw new NotImplementedException();
    }

    public bool IsFixedSize
    {
      get { return false; }
    }

    public bool IsReadOnly
    {
      get { return true; }
    }

    public void Remove(object value)
    {
      throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
      throw new NotImplementedException();
    }

    public object this[int index]
    {
      get { return CopyObject; }
      set { throw new NotImplementedException(); }
    }

    public void CopyTo(Array array, int index)
    {
      throw new NotImplementedException();
    }

    public int Count
    {
      get { return this.CopyCount; }
      set
      {
        if (value != this.CopyCount)
        {
          this.CopyCount = value;
          Reset();
        }
      }
    }

    public bool IsSynchronized
    {
      get { return false; }
    }

    public object SyncRoot
    {
      get { return this; }
    }

    public void Reset()
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (this.PropertyChanged != null)
        this.PropertyChanged(this, e);
    }

    private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this.CollectionChanged != null)
        this.CollectionChanged(this, e);
    }


    //-------------------------------------------------------
    #region TableCellCollection Enumerator
    private class TableCellCollectionEnumerator : IEnumerator
    {
      int _current;
      int _count;
      object _copyObject;

      public TableCellCollectionEnumerator(TableViewCellCollection col)
      {
        _current = -1;
        _count = col.Count;
        _copyObject = col.CopyObject;
      }

      public object Current
      {
        get { return (_current == -1) ? null : _copyObject; }
      }

      public bool MoveNext()
      {
        if (_current == _count)
          return false;
        ++_current;
        return true;
      }

      public void Reset()
      {
        _current = -1;
      }
    }
    #endregion

    public IEnumerator GetEnumerator()
    {
      return new TableCellCollectionEnumerator(this);
    }
  }
}
