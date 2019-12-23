using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVLint.Tools
{
    /// <summary>
    /// Read-only stream that can have several underlying streams, reading seamlessly from each one in order
    /// </summary>
    public class ConcatenatingStream : Stream
    {
        private readonly bool _closeStreams;
        private Stream _current;
        private IEnumerator<Stream> _iterator;
        private long _position;

        /// <summary>
        /// Creates a new ConcatenatingStream, using the <paramref name="sources"/> as underlying streams
        /// </summary><inheritdoc/>
        /// <param name="sources"> Streams to read from, in order. Enumerable will only be iterated when preceding stream is read to the end </param>
        /// <param name="closeStreams"><see langword="true" /> to close underlying streams when read to the end, <see langword="false" /> to leave streams open </param>
        public ConcatenatingStream(IEnumerable<Stream> sources, bool closeStreams)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));
            this._iterator = sources.GetEnumerator();
            this._closeStreams = closeStreams;
        }

        /// <summary>
        /// Creates a new <see cref="ConcatenatingStream"/> using a set of factory functions to create the underlying streams
        /// </summary>
        /// <param name="closeStreams"> <see langword="true" /> to close underlying streams when read to the end, <see langword="false" /> to leave streams open </param>
        /// <param name="factories"> Functions to create the underlying streams. The function will only be called after the previous stream has been read to the end </param>
        public static ConcatenatingStream FromFactoryFunctions<T>(bool closeStreams, params Func<T>[] factories)
            where T : Stream
        {
            return new ConcatenatingStream(factories.Select(factory => factory()), closeStreams);
        }

        private Stream NextStream()
        {
            if (this._closeStreams) this._current?.Dispose();

            this._current = null;
            if (this._iterator == null) throw new ObjectDisposedException(this.GetType().Name);
            if (this._iterator.MoveNext()) this._current = this._iterator.Current;
            return this._current;
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override bool CanSeek => false;
        public override bool CanTimeout => false;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => this._position;
            set => throw new NotSupportedException();
        }

        private void EndOfStream()
        {
            if (this._closeStreams) this._current?.Dispose();

            this._current = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._closeStreams)
                    do
                        this._iterator.Current?.Dispose(); while (this._iterator.MoveNext());

                this._iterator.Dispose();
                this._iterator = null;
                this._current = null;
            }

            base.Dispose(disposing);
        }

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override void WriteByte(byte value) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void Flush() { } // nothing to do
        

        public override int Read(byte[] buffer, int offset, int count)
        {
            var result = 0;
            while (count > 0)
            {
                var stream = this._current ?? this.NextStream();
                if (stream == null) break;
                var thisCount = stream.Read(buffer, offset, count);
                result += thisCount;
                count -= thisCount;
                offset += thisCount;
                if (thisCount == 0) this.EndOfStream();
            }

            this._position += result;
            return result;
        }
    }
}