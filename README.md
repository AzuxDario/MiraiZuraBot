# Mirai Zura bot
<p align="center">
<a href="https://travis-ci.org/AzuxDario/MiraiZuraBot"><img src="https://travis-ci.org/AzuxDario/MiraiZuraBot.svg?branch=master" alt="Build status"></img></a>
<a href="https://github.com/AzuxDario/MiraiZuraBot/pulls?q=is%3Apr+is%3Aclosed"><img src="https://img.shields.io/github/issues-pr-closed-raw/AzuxDario/MiraiZuraBot" alt="Closed pull requests"></img></a>
<a href="https://github.com/AzuxDario/MiraiZuraBot/blob/master/LICENSE"><img src="https://img.shields.io/github/license/AzuxDario/MiraiZuraBot" alt="License"></img></a>
</p>

Simple Discord bot developed for polish community server of Love Live! series.

# Bot features
Bots has features like system which allows members to auto assign roles, simple usage of School Idol Tomodachi API, counting emoji and advertising characters/seiyuu birthdays.

## Emoji counter
Bot counts emojis used on channels and used in reactions and store emoji's count in database.

## Birthdays
Bot has advertising feature, which allows to auto post messages every year on certain date. It's used to send birthday messages on server.

# Commands
Since it's polish bot, most commands are in polish.
## Emoji
  * policzEmoji - Show emoji usage counters.
## Advertising
  * aktywneTematyUrodzin - Show active topics of birthdays on certain channel.
  * tematyUrodzin - Show available topic of birthdays.
  * wlaczTematUrodzin - Enable birthday topic.
  * wylaczTematUrodzin - Disable birthday topic.
## SIF (School Idol Festival)
  * idolka - Shows idol informations based on her name.
  * karta - Shows SIF card based on id and information about idolisation.
  * losowaIdolka - Shows random idol.
  * losowaKarta - Shows random card.
  * obecnyEventEN - Shows info about current event in SIF EN.
  * obecnyEventJP - Shows info about current event in SIF JP.
  * nastepnyEventEN - Shows info about next event in SIF EN.
  * nastepnyEventJP - Shows info about next event in SIF JP.
## Roles
  * pokazRole - Shows the roles that you can assign yourself on server.
  * nadajRole - Assign role to you from the role list.
  * odbierzRole - Remove role from you from the role list.
  * dodajRole - Adds a role to the role list that server members can assign.
  * usunRole - Removes a role from the role list that can be assigned by server members.
## Help
  * help - Shows help.
## Special
  * ping - Check ping
 
# Used libraries
  * [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus)
  * [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
  * [WebSocket4Net](https://github.com/kerryjiang/WebSocket4Net)

# Used APIs
  * [School Idol Tomodachi](https://github.com/MagiCircles/SchoolIdolAPI/wiki/LoveLive!-School-Idol-API)


# Running
If you want to run this bot you need to do following steps.
## API keys
You need to create 2 files 'release.json' and 'debug.json' in main directory, using 'release.debug.example.json' as template. These files are used to run in release and debug mode respectively. They contains Discord Bot token, id of creator, and prefix for commands.
## Database
You need to create file called 'DynamicDatabase.sqlite' in main directory, using 'DynamicDatabase.example.sqlite' as template.