using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class VectorExtension
{ 
    // ------------------------------------------
    public static Vector2 loopedDirection(this Vector2 v, Vector2 w, float width, float height)
    {
        Vector2 diff = w - v;
        diff.x -= Mathf.Round(diff.x / width) * width;
        diff.y -= Mathf.Round(diff.y / height) * height;
        return diff;
    }
    public static Vector3 loopedDirection(this Vector3 v, Vector3 w, float width, float height, float depth)
    {
        Vector3 diff = w - v;
        diff.x -= Mathf.Round(diff.x / width) * width;
        diff.y -= Mathf.Round(diff.y / height) * height;
        diff.z -= Mathf.Round(diff.z / depth) * depth;
        return diff;
    }

    public static float maxDirection(this Vector3 v)
    {
        return new float[] { v.x, v.y, v.z}.OrderByDescending(f => Mathf.Abs(f)).First();
    }

    public static Vector3 Positivize(this Vector3 v)
    {
        if (v.maxDirection() < 0)
            return v.flip();
        return v;
    }
    // ------------------------------------------
    public static Vector3 round(this Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }
    public static Vector2 round(this Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
    public static Vector3 ceil(this Vector3 v)
    {
        return new Vector3(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));
    }
    public static Vector2 ceil(this Vector2 v)
    {
        return new Vector2(Mathf.Ceil(v.x), Mathf.Ceil(v.y));
    }
    public static Vector3 floor(this Vector3 v)
    {
        return new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
    }
    public static Vector2 floor(this Vector2 v)
    {
        return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
    }
    // ------------------------------------------
    public static Vector3 mutate(this Vector3 v, Vector3 amount)
    {
        return new Vector3(v.x + Randomg.Symmetrical(amount.x), v.y + Randomg.Symmetrical(amount.y), v.z + Randomg.Symmetrical(amount.z));
    }
    public static Vector3 mutateX(this Vector3 v, float amount)
    {
        return v.addx(Randomg.Symmetrical(amount));
    }
    public static Vector3 mutateY(this Vector3 v, float amount)
    {
        return v.addy(Randomg.Symmetrical(amount));
    }
    public static Vector3 mutateZ(this Vector3 v, float amount)
    {
        return v.addz(Randomg.Symmetrical(amount));
    }
    public static Vector2 mutate(this Vector2 v, Vector2 amount)
    {
        return new Vector2(v.x + Randomg.Symmetrical(amount.x), v.y + Randomg.Symmetrical(amount.y));
    }
    public static Vector2 mutateX(this Vector2 v, float amount)
    {
        return v.addx(Randomg.Symmetrical(amount));
    }
    public static Vector2 mutateY(this Vector2 v, float amount)
    {
        return v.addy(Randomg.Symmetrical(amount));
    }
    // ------------------------------------------
    public static bool approximately(this Vector2 v1, Vector2 v2)
    {
        return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y);
    }
    public static bool approximately(this Vector3 v1, Vector3 v2)
    {
        return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y) && Mathf.Approximately(v1.z, v2.z);
    }
    public static bool approximatelyXY(this Vector3 v1, Vector3 v2)
    {
        return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y);
    }
    public static bool approximatelyXZ(this Vector3 v1, Vector3 v2)
    {
        return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.z, v2.z);
    }
    public static bool approximatelyYZ(this Vector3 v1, Vector3 v2)
    {
        return Mathf.Approximately(v1.y, v2.y) && Mathf.Approximately(v1.z, v2.z);
    }
    // ------------------------------------------
    public static Vector2 clamp(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
    }
    public static Vector3 clamp(this Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y), Mathf.Clamp(v.z, min.z, max.z));
    }
    // ------------------------------------------
    public static Vector2 ratio(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.InverseLerp(min.x, max.x, v.x), Mathf.InverseLerp(min.y, max.y, v.y));
    }
    public static Vector3 ratio(this Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.InverseLerp(min.x, max.x, v.x), Mathf.InverseLerp(min.y, max.y, v.y), Mathf.InverseLerp(min.z, max.z, v.z));
    }
    public static Vector2 map(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Lerp(min.x, max.x, v.x), Mathf.Lerp(min.y, max.y, v.y));
    }
    public static Vector3 map(this Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Lerp(min.x, max.x, v.x), Mathf.Lerp(min.y, max.y, v.y), Mathf.Lerp(min.z, max.z, v.z));
    }
    // ------------------------------------------
    public static Vector2 add(this Vector2 v, float val)
    {
        return new Vector2(v.x + val, v.y + val);
    }
    public static Vector2 add(this Vector2 v, float x, float y)
    {
        return new Vector2(v.x + x, v.y + y);
    }
    public static Vector2 add(this Vector2 v, Vector2 a)
    {
        return new Vector2(v.x + a.x, v.y + a.y);
    }
    public static Vector2 addx(this Vector2 v, float val)
    {
        return new Vector2(v.x + val, v.y);
    }
    public static Vector2 addy(this Vector2 v, float val)
    {
        return new Vector2(v.x, v.y + val);
    }
    // -------
    public static Vector3 add(this Vector3 v, float val)
    {
        return new Vector3(v.x + val, v.y + val, v.z + val);
    }
    public static Vector3 add(this Vector3 v, float x, float y, float z)
    {
        return new Vector3(v.x + x, v.y + y, v.z + z);
    }
    public static Vector3 add(this Vector3 v, Vector3 a)
    {
        return new Vector3(v.x + a.x, v.y + a.y, v.z + a.z);
    }
    public static Vector3 addx(this Vector3 v, float val)
    {
        return new Vector3(v.x + val, v.y, v.z);
    }
    public static Vector3 addy(this Vector3 v, float val)
    {
        return new Vector3(v.x, v.y + val, v.z);
    }
    public static Vector3 addz(this Vector3 v, float val)
    {
        return new Vector3(v.x, v.y, v.z + val);
    }
    public static Vector3 addxy(this Vector3 v, float val)
    {
        return new Vector3(v.x + val, v.y + val, v.z);
    }
    public static Vector3 addxy(this Vector3 v, float x, float y)
    {
        return new Vector3(v.x + x, v.y + y, v.z);
    }
    public static Vector3 addxy(this Vector3 v, Vector2 a)
    {
        return new Vector3(v.x + a.x, v.y + a.y, v.z);
    }
    public static Vector3 addxz(this Vector3 v, float val)
    {
        return new Vector3(v.x + val, v.y, v.z + val);
    }
    public static Vector3 addxz(this Vector3 v, float x, float z)
    {
        return new Vector3(v.x + x, v.y, v.z + z);
    }
    public static Vector3 addxz(this Vector3 v, Vector2 a)
    {
        return new Vector3(v.x + a.x, v.y, v.z + a.y);
    }
    public static Vector3 addyz(this Vector3 v, float val)
    {
        return new Vector3(v.x, v.y + val, v.z + val);
    }
    public static Vector3 addyz(this Vector3 v, float y, float z)
    {
        return new Vector3(v.x, v.y + y, v.z + z);
    }
    public static Vector3 addyz(this Vector3 v, Vector2 a)
    {
        return new Vector3(v.x, v.y + a.x, v.z + a.y);
    }
    // ------------------------------------------
    public static Vector2 times(this Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }
    public static Vector2 timesx(this Vector2 v1, float v)
    {
        return new Vector2(v1.x * v, v1.y);
    }
    public static Vector2 timesy(this Vector2 v1, float v)
    {
        return new Vector2(v1.x, v1.y * v);
    }
    // -------
    public static Vector3 times(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }
    public static Vector3 timesx(this Vector3 v1, float v)
    {
        return new Vector3(v1.x * v, v1.y, v1.z);
    }
    public static Vector3 timesy(this Vector3 v1, float v)
    {
        return new Vector3(v1.x, v1.y * v, v1.z);
    }
    public static Vector3 timesz(this Vector3 v1, float v)
    {
        return new Vector3(v1.x, v1.y, v1.z * v);
    }
    public static Vector3 timesxy(this Vector3 v1, float v)
    {
        return new Vector3(v1.x * v, v1.y * v, v1.z);
    }
    public static Vector3 timesxy(this Vector3 v1, Vector2 v)
    {
        return new Vector3(v1.x * v.x, v1.y * v.y, v1.z);
    }
    public static Vector3 timesxz(this Vector3 v1, float v)
    {
        return new Vector3(v1.x * v, v1.y, v1.z * v);
    }
    public static Vector3 timesxz(this Vector3 v1, Vector2 v)
    {
        return new Vector3(v1.x * v.x, v1.y, v1.z * v.y);
    }
    public static Vector3 timesyz(this Vector3 v1, float v)
    {
        return new Vector3(v1.x, v1.y * v, v1.z * v);
    }
    public static Vector3 timesyz(this Vector3 v1, Vector2 v)
    {
        return new Vector3(v1.x, v1.y * v.x, v1.z * v.y);
    }
    // ------------------------------------------
    public static Vector2 divide(this Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x / v2.x, v1.y / v2.y);
    }
    public static Vector2 dividex(this Vector2 v1, float v)
    {
        return new Vector2(v1.x / v, v1.y);
    }
    public static Vector2 dividey(this Vector2 v1, float v)
    {
        return new Vector2(v1.x, v1.y / v);
    }
    // -------
    public static Vector3 divide(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }
    public static Vector3 dividex(this Vector3 v1, float v)
    {
        return new Vector3(v1.x / v, v1.y, v1.z);
    }
    public static Vector3 dividey(this Vector3 v1, float v)
    {
        return new Vector3(v1.x, v1.y / v, v1.z);
    }
    public static Vector3 dividez(this Vector3 v1, float v)
    {
        return new Vector3(v1.x, v1.y, v1.z / v);
    }
    public static Vector3 dividexy(this Vector3 v1, float v)
    {
        return new Vector3(v1.x / v, v1.y / v, v1.z);
    }
    public static Vector3 dividexy(this Vector3 v1, Vector2 v)
    {
        return new Vector3(v1.x / v.x, v1.y / v.y, v1.z);
    }
    public static Vector3 dividexz(this Vector3 v1, float v)
    {
        return new Vector3(v1.x / v, v1.y, v1.z / v);
    }
    public static Vector3 dividexz(this Vector3 v1, Vector2 v)
    {
        return new Vector3(v1.x / v.x, v1.y, v1.z / v.y);
    }
    public static Vector3 divideyz(this Vector3 v1, float v)
    {
        return new Vector3(v1.x, v1.y / v, v1.z / v);
    }
    public static Vector3 divideyz(this Vector3 v1, Vector2 v)
    {
        return new Vector3(v1.x, v1.y / v.x, v1.z / v.y);
    }
    // ------------------------------------------
    /// <summary> The angle on the unit circle of this vector (-pi, pi] </summary>
    public static Quaternion rotation(this Vector2 v)
    {
        return Quaternion.Euler(0f, 0f, v.angle() * Mathf.Rad2Deg);
    }
    /// <summary> The angle on the unit circle of this vector (-pi, pi] </summary>
    public static float angle(this Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x);
    }
    /// <summary> The angle between 2 vectors. Angle is directional: returns positive value 
    /// if this vector should rotate counter clockwise in order to reach the 'other' </summary>
    public static float angle(this Vector2 v, Vector2 other)
    {
        float a1 = v.angle();
        float a2 = other.angle();
        return a2 - a1;
    }
    /// <summary> The angle on the unit circle of this vector [0, 2pi) </summary>
    public static float angleFull(this Vector2 v)
    {
        float angle = v.angle();
        if (angle < 0f) return angle + 2f * Mathf.PI;
        return angle;
    }

    public static float angle(this Vector3 a, Vector3 b)
    {
        var angle = Vector3.Angle(a, b); // calculate angle
        // assume the sign of the cross product's Y component:
        return angle * Mathf.Sign(Vector3.Cross(a, b).y);
    }

    // ------------------------------------------
    public static Vector2 flip(this Vector2 v)
    {
        return new Vector2(-v.x, -v.y);
    }
    public static Vector2 flipx(this Vector2 v)
    {
        return new Vector3(-v.x, v.y);
    }
    public static Vector2 flipy(this Vector2 v)
    {
        return new Vector2(v.x, -v.y);
    }
    public static Vector3 flipx(this Vector3 v)
    {
        return new Vector3(-v.x, v.y, v.z);
    }
    public static Vector3 flipy(this Vector3 v)
    {
        return new Vector3(v.x, -v.y, v.z);
    }
    public static Vector3 flipz(this Vector3 v)
    {
        return new Vector3(v.x, v.y, -v.z);
    }
    public static Vector3 flipxy(this Vector3 v)
    {
        return new Vector3(-v.x, -v.y, v.z);
    }
    public static Vector3 flipxz(this Vector3 v)
    {
        return new Vector3(-v.x, v.y, -v.z);
    }
    public static Vector3 flipyz(this Vector3 v)
    {
        return new Vector3(v.x, -v.y, -v.z);
    }
    public static Vector3 flip(this Vector3 v)
    {
        return new Vector3(-v.x, -v.y, -v.z);
    }
    // ------------------------------------------
    public static Vector2 rotate(this Vector2 v, float degrees)
    {
        degrees = Mathf.Deg2Rad * degrees;
        float cs = Mathf.Cos(degrees);
        float sn = Mathf.Sin(degrees);
        return new Vector2(v.x * cs - v.y * sn, v.x * sn + v.y * cs);
    }
    public static Vector3 rotate(this Vector3 v, Vector3 around, float degrees)
    {
        return Quaternion.AngleAxis(degrees, around) * v;
    }
    public static Vector3 rotatez(this Vector3 v, float degrees)
    {
        return Quaternion.AngleAxis(degrees, Vector3.forward) * v;
    }
    public static Vector3 rotatex(this Vector3 v, float degrees)
    {
        return Quaternion.AngleAxis(degrees, Vector3.right) * v;
    }
    public static Vector3 rotatey(this Vector3 v, float degrees)
    {
        return Quaternion.AngleAxis(degrees, Vector3.up) * v;
    }
    // ------------------------------------------
    public static Vector2 perpL(this Vector2 v, float degrees)
    {
        return v.rotate(degrees);
    }
    public static Vector2 perpR(this Vector2 v, float degrees)
    {
        return v.rotate(-degrees);
    }
    public static Vector2 perpL(this Vector2 v)
    {
        return new Vector2(-v.y, v.x);
    }
    public static Vector2 perpR(this Vector2 v)
    {
        return new Vector2(v.y, -v.x);
    }
    // ----
    /// <summary> Perpendicular around z axis </summary>
    public static Vector3 perpRz(this Vector3 v)
    {
        return new Vector3(v.y, -v.x, v.z);
    }
    /// <summary> Perpendicular around y axis </summary>
    public static Vector3 perpRy(this Vector3 v)
    {
        return new Vector3(v.z, v.y, -v.x);
    }
    /// <summary> Perpendicular around x axis </summary>
    public static Vector3 perpRx(this Vector3 v)
    {
        return new Vector3(v.x, v.z, -v.y);
    }
    /// <summary> Perpendicular around z axis </summary>
    public static Vector3 perpLz(this Vector3 v)
    {
        return new Vector3(-v.y, v.x, v.z);
    }
    /// <summary> Perpendicular around y axis </summary>
    public static Vector3 perpLy(this Vector3 v)
    {
        return new Vector3(-v.z, v.y, v.x);
    }
    /// <summary> Perpendicular around x axis </summary>
    public static Vector3 perpLx(this Vector3 v)
    {
        return new Vector3(v.x, -v.z, v.y);
    }
    // ------------------------------------------
    public static Vector2 setx(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }
    public static Vector2 sety(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector3 setz(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
    public static Vector3 setx(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }
    public static Vector3 sety(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }
    public static Vector3 setxy(this Vector3 v, float x, float y)
    {
        return new Vector3(x, y, v.z);
    }
    public static Vector3 setxz(this Vector3 v, float x, float z)
    {
        return new Vector3(x, v.y, z);
    }
    public static Vector3 setyz(this Vector3 v, float y, float z)
    {
        return new Vector3(v.x, y, z);
    }
    public static Vector3 setxy(this Vector3 v, Vector2 s)
    {
        return new Vector3(s.x, s.y, v.z);
    }
    public static Vector3 setxz(this Vector3 v, Vector2 s)
    {
        return new Vector3(s.x, v.y, s.y);
    }
    public static Vector3 setyz(this Vector3 v, Vector2 s)
    {
        return new Vector3(v.x, s.x, s.y);
    }
    // ------------------------------------------
    public static float manhattan(this Vector3 v, Vector3 s)
    {
        return Mathf.Abs(v.x - s.x) + Mathf.Abs(v.y - s.y) + Mathf.Abs(v.z - s.z);
    }
    public static float manhattan(this Vector2 v, Vector2 s)
    {
        return Mathf.Abs(v.x - s.x) + Mathf.Abs(v.y - s.y);
    }
    public static float maxDistance(this Vector3 v, Vector3 s)
    {
        return Mathf.Max(Mathf.Abs(v.x - s.x), Mathf.Abs(v.y - s.y), Mathf.Abs(v.z - s.z));
    }
    public static float maxDistance(this Vector2 v, Vector2 s)
    {
        return Mathf.Max(Mathf.Abs(v.x - s.x), Mathf.Abs(v.y - s.y));
    }
    public static float minDistance(this Vector3 v, Vector3 s)
    {
        return Mathf.Min(Mathf.Abs(v.x - s.x), Mathf.Abs(v.y - s.y), Mathf.Abs(v.z - s.z));
    }
    public static float minDistance(this Vector2 v, Vector2 s)
    {
        return Mathf.Min(Mathf.Abs(v.x - s.x), Mathf.Abs(v.y - s.y));
    }
    public static float distance(this Vector3 v, Vector3 s)
    {
        return Vector3.Distance(v, s);
    }
    public static float distance(this Vector2 v, Vector2 s)
    {
        return Vector2.Distance(v, s);
    }
    // ------------------------------------------
    public static Vector3 cross(this Vector3 v, Vector3 s)
    {
        return Vector3.Cross(v, s);
    }
    public static float dot(this Vector3 v, Vector3 s)
    {
        return Vector3.Dot(v, s);
    }
    public static float dot(this Vector2 v, Vector2 s)
    {
        return Vector2.Dot(v, s);
    }
    // ------------------------------------------
    public static Vector3 castxy(this Vector2 v)
    {
        return v;
    }
    public static Vector3 castxz(this Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }
    public static Vector3 castyz(this Vector2 v)
    {
        return new Vector3(0f, v.x, v.y);
    }
    public static Vector2 castxy(this Vector3 v)
    {
        return v;
    }
    public static Vector2 castxz(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    public static Vector2 castyz(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    // ------------------------------------------
    private static float _rand()
    {
        return UnityEngine.Random.Range(-1f, 1f);
    }
    public static Vector3 randx()
    {
        return new Vector3(_rand(), 0f, 0f);
    }
    public static Vector3 randx(float y, float z)
    {
        return new Vector3(_rand(), y, z);
    }
    public static Vector3 randy()
    {
        return new Vector3(0f, _rand(), 0f);
    }
    public static Vector3 randz()
    {
        return new Vector3(0f, 0f, _rand());
    }
    public static Vector3 randxy()
    {
        return new Vector3(_rand(), _rand(), 0f);
    }
    public static Vector3 randxz()
    {
        return new Vector3(_rand(), 0f, _rand());
    }
    public static Vector3 randyz()
    {
        return new Vector3(0f, _rand(), _rand());
    }
    public static Vector3 rand()
    {
        return new Vector3(_rand(), _rand(), _rand());
    }
    public static Vector3 randxyz()
    {
        return rand();
    }

    private static float _rand01()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }
    public static Vector3 randx01()
    {
        return new Vector3(_rand01(), 0f, 0f);
    }
    public static Vector3 randy01()
    {
        return new Vector3(0f, _rand01(), 0f);
    }
    public static Vector3 randz01()
    {
        return new Vector3(0f, 0f, _rand01());
    }
    public static Vector3 randxy01()
    {
        return new Vector3(_rand01(), _rand01(), 0f);
    }
    public static Vector3 randxz01()
    {
        return new Vector3(_rand01(), 0f, _rand01());
    }
    public static Vector3 randyx01()
    {
        return new Vector3(0f, _rand01(), _rand01());
    }
    public static Vector3 rand01()
    {
        return new Vector3(_rand01(), _rand01(), _rand01());
    }
    // ------------------------------------------
    public static Point2 point2(this Vector2 v)
    {
        return new Point2((int)v.x, (int)v.y);
    }
    public static Vector2 vector2(this Point2 p)
    {
        return new Vector2(p.x, p.y);
    }
    public static Point2 point2(this Vector3 v)
    {
        return new Point2((int)v.x, (int)v.y);
    }
    public static Vector3 vector3(this Point2 p)
    {
        return new Vector3(p.x, p.y, 0);
    }


}