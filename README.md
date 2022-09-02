# Pushbullet CLI

## 0. Setup

Set application's path in system's environment "Path" variables.

## 1. Using

##### Command tree of all (sub-) commands and parameters:

```
> pb [command] [subcommand] [parameter]

[ key ]
+--[ key ]

[ push ]
+--[ device ]
|  +--[ deviceid ]
+--[ text ]
|  +--[ body ]
+--[ note ]
   +--[ title ][ body ]

[ pushinfo ]
+--[ pushid ]

[ pushes ]
+--[ number ]

[ link ]
+--[ device ]
|  +--[ deviceid ]
+--[ url ]
|  +--[ link ]
+--[ title ]
|  +--[ title ]
+--[ body ]
   +--[ body ]

[ device ]
+--[ deviceid ]

[ devices ]

[ chat ]
+--[ update ]
|  +--[ id ]
|  +--[ status ]
+--[ create ]
   +--[ email ]

[ chats ]

[ sms ]
+--[ device ]
+--[ number ]
+--[ body ]

[ clip ]
+--[ json ]

[ subscriptions ]

[ channelinfo ]
+--[ tag ]

[ delete ]
+--[ push ]
|  +--[ id ]
+--[ chat ]
|  +--[ id ]
+--[ device ]
|  +--[ id ]
+--[ subscription ]
|  +--[ id ]
+--[ sms ]
|  +--[ id ]
+--[ key ]

[ profile ]

[ limits ]

[ grants ]

[ version ]

[ help ]
```
<br />

Almost all functions are wrapped here.
See API at: https://docs.pushbullet.com/