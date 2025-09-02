# WaccaSongBrowser
MusicParameterTable and Message editor
This is a nice tool used to search for existing songs metadata and edit them.
you can sort all categories to your liking quickly with it.

Message editor is used to convert the Message folder into a txt, that can be converted back to a new message folder.
<img width="1184" height="697" alt="image" src="https://github.com/user-attachments/assets/ff07d6a4-1999-4989-ba09-504572842146" />

## it does not add new entries!! use WsongInject to add a new song instead

## update notes
you can now apply as much filters as you want as long as you enable them! <br>
you can still take the hot shortcut of pressing enter (with the keyboard) on any filter to any search with this one only

## how to edit songs
Drag and drop MusicParameterTable.uasset onto the window (found in WindowsNoEditor/Mercury/Content/Table)
<br>You're then free to use filters or manually enter the song ID you wanna see.
<br>In case you haven't guessed already, you don't need to edit the filters when editing a song. The right part of the program is only used to change song ID or save.

## how to add a song
Manually enter a songID that does not exist, and click validate.
<br>Upon next save, it will click add this row to the bottom of the uasset with all values specified in the window.

## how to save
Everytime you change songID (through clicking Search on a filter, or through clicking Validate), the program will check wheter save in RAM is checked. if it is, on next save, it will write all edits done to all songID, even new ones.
<br> If autosave is checked, the program will overwrite the file each time you change song ID (I do not suggest checking it because it'll write around 170MB if you change songs 200 times)
<br> the save button gets every value on screen, saves it in ram, then overwrites the file.
<br> so, if you messed up an edit, (even if RAM save is checked) just uncheck save in RAM, change song ID, and change again to the song ID you wanna edit. It'll work because it only saves when changing song ID, not before.
