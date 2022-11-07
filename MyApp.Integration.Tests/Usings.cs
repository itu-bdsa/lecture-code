global using System.Net;

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.EntityFrameworkCore;

global using FluentAssertions;
global using Xunit;

global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Configurations;
global using DotNet.Testcontainers.Containers;

global using MyApp.Core;
global using MyApp.Infrastructure;
global using MyApp.Integration.Tests.Setup;

global using static MyApp.Core.Gender;
