# Pushbullet CLI

## Setup

Set application's path in system's environment "Path" variables.

## Utilization

First of all, create your API key from [Pushbullet](http://pushbullet.com) and set it using: <br />
`> pb key <API-KEY>`

Then you're able to push text to your devices:

`> pb --text "Hello world!"`

or

`> pb link http://pushbullet.com "Title of link" "Description of Link"`

You can drop arguments by pass an empty string like:

`> pb -u http://pushbullet.com "" "Description of Link"`

View your last 3 pushes (returns as json):

`> pb list 3` or <br />
`> pb list` (retuns last push)

Delete specific push:

`> pb --del push ujyLNXNNKHAsjxrgo17lyF`

<br />

These commands are available at the moment:

Command                    | Arguments                                                                                                                | Alias      | Description                  |
---------------------------|--------------------------------------------------------------------------------------------------------------------------|------------|------------------------------|
`key`                      | `<API-KEY>` for set empty for get <br /> remove key with `""`                                                            | `-k`       | Set or get API key.          |
`me`                       | /                                                                                                                        | `i`        | Gets the current user.       |
`push`, `--text`           | `<TEXT>` or `<TITLE> <TEXT>` <br /> `"Hello"` or `"Hello" "World!"`                                                      | `-p`, `-t` | Push text to all devices.    |
`link`, `--url`, `--link`  | `<URL>` or `<URL> <TITLE>` or `<URL> <TITLE> <TEXT>` <br/> `http://pushbullet.com "Title of link" "Description of link"` | `-u`       | Push a link to all devices.  |
`list`, `--list`           | `<LIMIT>` <br /> `5`                                                                                                     | `-l`       | List last e.g. 5 pushes.     |
`delete`, `--del`          | Delete push (`push`, `-p`) and iden <br /> e.g.: `ujyLNXNNKHAsjxrgo17lyF`                                                | `-d`       | Delete push object with iden.|
