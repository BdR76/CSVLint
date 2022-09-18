# generate csv with unicode characters
# ------------------------------------
# Generate random data with different unicode characters
# for testing syntax highlighting and unicode/utf8
# 
# Bas de Reuver - bdr1976@gmail.com (sep 2022)

import pandas as pd
import random
import math
import datetime
from datetime import timedelta
import string

# constants
FILE_NAME = "random_data_unicode.csv"

UNICODE_RANGE_1 = 0x4e00 # CJK Unified Ideographs 4E00-9FFF Common
UNICODE_RANGE_2 = 0x9fff

#UNICODE_RANGE_1 = 0x0400 # Cyrillic 0400-04FF
# UNICODE_RANGE_2 = 0x04ff

#UNICODE_RANGE_1 = 0x0e00 # Thai 0E00—0E7F
#UNICODE_RANGE_2 = 0x0e7f

testdate = datetime.datetime.now()
curryear = testdate.year

# random digits as string
def random_digits_string(c_min, c_max=None, c_thou=None):
    # how many digits
    howmany = c_min
    if c_max != None:
        howmany = random.randint(c_min, c_max)

    # built random digits string
    retval = ""
    for i in range(1, howmany+1):
        retval += str(random.randint(0, 9))

    # add thousand separators
    if c_thou != None:
        retval = retval.lstrip("0")
        # insert thousand separators
        for i in range(len(retval)-3, 0, -3): # reversed, step -1
            retval = retval[:i] + c_thou + retval[i:]
    return retval

# random date as string
def random_datetime_string(mask, yr_min=None, yr_max=None):
    year1 = curryear
    year2 = curryear
    if yr_min != None:
        if yr_min < 0:
            year1 = curryear + yr_min
        else:
            year1 = yr_min
            
    year2 = (curryear if yr_max == None else yr_max)

    # random date
    r_year = random.randint(year1, year2)
    r_month = random.randint(1, 12)
    r_daymax = (30 if r_month in [4,6,9,11] else (28 if r_month == 2 else 31))
    r_day = random.randint(1, r_daymax)

    r_hour = random.randint(0, 23)
    r_min = random.randint(0, 59)
    r_sec = random.randint(0, 59)
    
    rnddate = datetime.datetime(r_year, r_month, r_day, r_hour, r_min, r_sec)
    #print ("TESTING %d : %d : %d" % (r_hour, r_min, r_sec))

    return rnddate.strftime(mask)

# random unicode characters
def random_unicode(c_min, c_max=None):
    # text how long
    howlong = c_min
    if c_max != None:
        howlong = random.randint(c_min, c_max)

    # built random unicode characters string
    retval = ""
    for idx in range(0, howlong):
        rand_char = random.randint(UNICODE_RANGE_1, UNICODE_RANGE_2)
        retval += chr(rand_char)

    return retval

# create an Empty DataFrame object
df = pd.DataFrame()

print("Generating unicode columns:")

for col in range(1, 10+1):
    tmpval = []
    for row in range(1, 100):
        # differnt columns values
        val = None
        # integers
        if col == 1:
           val = ("%04d" % row)
        elif col == 2:
            val = random_datetime_string("%Y-%m-%d %H:%M:%S", -3)
        elif col == 3:
            val = random_unicode(5)
        elif col == 4:
            val = ("%s-%s" % (random_unicode(5), random_unicode(5)))
        elif col == 5:
            r_int = random.randint(10, 12)
            val = ("%d.%s" % ( r_int, random_digits_string(3) ) )
        elif col == 6:
            r_int = random.randint(20, 50)
            val = ("%d.%s" % ( r_int, random_digits_string(1) ) )
        elif col == 7:
            r_int = random.randint(10, 70)
            val = ("%d.%s" % ( r_int, random_digits_string(1) ) )
        elif col == 8:
            val = ("%s %s - %s" % (random_unicode(2), random_unicode(3), random_unicode(3)))
        elif col == 9:
            val = random_unicode(2)
        elif col == 10:
            val = ("%d年%d月%d日" % (random.randint(1990, 2020), random.randint(1, 12), random.randint(1, 28)))
        else:
            val = "..."

        # add value to column
        tmpval.append(val)

    # determine column name
    colname = ("dummy_data_%d" % col)
    
    # add column to dataframe
    df[colname] = tmpval

# Generating columns iss ready
print("")
print("ready.")

# Observe the result
print(df)

# csv write new output
df.to_csv(FILE_NAME, sep='\t', na_rep='', header=True, index=False, encoding='utf-8')


