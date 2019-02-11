using RogueSharp;
using RLNET;

//DungeonMap extends Map, which comes with RogueSharp


namespace LearningtoRoguelike.Core
{
    public class DungeonMap : Map
    {
        //Call draw when a math is update
        //Render all symbols/colors to the console
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }
        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            //When a cell isn't explored, don't draw anything
            if (!cell.IsExplored)
            {
                return;
            }

            //When a cell is in a field of view, use higher lights
            if (IsInFov(cell.X, cell.Y))
            {
                //Choose a symbol to draw based on if the cell is walkable or not
                //'.' for floor and '#' for walls
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }
    }
}