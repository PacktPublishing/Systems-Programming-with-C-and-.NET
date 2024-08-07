﻿using System.IO.Ports;

namespace SerialMonitor;

internal class AsyncSerial : IAsyncSerial
{
    private SerialPort? _serialPort;

    public bool IsOpen { get; private set; }

    public void Open(
        string portName,
        int baudRate = 9600,
        Parity parity = Parity.None,
        int dataBits = 8,
        StopBits stopBits = StopBits.One)
    {
        if (IsOpen) throw new InvalidOperationException("Serial port is already open");

        _serialPort = new SerialPort(
            portName,
            baudRate,
            parity,
            dataBits,
            stopBits);
        _serialPort.Open();

        IsOpen = true;
    }

    public void Close()
    {
        if (!IsOpen) throw new InvalidOperationException("Serial port is not open");

        _serialPort?.Close();
        _serialPort?.Dispose();
        _serialPort = null;

        IsOpen = false;
    }

    public Task<byte> ReadByteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            if (!IsOpen) throw new InvalidOperationException("Serial port is not open");
            var buffer = new byte[1];
            try
            {
                _serialPort?.Read(buffer, 0, 1);
            }
            catch (OperationCanceledException)
            {
                // This happens when the device has been unplugged
                // We pass it a 0xFF to indicate that the device is no longer available
                buffer[0] = 255;
            }

            return buffer[0];
        }, stoppingToken);
    }
}