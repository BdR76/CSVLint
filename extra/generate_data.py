# generate data
# -------------
# Generate a large dataset for testing purposes
# Random but realistic data, incl. data entry errors
# Structure is based on a real medical dataset
# Change constants for file name and data size
# 
# Bas de Reuver - bdr1976@gmail.com (jan 2020)

import csv
import random
import math
import array as arr
import datetime
from datetime import timedelta

# constants
FILE_NAME = "cardio.txt"
TOTAL_LINES = 100000 # 1000 lines is approx. 92KB
#TOTAL_LINES = 24000000 # generate ~2GB takes about 3m40s depending on hardware factors
MAX_PERSONS = 2000
protos = ("XW duur", "CWRT X Watt", "Diabetes HIIT X - Y", "Hart XW duur", "Hart Interval X-YW  (2-2)", "Hart Interval X-YW  (3-4)", "Hart interval Y-X 4'-3'", "Interval X - Y", "Long HIIT XW 30 sec.", "Long HIIT XW 45 sec.", "Long HIIT XW 60 sec.", "Long Interval X - Y W", "Long duur X W", "ONCO XW duur", "ONCO Interval X- Y (3.00-4.00)", "transplantatie CWRT X watt")  # replace X and Y with random nrs 10,15,20,25..220
stages = ("Warmup", "Training", "Recovery")
durats = (130, 1760, 220)
hrtavg = (60, 100, 80)
# display progress when generating lots of lines (nearest base 10 factor)
line_upd = TOTAL_LINES / 15
fac10 = 10 ** (math.floor(math.log10(line_upd)))
PROGRESS_UPDATE = round( line_upd / fac10) * fac10

# Average time interval, 10 min to 30 min for 100000 lines, or smaller time steps when generating more lines
VISIT_AVG = int((600 * 100000) / TOTAL_LINES)
# Cannot be smaller than 1, so clamp value between 1 and 600
VISIT_AVG = min(max(VISIT_AVG, 1), 600)

# display start time
run_t0 = datetime.datetime.now() # start
print("%s Generate data, file=%s, lines=%d" % (run_t0, FILE_NAME, TOTAL_LINES))

# list of dates
alldates = []
testdate = datetime.datetime.now()
curryear = testdate.year
for i in range(int(TOTAL_LINES / 3)+1):
    sec = random.randrange(VISIT_AVG, (3*VISIT_AVG))
    testdate = testdate - datetime.timedelta(seconds=sec)
    # skip night-time
    if testdate.hour <= 7:
        testdate = testdate - datetime.timedelta(hours=14)
    # skip holidays
    test = testdate.strftime("%m%d")
    if test=="0101":
        testdate = testdate - datetime.timedelta(days=8)
    # skip weekends, 5=saturday
    if testdate.weekday() >= 5:
        testdate = testdate - datetime.timedelta(days=2)
    # date to add, glitch one date on purpose
    newdate = testdate
    if i == 100: newdate = testdate + 800 * datetime.timedelta(days=365)
    alldates.append(newdate)

random.shuffle(alldates)

# list of participants
patnrs = []
patsex = []
patdob = []
for i in range(MAX_PERSONS):
    n = random.randrange(10000000, 99999999)
    glitch = random.randrange(1, 50)
    patnrs.append(n)
    patsex.append(random.choice(['Male','Female']))
    rnddate = datetime.datetime(random.randrange(curryear-80,curryear-15), random.randrange(1,12), random.randrange(1,28))
    if i == 10: rnddate = rnddate - 900 * datetime.timedelta(days=365)
    dob = rnddate.strftime("%#d-%#m-%Y")
    if glitch == 1: dob = "NaN"
    patdob.append(dob)


# open file and write header
g = open(FILE_NAME, 'w', newline='')
w = csv.writer(g, delimiter=";")
w.writerow(('TestDate', 'Protocol', 'CustomId', 'SubjectId', 'BirthDate', 'Gender', 'TestStage', 'Duration', 'AvgHeartRate', 'AvgLoad'))

# write random lines to file
s = 99
for i in range(TOTAL_LINES):
    # progress message
    if (i % PROGRESS_UPDATE == 0) and (i > 0):
        print(i, "lines generated")
    
    if s > 3:
        # next time
        testdate = alldates.pop()
        # random values per set of three
        s = 1
        dat = testdate.strftime("%#d-%#m-%Y %H:%M:%S")
        # protocol description X or Y, replace with wattage 20..220
        pro = random.choice(protos)
        w2 = random.randrange(2,22) * 10
        w1 = w2 - (random.randrange(2,10) * 5)
        if w1 < 10: w1 = 10
        if "Y" not in pro: w2 = w1
        pro = pro.replace("X", str(w1))
        pro = pro.replace("Y", str(w2))
        # random patient
        idx = random.randint(0, len(patnrs)-1)
        pid = patnrs[idx]
        dob = patdob[idx]
        sex = patsex[idx]
        heartrate = random.randrange(80, 100) + ( 80.0 * (w1 / 220))
        # data entry error, extra space
        if i == 120:
            pid = " " + str(pid)
        # data entry error, entered current date for dob
        if i == 300:
            dob = testdate.strftime("%#d-%#m-%Y")
        # glitch error on purpose
        glitch = random.randrange(1, 15)

    # random measurements
    sta = stages[s-1]
    dur = durats[s-1]
    dur = dur + random.randrange(-1*(dur/2),dur/2)
    hea = random.randrange(hrtavg[s-1]-20, hrtavg[s-1]+20) * heartrate / 100
    hrt = "{:.1f}".format(hea).replace('.', ',')
    lod = ((w1 + w2) / 2.0) - random.uniform(0.5, 2.5)
    if s==1:lod = lod * 0.5
    if s==3:lod = lod * 0.1
    avl = "{:.1f}".format(lod).replace('.', ',')
    # glitch out
    if glitch == 1: dur = "NaN"
    if glitch == 2: hrt = "NaN"
    if glitch == 3: avl = "NaN"

    # write next line
    w.writerow((dat, pro, '', pid, dob, sex, sta, dur, hrt, avl))
    s = s + 1

g.close()

# display time to finish
run_tf = datetime.datetime.now() # finish
difference = run_tf - run_t0
print("%s Ready, duration time = %s" % (run_tf, str(difference)))
