using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Ara3D.Serialization.BFAST
{
    /// <summary>
    /// Wraps an array of byte buffers encoding a BFast structure and provides validation and safe access to the memory. 
    /// The BFAST file/data format is structured as follows:
    ///   * File header   - Fixed size file descriptor
    ///   * Ranges        - An array of pairs of offsets that point to the begin and end of each data arrays
    ///   * Array data    - All of the array data is contained in this section.
    /// </summary>
    public static class BFast
    {
        /// <summary>
        /// Given a position in the stream, tells us where the the next aligned position will be, if it the current position is not aligned.
        /// </summary>
        public static long ComputeNextAlignment(long n)
            => IsAligned(n) ? n : n + Constants.ALIGNMENT - (n % Constants.ALIGNMENT);

        /// <summary>
        /// Given a position in the stream, computes how much padding is required to bring the value to an aligned point. 
        /// </summary>
        public static long ComputePadding(long n)
            => ComputeNextAlignment(n) - n;

        /// <summary>
        /// Computes the padding requires after the array of BFastRanges are written out. 
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public static long ComputePadding(BFastRange[] ranges)
            => ComputePadding(BFastPreamble.Size + ranges.Length * BFastRange.Size);

        /// <summary>
        /// Given a position in the stream, tells us whether the position is aligned.
        /// </summary>
        public static bool IsAligned(long n)
            => n % Constants.ALIGNMENT == 0;

        /// <summary>
        /// Writes n zero bytes.
        /// </summary>
        public static void WriteZeroBytes(this BinaryWriter bw, long n)
        {
            for (var i = 0L; i < n; ++i)
                bw.Write((byte)0);
        }

        /// <summary>
        /// Checks that the stream (if seekable) is well aligned
        /// </summary>
        public static void CheckAlignment(Stream stream)
        {
            if (!stream.CanSeek)
                return;
            // TODO: Check with CD: Should we bail out here?  This means that any
            // alignment checks for a currently-writing stream are effectively ignored.
            if (stream.Position == stream.Length)
                return;
            if (!IsAligned(stream.Position))
                throw new Exception($"Stream position {stream.Position} is not well aligned");
        }

        /// <summary>
        /// Converts a collection of strings, into a null-separated byte[] array 
        /// </summary>
        public static byte[] PackStrings(this IEnumerable<string> strings)
        {
            var r = new List<byte>();
            foreach (var name in strings)
            {
                var bytes = Encoding.UTF8.GetBytes(name);
                r.AddRange(bytes);
                r.Add(0);
            }

            return r.ToArray();
        }

        /// <summary>
        /// Converts a byte[] array encoding a collection of strings separate by NULL into an array of string   
        /// </summary>
        public static string[] UnpackStrings(this byte[] bytes)
        {
            var r = new List<string>();
            if (bytes.Length == 0)
                return r.ToArray();
            var prev = 0;
            for (var i = 0; i < bytes.Length; ++i)
            {
                if (bytes[i] == 0)
                {
                    r.Add(Encoding.UTF8.GetString(bytes, prev, i - prev));
                    prev = i + 1;
                }
            }

            if (prev < bytes.Length)
                r.Add(Encoding.UTF8.GetString(bytes, prev, bytes.Length - prev));
            return r.ToArray();
        }

        /// <summary>
        /// Creates a BFAST structure, without any actual data buffers, from a list of sizes of buffers (not counting the name buffer). 
        /// Used as an intermediate step to create a BFAST. 
        /// </summary>
        public static BFastHeader CreateBFastHeader(this long[] bufferSizes, string[] bufferNames)
        {
            if (bufferNames.Length != bufferSizes.Length)
                throw new Exception(
                    $"The number of buffer sizes {bufferSizes.Length} is not equal to the number of buffer names {bufferNames.Length}");

            var header = new BFastHeader
            {
                Names = bufferNames
            };
            header.Preamble.Magic = Constants.Magic;
            header.Preamble.NumArrays = bufferSizes.Length + 1;

            // Allocate the data for the ranges
            header.Ranges = new BFastRange[header.Preamble.NumArrays];
            header.Preamble.DataStart = ComputeNextAlignment(header.Preamble.RangesEnd);

            var nameBufferLength = PackStrings(bufferNames).LongLength;
            var sizes = (new[] { nameBufferLength }).Concat(bufferSizes).ToArray();

            // Compute the offsets for the data buffers
            var curIndex = header.Preamble.DataStart;
            var i = 0;
            foreach (var size in sizes)
            {
                curIndex = ComputeNextAlignment(curIndex);
                Debug.Assert(IsAligned(curIndex));

                header.Ranges[i].Begin = curIndex;
                curIndex += size;

                header.Ranges[i].End = curIndex;
                i++;
            }

            // Finish with the header
            // Each buffer we contain is padded to ensure the next one
            // starts on alignment, so we pad our DataEnd to reflect this reality
            header.Preamble.DataEnd = ComputeNextAlignment(curIndex);

            // Check that everything adds up 
            return header.Validate();
        }

        /// <summary>
        /// Checks that the header values are sensible, and throws an exception otherwise.
        /// </summary>
        public static BFastPreamble Validate(this BFastPreamble header)
        {
            if (header.Magic != Constants.SameEndian && header.Magic != Constants.SwappedEndian)
                throw new Exception($"Invalid magic number {header.Magic}");

            if (header.DataStart < BFastPreamble.Size)
                throw new Exception(
                    $"Data start {header.DataStart} cannot be before the file header size {BFastPreamble.Size}");

            if (header.DataStart > header.DataEnd)
                throw new Exception($"Data start {header.DataStart} cannot be after the data end {header.DataEnd}");

            if (!IsAligned(header.DataEnd))
                throw new Exception($"Data end {header.DataEnd} should be aligned");

            if (header.NumArrays < 0)
                throw new Exception($"Number of arrays {header.NumArrays} is not a positive number");

            if (header.NumArrays > header.DataEnd)
                throw new Exception($"Number of arrays {header.NumArrays} can't be more than the total size");

            if (header.RangesEnd > header.DataStart)
                throw new Exception(
                    $"End of range {header.RangesEnd} can't be after data-start {header.DataStart}");

            return header;
        }

        public static bool IsValid(this BFastPreamble header)
        {
            if (header.Magic != Constants.SameEndian && header.Magic != Constants.SwappedEndian)
                return false;

            if (header.DataStart < BFastPreamble.Size)
                return false;

            if (header.DataStart > header.DataEnd)
                return false;

            if (!IsAligned(header.DataEnd))
                return false;

            if (header.NumArrays < 0)
                return false;

            if (header.NumArrays > header.DataEnd)
                return false;

            if (header.RangesEnd > header.DataStart)
                return false;

            return true;
        }

        /// <summary>
        /// Checks that the header values are sensible, and throws an exception otherwise.
        /// </summary>
        public static BFastHeader Validate(this BFastHeader header)
        {
            var preamble = header.Preamble.Validate();
            var ranges = header.Ranges;
            var names = header.Names;

            if (preamble.RangesEnd > preamble.DataStart)
                throw new Exception(
                    $"Computed arrays ranges end must be less than the start of data {preamble.DataStart}");

            if (ranges == null)
                throw new Exception("Ranges must not be null");

            var min = preamble.DataStart;
            var max = preamble.DataEnd;

            for (var i = 0; i < ranges.Length; ++i)
            {
                var begin = ranges[i].Begin;
                if (!IsAligned(begin))
                    throw new Exception($"The beginning of the range is not well aligned {begin}");
                var end = ranges[i].End;
                if (begin < min || begin > max)
                    throw new Exception($"Array offset begin {begin} is not in valid span of {min} to {max}");
                if (i > 0)
                {
                    if (begin < ranges[i - 1].End)
                        throw new Exception(
                            $"Array offset begin {begin} is overlapping with previous array {ranges[i - 1].End}");
                }

                if (end < begin || end > max)
                    throw new Exception($"Array offset end {end} is not in valid span of {begin} to {max}");
            }

            if (names.Length < ranges.Length - 1)
                throw new Exception(
                    $"Number of buffer names {names.Length} is not one less than the number of ranges {ranges.Length}");

            return header;
        }

        /// <summary>
        /// Writes the BFAST header and name buffer to stream using the provided BinaryWriter. The BinaryWriter will be properly aligned by padding zeros 
        /// </summary>
        public static BinaryWriter WriteBFastHeader(this Stream stream, BFastHeader header)
        {
            if (header.Ranges.Length != header.Names.Length + 1)
                throw new Exception(
                    $"The number of ranges {header.Ranges.Length} must be equal to one more than the number of names {header.Names.Length}");
            var bw = new BinaryWriter(stream);
            bw.Write(header.Preamble.Magic);
            bw.Write(header.Preamble.DataStart);
            bw.Write(header.Preamble.DataEnd);
            bw.Write(header.Preamble.NumArrays);
            foreach (var r in header.Ranges)
            {
                bw.Write(r.Begin);
                bw.Write(r.End);
            }

            WriteZeroBytes(bw, ComputePadding(header.Ranges));

            CheckAlignment(stream);
            var nameBuffer = PackStrings(header.Names);
            bw.Write(nameBuffer);
            WriteZeroBytes(bw, ComputePadding(nameBuffer.LongLength));

            CheckAlignment(stream);
            return bw;
        }

        public static bool IsBFast(this MemoryMappedView view)
        {
            if (view.Size < BFastPreamble.Size)
                return false;
            view.Accessor.Read(0, out BFastPreamble tmp);
            return tmp.IsValid();
        }

        public static T[] ReadBFastBuffers<T>(this MemoryMappedView view, Func<string, MemoryMappedView, T> f)
        {
            var reader = new BFastReader(view);
            var r = new T[reader.BufferNames.Length];
            reader.Read((name, subView, i) => r[i] = f(name, subView));
            return r;
        }
    }
}
