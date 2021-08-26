CSV Lint - Notepad++ plugin
===========================

CSV Lint is a plug-in for Notepad++ for metadata discovery, technical data
validation and reformatting on tabular data files.

Use CSV Lint to quickly detect any technical errors in csv data or fix datetime and decimal formatting.  
It is _not_ meant to be a replacement for spreadsheet programs like Excel or SPSS.

With CSV Lint you can take a dataset and:

* Scan for metadata, i.e. detect columns and datatypes
* Create schema.ini based on metadata
* Validate data against schema.ini
* Convert datetime/decimal values to different formats
* Convert between comma, semicolon, tab separated, fixed width formats
* Convert csv data to SQL insert script

![preview screenshot](/csvlint_preview.png?raw=true "CSVLint plug-in preview")

CSV Lint is stable and usable for most general use-cases, but it is a work-in-progress.
If you encounter any bugs or unexpected output I encourage you to [report issues here](https://github.com/BdR76/CSVLint/issues).

CSVLint is based on a prototype project [Dataset MultiTool](https://github.com/BdR76/datasetmultitool)

How to use it
-------------

1. Open your dataset in Notepad++
2. Open the "CSV Lint window" from the plug-in menu or toolbar
3. Press "Refresh from data" to automatically detect format
4. Optionally, manually enter or adjust metadata
5. Press "Validate data" to detect any data errors

If there are no errors in the data, you can click "Reformat data" for data reformatting options,
or select "Convert to SQL" menu item to generate an SQL insert script.

How to install
--------------
The distributed output file is `CSVLint.dll`. In your \Notepad++\plugins\ folder,
create a new folder `CSVLint` and place the .dll file there, so:

* copy the file [.\CSVLintNppPlugin\bin\Release\CSVLint.dll](/CSVLintNppPlugin/bin/Release/)  
 to new folder `.\Program Files (x86)\Notepad++\plugins\CSVLint\CSVLint.dll`
* copy the file [.\config\CSVLint.xml](/config/)  
to folder `%USERPROFILE%\AppData\Roaming\Notepad++\plugins\config\CSVLint.xml`

For the 64-bit version it is the same, except the output file is in the
[Release-x64](/CSVLintNppPlugin/bin/Release-x64/) folder and Notepad++ is
in the `.\Program Files\Notepad++\` folder.

Schema.ini
----------
The metadata uses the standard schema.ini format, see documentation
[here](https://docs.microsoft.com/en-us/sql/odbc/microsoft/schema-ini-file-text-file-driver?view=sql-server-ver15)

See schema.ini example below:

	[mydata.csv]
	Format=TabDelimited
	DateTimeFormat=dd-mm-yyyy
	DecimalSymbol=.
	NumberDigits=2
	Col1=OrderId Integer Width 8
	Col2=Quantity Integer Width 3
	Col3=PartName Text Width 50
	Col4=OrderDate DateTime Width 10

Format can be `TabDelimited` for tabs, `CSVDelimited` for commas, for any other
delimiter use for example `Format=Delimited(;)`. Use `FixedLength` for fixed
width text files and set the `Width` for each column.

DateTimeFormat is not case sensitive and uses `dd/mm/yyyy` or `yyyy-mm-dd hh:nn:ss` etc.

DecimalSymbol can be either `.` or `,` and CSV Lint will assume the thousand
separators symbol is the opposite of the DecimalSymbol. Define the maximum
decimals digits for example `NumberDigits=2` for values like "1.23" or "-45.67" etc.

Validating 
----------
When you press "Validate data", the input data will be checked for technical errors,
the line numbers of any errors will be logged in the textbox on the right.
It will check the input data for the following errors:

* Values that are too long, example value "abcde" when column is "Width 4"
* Non-numeric values in numeric columns, example value "n/a" when column datatype is Integer
* Incorrect decimal separator, example value "12.34" when DecimalSymbol is set to comma
* Too many decimals, example value "12.345" when NumberDigits=2.
* Incorrect date format, example value "12/31/2018" when DateTimeFormat=dd/mm/yyyy

Roadmap/goals
-------------
The CSV Lint plugin is work-in-progress, here is list of features I want to add (~~strikethrough~~ is done)

* ~~Convert datetime values to different formats~~
* ~~Convert decimal symbol to point/comma~~
* ~~Toggle between comma, semicolon, tab separated formats~~
* Improve GUI instead of plain text
* Improve file reading, to process/edit large files (>1MB)
* Count unique values based on column(s)
* Allow format masks per individual column
* ~~Support quoted strings~~
* Support two-digit year date values
* Support FrictionlessData schema.json format
* Load/save schema.ini/json
* Improve automatic datatype detection
* Add feature GUI click to jump to error line
* Add feature generate ~~scripts (SQL,~~ SPSS, Python(?), XML+column=xpath?)
* ~~Syntax highlighting, display columns as [colors](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad/)~~
* Search for value in column, search next empty/non-empty in column
* Search/replace in single column, multiple columns (option only replace n-th occurance? example "datetime(2008, 1, 1, 12, 59, 00)" replace ', ' with '-' or ':')
* Search/replace only n-th occurance? Or only empty occurance?
* Split column into new column ("123/456" -> "123", "456")
* Support code=label values (in schema.json?) + error check + replace-code-with-label

Acknowledgements
----------------
With thanks to:

* kbilsted for providing the excellent
[NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net)
* jokedst, [CsvQuery](https://github.com/jokedst/CsvQuery) was the inspiration for converting [datasetmultitool](https://github.com/BdR76/datasetmultitool) as a Notepad++ plug-in
* Anyone who shared their [plug-in on GitHub](https://github.com/search?l=C%23&q=notepad%2B%2B&type=Repositories)

The CSV Lint plug-in couldn't have been created without their source examples and suggestions.

Disclaimer
----------
This software is free-to-use and it is provided as-is without warranty of any kind,
always back-up your data files to prevent data loss.

History
-------
15-dec-2019 - v0.1 first release  
02-may-2021 - v0.2 reformat data, double-click jumps to line, various bugfixes  
25-aug-2021 - v0.3 quoted string values, syntax highlighting, SQL export

BdR©2021 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
