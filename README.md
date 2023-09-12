# gPRC-service
This repository is a collection of my gRPC projects, tutorials, and resources as I explore the world of high-performance API development with gRPC

# gRPC JSON transcoding in ASP.NET Core
## what is  gPRC

[gRPC](https://grpc.io/) is a high-performance Remote Procedure Call (RPC) framework. gRPC uses HTTP/2, streaming, Protobuf, and message contracts to create high-performance, real-time services.

## HTTP protocol
The ASP.NET Core gRPC service template, included in the .NET SDK, creates an app that's only configured for HTTP/2. HTTP/2 is a good default when an app only supports traditional gRPC over HTTP/2. Transcoding, however, works with both HTTP/1.1 and HTTP/2. Some platforms, such as UWP or Unity, can't use HTTP/2. To support all client apps, configure the server to enable HTTP/1.1 and HTTP/2.

Update the default protocol in appsettings.json:


```c#
{
  "Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http1AndHttp2"
    }
  }
}
```
Enabling HTTP/1.1 and HTTP/2 on the same port requires TLS for protocol negotiation. For more information about configuring HTTP protocols in a gRPC app, see [ASP.NET Core gRPC protocol negotiation.](https://learn.microsoft.com/en-us/aspnet/core/grpc/aspnetcore?view=aspnetcore-7.0#protocol-negotiation)

## Annotatating gRPC methods
gRPC methods must be annotated with an HTTP rule before they support transcoding. The HTTP rule includes information about how to call the gRPC method, such as the HTTP method and route.

```protobuff
service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply) {
    option (google.api.http) = {
      get: "/v1/greeter/{name}"
    };
  }
}
```

The proceeding example:

Defines a Greeter service with a SayHello method. The method has an HTTP rule specified using the name google.api.http.
The method is accessible with GET requests and the /v1/greeter/{name} route.
The name field on the request message is bound to a route parameter.
Many options are available for customizing how a gRPC method binds to a RESTful API. For more information about annotating gRPC methods and customizing JSON, see [Configure HTTP and JSON for gRPC JSON transcoding.](https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding-binding?view=aspnetcore-7.0)

## Streaming methods
Traditional gRPC over HTTP/2 supports streaming in all directions. Transcoding is limited to server streaming only. Client streaming and bidirectional streaming methods aren't supported.

Server streaming methods use line-delimited JSON. Each message written using WriteAsync is serialized to JSON and followed by a new line.

The following server streaming method writes three messages:

```c#
public override async Task StreamingFromServer(ExampleRequest request,
    IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
{
    for (var i = 1; i <= 3; i++)
    {
        await responseStream.WriteAsync(new ExampleResponse { Text = $"Message {i}" });
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
```
The client receives three line-delimited JSON objects:
```JSON
{"Text":"Message 1"}
{"Text":"Message 2"}
{"Text":"Message 3"}
```
