# VSCC
## Visual Simple Character Creator

Visual Simple Character Creator is a simple WPF app for managing your characters for DnD. It has most of the things you'd need:

* A tab for your basic things - Health, XP, Temp. HP, Stats, etc.
* A tab for extra information like your feats, biography, age, etc.
* A tab for simple visual inventory - see your items visually as icons with quick information rather than lines of text.
* A tab with a list of basic items available for DnD so you can add a new item into your inventory with a simple click of a mouse.
* A spellbook for your spells, organized into 10 different tabs for 10 levels of your spells.
* A tab with a list of spells available so you can add spells with one click of a mouse. Also lets you browse spells right from the app.
* A tab which allows a Roll20 integration. Roll your character stats straight into your Roll20 session!

And it's free! Just download and run the app, that's it.
It saves your characters as json files, so you can easilhy edit them even without the app.

## Roll20 Integration.
Roll20 integration works by creating an insecure websocket on your webbrowser page. The app will connect to said websocket. 
The websocket itself is used for a javascript backend that gets the chat from the Roll20 website and allows pasting into it. That's all it does.
The app itself creates a server that accepts the websocket. It gives commands to the underlying js socket. It is going to ask you for permissions (or at least if you are running windows your firewall is going to ask you).
