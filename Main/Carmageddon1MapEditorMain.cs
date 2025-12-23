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

        private Camera3D camera3D;
        private CarmDatParser parser;

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

            string tempDebugPath1 = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\MODELS\\SCREW2.DAT";
            string tempDebugPath2 = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\MODELS\\ANNIEX.DAT";
            string tempDebugPath3 = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\MODELS\\NETDESRT.DAT";
            parser = new CarmDatParser(File.ReadAllBytes(tempDebugPath3));

            camera3D = new Camera3D(parser.vertexPositions.Aggregate((v1, v2) => v1+v2) / parser.vertexPositions.Count);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            camera3D.Update(GraphicsDevice.Viewport.Bounds, (float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Renderer.Render(GraphicsDevice, camera3D, null, parser.vertexPositions, parser.faces);

            base.Draw(gameTime);
        }
    }
}
