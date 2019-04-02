Aliyun FunctionCompute C# Libraries
=================================

[![Software License](https://img.shields.io/badge/license-apache2.0-brightgreen.svg)](LICENSE)
[![GitHub version](https://badge.fury.io/gh/aliyun%2Ffc-dotnet-libs.svg)](https://badge.fury.io/gh/aliyun%2Ffc-dotnet-libs)
[![Build Status](https://travis-ci.org/aliyun/fc-dotnet-libs.svg?branch=master)](https://travis-ci.org/aliyun/fc-dotnet-libs)

Overview
--------

The SDK of this version is dependent on the third-party library [Json.NET](https://www.newtonsoft.com/json).
This componenent is for Aliyun FunctionCompute developers to build C# functions.

Running environment
-------------------

Applicable to .net core 2.1 or above


Installation
-------------------

#### Install the SDK through NuGet
 - If NuGet hasn't been installed for Visual Studio, install [NuGet](http://docs.nuget.org/docs/start-here/installing-nuget) first. 
 
 - After NuGet is installed, access Visual Studio to create a project or open an existing project, and then select `TOOLS` > `NuGet Package Manager` > `Manage NuGet Packages for Solution`.
 
 - For normal invoke function, type `Aliyun.Serverless.Core` in the search box and click *Search*, select the latest version, and click *Install*.
 - For http invoke function, beside `Aliyun.Serverless.Core`, you also need to install `Aliyun.Serverless.Core.Http`.

Getting started
-------------------

```csharp
using System;
using System.IO;
using System.Threading.Tasks;
using Aliyun.Serverless.Core;
using Microsoft.Extensions.Logging;
namespace samples
{
    public class TestHandler
    {
        public async Task<Stream> EchoEvent(Stream input, IFcContext context)
        {
            context.Logger.LogInformation("Handle Request: {0}", context.RequestId);
            MemoryStream memoryStream = new MemoryStream();
            await input.CopyToAsync(memoryStream);
            return memoryStream;
        }
    }
}
```

More resources
--------------
- [Aliyun FunctionCompute docs](https://help.aliyun.com/product/50980.html)

Contacting us
-------------
- [Links](https://help.aliyun.com/document_detail/53087.html)
