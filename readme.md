CSV Lint - Notepad++ plugin
===========================
![Release version](https://img.shields.io/github/v/release/BdR76/CSVLint) ![GitHub all releases](https://img.shields.io/github/downloads/BdR76/CSVLint/total) ![GitHub latest release](https://img.shields.io/github/downloads/BdR76/CSVLint/latest/total) [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/donate/?hosted_button_id=T8QZSFBNAPERL)   

CSV Lint plug-in for [Notepad++](http://notepad-plus-plus.org/) adds
syntax highlighting to [comma-separated values](https://en.wikipedia.org/wiki/Comma-separated_values)
(csv) and fixed width data files. It automatically detects column datatypes,
can detect data formatting errors and adjust datetime and decimal formats.

![preview screenshot](/csvlint_preview.png?raw=true "CSVLint plug-in preview")

You can use CSV Lint to analyse, validate, convert and reformat text data
files. It is _not_ meant to be a replacement for spreadsheet programs like
Excel or SPSS, but rather it's a quality control tool to examine, verify or
polish up a dataset before further processing.

With CSV Lint you can take a dataset and:

* Scan for metadata, i.e. detect columns and datatypes
* Create schema.ini based on metadata
* Validate data against schema.ini
* Convert datetime/decimal values to different formats
* Convert between comma, semicolon, tab separated, fixed width formats
* Split valid/invalid values into two separate columns
* Count unique values of one or more columns
* Sort based on one of the columns
* Convert csv data to SQL insert script or XML or JSON format.
* Generate Python, R or PowerShell scripts

CSV Lint doesn't require an internet connection and doesn't use any cloud service.
All data processing is done offline on the pc that runs Notepad++.
The plug-in is stable and works for most use-cases, if you encounter any bugs or
unexpected behaviour feel free to [report any issues here](https://github.com/BdR76/CSVLint/issues).
CSVLint is based on a prototype project [Dataset MultiTool](https://github.com/BdR76/datasetmultitool)

**If you find the CSV Lint plug-in useful you can buy me a coffee!** ☕  
[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate/?hosted_button_id=T8QZSFBNAPERL)

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
* 32bit version; unzip [CSVLint.dll (32bit)](../../releases/download/0.4.6.8/CSVLint_x86.zip/) to folder `.\Program Files (x86)\Notepad++\plugins\CSVLint\CSVLint.dll`.
* 64bit version; unzip [CSVLint.dll (64bit)](../../releases/download/0.4.6.8/CSVLint_x64.zip/) to folder `.\Program Files\Notepad++\plugins\CSVLint\CSVLint.dll`.
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

[![Watch video, CSV Lint plug-in features oveview](http://img.youtube.com/vi/_Me-ICCBu60/mqdefault.jpg)](http://www.youtube.com/watch?v=_Me-ICCBu60 "CSV Lint plug-in Notepad++") [![Watch video, CSV File: View, Add Column Colors and Sort Data using Notepad++](http://img.youtube.com/vi/k6w5BcaSqHc/mqdefault.jpg)](http://www.youtube.com/watch?v=k6w5BcaSqHc "CSV File: View, Add Column Colors and Sort Data using Notepad++")


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
The plug-in can be used as a CSV validator, press "Validate data" to check the
input data for technical errors, based on the metadata in the textbox on the
left. The line numbers of any errors will be logged in the textbox on the
right. It will check the input data for the following errors:

* Values that are too long, example value "abcde" when column is "Width 4"
* Non-numeric values in numeric columns, example value "n/a" when column datatype is Integer
* Incorrect decimal separator, example value "12.34" when DecimalSymbol is set to comma
* Too many decimals, example value "12.345" when NumberDigits=2.
* Incorrect date format, example value "12/31/2025" when DateTimeFormat=dd/mm/yyyy

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
- [x] ~~Support enumeration coded + error check~~
- [ ] Replace-code-with-label (using schema.json?)
- [ ] Support FrictionlessData schema.json format
- [ ] Improve GUI instead of plain text
- [x] ~~Add feature generate scripts (SQL, Python, R)~~
- [x] ~~Syntax highlighting, display columns as [colors](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad/)~~
- [ ] Search for value in column, search next empty/non-empty in column
- [x] ~~Search/replace in single column~~
- [x] ~~Split column into new column ("123/456" -> "123", "456")~~

Trouble shooting / Known issues
-------------------------------
* When you press the "Validate Data" button after editing the data file, the
text and metadata are not always synchronised immediately. if you get
unexpected validation results, try saving the datafile or refreshing the meta
data before clicking "Detect columns".

* When you press "Detect columns" the datetime format of the data isn't always
detected correctly. Especially when the data contains values like `05/06/2025`
the order of day and month can be incorrect. You can adjust it manually to
match your data before pressing the "Validate data" button.

* When you select `Language > CSVLint` to enable the syntax highlighing colors,
or change the metadata manually, the column colors aren't always updated
immediately. Click inside the textfile or switch tabs to a different file and
then back and it should display correctly.

Acknowledgements
----------------
With thanks to:

* kbilsted for providing the excellent
[NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net)
* jokedst, [CsvQuery](https://github.com/jokedst/CsvQuery) was the inspiration for converting [datasetmultitool](https://github.com/BdR76/datasetmultitool) to a Notepad++ plug-in
* [DonHo](https://github.com/donho) for creating Notepad++ and everyone at the [Notepad++ Community forum](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad?_=1669066219457) for providing feedback
* Anyone who shared their [plug-in on GitHub](https://github.com/search?l=C%23&q=notepad%2B%2B+plugin&type=Repositories)

The CSV Lint plug-in couldn't have been created without their source examples, suggestions and valuable feedback.
For a similar plug-in for Visual Studio Code check out [Rainbow CSV](https://github.com/mechatroner/vscode_rainbow_csv).

Disclaimer
----------
This software is free-to-use and it is provided as-is without warranty of any kind.  
Always back-up your data files to prevent data loss.  
All [test files](../../tree/master/testdata/), examples and screenshots provided in this github repository contain fictitious and 
[randomly generated](https://github.com/BdR76/RandomValuesNPP) data,
any resemblance to real-life cases is the result of chance.

BdR©2019-2025 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
