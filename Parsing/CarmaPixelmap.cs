
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

        internal Texture2D GetTexture(GraphicsDevice graphicsDevice, Texture2D paletteTexture)
        {
            if (texture == null)
            {
                if (bytesPerPixel == 4)
                {
                    // assume ARGB
                    texture = new Texture2D(graphicsDevice, assumedWidth, assumedHeight, false, SurfaceFormat.Color);
                    Color[] colors = new Color[assumedWidth * assumedHeight];
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = new Color(pixelData[i * 4 + 1], pixelData[i * 4 + 2], pixelData[i * 4 + 3], (byte)255);
                    }
                    texture.SetData(colors);
                }
                else if (bytesPerPixel == 3)
                {
                    // assume RGB
                    texture = new Texture2D(graphicsDevice, assumedWidth, assumedHeight, false, SurfaceFormat.Color);
                    Color[] colors = new Color[assumedWidth * assumedHeight];
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = new Color(pixelData[i * 3], pixelData[i * 3 + 1], pixelData[i * 3 + 2], (byte)255);
                    }
                    texture.SetData(colors);
                }
                else if (bytesPerPixel == 2)
                {
                    // TODO: 16bit color
                    throw new NotImplementedException("16bit color not implemented");
                }
                else if (bytesPerPixel == 1)
                {
                    texture = new Texture2D(graphicsDevice, assumedWidth, assumedHeight, false, SurfaceFormat.Color);
                    Color[] paletteColors = new Color[paletteTexture.Width * paletteTexture.Height];
                    paletteTexture.GetData(paletteColors);
                    Color[] colors = new Color[pixelData.Length];
                    for (int i = 0; i < pixelData.Length; i++)
                    {
                        colors[i] = paletteColors[pixelData[i]];
                    }
                    texture.SetData(colors);
                }
                else
                {
                    throw new ArgumentException($"invalid bytesPerPixel: {bytesPerPixel}");
                }
            }
            return texture;
        }
    }
}
