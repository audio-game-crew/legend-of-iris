
using UnityEngine;

[System.Serializable]
public struct Point2
{
    public int x, y;

    public Point2(int px, int py)
    {
        x = px;
        y = py;
    }

    public int Manhattan { get { return Mathf.Abs(x) + Mathf.Abs(y); } }

    public Point2 MoveOneX(int direction)
    {
        x += (int)Mathf.Sign(direction);
        return this;
    }

    public Point2 MoveOneY(int direction)
    {
        y += (int)Mathf.Sign(direction);
        return this;
    }

    public override string ToString()
    {
        return "<" + x + ", " + y + ">";
    }

    public static bool operator ==(Point2 a, Point2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Point2 a, Point2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static Point2 operator -(Point2 a, Point2 b)
    {
        return new Point2(a.x - b.x, a.y - b.y);
    }

    public static Point2 operator +(Point2 a, Point2 b)
    {
        return new Point2(a.x + b.x, a.y + b.y);
    }

    public static Point2 operator *(Point2 a, int b)
    {
        return new Point2(a.x * b, a.y * b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Point2) return this == (Point2)obj;
        else return false;
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            return hash;
        }
    }
}
