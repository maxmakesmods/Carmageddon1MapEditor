using Carmageddon1MapEditor.Parsing;
using Carmageddon1MapEditor.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Collections.Generic;
using System.IO;

namespace Carmageddon1MapEditor.Main
{
    public class Carmageddon1MapEditorMain : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly List<EditorView> editorViews = [];
        private CarmDatParser datParser;
        private CarmMatParser matParser;
        private CarmPixParser pixParser;
        private CarmPixParser palParser;

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
            string tempDebugPalettePath = "E:\\SteamLibrary\\steamapps\\common\\Carmageddon1\\CARMA\\DATA\\REG\\PALETTES\\";

            string tempDebugModelPath1 = tempDebugModelPath + "SCREW2.DAT";
            string tempDebugModelPath2 = tempDebugModelPath + "ANNIEX.DAT";
            string tempDebugModelPath3 = tempDebugModelPath + "NETDESRT.DAT";

            string tempDebugMaterialPath1 = tempDebugMaterialPath + "DESERT8.MAT";

            string tempDebugPixelmapPath1 = tempDebugPixelmapPath + "DESERT8.PIX";

            string tempDebugPalettePath1 = tempDebugPalettePath + "DRRENDER.PAL";


            palParser = new CarmPixParser(File.ReadAllBytes(tempDebugPalettePath1));
            pixParser = new CarmPixParser(File.ReadAllBytes(tempDebugPixelmapPath1));
            matParser = new CarmMatParser(File.ReadAllBytes(tempDebugMaterialPath1));
            datParser = new CarmDatParser(File.ReadAllBytes(tempDebugModelPath3));

            editorViews.Add(new EditorView(new Camera3D(), new RectangleF(0, 0, 0.5f, 0.5f)));
            //editorViews.Add(new EditorView(new Camera2D(), new RectangleF(0.5f, 0, 0.5f, 0.5f)));
            //editorViews.Add(new EditorView(new Camera2D(), new RectangleF(0, 0.5f, 0.5f, 0.5f)));
            //editorViews.Add(new EditorView(new Camera2D(), new RectangleF(0.5f, 0.5f, 0.5f, 0.5f)));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var editorView in editorViews)
            {
                editorView.Update(GraphicsDevice, GraphicsDevice.Viewport, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var editorView in editorViews)
            {
                Renderer.Render(GraphicsDevice, editorView, datParser.meshes, matParser.materials, pixParser.pixelmaps, palParser.pixelmaps);
            }


            /*
            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();
            foreach (var editorView in editorViews)
            {
                spriteBatch.Draw(editorView.RenderTarget, editorView.Viewport, Color.White);
            }
            spriteBatch.End();
            */


            base.Draw(gameTime);
        }
    }
}
