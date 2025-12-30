
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Carmageddon1MapEditor.Parsing
{
    public class CarmaPixelmap
    {
        public string name;
        public byte unknown1;
        public short unknown2;
        public short assumedWidth;
        public short assumedHeight;
        public short assumedHalfWidth;
        public short assumedHalfHeight;
        public int bytesPerPixel;
        public byte[] pixelData;

        private Texture2D texture;

        internal Texture2D GetTexture(GraphicsDevice graphicsDevice)
        {
            if (texture == null)
            {
                texture = new Texture2D(graphicsDevice, assumedWidth, assumedHeight, false, SurfaceFormat.Color);
                Color[] colors = new Color[pixelData.Length];
                for (int i = 0; i < pixelData.Length; i++)
                {
                    colors[i] = new Color(0, (int)pixelData[i], 0, 255);
                }
                texture.SetData(colors);
            }
            return texture;
        }
    }
}
