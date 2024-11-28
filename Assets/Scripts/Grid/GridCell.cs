namespace Match3
{
    /// <summary>
    /// The cell in which the object that the player interacts with is stored
    /// </summary>
    public class GridCell
    {
        public GridElement Value {  get; private set; }

        public void SetValue(GridElement value) => Value = value;
    }
}
