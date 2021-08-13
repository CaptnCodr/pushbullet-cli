
Syntax: pb [command] [subcommand] [arguments]
        
Commands:\n
key | -k [api key]                          Set API key with argument. Show API key without argument.
me | -i                                     Get profile of configured API key.
limits | -x                                 Get rate limits.
grants | -g                                 Get grants that have access to your PB account.
push | -p | text | -t [arguments]           Push text or note. Use push [device / -d] to push to a specific device.
link | -l | url | -u [arguments]            Push a link to device(s). Use push [device / -d] to push to a specific device.
pushes | -ps [number]                       List [number] of pushes or else last push.
sms | -m [arguments]                        Send sms to eligible Device. sms [device] [mobile number] [text]
devices | -ds                               Lists devices of current account. Including identifiers and indexes to identify.
device | -d [iden / index]                  Shows information about a device. Select with identifier or index shown in the [devices / -ds] command.
chats | -cs                                 List chats of current account.
subscriptions | subs | -s                   List subscriptions with channel tag of current account.
channelinfo | -ci [tag]                     Show information about a specific channel with channel tag as shown in [subscriptions / subs / -s].

delete | --del | remove | -r [subcommand]   Deletes an object:
    push | -p [iden]                        Deletes a push using its iden.
    device | -d [iden]                      Deletes a device using its iden.
    chat | -c [iden]                        Deletes a chat using its iden.
    subscription | -s [iden]                Deletes a subscription using its iden.
    key | -k                                Deletes the current configured API key

help | -h                                   This help.