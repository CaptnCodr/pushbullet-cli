# Pushbullet CLI

## Setup

Set application's path in system's environment "Path" variables.

## Utilization

First of all, create your API key from [Pushbullet](http://pushbullet.com) and set it using: <br />
`> pb --set-key <API-KEY>`

Then you're able to push text to your devices:

`> pb --text "Hello world!"`

or

`> pb -u http://pushbullet.com "Title of link" "Description of Link"`

You can drop arguments by pass an empty string like:

`> pb -u http://pushbullet.com "" "Description of Link"`

View your last 3 pushes (returns as json):

`> pb list 3` or <br />
`> pb list` (retuns last push)

<br />

These commands are available at the moment:

Command                    | Arguments                             | Alias | Description              |
---------------------------|---------------------------------------|-------|--------------------------|
`--set-key`                | `<API-KEY>` <br/> e.g.: o.Abc12345xyz | `-k`  | Set API key.             |
`--api-key`                | /                                     | `-a`  | Get configured API key.  |
`--text`, `--push`, `push` | `<TEXT>` or `<TITLE> <TEXT>` <br /> `"Hello"` or `"Hello" "World!"` | `-t`, `-p` | Push text to all devices.|
`--url`, `--link`, `link`  | `<URL>` or `<URL> <TITLE>` or `<URL> <TITLE> <TEXT>` <br/> `http://pushbullet.com "Title of link" "Description of Link"` | `-u` | Push a link to all devices.|
`list`, `--list`           | `<LIMIT>` <br /> `5`                  | `-l`  | List last e.g. 5 pushes  |
