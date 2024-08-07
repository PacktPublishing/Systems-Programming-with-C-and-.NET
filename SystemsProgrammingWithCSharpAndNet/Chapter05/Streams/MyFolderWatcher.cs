﻿namespace Streams;

internal class MyFolderWatcher : IDisposable
{
    private FileSystemWatcher? _watcher;

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    // Create an event that is raised when a file has been added

    public event EventHandler<FileCreatedEventArgs>? FileAdded;
    public event EventHandler<FileChangedEventArgs>? FileChanged;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        if (_watcher == null)
            return;

        // Stop raising events
        _watcher.EnableRaisingEvents = false;
        // Clean whoever has subscribed to us
        // to prevent memory leaks
        FileAdded = null;
        FileChanged = null;

        _watcher.Dispose();
        _watcher = null;
    }

    ~MyFolderWatcher()
    {
        Dispose(false);
    }

    public void SetupWatcher(string pathToWatch)
    {
        if (_watcher != null)
            throw new InvalidOperationException(
                "The watcher has already been set up");

        if (!Path.Exists(pathToWatch))
            throw new ArgumentOutOfRangeException(
                nameof(pathToWatch),
                "The path does not exist");

        // Set the folder to keep an eye on
        _watcher = new FileSystemWatcher(pathToWatch);
        // We only want notifications when a file is created or when it has changed .
        _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
        // Set the callbacks
        _watcher.Created += WatcherCallback;
        _watcher.Changed += WatcherCallback;
        // Start watching
        _watcher.EnableRaisingEvents = true;
    }

    private void WatcherCallback(object sender, FileSystemEventArgs e)
    {
        switch (e.ChangeType)
        {
            case WatcherChangeTypes.Created:
                FileAdded?.Invoke(this, new FileCreatedEventArgs(e.FullPath));
                break;

            case WatcherChangeTypes.Changed:
                FileChanged?.Invoke(this, new FileChangedEventArgs(e.FullPath));
                break;
        }
    }
}