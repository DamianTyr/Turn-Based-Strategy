using System;
using UnityEngine;

public struct GridPosition : IEquatable<GridPosition>
{
    public int X;
    public int Z;

    public GridPosition(int x, int z)
    {
        X = x;
        Z = z;
    }
    
    public override bool Equals(object obj)
    {
        return obj is GridPosition other && Equals(other);
    }

    public bool Equals(GridPosition other)
    {
        return this == other; 
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }
    
    public override string ToString()
    {
        return "x: " + X + " z: " + Z;
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.X == b.X && a.Z == b.Z;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }
    
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X + b.X, a.Z + b.Z);
    }
    
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X - b.X, a.Z - b.Z);
    }
}
 