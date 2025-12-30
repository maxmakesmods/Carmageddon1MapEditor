using System;
using System.Linq;

namespace Carmageddon1MapEditor.Parsing
{
    public abstract class CarmBaseParser
    {
        protected readonly byte[] rawdata;
        protected int currentoffset;

        protected abstract byte[] Fileheader { get; }

        protected CarmBaseParser(byte[] rawdata)
        {
            this.rawdata = rawdata;

            if (rawdata.Length < Fileheader.Length)
            {
                throw new ArgumentException("invalid data");
            }
            if (!rawdata[..Fileheader.Length].SequenceEqual(Fileheader))
            {
                throw new ArgumentException("invalid data");
            }

            currentoffset = Fileheader.Length;

            while (ParseBlock());
        }

        protected abstract bool ParseBlock();

        protected void SkipBlock(int size)
        {
            currentoffset += size;
        }

        protected string ParseString()
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

        protected byte ParseByte()
        {
            byte value = rawdata[currentoffset];
            currentoffset += sizeof(byte);
            return value;
        }

        protected short ParseInt16()
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

        protected int ParseInt32()
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

        protected float ParseFloat()
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

        protected byte[] CopyData(int count)
        {
            if (currentoffset + count >= rawdata.Length)
            {
                throw new ArgumentException("CopyData: out of range");
            }
            byte[] copy = new byte[count];
            for (int i = 0; i < count; i ++)
            {
                copy[i] = rawdata[currentoffset];
                currentoffset++;
            }
            return copy;
        }
    }
}
