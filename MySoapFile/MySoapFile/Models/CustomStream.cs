using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySoapFile.Models
{
    // CustomStream Event Args
    public class ProgressChangedEventArgs : EventArgs
    {
        public long BytesRead;
        public long Length;

        public ProgressChangedEventArgs(long bytesRead, long length)
        {
            BytesRead = bytesRead;
            Length = length;
        }
    }

    /// <summary>
    /// 파일 전송 진행율을 보기 위해서 만듬
    /// 필요없어면 사용하지 않아도 됨
    /// </summary>
    public class CustomStream : Stream
    {
        private readonly Stream _file;
        private readonly long _length;
        private long _bytesRead;

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        public CustomStream(Stream fileStream, long fileLength)
        {
            _file = fileStream;
            //_length = _file.Length;
            _length = fileLength;
            _bytesRead = 0;
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new ProgressChangedEventArgs(_bytesRead, _length));
            }
        }


        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite { get { return false; } }

        public override long Length { get { return _length; } }

        public override long Position
        {
            get { return _bytesRead; }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = _file.Read(buffer, offset, count);
            _bytesRead += result;
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new ProgressChangedEventArgs(_bytesRead, _length));
            }
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

    } // END CustomStream

}
