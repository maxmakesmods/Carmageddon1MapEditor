
using System;
using System.Collections.Generic;

namespace Carmageddon1MapEditor.Parsing
{
    public class CarmPixParser : CarmBaseParser
    {
        private static readonly byte[] fileheader = { 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02 };


        private const int BlockTypePixelMap = 3;
        private const int BlockTypePixelData = 33;

        protected override byte[] Fileheader => fileheader;

        public Dictionary<string, CarmaPixelmap> pixelmaps = [];
        private CarmaPixelmap currentPixelmap = null;

        public CarmPixParser(byte[] rawdata)
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
                case BlockTypePixelMap:
                    ParsePixelMap(size);
                    break;
                case BlockTypePixelData:
                    ParsePixelData(size);
                    break;
                default:
                    //Debug.WriteLine($"unknown block type: {blockType}");
                    SkipBlock(size);
                    break;
            }

            return true;
        }

        private void ParsePixelMap(int size)
        {
            currentPixelmap = new()
            {
                unknown1 = ParseByte(),
                unknown2 = ParseInt16(),
                assumedWidth = ParseInt16(),
                assumedHeight = ParseInt16(),
                assumedHalfWidth = ParseInt16(),
                assumedHalfHeight = ParseInt16(),
                name = ParseString()
            };
            if (size != currentPixelmap.name.Length + 1 + 5 * sizeof(short) + sizeof(byte))
            {
                throw new ArgumentException($"invalid PixelMap block!");
            }
            pixelmaps.Add(currentPixelmap.name, currentPixelmap);
        }

        private void ParsePixelData(int size)
        {
            int numPixels = ParseInt32();
            int bytesPerPixel = ParseInt32();
            int pixelDataSize = numPixels * bytesPerPixel;
            if (size != sizeof(int) + sizeof(int) + pixelDataSize)
            {
                throw new ArgumentException($"invalid PixelData block!");
            }
            currentPixelmap.bytesPerPixel = bytesPerPixel;
            currentPixelmap.pixelData = CopyData(pixelDataSize);
        }
    }
}
