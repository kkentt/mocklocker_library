# mocklocker_library

MockLocker is a simple library that simulates locker functionality, allowing you to perform actions like unlocking, checking status, and closing locks. This library is easy to use and integrate into your .NET projects.

## Lock Types

The MockLocker library supports four lock types:

1. **Lock16** - simulates a locker with 16 locks
2. **Lock32** - simulates a locker with 32 locks
3. **Lock48** - simulates a locker with 48 locks
4. **Lock64** - simulates a locker with 64 locks

You can choose the desired lock type when instantiating the `MockLocker` class.

## Getting Started

To start using the MockLocker library, follow these steps:

1. Add a reference to the `MockLocker.Library` in your project.
2. In your code, add a `using` directive for `MockLocker.Library`.
3. Instantiate the `MockLocker` class by providing the `LockType`:

```csharp
using MockLocker.Library;
using MockLocker.Library.Models;

var mockLocker = new MockLocker(LockType.Lock16);
```


### Unlock a lock

To unlock a lock, call the `Unlock` method with the lock number:

```csharp
  var response = mockLocker.Unlock(1);

  if (response.IsSuccess)
  {
      Console.WriteLine("Lock unlocked successfully.");
  }
  else
  {
      Console.WriteLine($"Error: {response.Error}");
  }
```

### Close all locks

To close all locks at once, call the `CloseAllLocks` method:

```csharp
var response = mockLocker.CloseAllLocks();

if (response.IsSuccess)
{
    Console.WriteLine("All locks closed successfully.");
}
else
{
    Console.WriteLine($"Error: {response.Error}");
}
```

### Check lock statuses

To check the status of all locks, call the `CheckStatus` method:

```csharp
var statusResponse = mockLocker.CheckStatus();

if (statusResponse.IsSuccess)
{
    foreach (var status in statusResponse.Statuses)
    {
        Console.WriteLine($"Lock {status.LockNumber}: {(status.IsLocked ? "Locked" : "Unlocked")}");
    }
}
else
{
    Console.WriteLine($"Error: {statusResponse.Error}");
}

