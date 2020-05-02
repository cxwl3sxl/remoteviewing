﻿#region License
/*
RemoteViewing VNC Client/Server Library for .NET
Copyright (c) 2020 Quamotion bvba
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using RemoteViewing.Vnc;
using System;
using System.Runtime.InteropServices;

namespace RemoteViewing.LibVnc.Interop
{
    /// <summary>
    /// Provides access to <c>libvncserver</c> methods.
    /// </summary>
    public static unsafe class NativeMethods
    {
        /// <summary>
        /// The name of the libvncserver library.
        /// </summary>
        public const string LibraryName = "libvncserver";

        /// <summary>
        /// The calling convention used by the libvncserver library.
        /// </summary>
        public const CallingConvention LibraryCallingConvention = CallingConvention.Cdecl;

        /// <summary>
        /// A delegate which is invoked when a new client connects to a server.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        /// <returns>
        /// A <see cref="RfbNewClientAction"/> which determines how to proceed.
        /// </returns>
        public delegate RfbNewClientAction RfbNewClientHookPtr(IntPtr cl);

        /// <summary>
        /// A delegate which is invoked when a keyboard event occurs.
        /// </summary>
        /// <param name="down">
        /// A value indicating whether the key is currently down.
        /// </param>
        /// <param name="keySym">
        /// The key for which the event occurs.
        /// </param>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        public delegate void RfbKbdAddEventProcPtr(byte down, KeySym keySym, IntPtr cl);

        /// <summary>
        /// A delegate which is invoked when an mouse event occurs.
        /// </summary>
        /// <param name="buttonMask">
        /// A bit mask of pressed mouse buttons, in X11 convention.
        /// </param>
        /// <param name="x">
        /// The x coordinate of the point at which the touch occurred.
        /// </param>
        /// <param name="y">
        /// The y coordinate of the point at which the touch occurred.
        /// </param>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        public delegate void RfbPtrAddEventProcPtr(int buttonMask, int x, int y, IntPtr cl);

        /// <summary>
        /// A delegate which is invoked when a client disconnects.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        public delegate void ClientGoneHookPtr(IntPtr cl);

        /// <summary>
        /// A delegate which is invoked when a client requests a framebuffer update.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        /// <param name="furMsg">
        /// The framebuffer update request message.
        /// </param>
        public delegate void ClientFramebufferUpdateRequestHookPtr(IntPtr cl, IntPtr /*rfbFramebufferUpdateRequestMsg*/ furMsg);

        /// <summary>
        /// A delegate which is invoked to determine whether the X11 server permits input from the
        /// local user.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        /// <param name="status">
        /// <c>0</c> when the X11 user does not permit input from the local user; otherwise,
        /// <c>1</c>.
        /// </param>
        public delegate void rfbSetServerInputProcPtr(IntPtr cl, int status);

        /// <summary>
        /// A delegate which is invoked to determine whether to allow or deny file transfers. The default is
        /// to deny file transfers. It is called when a client initiates a connection to determine if it is permitted.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        /// <returns>
        /// <c>0</c> when file transfers are denied; otherwise, <c>1</c>.
        /// </returns>
        public delegate int rfbFileTransferPermitted(IntPtr cl);

        /// <summary>
        /// A delegate which is invoked to handle textchag messages.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        /// <param name="length">
        /// The length of the textchat message.
        /// </param>
        /// <param name="message">
        /// The textchat message.
        /// </param>
        public delegate void rfbSetTextChat(IntPtr cl, int length, char* message);

        /// <summary>
        /// A delegate which is invoked just before a frame buffer update.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        public delegate void rfbDisplayHookPtr(IntPtr cl);

        /// <summary>
        /// A delegate which is invoked to get the caps/num/scroll states of the X server.
        /// </summary>
        /// <param name="screen">
        /// The server structure.
        /// </param>
        /// <returns>
        /// The caps/num/scroll states of the X server.
        /// </returns>
        public delegate int rfbGetKeyboardLedStateHookPtr(IntPtr screen);

        /// <summary>
        /// A delegate which is invoked to validate the user password.
        /// </summary>
        /// <param name="cl">
        /// The server structure.
        /// </param>
        /// <param name="encryptedPassWord">
        /// A pointer to the password provided by the user.
        /// </param>
        /// <param name="len">
        /// The length of the password.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the password is correct; otherwise, <see langword="true"/>.
        /// </returns>
        public delegate bool rfbPasswordCheckProcPtr(IntPtr cl, sbyte* encryptedPassWord, int len);

        /// <summary>
        /// Initialises a server structure.
        /// </summary>
        /// <param name="argc">
        /// The number of command-line arguments.
        /// </param>
        /// <param name="argv">
        /// The command-line arguments passed to libvncserver.
        /// </param>
        /// <param name="width">
        /// The width of the screen, in pixels.
        /// </param>
        /// <param name="height">
        /// The height of the screen, in pixels.
        /// </param>
        /// <param name="bitsPerSample">
        /// The number of bits per sample.
        /// </param>
        /// <param name="samplesPerPixel">
        /// The number of samples per pixel.
        /// </param>
        /// <param name="bytesPerPixel">
        /// The number of bytes per pixel.
        /// </param>
        /// <returns>
        /// A <see cref="RfbScreenInfoPtr"/> which represents the server structure.
        /// </returns>
        [DllImport(LibraryName, CallingConvention = LibraryCallingConvention, EntryPoint = "rfbGetScreen")]
        public static extern RfbScreenInfoPtr rfbGetScreen(ref int argc, char** argv, int width, int height, int bitsPerSample, int samplesPerPixel, int bytesPerPixel);

        /// <summary>
        /// Initialises a server structure.
        /// </summary>
        /// <param name="width">
        /// The width of the screen, in pixels.
        /// </param>
        /// <param name="height">
        /// The height of the screen, in pixels.
        /// </param>
        /// <param name="bitsPerSample">
        /// The number of bits per sample.
        /// </param>
        /// <param name="samplesPerPixel">
        /// The number of samples per pixel.
        /// </param>
        /// <param name="bytesPerPixel">
        /// The number of bytes per pixel.
        /// </param>
        /// <returns>
        /// A <see cref="RfbScreenInfoPtr"/> which represents the server structure.
        /// </returns>
        public static RfbScreenInfoPtr rfbGetScreen(int width, int height, int bitsPerSample, int samplesPerPixel, int bytesPerPixel)
        {
            int count = 0;
            return rfbGetScreen(ref count, null, width, height, bitsPerSample, samplesPerPixel, bytesPerPixel);
        }

        // TODO: Enable ZRLE

        /// <summary>
        /// Initialize the server.
        /// </summary>
        /// <param name="rfbScreen">
        /// The server structure to initialize.
        /// </param>
        [DllImport(LibraryName, CallingConvention = LibraryCallingConvention, EntryPoint = "rfbInitServerWithPthreadsButWithoutZRLE")]
        public static extern void rfbInitServer(RfbScreenInfoPtr rfbScreen);

        /// <summary>
        /// Updates a server structure to use a new framebuffer.
        /// </summary>
        /// <param name="rfbScreen">
        /// The server structure.
        /// </param>
        /// <param name="framebuffer">
        /// The new framebuffer.
        /// </param>
        /// <param name="width">
        /// The width of the screen, in pixels.
        /// </param>
        /// <param name="height">
        /// The height of the screen, in pixels.
        /// </param>
        /// <param name="bitsPerSample">
        /// The number of bits per sample.
        /// </param>
        /// <param name="samplesPerPixel">
        /// The number of samples per pixel.
        /// </param>
        /// <param name="bytesPerPixel">
        /// The number of bytes per pixel.
        /// </param>
        [DllImport(LibraryName, CallingConvention = LibraryCallingConvention, EntryPoint = "rfbNewFramebuffer")]
        public static extern void rfbNewFramebuffer(RfbScreenInfoPtr rfbScreen, void* framebuffer, int width, int height, int bitsPerSample, int samplesPerPixel, int bytesPerPixel);

        /// <summary>
        /// Cleans up a server structure.
        /// </summary>
        /// <param name="screen">
        /// The server structure to clean up.
        /// </param>
        [DllImport(LibraryName, CallingConvention = LibraryCallingConvention, EntryPoint = "rfbScreenCleanup")]
        public static extern void rfbScreenCleanup(IntPtr screen);

        /// <summary>
        /// Marks a rectangle as modified.
        /// </summary>
        /// <param name="rfbScreen">
        /// The server structure.
        /// </param>
        /// <param name="x1">
        /// The x coordinate of the lower-left corner of the rectangle.
        /// </param>
        /// <param name="y1">
        /// The y coordinate of the lower-left corner of the rectangle.
        /// </param>
        /// <param name="x2">
        /// The x coordinate of the upper-right corner of the rectangle.
        /// </param>
        /// <param name="y2">
        /// The y coordinate of the upper-right corner of the rectangle.
        /// </param>
        [DllImport(LibraryName, CallingConvention = LibraryCallingConvention, EntryPoint = "rfbMarkRectAsModified")]
        public static extern void rfbMarkRectAsModified(RfbScreenInfoPtr rfbScreen, int x1, int y1, int x2, int y2);

        /// <summary>
        /// Runs the VNC event loop.
        /// </summary>
        /// <param name="screenInfo">
        /// The server for which to run the event loop.
        /// </param>
        /// <param name="usec">
        /// The number of microseconds the select on the fds waits.
        /// If you are using the event loop, set this to some value > 0, so the
        /// server doesn't get a high load just by listening.
        /// </param>
        /// <param name="runInBackground">
        /// A value indicating whether to run as a background thread or not.
        /// </param>
        [DllImport(LibraryName, CallingConvention = LibraryCallingConvention, EntryPoint = "rfbRunEventLoop")]
        public static extern void rfbRunEventLoop(RfbScreenInfoPtr screenInfo, long usec, byte runInBackground);

        /// <summary>
        /// Processes VNC server events.
        /// </summary>
        /// <param name="screenInfo">
        /// The server for which to process events.
        /// </param>
        /// <param name="usec">
        /// The number of microseconds the select on the fds waits.
        /// If you are using the event loop, set this to some value > 0, so the
        /// server doesn't get a high load just by listening.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if an update was pending.
        /// </returns>
        [DllImport(LibraryName, CallingConvention = LibraryCallingConvention, EntryPoint = "rfbProcessEvents")]
        public static extern byte rfbProcessEvents(RfbScreenInfoPtr screenInfo, long usec);
    }
}