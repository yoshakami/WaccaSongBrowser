# WaccaSongBrowser
Table and Message editor. It has several menus listed below

## MusicParameterTable
This is a nice tool used to search for existing songs metadata and edit them.<br>
you can sort all categories to your liking quickly with it.<br>
also edits UnlockMusicTable and UnlockInfernoTable to set the "new" attribute and inject new id<br><br>
<img width="1340" height="713" alt="image" src="https://github.com/user-attachments/assets/656d6a69-a878-4ca6-8c0d-4cd467878e54" />


## ConditionTable
Here you can edit conditions for unlockables and also edit the unlockable ID<br>
it edits TotalResultItemJudgementTable and shows all other loaded files for read only<br><br>
<img width="1340" height="713" alt="image" src="https://github.com/user-attachments/assets/818f2454-d77d-4921-a50d-a169800ba49e" />


## Message
You can convert all japanese strings of the game to Wacca.txt (or .po) and back from the txt to a new folder with all the corresponding uassets.

## TrophyTable
You can onvert all japanese strings of the trophy names to Trophy.txt, and back from the txt to a new uasset

## GradeTable
You can convert all japanese strings of the title names to Titles.txt, and back from the txt to a new uasset

##
in order to grant yourself a new icon:
1. (artemis) you need to edit wacca_item in mysql database, and add item id and type 6 (icon)
2. (everywhere) you need to drag and drop ConditionTable in WaccaSongBrowser, and inject a new condition.
these instructions also work for every other item in ConditionTable

## it does not add jackets! use WsongInject to add a new jacket

## update notes
you can now apply as much filters as you want as long as you enable them! <br>
you can still take the hot shortcut of pressing enter (with the keyboard) on any filter to any search with this one only (for MusicParameterTable) <br>
MusicParameterTable, Message, Condition, trophy, and title are fully functionnal. <br>
future updates will include icon injector, plate background injector, and title injector

## how to edit songs
Drag and drop MusicParameterTable.uasset onto the window (found in WindowsNoEditor/Mercury/Content/Table)
<br>You're then free to use filters or manually enter the song ID you wanna see.

## how to save
Everytime you change songID (through clicking Search, Next, Previous, or through clicking Validate), the program will check wheter save in RAM is checked. if it is, on next save, it will write all edits done to all songID, even new ones.
<br> If autosave is checked, the program will overwrite the file each time you change song ID (I do not suggest checking it because it'll write around 170MB if you change songs 200 times)
<br> the save button gets every value on screen, saves it in ram, then overwrites the file.
<br> so, if you messed up an edit, (even if RAM save is checked) just uncheck save in RAM, change song ID, and change again to the song ID you wanna edit. It'll work because it only saves when changing song ID, not before.

## how new works
the "new" attribute applied to songs is only valid for 28 days (the game checks UnlockMusicTable > ItemActivateStartTime), so you would need to check it again later if you want your song to stay in that category. <br>
also, If you want your song to appear in the new category, it must have the "version" set to Wacca Reverse. if you just want your song to have the "new" UI on the jacket, ignore this instruction
