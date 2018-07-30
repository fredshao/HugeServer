using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;


public struct RawPackage
{
    public int sessionId;
    public byte[] data;
}


public class TCPServer
{
    private ConcurrentQueue<RawPackage> inQueue = new ConcurrentQueue<RawPackage>();
    private ConcurrentQueue<RawPackage> outQueue = new ConcurrentQueue<RawPackage>();
    private ConcurrentDictionary<int, Session> sessions = new ConcurrentDictionary<int, Session>();

    private int maxListenCount = 0;
    private int socketReceiveBufferSize = 0;

    public TCPServer(int _maxListenCount, int _socketReceiveBufferSize)
    {
        this.maxListenCount = _maxListenCount;
        this.socketReceiveBufferSize = _socketReceiveBufferSize;
    }
    

}