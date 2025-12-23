using Carmageddon1MapEditor.Parsing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Carmageddon1MapEditor.Rendering
{
    internal class Renderer
    {
        public static void Render(
            GraphicsDevice graphicsDevice,
            Camera3D camera,
            RenderTarget2D target,
            List<Vector3> vertices,
            List<CarmaFace> faces)
        {
            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(Color.Pink);
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.BlendState = BlendState.Opaque;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = rasterizerState;

            // TODO: Effect

            BasicEffect effect = new BasicEffect(graphicsDevice);

            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;

            List<short> indices = new();
            foreach (CarmaFace face in faces)
            {
                indices.Add(face.indices.x);
                indices.Add(face.indices.y);
                indices.Add(face.indices.z);
            }

            VertexPositionColor[] triangleVerticesRed = new VertexPositionColor[indices.Count];
            for (int i = 0; i < triangleVerticesRed.Length; i++)
            {
                triangleVerticesRed[i].Position = vertices[indices[i]];
                triangleVerticesRed[i].Color = Color.BlanchedAlmond;
            }

            VertexPositionColor[] verticesBlack = new VertexPositionColor[vertices.Count];
            for (int i = 0; i < verticesBlack.Length; i++)
            {
                verticesBlack[i].Position = vertices[i];
                verticesBlack[i].Color = Color.Black;
            }


            List<short> lines = new();
            foreach (CarmaFace face in faces)
            {
                lines.Add(face.indices.x);
                lines.Add(face.indices.y);
                lines.Add(face.indices.y);
                lines.Add(face.indices.z);
                lines.Add(face.indices.z);
                lines.Add(face.indices.x);
            }


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList,
                    triangleVerticesRed,
                    0,
                    indices.Count / 3,
                    VertexPositionColor.VertexDeclaration
                );

                graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList,
                    verticesBlack,
                    0,
                    verticesBlack.Length,
                    lines.ToArray(),
                    0,
                    lines.Count / 2,
                    VertexPositionColor.VertexDeclaration
                );
            }

            graphicsDevice.SetRenderTarget(null);
        }
    }
}
