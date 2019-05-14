using System;
using System.Collections.Generic;
using System.Text;

namespace EsHelper
{
    public class HttpLog<T1, T2>
    {
        public string ConverSationId { get; set; }

        public DateTime SentTime { get; set; }

        public string Version { get; set; }

        public DateTime TimeStamp { get; set; }

        public T1 Message { get; set; }

        public T2 Host { get; set; }

        public string[] MessageType { get; set; }

        public string Type { get; set; }

        public string CorrelationId { get; set; }

        public string MessageId { get; set; }

        public string SourceAddress { get; set; }

        public string DestinationAddress { get; set; }
    }

    public class Message
    {
        public DateTime TimeRequest { get; set; }

        public string GuidRequest { get; set; }

        public string RemoteAddress { get; set; }

        public string Uri { get; set; }

        public string CorrelationId { get; set; }

        public string Headers { get; set; }

        public string RequestBody { get; set; }

        public string ForwardedAddress { get; set; }

        public string ResponseBody { get; set; }

        public int ElapsedMilliseconds { get; set; }

    }

    public class Host
    {
        public int ProcessId { get; set; }

        public string MachineName { get; set; }

        public string ProcessName { get; set; }

        public string AssemblyVersion { get; set; }

        public string MassTransitVersion { get; set; }

        public string OperatingSystemVersion { get; set; }

        public string Assembly { get; set; }

        public string FrameworkVersion{ get; set; }
    }
}
