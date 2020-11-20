# Pushbullet CLI

## Setup

Set application's path in system's environment "Path" variables.

<br />

## Utilization

First of all, creat your API key from [Pushbullet](http://pushbullet.com) and set it using: <br />
`> pb --set-key <YOUR API KEY>`

<br />

Then you're able to push text to your devices:

`> pb --text "Hello world!"`

<br />

All commands are available at the moment:

Command    | Alias | Description              |
-----------|-------|--------------------------|
`--set-key`| `-k`  | Set API key.             |
`--api-key`| `-a`  | Get configured API key.  |
`--text`   | `-t`  | Push text to all devices.|
