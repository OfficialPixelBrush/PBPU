import os
import re
import sys

try:
    t = open("mathThing.asm","r")#sys.argv[1], "r")
    text = t.read()
    text = text.upper()
    text = re.sub(';.*?\n', '\n', text)
    text = text.replace(","," ")
    text = text.split()
except:
    print("No file given")
    exit()

print(text)
final=[]
labels=[]

for i,e in enumerate(text):
    # Labels
    if e[-1] == ':':
        labels.append(int(i/2)))
        # todo: make these useful by having a JMP macro
    
    # Instructions
    if (e=="NOP"):
        final.append(0*16)
    if (e=="ADD"):
        final.append(1*16)
    if (e=="SUB"):
        final.append(2*16)
    if (e=="WT1"):
        num = int(text[i+1])
        final.append(3*16+num)
    if (e=="WT2"):
        num = int(text[i+1])
        final.append(4*16+num)
    if (e=="WTX"):
        num = int(text[i+1])
        final.append(5*16+num)
    if (e=="WTY"):
        num = int(text[i+1])
        final.append(6*16+num)
    if (e=="WTZ"):
        num = int(text[i+1])
        final.append(7*16+num)
    if (e=="ZTR"):
        final.append(8*16)
    if (e=="RTZ"):
        final.append(9*16)
    if (e=="PC1"):
        num = int(text[i+1])
        final.append(10*16+num)
    if (e=="PC2"):
        num = int(text[i+1])
        final.append(11*16+num)
    if (e=="JMP"):
        final.append(12*16)
    if (e=="RTX"):
        final.append(13*16)
    if (e=="RTY"):
        final.append(14*16)
    if (e=="USC"):
        final.append(15*16)

output = open(t.name + ".bin" ,'wb')
t.close()

for i in final:
    i = i.to_bytes(1, 'little')
    output.write(i)
output.close()

