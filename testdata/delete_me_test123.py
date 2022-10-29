# Python - read csv with datatypes
# CSV Lint plug-in: v0.4.6.2
# File: bpfile.txt
# Date: 29-Oct-2022 15:10
#
# NOTE:
# This is a generated script and it doesn't handle all potential data errors.
# The script is meant as a starting point for processing your data files.
# Adjust and expand the script for your specific data processing needs.
# Always back-up your data files to prevent data loss.

# Library
import os
import numpy as np
import pandas as pd

# working directory and filename
os.chdir("C:\\Users\\bas_d\\source\\repos\\CSVLintNppPlugin\\testdata")
filename = "C:\\Users\\bas_d\\source\\repos\\CSVLintNppPlugin\\testdata\\bpfile.txt"

# column datatypes
# NOTE: using colClasses parameter doesn't work when for example integers are in quotes etc.
# and read.csv will mostly interpret datatypes correctly anyway
col_types = {
    "StudyNr": str,
    "Age": np.int64,
    "Visit": str,
    "Position": str,
    "Measure": np.int64,
    "Systolic": np.int64,
    "Diastolic": np.int64,
    "Pulse": np.int64
}

# datetime columns; '%Y/%m/%d', '%H:%M:%S'
col_dates = ['Date', 'Time']

# read csv file
#df = pd.read_csv(filename, sep='|', decimal='.', header=0, parse_dates=col_dates, dtype=col_types)
df = pd.read_csv(filename, sep='|', decimal='.', header=0, parse_dates=col_dates)

# NOTE: Python treats NaN values as float, thus columns with Int64+NaNs are converted to float,
# you can convert them to string and then rstrip to undo the float '.0' parts
#df['Age'] = df['Age'].astype(str).str.rstrip('0').str.rstrip('.')
#df['Measure'] = df['Measure'].astype(str).str.rstrip('0').str.rstrip('.')
#df['Systolic'] = df['Systolic'].astype(str).str.rstrip('0').str.rstrip('.')
#df['Diastolic'] = df['Diastolic'].astype(str).str.rstrip('0').str.rstrip('.')
#df['Pulse'] = df['Pulse'].astype(str).str.rstrip('0').str.rstrip('.')

# NOTE: Python treats datetime columns that also have NaN/string values as string

# double check datatypes
print(df.dtypes)

# --------------------------------------
# Data transformation suggestions
# --------------------------------------

# reorder or remove columns
df = df[[
    'StudyNr',
    'Age',
    'Visit',
    'Position',
    'Measure',
    'Date',
    'Time',
    'Systolic',
    'Diastolic',
    'Pulse'
]]

# date to string format MM/dd/yyyy
#df['Date'] = df['Date'].dt.strftime('%m/%d/%Y')

# replace codes with labels
lookuplist = {'ZIT': 1, 'STA': 2, 'LIG': 3}
df['Position_code'] = df['Position'].map(lookuplist)

# csv write new output
filenew = "output.txt"
df.to_csv(filenew, sep='|', decimal=',', na_rep='', header=True, index=False, encoding='utf-8')
