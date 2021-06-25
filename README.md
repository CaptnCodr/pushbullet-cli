# Pushbullet CLI

## 1. Setup

Set application's path in system's environment "Path" variables.

## 2. Usings

### 2.1 `key`

First of all, create your API key from [Pushbullet](http://pushbullet.com) and set it using:

`> pb key <API-KEY>`

Get API-Key:

`> pb key`

<br />

### 2.2 Meta data

Get your push limits:

`> pb limits` or

`> pb -x`

<br />
Get your profile data:

`> pb me` or

`> pb -i`

<br />

### 2.3 `push` / `text` / `link` / `url`

Then you're able to push text to your devices (different writing styles):

`> pb push "Hello world!"`

`> pb -p "Hello world!"`

`> pb text "Hello world!"`

`> pb -t "Hello world!"`

or

`> pb link http://pushbullet.com "Title of link" "Description of Link"`

`> pb url http://pushbullet.com "Title of link" "Description of Link"`

<br />
You can drop arguments by pass an empty string like:

`> pb url http://pushbullet.com "" "Description of Link"`

`> pb -u http://pushbullet.com "" "Description of Link"`

<br />

### 2.4 List things (changes in some time)

Show your last 3 pushes:

`> pb pushes 3` or

`> pb -ps` (retuns last push)

<br />
Show all your devices:

`> pb devices` or

`> pb -ds`

<br />
Show all your chats:

`> pb chats` or

`> pb -cs`

<br />
Show all your subscriptions:

`> pb subscriptions` or

`> pb subs` or

`> pb -s`

<br />

### 2.5 `list` (as in 2.4)

Show your last 3 pushes:

`> pb list pushes 3` or

`> pb list -p` (retuns last push)

<br />
Show all your devices:

`> pb list devices` or

`> pb list -d`

<br />
Show all your chats:

`> pb list chats` or

`> pb list -c`

<br />
Show all your subscriptions:

`> pb list subscriptions` or

`> pb list -s`

<br />

### 2.6 `delete`

Delete configured api key:

`> pb delete key` or

`> pb -d -k`

<br />
Delete specific push with different writing styles:

`> pb --del push <PUSH-ID>` or

`> pb delete -p <PUSH-ID>` or

`> pb -d -p <PUSH-ID>`

<br />
Delete device:

`> pb delete device <DEVICE-ID>` or

`> pb -d -d <DEVICE-ID>`

<br />
Delete chat:

`> pb delete chat <CHAT-ID>` or

`> pb -d -c <CHAT-ID>`

<br />
Delete subscription:

`> pb delete subscription <SUBSCRIPTION-ID>` or

`> pb -d -s <SUBSCRIPTION-ID>`
