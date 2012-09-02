using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 请求读线程锁
    /// </summary>
    public class AcquireReadLock : IDisposable
    {
        private ReaderWriterLockSlim _locker;
        private bool _disposedValue;

        public AcquireReadLock(ReaderWriterLockSlim rwLock)
        {
            _disposedValue = false;
            _locker = rwLock;
            _locker.EnterReadLock();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing)
            {
                _locker.ExitReadLock();
            }
            _disposedValue = true;
        }
    }

    /// <summary>
    /// 请求写线程锁
    /// </summary>
    public class AcquireWriteLock : IDisposable
    {
        private ReaderWriterLockSlim _locker;
        private bool _disposedValue;

        public AcquireWriteLock(ReaderWriterLockSlim rwLock)
        {
            _disposedValue = false;
            _locker = rwLock;
            _locker.EnterWriteLock();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing)
            {
                _locker.ExitWriteLock();
            }
            _disposedValue = true;
        }
    }
}
