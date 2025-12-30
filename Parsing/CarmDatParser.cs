
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Carmageddon1MapEditor.Parsing
{
    public class CarmDatParser : CarmBaseParser
    {
        private static readonly byte[] fileheader = { 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0xFA, 0xCE, 0x00, 0x00, 0x00, 0x02 };

        protected override byte[] Fileheader => fileheader;

        private const int BlockTypeVertexPositions = 23;
        private const int BlockTypeVertexUVs = 24;
        private const int BlockTypeIndices = 53;
        private const int BlockTypeMeshHeader = 54;
        private const int BlockTypeMaterial = 22;
        private const int BlockTypeFaceMatIndices = 26;

        public readonly List<CarmaMesh> meshes = [];

        public CarmDatParser(byte[] rawdata)
            : base(rawdata)
        {
        }

        protected override bool ParseBlock()
        {
            if (currentoffset >= rawdata.Length)
            {
                return false;
            }

            int blockType = ParseInt32();
            int size = ParseInt32();

            //Debug.WriteLine($"block {blockType}, size {size}, offset {currentoffset}");

            if (size == 0)
            {
                if (blockType != 0)
                {
                    throw new ArgumentException($"block with zero size: {blockType}");
                }
                if (currentoffset != rawdata.Length)
                {
                    //throw new ArgumentException();
                    //Debug.WriteLine("unexpected end of data before end of file");
                }
                //return false;
            }

            switch (blockType)
            {
                case BlockTypeVertexPositions:
                    ParseVertexPositions(size);
                    break;
                case BlockTypeVertexUVs:
                    ParseVertexUVs(size);
                    break;
                case BlockTypeIndices:
                    ParseFaces(size);
                    break;
                case BlockTypeMeshHeader:
                    ParseMeshHeader(size);
                    break;
                case BlockTypeMaterial:
                    ParseMaterial(size);
                    break;
                case BlockTypeFaceMatIndices:
                    ParseMatIndices(size);
                    break;
                default:
                    //Debug.WriteLine($"unknown block type: {blockType}");
                    SkipBlock(size);
                    break;
            }

            return true;
        }

        private void ParseMatIndices(int size)
        {
            int numFaces = ParseInt32();
            int numBytesPerFace = ParseInt32();
            if ((size - sizeof(int) - sizeof(int)) != numFaces * numBytesPerFace)
            {
                throw new ArgumentException($"invalid MatIndices block!");
            }
            for (int i = 0; i < numFaces; i++)
            {
                var faceMatIndex = numBytesPerFace switch
                {
                    1 => ParseByte(),
                    2 => ParseInt16(),
                    4 => ParseInt32(),
                    _ => throw new ArgumentException($"invalid numBytesPerFace: {numBytesPerFace}"),
                };
                meshes[^1].faces[i].matIndex = faceMatIndex - 1;
            }
        }

        private void ParseMaterial(int size)
        {
            int numMaterials = ParseInt32();
            for (int i = 0; i < numMaterials; i++)
            {
                meshes[^1].materialNames.Add(ParseString());
            }
            int totalStringLength = meshes[^1].materialNames.Select(s => s.Length).Aggregate((l1, l2) => l1 + l2 + 1) + 1;
            if (size != totalStringLength + sizeof(int))
            {
                throw new ArgumentException($"invalid material block!");
            }
        }

        private void ParseMeshHeader(int size)
        {
            short unknown = ParseInt16();
            string name = ParseString();
            if (name.Length + 1 + sizeof(short) != size)
            {
                throw new ArgumentException($"invalid mesh header!");
            }
            meshes.Add(new CarmaMesh(unknown, name));
        }

        private void ParseFaces(int size)
        {
            int numFaces = ParseInt32();
            if (numFaces * (3 * sizeof(short) + sizeof(short) + sizeof(byte)) != (size - sizeof(int)))
            {
                throw new ArgumentException("face size mismatch");
            }
            for (int i = 0; i < numFaces; i++)
            {
                meshes[^1].faces.Add(new()
                {
                    indices = new(ParseInt16(), ParseInt16(), ParseInt16()),
                    unknownbitflag = ParseInt16(),
                    unknown = ParseByte()
                });
            }
        }

        private void ParseVertexUVs(int size)
        {
            int numVertices = ParseInt32();
            if (numVertices * 2 * sizeof(float) != (size - sizeof(int)))
            {
                throw new ArgumentException("vertex size mismatch");
            }
            for (int i = 0; i < numVertices; i++)
            {
                meshes[^1].vertexUVs.Add(new(ParseFloat(), ParseFloat()));
            }
        }

        private void ParseVertexPositions(int size)
        {
            int numVertices = ParseInt32();
            if (numVertices * 3 * sizeof(float) != (size - sizeof(int)))
            {
                throw new ArgumentException("vertex size mismatch");
            }
            for (int i = 0; i < numVertices; i++)
            {
                meshes[^1].vertexPositions.Add(new(ParseFloat(), ParseFloat(), ParseFloat()));
            }
        }
    }
}
