# EasyBulkCopy
Easy and automatic mapping for SqlBulkCopy

## Installation
- Option 1: Install via package manager
- Option 2: Install via NuGet
```
Install-Package EasyBulkCopy
```
- Option 3: Install via dotnet command line interface
```
dotnet add package EasyBulkCopy
```

## Usage
### Create classes that map to your MSSQL tables and decorate them with the attribute to provide the table name
```csharp
[BulkTableName("dbo.MyTable")]
public class MyTable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
```

### Register your classes in the .NET Core DI container
- Option 1: Piecemeal registration
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.UseEasyBulkCopy();
    services.RegisterBulkInsertType<MyTable>();
}
```

- Option 2: Scan assembly and automatically register decorated types
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.UseEasyBulkCopy();
    services.RegisterBulkInsertTypesInAssembly(Assembly.GetExecutingAssembly());
}
```

### Inject the `IBulkInserter` interface where needed
```csharp
public class Foo
{
    // ...
    public Foo(IBulkInserter bulkInserter)
    {
        _bulkInserter = bulkInserter
        _connectionString = "Server=localhost;Database=master;"
    }

    public async Task Write(IEnumerable<MyTable> rows)
    {
        await _bulkInserter.Insert(_connectionString, rows);
    }
}
```

## Provide custom `SqlBulkCopyOptions` when defining your tables
Optionally customize using the `BulkTableName` attribute. Defaults to `SqlBulkCopyOptions.Default`.

```csharp
[BulkTableName("dbo.MyTable", SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepIdentity)]
public class MyTable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
```
