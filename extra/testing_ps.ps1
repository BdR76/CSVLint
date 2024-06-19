# PowerShell - read csv with datatypes
# CSV Lint plug-in: v0.4.6.7β2
# File: sort_test123.csv
# Date: 19-jun-2024 22:57
#
# NOTE:
# This is a generated script and it doesn't handle all potential data errors.
# The script is meant as a starting point for processing your data files.
# Adjust and expand the script for your specific data processing needs.
# Always back-up your data files to prevent data loss.

# working directory and filename
$pathname = "C:\Users\bas_d\source\repos\CSVLintNppPlugin\extra\"
$filename = $pathname + "sort_test123.csv"

# read csv data file
$csvdata = Get-Content -Path $filename | Select-Object -Skip 2 | ConvertFrom-Csv -Delimiter "`t"

# Explicit datatypes
# WARNING: PowerShell has very basic error handling for null or invalid values,
# so if your data file contains integer, decimal or datetime columns with empty or incorrect values,
# this script can throw errors, silently change values to '0' or omit rows in the output csv, so beware.
$line = 0
foreach ($row in $csvdata)
{
	$line += 1
	try {
		$row.test123           = [decimal]($row.test123 -replace '\.', '' -replace ',', '.')
		$row."Order date"      = [datetime]::parseexact($row."Order date", 'dd-MM-yy', $null)
		$row."Order price"     = [decimal]($row."Order price" -replace '\.', '' -replace ',', '.')
		$row."Parts group"     = $row."Parts group".Trim(' "')
		$row."Order description" = $row."Order description".Trim(' "')
	} catch {
		Write-Error "Data conversion error(s) on line $line" -TargetObject $row
	}
}

# Enumeration allowed values
$Parts_group_array = @("CARBODY", "CHASSIS", "CLIMATE", "ELECTRA", "ENGINE", "INTERIO")

# enumeration check invalid values
$line = 0
foreach ($row in $csvdata)
{
	# check invalid values
	$errmsg = ""
	if ($row."Parts group" -and !($Parts_group_array -contains $row."Parts group")) {$errmsg += "Invalid ""Parts group"" ""$($row."Parts group")"" "}

	# report invalid values
	$line = $line + 1
	if ($errmsg) {Write-Error "$errmsg on line $line" -TargetObject $row}
}

# Remove or uncomment the script parts below to filter, transform, merge as needed

# --------------------------------------
# Data filter suggestions
# --------------------------------------
# filter on value or date range
$csvdata = $csvdata | Where-Object { $_."Order date" -gt [DateTime]::Parse("1970-01-01") -and $_."Order date" -lt [DateTime]::Parse("2000-01-01") }

# Reorder or remove columns (edit code below)
$csvnew = $csvdata | ForEach-Object {
	[PSCustomObject]@{
		# Reorder columns
		test123           = $_.test123
		"Order date"      = $_."Order date".ToString("dd-MM-yyyy")
		"Order price"     = $_."Order price"
		"Parts group"     = $_."Parts group"
		"Order description" = $_."Order description"
		YesNo_int         = switch ($_."Parts group") {
						"CARBODY" {"111"}
						"CHASSIS" {"222"}
						"CLIMATE" {"333"}
						"ELECTRA" {"444"}
						default {$_}
					}
		#		bmi               = [math]::Round($_.Weight / ($_.Height * $_.Height), 2)
		#		cent_pat          = $_.centercode.SubString(0, 2) + "-" + patientcode # '01-123' etc
	}
}

# --------------------------------------
# Merge data example
# --------------------------------------
## Merge datasets in PowerShell requires custom external modules which goes beyond the scope of this generated script
##Install-Module -Name Join-Object
##$merged_df = Join-Object -Left $patients -Right $visits -LeftJoinProperty 'PATIENT_ID' -RightJoinProperty 'PATIENT_ID' -ExcludeRightProperties 'Junk' -Prefix 'R_' | Format-Table

# csv write new output
$filenew = $pathname + "output.txt"
#$csvnew | Export-Csv -Path $filenew -Encoding utf8 -Delimiter "`t" -NoTypeInformation

# alternatively, write as fixed width
$stream_out = New-Object System.IO.StreamWriter $filenew
foreach ($row in $csvnew)
{
	# {colnr,width} space etc, negative width means left aligned
	$stream_out.WriteLine(("{0,15} {1,-9} {2,6} {3,-9} {4,-17}" -f $row.test123, $row."Order date", $row."Order price", $row."Parts group", $row."Order description"))
}
$stream_out.Dispose()
