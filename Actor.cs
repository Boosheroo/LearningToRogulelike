using LearningtoRoguelike.Interfaces;
using RLNET;
using RogueSharp;

namespace LearningtoRoguelike.Core
{
    public class Actor : IActor, IDrawable
    {
        //IActor
        public string Name { get; set; }
        public int Awareness { get; set; }
        
        //IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(RLConsole console, IMap map)
        {
            //Don't draw actors in unexplored cells
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            //Only draw the actor with the color and symbol when they are in the field of view
            if ( map.IsInFov( X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                //When not in field of view just draw normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }
        }
    }
}
