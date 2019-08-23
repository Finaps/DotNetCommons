# Asp.Net Core utilities

## Pagination

Use the `Pagination` object as a parameter in your controller methods to allow the client to call the function with limit and offset query parameters

```csharp
[Route("[controller]")]
[ApiController]
class ObjectsController : ControllerBase
{
  ...

  [HttpGet]
  public ActionResult<PaginatedResponse<Object>> GetObjects(Pagination pagination)
  {
    return repository.GetObjects(pagination.limit, pagination.offset);
  }
}
```

The client can then call this method in the following way: `GET http://{address}/objects?limit=10&offset=0`.  

The `PaginatedResponse<T>` contains a list of objects of type `T`, a total count of the number of items in the database and the given limit and offset. To construct a `PaginatedResponse<T>`, use the static factory method `Create`:

```csharp
PaginatedResponse<T>.Create(IList<T> data, int limit, int offset, long total);
```

To transform a `PaginatedResponse<T>` into a `PaginatedResponse<TF>`, for example if the repository returns database objects and you want to transform these to API response data transfer objects the `CreateFrom` method is available:

```csharp
PaginatedResponse<string> stringPR = ...
PaginatedResponse<int> intPR = PaginatedResponse.CreateFrom(stringPR, int.Parse);
```

An async method `CreateFromAsync` is also available.

## Handling files

To handle file uploads, you can use the `DisableFormValueModelBindingAttribute` and `MultipartRequestHelper` classes. These are based on Microsoft's own documentation on file uploads [here](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-2.2#uploading-large-files-with-streaming).

## Request validation

Methods in a controller class decorated with the `ApiController` attribute will automatically validate incoming responses based on attributes you used in your models. Read the official docs [here](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-2.2). This module adds some extra validation attributes:

### ValidateObjectAttribute

Normally the validation will only check the top level attributes and not attributes in any nested objects. To validate nested objects, you can decorate properties with the `ValidateObject` attribute.

```csharp
class Workflow
{
  ...
  [ValidateObject]
  public WorkflowStep Step { get; set; }
  ...
}
```

This attribute also works on lists.

### RequiredIfOtherPropertyHasValueAttribute

Sometimes you want a property to be required only if another property has a certain value. This attribute allows you to specify this behavior. Use it as follows:

```csharp
class WorkflowStep
{
  ...
  public bool PauseWorkflowAfterFinishing { get; set; }
  [RequiredIfOtherPropertyHasValue(nameof(PauseWorkflowAfterFinishing), true)]
  public long? PauseDurationInSeconds { get; set; }
  ...
}
```
