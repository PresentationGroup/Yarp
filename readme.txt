Mapping the request URL path to a route in proxy configuration.
Routes are mapped to clusters which are a collection of destination endpoints.
The destinations are filtered based on health status, and session affinity (not used in this sample).
From the remaining destinations, one is selected using a load balancing algorithm.
The request is proxied to that destination.



Which includes further restrictions:
Path must be /something/*
Host must be localhost, www.aaaaa.com or www.bbbbb.com
Http Method must be GET or POST
Must have a header "MyCustomHeader" with a value of "value1", "value2" or "another value"
A "MyHeader" header will be added with the value "MyValue"
Must have a query parameter "MyQueryParameter" with a value of "value1", "value2" or "another value"
This will route to cluster "allClusterProps" which has 2 destinations - https://www.microsoft.com and https://10.20.30.40
Requests will be load balanced between destinations using a "PowerOfTwoChoices" algorithm, which picks two destinations at random, then uses the least loaded of the two.
It includes session affinity using a cookie which will ensure subsequent requests from the same client go to the same host.
It is configured to have both active and passive health checks - note the second destination will timeout for active checks (unless you have a host with that IP on your network)
It includes HttpClient configuration setting outbound connection properties
HttpRequest properties defaulting to HTTP/2 with a 2min timout


curl -v -k -X GET -H "MyCustomHeader: value1" http://localhost:5000/api/downloads?MyQueryParameter=value2

