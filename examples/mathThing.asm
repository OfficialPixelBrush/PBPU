; Roughly same Program as featured about halfway through of https://youtu.be/zltgXvg6r3k
; Could've made it more similar by using the same RAM locations, but that would've been painful
; Not to mention confusing
; So this simplifed version will have to do
; --------------------------------------------------

WTY 1 ; (CPU specific prep)
WTZ 1 ;
WTX 1 ; LOAD_B
RTY   ; LOAD_A from RAM
ADD   ; ADD B A
ZTR   ; STORE_A to RAM
PC1 3 ; \
WTZ 0 ;  }-JUMP
JMP   ; /
