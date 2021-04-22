
[![NuGet](https://img.shields.io/nuget/v/SunMapper.svg?label=SunMapper%20-%20nuget)](https://www.nuget.org/packages/SunMapper) [![NuGet](https://img.shields.io/nuget/dt/SunMapper.svg)](https://www.nuget.org/packages/SunMapper)

[![.NET](https://github.com/DenDeline/SunMapper/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DenDeline/SunMapper/actions/workflows/dotnet.yml)

# SunMapper
Lightweight and fast object mapper, that creates methods for mapping at the [compilation time](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/).

# Testing 🛠
There are a lot of things that not implemented yet or some code can give you unexpected result, so use this package at your own risk. 

# Install

* PowerShell:
```
Install-Package SunMapper
```
-- OR --

* Add reference to the project file:

```
<PackageReference Include="SunMapper"/>
```

You can find other methods for installing [here](https://www.nuget.org/packages/SunMapper/).

# Usage

Add `MapTo` attribute to the model you want to get data from:
```c#
using SunMapper.Common.Attributes;

[MapTo(typeof(UserGetDto))]
public class User {
    public int Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
}

public UserGetDto {
    public string Name { get; set; }
}
```
Bunch stuff will be generated under the hood, so why you can use extension method `TryMapTo` of `User`:

```c#
var user = new User { Id = 0, Name = "User", PasswordHash = "Hash" };

if (user.TryMapTo(out UserGetDto dto)) {
    // working with dto
}
```
# Limitations 

You can map only **public** properties with **getter and setter** and **the same type**. 

# Milestones 🚩
For comfortable using library, this things should be implemented: 
- [ ] Mapping profiles
- [ ] Custom type conversation
- [ ] Upselling architecture
- [ ] Support of popular libraries such as FluentValidation
- [ ] ASP.NET Support
- [ ] Readable generated code

