using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ColorExtension
{
    public static Color clamp(this Color v)
    {
        return new Color(Mathf.Clamp01(v.r), Mathf.Clamp01(v.g), Mathf.Clamp01(v.b), Mathf.Clamp01(v.a));
    }

    public static Color castc3(this Vector3 v)
    {
        return new Color(v.x, v.y, v.z);
    }
    public static Color castc3(this Vector3 v, float a)
    {
        return new Color(v.x, v.y, v.z, a);
    }
    public static Color castc4(this Vector4 v)
    {
        return new Color(v.x, v.y, v.z, v.w);
    }
    public static HSV hsv(this Color v)
    {
        return new HSV(v);
    }
    public static Vector3 castc3(this Color v)
    {
        return new Vector3(v.r, v.g, v.b);
    }
    public static Vector4 castc4(this Color v)
    {
        return new Vector4(v.r, v.g, v.b, v.a);
    }

    public static Color addr(this Color c, float r)
    {
        return c.setr(c.r + r);
    }
    public static Color addg(this Color c, float g)
    {
        return c.setg(c.g + g);
    }
    public static Color addb(this Color c, float b)
    {
        return c.setb(c.b + b);
    }
    public static Color adda(this Color c, float a)
    {
        return c.seta(c.a + a);
    }
    public static Color addh(this Color c, float h)
    {
        HSV hsv = new HSV(c);
        return hsv.RotateH((h + hsv.h) % 1f).ToColor();
    }
    public static Color adds(this Color c, float s)
    {
        HSV hsv = new HSV(c);
        return hsv.S(s + hsv.s).ToColor();
    }
    public static Color addv(this Color c, float v)
    {
        HSV hsv = new HSV(c);
        return hsv.V(v + hsv.v).ToColor();
    }

    public static Color setr(this Color c, float r)
    {
        return new Color(r, c.g, c.b, c.a).clamp();
    }
    public static Color setg(this Color c, float g)
    {
        return new Color(c.r, g, c.b, c.a).clamp();
    }
    public static Color setb(this Color c, float b)
    {
        return new Color(c.r, c.g, b, c.a).clamp();
    }
    public static Color seta(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a).clamp();
    }
    public static Color seth(this Color c, float h)
    {
        return new HSV(c).H(h).ToColor();
    }
    public static Color sets(this Color c, float s)
    {
        return new HSV(c).S(s).ToColor();
    }
    public static Color setv(this Color c, float v)
    {
        return new HSV(c).V(v).ToColor();
    }
    public static Color mutateH(this Color c, float amount = 0.1f)
    {
        return HSV.MutateH(c, amount);
    }
    public static Color mutateS(this Color c, float amount = 0.1f)
    {
        return HSV.MutateS(c, amount);
    }
    public static Color mutateV(this Color c, float amount = 0.1f)
    {
        return HSV.MutateV(c, amount);
    }
}