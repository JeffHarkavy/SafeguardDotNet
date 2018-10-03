﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace OneIdentity.SafeguardDotNet.Event
{
    internal abstract class PersistentSafeguardEventListenerBase : ISafeguardEventListener
    {
        private bool _disposed;

        private SafeguardEventListener _eventListener;
        private readonly EventHandlerRegistry _eventHandlerRegistry;

        private Task _reconnectTask;
        private CancellationTokenSource _reconnectCancel;

        protected PersistentSafeguardEventListenerBase()
        {
            _eventHandlerRegistry = new EventHandlerRegistry();
        }

        public void RegisterEventHandler(string eventName, SafeguardEventHandler handler)
        {
            if (_disposed)
                throw new ObjectDisposedException("PersistentSafeguardEventListener");
            _eventHandlerRegistry.RegisterEventHandler(eventName, handler);
        }

        protected abstract SafeguardEventListener ReconnectEventListener();

        private void PersistentReconnectAndStart()
        {
            if (_reconnectTask != null)
                return;
            _reconnectCancel = new CancellationTokenSource();
            _reconnectTask = Task.Run(() =>
            {
                while (!_reconnectCancel.IsCancellationRequested)
                {
                    try
                    {
                        _eventListener = ReconnectEventListener();
                        _eventListener.SetEventHandlerRegistry(_eventHandlerRegistry);
                        _eventListener.Start();
                        _eventListener.SetDisconnectHandler(PersistentReconnectAndStart);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Persistent event listener reconnect error, sleeping for 5 seconds...");
                        Thread.Sleep(5000);
                    }
                }
            }, _reconnectCancel.Token);
            _reconnectTask.ContinueWith((task) =>
            {
                _reconnectCancel.Dispose();
                _reconnectCancel = null;
                _reconnectTask = null;
            });
        }

        public void Start()
        {
            if (_disposed)
                throw new ObjectDisposedException("PersistentSafeguardEventListener");
            PersistentReconnectAndStart();
        }

        public void Stop()
        {
            if (_disposed)
                throw new ObjectDisposedException("PersistentSafeguardEventListener");
            _reconnectCancel?.Cancel();
            _eventListener?.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;
            try
            {
                _eventListener?.Dispose();
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
