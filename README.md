# Pushbullet CLI

## Setup

Set application's path in system's environment "Path" variables.

## Utilization

First of all, create your API key from [Pushbullet](http://pushbullet.com) and set it using: <br />
`> pb key <API-KEY>`

Then you're able to push text to your devices:

`> pb text "Hello world!"`

or

`> pb link http://pushbullet.com "Title of link" "Description of Link"`

You can drop arguments by pass an empty string like:

`> pb url http://pushbullet.com "" "Description of Link"`

View your last 3 pushes (returns as json):

`> pb list 3` or <br />
`> pb list` (retuns last push)

Delete specific push:

`> pb --del push ujyLNXNNKHAsjxrgo17lyF`

Delete configured api key:

`> pb delete key`

<br />

These commands are available at the moment:

Command                   | Arguments                                                                                                                | Alias      | Description                  |
--------------------------|--------------------------------------------------------------------------------------------------------------------------|------------|------------------------------|
`key`                     | `<API-KEY>` for set empty for get <br /> remove key with `""` or with `delete` command                                   | `-k`       | Set or get API key.          |
`me`                      | /                                                                                                                        | `-i`       | Gets the current user.       |
`push` or `text`          | `<TEXT>` or `<TITLE> <TEXT>` <br /> `"Hello"` or `"Hello" "World!"`                                                      | `-p`, `-t` | Push text to all devices.    |
`link` or `url` or `link` | `<URL>` or `<URL> <TITLE>` or `<URL> <TITLE> <TEXT>` <br/> `http://pushbullet.com "Title of link" "Description of link"` | `-u`       | Push a link to all devices.  |
`list` or `list`          | `<LIMIT>` <br /> `5`                                                                                                     | `-l`       | List last e.g. 5 pushes.     |
`delete` or `--del`       | Delete push (`push`, `-p`) and iden <br /> e.g.: `ujyLNXNNKHAsjxrgo17lyF` <br />                                         | `-d`       | Delete push object with iden.|
