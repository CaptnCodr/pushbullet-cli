# Pushbullet CLI

## 1. Setup

Set application's path in system's environment "Path" variables.

## 2. Usings

### 2.0 Get help to the commands

`> pb help`

`> pb -h`

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

#### 2.2.3 Get grants that have access to your pushbullet account:

`> pb grants` or

`> pb -g`

<br />

### 2.3 `push` / `text` / `link` / `url`

#### 2.3.1 Then you're able to push text to your devices (different writing styles):

`> pb push "Hello world!"`

`> pb -p http://pushbullet.com`

`> pb text "Hello world!"`

`> pb -t "Hello world!"`

or

`> pb link http://pushbullet.com "Title of link" "Description of Link"`

`> pb -l http://pushbullet.com "Title of link" "Description of Link"`

`> pb url http://pushbullet.com "Title of link" "Description of Link"`

`> pb -u http://pushbullet.com "Title of link" "Description of Link"`

<br />

#### 2.3.2 Push a clip

`> pb clip http://pushbullet.com`

`> pb -cl http://pushbullet.com`


<br />

#### 2.3.2 You can drop arguments by pass an empty string like:

`> pb url http://pushbullet.com "" "Description of Link"`

`> pb -u http://pushbullet.com "" "Description of Link"`

<br />

#### 2.3.3 Push to specific device (`-d` / `device`):
(2.4.2 return devices with indexes and device_iden)

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

### 2.4 List things

#### 2.4.1 Show your last 3 pushes:

`> pb pushes 3` or

`> pb -ps` (retuns last push)

<br />

#### 2.4.2 Show all your devices:

`> pb devices` or

`> pb -ds`

<br />

#### 2.4.3 Information about device:
(2.4.2 return devices with indexes and device_iden)

`> pb device <DEVICE_IDEN>` or

`> pb -d <INDEX>`

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

### 2.5 `delete`

#### 2.5.1 Delete configured api key:

`> pb delete key` or

`> pb -del -k`

`> pb remove -k`

`> pb -r -k`

<br />

#### 2.5.2 Delete specific push with different writing styles:

`> pb -del push <PUSH-ID>` or

`> pb delete -p <PUSH-ID>` or

`> pb -r -p <PUSH-ID>`

<br />

#### 2.5.3 Delete device:

`> pb delete device <DEVICE-ID>` or

`> pb -del -d <DEVICE-ID>`

<br />

#### 2.5.4 Delete chat:

`> pb delete chat <CHAT-ID>` or

`> pb -r -c <CHAT-ID>`

<br />

#### 2.5.5 Delete subscription:

`> pb delete subscription <SUBSCRIPTION-ID>` or

`> pb remove -s <SUBSCRIPTION-ID>`

<br />

### 2.6 `chat`

#### 2.6.1 Create a new chat:

`> pb chat someone@example.com` or

`> pb -c someone@example.com`

<br />

#### 2.6.2 Mute / unmute a chat:

MUTE possibilities: `mute` / `true` / `1` <br />
UNMUTE possibilities: `unmute` / `false` / `0`

`> pb chat <CHAT-ID> mute`

`> pb -c <CHAT-ID> false`