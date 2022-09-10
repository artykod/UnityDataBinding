using System;
using System.Collections.Generic;

public interface IDataSource : IDataNode
{
    void AddNode(IDataNode node);
    void RemoveNode(string nodeName);
    bool TryGetNode<T>(ulong nodeHash, out T result) where T : IDataNode;
    bool TryGetNode<T>(string nodeName, out T result) where T : IDataNode;
    bool TryGetNodeByPath<T>(string nodePath, out T result) where T : IDataNode;
    void ForEach<T>(Action<T> action) where T : IDataNode;

    static ulong HashNodeName(string value)
    {
        var hashedValue = 3074457345618258791ul;

        foreach (var ch in value)
        {
            hashedValue += ch;
            hashedValue *= 3074457345618258799ul;
        }

        return hashedValue;
    }

    static ulong HashNodeName(string value, int idx, int length)
    {
        var hashedValue = 3074457345618258791ul;

        for (int i = idx, l = idx + length; i < l; ++i)
        {
            hashedValue += value[i];
            hashedValue *= 3074457345618258799ul;
        }

        return hashedValue;
    }
}

public class DataSource : IDataSource
{
    private readonly string _name;
    private readonly Dictionary<ulong, IDataNode> _dataNodes;

    public string Name => _name;

    public DataSource(string name)
    {
        _name = name;
        _dataNodes = new Dictionary<ulong, IDataNode>();
    }

    public void AddNode(IDataNode node)
    {
        _dataNodes[IDataSource.HashNodeName(node.Name)] = node;
    }

    public void RemoveNode(string name)
    {
        _dataNodes.Remove(IDataSource.HashNodeName(name));
    }

    public bool TryGetNode<T>(ulong nodeHash, out T result) where T : IDataNode
    {
        if (_dataNodes.TryGetValue(nodeHash, out var node) && node is T resultNode)
        {
            result = resultNode;

            return true;
        }

        result = default;

        return false;
    }

    public bool TryGetNode<T>(string nodeName, out T result) where T : IDataNode
    {
        return TryGetNode(IDataSource.HashNodeName(nodeName), out result);
    }

    public bool TryGetNodeByPath<T>(string nodePath, out T result) where T : IDataNode
    {
        if (!nodePath.Contains('.'))
        {
            return TryGetNode(nodePath, out result);
        }

        result = default;

        var dataSource = (IDataSource)this;
        var node = default(IDataNode);
        var nameIdx = 0;
        var nameLength = 0;

        for (int i = 0, l = nodePath.Length; i < l; ++i)
        {
            if (nodePath[i] != '.')
            {
                ++nameLength;

                if (i < l - 1)
                {
                    continue;
                }
            }

            var nameHash = IDataSource.HashNodeName(nodePath, nameIdx, nameLength);

            if (!dataSource.TryGetNode<IDataNode>(nameHash, out node))
            {
                node = null;

                break;
            }

            nameIdx = i + 1;
            nameLength = 0;

            if (node is IDataSource)
            {
                dataSource = node as IDataSource;
            }
        }

        if (node is T)
        {
            result = (T)node;

            return true;
        }

        return false;
    }

    public void ForEach<T>(Action<T> action) where T : IDataNode
    {
        if (action == null)
        {
            return;
        }

        foreach (var pair in _dataNodes)
        {
            if (pair.Value is T target)
            {
                action.Invoke(target);
            }
        }
    }
}