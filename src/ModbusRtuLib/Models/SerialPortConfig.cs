using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Models
{
    public class SerialPortConfig
    {
        public string SerialPortName { get; set; }

        public int BaudRate { get; set; }

        public int DataBit { get; set; }

        public StopBits StopBit { get; set; }

        public Parity Parity { get; set; }

        public Handshake Handshake { get; set; }

        public DataFormat DataFormat { get; set; }

        public byte DefaultSlaveDevice { get; set; } = 1;
        public int ReadTimeSpan { get; set; }
        public int WriteTimeSpan { get; set; }
    }
}
