
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Carmageddon1MapEditor.Rendering
{
    internal class EditorView
    {
        public Camera Camera { get; set; }

        public RenderTarget2D RenderTarget { get; set; }

        private Viewport realViewport;

        private readonly RectangleF relativeViewport;

        public Rectangle Viewport => new RectangleF(
                relativeViewport.X * realViewport.Width,
                relativeViewport.Y * realViewport.Height,
                relativeViewport.Width * realViewport.Width,
                relativeViewport.Height * realViewport.Height
            ).ToRectangle();

        public EditorView(Camera camera, RectangleF relativeViewport)
        {
            Camera = camera;
            this.relativeViewport = relativeViewport;
        }

        public void Update(GraphicsDevice graphicsDevice, Viewport viewport, float deltaTime)
        {
            realViewport = viewport;
            RecreateRenderTargetIfNecessary(graphicsDevice);
            Camera.Update(Viewport, deltaTime);
        }

        private void RecreateRenderTargetIfNecessary(GraphicsDevice graphicsDevice)
        {
            if (RenderTarget == null || Viewport.Width != RenderTarget.Width || Viewport.Height != RenderTarget.Height)
            {
                RenderTarget = new RenderTarget2D(graphicsDevice,
                Viewport.Width, Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0, RenderTargetUsage.DiscardContents, false); 
                //new RenderTarget2D(graphicsDevice, Viewport.Width, Viewport.Height);
            }
        }
    }
}
