public class Cell
{
    public Cell(Placeable element)
    {
        Element = element;
    }
    
    public Cell()
    {
        Element = null;
    }

    public Placeable Element { get; set; }
}