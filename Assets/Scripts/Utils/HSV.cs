using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct HSV
{
    public float h;
    public float s;
    public float v;
    public float a;

    public HSV(float h, float s, float v, float a)
    {
        this.h = h;
        this.s = s;
        this.v = v;
        this.a = a;
    }

    public HSV(float h, float s, float v)
    {
        this.h = h;
        this.s = s;
        this.v = v;
        this.a = 1f;
    }

    public HSV(Color col)
    {
        HSV temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        v = temp.v;
        a = temp.a;
    }

    public static HSV RandomH(float min, float max, float s, float v)
    {
        return new HSV(UnityEngine.Random.Range(min, max), s, v);
    }

    public static HSV RandomH(float s, float v)
    {
        return RandomH(0f, 1f, s, v);
    }

    public HSV RotateRandomH(float min, float max)
    {
        float val = UnityEngine.Random.Range(min, max) * Mathf.Sign(UnityEngine.Random.Range(0f, 1f) - 0.5f);
        return RotateH(val);
    }

    public HSV RotateH(float val)
    {
        h = (h + val) % 1f;
        return this;
    }

    public HSV H(float val)
    {
        h = Mathf.Clamp01(val);
        return this;
    }

    public HSV S(float val)
    {
        s = Mathf.Clamp01(val);
        return this;
    }

    public HSV V(float val)
    {
        v = Mathf.Clamp01(val);
        return this;
    }

    public HSV A(float val)
    {
        a = Mathf.Clamp01(val);
        return this;
    }

    public HSV Hx(float val)
    {
        h = Mathf.Clamp01(h * val);
        return this;
    }

    public HSV Sx(float val)
    {
        s = Mathf.Clamp01(s * val);
        return this;
    }

    public HSV Vx(float val)
    {
        v = Mathf.Clamp01(v * val);
        return this;
    }

    public HSV Ax(float val)
    {
        a = Mathf.Clamp01(a * val);
        return this;
    }

    public static Color Mutate(Color c, float amount = 0.1f, bool mutateAlpha = false)
    {
        return Mutate(FromColor(c), amount, mutateAlpha).ToColor();
    }

    public static Color MutateH(Color c, float amount = 0.1f)
    {
        return MutateH(FromColor(c), amount).ToColor();
    }

    public static Color MutateS(Color c, float amount = 0.1f)
    {
        return MutateS(FromColor(c), amount).ToColor();
    }

    public static Color MutateV(Color c, float amount = 0.1f)
    {
        return MutateV(FromColor(c), amount).ToColor();
    }

    public static Color MutateA(Color c, float amount = 0.1f)
    {
        return MutateA(FromColor(c), amount).ToColor();
    }

    public static HSV Mutate(HSV c, float amount = 0.1f, bool mutateAlpha = false)
    {
        if (mutateAlpha)
            return MutateA(MutateV(MutateS(MutateH(c))));
        else
            return MutateV(MutateS(MutateH(c)));
    }

    public static HSV MutateH(HSV c, float amount = 0.1f)
    {
        c.h = MutateWrapped(c.h, amount);
        return c;
    }

    public static HSV MutateS(HSV c, float amount = 0.1f)
    {
        c.s = Mutate(c.s, amount);
        return c;
    }

    public static HSV MutateV(HSV c, float amount = 0.1f)
    {
        c.v = Mutate(c.v, amount);
        return c;
    }

    public static HSV MutateA(HSV c, float amount = 0.1f)
    {
        c.a = Mutate(c.a, amount);
        return c;
    }

    private static float MutateWrapped(float v, float amount = 0.1f)
    {
        return (1f + v + Randomg.Gaussian(amount)) % 1f;
    }

    private static float Mutate(float v, float amount = 0.1f)
    {
        return Mathf.Clamp01(v + Randomg.Gaussian(amount));
    }

    private static void swap(ref float a, ref float b)
    {
        float c = a;
        a = b;
        b = c;
    }

    public static HSV FromColor(Color color)
    {
        HSV ret = new HSV(0f, 0f, 0f, color.a);
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float K = 0f;

        if (g < b)
        {
            swap(ref g, ref b);
            K = -1f;
        }

        if (r < g)
        {
            swap(ref r, ref g);
            K = -2f / 6f - K;
        }

        float chroma = r - Mathf.Min(g, b);
        ret.h = Mathf.Abs(K + (g - b) / (6f * chroma + 1e-20f));
        ret.s = chroma / (r + 1e-20f);
        ret.v = r;
        return ret;
    }

    private static Vector3 Hue(float h)
    {
        return new Vector3(
            Mathf.Clamp01(Mathf.Abs(h * 6f - 3f) - 1f), 
            Mathf.Clamp01(2f - Mathf.Abs(h * 6f - 2f)), 
            Mathf.Clamp01(2f - Mathf.Abs(h * 6f - 4f))
            );
    }

    public static Color ToColor(HSV hsv)
    {
        return ((((Hue(hsv.h) - Vector3.one) * hsv.s) + Vector3.one) * hsv.v).castc3(hsv.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " V:" + v + " A:" + a;
    }

    public static HSV Lerp(HSV a, HSV b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.v == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.v == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                // works around bug with LerpAngle
                float angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }
            s = Mathf.Lerp(a.s, b.s, t);
        }
        return new HSV(h, s, Mathf.Lerp(a.v, b.v, t), Mathf.Lerp(a.a, b.a, t));
    }

    public static void Test()
    {
        HSV color;

        color = new HSV(Color.red);
        Debug.Log("red: " + color);
        Debug.Log("red: " + color.ToColor());

        color = new HSV(Color.green);
        Debug.Log("green: " + color);
        Debug.Log("green: " + color.ToColor());

        color = new HSV(Color.blue);
        Debug.Log("blue: " + color);
        Debug.Log("blue: " + color.ToColor());

        color = new HSV(Color.grey);
        Debug.Log("grey: " + color);
        Debug.Log("grey: " + color.ToColor());

        color = new HSV(Color.white);
        Debug.Log("white: " + color);
        Debug.Log("white: " + color.ToColor());

        color = new HSV(new Color(0.4f, 1f, 0.84f, 1f));
        Debug.Log("0.4, 1f, 0.84: " + color);
        Debug.Log("0.4, 1f, 0.84: " + color.ToColor());

        Debug.Log("164,82,84   .... 0.643137f, 0.321568f, 0.329411f  :" + ToColor(new HSV(new Color(0.643137f, 0.321568f, 0.329411f))));
    }
}
