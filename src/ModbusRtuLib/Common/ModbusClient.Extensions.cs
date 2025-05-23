﻿using System;
using System.Collections.Generic;
using System.Text;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Contracts.Modbus.Ascii;
using ModbusRtuLib.Contracts.Modbus.Rtu;
using ModbusRtuLib.Contracts.Modbus.Tcp;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;
using ModbusRtuLib.Services.Modbus.Ascii;
using ModbusRtuLib.Services.Modbus.Rtu;

namespace ModbusRtuLib.Common;

public static class ModbusClientExtensions
{
    public static IModbusRtuClient InitSerialPort(
        this IModbusRtuClient client,
        Action<SerialPortConfig> p
    )
    {
        var serialPortConfig = new SerialPortConfig();
        p(serialPortConfig);
        client.Config = serialPortConfig;
        return client;
    }

    public static IModbusAsciiClient InitSerialPort(
        this IModbusAsciiClient client,
        Action<SerialPortConfig> p
    )
    {
        var serialPortConfig = new SerialPortConfig();
        p(serialPortConfig);
        client.Config = serialPortConfig;
        return client;
    }

    public static IModbusTcpClient InitServerPort(
        this IModbusTcpClient client,
        Action<ServerPortConfig> p
    )
    {
        var serialPortConfig = new ServerPortConfig("127.0.0.1");
        p(serialPortConfig);
        client.Config = serialPortConfig;
        return client;
    }

    /// <summary>
    /// 启动！
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    public static IModbusRtuClient SetupStart(this IModbusRtuClient client)
    {
        if (client is ModbusRtuClient rtu)
        {
            rtu.Setup();
        }
        return client;
    }

    public static IModbusAsciiClient SetupStart(this IModbusAsciiClient client)
    {
        if (client is ModbusAsciiClient rtu)
        {
            rtu.Setup();
        }

        return client;
    }

    public static IModbusRtuClient SetDataFormat(
        this IModbusRtuClient client,
        DataFormat dataFormat
    )
    {
        if (client is ModbusRtuClient rtu)
        {
            if (rtu.Config == null)
            {
                return client;
            }
            rtu.Config.DataFormat = dataFormat;
        }
        return client;
    }

    /// <summary>
    /// 预先分配从站
    /// </summary>
    /// <param name="slave"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IModbusRtuClient AddSlave(
        this IModbusRtuClient slave,
        Action<ModbusSlaveConfig> config
    )
    {
        var modbusRtuSlaveConfig = new ModbusSlaveConfig();
        config(modbusRtuSlaveConfig);
        slave.AddSlave(modbusRtuSlaveConfig);
        return slave;
    }

    /// <summary>
    /// 预先分配从站
    /// </summary>
    /// <param name="slave"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IModbusAsciiClient AddSlave(
        this IModbusAsciiClient slave,
        Action<ModbusSlaveConfig> config
    )
    {
        var modbusRtuSlaveConfig = new ModbusSlaveConfig();
        config(modbusRtuSlaveConfig);
        slave.AddSlave(modbusRtuSlaveConfig);
        return slave;
    }
}