using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BBS
{
    /// <summary>
    /// 파일 서비스 인터페이스
    /// </summary>
    [ServiceContract(Namespace = "http://nakdong.wcf.service")]
    public interface IFileService
    {
        [OperationContract]
        UploadResponse UploadFile(UploadRequest request);

        [OperationContract]
        DownloadResponse Download(DownloadRequest request);

        [OperationContract]
        CheckFileResponse SeverCheckFile(string fileName);

        //[OperationContract]
        //string  GetFilePath(string fileName);

    }

    /// <summary>
    /// Stream MessageBodyMember이 있을 경우 한개만 bodyMember가능
    /// </summary>
    [MessageContract]
    public class UploadRequest : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public string FileVersion { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public bool CheckVersion;

        [MessageBodyMember]
        public Stream MyStream { get; set; }

        public void Dispose()
        {
            if (MyStream == null) return;

            MyStream.Close();
            MyStream = null;
        }
    }

    [MessageContract]
    public class UploadResponse
    {
        [MessageBodyMember(Order = 0)]
        public string ReturnCD { get; set; } // "FAIL", "OK"
        [MessageBodyMember(Order = 1)]
        public string ReturnMsg { get; set; }
    }
    [DataContract]
    public class CheckFileResponse
    {
        [DataMember(Order = 0)]
        public bool FileExists { get; set; }

        [DataMember(Order = 1)]
        public string FileVersion { get; set; }
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember(Order = 0)]
        public string FileName { get; set; }

        [MessageBodyMember(Order = 1)]
        public string FileVersion { get; set; }

        [MessageBodyMember(Order = 2)]
        public bool CheckVersion;
    }


    [MessageContract]
    public class DownloadResponse
    {

        [MessageHeader]
        public string ReturnCD { get; set; } // "FAIL", "OK"
        [MessageHeader]
        public string ReturnMsg { get; set; }
        [MessageHeader]
        public string FileVersion { get; set; }
        [MessageHeader]
        public long FileLength { get; set; }
        [MessageBodyMember]
        public Stream MyStream { get; set; }
    }



}
