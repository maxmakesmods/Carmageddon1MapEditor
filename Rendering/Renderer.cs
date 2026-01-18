using Carmageddon1MapEditor.Parsing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carmageddon1MapEditor.Rendering
{
    internal class Renderer
    {
        public static void Render(
            GraphicsDevice graphicsDevice,
            EditorView editorView,
            List<CarmaMesh> meshes,
            Dictionary<string, Carmaterial> materials,
            Dictionary<string, CarmaPixelmap> pixelmaps,
            Dictionary<string, CarmaPixelmap> palettes)
        {
            graphicsDevice.SetRenderTarget(editorView.RenderTarget);
            graphicsDevice.Clear(Color.Pink);
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.BlendState = BlendState.Opaque;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = rasterizerState;

            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // TODO: Effect
            BasicEffect effect = new BasicEffect(graphicsDevice);

            effect.View = editorView.Camera.View;
            effect.Projection = editorView.Camera.Projection;
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = true;

            foreach (CarmaMesh mesh in meshes)
            {
                Render(graphicsDevice, editorView, effect, mesh, materials, pixelmaps, palettes);
            }

            graphicsDevice.SetRenderTarget(null);
        }

        private static void Render(
            GraphicsDevice graphicsDevice,
            EditorView editorView,
            BasicEffect effect,
            CarmaMesh mesh,
            Dictionary<string, Carmaterial> materials,
            Dictionary<string, CarmaPixelmap> pixelmaps,
            Dictionary<string, CarmaPixelmap> palettes)
        {
            foreach (CarmaFace face in mesh.faces)
            {
                Render(graphicsDevice, editorView, effect, mesh, face, materials, pixelmaps, palettes);
            }
        }

        private static void Render(
            GraphicsDevice graphicsDevice,
            EditorView editorView,
            BasicEffect effect,
            CarmaMesh mesh,
            CarmaFace face,
            Dictionary<string, Carmaterial> materials,
            Dictionary<string, CarmaPixelmap> pixelmaps,
            Dictionary<string, CarmaPixelmap> palettes)
        {
            effect.Texture = GetTempTestTexture(graphicsDevice);

            // triangles
            if (editorView.Mode == EditorView.ViewMode.Textured)
            {
                // TODO: get palette by name?
                CarmaPixelmap palette = palettes.Values.FirstOrDefault();
                Texture2D paletteTexture = palette.GetTexture(graphicsDevice, null);

                if (face.matIndex >= 0 && face.matIndex < mesh.materialNames.Count)
                {
                    string materialName = mesh.materialNames[face.matIndex];
                    if (materialName != null && materials.TryGetValue(materialName, out Carmaterial material))
                    {
                        if (material.pixName != null && pixelmaps.TryGetValue(material.pixName, out CarmaPixelmap pixelmap))
                        {
                            effect.Texture = pixelmap.GetTexture(graphicsDevice, paletteTexture);
                        }
                    }
                }
                short[] indices = [face.indices.x, face.indices.y, face.indices.z];
                VertexPositionColorTexture[] triangleVertices = new VertexPositionColorTexture[indices.Length];
                for (int i = 0; i < triangleVertices.Length; i++)
                {
                    triangleVertices[i].Position = mesh.vertexPositions[indices[i]];
                    triangleVertices[i].Color = Color.White;
                    triangleVertices[i].TextureCoordinate = mesh.vertexUVs[indices[i]];
                }
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleVertices, 0, 1, VertexPositionColorTexture.VertexDeclaration);
                }
            }

            // lines
            if (editorView.Mode == EditorView.ViewMode.Wireframe)
            {
                VertexPositionColor[] lineVertices = new VertexPositionColor[6];
                for (int i = 0; i < lineVertices.Length; i++)
                {
                    lineVertices[i].Color = Color.Black;
                }
                lineVertices[0].Position = mesh.vertexPositions[face.indices.x];
                lineVertices[1].Position = mesh.vertexPositions[face.indices.y];
                lineVertices[2].Position = mesh.vertexPositions[face.indices.y];
                lineVertices[3].Position = mesh.vertexPositions[face.indices.z];
                lineVertices[4].Position = mesh.vertexPositions[face.indices.z];
                lineVertices[5].Position = mesh.vertexPositions[face.indices.x];
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, lineVertices, 0, 3, VertexPositionColor.VertexDeclaration);
                }
            }
        }






        private static Texture2D tempTestTexture = null;
        private static Texture2D GetTempTestTexture(GraphicsDevice graphicsDevice)
        {
            if (tempTestTexture == null)
            {
                tempTestTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
                tempTestTexture.SetData(new byte[4] { 255, 255, 255, 255 });
            }
            return tempTestTexture;
        }
    }
}
