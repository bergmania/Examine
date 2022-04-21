using System;
using System.IO;
using System.Security;
using Lucene.Net.Store;

namespace Examine.Lucene.Directories
{
    /// <summary>
    /// Stream wrapper around IndexInput
    /// </summary>
    
    public class StreamInput : Stream
    {
        public IndexInput Input { get; set; }

        public StreamInput(IndexInput input) => Input = input;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override void Flush() { }

        public override long Length => Input.Length;

        public override long Position
        {
            get => Input.Position;
            set => Input.Seek(value);
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            var pos = Input.Position;
            //try
            //{
                var len = Input.Length;
                if (count > (len - pos))
            {
                count = (int)(len - pos);
            }

            Input.ReadBytes(buffer, offset, count);
            //}
            //catch (Exception) { }
            return (int)(Input.Position - pos);
        }

        
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Input.Seek(offset);
                    break;
                case SeekOrigin.Current:
                    Input.Seek(Input.Position + offset);
                    break;
                case SeekOrigin.End:
                    throw new System.NotImplementedException();
            }
            return Input.Position;
        }

        public override void SetLength(long value) => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();


        public override void Close()
        {
            Input.Dispose();
            base.Close();
        }
    }
}
