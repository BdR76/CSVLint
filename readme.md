CSVLint
=======

CSVLint is a plug-in for Notepad++ to validate and reformat tabular data files.
You can take a dataset and:

* Validate data
* Create schema.ini based on data

![preview screenshot](/csvlint_preview.png?raw=true "preview")

How to use it
-------------

1. Open your dataset in Notepad++
2. Press "Refresh from data" to automatically detect format
3. Optionally, enter or adjust it manually
4. Press "Validate data" to detect any data errors

Schema.ini
----------
The metadata uses the standard schema.ini format, see documentation [here](https://docs.microsoft.com/en-us/sql/odbc/microsoft/schema-ini-file-text-file-driver?view=sql-server-ver15)

See schema.ini example below:

	[mydata.csv]
	Format=TabDelimited
	DateTimeFormat=dd-mm-yyyy
	DecimalSymbol=.
	Col1=OrderId int Width 8
	Col2=Quantity Int Width 3
	Col2=PartName Text Width 50
	Col4=OrderDate dateint Width 10

Format can be `TabDelimited` for tabs, `CSVDelimited` for commas, for any other delimiter use for example `Format=Delimited(;)`.
Use `FixedLength` for fixed width text files and set the `Width` for each column.

DateTimeFormat is not case sensitive and uses `dd-mm-yyyy` or `mm/dd/yy` or `yyy-mm-dd hh:nn:ss` etc.

DecimalSymbol can be either `.` or `,` and CsvLint will assume the thousand separators symbol is the opposite of the DecimalSymbol.

Validating 
-------------
When you press "Validate data", the input data will be checked for technical errors,
the line numbers of any errors will be logged in the textbox on the right.
Note that input columns that are not in the output rows will not be checked.
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
* Support FrictionlessData schema.json format
* Load/save schema.ini/json
* Improve automatic datatype detection
* Improve error handling for incorrect input and non-data files

Acknowledgesments
-----------------
Thanks to kbilsted for providing the excellent NotepadPlusPlusPluginPack.Net
and others for sharing their plugins on github.
This plugin couldn't have been create without their examples.

History
-------
15-dec-2019 - first release v0.1

BdR©2019 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
