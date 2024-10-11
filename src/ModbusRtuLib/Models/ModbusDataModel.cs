using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Models
{
    public class ModbusDataModel
    {
        public ModbusMessageDataType Type { get; set; }

        public byte[] Bytes { get; set; }

        public byte[] String { get; set; }

        public Encoding Encoding { get; set; }
    }
}
