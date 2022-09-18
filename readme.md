CSV Lint - Notepad++ plugin
===========================
![Release version](https://img.shields.io/github/v/release/BdR76/CSVLint) ![GitHub all releases](https://img.shields.io/github/downloads/BdR76/CSVLint/total) ![GitHub latest release](https://img.shields.io/github/downloads/BdR76/CSVLint/latest/total)  

CSV Lint is a plug-in for [Notepad++](http://notepad-plus-plus.org/) which
adds syntax highlighting to [comma-separated values](https://en.wikipedia.org/wiki/Comma-separated_values)
(csv) and fixed width data files to make them more readable. It can also
detect technical data errors and fix datetime and decimal formatting errors.

![preview screenshot](/csvlint_preview.png?raw=true "CSVLint plug-in preview")

Use CSV Lint for metadata discovery, technical data validation and
reformatting on tabular data files. It is _not_ meant to be a replacement for
spreadsheet programs like Excel or SPSS, but rather it's a quality control
tool to examine, verify or polish up a dataset before further processing.

With CSV Lint you can take a dataset and:

* Scan for metadata, i.e. detect columns and datatypes
* Create schema.ini based on metadata
* Validate data against schema.ini
* Convert datetime/decimal values to different formats
* Convert between comma, semicolon, tab separated, fixed width formats
* Split valid/invalid values into two separate columns
* Count unique values of one or more columns
* Convert csv data to SQL insert script

CSV Lint is stable and usable for most general use-cases, but it is a work-in-progress, 
so if you encounter any bugs or unexpected output I encourage you to [report issues here](https://github.com/BdR76/CSVLint/issues).
CSVLint is based on a prototype project [Dataset MultiTool](https://github.com/BdR76/datasetmultitool)

<p align="center">
<b>If you like the CSV Lint plug-in you can support the project by buying me a coffee!</b><br>
<a href="https://www.buymeacoffee.com/bdr76" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
</p>

How to install
--------------

The CSV Lint plugin is available in Notepad++ in the Plugins Admin menu.

* Install [Notepad++](https://notepad-plus-plus.org/)
* In Notepad++ go to menu item `Plugins > Plugins Admin...`
* On tab `Available` search for `csv lint`
* Check the checkbox and press `Install` button
* Click `Yes` to quit Notepad++ and "continue the operations"
* Click `Yes` on the Windows notification "Allow app to make changes"

Note: the plug-in is available in the Plugins Admin menu starting with
Notepad++ v8.1.9.1. If you have a Notepad++ version older than v8.1.9.1 or
want to install the plug-in manually:

* Go to the [releases page](https://github.com/BdR76/CSVLint/releases)
* Find the latest release
* 32bit version; unzip [CSVLint.dll (32bit)](../../releases/download/0.4.5.4/CSVLint_x86.zip/) to folder `.\Program Files (x86)\Notepad++\plugins\CSVLint\CSVLint.dll`.
* 64bit version; unzip [CSVLint.dll (64bit)](../../releases/download/0.4.5.4/CSVLint_x64.zip/) to folder `.\Program Files\Notepad++\plugins\CSVLint\CSVLint.dll`.
* restart Notepad++

How to use it
-------------

1. Open your dataset in [Notepad++](http://notepad-plus-plus.org/)
2. Open the "CSV Lint window" from the plug-in menu or toolbar
3. Press "Detect columns" to automatically detect format
4. Optionally, manually enter or adjust metadata
5. Press "Validate data" to detect any data errors

If there are no errors in the data, you can click "Reformat data" for data
reformatting options, or select "Convert to SQL" menu item to generate an
SQL insert script.

Also see this quick tour video, which shows how the plug-in works.

[![Watch video, CSV Lint plug-in features oveview](http://img.youtube.com/vi/_Me-ICCBu60/mqdefault.jpg)](http://www.youtube.com/watch?v=_Me-ICCBu60 "CSV Lint plug-in Notepad++")

Schema.ini
----------
The metadata uses the standard schema.ini format, see documentation
[here](https://docs.microsoft.com/en-us/sql/odbc/microsoft/schema-ini-file-text-file-driver?view=sql-server-ver15)

When you open a csv file the plug-in try to determine the column meta data.
It will first look for a `schema.ini` file in the same folder as the data
file, and check to see if it contains a section with the filename. If the file
or section doesn't exist, it will scan the data and try to infer the columns
and datatypes. You can manually change the meta data and press the blue disk
icon to save it to a `schema.ini` file in the same folder as the data file for
later use.

See schema.ini example below:

	[mydata.csv]
	Format=TabDelimited
	DateTimeFormat=dd-mm-yyyy
	DecimalSymbol=.
	NumberDigits=2
	Col1=OrderId Integer Width 8
	Col2=Price Float Width 7
	Col3=PartName Text Width 50
	Col4=OrderDate DateTime Width 10

Format can be `TabDelimited` for tabs, `CSVDelimited` for commas, for any other
delimiter use for example `Format=Delimited(;)`. Use `FixedLength` for fixed
width text files and set the `Width` for each column.

DateTimeFormat is not case sensitive and uses `dd/mm/yyyy` or
`yyyy-mm-dd hh:nn:ss` etc.

DecimalSymbol can be either `.` or `,` and CSV Lint will assume the thousand
separators symbol is the opposite of the DecimalSymbol. Define the maximum
decimals digits for example `NumberDigits=2` for values like "1.23" or "-45.67"
etc.

Validating 
----------
When you press "Validate data", the input data will be checked for technical
errors, based on the metadata in the textbox on the left. The line numbers of
any errors will be logged in the textbox on the right. It will check the input
data for the following errors:

* Values that are too long, example value "abcde" when column is "Width 4"
* Non-numeric values in numeric columns, example value "n/a" when column datatype is Integer
* Incorrect decimal separator, example value "12.34" when DecimalSymbol is set to comma
* Too many decimals, example value "12.345" when NumberDigits=2.
* Incorrect date format, example value "12/31/2018" when DateTimeFormat=dd/mm/yyyy

Roadmap/goals
-------------
The CSV Lint plugin is work-in-progress, here is list of features I want to add (~~strikethrough~~ is done)

- [x] ~~Convert datetime values to different formats~~
- [x] ~~Convert decimal symbol to point/comma~~
- [x] ~~Toggle between comma, semicolon, tab separated formats~~
- [x] ~~Improve file reading, to process/edit large files (>1MB)~~
- [x] ~~Count unique values based on column(s)~~
- [x] ~~Allow format masks per individual column~~
- [x] ~~Support quoted strings~~
- [x] ~~Support two-digit year date values~~
- [ ] Support for currency/thousand separator "12.345,00" or "1,250,000.00" etc.
- [x] ~~Load/save schema.ini~~
- [x] ~~Improve automatic datatype detection~~
- [x] ~~Add feature GUI click to jump to error line~~
- [ ] Support code=label values (in schema.json?) + error check + replace-code-with-label
- [ ] Support FrictionlessData schema.json format
- [ ] Improve GUI instead of plain text
- [x] ~~Add feature generate scripts (SQL, Python, R)~~
- [x] ~~Syntax highlighting, display columns as [colors](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad/)~~
- [ ] Search for value in column, search next empty/non-empty in column
- [ ] Search/replace in single column, multiple columns (option only replace n-th occurance? example "datetime(2008, 1, 1, 12, 59, 00)" replace ', ' with '-' or ':')
- [ ] Search/replace only n-th occurance? Or only empty occurance?
- [x] ~~Split column into new column ("123/456" -> "123", "456")~~

Trouble shooting / Known issues
-------------------------------
* When you press the "Validate Data" button after editing the data file, the
text and metadata are not always synchronised immediately. if you get
unexpected validation results, try saving the datafile or refreshing the meta
data before clicking "Detect columns".

* When you press "Detect columns" the datetime format of the data isn't always
detected correctly. Especially when the data contains values like `05/06/2021`
the order of day and month can be incorrect. You can adjust it manually to
match your data before pressing the "Validate data" button.

* When you select Language > CSVLint to enable the syntax highlighing colors,
or change the metadata manually, the column colors aren't always updated
immediately. Click inside the textfile or switch tabs to a different file and
then back and it should display correctly.

Acknowledgements
----------------
With thanks to:

* kbilsted for providing the excellent
[NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net)
* jokedst, [CsvQuery](https://github.com/jokedst/CsvQuery) was the inspiration for converting [datasetmultitool](https://github.com/BdR76/datasetmultitool) to a Notepad++ plug-in
* [DonHo](https://github.com/donho) for creating Notepad++ and everyone at the [Notepad++ Community forum](https://community.notepad-plus-plus.org/) for providing feedback
* Anyone who shared their [plug-in on GitHub](https://github.com/search?l=C%23&q=notepad%2B%2B+plugin&type=Repositories)

The CSV Lint plug-in couldn't have been created without their source examples, suggestions and valuable feedback.

Disclaimer
----------
This software is free-to-use and it is provided as-is without warranty of any
kind, always back-up your data files to prevent data loss.  
The [test data](./testdata/), examples and screenshots provided in this github
repository do not contain real data, it is
[randomly generated](https://github.com/BdR76/RandomValuesNPP) test data.

History
-------
15-dec-2019 - v0.1 first release  
02-may-2021 - v0.2 reformat data, double-click jumps to line, various bugfixes  
25-aug-2021 - v0.3 quoted string values, syntax highlighting, SQL export  
26-sep-2021 - v0.4 performance improvement, save/load metadata, split column option, count unique values  
17-oct-2021 - v0.4.1 various bugfixes  
29-oct-2021 - v0.4.2 startup error "CSVLint.xml is missing" fixed, toggle toolbar icon, clean up settings  
12-nov-2021 - v0.4.3 dark mode icons and colors, save form settings, documentation, bugfixes  
19-dec-2021 - v0.4.4 Support large integer values, various bugfixes  
12-mar-2022 - v0.4.5 Render on background thread, transparent cursor line, convert to XML/JSON, generate metadata, quotes reformat, help icons, bugfixes  
27-apr-2022 - v0.4.5.1 bugfix for Notepad++ 8.4 Lexer v5 update  
03-jun-2022 - v0.4.5.2 Another Lexer v5 bugfix, generate Python script  
25-jul-2022 - v0.4.5.3 Manually detect columns, improved fixed width support  
14-aug-2022 - v0.4.5.4 Improved datatype and datetime mask detection, various bugfixes  

BdR©2021 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
