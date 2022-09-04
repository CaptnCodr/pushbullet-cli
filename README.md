# Pushbullet CLI

## 0. Setup

Set application's path in system's environment "Path" variables.

## 1. Using

##### Command tree of all (sub-) commands and parameters:

```
> pb [command | alias] [subcommand | alias] [parameter]

[ key | -k ]
+--[ key ]

[ push | -p | text | -t ]
+--[ device | -d ]
|  +--[ deviceid ]
+--[ text | -t ]
|  +--[ body ]
+--[ note | -n ]
   +--[ title ][ body ]

[ pushinfo | -pi ]
+--[ pushid ]

[ pushes | -ps ]
+--[ number ]

[ link | -l | url | -u ]
+--[ device | -d ]
|  +--[ deviceid ]
+--[ url | -u ]
|  +--[ link ]
+--[ title | -t ]
|  +--[ title ]
+--[ body | -b ]
   +--[ body ]

[ device | -d ]
+--[ deviceid ]

[ devices | -ds ]

[ chat | -c ]
+--[ update | -u ]
|  +--[ id ]
|  +--[ status ]
+--[ create | -c ]
   +--[ email ]

[ chats | -cs ]

[ sms | -m ]
+--[ device ]
+--[ number ]
+--[ body ]

[ clip | -cl ]
+--[ json ]

[ subscriptions | -s | subs ]

[ channelinfo | -ci ]
+--[ tag ]

[ delete | -del | -r ]
+--[ push | -p ]
|  +--[ id ]
+--[ chat | -c ]
|  +--[ id ]
+--[ device | -d ]
|  +--[ id ]
+--[ subscription | -s ]
|  +--[ id ]
+--[ sms | -m ]
|  +--[ id ]
+--[ key | -k ]

[ profile | -me | -i ]

[ limits | -x ]

[ grants | -g ]

[ version | -v ]

[ help | h ]
```

Almost all functions are wrapped here.<br>
See API at: https://docs.pushbullet.com/