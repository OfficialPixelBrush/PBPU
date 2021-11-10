; -----------------------------------
; Smiley drawing Program for the PBPU
; -----------------------------------

NOP 	; No Operation

; Make sure we're at the start of RAM
; It's where the screen memory starts
; It goes from 0x00 up to 0x03
; The 4-bits that're in these RAM Locations will be represented as 4 1-bit pixels

; Move to the 1st line of the screen
WT1 0 	; Set high  Byte of RAM to 0
WT2 0 	; Set low Byte of RAM to 0
; Write Binary 1001 to Z Register
WTZ 9 	; Write 9 to Z Register	
; Write that to RAM so it'll be displayed on the Screen
ZTR		; Write Z to RAM

; Move to the 3rd line of the screen 
WT2 2	; Set low Byte of RAM to 2
; Write Binary 1001 to Z Register
WTZ 9	; Write 9 to Z
ZTR		; Write Z to RAM

; Move to the 4th line of the screen 
WT2 3	; Set low Byte of RAM to 3
; Write Binary 0110 to Z Register
WTZ 6	; Write 6 to Z
ZTR		; Write Z to RAM

; Congratulations, you've put a Smiley onto a 4x4 Pixel display!