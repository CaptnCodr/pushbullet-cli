# Pushbullet CLI

## Setup

Set application's path in system's environment "Path" variables.

## Usings

### `key`

First of all, create your API key from [Pushbullet](http://pushbullet.com) and set it using:
`> pb key <API-KEY>`

Get API-Key:
`> pb key`

### Meta data

Get your push limits:
`> pb limits` or
`> pb -x`

Get your profile data:
`> pb me` or
`> pb -i`

### `push` / `text` / `link` / `url`

Then you're able to push text to your devices (different writing styles):

`> pb push "Hello world!"`
`> pb -p "Hello world!"`
`> pb text "Hello world!"`
`> pb -t "Hello world!"`

or

`> pb link http://pushbullet.com "Title of link" "Description of Link"`
`> pb url http://pushbullet.com "Title of link" "Description of Link"`

You can drop arguments by pass an empty string like:

`> pb url http://pushbullet.com "" "Description of Link"`
`> pb -u http://pushbullet.com "" "Description of Link"`

### List things (changes in some time)

Show your last 3 pushes:
`> pb pushes 3` or
`> pb -ps` (retuns last push)

Show all your devices:
`> pb devices` or
`> pb -ds`

Show all your chats:
`> pb chats` or
`> pb -cs`

Show all your subscriptions:
`> pb subscriptions` or
`> pb subs` or
`> pb -s`

### `delete`

Delete configured api key:
`> pb delete key` or
`> pb -d -k`

Delete specific push with different writing styles:
`> pb --del push <PUSH-ID>` or
`> pb delete -p <PUSH-ID>`
`> pb -d -p <PUSH-ID>`

Delete device:
`> pb delete device <DEVICE-ID>` or
`> pb -d -d <DEVICE-ID>` or

Delete chat:
`> pb delete chat <CHAT-ID>` or
`> pb -d -c <CHAT-ID>` or

Delete subscription:
`> pb delete subscription <SUBSCRIPTION-ID>` or
`> pb -d -s <SUBSCRIPTION-ID>` or


