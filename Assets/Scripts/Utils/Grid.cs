using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid
{

    private List<List<GridCell>> grid;

    public int Width
    {
        get;
        private set;
    }

    public int Height
    {
        get;
        private set;
    }

    public Grid(int w, int h, GridCell fill)
    {
        grid = new List<List<GridCell>>();
        Width = w;
        Height = h;

        for (int x = 0; x < w; x++)
        {
            List<GridCell> column = new List<GridCell>(h);
            grid.Add(column);

            for (int y = 0; y < h; y++)
            {
                column.Add(fill.Clone(this, x, y));
            }
        }
    }

    public Point2 ToMapCoordinates(Vector3 p)
    {
        return new Point2(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y));
    }

    public Point2 ToMapCoordinates(float x, float y)
    {
        return new Point2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }

    public GridCell this[Point2 p]
    {
        get
        {
            return Get(p);
        }
        set
        {
            grid[p.x][p.y] = value.Clone(this, p.x, p.y);
        }
    }

    public GridCell this[int x, int y]
    {
        get
        {
            return Get(x, y);
        }
        set
        {
            grid[x][y] = value.Clone(this, x, y);
        }
    }

    public GridCell Get(Point2 p)
    {
        return grid[p.x][p.y];
    }

    public GridCell Get(int x, int y)
    {
        return grid[x][y];
    }

    public bool IsValid(Point2 p)
    {
        return IsValid(p.x, p.y);
    }

    public bool IsValid(int x, int y)
    {
        return 0 <= x && x < Width && 0 <= y && y < Height;
    }

    public GridArea Area(int centerX, int centerY, int radius)
    {
        return new GridArea(this, centerX - radius, centerY - radius, radius * 2 + 1, radius * 2 + 1);
    }

    public GridArea Area(int x, int y, int w, int h)
    {
        return new GridArea(this, x, y, w, h);
    }

    public IEnumerator GetEnumerator()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                yield return grid[x][y];
            }
        }
    }

    public class GridArea
    {
        Grid g;
        int minX, minY, maxX, maxY;

        public GridArea(Grid g, int x, int y, int w, int h)
        {
            this.g = g;
            this.minX = x;
            this.minY = y;
            this.maxX = x + w;
            this.maxY = y + h;
        }

        public IEnumerator GetEnumerator()
        {
            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    if (g.IsValid(x, y))
                    {
                        yield return g.grid[x][y];
                    }
                }
            }
        }
    }
}

public abstract class GridCell
{
    private Grid g;
    private int x;
    private int y;

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    }

    public Vector2 Vector
    {
        get
        {
            return new Vector2(x, y);
        }
    }

    public Point2 Point
    {
        get
        {
            return new Point2(x, y);
        }
    }

    public GridCell GetLeft()
    {
        return g[new Point2((x + g.Width - 1) % g.Width, y)];
    }

    public GridCell GetRight()
    {
        return g[new Point2((x + 1) % g.Width, y)];
    }

    public GridCell GetTop()
    {
        return g[new Point2(x, (y + 1) % g.Height)];
    }

    public GridCell GetBottom()
    {
        return g[new Point2(x, (y + g.Height - 1) % g.Height)];
    }

    public GridCell Clone(Grid g, int x, int y)
    {
        GridCell cloned = clone();
        cloned.g = g;
        cloned.x = x;
        cloned.y = y;
        return cloned;
    }

    abstract protected GridCell clone();
}