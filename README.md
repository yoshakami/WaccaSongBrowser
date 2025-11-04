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
List of conditions and their meaning:
ConditionTable		unless sepecified, the condition is validated if you have a higher difficulty or higher grade				
ID	Internal Name	Value1	Value2	Value3	Value4	Value5
-2	INVALID	unused				
-1	SYSTEM	unused				
0	INIT	unused				
1	LEVEL_UP_COUNT	unused				
2	TOTAL_WACCA_POINT	total amount of RP				
3	TOTAL_SCORE	total value of every song best score added together				
4	GRADE_COUNT	number of titles owned				
5	ICON_COUNT	number of icons owned				
6	SYMBOL_COLOR_COUNT	number of ring colors owned				
7	INPUT_SE_COUNT	number of sound effects owned				
8	UNLOCK_MUSIC_COUNT	number of musics the user need to unlock				
9	PLAY_COUNT	NORMAL | HARD | EXPERT | INFERNO  (choose only 1 of them)	PLAY | CLEAR | MISSLESS | FULLCOMBO | MASTER	total amount required, over all songs		
10	PLAY_COUNT_RATE	NORMAL | HARD | EXPERT | INFERNO  (choose only 1 of them)	S | SS | SSS | MASTER	total amount required, over all songs		
11	PLAY_MUSIC_COUNT	unused				
12	PLAY_MUSIC_COUNT_RATE	NORMAL | HARD | EXPERT | INFERNO  (choose only 1 of them)	S | SS | SSS | MASTER	total amount required, over all songs		
13	PLAY_MUSIC_SCORE_COUNT	unused				
14	PLAY_MUSIC_SCORE_COUNT_RATE	unused				
15	STAGE_UP	unused				
16	GAME_PLAY_COUNT	Total number of plays				
17	MULTI_PLAY_COUNT	Total number of MultiPlay				
18	GAME_PLAY_COUNT_MODE	VS | CP | FREETIME	Total number required			
19	PLAY_MUSIC_TAG_ALL_DIFFICULTY	unused				
20	PLAY_MUSIC_TAG_ALL_DIFFICULTY_RATE	unused				
21	1PLAY_MUSIC	NORMAL | HARD | EXPERT | INFERNO  (choose only 1 of them)	PLAY | CLEAR | MISSLESS | FULLCOMBO | MASTER			
22	1PLAY_MUSIC_RATE	NORMAL | HARD | EXPERT | INFERNO  (choose only 1 of them)	S | SS | SSS | MASTER			
23	PLAY_MUSIC_TAG	BingoID (TAG ID)	PLAY | CLEAR | MISSLESS | FULLCOMBO | MASTER	total amount required, over all songs		
24	RATE_COUNT_LOW	D | A (I guess other work too, also unlocks if you get a lower grade)	total amount required, over all songs			
25	TROPHY_COMPLETE	null	yeah litteraly all params are null. unlock all trophies and this will unlock the crown icon			
26	OPEN_MUSIC_TAG	unused				
27	PLAY_MUSIC_ID	MusicID	PLAY | CLEAR | MISSLESS | FULLCOMBO | MASTER	total amount required, over all songs		
28	USE_ICON_NUM	total amount required, (it only unlocks a title in vanilla called "accessory collector")				
29	MUSIC_CONTINUE_PLAY	Total amount required.				
30	MUSIC_SELECT_CANCEL	Total amount required.				
31	DATE	null. Play between ConditionGatableStartTime and End time to validate 				
32	PREFECTURES	Japanese prefecture name				
33	PLAY_AREA	Japanese Area Name				
34	PLAY_AREA_COMPLETE	null. I guess u need to play everywhere in japan				
35	GRADE_CUSTOMIZE_PLAY	null. (change your title then play?)				
36	MUSIC_ID_DIFFICULTY_FILL	MusicID	NORMAL | HARD | EXPERT | INFERNO	PLAY | CLEAR | MISSLESS | FULLCOMBO | MASTER		
37	PLAY_MUSIC_ID_RATE	MusicID	D | A | S | SS | SSS	Total count of THAT exact rate		
38	LOGIN	unused				
39	PLAY_MUSIC_TAG_DIFFICULTY_RATE	BingoID (TAG ID)	NORMAL | HARD | EXPERT | INFERNO	D | A | S | SS | SSS	Total amount required	
40	CONTINUOUS_LOGIN	Total days streak				
41	TOTAL_LOGIN	Total number required				
42	COMBO	unused				
43	1MISS_ALL_MARVELOUS	NORMAL | HARD | EXPERT | INFERNO				
44	GRADE_GET	TitleID	TitleID or null	TitleID or null	TitleID or null	TitleID or null
45	HIGH_SPEED	unused				
46	RATING	total rate value				
47	USER_PLATE_COUNT	Amount of Background plate owned				
48	GACHA_COUNT	20011 (Gacha Box ID ?)	number of pulls			
49	GET_ITEM	ItemID				
50	SCORE_MULTIPLE	unused				
51	SCORE_LAST_DIGITS	unused				
52	1PLAY_MUSIC_TAG_STATUS	unused				
53	TOTAL_GATE_POINT	Total Gate Points				
54	TOTAL_USER_LEVEL	user level				
55	FRIEND_COUNT	friend count				
56	BINGO_LINE_NUM	number of bingo lines completed				
57	BINGO_SHEET_NUM	Bingo Sheet Number				
58	GALLERY_MODE_PLAY	null. seems like auto validate between ConditionGatableStartTime and end time?				
59	MUSIC_ID_DIFFICULTY_RATE	unused				
60	MUSIC_ID_DIFFICULTY_STATUS	unused				
61	MUSIC_TAG_1PLAY	unused				
62	LEVEL_TOTAL_1PLAY	unused				
63	MUSIC_ID_RATE_1PLAY	MusicID	NORMAL | HARD | EXPERT | INFERNO	SSS+ | MASTER (other should work too)		
64	MUSIC_ID_STATUS_1PLAY	unused				


<img width="1340" height="713" alt="image" src="https://github.com/user-attachments/assets/818f2454-d77d-4921-a50d-a169800ba49e" />

## IconTable
You can view, browse, edit, and inject icons + their message text (even though you can drag and drop the message folder and use the txt to translate) <br>
if you want to add, or change an existing picture, use another tool (will add the name later)

## Message
You can convert all japanese strings of the game to Wacca.txt (or .po) and back from the txt to a new folder with all the corresponding uassets.

## TrophyTable
You can onvert all japanese strings of the trophy names to Trophy.txt, and back from the txt to a new uasset

## GradeTable (WIP)
You can convert all japanese strings of the title names to Titles.txt, and back from the txt to a new uasset (these two buttons use GradePartsTable) <br>
using other save buttons is useless as of now I'm not using GradePartsTable

## UserPlateBackgroundTable
Not implemented yet

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
future updates will include plate background injector, and title editor.

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
