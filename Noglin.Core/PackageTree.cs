#nullable enable
using System.Collections.Generic;

namespace Noglin.Core
{
    public struct PackageNode
    {
        public string Name;
        public object Value;
    }
    
    public struct PackageTree
    {
        public string NodeName { get; }
        public List<PackageTree> SubTrees { get; }
        public List<PackageNode> SubNodes { get; }

        public PackageTree(string name)
        {
            NodeName = name;
            SubTrees = new List<PackageTree>();
            SubNodes = new List<PackageNode>();
        }

        public PackageTree? FindSubTree(string name)
        {
            foreach (PackageTree subtree in SubTrees)
            {
                if (subtree.NodeName == name)
                {
                    return subtree;
                }
            }

            return null;
        }

        public T? FindNode<T>(string name)
            where T : class
        {
            foreach (PackageNode node in SubNodes)
            {
                if (node.Name == name && node.Value is T nodeValue)
                {
                    return nodeValue;
                }
            }

            return null;
        }

        public IEnumerable<PackageNode> FindNodes<T>()
        {
            foreach (PackageNode node in SubNodes)
            {
                if (node.Value is T)
                {
                    yield return node;
                }
            }
        }
    }
}