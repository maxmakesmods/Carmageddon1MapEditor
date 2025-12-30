
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Carmageddon1MapEditor.Parsing
{
    public class CarmMatParser : CarmBaseParser
    {
        private static readonly byte[] fileheader = { 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x02 };

        protected override byte[] Fileheader => fileheader;

        private const int BlockTypeMaterial = 4;
        private const int BlockTypePixName = 28;
        private const int BlockTypeTabName = 31;

        public readonly Dictionary<string, Carmaterial> materials = [];
        private Carmaterial currentMaterial = null;

        public CarmMatParser(byte[] rawdata)
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
                case BlockTypeMaterial:
                    ParseMaterial(size);
                    break;
                case BlockTypePixName:
                    ParsePixName(size);
                    break;
                case BlockTypeTabName:
                    ParseTabName(size);
                    break;
                default:
                    //Debug.WriteLine($"unknown block type: {blockType}");
                    SkipBlock(size);
                    break;
            }

            return true;
        }

        private void ParseMaterial(int size)
        {
            /*
        public int unknown1;
        public int assumedToBeFFFFFFFF;
        public float f1;
        public float f2;
        public float f3;
        public float f4;
        public int unknown2;
        public int assumedToBeZero1;
        public int assumedToBeZero2;
        public int unknown3;
        public int assumedToBeZero3;
        public int assumedToBeZero4;
        public int unknown4;
        public string materialName;
            */
            currentMaterial = new()
            {
                startoffset = currentoffset,
                unknown1 = size,
                assumedToBeFFFFFFFF = ParseInt32(),
                f1 = ParseFloat(),
                f2 = ParseFloat(),
                f3 = ParseFloat(),
                f4 = ParseFloat(),
                unknown2 = ParseInt32(),
                assumedToBeZero1 = ParseInt32(),
                assumedToBeZero2 = ParseInt32(),
                unknown3 = ParseInt32(),
                assumedToBeZero3 = ParseInt32(),
                assumedToBeZero4 = ParseInt32(),
                unknown4 = ParseInt32(),
                materialName = ParseString(),
                endoffset = currentoffset
            };
            currentMaterial.actualsize = currentMaterial.endoffset - currentMaterial.startoffset;
            materials.Add(currentMaterial.materialName, currentMaterial);
        }

        private void ParsePixName(int size)
        {
            currentMaterial.unknownPixSize = size;
            currentMaterial.pixName = ParseString();
        }

        private void ParseTabName(int size)
        {
            currentMaterial.unknownTabSize = size;
            currentMaterial.tabName = ParseString();
        }
    }
}
