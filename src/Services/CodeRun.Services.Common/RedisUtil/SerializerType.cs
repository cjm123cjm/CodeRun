using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRun.Services.Common.RedisUtil
{
    public enum SerializerType
    {
        Json,
        ProtoBuf
    }
    public class SuperObj
    {
        public bool HasValue { get; set; }
        public object? Value { get; set; }
    }
}
