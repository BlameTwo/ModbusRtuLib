﻿using ModbusRtuLib.Contracts.Mitsubishi;

namespace ModbusRtuLib.Models.Handlers;

public delegate void ModbusConnectChanged(object sender, bool connect);

public delegate void MitsubishiQna3EConnectChanged(IMcNetSerialPort port, bool connect);
