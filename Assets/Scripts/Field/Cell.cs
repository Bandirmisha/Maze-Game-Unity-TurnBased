
public struct Cell
{
    public Cell(int x, int y, CellType ct)
    {
        X = x;
        Y = y;
        type = ct;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public CellType type { get; set; }
}