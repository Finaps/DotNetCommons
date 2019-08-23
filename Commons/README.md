# Common utilities

Sometimes the client wants to get multiple objects by their id. Instead of doing multiple requests with one id, it is often useful to offer the possibility to get all objects in one call. A call like that could look like `http://{address}/objects?ids=87318705-e9ed-49e8-8fb8-297708bb8019,aca9a2ca-9d87-4885-80f2-e121d96a3812`.  
This module provides a small extension method to separate a Guid string like that into a list of Guids:

```csharp
string idString = "87318705-e9ed-49e8-8fb8-297708bb8019,aca9a2ca-9d87-4885-80f2-e121d96a3812";
List<Guid> ids = idString.ParseListOfGuids();
```

This method will throw a FormatException if your ids are not formatted according to the Guid specification.
