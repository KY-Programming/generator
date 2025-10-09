@title KY-Generator Test Builder

cd Tests
start build
cd ..

cd Examples
start build
cd ..

nircmd cmdwait 100 win center ititle "KY-Generator Test Builder"
nircmd win center ititle "KY-Generator Examples"
nircmd win move ititle "KY-Generator Examples" -500 -260 0 0
nircmd win center ititle "KY-Generator Examples Angular"
nircmd win move ititle "KY-Generator Examples Angular" 500 -260 0 0
nircmd win center ititle "KY-Generator Examples Reflection"
nircmd win move ititle "KY-Generator Examples Reflection" -500 260 0 0
nircmd win center ititle "KY-Generator Tests"
nircmd win move ititle "KY-Generator Tests" 500 260 0 0

nircmd win hide ititle "KY-Generator Test Builder"
nircmd win show ititle "KY-Generator Test Builder"

@echo Press ANY key to close all windows...

pause

nircmd win close ititle "KY-Generator Examples"
nircmd win close ititle "KY-Generator Examples Angular"
nircmd win close ititle "KY-Generator Examples Reflection"
nircmd win close ititle "KY-Generator Tests"