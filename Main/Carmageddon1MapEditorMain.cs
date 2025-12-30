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
        private CarmDatParser datParser;
        private CarmMatParser matParser;
        private CarmPixParser pixParser;

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

            string tempDebugModelPath = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\MODELS\\";
            string tempDebugMaterialPath = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\MATERIAL\\";
            string tempDebugPixelmapPath = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\PIXELMAP\\";

            string tempDebugModelPath1 = tempDebugModelPath + "SCREW2.DAT";
            string tempDebugModelPath2 = tempDebugModelPath + "ANNIEX.DAT";
            string tempDebugModelPath3 = tempDebugModelPath + "NETDESRT.DAT";

            string tempDebugMaterialPath1 = tempDebugMaterialPath + "DESERT8.MAT";

            string tempDebugPixelmapPath1 = tempDebugPixelmapPath + "DESERT8.PIX";


            pixParser = new CarmPixParser(File.ReadAllBytes(tempDebugPixelmapPath1));
            matParser = new CarmMatParser(File.ReadAllBytes(tempDebugMaterialPath1));
            datParser = new CarmDatParser(File.ReadAllBytes(tempDebugModelPath3));

            camera3D = new Camera3D(datParser.meshes[0].vertexPositions.Aggregate((v1, v2) => v1+v2) / datParser.meshes[0].vertexPositions.Count);
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

            Renderer.Render(GraphicsDevice, camera3D, null, datParser.meshes, matParser.materials, pixParser.pixelmaps);

            base.Draw(gameTime);
        }
    }
}
