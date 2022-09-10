using System;

public interface IDataProperty : IDataNode
{
    Type NativeType { get; }
    ValueT GetValue<ValueT>();
    void SetValue<ValueT>(ValueT value);
}

public class DataProperty<T> : IDataProperty
{
    private readonly string _name;

    private T _data;

    public string Name => _name;

    public DataProperty(string name, T data)
    {
        _name = name;

        SetValue(data);
    }

    public Type NativeType => typeof(T);

    public ValueT GetValue<ValueT>()
    {
        return DataConverter.Convert<T, ValueT>(_data);
    }

    public void SetValue<ValueT>(ValueT value)
    {
        _data = DataConverter.Convert<ValueT, T>(value);
    }
}