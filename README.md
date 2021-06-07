# doctr-net
![example workflow](https://github.com/netxph/doctr-net/actions/workflows/dotnet.yml/badge.svg)

Set of diagnostics and debugging tools for .NET core. Helpful for workflows that does not rely on VSCode or Visual Studio.

## Installation

Available in NuGet and can be added with `dotnet-cli`.

```powershell
dotnet add package Doctr.Diagnostics
```

## Usage

**Microsoft.Extensions.Logging Trace Listener**

It's very convenient to write `Trace.WriteLine` or `Debug.WriteLine` anywhere in the code, whether to inspect values or verify flows. However, they don't show up by default on `aspnet` projects. Here's how you can enable it.

```c#
using Doctr.Diagnostics;

//Register to DI first
services.AddDoctr();

//Bind Trace to Microsoft.Extensions.Logging
app.ApplicationServices.BindTrace();
```

**First Chance Exception**

A usual workflow is to capture where the first exception occurs. When using VSCode or Visual Studio, they have that functionality where you can set the application to break on first occurrence of exception. This library can perform `Trace` logs on the first throw of exception which helps you identify the where the error occurred.

To do this, wrap a block of code that you want to track for these First Chance Exceptions.

```c#
using(new FirstChanceExceptionHandler())
{
	var service = new BuggyService();
    service.PerformOperation();
}
```

**Object Inspection**

You can quickly peek into object's public content.

```c#
var person = new Person();

Trace.WriteLine(person.Dump());
```

However, if you want to look at non-public members:

```c#
Trace.WriteLine(person.Dump("_internalName"));
```

Both of these dump commands outputs a prettified `json`.

If you want to return the internal object itself:

```c#
var internalName = person.Internal("_internalName");

//specific data type
var hidden = person.Internal<Hidden>("_hidden");
```

If you don't know what you're looking at, you can inspect the object's Type first.

```c#
Trace.WriteLine(person.GetType().ReflectDump());
Trace.WriteLine(typeof(Person).ReflectDump());
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

**Feature Guidelines**

This library aims to make it easy for developers to debug their code. With this in goal, we want to make things really simple and should be accomplished with a single line and easy to remember as much as possible. If .NET already provides these information really easy, there's no need for us to include those features in this library.



## License
[MIT](https://choosealicense.com/licenses/mit/)
