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

#### 2.2.1 Get your push limits:

`> pb limits` or

`> pb -x`

<br />

#### 2.2.2 Get your profile data:

`> pb me` or

`> pb -i`

<br />

### 2.3 `push` / `text` / `link` / `url`

#### 2.3.1 Then you're able to push text to your devices (different writing styles):

`> pb push "Hello world!"`

`> pb -p http://pushbullet.com`

`> pb text "Hello world!"`

`> pb -t "Hello world!"`

or

`> pb link http://pushbullet.com "Title of link" "Description of Link"`

`> pb url http://pushbullet.com "Title of link" "Description of Link"`

<br />

#### 2.3.2 You can drop arguments by pass an empty string like:

`> pb url http://pushbullet.com "" "Description of Link"`

`> pb -u http://pushbullet.com "" "Description of Link"`

<br />

#### 2.3.3 Push to specific device (`-d` / `device`):
(2.4.2 / 2.5.2 return devices with indexes and device_iden)

`> pb push device 0 "Hello world!"`

`> pb -p -d 1 http://pushbullet.com`

`> pb text device 0 "Hello world!"`

`> pb -t -d 1 "Hello world!"`

`> pb push device uy123456abcd "Hello world!"`

or

`> pb link -d 0 http://pushbullet.com "Title of link" "Description of Link"`

`> pb url device 1 http://pushbullet.com "Title of link" "Description of Link"`

( ... etc.)

<br />

### 2.4 List things (changes in some time)

#### 2.4.1 Show your last 3 pushes:

`> pb pushes 3` or

`> pb -ps` (retuns last push)

<br />

#### 2.4.2 Show all your devices:

`> pb devices` or

`> pb -ds`

<br />

### 2.4.3 Information about device:
device_iden / index: as showed in 2.4.2
`> pb device [device_iden]` or

`> pb -di [index]`

<br />

#### 2.4.4 Show all your chats:

`> pb chats` or

`> pb -cs`

<br />

#### 2.4.5 Show all your subscriptions:

`> pb subscriptions` or

`> pb subs` or

`> pb -s`

<br />

### 2.5 `list` (as in 2.4)

#### 2.5.1 Show your last 3 pushes:

`> pb list pushes 3` or

`> pb list -p` (retuns last push)

<br />

#### 2.5.2 Show all your devices:

`> pb list devices` or

`> pb list -d`

<br />

#### 2.5.3 Show all your chats:

`> pb list chats` or

`> pb list -c`

<br />

#### 2.5.4 Show all your subscriptions:

`> pb list subscriptions` or

`> pb list -s`

<br />

### 2.6 `delete`

#### 2.6.1 Delete configured api key:

`> pb delete key` or

`> pb -d -k`

<br />

#### 2.6.2 Delete specific push with different writing styles:

`> pb --del push <PUSH-ID>` or

`> pb delete -p <PUSH-ID>` or

`> pb -d -p <PUSH-ID>`

<br />

#### 2.6.3 Delete device:

`> pb delete device <DEVICE-ID>` or

`> pb -d -d <DEVICE-ID>`

<br />

#### 2.6.4 Delete chat:

`> pb delete chat <CHAT-ID>` or

`> pb -d -c <CHAT-ID>`

<br />

#### 2.6.5 Delete subscription:

`> pb delete subscription <SUBSCRIPTION-ID>` or

`> pb -d -s <SUBSCRIPTION-ID>`
