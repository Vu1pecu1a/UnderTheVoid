using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs
{
    public class Vertex : IEquatable<Vertex>
    {
        public Vector3 Position { get; private set; }

        public Vertex() { }

        public Vertex(Vector3 position)
        {
            Position = position;
        }

        public override bool Equals(object obj)
        {
            if(obj is Vertex v) {
                return Position == v.Position;
            }
            return false;
        }

        public bool Equals(Vertex other)
        {
            return Position == other.Position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

    public class Vertex<T> : Vertex
    {
        public T Item { get; private set; }

        public Vertex(T item)
        {
            Item = item;
        }

        public Vertex(Vector3 position, T item) : base(position)
        {
            Item = item;
        }
    }

    public class Edge : IEquatable<Edge>
    {
        public Vertex U { get; private set; }
        public Vertex V { get; private set; }
        public Edge() { }
        public Edge(Vertex u, Vertex v)
        {
            U = u;
            V = v;
        }

        public static bool operator ==(Edge left, Edge right) //같을 경우 비교 함수
        {
            return (left.U == right.U || left.U == right.V)
                && (left.V == right.U || left.V == right.V);
        }

        public static bool operator !=(Edge left, Edge right)//다를 경우 비교함수
        {
            return !(left == right);
        }

        public override bool Equals(object obj)//타입이 맞는지 확인하는 함수
        {
            if (obj is Edge e)
            {
                return this == e;
            }
            return false;
        }

        public bool Equals(Edge e)
        {
            return this == e;
        }

        public override int GetHashCode()
        {
            return U.GetHashCode() ^ V.GetHashCode();//둘중 하나만  
        }
    }
}