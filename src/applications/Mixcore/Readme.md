# Run module as a microservice :
## 1. Remove Reference to module from Mixcore project
## 2. Copy MixContent to Module's source code
## 3. Update ocelot.json.(Ref: https://ocelot.readthedocs.io/en/latest/features/configuration.html). 
Ex:
```
{
    "DownstreamPathTemplate": "/api/v2/rest/mix-portal/{catchALl}",
    "DownstreamScheme": "https",
    "DownstreamHostAndPorts": [
    {
        "Host": "localhost",
        "Port": 5006
    }
    ],
    "UpstreamPathTemplate": "/api/v2/rest/mix-portal/{catchALl}",
    "UpstreamHttpMethod": [ "Get", "Post", "Put", "Patch", "Delete" ]
}
```