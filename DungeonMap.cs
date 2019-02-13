using RogueSharp;
using RLNET;

//DungeonMap extends Map, which comes with RogueSharp


namespace LearningtoRoguelike.Core
{
    public class DungeonMap : Map
    {
        
        //This method will be called any time we move the player and update the field of view
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            //Compute the field of view based on the players position and awareness value
            ComputeFov(player.X, player.Y, player.Awareness, true);
            //Mark all cells in FOV as having been explored
            foreach (ICell cell in GetAllCells())
            {
                if (IsInFov( cell.X, cell.Y ))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }
        
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
        //This Boolean returns true when the actor is placed on a cell
        public bool SetActorPosition( Actor actor, int x, int y )
        {
            //Only put the actor where he can walk
            if (GetCell( x, y).IsWalkable)
            {
                //The cell the actor was on previously is now walkable
                SetIsWalkable(actor.X, actor.Y, true);
                //Update actor's position
                actor.X = x;
                actor.Y = y;
                //The new cell the actor is on is now not walkable
                SetIsWalkable(actor.X, actor.Y, false);
                if (actor is Player )
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }
        public void SetIsWalkable( int x, int y, bool isWalkable)
        {
            ICell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }
    }
}