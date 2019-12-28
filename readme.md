CSV Lint - Notepad++ plugin
===========================

CSVLint is a plug-in for Notepad++ to validate and reformat tabular data files.
You can take a dataset and:

* Validate data
* Create schema.ini based on data

![preview screenshot](/csvlint_preview.png?raw=true "CSVLint plug-in preview")

How to use it
-------------

1. Open your dataset in Notepad++
2. Press "Refresh from data" to automatically detect format
3. Optionally, manually enter or adjust metadata
4. Press "Validate data" to detect any data errors

How to install
--------------
The distributed output file is `CSVLint.dll`. In your \Notepad++\plugins\ folder, create a new folder `CSVLint` and place the .dll file there, so:

* copy the file [.\CSVLintNppPlugin\bin\Release\CSVLint.dll](/CSVLintNppPlugin/bin/Release/)
* to new folder .\Program Files (x86)\Notepad++\plugins\CSVLint\CSVLint.dll
	
For the 64-bit version it is the same, except the output file is in [\Release-x64\](/tree/master/CSVLintNppPlugin/bin/Release-x64/) and Notepad++ is in the `.\Program Files\Notepad++\` folder.

Schema.ini
----------
The metadata uses the standard schema.ini format, see documentation [here](https://docs.microsoft.com/en-us/sql/odbc/microsoft/schema-ini-file-text-file-driver?view=sql-server-ver15)

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

Format can be `TabDelimited` for tabs, `CSVDelimited` for commas, for any other delimiter use for example `Format=Delimited(;)`.
Use `FixedLength` for fixed width text files and set the `Width` for each column.

DateTimeFormat is not case sensitive and uses `dd/mm/yyyy` or `yyyy-mm-dd hh:nn:ss` etc.

DecimalSymbol can be either `.` or `,` and CsvLint will assume the thousand separators symbol is the opposite of the DecimalSymbol.
Define the maximum decimals digits for example `NumberDigits=2` for values like "1.23" or "-45.67" etc.

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

Missing features
----------------
The CSVLint plugin is work-in-progress, here is list of still missing features

* Improve GUI instead of plain text
* Convert datetime values to different formats
* Convert decimal symbol to point/comma
* Count unique values based on column(s)
* Allow format masks per individual column
* Support quoted strings
* Support two-digit year date values
* Support FrictionlessData schema.json format
* Load/save schema.ini/json
* Improve automatic datatype detection
* Add feature GUI click to jump to error line
* Add feature generate import scripts (SQL, SPSS, Python(?), XML+column=xpath?)
* Add feature display columns as [colors](https://community.notepad-plus-plus.org/topic/13921/setting-text-color-via-a-net-plugin) (custom Lexer?)

Acknowledgements
----------------
Thanks to kbilsted for providing the excellent [NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net)
and [jokedst](https://github.com/jokedst/CsvQuery) and others for sharing their plugins on github.
This plugin couldn't have been created without their examples.

History
-------
15-dec-2019 - first release v0.1

BdR©2019 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
