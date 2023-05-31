# SQLtoCSharpObjectList
 
Converting SQL insert lines to List of defined C# object list.

## Using
 - Copy your SQL insert commands as text file to `Data` folder.
 - Update `TransformedClass` with your expected result object.

   > Note: Order of class property should be same with SQL insert commands property order.
 - Change the Line:7 in `Program.cs` with your changes.
```sh
ProcessFile.Process(`String: Your text file name`, `Object: Object of your C# class like "new TransformedClass()"`, `String: Name of your object like "typeof(TransformedClass).Name"`, `Boolean: Give true if you want to print to console, give false if you want to print to file`);
```
**Emre Hancı**
