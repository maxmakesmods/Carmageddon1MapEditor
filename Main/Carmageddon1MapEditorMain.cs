using Carmageddon1MapEditor.Parsing;
using Carmageddon1MapEditor.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Carmageddon1MapEditor.Main
{
    public class Carmageddon1MapEditorMain : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Carmageddon1MapEditorMain()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            string tempDebugPath = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\MODELS\\SCREW2.DAT";

            CarmDatParser parser = new CarmDatParser(File.ReadAllBytes(tempDebugPath));

            Debug.WriteLine(parser.name);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
