﻿using System;
using System.Runtime.InteropServices;

namespace MSDAD.PCS.Commands
{
    abstract class Command
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        public const string CLIENT = "Client";
        public const string CLIENT_EXE = "Client.exe";

        public const string SERVER = "Server";
        public const string SERVER_EXE = "Server.exe";

        public const string SERVER_SCRIPTS = "Server_Scripts";

        public PCSLibrary pcsLibrary;
        public Command(ref PCSLibrary pcsLibrary)
        {
            this.pcsLibrary = pcsLibrary;
        }
        public abstract object Execute();
    }
}
