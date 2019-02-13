using RogueSharp;
using LearningtoRoguelike.Core;

namespace LearningtoRoguelike.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;

        //Constructing a new MapGenerator requires the dimensions of the map it will create
        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }
        //Simple map plan
        public DungeonMap CreateMap()
        {
            //Initialize every cell
            //See in sight, walkable, explored, etc.
            _map.Initialize(_width, _height);
            foreach (Cell cell in _map.GetAllCells() )
            {
                _map.SetCellProperties(cell.X, cell.Y, false, true, true);
            }
            //Set the first and last rows in the map to not be transparent or walkable
            foreach (Cell cell in _map.GetCellsInRows( 0, _height -1 ) )
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            //Set the first and last columns in the map to not be transparent or walkable
            foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }
    }
}
