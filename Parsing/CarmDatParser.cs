
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Carmageddon1MapEditor.Parsing
{
    public class CarmDatParser
    {
        private static readonly byte[] fileheader = { 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0xFA, 0xCE, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x36 };

        private const int BlockTypeVertexPositions = 23;
        private const int BlockTypeVertexUVs = 24;
        private const int BlockTypeIndices = 53;

        private readonly byte[] rawdata;
        private int currentoffset;

        public readonly int filetype;
        public readonly short unknown;
        public readonly string name;

        public readonly List<Vector3> vertexPositions = [];
        public readonly List<Vector2> vertexUVs = [];
        public readonly List<CarmaFace> faces = [];

        public CarmDatParser(byte[] rawdata)
        {
            if (rawdata.Length < fileheader.Length)
            {
                throw new ArgumentException("invalid data");
            }
            if (!rawdata[..fileheader.Length].SequenceEqual(fileheader))
            {
                throw new ArgumentException("invalid data");
            }

            this.rawdata = rawdata;
            currentoffset = fileheader.Length;

            filetype = ParseInt32();
            unknown = ParseInt16();
            name = ParseString();

            while (ParseBlock()) ;
        }

        private bool ParseBlock()
        {
            if (currentoffset >= rawdata.Length)
            {
                return false;
            }

            int blockType = ParseInt32();
            int size = ParseInt32();
            if (size == 0)
            {
                if (blockType != 0)
                {
                    throw new ArgumentException($"block with zero size: {blockType}");
                }
                if (currentoffset != rawdata.Length)
                {
                    //throw new ArgumentException();
                    Debug.WriteLine("unexpected end of data before end of file");
                }
                return false;
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
                default:
                    Debug.WriteLine($"unknown block type: {blockType}");
                    SkipBlock(size);
                    break;
            }

            return true;
        }

        private void SkipBlock(int size)
        {
            currentoffset += size;
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
                faces.Add(new()
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
                vertexUVs.Add(new(ParseFloat(), ParseFloat()));
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
                vertexPositions.Add(new(ParseFloat(), ParseFloat(), ParseFloat()));
            }
        }

        private string ParseString()
        {
            int count = 0;
            while (rawdata[currentoffset + count] != 0)
            {
                count++;
            }
            string value = System.Text.Encoding.ASCII.GetString(rawdata, currentoffset, count);
            currentoffset += count + 1; // include delimiter zero
            return value;
        }

        private byte ParseByte()
        {
            byte value = rawdata[currentoffset];
            currentoffset += sizeof(byte);
            return value;
        }

        private short ParseInt16()
        {
            short value;
            if (BitConverter.IsLittleEndian)
            {
                byte[] endianbuffer = new byte[sizeof(short)];
                Array.Copy(rawdata, currentoffset, endianbuffer, 0, sizeof(short));
                Array.Reverse(endianbuffer);
                value = BitConverter.ToInt16(endianbuffer);
            }
            else
            {
                value = BitConverter.ToInt16(rawdata, currentoffset);
            }
            currentoffset += sizeof(short);
            return value;
        }

        private int ParseInt32()
        {
            int value;
            if (BitConverter.IsLittleEndian)
            {
                byte[] endianbuffer = new byte[sizeof(int)];
                Array.Copy(rawdata, currentoffset, endianbuffer, 0, sizeof(int));
                Array.Reverse(endianbuffer);
                value = BitConverter.ToInt32(endianbuffer);
            }
            else
            {
                value = BitConverter.ToInt32(rawdata, currentoffset);
            }
            currentoffset += sizeof(int);
            return value;
        }

        private float ParseFloat()
        {
            float value;
            if (BitConverter.IsLittleEndian)
            {
                byte[] endianbuffer = new byte[sizeof(float)];
                Array.Copy(rawdata, currentoffset, endianbuffer, 0, sizeof(float));
                Array.Reverse(endianbuffer);
                value = BitConverter.ToSingle(endianbuffer);
            }
            else
            {
                value = BitConverter.ToSingle(rawdata, currentoffset);
            }
            currentoffset += sizeof(float);
            return value;
        }

        private static void ParseVertices()
        {

        }
    }
}
