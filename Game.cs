using RLNET;
using LearningtoRoguelike.Core;
using LearningtoRoguelike.Systems;

namespace LearningtoRoguelike
{
    public static class Game
    {
        //This is the screen height and width in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        //The map console is the center
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        //Message console below map console
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        //Stat console to the right of the map
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        //Inventory console
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;
        private static DungeonMap dungeonMap;

        //Adds renderRequired and a new property, CommandSystem
        private static bool _renderRequired = true;
        public static CommandSystem CommandSystem
        {
            get;
            private set;
        }

        public static Player Player { get; private set; }
        public static DungeonMap DungeonMap { get; private set; }

        private static void SetDungeonMap(DungeonMap value) { dungeonMap = value; }

        public static void Main()
        {
            //font file we're using, this is in my file
            string fontFileName = "terminal8x8.png";
            string consoleTitle = "RogueSharp V3 Tutorial - Level 1";

            //Initialize the sub consoles that will attach to root
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            Player = new Player();
            //Use map generator to generate dungeon
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

            //Instantiate CommandSystem();
            CommandSystem = new CommandSystem();

            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;
            //Set background color and text for each console
            //Used to verify correct positions
            //Map console color for text and background
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

            //Message console color for text and background
            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);

            //Stat console color for text and background
            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            _statConsole.Print(1, 1, "Stats", Colors.TextHeading);

            //Inventory console color for text and background
            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);
            _rootConsole.Run();

        }
        //This is the event handler for the Update Event of RLNET
        private static void OnRootConsoleUpdate( object sender, UpdateEventArgs e )
        {
            //Determines player keyboard input using rootConsole
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            //Reads and commands the player movement based on keyboard input
            if ( keyPress != null)
            {
                if ( keyPress.Key == RLKey.Up )
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                }
                else if (keyPress.Key == RLKey.Down)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                }
                else if ( keyPress.Key == RLKey.Left)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                }
                else if ( keyPress.Key == RLKey.Right)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                }
                else if ( keyPress.Key == RLKey.Escape)
                {
                    _rootConsole.Close();
                }
            }
            if (didPlayerAct)
            {
                _renderRequired = true;
            }
        }
        private static void OnRootConsoleRender( object sender, UpdateEventArgs e )
        {
            if (_renderRequired)
            {
                DungeonMap.Draw(_mapConsole);
                Player.Draw(_mapConsole, DungeonMap);
                //Blit the sub consoles to the root in the right spot   
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                //Tells RLNet to draw the console we set
                _rootConsole.Draw();

                _renderRequired = false;
            }
        }
    }
}
